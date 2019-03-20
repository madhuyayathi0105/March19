<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Student_Stage_Count.aspx.cs" Inherits="Student_Stage_Count"
    EnableEventValidation="false" %>

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
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style>
        body
        {
            font-family: Book Antiqua;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblsmserror.ClientID %>').innerHTML = "";
            }
            function display1() {
                document.getElementById('<%=lblsmserror1.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <span class="fontstyleheader" style="color: Green">StageWise Student Details Report</span>
            <br />
            <div class="maindivstyle" style="height: auto; width: 950px;">
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblcoll" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                CssClass="textbox1 ddlheight5" OnSelectedIndexChanged="ddlcollege_change" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblStage" runat="server" Text="Stage Name" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updStage" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtStage" runat="server" Text="--Select--" CssClass="textbox textbox1 txtheight3"></asp:TextBox>
                                    <asp:Panel ID="pnlStage" runat="server" CssClass="multxtpanel" Height="200px">
                                        <asp:CheckBox ID="cbStage" runat="server" Text="Select All" OnCheckedChanged="cbStage_Change"
                                            AutoPostBack="true" />
                                        <asp:CheckBoxList ID="cblStage" runat="server" OnSelectedIndexChanged="cblStage_Change"
                                            AutoPostBack="true">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popStage" runat="server" TargetControlID="txtStage"
                                        PopupControlID="pnlStage" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblAutoStage" runat="server" Text="Stage Name" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_AutoStage" runat="server" OnTextChanged="txt_AutoStage_Change"
                                AutoPostBack="true" MaxLength="50" Style="font-family: book antiqua; margin-left: 0px;
                                font-size: medium;"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetStageName" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_AutoStage"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="txtsearchpan">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                        <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                            <ContentTemplate>
                            <asp:Button ID="btnGo" runat="server" Text="GO" CssClass="textbox1 btn1" OnClick="btnGo_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblMainErr" runat="server" Visible="false" Text="" Font-Bold="true"
                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red"></asp:Label>
                <br />
                <br />
                <table>
                    <tr>
                        <td>
                            <center>
                                <asp:Button ID="btntotalDayScholarCount" runat="server" Visible="false" Text="DayScholars"
                                    OnClick="btntotalDayScholarCount_onclick" />
                                <asp:Button ID="btnSelectedOnline" runat="server" Visible="false" Text="Selected Thorugh Online"
                                    OnClick="btnSelectedOnline_onclick" />
                                <asp:Button ID="btnselectedgeneral" runat="server" Visible="false" Text="Selected Directly"
                                    OnClick="btnselectedgeneral_onclick" />
                                <asp:Button ID="btnStageNotSelectedStudent" runat="server" Visible="false" Text="Stage Unselected Students"
                                    OnClick="btnStageNotSelectedStudent_onclick" />
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="btnprintupdatepanel" runat="server">
                                <ContentTemplate>
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="1px" Width="666px" Style="height: 300px; overflow: auto;
                                background-color: White;" CssClass="spreadborder" ShowHeaderSelection="false"
                                OnButtonCommand="Fpspread1_Command">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </td>
                    </tr>
                </table>
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
                </div>
                <br />
                <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="1px" Width="850px" Style="height: 350px; overflow: auto;
                    background-color: White;" CssClass="spreadborder" ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <div id="rprint1" visible="false" runat="server">
                    <asp:Label ID="lblsmserror1" Text="Please Enter Your Report Name" Font-Size="Large"
                        Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblexcel1" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <asp:TextBox ID="txtexcel1" onkeypress="display1()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcel1"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnexcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        CssClass="textbox textbox1 btn2" Width="150px" Text="Export Excel" OnClick="btnexcel1_Click" />
                    <asp:Button ID="btnprintmaster1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Text="Print" OnClick="btnprintmaster1_Click" CssClass="textbox textbox1 btn2"
                        Width="100px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
                </div>
                <br />
            </div>
        </center>
        <center>
            <div id="divPopAlertNEW" runat="server" visible="false" style="height: 550em; z-index: 2000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                left: 0%;">
                <center>
                    <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                        height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%; padding: 5px;">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblAlertMsgNEW" runat="server" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnPopAlertCloseNEW" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                                Text="Ok" runat="server" />
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
        </ContentTemplate>
     </asp:UpdatePanel>

     <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>

    

     <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="btnprintupdatepanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>

    </html>
</asp:Content>
