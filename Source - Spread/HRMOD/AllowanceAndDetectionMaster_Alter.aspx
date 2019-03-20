<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="AllowanceAndDetectionMaster_Alter.aspx.cs" Inherits="AllowanceAndDetectionMaster_Alter" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <%-- <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .stNew
        {
            text-transform: uppercase;
        }
    </style>
    <body>
        <script type="text/javascript">
            function check() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";
                id = document.getElementById("<%=txt_name.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_name.ClientID %>");
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
                document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
            }
            function display1() {
                document.getElementById('<%=txt_excelname.ClientID %>').innerHTML = "";
            }
            function getallded(txt) {
                var list = document.getElementById('<%=rb_allow.ClientID %>');
                var list1 = document.getElementById('<%=rb_deduct.ClientID %>'); //Client ID of the radiolist
                var lst = "";
                if (list.checked == true) {
                    lst = "0";
                }
                if (list1.checked == true) {
                    lst = "1";
                }

                $.ajax({
                    type: "POST",
                    url: "AllowanceAndDetectionMaster_Alter.aspx/checkAlldedName",
                    data: '{AlldedName: "' + txt + '",alltype: "' + lst + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: Success,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function Success(response) {
                var mesg1 = $("#screrr")[0];
                switch (response.d) {
                    case "0":
                        mesg1.style.color = "green";
                        mesg1.innerHTML = "";
                        break;
                    case "1":
                        mesg1.style.color = "red";
                        document.getElementById('<%=txt_name.ClientID %>').value = "";
                        mesg1.innerHTML = "Already Exist!";
                        break;
                    case "2":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Enter AllowDedName";
                        break;
                    case "error":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Error occurred";
                        break;
                }
            }

            function getalldedacr(txt) {
                $.ajax({
                    type: "POST",
                    url: "AllowanceAndDetectionMaster_Alter.aspx/checkAlldedAcr",
                    data: '{AlldedAcr: "' + txt + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: onSuccess,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function onSuccess(response) {
                var mesg1 = $("#screrracr")[0];
                switch (response.d) {
                    case "0":
                        mesg1.style.color = "green";
                        mesg1.innerHTML = "";
                        break;
                    case "1":
                        mesg1.style.color = "red";
                        document.getElementById('<%=txt_allacr.ClientID %>').value = "";
                        mesg1.innerHTML = "Already Exist!";
                        break;
                    case "2":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Enter AllowDedAcr";
                        break;
                    case "error":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Error occurred";
                        break;
                }
            }

        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Allowance/Addition And Deduction
                        Master</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle">
                <center>
                    <br />
                    <div>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox1 ddlheight5"
                                        Width="250px" AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rb_allow" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" RepeatDirection="Horizontal" GroupName="same" Text="Allowances/Additions"
                                        OnCheckedChanged="rb_allow_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rb_deduct" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" RepeatDirection="Horizontal" GroupName="same" Text="Deductions"
                                        OnCheckedChanged="rb_deduct_OncheckedChanged" AutoPostBack="true"></asp:RadioButton>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" runat="server" Width="60px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Height="32px" CssClass="textbox textbox1" Text="Go" OnClick="btn_go_Click" />
                                </td>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_addnew" runat="server" Width="88px" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="32px" CssClass="textbox textbox1" Text="Add New" OnClick="btn_addnew_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
                <br />
                <asp:Label ID="lbl_error" runat="server" Font-Bold="true" ForeColor="Red" Visible="false"></asp:Label>
                <br />
                <center>
                    <table>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                                    BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                    background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder"
                                    OnCellClick="Cell_Click" OnButtonCommand="Fpspread1_ButtonCommand" ShowHeaderSelection="false">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                            <td>
                                <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" BorderStyle="Solid"
                                    BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                    background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder"
                                    OnCellClick="Cell_Click1" OnButtonCommand="Fpspread2_ButtonCommand" ShowHeaderSelection="false">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Button ID="btndel" runat="server" Text="Delete" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="medium" OnClick="btndel_Click" Width="88px" Height="32px" CssClass="textbox textbox1" />
                    <asp:Button ID="btnupdate" runat="server" Text="Update" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="medium" OnClick="btnupdate_Click" Width="88px" Height="32px" CssClass="textbox textbox1" />
                    <br />
                    <br />
                    <div id="rptprint" runat="server" visible="true" style="font-weight: bold;">
                        <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lbl_rptname" runat="server" Text="Report Name" Style="font-family: Book Antiqua;
                            font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                            CssClass="textbox textbox1"></asp:TextBox>
                        <asp:Button ID="btn_excel" runat="server" Text="Export To Excel" Width="127px" CssClass="textbox btn2"
                            OnClick="btn_excel_Click" Style="font-weight: bold; font-family: Book Antiqua;
                            font-size: medium;" />
                        <asp:FilteredTextBoxExtender ID="filt_extenderexcel" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox textbox1"
                            OnClick="btn_printmaster_Click" Width="60px" Height="30px" Style="font-weight: bold;
                            font-family: Book Antiqua; font-size: medium;" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                    <br />
                </center>
                <center>
                    <div id="addnew" runat="server" visible="false" style="height: 200em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 95px; margin-left: 278px;"
                            OnClick="imagebtnpopclose1_Click" />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <div style="background-color: White; height: 440px; width: 584px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <center>
                                <br />
                                <asp:Label ID="lbl_newdesg" runat="server" Font-Bold="true" Style="font-size: large;
                                    color: #790D03;" Text=""></asp:Label>
                                <br />
                            </center>
                            <div>
                                <center>
                                    <table style="padding: 30px;">
                                        <tr>
                                            <td>
                                                College
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_popclg" runat="server" CssClass="textbox1 ddlheight3" Width="200px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_popclg_Change">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_name" runat="server" Text="Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_name" runat="server" MaxLength="25" Font-Names="Book Antiqua"
                                                    Font-Size="medium" Height="25px" CssClass="textbox textbox1" Width="150px" onfocus="return myFunction(this)"
                                                    onblur="return getallded(this.value)"></asp:TextBox>
                                                <span style="color: Red;">*</span><span style="font-weight: bold; font-size: medium;"
                                                    id="screrr"></span>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_name"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_acr" runat="server" Visible="false" Text="Acronym"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_allacr" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                                    Height="25px" Visible="false" MaxLength="3" CssClass="textbox textbox1 stNew"
                                                    Width="150px" onfocus="return myFunction(this)" onblur="return getalldedacr(this.value)"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_desc" runat="server" Text="Description"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_desc" runat="server" MaxLength="20" Font-Names="Book Antiqua"
                                                    Font-Size="medium" CssClass="textbox textbox1" Width="150px" Height="25px" onfocus="return myFunction(this)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_desc"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 10px;">
                                                <asp:CheckBox ID="cb_splallow" runat="server" AutoPostBack="true" Visible="false"
                                                    Text="Special Allowance/Addition" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 10px;">
                                                <asp:CheckBox ID="cb_ItCalcAllow" runat="server" AutoPostBack="true" Visible="false"
                                                    Text="IT Calculation Allowance" />
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="2" style="padding-top: 10px;">
                                                <asp:CheckBox ID="cb_pfdeduct" runat="server" AutoPostBack="true" Visible="false"
                                                    Text="PF Deduction" OnCheckedChanged="cb_pfdeduct_OnCheckedChanged" />
                                                <asp:CheckBox ID="cb_autodeduct" runat="server" AutoPostBack="true" Visible="false"
                                                    Text="Auto Deduction from NetAmount" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 10px;">
                                                <asp:CheckBox ID="cb_ItCalcDeduc" runat="server" AutoPostBack="true" Visible="false"
                                                    Text="IT Calculation Deduction" />
                                            </td>
                                        </tr>
                                    </table>
                                    <center>
                                        <div>
                                            <asp:Button ID="btn_save" runat="server" Visible="true" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="medium" Width="88px" Height="32px" CssClass="textbox textbox1" Text="Save"
                                                OnClientClick="return check()" OnClick="btn_save_Click" />
                                            <asp:Button ID="btn_exit" runat="server" Visible="true" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="medium" Width="88px" Height="32px" CssClass="textbox textbox1" Text="Exit"
                                                OnClick="btn_exit_Click" />
                                        </div>
                                    </center>
                                </center>
                            </div>
                        </div>
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
                                                    <asp:Button ID="btnyes" CssClass="textbox textbox1" Style="height: 28px; width: 65px;"
                                                        OnClick="btnyes_Click" Text="Yes" runat="server" />
                                                    <asp:Button ID="btnno" CssClass="textbox textbox1" Style="height: 28px; width: 65px;"
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
            </div>
        </center>
    </body>
    </html>
</asp:Content>
