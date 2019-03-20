<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    CodeFile="HeaderColumnSettings.aspx.cs" Inherits="HeaderColumnSettings" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/PrintMaster.ascx" TagName="printmaster" TagPrefix="InsproPlus" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>
        <script type="text/javascript">
            function NumValid() {
                var NumVal = document.getElementById('<%=txtRows.ClientID %>').value;
                if (NumVal > 6) {
                    document.getElementById('<%=txtRows.ClientID %>').value = "";
                    alert("Please Enter the No of Rows less than or Equal to '6'");
                }
            }

            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="scriptmanager" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green">Header Column Settings - ModuleWise</span>
        </center>
        <br />
        <center>
            <div class="maindivstyle" style="width: 950px; height: auto;">
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            College Name
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updcoll" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtcoll" runat="server" ReadOnly="true" Text="--Select--" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    <asp:Panel ID="pnlcoll" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="chkcoll" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkcoll_change" />
                                        <asp:CheckBoxList ID="chklstcoll" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstcoll_change" />
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popcoll" runat="server" TargetControlID="txtcoll" PopupControlID="pnlcoll"
                                        Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            Module Name
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtMod" runat="server" ReadOnly="true" Text="--Select--" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Height="250px" Width="200px">
                                        <asp:CheckBox ID="cbMod" runat="server" Text="Select All" Checked="true" AutoPostBack="true"
                                            OnCheckedChanged="cbMod_change" />
                                        <asp:CheckBoxList ID="cblMod" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblMod_change">
                                            <asp:ListItem Selected="True" Text="Attendance" Value="0"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Black Box" Value="1"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="CAM" Value="2"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Chart" Value="3"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="COE" Value="4"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Feed Back" Value="5"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Finance" Value="6"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Hostel" Value="7"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="HR" Value="8"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Inventory" Value="9"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="I Patch" Value="10"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Library" Value="11"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Office" Value="12"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Question" Value="13"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Request" Value="14"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Schedule" Value="15"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="SMS" Value="16"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Student" Value="17"></asp:ListItem>
                                            <asp:ListItem Selected="True" Text="Transport" Value="18"></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtMod"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btngo" runat="server" Text="GO" CssClass="textbox textbox1 btn1"
                                OnClick="btngo_click" />
                            <asp:Button ID="btnaddnew" runat="server" Text="Add New" CssClass="textbox textbox1 btn2"
                                OnClick="btnaddnew_click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lbl_Error" runat="server" Text="" Visible="false" ForeColor="Red"
                    Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                <br />
                <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" Style="height: auto;"
                    ShowHeaderSelection="false" CssClass="spreadborder" OnCellClick="FpSpread1_Click"
                    OnPreRender="FpSpread1_Render" ActiveSheetViewIndex="0">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" MaxLength="15" CssClass="textbox textbox1 txtheight4"
                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="filt_extenderexcel" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        OnClick="btnExcel_Click" Font-Size="Medium" CssClass="textbox textbox1 btn2"
                        Width="140px" Text="Export To Excel" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" CssClass="textbox textbox1 btn2"
                        Width="100px" />
                    <InsproPlus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
                <br />
                <center>
                    <div id="popper1" runat="server" visible="false" style="height: 70em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                            width: 30px; position: absolute; margin-top: 18px; margin-left: 428px;" OnClick="imagebtnpopclose1_Click" />
                        <br />
                        <center>
                            <div style="height: 600px; width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                                border-radius: 10px; background-color: White;">
                                <br />
                                <div>
                                    <center>
                                        <span style="color: Green; font-size: large;">Header Column Settings</span>
                                    </center>
                                </div>
                                <br />
                                <div>
                                    <center>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    College Name
                                                </td>
                                                <td>
                                                    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>--%>
                                                    <asp:TextBox ID="txtCollPop" runat="server" ReadOnly="true" Text="--Select--" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                    <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel">
                                                        <asp:CheckBox ID="chkcollPop" runat="server" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="chkcollPop_change" />
                                                        <asp:CheckBoxList ID="chklstcollPop" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstcollPop_change" />
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtCollPop"
                                                        PopupControlID="Panel2" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                    <%-- </ContentTemplate>
                                                </asp:UpdatePanel>--%>
                                                </td>
                                                <td>
                                                    Module Name
                                                </td>
                                                <td>
                                                    <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>--%>
                                                    <asp:TextBox ID="txtModPop" runat="server" ReadOnly="true" Text="--Select--" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="250px" Width="200px">
                                                        <asp:CheckBox ID="cbModPop" runat="server" Text="Select All" Checked="true" AutoPostBack="true"
                                                            OnCheckedChanged="cbModPop_change" />
                                                        <asp:CheckBoxList ID="cblModPop" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblModPop_change">
                                                            <asp:ListItem Selected="True" Text="Attendance" Value="0"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Black Box" Value="1"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="CAM" Value="2"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Chart" Value="3"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="COE" Value="4"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Feed Back" Value="5"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Finance" Value="6"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Hostel" Value="7"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="HR" Value="8"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Inventory" Value="9"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="I Patch" Value="10"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Library" Value="11"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Office" Value="12"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Question" Value="13"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Request" Value="14"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Schedule" Value="15"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="SMS" Value="16"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Student" Value="17"></asp:ListItem>
                                                            <asp:ListItem Selected="True" Text="Transport" Value="18"></asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtModPop"
                                                        PopupControlID="Panel3" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                    <%-- </ContentTemplate>
                                                </asp:UpdatePanel>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    No.of Rows
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRows" runat="server" CssClass="textbox textbox1 txtheight" onkeyup="return NumValid();"
                                                        Width="50px" MaxLength="1"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                                        TargetControlID="txtRows">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:Button ID="btngoPop" runat="server" Text="GO" CssClass="textbox textbox1 btn1"
                                                        OnClick="btngoPop_click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:Label ID="lbl_PopError" runat="server" Text="" Visible="false" ForeColor="Red"
                                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        <br />
                                        <br />
                                        <%--<asp:UpdatePanel ID="UpdSpr" runat="server">
                                        <ContentTemplate>--%>
                                        <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" ShowHeaderSelection="false"
                                            CssClass="spreadborder" Height="300px" ActiveSheetViewIndex="0">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                        <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                        <br />
                                        <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>--%>
                                        <div id="myDiv" style="margin-left: 400px; margin-top: -16px;">
                                            <asp:CheckBox ID="chkleft_Logo" runat="server" Text="Left Logo" />
                                            <asp:CheckBox ID="chkright_Logo" runat="server" Text="Right Logo" />
                                            <asp:Button ID="btnAddRows" runat="server" Text="Add Rows" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1 btn2" OnClick="btnAddRows_Click" />
                                            <asp:Button ID="btnRemRows" runat="server" Text="Remove" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1 btn2" OnClick="btnRemRows_Click" />
                                        </div>
                                        <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                        <br />
                                        <center>
                                            <%--<asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>--%>
                                            <asp:Button ID="btnupdate" runat="server" Text="Update" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Font-Bold="true" CssClass="textbox btn2 textbox1" OnClick="btnupdate_Click"
                                                Visible="false" />
                                            <asp:Button ID="btndelete" runat="server" Text="Delete" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Font-Bold="true" CssClass="textbox btn2 textbox1" OnClick="btndelete_Click"
                                                Visible="false" />
                                            <asp:Button ID="btnsave" runat="server" Text="Save" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" OnClick="btnsave_Click" Visible="false" CssClass="textbox btn2 textbox1" />
                                            <asp:Button ID="btnexit" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" Text="Exit" Visible="false" CssClass="textbox btn2 textbox1"
                                                OnClick="btnexit_Click" />
                                            <%--</ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                        </center>
                                    </center>
                                </div>
                            </div>
                        </center>
                    </div>
                </center>
                <center>
                    <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: auto;
                                width: 400px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnerrclose" CssClass=" textbox textbox1 btn1 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btnerrclose_Click" Text="Ok" runat="server" />
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
        </center>
    </body>
    </html>
</asp:Content>
