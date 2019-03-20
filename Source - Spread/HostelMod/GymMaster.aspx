<%@ Page Title="" Language="C#" MasterPageFile="~/HostelMod/hostelsite.master" AutoEventWireup="true"
    CodeFile="GymMaster.aspx.cs" Inherits="HostelMod_GymMaster" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
       
        function Test() {
            var id = "";
            var idvl = "";
            var empty = "";

            id = document.getElementById("<%=txtgymname.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtgymname.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtAcry.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtAcry.ClientID %>");
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
        function gym() {
            var id = "";
            var idvl = "";
            var empty = "";
            id = document.getElementById("<%=txt_name.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_name.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_acry.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_acry.ClientID %>");
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

        function mygym(x) {
            x.style.borderColor = "#c4c4c4";
        }

        function checkEmail(id) {
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!filter.test(id.value)) {
                id.style.borderColor = 'Red';
                id.value = "";
                email.focus;
            }
            else {
                id.style.borderColor = '#c4c4c4';
            }
        }
        function get(txt1) {
            $.ajax({
                type: "POST",
                url: "GymMaster.aspx/CheckUserName",
                data: '{GymMaster: "' + txt1 + '"}',
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
                    mesg.innerHTML = "Please Enter The Gym Details!";
                    break;
                case "1":
                    mesg.style.color = "green";
                    document.getElementById('<%=txtgymname.ClientID %>').value = "";
                    mesg.innerHTML = "Gym Name Available";
                    break;
//                case "2":
//                    mesg.style.color = "red";
//                    mesg.innerHTML = "Please Enter The Gym Details!";
//                    break;
//                case "error":
//                    mesg.style.color = "red";
//                    mesg.innerHTML = "Error occurred";
//                    break;
            }
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Gym Master</span>
            </div>
        </center>
    </div>
    <br />
    <div>
        <center>
            <div>
                <table>
                    <tr>
                        <td>
                            <center>
                                <div>
                                    <table class="maintablestyle">
                                        <tr>
                                            <td>
                                                &nbsp;
                                                <asp:Button ID="btn_addnew" Text="Add New" runat="server" CssClass="textbox btn2"
                                                    OnClick="btnaddnew_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                                    <br />
                                    <table>
                                        <tr>
                                            <div id="divtable" runat="server">
                                                <table class="tabl" style="width: 409px;">
                                                    <tr>
                                                        <center>
                                                            <FarPoint:FpSpread ID="Fpload1" OnCellClick="Cell_Click1" runat="server" BorderStyle="Solid"
                                                                OnButtonCommand="Fpload_OnButtonCommand" OnPreRender="Fpspread_render" AutoPostBack="false"
                                                                Visible="false" BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                                class="spreadborder">
                                                                <Sheets>
                                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                                    </FarPoint:SheetView>
                                                                </Sheets>
                                                            </FarPoint:FpSpread>
                                                        </center>
                                                    </tr>
                                        </tr>
                                    </table>
                                    <table>
                                        <br />
                                        <tr>
                                            <div style="text-align: center;">
                                                <asp:Label ID="lbprint" runat="server" Visible="false" Style="color: Red;"></asp:Label>
                                                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Visible="false" Text="Report Name"></asp:Label>
                                                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Visible="false" onkeypress="return keyvalue(this)"
                                                    Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:TextBox>
                                                <asp:Button ID="btn_excel" runat="server" Text="Export Excel" Visible="false" Font-Bold="true"
                                                    Font-Names="Book Antiqua" OnClick="btn_excel_Click" />
                                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" Visible="false" OnClick="btnprintmaster_Click"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                            </div>
                                        </tr>
                                    </table>
                                </div>
                    </tr>
                </table>
            </div>
        </center>
    </div>
    <center>
        <div id="divadd" runat="server" class="popupstyle" visible="false" style="height: 39em;
            z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
            top: 15px; left: 0;">
            <asp:ImageButton ID="imgclose" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: -12px; margin-left: 167px;"
                OnClick="imgclose_Click" />
            <center>
                <div style="background-color: White; height: 168px; width: 365px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green;">Gym Details</span></div>
                        <br />
                    </center>
                    <center>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblname" Text="Gym Name" runat="server"></asp:Label>
                                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtgymname" runat="server" OnTextChanged="Gymname_TextChanged" Width="150px"
                                            AutoPostBack="True"></asp:TextBox>
                                         <span style="color: Red;">*</span><span style="font-size: medium;" id="msg1"></span>
                                
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblacr" Text="Gym Acronym" runat="server"></asp:Label>
                                        &nbsp; &nbsp;<asp:TextBox ID="txtAcry" runat="server" OnTextChanged="Gymacr_TextChanged"
                                            Width="150px" AutoPostBack="True"></asp:TextBox>
                                            <span style="color: Red;">*</span><span style="font-size: medium;" id="Span1"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    <br />
                                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                                        <asp:Button ID="Buttonsave" Width="60px" Font-Size="15px" font-weight="bold" runat="server"
                                            Text="Save" CssClass="textbox btn1" OnClick="btnSave_Click" OnClientClick="return Test()"/>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
                <center>
                    <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
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
                                                    <asp:Button ID="btn_alertclose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </center>
        </div>
        <tr>
    </center>
    <center>
        <div id="popwindow1" runat="server" visible="false" class="popupstyle popupheight">
            <br />
            <div class="subdivstyle" style="background-color: White; height: 169px; width: 431px;">
                <asp:ImageButton ID="imgbtn_popclose" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: -38px; margin-left: 206px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_name" Text="Gym Name" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_name" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                            <span style="color: Red;">*</span><span style="font-size: medium;" id="Span2"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_acry" Text="Gym Acroynm" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_acry" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                            <span style="color: Red;">*</span><span style="font-size: medium;" id="Span3"></span>
                        </td>
                    </tr>
                </table>
                <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="panel_description" runat="server" visible="false" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Text="Gym Name" ReadOnly="true" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox1" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Text="Gym Acroynm" ReadOnly="true" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox2" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
                <br />
                <center>
                    <asp:Button ID="btn_update" Text="Update" Visible="false" OnClick="btnupdate_Click"
                        CssClass="textbox btn2" OnClientClick="return gym()" runat="server" />
                    <asp:Button ID="btn_delete" Text="Delete" Visible="false" OnClick="btndelete_Click"
                        CssClass="textbox btn2" OnClientClick="return gym()" runat="server" />
                    <asp:Button ID="btn_save1" Text="Save" runat="server" CssClass="textbox btn2" OnClick="btnpopsave_Click"
                        OnClientClick="return valid1()" />
                    <asp:Button ID="btn_exit1" Text="Exit" runat="server" CssClass="textbox btn2" OnClick="btnpopexit_Click" /></center>
            </div>
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
        <div id="Div1" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label3" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="Button1" CssClass=" textbox textbox1 btn1" Style="height: 28px; width: 65px;"
                                            OnClick="Button1_Click" Text="ok" runat="server" />
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
        <div id="Divdelete" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label4" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="Button2" CssClass=" textbox textbox1 btn1" Style="height: 28px; width: 65px;"
                                            OnClick="Button2_Click" Text="ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
