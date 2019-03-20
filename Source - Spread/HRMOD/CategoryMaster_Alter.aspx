<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="CategoryMaster_Alter.aspx.cs" Inherits="CategoryMaster_Alter" %>

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
    <script type="text/javascript">

        function check() {
            var id = "";
            var value1 = "";
            var idval = "";
            var empty = "";
            id = document.getElementById("<%=txt_categname.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_categname.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_catcode.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_catcode.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            if (empty != "") {
                return false;
            }
            else {

                return true;
            }
        }
        function checkadd() {
            var id = "";
            var value1 = "";
            var idval = "";
            var empty = "";
            id = document.getElementById("<%=txt_description11.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_description11.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            if (empty != "") {
                return false;
            }
            else {

                return true;
            }
        }
        function display(x) {
            x.style.borderColor = "#c4c4c4";
        }
        function display() {
            document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
        }

        function getCatAcr(txt) {
            $.ajax({
                type: "POST",
                url: "CategoryMaster_Alter.aspx/checkCatAcr",
                data: '{CatAcr: "' + txt + '"}',
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
                    document.getElementById('<%=txt_catcode.ClientID %>').value = "";
                    mesg1.innerHTML = "Already Exist!";
                    break;
                case "2":
                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Enter Category Code";
                    break;
                case "error":
                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Error occurred";
                    break;
            }
        }

        function getCatName(txt) {
            $.ajax({
                type: "POST",
                url: "CategoryMaster_Alter.aspx/checkCatName",
                data: '{CatName: "' + txt + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onSuccess,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function onSuccess(response) {
            var mesg1 = $("#screrrname")[0];
            switch (response.d) {
                case "0":
                    mesg1.style.color = "green";
                    mesg1.innerHTML = "";
                    break;
                case "1":
                    mesg1.style.color = "red";
                    document.getElementById('<%=txt_categname.ClientID %>').value = "";
                    mesg1.innerHTML = "Already Exist!";
                    break;
                case "2":
                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Enter CategoryName";
                    break;
                case "error":
                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Error occurred";
                    break;
            }
        }

    </script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <br />
                <div>
                    <span class="fontstyleheader" style="color: Green;">Category Master</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle">
                <%--maincontent--%>
                <center>
                    <div>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox1 ddlheight4"
                                        Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_catname" runat="server" Text="Category Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_catname" runat="server" CssClass="textbox1 ddlheight4"
                                        Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddl_catname_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_grp" runat="server" Text="Group"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_grp" runat="server" CssClass="textbox1 ddlheight4" Width="200px"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_grp_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox textbox1" Width="45px" Height="32px"
                                        Text="Go" OnClick="btn_go_Click" />
                                    <asp:Button ID="btn_addnew" runat="server" CssClass="textbox textbox1" Width="68px"
                                        Height="32px" Text="Add New" OnClick="btn_addnew_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
                <br />
                <br />
                <center>
                    <div id="div1" runat="server" visible="true" style="width: 850px; height: 350px;
                        overflow: auto; border: 1px solid Gray; background-color: White; border-radius: 10px;">
                        <br />
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                            BorderWidth="0px" Width="750px" Height="300px" Style="overflow: auto; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            CssClass="spreadborder" OnCellClick="FpSpread1_OnCellClick" OnPreRender="FpSpread1_Selectedindexchange">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <div id="rptprint" runat="server" visible="true" style="font-weight: bold;">
                        <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false" onkeypress="display()"></asp:Label>
                        <asp:Label ID="lbl_rptname" runat="server" Text="Report Name" Style="font-family: Book Antiqua;
                            font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                            CssClass="textbox textbox1"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_excel" runat="server" Text="Export To Excel" Width="145px" Height="32px"
                            CssClass="textbox textbox1" OnClick="btn_excel_Click" Style="font-weight: bold;
                            font-family: Book Antiqua; font-size: medium;" />
                        <asp:FilteredTextBoxExtender ID="filt_extenderexcel" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" Width="77px" Height="32px"
                            CssClass="textbox textbox1" OnClick="btn_printmaster_Click" Style="font-weight: bold;
                            font-family: Book Antiqua; font-size: medium;" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </center>
                <br />
                <%-- ******************popup1**********--%>
                <center>
                    <div id="addnew" runat="server" visible="false" style="height: 50em; z-index: 1000;
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
                        <div style="background-color: White; height: 390px; width: 584px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <center>
                                <asp:Label ID="lbl_newdesg" runat="server" Style="font-size: large; color: #790D03;"
                                    Text="Category Name"></asp:Label>
                                <br />
                                <br />
                                <br />
                            </center>
                            <div>
                                <center>
                                    <table style="line-height: 40px;">
                                        <tr>
                                            <td>
                                                College
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_popclg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_popclg_Change"
                                                    CssClass="textbox1 ddlheight4" Width="208px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_catcode" runat="server" Text="Category Code"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_catcode" runat="server" Height="25px" CssClass="textbox textbox1 stNew"
                                                    MaxLength="6" Width="100px" onblur="return getCatAcr(this.value)"></asp:TextBox>
                                                <span style="color: Red;">*</span><span style="font-weight: bold; font-size: larger;"
                                                    id="screrr"></span>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_catcode"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_categname" runat="server" Text="Category Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_categname" runat="server" Height="25px" MaxLength="40" onfocus=" return display(this)"
                                                    CssClass="textbox textbox1" Width="200px" onblur="return getCatName(this.value)"></asp:TextBox>
                                                <span style="color: Red;">*</span><span style="font-weight: bold; font-size: larger;"
                                                    id="screrrname"></span>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_categname"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_group" runat="server" Text="Group"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox textbox1" Font-Bold="true"
                                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" Height="33px"
                                                    Width="35px" />
                                                <asp:DropDownList ID="ddl_group" runat="server" Height="35px" CssClass="textbox textbox1 ddlstyle ddlheight4">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                    Height="33px" Width="35px" Font-Names="Book Antiqua" CssClass="textbox textbox1"
                                                    OnClick="btnminus_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cb_report" runat="server" Text="Display in Reports" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                    </table>
                                    <center>
                                        <div>
                                            <p>
                                                <asp:Label ID="msgg1" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                            </p>
                                            <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" Width="700px" Style="overflow: auto;
                                                height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                                                box-shadow: 0px 0px 8px #999999;">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                        </div>
                                        <br />
                                        <center>
                                            <div>
                                                <asp:Button ID="btn_save" runat="server" Visible="true" Width="68px" Height="32px"
                                                    CssClass="textbox textbox1" Text="Save" OnClientClick="return check()" OnClick="btn_save_Click" />
                                                <asp:Button ID="btndel" runat="server" Visible="true" Width="68px" Height="32px"
                                                    CssClass="textbox textbox1" Text="Delete" OnClientClick="return check()" OnClick="btndel_Click" />
                                                <asp:Button ID="btn_exit" runat="server" Visible="true" Width="68px" Height="32px"
                                                    CssClass="textbox textbox1" Text="Exit" OnClick="btn_exit_Click" /><br />
                                            </div>
                                        </center>
                                    </center>
                                </center>
                            </div>
                        </div>
                    </div>
                </center>
                <%---------end of popup--------%>
                <div id="imgdiv3" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="panel_description" runat="server" visible="false" class="table" style="background-color: White;
                            height: auto; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <table>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_description11" runat="server" Text="Add Group"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_description11" runat="server" Height="25px" onfocus=" return display(this)"
                                            Style="text-transform: capitalize;" CssClass="textbox textbox1" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <br />
                                <tr>
                                    <td align="center">
                                        <br />
                                        <asp:Button ID="btn_adddesc1" runat="server" Visible="true" CssClass="textbox textbox1"
                                            Width="68px" Height="32px" Text="Add" OnClientClick="return checkadd()" OnClick="btndescpopadd_Click" />
                                        <asp:Button ID="btn_exitdesc1" runat="server" Visible="true" CssClass="textbox textbox1"
                                            Width="68px" Height="32px" Text="Exit" OnClick="btndescpopexit_Click" />
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
                <%--************--%>
                <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="panel_erroralert" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_erroralert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_erroralert" CssClass="textbox textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
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
    </body>
    </html>
</asp:Content>
