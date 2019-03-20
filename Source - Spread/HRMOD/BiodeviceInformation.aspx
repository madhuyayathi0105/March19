<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="BiodeviceInformation.aspx.cs" Inherits="BiodeviceInformation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <script type="text/javascript">
            function display12() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function checktxt() {
                empty = "";
                id = document.getElementById("<%=txtexcelname.ClientID %>").value;
                if (id.trim() == "") {
                    document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "Please Enter Your Report Name";
                    empty = "E";
                }

                if (empty != "") {
                    return false;
                }
                else {

                    return true;
                }
            }
            function check() {

                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";

                id = document.getElementById("<%=txtDevname.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txtDevname.ClientID %>");
                    id.style.borderColor = 'Red';

                    empty = "E";
                }
                id = document.getElementById("<%=txtIpaddress.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txtIpaddress.ClientID %>");
                    id.style.borderColor = 'Red';

                    empty = "E";
                }
                id = document.getElementById("<%=txtMachno.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txtMachno.ClientID %>");
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
        </script>
        <style type="text/css">
            .sty
            {
                font-size: medium;
                font-family: Book Antiqua;
                font-weight: bold;
            }
            .multicheckbox
            {
                z-index: 1;
                left: 258px;
                top: -1222px;
                position: absolute;
                overflow: auto;
                background-color: white;
                border: 1px solid gray;
                color: Black;
            }
        </style>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <br />
                <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="left: 0;
                    position: absolute; width: 100%; height: 21px">
                    <center>
                        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="White" Text="Device Information"></asp:Label>
                    </center>
                    <br />
                </asp:Panel>
                <center>
                    <br />
                    <br />
                    <table class="maintablestyle" style="background-color: #0CA6CA;">
                        <tr>
                            <td>
                                <asp:Label ID="lblcoll" runat="server" Text="College Name  :" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 21px; width: 100px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlcollege_change" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="250px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblName" runat="server" Text="Device Name  :" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 21px; width: 100px;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtdn" runat="server" CssClass="textbox textbox1 txtheight3" ReadOnly="true"
                                            Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">--Select--</asp:TextBox>
                                        <asp:Panel ID="pdn" runat="server" CssClass="multxtpanel" Height="200px" Style="font-family: 'Book Antiqua';"
                                            Width="200px">
                                            <asp:CheckBox ID="chkdn" runat="server" Text="Select All" OnCheckedChanged="chkdn_CheckedChanged"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" />
                                            <asp:CheckBoxList ID="chkldn" runat="server" Font-Size="Medium" Style="font-family: 'Book Antiqua';
                                                width: auto;" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chkldn_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdn"
                                            PopupControlID="pdn" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" CssClass="sty" Text="Go" OnClick="btngo_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="sty" OnClick="btnAdd_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <asp:Panel ID="Panel21" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
                    Style="left: 0; position: absolute; width: 100%;">
                </asp:Panel>
                <br />
                <asp:Label ID="errmsg" runat="server" ForeColor="Red" CssClass="sty" Visible="false"></asp:Label>
                <br />
                <div id="showdata" runat="server">
                    <center>
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                            OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_PreRender" BorderWidth="1px"
                            Visible="true" VerticalScrollBarPolicy="AsNeeded" HorizontalScrollBarPolicy="AsNeeded">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                        <div id="rptprint" runat="server" visible="true">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="" CssClass="sty"
                                Visible="true"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" CssClass="sty" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" CssClass="sty" onkeypress="display12()"></asp:TextBox>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="sty"
                                OnClientClick="return checktxt()" Text="Export To Excel" Width="130px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                CssClass="sty" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                </div>
                <br />
                <br />
                <br />
                <center>
                    <div id="popuperrdiv" visible="false" runat="server" style="height: 400%; z-index: 1000;
                        width: 100%; background-color: rgba(0, 0, 0, 0.72); position: fixed; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div1" runat="server" class="table" style="background-color: white; border-image: none;
                                border-radius: 10px; border-width: 25px 5px 5px; height: auto; margin-top: 135px;
                                width: 500px;">
                                <center>
                                    <asp:Panel ID="pnlstaffm" runat="server" BackColor="LightYellow" Width="500px">
                                        <br />
                                        <table style="height: 80px; width: 100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblDevname" runat="server" Text="Device Name  : " Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Width="130px"></asp:Label>
                                                    <asp:Label ID="lblDevid" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDevname" runat="server" Width="316px" MaxLength="40" Font-Bold="True"
                                                        Font-Names="Book Antiqua" onfocus="return display(this)" Font-Size="Medium" Height="22px">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtDevname"
                                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=".  " />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblIpaddress" runat="server" Width="130px" Text="IP Address*  : "
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtIpaddress" runat="server" Width="200px" Font-Bold="True" MaxLength="25"
                                                        Font-Names="Book Antiqua" onfocus="return display(this)" Font-Size="Medium" Height="22px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtIpaddress"
                                                        FilterType="Numbers,Custom" ValidChars="." />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblMachno" runat="server" Width="130px" Text="Machine No*  : " Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMachno" runat="server" Width="200px" Font-Bold="True" MaxLength="4"
                                                        Font-Names="Book Antiqua" onfocus="return display(this)" Font-Size="Medium" Height="22px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtMachno"
                                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=".  " />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblPortno" runat="server" Text="Port No*  : " Width="130px" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPortno" runat="server" Width="200px" Text="4370" ReadOnly="true"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="22px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblDevtype" runat="server" Text="Device Type*  : " Width="130px" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <div style="border-color: Black;">
                                                        <asp:RadioButton ID="radbtnfinger" runat="server" Text="Finger" GroupName="dt" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                                        <asp:RadioButton ID="radbtnface" runat="server" Text="Face" GroupName="dt" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                                        <asp:RadioButton ID="radbtnfingerface" runat="server" Text="Finger & Face" GroupName="dt"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                        <asp:RadioButton ID="radbtnrfid" runat="server" Text="RFID" Font-Bold="True" GroupName="dt"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lbldevcolor" runat="server" Text="Device Color*  : " Width="130px"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <div>
                                                        <asp:RadioButton ID="radbtnbw" runat="server" Text="B/W" GroupName="ss" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                                        <asp:RadioButton ID="radbtn" runat="server" Text="Color" GroupName="ss" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lbldevfor" runat="server" Text="Device For*  : " Width="130px" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <div>
                                                        <asp:RadioButton ID="radbtnstudent" runat="server" Text="Student" GroupName="studtype"
                                                            Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="true" OnCheckedChanged="radbtnstudent_CheckedChanged"
                                                            Font-Size="Medium" />
                                                        <asp:RadioButton ID="radbtnataff" runat="server" Text="Staff" GroupName="studtype"
                                                            Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="true" OnCheckedChanged="radbtnstudent_CheckedChanged"
                                                            Font-Size="Medium" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlstudtype" runat="server" CssClass="sty" Width="143px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                            <td>
                  <asp:LinkButton ID="myLink" Text="Gatepass SMS Rights" OnClick="LinkButton_Click" 
                                    runat="server"  /></td>
                  </tr>
                                            <tr>
                                                <td align="right" colspan="5">
                                                    <div align="right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="sty" Text="" OnClientClick="return check()"
                                                                        OnClick="btnSave_Click" />
                                                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="sty" OnClick="btnDelete_Click" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnexit" runat="server" CssClass="sty" Text="Exit" OnClick="btnexit_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <div id="imgdiv2" runat="server" visible="false" style="height: 500%; z-index: 1000;
                    width: 100%; background-color: rgba(0, 0, 0, 0.72); position: fixed; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: white; border-image: none;
                            border-radius: 10px; border-width: 25px 5px 5px; height: 300px; margin-top: 100px;
                            width: 500px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: #c94e50; font-family: inherit;
                                                font-size: 34px; font-weight: bold; font-family: Book Antiqua; position: relative;
                                                top: 73px;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" Style="background: #c94e50 none repeat scroll 0 0;
                                                    border: medium none; color: #fff; font-weight: 600; outline: medium none; padding: 1em 2em;
                                                    position: relative; top: 118px;" OnClick="btn_errorclose_Click" Text="Close"
                                                    runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>

                 <div id="Div2" runat="server" visible="false" style="height: 500%; z-index: 1000;
                    width: 100%; background-color: rgba(0, 0, 0, 0.72); position: fixed; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div3" runat="server" class="table" style="background-color: white; border-image: none;
                            border-radius: 10px; border-width: 25px 5px 5px; height: 300px; margin-top: 100px;
                            width: 500px;">
                            <center>
                            <br />
                            <br />
                                <table style="height: 100px; width: 100%">
                                    <tr align="center">
                                    <td>
                                     <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" BorderStyle="Outset" Style="height: 107px; width: 198px;">
                                                <asp:CheckBoxList ID="cblcollege" runat="server" OnSelectedIndexChanged="cblcollege_ChekedChange" AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel></td>
                                        <td >
                                            <asp:Panel ID="Panel4" runat="server" ScrollBars="Auto" BorderStyle="Outset"  Style="height: 119px; width: 210px;">
                                             <asp:CheckBoxList ID="chkStu" runat="server">
                                             <asp:ListItem Value='1'>Student</asp:ListItem>
                                              <asp:ListItem Value='2'>Father</asp:ListItem>
<asp:ListItem Value='3'>Mother</asp:ListItem><asp:ListItem Value='4'>Tutor</asp:ListItem>

                                              </asp:CheckBoxList>
                                                 </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                             <asp:Button ID="BtnSavegate" Style="background: #c94e50 none repeat scroll 0 0;
                                                    border: medium none; color: #fff; font-weight: 600; outline: medium none; padding: 1em 2em;
                                                    position: relative;"  OnClick="btn_Save_Click1" Text="Save"
                                                    runat="server" />
                                                <asp:Button ID="Button1" Style="background: #c94e50 none repeat scroll 0 0;
                                                    border: medium none; color: #fff; font-weight: 600; outline: medium none; padding: 1em 2em;
                                                    position: relative;" OnClick="btn_errorclose_Click1" Text="Close"
                                                    runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
            </Triggers>
        </asp:UpdatePanel>
    </body>
    </html>
</asp:Content>
