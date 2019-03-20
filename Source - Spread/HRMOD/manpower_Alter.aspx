<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="manpower_Alter.aspx.cs" Inherits="manpower_Alter" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function myFunction1(y) {
            y.style.borderColor = "#c4c4c4";
        }
        function Test2() {
            var empty = "";
            var deptname = document.getElementById("<%=txt_dptname.ClientID %>").value;

            if (empty.trim() != "") {

                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <body>
        <script type="text/javascript">


            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function display() {
                document.getElementById('<%=lbl_norec.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Man Power Master</span></div>
                </center>
                <center>
                    <div class="maindivstyle" style="height: 550px; width: 1000px;">
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lblclg" runat="server" Text="CollegeName" Fontcolor="black" Font-Names="Book Antiqua"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlclg" runat="server" Style="height: 30px; width: 209px;"
                                        OnSelectedIndexChanged="ddlclgonselected" AutoPostBack="true" Font-Names="Book Antiqua"
                                        Font-Bold="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldptname" runat="server" Text="Department Name" Fontcolor="black"
                                        Font-Names="Book Antiqua" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel_manpower" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_dptname" runat="server" Width="150px" Font-Names="Book Antiqua"
                                                AutoPostBack="true" CssClass="textbox textbox1" ReadOnly="true"> --Select--</asp:TextBox>
                                            <asp:Panel ID="panel_manpower" runat="server" Width="490px" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="cb_dptname" runat="server" Text="Select All" AutoPostBack="True"
                                                    Font-Names="Book Antiqua" OnCheckedChanged="cb_dptname_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_dptname" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                    Font-Bold="true" OnSelectedIndexChanged="cbl_dptname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popdptname" runat="server" TargetControlID="txt_dptname"
                                                PopupControlID="panel_manpower" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbldesname" runat="server" Text="Designation Name" Fontcolor="black"
                                        Font-Names="Book Antiqua" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upddes" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_desname" runat="server" Width="200px" Font-Names="Book Antiqua"
                                                AutoPostBack="true" CssClass="textbox textbox1" ReadOnly="true"> --Select--</asp:TextBox>
                                            <asp:Panel ID="pnldesname" runat="server" Width="490px" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="cb_desname" runat="server" Text="Select All" AutoPostBack="True"
                                                    Font-Names="Book Antiqua" OnCheckedChanged="cb_desname_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_desname" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                    Font-Bold="true" OnSelectedIndexChanged="cbl_desname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_desname"
                                                PopupControlID="pnldesname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblsearch" runat="server" Text="Search by Designation" Fontcolor="black"
                                        Font-Names="Book Antiqua" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_search" Visible="true" runat="server" MaxLength="20" CssClass="textbox textbox1"
                                        Width="200px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" Text="Go" runat="server" Font-Bold="true" CssClass="textbox textbox1 btn2"
                                        Font-Names="Book Antiqua" OnClick="btn_go_Click" />
                                    <%--OnClientClick="return Test2()"--%>
                                    <asp:Label ID="lblvalue" runat="server" Text="value" Visible="false" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="div1" runat="server" visible="false" style="box-shadow: 0 0 8px #999999;
                            height: 340px; overflow: hidden; width: 800px;" class="reportdivstyle">
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Width="800px" Height="340px" ShowHeaderSelection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <div id="rptprint" runat="server" visible="true">
                            <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1" Height="20px"
                                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                                Height="35px" CssClass="textbox textbox1" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                                CssClass="textbox textbox1" />
                            <asp:Button ID="btnsave" runat="server" OnClientClick="return Test2()" Text="Save"
                                OnClick="btnsave_Click" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Width="50px" Height="35px" CssClass="textbox textbox1" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </div>
                </center>
                <center>
                    <div id="imgdiv2" runat="server" visible="false" style="height: 500%; z-index: 1000;
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
            <Triggers>
                <asp:PostBackTrigger ControlID="btn_go" />
                <asp:PostBackTrigger ControlID="btnsave" />
                <asp:PostBackTrigger ControlID="btn_errorclose" />
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
            </Triggers>
        </asp:UpdatePanel>
    </body>
    </html>
</asp:Content>
