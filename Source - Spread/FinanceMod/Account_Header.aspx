<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Account_Header.aspx.cs" Inherits="Account_Header" %>

<%@ Register Src="~/UserControls/PrintMaster.ascx" TagName="printmaster" TagPrefix="InsproPlus" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title>Header Master</title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .stNew
        {
            text-transform: uppercase;
        }
    </style>
    <body>
        <script type="text/javascript">

            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function valid2() {
                var idval = "";
                var empty = "";
                var id = "";
                var value1 = "";
                idval = document.getElementById("<%=txthdracr.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txthdracr.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txtacchdr.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txtacchdr.ClientID %>");
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

            function getaccheader(txt) {
                $.ajax({
                    type: "POST",
                    url: "Account_Header.aspx/CheckAccHeader",
                    data: '{AccHeader:"' + txt + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccess,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }

            function OnSuccess(response) {
                var errmsg = $("#screrr")[0];
                switch (response.d) {
                    case "0":
                        errmsg.style.color = "green";
                        errmsg.innerHTML = "Does Not Exist!";
                        break;
                    case "1":
                        errmsg.style.color = "green";
                        errmsg.innerHTML = "Already Exist!";
                        document.getElementById('<%=txtacchdr.ClientID %>').value = "";
                        break;
                    case "2":
                        errmsg.style.color = "red";
                        errmsg.innerHTML = "Please enter Account Header";
                        break;
                    case "error":
                        errmsg.style.color = "red";
                        errmsg.innerHTML = "Error occurred";
                        break;
                }
            }

            function getaccacr(txt) {
                $.ajax({
                    type: "POST",
                    url: "Account_Header.aspx/CheckAccAcr",
                    data: '{AccAcr:"' + txt + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: Success,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }

            function Success(response) {
                var errmsg = $("#screrracr")[0];
                switch (response.d) {
                    case "0":
                        errmsg.style.color = "green";
                        errmsg.innerHTML = "Does Not Exist!";
                        break;
                    case "1":
                        errmsg.style.color = "green";
                        errmsg.innerHTML = "Already Exist!";
                        document.getElementById('<%=txthdracr.ClientID %>').value = "";
                        break;
                    case "2":
                        errmsg.style.color = "red";
                        errmsg.innerHTML = "Please enter Account Acr";
                        break;
                    case "error":
                        errmsg.style.color = "red";
                        errmsg.innerHTML = "Error occurred";
                        break;
                }
            }

        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Header Master</span></div>
            <center>
                <div class="maindivstyle" style="height: auto; width: 1000px;">
                    <center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lblcol" runat="server" Text="College Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlclgname" runat="server" CssClass="textbox textbox1 ddlheight4"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlclgname_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblacct" runat="server" Text="Header Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_acctname" runat="server" CssClass="textbox textbox1 txtheight4"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pacctname" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Width="200px">
                                                <asp:CheckBox ID="cbacctname" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cbacctname_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblacctname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblacctname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pextender" runat="server" TargetControlID="txt_acctname"
                                                PopupControlID="pacctname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblsearch" runat="server" Text="Search By"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsearch" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getheader" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbhdpriority" runat="server" Text="Priority" AutoPostBack="true"
                                        OnCheckedChanged="cbhdpriority_Changed" />
                                </td>
                                <td>
                                    <asp:Button ID="bttngo" runat="server" CssClass="textbox textbox1 btn" Text="Go"
                                        OnClick="btngo_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="bttnadd" runat="server" CssClass="textbox textbox1 btn2" Text="Add New"
                                        OnClick="btnNew_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <asp:Label ID="lblerror" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                        Font-Size="Medium"></asp:Label>
                    <br />
                    <div id="div1" runat="server" visible="false" style="width: 850px;">
                        <FarPoint:FpSpread ID="Fpspreadnew" runat="server" Visible="false" Style="overflow: auto;
                            background-color: White;" Width="800px" OnCellClick="Cellcont_Click" OnPreRender="Fpspreadnew_render">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <FarPoint:FpSpread ID="spreadPriority" runat="server" Visible="false" Style="overflow: auto;
                            background-color: White;" Width="800px" OnCellClick="Cell_Click" OnPreRender="spreadPriority_render"
                            OnButtonCommand="spreadPriority_ButtonCommand">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <div id="divpriority" runat="server" visible="false">
                        <asp:Button ID="btnSetPriority" runat="server" Text="Set Priority" CssClass=" textbox textbox1 btn2"
                            Width="100px" OnClick="btnSetPriority_Click" />
                        <asp:Button ID="btnResetPriority" runat="server" Text="Reset" CssClass=" textbox textbox1 btn2"
                            OnClick="btnResetPriority_Click" />
                    </div>
                    <div id="rportprint" runat="server" visible="true">
                        <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <br />
                        <asp:Label ID="lblrporttname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" CssClass="textbox textbox1 txtheight3"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                            Width="130px" Text="Export To Excel" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            CssClass="textbox textbox1 btn2" />
                        <InsproPlus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                    <br />
                </div>
            </center>
            <center>
                <div id="poppernew" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 9px; margin-left: 430px;" OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <center>
                        <div style="background-color: White; height: 480px; width: 900px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <div>
                                <center>
                                    <span class="fontstyleheader" style="color: Green;">Create a Header</span>
                                </center>
                            </div>
                            <br />
                            <div style="float: left; width: 900px; height: 380px;">
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMainacc" runat="server" Text="College Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlmainacc" runat="server" Style="width: 300px;" onfocus="return myFunction(this)"
                                                    CssClass="textbox textbox1 ddlheight4" AutoPostBack="true" OnSelectedIndexChanged="ddlmainacc_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span style="color: Red;" runat="server" id="lblmainerr" visible="false">Select Main
                                                    Account</span>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblhdracr" runat="server" Text="Header Acronym"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txthdracr" CssClass="textbox textbox1 txtheight stNew" MaxLength="6"
                                                    runat="server" onfocus="return myFunction(this)" onblur="return getaccacr(this.value)"></asp:TextBox>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td>
                                                <span style="font-weight: bold; font-size: medium;" id="screrracr"></span>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblacchdr" runat="server" Text="Account Header Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtacchdr" CssClass="textbox textbox1 txtheight6" Style="width: 290px;"
                                                    onfocus="return myFunction(this)" onblur="return getaccheader(this.value)" runat="server"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="filterextender" runat="server" TargetControlID="txtacchdr"
                                                    FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers" ValidChars=" ,-">
                                                </asp:FilteredTextBoxExtender>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbincmis" runat="server" Text="Miscellaneous" />
                                            </td>
                                            <td>
                                                <span style="font-weight: bold; font-size: medium;" id="screrr"></span>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_openbal" runat="server" Visible="true" Text="Opening Balance"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_openbal" CssClass="textbox textbox1 txtheight2" Visible="true"
                                                    runat="server"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_openbal"
                                                    FilterType="Numbers,Custom" ValidChars=". ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpay1" runat="server" Text="Payment Incharge Designation1"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtpay1" CssClass="textbox textbox1 txtheight5" ReadOnly="true"
                                                    Width="255px" onfocus="return myFunction(this)" runat="server"></asp:TextBox>
                                                <asp:Button ID="btnpoppay1" runat="server" CssClass="textbox textbox1 btn1" Text="?"
                                                    OnClick="btnpoppay1_Click" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpay2" runat="server" Text="Payment Incharge Designation2"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtpay2" CssClass="textbox textbox1 txtheight5" ReadOnly="true"
                                                    Width="255px" onfocus="return myFunction(this)" runat="server"></asp:TextBox>
                                                <asp:Button ID="btnpoppay2" runat="server" CssClass="textbox textbox1 btn1" Text="?"
                                                    OnClick="btnpoppay2_Click" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpay3" runat="server" Text="Payment Incharge Designation3"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtpay3" CssClass="textbox textbox1 txtheight5" ReadOnly="true"
                                                    Width="255px" onfocus="return myFunction(this)" runat="server"></asp:TextBox>
                                                <asp:Button ID="btnpoppay3" runat="server" CssClass="textbox textbox1 btn1" Text="?"
                                                    OnClick="btnpoppay3_Click" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblrec" runat="server" Text="Receipt Incharge"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtrec" CssClass="textbox textbox1 txtheight5" ReadOnly="true" Width="255px"
                                                    onfocus="return myFunction(this)" runat="server"></asp:TextBox>
                                                <asp:Button ID="btnrec" runat="server" CssClass="textbox textbox1 btn1" Text="?"
                                                    OnClick="btnrec_Click" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpur" runat="server" Text="Purpose of Header"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtpur" TextMode="MultiLine" CssClass="textbox textbox1" onfocus="return myFunction(this)"
                                                    Width="290px" Height="75px" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </center>
                                <asp:Button ID="btnupdate" runat="server" Text="Update" CssClass="textbox textbox1 btn2"
                                    OnClick="btnupdate_Click" OnClientClick="return valid2()" Visible="false" />
                                <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="textbox textbox1 btn2"
                                    OnClick="btndelete_Click" OnClientClick="return valid2()" Visible="false" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" Visible="false"
                                    OnClientClick="return valid2()" CssClass="textbox textbox1 btn2" />
                                <asp:Button ID="btnexit" runat="server" Text="Exit" CssClass="textbox textbox1 btn2"
                                    OnClick="btnexit_Click" />
                            </div>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="popper1" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 9px; margin-left: 420px;" OnClick="imagebtnpopclose2_Click" />
                    <center>
                        <br />
                        <div style="background-color: White; height: 470px; width: 900px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <div>
                                <center>
                                    <span class="fontstyleheader" style="color: Green;">Account Header</span>
                                </center>
                                <br />
                            </div>
                            <center>
                                <div>
                                    <table class="maintablestyle">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblsearch1" runat="server" Text="Search By"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlsearch1" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlsearch1_OnSelectedIndexChanged">
                                                    <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtsearch1" runat="server" Visible="false" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                                <asp:TextBox ID="txtsearch1c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetdesCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1c"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btngo1" runat="server" CssClass="textbox textbox1 btn1" Text="Go"
                                                    OnClick="btngo1_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:Label ID="lblerr1" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                    <div id="div2" runat="server" visible="true" style="width: 599px; height: 300px;
                                        overflow: auto; border: 1px solid Gray; background-color: White; border-radius: 10px;">
                                        <FarPoint:FpSpread ID="Fpspreadpay1" runat="server" Visible="false" BorderColor="Gray"
                                            BorderStyle="Solid" BorderWidth="1px" Width="570px" Height="270px" OnCellClick="Cellpay1_Click"
                                            OnPreRender="Fpspreadpay1_render">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White" SelectionBackColor="LightGreen">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </div>
                                    <br />
                                    <div id="Div3" runat="server" visible="true">
                                        <asp:Button ID="btnok1" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                            OnClick="btnok1_Click" Font-Size="Medium" CssClass="textbox textbox1" Text="OK"
                                            Width="60px" Height="35px" />
                                        <asp:Button ID="btncancel1" runat="server" Text="Exit" OnClick="btncancel1_Click"
                                            CssClass="textbox textbox1 btn2" />
                                    </div>
                                </div>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="popper2" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 9px; margin-left: 420px;" OnClick="imagebtnpopclose3_Click" />
                    <br />
                    <center>
                        <div style="background-color: White; height: 470px; width: 900px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <div>
                                <center>
                                    <span class="fontstyleheader" style="color: Green;">Account Header</span>
                                </center>
                            </div>
                            <br />
                            <center>
                                <div>
                                    <center>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblsearch2" runat="server" Text="Search By"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsearch2" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlsearch2_OnSelectedIndexChanged">
                                                        <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtsearch2" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch2"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:TextBox ID="txtsearch2c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetdesCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch2c"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btngo2" runat="server" CssClass="textbox textbox1 btn1" Text="Go"
                                                        OnClick="btngo2_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                    <br />
                                    <asp:Label ID="lblerr2" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                    <div id="div5" runat="server" visible="true" style="width: 599px; height: 300px;
                                        overflow: auto; border: 1px solid Gray; background-color: White;">
                                        <FarPoint:FpSpread ID="Fpspreadpay2" runat="server" Visible="false" BorderColor="Gray"
                                            BorderStyle="Solid" BorderWidth="1px" Width="570px" Height="270px" OnCellClick="Cellpay2_Click"
                                            OnPreRender="Fpspreadpay2_render">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White" SelectionBackColor="LightGreen">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </div>
                                    <br />
                                    <div id="Div6" runat="server" visible="true">
                                        <asp:Button ID="btnok2" runat="server" OnClick="btnok2_Click" CssClass="textbox textbox1 btn2"
                                            Text="OK" Visible="false" />
                                        <asp:Button ID="btncancel2" runat="server" Text="Exit" OnClick="btncancel2_Click"
                                            CssClass="textbox textbox1 btn2" />
                                    </div>
                                </div>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="popper3" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 9px; margin-left: 420px;" OnClick="imagebtnpopclose4_Click" />
                    <br />
                    <center>
                        <div style="background-color: White; height: 470px; width: 900px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <div>
                                <center>
                                    <span class="fontstyleheader" style="color: Green;">Account Header</span>
                                </center>
                            </div>
                            <br />
                            <center>
                                <div>
                                    <center>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblsearch3" runat="server" Text="Search By"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsearch3" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlsearch3_OnSelectedIndexChanged">
                                                        <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtsearch3" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch3"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:TextBox ID="txtsearch3c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetdesCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch3c"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btngo3" runat="server" CssClass="textbox textbox1 btn2" Text="Go"
                                                        OnClick="btngo3_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                </div>
                            </center>
                            <br />
                            <asp:Label ID="lblerr3" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                                Font-Size="Medium"></asp:Label>
                            <div id="div7" runat="server" visible="true" style="width: 599px; height: 300px;
                                overflow: auto; border: 1px solid Gray; background-color: White;">
                                <FarPoint:FpSpread ID="Fpspreadpay3" runat="server" Visible="false" BorderColor="Gray"
                                    BorderStyle="Solid" BorderWidth="1px" Width="570px" Height="270px" OnCellClick="Cellpay3_Click"
                                    OnPreRender="Fpspreadpay3_render">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" BackColor="White" SelectionBackColor="LightGreen">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                            <br />
                            <div id="Div8" runat="server" visible="true">
                                <asp:Button ID="btnok3" runat="server" Visible="false" OnClick="btnok3_Click" CssClass="textbox textbox1 btn2"
                                    Text="OK" />
                                <asp:Button ID="btncancel3" runat="server" Text="Exit" OnClick="btncancel3_Click"
                                    CssClass="textbox textbox1 btn2" />
                            </div>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="popper4" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 9px; margin-left: 420px;" OnClick="imagebtnpopclose5_Click" />
                    <center>
                        <br />
                        <div style="background-color: White; height: 470px; width: 900px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <div>
                                <center>
                                    <span class="fontstyleheader" style="color: Green;">Account Header</span>
                                </center>
                            </div>
                            <br />
                            <center>
                                <div>
                                    <center>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblsearch4" runat="server" Text="Search By"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsearch4" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlsearch4_OnSelectedIndexChanged">
                                                        <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtsearch4" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch4"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:TextBox ID="txtsearch4c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetdesCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch4c"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btngo4" runat="server" CssClass="textbox textbox1 btn1" Text="Go"
                                                        OnClick="btngo4_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                    <br />
                                    <asp:Label ID="lblerr4" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                    <div id="div9" runat="server" visible="true" style="width: 599px; height: 300px;
                                        overflow: auto; border: 1px solid Gray; background-color: White;">
                                        <FarPoint:FpSpread ID="Fpspreadpay4" runat="server" Visible="false" BorderColor="Gray"
                                            BorderStyle="Solid" BorderWidth="1px" Width="570px" Height="270px" OnCellClick="Cellpay4_Click"
                                            OnPreRender="Fpspreadpay4_render">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White" SelectionBackColor="LightGreen">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </div>
                                    <br />
                                    <div id="Div10" runat="server" visible="true">
                                        <asp:Button ID="btnok4" runat="server" Visible="false" OnClick="btnok4_Click" CssClass="textbox textbox1 btn2"
                                            Text="OK" />
                                        <asp:Button ID="btncancel4" runat="server" Text="Exit" OnClick="btncancel4_Click"
                                            CssClass="textbox textbox1 btn2" />
                                    </div>
                                </div>
                            </center>
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
                            width: 200px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
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
                                                <asp:Button ID="btn_errorclose" CssClass="textbox btn1 textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_errorclose_Click" Text="OK" runat="server" />
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
                <div id="imgdiv1" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div4" runat="server" class="table" style="background-color: White; height: 150px;
                            width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 150px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblalert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnyes" CssClass="textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                                    OnClick="btnyes_Click" Text="Yes" runat="server" />
                                                <asp:Button ID="btnno" CssClass="textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                                    OnClick="btnno_Click" Text="No" runat="server" />
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
        </center>
    </body>
    </html>
</asp:Content>
