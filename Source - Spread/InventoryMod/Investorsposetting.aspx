<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master" AutoEventWireup="true"
    CodeFile="Investorsposetting.aspx.cs" Inherits="Investorsposetting" %>

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
        <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style>
            .requiredfild
            {
                font-family: Book Antiqua;
                font-size: medium;
                font-weight: bold;
            }
        </style>
        <script>
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
        </script>
    </head>
    <body>
        <form id="form1">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <br />
                <div>
                    <center>
                        <br />
                        <asp:Label ID="Label1" runat="server" CssClass="fontstyleheader" Style="color: Green;"
                            Text="Investors PO Settings"></asp:Label>
                        <br />
                        <br />
                    </center>
                </div>
                <center>
                    <div class="maindivstyle" style="height: auto; width: 1000px;">
                        <br />
                        <span style="font-family: Book Antiqua; font-size: large; font-weight: bold;">Header</span>
                        <table style="width: 980px;">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkcontinuref" runat="server" CssClass="requiredfild" AutoPostBack="true"
                                        Text="Continuous Reference No." OnCheckedChanged="chkcontinuref_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_refno" runat="server" Width="120px" Text="Reference No"></asp:Label>
                                    <asp:TextBox ID="txt_refno" CssClass="textbox textbox1 txtheight3" onfocus="return myFunction(this)"
                                        runat="server" MaxLength="10" Style="text-transform: uppercase"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_refno"
                                        FilterType="UppercaseLetters,lowercaseletters,custom,numbers" ValidChars="/ - _  ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="lbl_Refheader" Width="130px" runat="server" Text="Reference Header"></asp:Label>
                                    <asp:TextBox ID="txt_Refheader" CssClass="textbox textbox1 txtheight3" runat="server"
                                        onfocus="return myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_Refheader"
                                        FilterType="UppercaseLetters,lowercaseletters,custom" ValidChars="  ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 980px;">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbplainsheet" runat="server" GroupName="gen" Text="Plain Sheet"
                                        CssClass="requiredfild" AutoPostBack="true" OnCheckedChanged="rbsheet_CheckedChanged" />
                                    <asp:RadioButton ID="rbletterpad" runat="server" GroupName="gen" Text="Letter Pad"
                                        CssClass="requiredfild" AutoPostBack="true" OnCheckedChanged="rbsheet_CheckedChanged" />
                                </td>
                            </tr>
                            <%--  <tr>
                    <td>
                        <asp:CheckBoxList ID="chkcollege" runat="server" Height="43px" AutoPostBack="true"
                            Width="850px" RepeatColumns="5" RepeatDirection="Horizontal">
                            <asp:ListItem>College Name</asp:ListItem>
                            <asp:ListItem>University</asp:ListItem>
                            <asp:ListItem>Affliated By</asp:ListItem>
                            <asp:ListItem>Address</asp:ListItem>
                            <asp:ListItem>City</asp:ListItem>
                            <asp:ListItem>District & State & Pincode</asp:ListItem>
                            <asp:ListItem>Phone No & Fax</asp:ListItem>
                            <asp:ListItem>Email & Web Site</asp:ListItem>
                            <asp:ListItem>Right Logo</asp:ListItem>
                            <asp:ListItem>Left Logo</asp:ListItem>
                            <asp:ListItem>Signature</asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>--%>
                        </table>
                        <table style="width: 980px;" runat="server" id="collinfo">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkselall" runat="server" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="chkselall_CheckedChanged" />
                                    <asp:CheckBoxList ID="chkcollege" runat="server" Height="43px" AutoPostBack="true"
                                        Width="850px" RepeatColumns="5" RepeatDirection="Horizontal">
                                        <asp:ListItem>College Name</asp:ListItem>
                                        <asp:ListItem>University</asp:ListItem>
                                        <asp:ListItem>Affliated By</asp:ListItem>
                                        <asp:ListItem>Address</asp:ListItem>
                                        <asp:ListItem>City</asp:ListItem>
                                        <asp:ListItem>District & State & Pincode</asp:ListItem>
                                        <asp:ListItem>Phone No & Fax</asp:ListItem>
                                        <asp:ListItem>Email & Web Site</asp:ListItem>
                                        <asp:ListItem>Right Logo</asp:ListItem>
                                        <asp:ListItem>Left Logo</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <span style="font-family: Book Antiqua; font-size: large; font-weight: bold;">Footer</span>
                        <table style="width: 980px;">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chk_terms" runat="server" AutoPostBack="true" Text="Terms & Conditions"
                                        CssClass="requiredfild" OnCheckedChanged="chk_terms_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                        <table runat="server" id="terms" style="width: 800px;">
                            <tr>
                                <td>
                                    <div style="border: 1px solid black; border-radius: 14px; margin-top: -58px; height: 150px;
                                        width: 380px;">
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbltermdesc" runat="server" Width="131px" Text="Terms Description"></asp:Label>
                                                    <asp:TextBox ID="txttermdesc" CssClass="textbox textbox1 txtheight3" onfocus="return myFunction(this)"
                                                        runat="server"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txttermdesc"
                                                        FilterType="UppercaseLetters,lowercaseletters,custom,numbers" ValidChars="/ - _  ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbltermvalue" Width="130px" runat="server" Text="Reference Header"></asp:Label>
                                                    <asp:TextBox ID="txttermvalue" CssClass="textbox textbox1 txtheight3" runat="server"
                                                        onfocus="return myFunction(this)"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txttermvalue"
                                                        FilterType="UppercaseLetters,lowercaseletters,custom" ValidChars="  ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Button ID="btnadd" runat="server" Text="Add" OnClick="btnadd_Click" Width="50px"
                                                        Height="35px" CssClass="textbox textbox1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td>
                                    <div style="border: 1px solid black; border-radius: 14px; height: 150px; width: 380px;
                                        overflow: hidden;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="AsNeeded" HorizontalScrollBarPolicy="Never"
                                                            OnButtonCommand="Fpspread1_Command">
                                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                                ButtonShadowColor="ControlDark">
                                                            </CommandBar>
                                                            <Sheets>
                                                                <FarPoint:SheetView SheetName="Sheet1">
                                                                </FarPoint:SheetView>
                                                            </Sheets>
                                                        </FarPoint:FpSpread>
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <table style="width: 379px;">
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Button ID="btndeletefp1" runat="server" Text="Delete" OnClick="btndeletefp1_Click"
                                                    Width="50px" Height="35px" CssClass="textbox textbox1" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table style="width: 980px;">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkfromaddress" runat="server" AutoPostBack="true" Text="From Address   Designation"
                                        CssClass="requiredfild" OnCheckedChanged="chkfromaddress_CheckedChanged" />
                                    <asp:TextBox ID="txtdesign" CssClass="textbox textbox1 txtheight3" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtdesign"
                                        FilterType="UppercaseLetters,lowercaseletters,custom" ValidChars="/ - _  ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 980px;">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkfootersign" runat="server" AutoPostBack="true" Text="Footer Signature"
                                        CssClass="requiredfild" OnCheckedChanged="chkfootersign_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                        <table runat="server" id="Table1" style="width: 800px;">
                            <tr>
                                <td>
                                    <div style="border: 1px solid black; border-radius: 14px; margin-top: -38px; height: 150px;
                                        width: 380px;">
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbldescrip" runat="server" Width="131px" Text="Description"></asp:Label>
                                                    <asp:TextBox ID="txtdescrip" CssClass="textbox textbox1 txtheight3" runat="server"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtdescrip"
                                                        FilterType="UppercaseLetters,lowercaseletters,custom,numbers" ValidChars="/ - _  ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblstaff" Width="130px" runat="server" Text="Staff"></asp:Label>
                                                    <asp:TextBox ID="txtstaff" CssClass="textbox textbox1 txtheight3" runat="server"
                                                        onfocus="return myFunction(this)"></asp:TextBox>
                                                    <asp:Label ID="lblstaffcode" Width="130px" Visible="false" runat="server"></asp:Label>
                                                    <%--  <asp:Button ID="FindBtn" runat="server" Text="Select Staff" Font-Names="Book Antiqua"
                                                            Font-Bold="true" OnClick="FindBtn_Click"  />--%>
                                                    <asp:Button ID="FindBtn" runat="server" Text="?" OnClick="FindBtn_Click" Width="50px"
                                                        Height="35px" CssClass="textbox textbox1" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Button ID="btnstaffadd" runat="server" Text="ADD" OnClick="btnstaffadd_Click"
                                                        Width="50px" Height="35px" CssClass="textbox textbox1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                </td>
                                <td>
                                    <div style="border: 1px solid black; border-radius: 14px; height: 150px; width: 380px;
                                        overflow: hidden;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="AsNeeded" HorizontalScrollBarPolicy="Never"
                                                            OnButtonCommand="Fpspread2_Command">
                                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                                ButtonShadowColor="ControlDark">
                                                            </CommandBar>
                                                            <Sheets>
                                                                <FarPoint:SheetView SheetName="Sheet1">
                                                                </FarPoint:SheetView>
                                                            </Sheets>
                                                        </FarPoint:FpSpread>
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <table style="width: 379px;">
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Button ID="btndeletefp2" runat="server" Text="Delete" OnClick="btndeletefp2_Click"
                                                    Width="50px" Height="35px" CssClass="textbox textbox1" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table style="width: 980px;">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chksignaturewithseal" runat="server" AutoPostBack="true" Text="Signature With Seal"
                                        CssClass="requiredfild" OnCheckedChanged="chksignaturewithseal_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:Button ID="btnfinalsave" runat="server" Text="Save" OnClick="btnfinalsave_Click"
                                        Width="50px" Height="35px" CssClass="textbox textbox1" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                    </div>
                </center>
                <center>
                    <div id="imgshowdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        position: fixed; left: 0px;">
                        <center>
                            <asp:Panel ID="panel3" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                                BorderWidth="2px" Style="left: 353px; top: 47px; position: absolute;" Height="480px"
                                Width="515px">
                                <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-size: Small;">
                                    <br />
                                    <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                                        left: 200px">
                                        Select Staff Incharge
                                    </caption>
                                    <br />
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblcollege" runat="server" Text="College"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcollege" runat="server" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddldepratstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblsearchby" runat="server" Text="Staff By"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                                    <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                                    AutoPostBack="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                        <FarPoint:FpSpread ID="fsstaff" runat="server" ActiveSheetViewIndex="0" Height="300"
                                            Width="398" CommandBar-Visible="false" VerticalScrollBarPolicy="AsNeeded" BorderWidth="0.5"
                                            Visible="False">
                                            <CommandBar BackColor="Control" ButtonType="PushButton">
                                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                        <fieldset style="position: absolute; left: 345px; visibility: visible; top: 426px;
                                            width: 140px; height: 25px;">
                                            <%--    <asp:Button runat="server" ID="Button1" AutoPostBack="True" Text="Ok" Font-Bold="true"
                                    OnClick="btnstaffadd_Click" Style="width: 75px; top: 2px; position: absolute;
                                    left: 2px;" />--%>
                                            <asp:Button ID="Button1" runat="server" Text="OK" OnClick="btnstaffadd1_Click" Style="width: 75px;
                                                top: 2px; position: absolute; left: 2px;" Width="50px" Height="35px" CssClass="textbox textbox1" />
                                            <%--    <asp:Button runat="server" ID="btnexit" AutoPostBack="True" Text="Exit" Font-Bold="true"
                                    OnClick="btnexit_Click" Style="width: 75px; top: 2px; position: absolute; left: 85px;" />--%>
                                            <asp:Button ID="btnexit" runat="server" Text="Exit" OnClick="btnexit_Click" Style="width: 75px;
                                                top: 2px; position: absolute; left: 85px;" Width="50px" Height="35px" CssClass="textbox textbox1" />
                                        </fieldset>
                                    </div>
                            </asp:Panel>
                        </center>
                    </div>
                </center>
                <center>
                    <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        position: fixed; left: 0px;">
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
                                                    <asp:Button ID="btn_errorclose" CssClass="textbox textbox1" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_errorclose_Click" Text="Ok" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </ContentTemplate>
            <%-- <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnprintmaster" />
        </Triggers>--%>
        </asp:UpdatePanel>
        </form>
    </body>
    </html>
</asp:Content>
