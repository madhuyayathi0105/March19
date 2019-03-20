<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_MessMaster.aspx.cs" Inherits="HM_MessMaster" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title>Mess Master</title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
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
        </style>
        <script type="text/javascript">
            function check() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";
                id = document.getElementById("<%=txt_messname.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_messname.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_messacr.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_messacr.ClientID %>");
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
                    url: "HM_MessMaster.aspx/CheckUserName",
                    data: '{MessName: "' + txt1 + '"}',
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
                        mesg.innerHTML = "Mess Name Does Not Exist";
                        break;
                    case "1":
                        mesg.style.color = "green";
                        document.getElementById('<%=txt_messname.ClientID %>').value = "";
                        mesg.innerHTML = "Mess Name available";
                        break;
                    case "2":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Please Enter Mess Name";
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
                <span class="fontstyleheader" style="color: Green;">Mess Master</span>
            </div>
        </center>
         <br />
        <center>
            <div class="maindivstyle" style="width: 1000px; height: 600px">
                <br />
                <div>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <span>Mess Name</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_messmaster" runat="server" CssClass="textbox1 ddlheight4"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_messmaster_OnSelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_search" Text="Search By" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_search" runat="server" placeholder="Mess Name" CssClass="textbox textbox1 txtheight2 "
                                    AutoPostBack="true" OnTextChanged="txt_search_OnTextChanged"></asp:TextBox>
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
                <div id="div1" runat="server" visible="false" style="width: 767px; height: 350px;
                    box-shadow: 0px 0px 8px #999999;" class="reportdivstyle">
                    <br />
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="0px" Width="750px" Height="300px" OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <br />
                    <asp:Label ID="lblrptname" Text="Report Name" runat="server"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1" Height="20px"
                        Width="180px" onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                        Width="127px" CssClass="textbox" Height="30px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Width="60px" Height="30px"  CssClass="textbox" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
                <br />
            </div>
            <div id="poperrjs" runat="server" visible="false" class="popupstyle popupheight1">
                <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 150px; margin-left: 280px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <br />
                <center>
                    <div class="subdivstyle" style="background-color: White; width: 600px; height: 300px;
                        margin-top: 100px;">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: Green;">Mess Master Entry</span>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Addmess" runat="server" Text="Mess Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_messname" CssClass="textbox textbox1 txtheight5" Width="200px"
                                            runat="server" onfocus="return myFunction(this)" onblur="return get(this.value)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_messname"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=" .-&" >
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red;">*</span> <span id="msg1"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_messacr" runat="server" Text="Mess Acronym"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_messacr" CssClass="textbox textbox1 txtheight" Width="75px"
                                            onfocus="return myFunction(this)" runat="server" Style="text-transform: uppercase;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_messacr"
                                            FilterType="UppercaseLetters,lowercaseLetters,Custom" ValidChars="">
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
                                            MaxLength="4" AutoPostBack="false" OnTextChanged="txtyear_Onchange"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_startyear"
                                            FilterType="Numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <%--<tr>
                                <td>
                                    <asp:Label ID="lbl_hostel" runat="server" Text="Hostel" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPHostelName" runat="server" Visible="false">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_hostelname" runat="server" CssClass="multxtpanel  multxtpanleheight">
                                                <asp:CheckBox ID="cb_hostel" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_hostel_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_hostel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostel_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popHostelName" runat="server" TargetControlID="txt_hostelname"
                                                PopupControlID="panel_hostelname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>--%>
                                <tr>
                                </tr>
                                <br />
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
                <br />
                <span style="font-weight: bold; font-size: larger;" id="Span1"></span>
                <br />
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
