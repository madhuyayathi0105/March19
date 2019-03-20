<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="SmsSettings.aspx.cs"
    Inherits="SmsSettings" MasterPageFile="~/smsmod/SMSSubSiteMaster.master" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>SMS Settings</title>
    <link rel="Shortcut Icon" href="../college/Left_Logo.jpeg" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=lblerr.ClientID%>').text("");
            $('#<%=lblvalidation1.ClientID%>').text("");
            $('#<%=addbtnsave.ClientID%>').click(function () {
                var txturl = $('#<%=txtsms.ClientID%>').val();
                var userid = $('#<%=txtuserid.ClientID%>').val();
                var sendid = $('#<%=txtsendid.ClientID%>').val();
                var passwd = $('#<%=txtpass.ClientID%>').val();
                if (txturl != "" && userid != "" && sendid != "" && passwd != "") {
                    $('#<%=lblerr.ClientID%>').text("");
                    return true;
                }
                else {
                    $('#<%=lblerr.ClientID%>').text("Please Fill The Values");
                    return false;
                }
            });
            $('#<%=txtsms.ClientID%>').keypress(function () {
                $('#<%=lblerr.ClientID%>').text("");
            });
            $('#<%=txtuserid.ClientID%>').keypress(function () {
                $('#<%=lblerr.ClientID%>').text("");
            });
            $('#<%=txtsendid.ClientID%>').keypress(function () {
                $('#<%=lblerr.ClientID%>').text("");
            });
            $('#<%=txtpass.ClientID%>').keypress(function () {
                $('#<%=lblerr.ClientID%>').text("");
            });
            $('#<%=txtexcelname.ClientID%>').keypress(function () {
                $('#<%=lblvalidation1.ClientID%>').text("");
            });

            $('#<%=btnExcel.ClientID %>').click(function () {
                var val = $('#<%=txtexcelname.ClientID%>').val();
                if (val != "") {
                    $('#<%=lblvalidation1.ClientID%>').text("");
                    return true;
                }
                else {
                    $('#<%=lblvalidation1.ClientID%>').text("Please Enter Your Sms Setting Report Name");
                    $('#<%=lblvalidation1.ClientID%>').show();
                    return false;
                }

            });

            $('#<%=btndel.ClientID %>').click(function () {
                if (confirm("Are You Sure Want To Delete This Record")) {
                    return true;
                }
                else
                    return false;
            });

        });
    </script>
    <div>
      <center>
        <span class="fontstyleheader" style="color: Green;">Sms Settings</span>
    </center>
    <br />
        <center>
            <div class="maindivstyle" style="width: 950px; height: 560px;">
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtclg" runat="server" CssClass="textbox textbox1 txtheight " Style="height: 20px;
                                        width: 250px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 270px;
                                        height: 120px;">
                                        <asp:CheckBox ID="cbclg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cbclg_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cblclg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblclg_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtclg"
                                        PopupControlID="pnlclg" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblDept" runat="server" Text="Branch"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Up_dept" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel multxtpanleheight">
                                        <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                        PopupControlID="panel_dept" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td colspan="2">
                            <asp:Button ID="btngo" runat="server" Height="30px" Width="80px" CssClass="textbox textbox1 btn3"
                                Text="Go" OnClick="btngo_Click" />
                            <asp:Button ID="btnadd" runat="server" Height="30px" Width="80px" CssClass="textbox textbox1 btn3"
                                Text="Add" OnClick="btnadd_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <div id="divspread" runat="server" visible="false" style="height: 400px; overflow: auto;">
                    <FarPoint:FpSpread ID="fpreport" runat="server" Visible="true" BorderStyle="Solid"
                        BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                        class="spreadborder" OnCellClick="fpreport_OnCellClick" OnPreRender="fpreport_Selectedindexchanged">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <div>
                    <center>
                        <div id="print" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" Style="margin-top: -18px; display: none;
                                margin-left: 10px; position: absolute;" Font-Names="Book Antiqua" Font-Size="Medium"
                                ForeColor="Red" Text=""></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                            <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                CssClass="textbox textbox1" Width="60px" />
                            <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <%--add screen--%>
        <div id="divadd" runat="server" visible="false" style="height: 44em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
            left: 0;">
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                width: 30px; position: absolute; margin-top: 12px; margin-left: 909px;" OnClick="imagepopclose_click" />
            <br />
            <center>
                <div style="height: 430px; width: 850px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                    border-radius: 10px; background-color: White;">
                    <br />
                    <span style="font-family: Book Antiqua; font-size: 25px; font-weight: bold; color: Green;">
                        Add Settings </span>
                    <br />
                    <table class="maintablestyle" width="800px">
                        <tr>
                            <td>
                                <asp:Label ID="addlblclg" runat="server" Text="College"></asp:Label>
                            </td>
                            <td id="tdddl" runat="server" visible="false">
                                <asp:DropDownList ID="addddlclg" CssClass="textbox textbox1 ddlheight4" Height="30px"
                                    Width="250px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="addddlclg_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="tdcbl" runat="server" visible="false">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="addtxtclg" CssClass="textbox textbox1 txtheight " runat="server"
                                            Style="width: 250px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="addpnlhedg" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="width: 270px; height: 120px;">
                                            <asp:CheckBox ID="addcbhedg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="addcbhedg_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="addcblhedg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="addcblhedg_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="addtxtclg"
                                            PopupControlID="addpnlhedg" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblDept2" runat="server" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updept2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtDept2" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlDept2" runat="server" CssClass="multxtpanleheight multxtpanel"
                                            Style="padding: 2px; width: 140px; margin: 0px;">
                                            <asp:CheckBox ID="cb_dept2" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_dept2_OnCheckedChanged" Style="padding: 3px; width: auto;
                                                margin: 0px;" />
                                            <asp:CheckBoxList ID="cbl_dept2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept2_OnSelectedIndexChanged"
                                                Style="padding: 3px; width: auto; margin: 0px;">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtDept2"
                                            PopupControlID="pnlDept2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rbmode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                    OnSelectedIndexChanged="rbmode_OnSelected">
                                    <asp:ListItem Text="Single" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Multiple" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblerr" runat="server" Text="" Style="color: Red;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <fieldset style="height: 180px; width: 432px; text-align: center;">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblsms" runat="server" Text="Sms Sender Api"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtsms" runat="server" Style="height: 20px; width: 300px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblreceive" runat="server" Text="Sms Reporter Api"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtreceive" runat="server" Style="height: 20px; width: 300px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblusid" runat="server" Text="User Id"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtuserid" runat="server" Style="height: 20px; width: 300px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblpas" runat="server" Text="Password"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtpass" runat="server" Style="height: 20px; width: 300px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblsendid" runat="server" Text="Token"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtsendid" runat="server" Style="height: 20px; width: 300px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            
                                        </table>
                                    </center>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <center>
                                    <asp:Button ID="addbtnsave" runat="server" Height="30px" Width="80px" CssClass="textbox textbox1 btn3"
                                        Text="Save" OnClick="addtbtnsave_Click" />
                                    <asp:Button ID="btndel" runat="server" Visible="false" Height="30px" Width="80px"
                                        CssClass="textbox textbox1 btn3" Text="Delete" OnClick="btndel_Click" />
                                    <%--OnClientClick="return confirm('Are you sure you want to delete this event?');"--%>
                                    <asp:Button ID="addbtncancel" runat="server" Height="30px" Width="80px" CssClass="textbox textbox1 btn3"
                                        Text="Cancel" OnClick="addbtncancel_Click" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
        <%--alertmsg--%>
        <center>
            <div id="imgalert" runat="server" visible="false" style="height: 200%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 216px;
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
</asp:Content>
