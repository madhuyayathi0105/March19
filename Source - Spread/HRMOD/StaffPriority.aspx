<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="StaffPriority.aspx.cs" Inherits="StaffPriority" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
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
            function display() {
                document.getElementById('<%=lblsmserror.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Staff Priority</span></div>
                </center>
                <div id="Div1" class="maindivstyle" runat="server" style="width: 1000px; height: auto;">
                    <br />
                    <table id="Table1" class="maintablestyle" runat="server" style="width: 850px;">
                        <tr>
                            <td>
                                College
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcoll" runat="server" CssClass="textbox1 ddlheight3" Width="250px"
                                    OnSelectedIndexChanged="ddlcoll_Change" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Department
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updept" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtdept" runat="server" CssClass="textbox textbox1 txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnldept" runat="server" CssClass="multxtpanel" Height="200px">
                                            <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popdept" runat="server" TargetControlID="txtdept" PopupControlID="pnldept"
                                            Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Designation
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updesig" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtdesig" runat="server" CssClass="textbox textbox1 txtheight3"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnldesig" runat="server" CssClass="multxtpanel" Height="200px">
                                            <asp:CheckBox ID="cb_desig" runat="server" Text="Select All" OnCheckedChanged="cb_desig_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cbl_desig" runat="server" OnSelectedIndexChanged="cbl_desig_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdesig"
                                            PopupControlID="pnldesig" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Staff Type
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updstaff" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtstftype" runat="server" CssClass="textbox textbox1 txtheight3"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlstftype" runat="server" CssClass="multxtpanel" Height="100px">
                                            <asp:CheckBox ID="cb_stftype" runat="server" Text="Select All" OnCheckedChanged="cb_stftype_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cbl_stftype" runat="server" OnSelectedIndexChanged="cbl_stftype_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popstftype" runat="server" TargetControlID="txtstftype"
                                            PopupControlID="pnlstftype" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2">
                                <asp:CheckBox ID="chkpriority" runat="server" Text="Priority" OnCheckedChanged="chkpriority_change"
                                    AutoPostBack="true" />
                                <asp:Button ID="btngo" runat="server" Text="GO" CssClass="textbox textbox1 btn1"
                                    OnClick="btngo_click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <center>
                        <asp:Label ID="lblspreaderr" runat="server" Text="" Visible="false" Style="color;
                            red; font-size: large;"></asp:Label>
                    </center>
                    <br />
                    <div id="divspr" runat="server">
                        <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" Style="margin-left: 2px; width: 850px;
                            height: auto;" CssClass="spreadborder" ShowHeaderSelection="false" OnButtonCommand="Fpspread_buttoncommand"
                            OnPreRender="Fpspread_render">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <center>
                        <asp:Button ID="btnsetpriority" runat="server" Visible="false" Text="Set Priority"
                            CssClass="textbox textbox1 btn2" OnClick="btnsetpriority_Click" />
                        <asp:Button ID="btnresetpriority" runat="server" Visible="false" Text="Reset" CssClass="textbox textbox1 btn2"
                            OnClick="btnresetpriority_Click" />
                    </center>
                    <br />
                    <center>
                        <div id="rprint" runat="server" visible="false">
                            <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                                Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="txtexcel" onkeypress="display()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnexcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox textbox1 btn2" Width="125px" Text="Export Excel"
                                OnClick="btnexcel_Click" />
                            <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn2" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                    <br />
                    <center>
                        <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                                    <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                            OnClick="btnerrclose_Click" Text="Ok" runat="server" />
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
            </div>
        </center>
    </body>
    </html>
</asp:Content>
