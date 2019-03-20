<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master" AutoEventWireup="true"
    CodeFile="Hm_StoreMasterNew.aspx.cs" Inherits="Hm_StoreMasterNew" %>

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
        <%--<link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />--%>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
        <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
        <style type="text/css">
            .maindivstylesize
            {
                height: 500px;
                width: 1000px;
            }
            .popupheight3
            {
                height: 43em;
            }
            .stNew
            {
                text-transform: uppercase;
            }
        </style>
        <script type="text/javascript">
            function check() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";
                id = document.getElementById("<%=txt_storename.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_storename.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_storeacr.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_storeacr.ClientID %>");
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
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

            function get(txt1) {
                $.ajax({
                    type: "POST",
                    url: "Hm_StoreMasterNew.aspx/CheckUserName",
                    data: '{StoreName: "' + txt1 + '"}',
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
                        mesg.innerHTML = "Store Name Not Exist";
                        break;
                    case "1":
                        mesg.style.color = "green";
                        document.getElementById('<%=txt_storename.ClientID %>').value = "";
                        mesg.innerHTML = "Store Name Available";
                        break;
                    case "2":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Please Enter Store Name";
                        break;
                    case "error":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Error Occurred";
                        break;
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
                <asp:Label ID="header" runat="server" Style="color: Green;" CssClass="fontstyleheader"
                    Text="Store Master"></asp:Label>
                <br />
                <br />
            </div>
        </center>
        <center>
            <div class="maindivstyle maindivstylesize">
                <br />
                <div>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <span>Store Name</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_storemaster" runat="server" CssClass="textbox1 ddlheight4">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_search" Text="Search By" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_search" runat="server" placeholder="Store Name" CssClass="textbox textbox1"
                                    Height="20px"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                                    CompletionListCssClass="autocomplete_completionListElement " CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_add" runat="server" OnClick="btn_addnew_Click" CssClass="textbox btn2"
                                    Text="Add New" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div style="text-align: left; text-indent: 50px; font-size: medium;">
                    <asp:Label ID="errorlable" runat="server" ForeColor="Red" Font-Size="Medium" Visible="false"></asp:Label>
                </div>
                <div id="div1" runat="server" visible="false" style="width: 700px; height: 350px;"
                    class="reportdivstyle table">
                    <br />
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" Width="650px" Height="300px" ShowHeaderSelection="false"
                        OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
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
                    <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1 txtheight2 "
                        Width="180px" onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                        Width="127px" Height="30px" CssClass="textbox" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Width="60px" Height="30px" CssClass="textbox" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </div>
            <div id="poperrjs" runat="server" visible="false" class="popupstyle popupheight3">
                <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 50px; margin-left: 339px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <br />
                <center>
                    <div class="subdivstyle" style="background-color: White; width: 700px; height: 456px;">
                        <br />
                        <center>
                            <asp:Label ID="Label1" runat="server" Style="color: Green;" CssClass="fontstyleheader"
                                Text="Store Master Entry"></asp:Label>
                        </center>
                        <br />
                        <span style="font-weight: bold; font-size: larger;" id="Span1"></span>
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Addstore" runat="server" Text="Store Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_storename" CssClass="textbox textbox1 txtheight5" Width="200px"
                                            runat="server" onfocus="return myFunction(this)" onkeyup="return get(this.value)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_storename"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red;">*</span> <span style="font-weight: bold; font-size: larger;"
                                            id="msg1"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_storeacr" runat="server" Text="Store Acronym"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_storeacr" CssClass="textbox textbox1 txtheight stNew" Width="75px"
                                            onfocus="return myFunction(this)" runat="server" MaxLength="15"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_storeacr"
                                            FilterType="UppercaseLetters,Custom,LowercaseLetters" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_startyear" runat="server" Text="Start Year"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="oldyeartxt" Visible="false" Text="1900" CssClass="textbox textbox1 txtheight"
                                            Width="75px" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txt_startyear" runat="server" CssClass="textbox textbox1 txtheight"
                                            MaxLength="4" AutoPostBack="true" OnTextChanged="txtyear_Onchange"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_startyear"
                                            FilterType="Numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                        <%-- <asp:DropDownList ID="ddl_startyear" CssClass="textbox ddlheight2" runat="server">
                                    </asp:DropDownList>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Department" runat="server" Text="Department"></asp:Label>
                                    </td>
                                    <td>
                                        <fieldset style="height: 200px; width: 500px">
                                            <legend>Department
                                                <asp:Button ID="btn_adddeptment" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_adddeptment_Click" />
                                            </legend>
                                            <asp:Panel ID="Panelbind" runat="server" ScrollBars="Auto" Style="height: 151px;">
                                                <asp:GridView ID="SelectdptGrid" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                                    HeaderStyle-ForeColor="White">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DeptCode">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldeptcode" runat="server" Text='<%# Eval("DeptCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DeptName">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldeptname" runat="server" Text='<%# Eval("DeptName") %>'></asp:Label>
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
                                    <td colspan="2">
                                        <center>
                                            <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="textbox btn2"
                                                OnClientClick="return check()" OnClick="btn_update_Click" Visible="false" />
                                            <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                                                OnClientClick="return check()" OnClick="btn_delete_Click" Visible="false" />
                                            <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="textbox btn2" OnClientClick="return check()"
                                                OnClick="btn_save_Click" Visible="false" />
                                            <asp:Button ID="btnexit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit_Click" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
            <div id="Newdiv" runat="server" visible="false" class="popupstyle popupheight3">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 31px; margin-left: 341px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <br />
                <center>
                    <div class="subdivstyle" style="height: 550px; width: 706px; background-color: White;">
                        <br />
                        <br />
                        <center>
                            <asp:Label ID="Label2" runat="server" Font-Bold="true" Style="font-size: large; color: Green;"
                                Text="Department Name"></asp:Label>
                        </center>
                        <br />
                        <div>
                            <asp:RadioButton ID="rdb_academic" runat="server" AutoPostBack="true" OnCheckedChanged="rdb_academic_CheckedChanged"
                                Text="Academic" GroupName="Dept" />
                            <asp:RadioButton ID="rdb_nonacademic" runat="server" AutoPostBack="true" OnCheckedChanged="rdb_nonacademic_CheckedChanged"
                                Text="Non-Academic" GroupName="Dept" />
                        </div>
                        <br />
                        <div class="reportdivstyle table" style="width: 515px; height: 300px;">
                            <asp:GridView ID="dptgrid" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                HeaderStyle-ForeColor="White">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbcheck" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DeptCode">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldeptcode" runat="server" Text='<%# Eval("DeptCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DeptName">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldeptname" runat="server" Text='<%# Eval("DeptName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="300px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <asp:CheckBox ID="cb_selectall" runat="server" Text="Select All" AutoPostBack="true"
                            OnCheckedChanged="cb_selectAll_ChekedChange" Style="margin-left: -156px; position: absolute;" />
                        <asp:Button ID="btn_deptpartmentsave" runat="server" Font-Bold="true" Text="Save"
                            CssClass="textbox btn2" OnClick="btn_deptpartmentsave_Click" />
                        <asp:Button ID="btn_deptexit" runat="server" Text="Exit" Font-Bold="true" CssClass="textbox btn2"
                            OnClick="btn_deptexit_Click" />
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
