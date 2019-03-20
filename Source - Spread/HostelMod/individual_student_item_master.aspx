<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="individual_student_item_master.aspx.cs" Inherits="individual_student_item_master" %>

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
        <style type="text/css">
            .div
            {
                left: 0%;
                top: 0%;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">
            function checkDecimal(el) {
                var ex = /^((\d{9})*|([1-9]\d{0,9}))(\.\d{0,2})?$/;
                if (ex.test(el.value) == false) {
                    el.value = '';
                    //alert('Enter Before Decimal six numbers & After Decimal two numbers');

                }
            }
            function Test() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";
                id = document.getElementById("<%=txt_cost.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_cost.ClientID %>");
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
                id = document.getElementById("<%=ddl_ledger.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    id = document.getElementById("<%=ddl_ledger.ClientID %>");
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
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <asp:Label ID="Label1" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="Individual Student Item Master"></asp:Label>
                    </div>
                </center>
                <br />
                <div class="maindivstyle" style="height: 500px; width: 1000px;">
                    <br />
                    <table class="maintablestyle" style="width: 740px;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_itemname" runat="server" Text="Item Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_itemname" runat="server" CssClass="textbox  txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                            width: 160px;">
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
                            <td>
                                <asp:Label ID="lbl_searchby" runat="server" Text="Search By"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_itemname" runat="server" CssClass="textbox  ddlheight3"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="0">Item Name</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_searchby" Visible="false" runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox btn2"
                                    OnClick="btn_addnew_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    <div id="div1" runat="server" visible="false" style="width: 767px; height: 300px;
                        overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                        box-shadow: 0px 0px 8px #999999;">
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Width="750px" Style="height: 350px; overflow: auto; background-color: White;"
                            OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <br />
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1" onkeypress="display()"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1"
                            Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Width="60px" CssClass="textbox textbox1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="poperrjs" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 23px; margin-left: 341px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 700px;
                    height: 400px;" align="center">
                    <br />
                    <center>
                        <asp:Label ID="lbl_studentitemmaster" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="Student Item Master"></asp:Label>
                    </center>
                    <br />
                    <div align="left" style="overflow: auto; width: 500px; height: 250px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <center>
                            <table>
                                <br />
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_itemname1" runat="server" Text="Item Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Button ID="btn_plus_itemname" runat="server" Text="+" CssClass="textbox btn"
                                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_plus_itemname_Click" />
                                        <asp:DropDownList ID="ddl_itemname1" CssClass="textbox ddlheight3" runat="server"
                                            AutoPostBack="true" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_minus_itemname" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_minus_itemname_Click" />
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_modeofpayment" runat="server" Text="Mode Of Payment"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:RadioButton ID="rdb_monthly" Text="Monthly" AutoPostBack="true" runat="server"
                                            GroupName="same" />
                                        <asp:RadioButton ID="rdb_yearly" Text="Yearly" AutoPostBack="true" runat="server"
                                            GroupName="same" />
                                        <asp:RadioButton ID="rdb_sem" Text="Semester" AutoPostBack="true" runat="server"
                                            GroupName="same" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cost" runat="server" Text="Cost"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_cost" onfocus="return myFunction(this)" CssClass="textbox  txtheight2"
                                            runat="server" onblur="checkDecimal(this);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_cost"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_ledger" runat="server" Text="Ledger"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_ledger" CssClass="textbox ddlheight3" runat="server" AutoPostBack="true"
                                            onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_desc" runat="server" Text="Description"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Button ID="btn_plus_desc" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_plus_desc_Click" />
                                        <asp:DropDownList ID="ddl_desc" CssClass="textbox ddlheight3" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_minus_desc" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_minus_desc_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                        <center>
                                            <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="textbox btn2"
                                                Visible="false" OnClick="btn_update_Click" OnClientClick="return Test()" />
                                            <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                                                Visible="false" OnClick="btn_delete_Click" OnClientClick="return Test()" />
                                            <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" Text="Save" OnClientClick="return Test()"
                                                OnClick="btn_save_Click" />
                                            <asp:Button ID="btn_exit" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit_Click" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="panel_reason" runat="server" visible="false" class="table" style="background-color: White;
                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_add" runat="server" Text="Item Name" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txt_itemname2" runat="server" Width="200px" Style="font-family: 'Book Antiqua';
                                        margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </td>
                            </tr>
                            <br />
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_add_itemname" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_add_itemname_Click" />
                                    <asp:Button ID="btn_exit_itemname" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_exit_itemname_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="imgdiv4" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="panel_desc" runat="server" visible="false" class="table" style="background-color: White;
                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_desc1" runat="server" Text="Description" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txt_desc" Width="200px" Style="margin-left: 13px" CssClass="textbox textbox1 txtheight3"
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <br />
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_add_desc" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_add_desc_Click" />
                                    <asp:Button ID="btn_exit_desc" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_exit_desc_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
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
                                            <asp:Button ID="btn_errorclose" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                OnClick="btn_errorclose_Click" Text="ok" runat="server" />
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
