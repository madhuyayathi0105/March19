<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HT_Return_item.aspx.cs" Inherits="HT_Return_item" %>

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
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }   
        </script>
        <form id="form1">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div>
                <center>
                    <div>
                        <br />
                        <span class="fontstyleheader" style="color: Green;">Daily Consumption-Item Return Entry</span></div>
                    <br />
                </center>
            </div>
            <center>
                <div class="maindivstyle" style="width: 1000px; height: 580px;">
                    <br />
                    <center>
                        <table style="border: 1px solid #0CA6CA; border-radius: 10px; margin-left: 54px;
                            background-color: #0CA6CA; position: absolute; width: 887px; height: 81px; box-shadow: 0px 0px 8px #7bc1f7;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblhostelname" Text="Mess Name" Style="top: 15px; left: 10px; position: absolute;"
                                        runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlhostelname" runat="server" Style="top: 10px; left: 100px;
                                        position: absolute;" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddlhostelname_change"
                                        CssClass="textbox1 ddlstyle ddlheight2">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldate" Text="Date" Style="top: 15px; left: 267px; position: absolute;"
                                        runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdate" runat="server" Width="100px" Style="top: 10px; left: 304px;
                                        position: absolute;" AutoPostBack="true" OnTextChanged="txtdate_Change" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="caldate" TargetControlID="txtdate" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblsessionname" runat="server" Style="top: 15px; left: 418px; position: absolute;"
                                        Text="Session Name"></asp:Label>
                                </td>
                                <td>
                                    <%--  <asp:UpdatePanel ID="Upp1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtsessionname" runat="server" ReadOnly="true" CssClass="textbox textbox1"
                                            Width="135px" Height="20px" Style="top: 10px; left: 516px; position: absolute;">--Select--</asp:TextBox>
                                        <asp:Panel ID="p1" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="chksessionname" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="chksession_checkedchange" />
                                            <asp:CheckBoxList ID="chklstsession" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstsession_Change ">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsessionname"
                                            PopupControlID="p1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                    <asp:DropDownList ID="ddlsession" runat="server" Style="top: 10px; left: 516px; position: absolute;"
                                        Width="130px" AutoPostBack="true" OnSelectedIndexChanged="ddlsession_change"
                                        CssClass="textbox1 ddlstyle ddlheight2">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_menutype" runat="server" Text="Menu Type" Style="top: 15px; left: 672px;
                                        position: absolute;"></asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanelnew" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_menutype" runat="server" CssClass="textbox textbox1" Width="115px"
                                                Height="20px" Style="top: 11px; left: 748px; position: absolute;" ReadOnly="true">--Select--</asp:TextBox>
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
                                    <asp:Label ID="lblmenuname" runat="server" Style="top: 46px; left: 11px; position: absolute;"
                                        Text="Menu Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtmenuname" runat="server" ReadOnly="true" CssClass="textbox textbox1"
                                                Width="149px" Height="20px" Style="top: 44px; left: 100px; position: absolute;">--Select--</asp:TextBox>
                                            <asp:Panel ID="p2" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chkmenuname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkmenuname_Change" />
                                                <asp:CheckBoxList ID="chk_lstmenuname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chk_lstmenuname_Change">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Pop2" runat="server" TargetControlID="txtmenuname"
                                                PopupControlID="p2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:DropDownList ID="ddlmenuname" Visible="false" runat="server" Style="top: 10px;
                                        left: 748px; position: absolute;" Width="130px" CssClass="textbox1 ddlstyle">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btngo" Text="Go" runat="server" Style="top: 45px; left: 600px; position: absolute;"
                                        CssClass="textbox btn1" OnClick="btngo_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdomenuitemcon" runat="server" Text="Menu Item Return" AutoPostBack="true"
                                        Style="top: 46px; left: 264px; position: absolute;" GroupName="con" OnCheckedChanged="rdomenuitemcon_check" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdocleanitem" runat="server" Text="Cleaning Item Return" AutoPostBack="true"
                                        Style="top: 46px; left: 406px; position: absolute;" GroupName="con" OnCheckedChanged="rdocleanitem_check" />
                                </td>
                                <td>
                                    <asp:Button ID="btnaddnew" Visible="false" Text="Add New" Style="top: 45px; left: 573px;
                                        position: absolute;" runat="server" CssClass="textbox btn1" OnClick="btnaddnew_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <center>
                            <div>
                                <%--style="text-align: left; text-indent: 50px; font-size: medium;"--%>
                                <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="Red" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </div>
                        </center>
                        <br />
                        <%-- <div id="spreaddiv1" runat="server" visible="false" style="width: 824px; height: 372px;"
                        class="spreadborder">--%>
                        <br />
                        <FarPoint:FpSpread ID="FpSpread1" Visible="false" runat="server" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" Style="width: 750px; height: 350px; overflow: auto;
                            background-color: White; border-radius: 10px; box-shadow: 0px 0px 8px #999999;"
                            OnUpdateCommand="FpSpread1_Command" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <%--   </div>--%>
                        <br />
                        <%-- </center>--%>
                        <%-- </center>--%>
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1" Height="20px"
                                Width="180px" onkeypress="display()"></asp:TextBox>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                                CssClass="textbox btn1" Width="127px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                CssClass="textbox btn1" Width="60px" />
                            <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="textbox btn1" Width="60px"
                                OnClick="btnsave_Click" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                </div>
            </center>
            <center>
                <div id="popwindow" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <br />
                    <br />
                    <br />
                    <br />
                    <div style="background-color: White; height: 300px; width: 600px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <div>
                            <center>
                                <asp:Label ID="lbldailyconsum" runat="server" Style="font-size: large; color: Green;"
                                    Text="Daily Consumsion"></asp:Label>
                            </center>
                        </div>
                        <br />
                        <table style="line-height: 35px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblpopitemname" runat="server" Text="Item Name"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtpopitemname" runat="server" CssClass="textbox textbox1" Width="460px"
                                        onfocus="return myFunction(this)"></asp:TextBox>
                                    <span style="color: red">*</span>
                                    <asp:FilteredTextBoxExtender ID="filtertextbox1" runat="server" TargetControlID="txtpopitemname"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblpopitemcode" runat="server" Text="Item Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpopitemcode" runat="server" Width="70px" CssClass="textbox textbox1"
                                        onfocus="return myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filtertextbox2" runat="server" TargetControlID="txtpopitemcode"
                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: red">*</span>
                                    <asp:Button ID="btnpop" runat="server" Text="?" CssClass="textbox btn" OnClick="btnpop_Click" />
                                </td>
                                <td>
                                    <asp:Label ID="lblpopstockQty" runat="server" Text="Stack Qty"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpopstockqty" runat="server" CssClass="textbox textbox1" onfocus="return myFunction(this)"></asp:TextBox>
                                    <span style="color: red">*</span>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtpopstockqty"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblpoprpu" runat="server" Text="RPU"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpoprpu" runat="server" CssClass="textbox textbox1" onfocus="return myFunction(this)"></asp:TextBox>
                                    <span style="color: red">*</span>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtpoprpu"
                                        FilterType="Numbers,custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblpopconqty" runat="server" Text="Conception Qty"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpopconqty" runat="server" CssClass="textbox textbox1" onfocus="return myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtpopconqty"
                                        FilterType="Numbers,custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                        </table>
                        <center>
                            <br />
                            <div>
                                <asp:Button ID="btnpopadd" runat="server" Text="Add" CssClass="textbox btn1" OnClick="btnpopadd_Click"
                                    OnClientClick="return Test()" />
                                <asp:Button ID="btnpopexit" runat="server" Text="Exit" CssClass="textbox btn1" OnClick="btnpopexit_Click" />
                            </div>
                        </center>
                    </div>
                </div>
            </center>
            <center>
                <div id="popwindow1" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <br />
                    <br />
                    <br />
                    <br />
                    <div style="background-color: White; height: 500px; width: 700px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <div>
                            <center>
                                <asp:Label ID="header" runat="server" Style="font-size: large; color: Green;" Text="Select Item Code"></asp:Label>
                            </center>
                        </div>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblpop1itemcode" runat="server" Text="Item Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpop1itemcode" runat="server" Width="70px" CssClass="textbox textbox1"
                                        onfocus="return myFunction1(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filtertextbox3" runat="server" TargetControlID="txtpop1itemcode"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: red">*</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblpop1itemname" runat="server" Text="Item Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpop1itemname" runat="server" CssClass="textbox textbox1" Width="400px"
                                        onfocus="return myFunction1(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtpop1itemname"
                                        FilterType="LowercaseLetters,UppercaseLetters,custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                        </table>
                        <center>
                            <br />
                            <div>
                                <asp:Button ID="btnpop1Save" runat="server" Text="Save" CssClass="textbox btn1" OnClick="btnpop1add_Click"
                                    OnClientClick="return test1()" />
                                <asp:Button ID="btnpop1exit" runat="server" Text="Exit" CssClass="textbox btn1" OnClick="btnpop1exit_Click" />
                            </div>
                        </center>
                    </div>
                    <center>
                    </center>
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
                                                <%-- <asp:ImageButton ID="btn_errorclose" Style="height: 40px; width: 40px;" OnClick="btn_errorclose_Click"
                                                ImageUrl="~/images/okimg.jpg" runat="server" />--%>
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
        </div>
        </form>
    </body>
    </html>
</asp:Content>
