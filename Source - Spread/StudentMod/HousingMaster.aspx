<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="HousingMaster.aspx.cs" Inherits="StudentMod_HousingMaster" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .div
            {
                left: 0%;
                top: 0%;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">
            function checkDecimal(el) {
                var ex = /^((\d{9})*|([1-9]\d{0,9}))(\.\d{0,2})?$/;
                if (ex.test(el.value) == false) {
                    el.value = '';
                    oTextBox.value = oTextBox.value[0].toUpperCase() + oTextBox.value.substring(1);
                }
            }
            function Test() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";
                id = document.getElementById("<%=txt_priority.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_priority.ClientID %>");
                    id.style.borderColor = 'Red';
                    document.getElementById('<%=lblerrpriority.ClientID %>').innerHTML = "Enter the Priority";
                    empty = "E";
                    id = document.getElementById("<%=ddl_housename1.ClientID %>");
                    value1 = id.options[id.selectedIndex].text;
                    if (value1.trim().toUpperCase() == "SELECT") {
                        id.style.borderColor = 'Red';
                        document.getElementById('<%=lblerrddlhouse.ClientID %>').innerHTML = "Select House Name";
                        empty = "E";
                    }
                    return false;
                }
            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
                document.getElementById('<%=lblerrddlhouse.ClientID %>').innerHTML = "";
            }
            function myFunctionP(y) {
                y.style.borderColor = "#c4c4c4";
                document.getElementById('<%=lblerrpriority.ClientID %>').innerHTML = "";
            }
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <asp:Label ID="Label1" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="House Master"></asp:Label>
                    </div>
                </center>
                <br />
                <div class="maindivstyle" style="height: 500px; width: 1000px;">
                    <br />
                    <table class="maintablestyle" style="width: 740px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblCollege" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upCollege" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtCollege" runat="server" CssClass="textbox txtheight2" Width="100px"
                                            ReadOnly="true" placeholder="College" onfocus="return myFunction1(this)"></asp:TextBox>
                                        <asp:Panel ID="pnlCollege" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_College" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_College_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_College" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_College_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceCollege" runat="server" TargetControlID="txtCollege"
                                            PopupControlID="pnlCollege" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_House" runat="server" Text="House Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpHouse" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtmainhouse" runat="server" CssClass="textbox txtheight2" Width="100px"
                                            ReadOnly="true" placeholder="College" onfocus="return myFunction1(this)"></asp:TextBox>
                                        <asp:Panel ID="Panelhouse" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_mainhouse" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_mainhouse_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_mainhouse" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_mainhouse_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtmainhouse"
                                            PopupControlID="Panelhouse" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <%--<td>
                                <asp:DropDownList ID="ddl_housename" runat="server" CssClass="textbox  ddlheight3"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="0">House Name</asp:ListItem>
                                </asp:DropDownList>
                            </td>--%>
                            
                            <td>
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox btn2"
                                    OnClick="btn_addnew_Click" />
                            </td>
                            <td>
                            <asp:LinkButton ID="lb_housepr" runat="server" Text="House Priority" OnClick="lb_housepr_click"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Large"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    <div id="div1" runat="server" visible="false" style="width: 767px; height: 300px;
                        overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                        box-shadow: 0px 0px 8px #999999;">
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Width="750px" Style="height: 350px; overflow: auto; background-color: White;"
                            OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <br />
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1" onkeypress="display()"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1"
                            Text="Export To Excel" Width="150px" Height="30px"/>
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Width="70px" Height="30px" CssClass="textbox textbox1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="divexit" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 23px; margin-left: 341px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 700px;
                    height: 400px;" align="center">
                    <br />
                    <center>
                        <asp:Label ID="lbl_housemaster" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="House Master"></asp:Label>
                    </center>
                    <br />
                    <div align="left" style="overflow: auto; width: 500px; height: 250px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlheight5" Width="250px"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_housename1" runat="server" Text="House Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Button ID="btn_plus_housename" runat="server" Text="+" CssClass="textbox btn"
                                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_plus_housename_Click" />
                                            
                                        <asp:DropDownList ID="ddl_housename1" CssClass="textbox ddlheight3" runat="server"
                                            AutoPostBack="true" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_minus_housename" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_minus_housename_Click" />
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblerrddlhouse" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                        <asp:Label ID="lbl_houseacronym" runat="server" Text="HouseAcronym"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txthouseacronym" onfocus="return myFunction(this)" CssClass="textbox   txtheight2" Width="40px" runat="server" MaxLength="5"></asp:TextBox>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_gender" runat="server" Text="Gender"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:RadioButton ID="rdb_gendermale" Text="Male" AutoPostBack="true" runat="server"
                                            GroupName="same" OnCheckedChanged="rdb_genderOnchange" />
                                        <asp:RadioButton ID="rdb_genderfemale" Text="Female" AutoPostBack="true" runat="server"
                                            GroupName="same" OnCheckedChanged="rdb_genderOnchange" />
                                        <asp:RadioButton ID="rdb_genderboth" Text="Both" AutoPostBack="true" runat="server"
                                            GroupName="same" OnCheckedChanged="rdb_genderOnchange" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_priority" runat="server" Text="Priority"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_priority" onfocus="return myFunctionP(this)" CssClass="textbox   txtheight2" Width="40px" MaxLength="2" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_priority"
                                            FilterType="Numbers" >
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblerrpriority" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="orderbyTR" visible="false">
                                    <td>
                                        <asp:Label ID="lbl_orderby" runat="server" Text="Order By"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:RadioButton ID="rdb_orderbymale1" Text="Male" AutoPostBack="true" runat="server"
                                            GroupName="same1" />
                                        <asp:RadioButton ID="rdb_orderbyfemale1" Text="Female" AutoPostBack="true" runat="server"
                                            GroupName="same1" />
                                        <asp:RadioButton ID="rdb_orderbyboth1" Text="Both" AutoPostBack="true" runat="server"
                                            GroupName="same1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                        <center>
                                            <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                                                Visible="false" OnClick="btn_delete_Click" />
                                            <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" Text="Save" OnClientClick="return Test()"
                                                OnClick="btn_save_Click" />
                                            <asp:Button ID="btn_exit" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit_Click" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </div>
            </div>
            <div id="pophouse" runat="server" visible="false" style="height: 50em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="imghouse" runat="server" ImageUrl="../images/close.png" Style="height: 30px;
                            width: 30px; position: absolute; margin-top: 98px; margin-left: 353px;" OnClick="imghouse_Click" />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <div style="background-color: White; height: 644px; width: 750px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <center>
                                <asp:Label ID="lblpopheader" runat="server" Text="House Priority" Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
                            </center>
                            <br />
                            <table id="Table1" class="maintablestyle" runat="server">
                                <tr>
                                    <td>
                                        College
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcolhouse" runat="server" Width="250px" CssClass="textbox1 ddlheight4"
                                            OnSelectedIndexChanged="ddlcolhouse_Change" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        House
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updhouse" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtlinkhousename" runat="server" Height="15px" CssClass="textbox textbox1 txtheight3"
                                                    ReadOnly="true"></asp:TextBox>
                                                <asp:Panel ID="phouse" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                                    <asp:CheckBox ID="cb_linkhousename" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_linkhousename_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_linkhousename" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_linkhousename_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="phousepop" runat="server" TargetControlID="txtlinkhousename"
                                                    PopupControlID="phouse" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                
                                    <td >
                                        <asp:Button ID="btnpophousego" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                                            OnClick="btnpophousego_click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <center>
                                <div id="divhouse" runat="server" visible="true" style="width: 700px; height: 375px;
                                    overflow: auto; border: 1px solid Gray; background-color: White; border-radius: 10px;">
                                    <br />
                                    <FarPoint:FpSpread ID="Fpspreadpophouse" runat="server" Visible="true" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="1px" Width="650px" Height="300px" OnCellClick="Cellpophouse_Click" OnPreRender="Fpspreadpophouse_render" OnButtonCommand="Fpspreadpophouse_buttoncommand"
                                        CssClass="spreadborder" ShowHeaderSelection="false">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </center>
                            <br />
                            <center>
                                <div id="DptPriorityDiv" runat="server" visible="false">
                                    <asp:Button ID="btnsethousepriority" runat="server" Text="Set Priority" CssClass="textbox textbox1 btn2"
                                        OnClick="btnsethousepriority_click" />
                                    <asp:Button ID="btnresethousepriority" runat="server" Text="Reset" CssClass="textbox textbox1 btn2"
                                        OnClick="btnresethousepriority_click" />
                                    <asp:Button ID="btnexithouse" runat="server" Text="Exit" CssClass="textbox textbox1 btn2"
                                        OnClick="btnexithouse_click" />
                                </div>
                            </center>
                        </div>
                    </div>
        </center>
        <center>
            <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="panel_reason" runat="server" visible="false" class="table" style="background-color: White;
                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_add" runat="server" Text="House Name" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txt_housename2" runat="server" Width="200px" Style="text-transform:capitalize; font-family: 'Book Antiqua'; margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" MaxLength="35"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderhouse" runat="server" TargetControlID="txt_housename2" FilterType="LowercaseLetters,UppercaseLetters,Numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <br />
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_add_housename" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_add_housename_Click" />
                                    <asp:Button ID="btn_exit_housename" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_exit_housename_Click" />
                                </td>
                            </tr>
                        </table>
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
                                            <asp:Button ID="btn_errorclose" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                OnClick="btn_errorclose_Click" Text="ok" runat="server" />
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
        </form>
    </body>
    </html>
</asp:Content>
