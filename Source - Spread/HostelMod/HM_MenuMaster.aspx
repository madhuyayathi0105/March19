<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_MenuMaster.aspx.cs" Inherits="HM_MenuMaster" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
        <title></title>
        <style type="text/css">
            .watermark
            {
                color: #999999;
            }
        </style>
        <script type="text/javascript">
            function Test() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";

                id = document.getElementById("<%=txt_menuid1.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_menuid1.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_menuname1.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_menuname1.ClientID %>");
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
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }

            function change1(id) {
                var value1 = id.value;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_group.ClientID %>");
                    idval.style.display = "block";
                }
                else {
                    var idval = document.getElementById("<%=txt_group.ClientID %>");
                    idval.style.display = "none";
                }

            }
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function get(txt1) {
                $.ajax({
                    type: "POST",
                    url: "HM_MenuMaster.aspx/CheckUserName",
                    data: '{MenuName: "' + txt1 + '"}',
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
                        mesg.innerHTML = "Menu Name not exist";
                        break;
                    case "1":
                        mesg.style.color = "green";
                        document.getElementById('<%=txt_menuname1.ClientID %>').value = "";
                        mesg.innerHTML = "Menu Name available";
                        break;
                    case "2":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Please enter Menu Name";
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
        <center>
            <br />
            <div>
                <asp:Label ID="lblmenumaster" Style="color: Green;" CssClass="fontstyleheader" Text="Menu Master"
                    runat="server"></asp:Label>
                <br />
                <br />
            </div>
            <div class="maindivstyle" style="width: 1000px; height: 520px;">
                <br />
                <center>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_menutype" runat="server" Text="Menu Type"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_menutype" runat="server" onfocus="return myFunction(this)" CssClass="textbox textbox1"
                                            ReadOnly="true" Width="120px" Height="18px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pmenutype" runat="server" CssClass="multxtpanel" Style="height: 153px;
                                            width: 126px;">
                                            <asp:CheckBox ID="cb_menutype" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_menutype_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_menutype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_menutype_SelectedIndexChanged">
                                                <%--<asp:ListItem Value="0">Veg</asp:ListItem>
                                                <asp:ListItem Value="1">Non-Veg</asp:ListItem>--%>
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pextender1" runat="server" TargetControlID="txt_menutype"
                                            PopupControlID="pmenutype" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_menuname" runat="server" Text="Menu Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_menuname" runat="server" onfocus="return myFunction(this)" CssClass="textbox textbox1"
                                            ReadOnly="true" Width="120px" Height="18px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pmenuname" runat="server" Style="height: 200px; width: 160px;" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_menuname" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_menuname_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_menuname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_menuname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pextender" runat="server" TargetControlID="txt_menuname"
                                            PopupControlID="pmenuname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <%--<td>
                            <asp:Label ID="lbl_searchby" runat="server" Text="Search By"></asp:Label>
                        </td>--%>
                            <td>
                                <asp:DropDownList ID="ddl_type" runat="server" CssClass="textbox  ddlheight3" OnSelectedIndexChanged="ddl_type_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="0">Menu Name</asp:ListItem>
                                    <asp:ListItem Value="1">Menu Id</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_search" Visible="false" runat="server" placeholder="Search Menu Name"
                                    CssClass="textbox  txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                                <asp:TextBox ID="txt_menuid" Visible="false" runat="server" placeholder="Search Menu Id"
                                    CssClass="textbox  txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getmenu" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_menuid"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_addnew" Text="Add New" runat="server" CssClass="textbox btn2"
                                    OnClick="btn_addnew_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <br />
                <center>
                    <%--<div style="float: left; width: 298px;">--%>
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                        Font-Size="Medium"></asp:Label>
                    <%--</div>--%></center>
                <div id="div1" runat="server" visible="false" style="width: 692px; height: 350px;
                    background-color: White;" class="reportdivstyle spreadborder">
                    <br />
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Width="650px" Height="350px" ShowHeaderSelection="false" OnCellClick="Cell_Click"
                        OnPreRender="Fpspread1_render">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <div id="printdiv" runat="server" visible="false" style="height: 100%; z-index: -1;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                </div>
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                        Width="180px" onkeypress="display()"></asp:TextBox>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                        Text="Export To Excel" Width="127px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        CssClass="textbox btn2" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 70px; margin-left: 260px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <br />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; height: 300px; width: 550px;">
                    <br />
                    <div>
                        <center>
                            <asp:Label ID="lbl_popmenumaster" runat="server" Style="font-size: large; font-weight: bold;
                                color: Green;" Text="Menu Master"></asp:Label>
                        </center>
                    </div>
                    <br />
                    <table style="line-height: 35px;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_menuid1" runat="server" Text="Menu ID"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_menuid1" runat="server" Width="100px" CssClass="textbox textbox1"
                                    Style="text-transform: uppercase;" onfocus="return myFunction(this)" Enabled="false"></asp:TextBox>
                                <span style="color: Red;">*</span>
                                <asp:FilteredTextBoxExtender ID="filtertextbox" runat="server" TargetControlID="txt_menuid1"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                <asp:Label ID="lbl_group1" runat="server" Text="Group"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_group1" runat="server" Style="float: left;" onchange="return change1(this)"
                                    CssClass="textbox textbox1 ddlheight4">
                                </asp:DropDownList>
                                <asp:TextBox ID="txt_group" runat="server" Style="display: none; float: left;" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <span style="color: Red; float: left;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_menuname1" runat="server" Text="Menu Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_menuname1" Width="200px" CssClass="textbox textbox1" runat="server"
                                    onfocus="return myFunction(this)" onblur="return get(this.value)"></asp:TextBox>
                                <span style="color: Red;">*</span><span style="font-weight: bold; font-size: larger;"
                                    id="msg1"></span>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_menuname1"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=", &">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Menutype1" runat="server" Text="Menu Type"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                <asp:DropDownList ID="ddlStudType" runat="server" CssClass="textbox  ddlheight3"
                                    onfocus="return myFunction(this)">
                                </asp:DropDownList>
                                <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                                <%--  <asp:RadioButton ID="rdb_veg1" runat="server" Text="Veg" GroupName="veg" />
                                <asp:RadioButton ID="rdb_nonveg1" runat="server" Text="Non Veg" GroupName="veg" />--%>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                <asp:CheckBox ID="cb_printtoken1" runat="server" Text="Print Token" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <center>
                        <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="textbox btn2"
                            OnClientClick="return valid()" OnClick="btn_update_Click" Visible="false" />
                        <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                            OnClientClick="return valid()" OnClick="btn_delete_Click" Visible="false" />
                        <asp:Button ID="btn_save" runat="server" Text="Save" OnClientClick="return Test()"
                            OnClick="btn_save_Click" CssClass="textbox btn2" Visible="false" />
                        <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit_Click" />
                    </center>
                </div>
            </div>
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
                                    <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Text="Student Type" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txtStudentType" runat="server" Width="200px" CssClass="textbox textbox1"
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
        <div id="surediv_del" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                    <asp:Label ID="lbl_suredel" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_suredel" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btn_suredel_Click" Text="Yes" runat="server" />
                                        <asp:Button ID="btn_delno" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btn_delno_Click" Text="No" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
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
        </form>
    </body>
    </html>
</asp:Content>
