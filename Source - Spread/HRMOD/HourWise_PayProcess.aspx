<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="HourWise_PayProcess.aspx.cs" Inherits="HourWise_PayProcess" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <%--   <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <style>
        body
        {
            font-family: Book Antiqua;
        }
    </style>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblsmserror.ClientID %>').innerHTML = "";
            }

            function onTotHrs() {
                var tothrs = document.getElementById('<%=txt_TotHrs.ClientID %>').value;
                if (parseInt(tothrs) > 10) {
                    document.getElementById('<%=txt_TotHrs.ClientID %>').value = "";
                    alert("Total Hours should be less than 10!");
                }
            }
            function onAmntPerHrs(el) {
                var ex = /^[0-9]+\.?[0-9]*$/;
                if (ex.test(el.value) == false) {
                    el.value = "";
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <span class="fontstyleheader" style="color: Green">HourWise - Pay Process Settings
            </span>
            <br />
            <br />
            <table class="maintablestyle">
                <tr>
                    <td>
                        College
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            CssClass="textbox1 ddlheight5" OnSelectedIndexChanged="ddlcollege_change" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Department
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upddept" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_dept" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                    Style="width: 135px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                    <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_dept"
                                    PopupControlID="p1" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        Designation
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtDesig" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                    Style="width: 135px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel1" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                    Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                    position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                    height: 200px;">
                                    <asp:CheckBox ID="cbDesig" runat="server" Text="Select All" OnCheckedChanged="cbDesig_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cblDesig" runat="server" OnSelectedIndexChanged="cblDesig_SelectedIndexChange"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtDesig"
                                    PopupControlID="Panel1" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkCommon" runat="server" Text="Common to All" OnCheckedChanged="chkCommon_Change"
                            AutoPostBack="true" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkstaff" runat="server" Text="StaffWise" OnCheckedChanged="chkstaff_Change"
                            AutoPostBack="true" />
                    </td>
                    
                    <td colspan="8">
                        &nbsp;&nbsp;&nbsp;<asp:Label ID="lblHrs" runat="server" Visible="false" Text="Total Hours"></asp:Label>
                        <asp:TextBox ID="txt_TotHrs" runat="server" MaxLength="2" Visible="false" onkeyup="return onTotHrs();"
                            CssClass="textbox textbox1" Width="50px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="filtertothrs" runat="server" TargetControlID="txt_TotHrs"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                        <asp:Label ID="lblAmntHrs" runat="server" Visible="false" Text="Amount/Hrs"></asp:Label>
                        <asp:TextBox ID="txt_AmntHrs" runat="server" MaxLength="5" Visible="false" onkeyup="return onAmntPerHrs(this);"
                            CssClass="textbox textbox1" Width="100px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_AmntHrs"
                            FilterType="Numbers,Custom" ValidChars=".">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnGo" runat="server" CssClass="textbox1 btn2" OnClick="btnGo_Click"
                            Text="GO" />
                        <asp:Button ID="btnSave" runat="server" Text="Save" Visible="false" BackColor="LightGreen"
                            CssClass="textbox1 btn2" OnClick="btnSave_Click" />
                        <asp:LinkButton ID="btn_convenesexp" Text="Convenes Exp" runat="server" OnClick="btn_convenesexpClick"></asp:LinkButton>
                    </td>

                </tr>
            </table>
            <br />
            <asp:Label ID="lblMainErr" runat="server" Visible="false" Text="" Font-Bold="true"
                Font-Size="Medium" ForeColor="Red" Font-Names="Book Antiqua"></asp:Label>
            <br />
            <br />
            <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderColor="Black"
                BorderStyle="Solid" BorderWidth="1px" Width="885px" Style="height: 380px; overflow: auto;
                background-color: White;" CssClass="spreadborder" OnButtonCommand="Fpspread1_command"
                ShowHeaderSelection="false">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br />
            <div id="rprint" visible="false" runat="server">
                <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                    Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
                <asp:TextBox ID="txtexcel" onkeypress="display()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnexcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    CssClass="textbox textbox1 btn2" Width="150px" Text="Export Excel" OnClick="btnexcel_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn2"
                    Width="100px" />
                <insproplus:printmaster runat="server" id="Printcontrol" visible="false" />
            </div>
            <br />
            <div id="divconvenesexp" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 285px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td>
                                        Convenes Expance
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_conexp" CssClass="textbox textbox1 txtheight" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_conexp"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <asp:Button ID="btn_convenesSave" CssClass="textbox textbox1 btn1" OnClick="btn_convenesSaveClick"
                                                Text="Save" runat="server" />
                                                <asp:Button ID="btn_exit" CssClass="textbox textbox1 btn1" OnClick="btn_convenesexitClick"
                                                Text="Exit" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
            <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                        <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" CssClass="textbox textbox1 btn1" OnClick="btn_errorclose_Click"
                                                Text="OK" runat="server" />
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
