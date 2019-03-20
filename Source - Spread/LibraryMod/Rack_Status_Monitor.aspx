<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Rack_Status_Monitor.aspx.cs" Inherits="LibraryMod_Rack_Status_Monitor" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Rack Status Monitor</span></div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 678px; font-family: Book Antiqua;
                font-weight: bold; height: auto">
                <asp:UpdatePanel ID="updatepanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <center>
                                            <div style="width: 1000px; height: auto">
                                                <table class="maintablestyle" style="height: auto; margin-left: -327px; margin-top: 10px;
                                                    margin-bottom: 10px; padding: 6px;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Text="College" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlstat_college" runat="server" Style="width: 204px; height: 30px;
                                                                margin-left: 72px;" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_sts_SelectedIndexChanged"
                                                                CssClass="textbox3 textbox1">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label212" runat="server" Text="Library" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddllibrary_sts" runat="server" Style="width: 204px; height: 30px;
                                                                margin-left: 31px;" AutoPostBack="true" OnSelectedIndexChanged="ddllibrary_sts_SelectedIndexChanged"
                                                                CssClass="textbox3 textbox1">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:Label ID="Label2" runat="server" Text="Rack Number" CssClass="commonHeaderFont">
                                                            </asp:Label>
                                                            <asp:DropDownList ID="ddlsts_rackno" runat="server" Style="width: 204px; height: 30px;
                                                                margin-left: 26px;" AutoPostBack="true" OnSelectedIndexChanged="ddlrack_sts_SelectedIndexChanged"
                                                                CssClass="textbox3 textbox1">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="upgo" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="btn_sts_Rack_Go" runat="server" ImageUrl="~/LibImages/Go.jpg"
                                                                        OnClick="btn_sts_Rack_Go_Click" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </center>
    </div>
    <br />
    <br />
    <br />
    <asp:UpdatePanel ID="updatepanel2" runat="server">
        <ContentTemplate>
            <fieldset id="fpfieldset" runat="server" visible="false">
                <center>
                    <asp:UpdatePanel ID="updatepanel3" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                            <asp:GridView ID="grdRackStatus" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" ShowHeaderWhenEmpty="true"
                                Width="980px" OnRowCreated="grdRackStatus_OnRowCreated" OnSelectedIndexChanged="grdRackStatus_SelectedIndexChanged">
                                <%----%>
                                <Columns>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                            </asp:GridView>
                            <br />
                            <br />
                            <table id="rack_st_field" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <fieldset id="Fieldset1" runat="server" style="width: 103px; font-family: Book Antiqua;
                                            font-weight: bold; height: 13px; background-color: Red; margin-left: -35px;">
                                            <asp:Label ID="Label6" runat="server" Text="CompletetyFilled"></asp:Label>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset id="Fieldset6" runat="server" enabled="false" style="width: 103px; font-family: Book Antiqua;
                                            font-weight: bold; height: 13px; background-color: PaleGreen; margin-left: 27px;">
                                            <asp:Label ID="Label3" runat="server" Text="PartiallyFilled"></asp:Label>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset id="Fieldset7" runat="server" enabled="false" style="width: 115px; font-family: Book Antiqua;
                                            font-weight: bold; height: 13px; background-color: Yellow; margin-left: 27px;">
                                            <asp:Label ID="Label7" runat="server" Text="No Shelf Entry"></asp:Label>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <table style="margin-left: -28px; margin-top: 10px;" id="rack_st_des" runat="server"
                                visible="false">
                                <tr>
                                    <td>
                                        <asp:Label ID="LabelShelf" Text="SH->Shelf" runat="server" Style="font-style: italic;
                                            font-weight: bold; margin-left: 20px;"></asp:Label>
                                        <asp:Label ID="LabelMaximum" Text="TOT->Maximum Copies" runat="server" Style="font-style: italic;
                                            font-weight: bold; margin-left: 20px;"></asp:Label>
                                        <asp:Label ID="LabelFill" Text="Filled->Filled Copies" runat="server" Style="font-style: italic;
                                            font-weight: bold; margin-left: 20px;"></asp:Label>
                                        <asp:Label ID="LabelAvailable" Text="AVAIL->Available Copies" runat="server" Style="font-style: italic;
                                            font-weight: bold; margin-left: 20px;"></asp:Label>
                                        <asp:Label ID="LabelCategory" Text="IM->Category Of Inward Material" runat="server"
                                            Style="font-style: italic; font-weight: bold; margin-left: 20px;"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="grdRackStatus" />
                        </Triggers>
                    </asp:UpdatePanel>
                </center>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
      <center>
        <asp:UpdatePanel ID="updatepanel43" runat="server">
            <ContentTemplate>
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                        Font-Size="Medium" CssClass="textbox textbox1"></asp:TextBox>
                    <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                        OnClick="btnExcel_Click" />
                    <asp:ImageButton ID="btnprintmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                        OnClick="btnprintmaster_Click" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                </div>
            </ContentTemplate>
               <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
            </Triggers>

        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="updatepanel4" runat="server">
            <ContentTemplate>
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
                                                <asp:ImageButton ID="btnerrclose" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btnerrclose_Click" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="updatepanel5" runat="server">
            <ContentTemplate>
                <div id="Divfspreadstatus" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 36px; margin-left: 439px;"
                        OnClick="btn_popclose5_Click" />
                    <br />
                    <div style="background-color: White; height: 500px; width: 900px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px; margin-left: 30px; margin-top: 30px;">
                        <br />
                        <asp:Label ID="Label4" runat="server" Text="Book Information In The Rack" Style="color: Green;
                            font-family: Book Antiqua;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        <br />
                        <div id="div1" runat="server" visible="false" style="width: 870px; height: 400px;
                            overflow: auto; background-color: White; border-radius: 10px;">
                            <asp:GridView ID="grdCellClick" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" ShowHeaderWhenEmpty="true"
                                Width="800px">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                            </asp:Label></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <asp:ImageButton ID="Buttonexit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                            OnClick="Buttonexit_Click" />
                        <br />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--Progress bar for  go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upgo">
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
</asp:Content>
