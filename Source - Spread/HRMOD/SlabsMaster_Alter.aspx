<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="SlabsMaster_Alter.aspx.cs" Inherits="SlabsMaster_Alter" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
   <%-- <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
      <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">

        function check() {
            var id = "";
            var value1 = "";
            var idval = "";
            var empty = "";
            id = document.getElementById("<%=txt_salfrm.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_salfrm.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_salto.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_salto.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_slbvalue.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_slbvalue.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_empslbvalues.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_empslbvalues.ClientID %>");
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

      
    </script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Slab Master</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="height: 550px; width: 1000px;">
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_clg" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_clg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_clg_SelectedChanged"
                                CssClass="textbox1 ddlheight3" Style="width: 250px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RadioButton ID="rb_all" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Text="Allowance" AutoPostBack="true" OnCheckedChanged="rb_all_CheckedChanged"
                                GroupName="rb_check" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rb_ded" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Text="Deduction" AutoPostBack="true" OnCheckedChanged="rb_ded_CheckedChanged"
                                GroupName="rb_check" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rb_grad" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Text="Gradepay" AutoPostBack="true" OnCheckedChanged="rb_grad_CheckedChanged"
                                GroupName="rb_check" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_slbval" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Text="Slab Value For"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddl_slbval" runat="server" CssClass="ddlheight3 textbox1" Style="width: 128px;"
                                OnSelectedIndexChanged="ddl_val_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:Button ID="btn_go" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Width="49px" Height="32px" CssClass="textbox textbox1" Text="Go" OnClick="btn_go_Click" />
                            <asp:Button ID="btn_addnew" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Text="Add New" Width="88px" Height="32px" CssClass="textbox textbox1" OnClick="btn_addnew_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
                <div id="div1" runat="server" visible="false" style="width: 731px; height: 351px;
                    overflow: auto; background-color: White; border-radius: 10px; box-shadow: 0px 0px 8px #999999;"
                    class="reportdivstyle">
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderStyle="Solid" BorderWidth="0px"
                        Width="728px" OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                        InvalidChars="/\">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                        Height="32px" CssClass="textbox textbox1" />
                    <asp:Button ID="btnprintmaster" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                        CssClass="textbox textbox1" Width="60px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </div>
            <div id="addnew" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 322px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; height: 360px; width: 690px;
                    top: 70px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_slbdetails" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Style="font-size: large; color: Green;" Text="Slab Details"></asp:Label>
                    </center>
                    <br />
                    <table>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblnewcoll" runat="server" Text="College Name" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                                <asp:DropDownList ID="ddl_newcol" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_newcol_Change"
                                    CssClass="textbox1 ddlheight3" Width="250px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:RadioButton ID="rb_all_add" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Text="Allowance" AutoPostBack="true" OnCheckedChanged="rb_all_add_CheckedChanged"
                                    GroupName="rb_check" />
                                <asp:RadioButton ID="rb_ded_add" runat="server" Text="Deduction" AutoPostBack="true"
                                    OnCheckedChanged="rb_ded_add_CheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                    GroupName="rb_check" />
                                <asp:RadioButton ID="rb_gradl_add" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Text="Gradepay" AutoPostBack="true" OnCheckedChanged="rb_gradl_add_CheckedChanged"
                                    GroupName="rb_check" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Font-Names="Book Antiqua" Text="Slab Value For"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_slbvaladd" runat="server" CssClass="textbox textbox1 ddlheight4" Style="width: 146px;"
                                    OnSelectedIndexChanged="ddl_slbvaladd_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_catg" Width="24px" runat="server" Font-Names="Book Antiqua" Text="Category"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_catg" runat="server" AutoPostBack="true" Width="146px"
                                    CssClass="textbox textbox1 ddlheight4">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_salfrm" runat="server" Width="130px" Font-Names="Book Antiqua"
                                    Text="Salary From"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_salfrm" CssClass="textbox textbox1 txtheight3" runat="server"
                                    OnTextChanged="txt_salfrm_OnTextChanged" AutoPostBack="true" MaxLength="7" onfocus="return myFunction(this)"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_salfrm"
                                    FilterType="Numbers,custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="lbl_oldsalf" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_salto" Width="80px" runat="server" Font-Names="Book Antiqua" Text="Salary To"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_salto" CssClass="textbox textbox1 txtheight3" runat="server"
                                    OnTextChanged="txt_salto_OnTextChanged" AutoPostBack="true" MaxLength="7" onfocus="return myFunction(this)"
                                    onkeyup="return get(this.value)"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_salto"
                                    FilterType="Numbers,custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="lbl_oldsalt" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_slbtype" Width="130px" Font-Names="Book Antiqua" runat="server"
                                    Text="Emp. Slab Type"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_slbtype" runat="server" CssClass="textbox textbox1" Width="145px"
                                    Height="30px" AutoPostBack="true" OnSelectedIndexChanged="ddl_slbtype_OnSelectedIndexChanged">
                                    <asp:ListItem>Amount</asp:ListItem>
                                    <asp:ListItem>Percent</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_slbvalue" Width="120px" runat="server" Font-Names="Book Antiqua"
                                    Text="Emp. Slab Value"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_slbvalue" CssClass="textbox textbox1 txtheight3" runat="server"
                                    MaxLength="7" onfocus="return myFunction(this)"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_slbvalue"
                                    FilterType="Numbers,custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_empslbtype" runat="server" Font-Names="Book Antiqua" Text="Mgt. Slap Type"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_empslbtype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_empslbtype_Change"
                                    CssClass="textbox textbox1" Width="145px" Height="30px">
                                    <asp:ListItem>Amount</asp:ListItem>
                                    <asp:ListItem>Percent</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_empslbval" runat="server" Width="150px" Font-Names="Book Antiqua"
                                    MaxLength="7" Font-Size="Medium" Text=" Mgt. Slab Values"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_empslbvalues" MaxLength="5" runat="server" CssClass="textbox textbox1"
                                    onfocus="return myFunction(this)" Height="20px" Width="135px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_empslbvalues"
                                    FilterType="Numbers,custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <center>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                        Width="88px" Height="32px" CssClass="textbox textbox1" OnClientClick="return check()"
                                        OnClick="btnsave_Click" Visible="false" />
                                    <asp:Button ID="btndel" runat="server" Text="Delete" Font-Bold="True" Font-Names="Book Antiqua"
                                        Width="88px" Height="32px" CssClass="textbox textbox1" OnClick="btndel_Click" />
                                    <asp:Button ID="btnexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        Width="88px" Height="32px" CssClass="textbox textbox1" OnClick="btnexit_Click" />
                                </center>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                </div>
            </div>
            <div id="Newdiv" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 51px; margin-left: 331px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
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
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
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
