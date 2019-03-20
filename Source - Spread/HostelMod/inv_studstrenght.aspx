<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="inv_studstrenght.aspx.cs" Inherits="inv_studstrenght" %>

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
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <form id="form1">
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function prevdateEnable() {
                var cblid = document.getElementById("<%=cb_prevdate.ClientID %>");
                var txtdate = document.getElementById("<%=txt_prevDate.ClientID %>");
                if (cblid.checked == true) {
                    txtdate.style.display = "block";
                } else { txtdate.style.display = "none"; }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <span style="color: #008000;" class="fontstyleheader">Student Strength Status</span>
                <br />
                <div class="maindivstyle" style="height: 520px; width: 1000px;">
                    <br />
                    <table style="margin-left: 10px; position: absolute; width: 981px; height: 83px;"
                        class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblhostelname" Text="Mess Name" Style="top: 15px; left: 10px; position: absolute;"
                                    runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlhostelname" runat="server" Style="top: 10px; left: 104px;
                                    position: absolute;" AutoPostBack="true" CssClass=" textbox1 ddlheight3" OnSelectedIndexChanged="ddlhostelname_Change">
                                </asp:DropDownList>
                                <%--<asp:UpdatePanel ID="upp1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txthostelname" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                        Width="120px" Height="18px" Style="top: 10px; left: 104px; position: absolute;
                                        " Font-Size="Medium">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel4" runat="server" Height="200px" Width="200px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="Chk1sec" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Text="Select All" AutoPostBack="true" OnCheckedChanged="Chksechosname" />
                                        <asp:CheckBoxList ID="Cbl1sec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cblsechosname">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txthostelname"
                                        PopupControlID="Panel4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                            </td>
                            <td>
                                <asp:Label ID="lblsessionname" Text="Session Name" Style="top: 15px; left: 244px;
                                    position: absolute;" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtsessionname" runat="server" CssClass="textbox textbox1 txtheight2"
                                            ReadOnly="true" Style="top: 10px; left: 353px; position: absolute;">--Select--</asp:TextBox>
                                        <asp:Panel ID="Psession" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                            width: 150px;">
                                            <asp:CheckBox ID="chksessionname" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="chksession_checkedchange" />
                                            <asp:CheckBoxList ID="chklstsession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklstsession_Change">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsessionname"
                                            PopupControlID="Psession" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Schedule Type" Style="top: 15px; left: 490px;
                                    position: absolute;"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButton ID="rb1" runat="server" Text="DateWise" AutoPostBack="true" OnCheckedChanged="radiobtn1"
                                    Style="top: 15px; left: 600px; position: absolute;" />
                                <asp:RadioButton ID="rb2" runat="server" Text="DayWise" AutoPostBack="true" OnCheckedChanged="radiobtn2"
                                    Style="top: 15px; left: 690px; position: absolute;" />
                            </td>
                            <td>
                                &nbsp;&nbsp;
                                <asp:Label ID="lbl_fromdate" runat="server" Text="From Date" Style="top: 53px; left: 19px;
                                    position: absolute;"></asp:Label>
                                <asp:TextBox ID="txtfrom" runat="server" AutoPostBack="true" ForeColor="Black" OnTextChanged="txtfrom_TextChanged"
                                    CssClass="textbox textbox1 txtheight" Style="top: 47px; left: 104px; position: absolute;"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfrom" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate" runat="server" Text="To Date" Style="top: 53px; left: 197px;
                                    position: absolute;"></asp:Label>
                                <asp:TextBox ID="txtto" runat="server" AutoPostBack="true" ForeColor="Black" OnTextChanged="txtto_TextChanged"
                                    CssClass="textbox textbox1 txtheight" Style="top: 47px; left: 255px; position: absolute;"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtto" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBoxList ID="cblTotaltype" runat="server" Enabled="false" RepeatDirection="Horizontal"
                                    Style="top: 50px; left: 348px; position: absolute;">
                                    <asp:ListItem>H</asp:ListItem>
                                    <asp:ListItem>D</asp:ListItem>
                                    <asp:ListItem>S</asp:ListItem>
                                    <asp:ListItem>G</asp:ListItem>
                                    <asp:ListItem>T</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_menutype" runat="server" Text="Menu Type" Style="top: 53px; left: 522px;
                                    position: absolute;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanelnew" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_menutype" runat="server" CssClass="textbox textbox1" Width="85px"
                                            Height="20px" ReadOnly="true" Style="top: 47px; left: 602px; position: absolute;">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Height="81px" Width="93px">
                                            <asp:CheckBox ID="cb_menutype" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_menutype_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_menutype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_menutype_SelectIndexChange">
                                                <asp:ListItem Value="0">Veg</asp:ListItem>
                                                <asp:ListItem Value="1">Non-Veg</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtendernew" runat="server" TargetControlID="txt_menutype"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_click" CssClass="textbox btn1"
                                    Style="top: 47px; left: 699px; position: absolute;" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cb_prevdate" runat="server" Text="Previous Date" Style="top: 15px;
                                    left: 770px; position: absolute;" onchange="return prevdateEnable(this);" />
                            </td>
                            <td>
                                <asp:TextBox ID="txt_prevDate" runat="server" ForeColor="Black" CssClass="textbox textbox1 txtheight"
                                    Style="top: 10px; left: 888px; position: absolute; display: none;"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_prevDate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_menuPurposeCatagory" runat="server" Text="Purpose Catagory" Style="top: 52px;
                                    left: 743px; position: absolute;"></asp:Label>
                            </td>
                            <td id="purposecatagoryTD" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_purposecatagory" runat="server" CssClass="textbox textbox1"
                                            Width="85px" Height="20px" ReadOnly="true" Style="top: 47px; left: 870px; position: absolute;">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Height="100px" Width="120px">
                                            <asp:CheckBox ID="cb_purposecatagory" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_purposecatagory_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_purposecatagory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_purposecatagory_SelectIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_purposecatagory"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%-- <asp:DropDownList ID="ddl_group" runat="server" CssClass="textbox  ddlheight1" Style="top: 47px;
                                    left: 870px; position: absolute;">
                                </asp:DropDownList>--%>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <asp:Label ID="lblerror" runat="server" Text="lblerrormsg" ForeColor="Red"></asp:Label>
                    <asp:Label ID="errorlable" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    <FarPoint:FpSpread ID="Fpspread1" Visible="false" runat="server" BorderColor="Black"
                        BorderStyle="Solid" BorderWidth="1px" OnCellClick="Cell_Click" Width="980px"
                        Height="330px" Style="border-radius: 10px; box-shadow: 0px 0px 8px #999999;"
                        HorizontalScrollBarPolicy="Always" VerticalScrollBarPolicy="Never" ShowHeaderSelection="false">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                            Width="180px" onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                            Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            CssClass="textbox btn1" />
                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" CssClass="textbox btn1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </div>
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
        </form>
    </body>
    </html>
</asp:Content>
