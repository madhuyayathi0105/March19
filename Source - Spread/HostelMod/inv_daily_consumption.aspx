<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="inv_daily_consumption.aspx.cs" Inherits="inv_daily_consumption" %>

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
        <script type="text/javascript">
            function Test() {
                var itemname = document.getElementById("<%=txt_itemname1.ClientID %>").value;
                var itemcode = document.getElementById("<%=txt_itemcode1.ClientID %>").value;
                var rpu = document.getElementById("<%=txt_rpu1.ClientID %>").value;
                var stockgty = document.getElementById("<%=txt_stockqty1.ClientID %>").value;
                var consumsionqty = document.getElementById("<%=txt_conqty1.ClientID %>").value;
                var empty = "";

                if (itemname.trim() == "") {
                    itemname = document.getElementById("<%=txt_itemname1.ClientID %>");
                    itemname.style.borderColor = 'Red';
                    empty = "E";

                }

                if (itemcode.trim() == "") {
                    itemcode = document.getElementById("<%=txt_itemcode1.ClientID %>");
                    itemcode.style.borderColor = 'Red';
                    empty = "E";

                }

                if (stockgty.trim() == "") {
                    stockgty = document.getElementById("<%=txt_stockqty1.ClientID %>");
                    stockgty.style.borderColor = 'Red';
                    empty = "E";

                }

                if (rpu.trim() == "") {
                    rpu = document.getElementById("<%=txt_rpu1.ClientID %>");
                    rpu.style.borderColor = 'Red';
                    empty = "E";
                }

                if (consumsionqty.trim() == "") {
                    consumsionqty = document.getElementById("<%=txt_conqty1.ClientID %>");
                    consumsionqty.style.borderColor = 'Red';
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
            function test1() {
                var pop1itemcode = document.getElementById("<%=txt_itemcode2.ClientID %>").value;
                var pop1itemname = document.getElementById("<%=txt_itemname2.ClientID %>").value;
                var empty = "";
                if (pop1itemcode.trim() == "") {
                    pop1itemcode = document.getElementById("<%=txt_itemcode2.ClientID %>");
                    pop1itemcode.style.borderColor = 'Red';
                    empty = "E";

                }

                if (pop1itemname.trim() == "") {
                    pop1itemname = document.getElementById("<%=txt_itemname2.ClientID %>");
                    pop1itemname.style.borderColor = 'Red';
                    empty = "E";

                }
                if (empty.trim() != "") {

                    return false;
                }
                else {
                    return true;
                }
            }
            function myFunction1(y) {
                y.style.borderColor = "#c4c4c4";
            }


            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

            function displayDir() {
                document.getElementById('<%=lblDirNorec.ClientID %>').innerHTML = "";
            }
   
        </script>
        <form id="form1">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Daily Consumption Status</span></div>
                    <br />
                </center>
            </div>
            <center>
                <div class="maindivstyle" style="width: 1000px; height: 600px; z-index: 100;">
                    <br />
                    <center>
                        <table style="border: 1px solid #0CA6CA; border-radius: 10px; margin-left: 34px;
                            background-color: #0CA6CA; position: absolute; width: 940px; height: 111px; box-shadow: 0px 0px 8px #7bc1f7;"
                            id="maintable" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_hostelname" Text="Mess Name" Style="top: 15px; left: 10px; position: absolute;"
                                        runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_hostelname" runat="server" Style="top: 10px; left: 100px;
                                        position: absolute;" Width="145px" AutoPostBack="true" Height="29px" OnSelectedIndexChanged="ddl_hostelname_SelectedIndexChange"
                                        CssClass="textbox1">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_date" Text="Date" Style="top: 15px; left: 267px; position: absolute;"
                                        runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_date" runat="server" Width="80px" Style="top: 10px; left: 304px;
                                        position: absolute; height: 20px;" AutoPostBack="true" OnTextChanged="txt_date_Change"
                                        CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="caldate" TargetControlID="txt_date" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_sessionname" runat="server" Style="top: 15px; left: 418px; position: absolute;"
                                        Text="Session Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sessionname" runat="server" CssClass="textbox textbox1" Width="135px"
                                                Height="20px" Style="top: 10px; left: 516px; position: absolute;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p1" runat="server" CssClass="multxtpanel" Width="150px" Height="200px">
                                                <asp:CheckBox ID="cb_sessionname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_session_CheckedChange" />
                                                <asp:CheckBoxList ID="cbl_session" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_session_SelectedIndexChange ">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_sessionname"
                                                PopupControlID="p1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:DropDownList ID="ddl_session" runat="server" Visible="false" Style="top: 10px;
                                        left: 516px; position: absolute;" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="ddl_session_SelectedIndexChange"
                                        CssClass="textbox ddlstyle">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbl_menutype" runat="server" Text="Menu Type" Style="top: 16px; left: 678px;
                                        position: absolute;"></asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanelnew" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_menutype" runat="server" CssClass="textbox textbox1" Width="115px"
                                                Height="20px" Style="top: 10px; left: 758px; position: absolute;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Width="120px" Height="100px">
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
                                    <asp:Label ID="lbl_menuname" runat="server" Style="top: 49px; left: 10px; position: absolute;"
                                        Text="Menu Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_menuname" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                Width="135px" Height="20px" Style="top: 45px; left: 100px; position: absolute;">--Select--</asp:TextBox>
                                            <asp:Panel ID="p2" runat="server" CssClass="multxtpanel" Width="200px" Height="250px">
                                                <asp:CheckBox ID="cb_menuname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_menuname_CheckedChange" />
                                                <asp:CheckBoxList ID="cbl_menuname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_menuname_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Pop2" runat="server" TargetControlID="txt_menuname"
                                                PopupControlID="p2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:DropDownList ID="ddl_menuname" Visible="false" runat="server" Style="top: 10px;
                                        left: 748px; position: absolute;" Width="130px" CssClass="textbox ddlstyle">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" Text="Go" runat="server" Style="top: 45px; left: 900px; position: absolute;"
                                        CssClass="textbox btn" OnClick="btn_go_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdb_menuitemcon" runat="server" Text="Menu Item Consumption"
                                        AutoPostBack="true" OnCheckedChanged="rdb_menuitemcon_CheckedChange" Style="top: 49px;
                                        left: 260px; position: absolute;" GroupName="con" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_cleanitem" runat="server" Text="Cleaning & Other Item Consumption"
                                        AutoPostBack="true" Style="top: 49px; left: 450px; position: absolute;" GroupName="con"
                                        OnCheckedChanged="rdb_cleanitem_check" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_Additionalitem" runat="server" Enabled="false" Text="Add Additional Items"
                                        AutoPostBack="true" Style="top: 49px; left: 728px; position: absolute;" OnCheckedChanged="cb_Additionalitem_check" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_addnew" Visible="false" Text="Add New" Style="top: 10px; left: 904px;
                                        position: absolute;" runat="server" CssClass="textbox btn1" OnClick="btn_addnew_Click" />
                                </td>
                            </tr>
                            <tr id="purposecatagoryTR" runat="server" visible="false">
                                <td>
                                    <asp:Label ID="lbl_menuPurposeCatagory" runat="server" Text="Purpose Category" Style="top: 80px;
                                        left: 10px; position: absolute;"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_purposeCategory" runat="server" CssClass="textbox  ddlheight3"
                                        Style="top: 78px; left: 138px; position: absolute;">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <br />
                        <br />
                        <table style="margin-right: 750px; margin-top: 5px; background-color: LightGreen;"
                            id="directDailyConsumption" runat="server">
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkdirconsume" runat="server" Text="Direct Daily Consumption"
                                        OnClick="lnkdirconsume_onclick"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div>
                            <asp:Label ID="labltotallable" runat="server" Visible="false" ForeColor="Green"></asp:Label>
                            <asp:Label ID="lbl_error1" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        </div>
                        <br />
                        <div>
                            <FarPoint:FpSpread ID="FpSpread1" Visible="false" runat="server" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="1px" Style="height: 300px; overflow: auto; background-color: White;
                                border-radius: 10px; box-shadow: 0px 0px 8px #999999;" OnUpdateCommand="FpSpread1_Command"
                                ShowHeaderSelection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                    </center>
                    <center>
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                                Width="180px" onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                CssClass="textbox btn1" />
                            <asp:Button ID="btn_save" runat="server" Text="Save" OnClick="btn_save_Click" CssClass="textbox btn2" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                        <br />
                    </center>
                </div>
            </center>
            <center>
                <div id="popwindow" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 70px; margin-left: 254px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div style="background-color: White; height: 300px; width: 600px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <div>
                            <center>
                                <asp:Label ID="lbl_dailyconsum" runat="server" Style="font-size: large; color: Green;"
                                    Text="Daily Consumsion"></asp:Label>
                            </center>
                        </div>
                        <br />
                        <table style="line-height: 35px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_itemname1" runat="server" Text="Item Name"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_itemname1" runat="server" CssClass="textbox textbox1" Width="460px"
                                        onfocus="return myFunction(this)"></asp:TextBox>
                                    <span style="color: red">*</span>
                                    <asp:FilteredTextBoxExtender ID="filtertextbox1" runat="server" TargetControlID="txt_itemname1"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_itemcode1" runat="server" Text="Item Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_itemcode1" runat="server" Width="70px" CssClass="textbox textbox1"
                                        onfocus="return myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filtertextbox2" runat="server" TargetControlID="txt_itemcode1"
                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: red">*</span>
                                    <asp:Button ID="btn_qmark" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_qmark_Click" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_stockQty1" runat="server" Text="Stack Qty"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_stockqty1" runat="server" CssClass="textbox textbox1" onfocus="return myFunction(this)"></asp:TextBox>
                                    <span style="color: red">*</span>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_stockqty1"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_rpu" runat="server" Text="RPU"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_rpu1" runat="server" CssClass="textbox textbox1" onfocus="return myFunction(this)"></asp:TextBox>
                                    <span style="color: red">*</span>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_rpu1"
                                        FilterType="Numbers,custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_conqty1" runat="server" Text="Conception Qty"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_conqty1" runat="server" CssClass="textbox textbox1" onfocus="return myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_conqty1"
                                        FilterType="Numbers,custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                        </table>
                        <center>
                            <br />
                            <div>
                                <asp:Button ID="btn_add1" runat="server" Text="Add" CssClass="textbox btn1" OnClick="btn_add1_Click"
                                    OnClientClick="return Test()" />
                                <asp:Button ID="btn_exit1" runat="server" Text="Exit" CssClass="textbox btn1" OnClick="btn_exit1_Click" />
                            </div>
                        </center>
                    </div>
                </div>
            </center>
            <center>
                <div>
                    <div id="popwindow1" runat="server" visible="false" style="height: 50em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="imgbtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 70px; margin-left: 254px;"
                            OnClick="imgbtnclose1_Click" />
                        <br />
                        <br />
                        <br />
                        <br />
                        <div style="background-color: White; height: 500px; width: 700px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <div>
                                <center>
                                    <asp:Label ID="lbl_header1" runat="server" Style="font-size: large; color: Green;"
                                        Text="Select Item Code"></asp:Label>
                                </center>
                            </div>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_itemcode2" runat="server" Text="Item Code"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_itemcode2" runat="server" Width="70px" CssClass="textbox textbox1"
                                            onfocus="return myFunction1(this)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filtertextbox3" runat="server" TargetControlID="txt_itemcode2"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: red">*</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_itemname2" runat="server" Text="Item Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_itemname2" runat="server" CssClass="textbox textbox1" Width="400px"
                                            onfocus="return myFunction1(this)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_itemname2"
                                            FilterType="LowercaseLetters,UppercaseLetters,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: red">*</span>
                                    </td>
                                </tr>
                            </table>
                            <center>
                                <br />
                                <div>
                                    <asp:Button ID="btn_save2" runat="server" Text="Save" CssClass="textbox btn1" OnClick="btn_save2_Click"
                                        OnClientClick="return test1()" />
                                    <asp:Button ID="btn_exit2" runat="server" Text="Exit" CssClass="textbox btn1" OnClick="btn_exit2_Click" />
                                </div>
                            </center>
                        </div>
                        <div>
                        </div>
                    </div>
                </div>
            </center>
            <center>
                <div id="dirDiv" runat="server" visible="false" style="height: 50em; z-index: 200;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 86px; margin-left: 429px;"
                        OnClick="imagebtndirDivclose_Click" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div style="background-color: White; height: 600px; width: 900px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <div>
                            <center>
                                <asp:Label ID="Label1" runat="server" Style="font-size: large; color: Green;" Text="Direct Daily Consumsion"></asp:Label>
                            </center>
                        </div>
                        <br />
                        <table style="border: 1px solid #0CA6CA; border-radius: 10px; background-color: #0CA6CA;
                            height: 80px; box-shadow: 0px 0px 8px #7bc1f7;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblDirMessName" Text="Mess Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDirMessName" runat="server" Width="160px" AutoPostBack="true"
                                        Height="29px" OnSelectedIndexChanged="ddlDirMessName_SelectedIndexChange" CssClass="textbox1">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblDirDt" Text="Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDirDt" runat="server" Width="100px" Style="height: 20px;" AutoPostBack="true"
                                        OnTextChanged="txtDirDt_Change" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtDirDt" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblDirSession" runat="server" Text="Session Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtDirSes" runat="server" CssClass="textbox textbox1" Width="135px"
                                                Height="20px" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Width="150px" Height="200px">
                                                <asp:CheckBox ID="cbDirSes" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbDirSes_CheckedChange" />
                                                <asp:CheckBoxList ID="cblDirSes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblDirSes_SelectedIndexChange ">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtDirSes"
                                                PopupControlID="Panel2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDirMenu" runat="server" Text="Menu Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtDirMenu" runat="server" CssClass="textbox textbox1" Width="115px"
                                                Height="20px" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Width="120px" Height="100px">
                                                <asp:CheckBox ID="cbDirMenu" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbDirMenu_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblDirMenu" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblDirMenu_SelectIndexChange">
                                                    <asp:ListItem Value="0">Veg</asp:ListItem>
                                                    <asp:ListItem Value="1">Non-Veg</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtDirMenu"
                                                PopupControlID="Panel3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblDirMenuName" runat="server" Text="Menu Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtDirMenuName" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                Width="135px" Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Width="200px" Height="250px">
                                                <asp:CheckBox ID="cbDirMenuName" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbDirMenuName_CheckedChange" />
                                                <asp:CheckBoxList ID="cblDirMenuName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblDirMenuName_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtDirMenuName"
                                                PopupControlID="Panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_dirmenuconssum" runat="server" Text="Menu Item" AutoPostBack="true"
                                        OnCheckedChanged="rdb_dirmenuconssum_CheckedChange" GroupName="dircon" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_dircleanconssum" runat="server" Text="Cleaning Item" AutoPostBack="true"
                                        GroupName="dircon" OnCheckedChanged="rdb_dircleanconssum_CheckedChange" />
                                </td>
                                <td>
                                    <asp:Button ID="btnDirGo" Text="Go" runat="server" CssClass="textbox btn" OnClick="btnDirGo_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lblDirErr1" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        <br />
                        <div>
                            <FarPoint:FpSpread ID="FpSpreadDirConsume" Visible="false" runat="server" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="1px" Style="height: 300px; overflow: auto; background-color: White;
                                border-radius: 10px; box-shadow: 0px 0px 8px #999999;" OnUpdateCommand="FpSpreadDirConsume_Command"
                                ShowHeaderSelection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <div id="divDirrPrint" runat="server" visible="false">
                            <asp:Label ID="lblDirNorec" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblDirRptName" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtDirExcel" CssClass="textbox textbox1" runat="server" Height="20px"
                                Width="180px" onkeypress="displayDir()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtDirExcel"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnDirExcel" runat="server" OnClick="btnDirExcel_Click" CssClass="textbox btn1"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnDirPrint" runat="server" Text="Print" OnClick="btnDirPrint_Click"
                                CssClass="textbox btn1" />
                            <asp:Button ID="btnDirSave" runat="server" Text="Save" OnClick="btnDirSave_Click"
                                CssClass="textbox btn2" />
                            <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
                        </div>
                    </div>
                </div>
            </center>
            <center>
                <div id="alertmessage" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alerterror" Visible="false" runat="server" Text="" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" CssClass=" textbox btn2 comm" Style="height: 28px;
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
                <center>
                    <div id="consum" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                                        OnClick="btn_sureyes_Click" Text="Yes" runat="server" />
                                                    <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                        OnClick="btn_sureno_Click" Text="No" runat="server" />
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
            <center>
                <center>
                    <div id="Div1" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblConsuption" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btn_consumption" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_consumption_Click" Text="Yes" runat="server" />
                                                    <asp:Button ID="btn_consumption_exit" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_consumption_exit_Click" Text="No" runat="server" />
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
        </form>
    </body>
    </html>
</asp:Content>
