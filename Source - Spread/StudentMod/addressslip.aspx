<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="addressslip.aspx.cs" Inherits="addressslip" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        *
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <table style="width: 946px">
                <tr>
                    <td align="center">
                        <asp:Panel ID="Panel2" runat="server" align="center" BackImageUrl="~/Menu/Top Band-2.jpg"
                            Height="20px" Style="margin-left: 0px; top: 145px; left: -23px; width: 1063px;
                            position: absolute;">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label1" runat="server" Text="Address Slip" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="White"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Small" ForeColor="White" PostBackUrl="~/strengthgrid.aspx">Back</asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton ID="lb1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Small" ForeColor="White" PostBackUrl="~/Default2.aspx">Home</asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton ID="lb2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Small" ForeColor="White" OnClick="lb2_Click">Logout</asp:LinkButton>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="LinkButtonb2" runat="server" Font-Bold="True" Visible="false"
                                Font-Names="Book Antiqua" Font-Size="Small" ForeColor="White" PostBackUrl="~/general reports.aspx">Back</asp:LinkButton>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <br />
    <br />
    <br />
    <br />
    <br />
    <table>
        <tr>
            <td>
                <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" AutoPostBack="True" CssClass="textbox1 ddlstyle ddlheight3"
                    OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="style14">
                <asp:Label ID="lblDegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Degree"></asp:Label>
            </td>
            <td class="style18">
                <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" CssClass="textbox1 ddlstyle ddlheight3"
                    OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </td>
            <td class="style15">
                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" CssClass="textbox1 ddlstyle ddlheight3"
                    OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Width="250px" Style="margin-left: 0px"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                </asp:DropDownList>
            </td>
            <td style="width: 30px">
                <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlSem" runat="server" Width="50px" AutoPostBack="True" CssClass="textbox1 ddlstyle ddlheight3"
                    OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium">
                </asp:DropDownList>
            </td>
            <td style="width: 30px">
                <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" CssClass="textbox1 ddlstyle ddlheight3"
                    OnSelectedIndexChanged="ddlSec_SelectedIndexChanged" Width="70px" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btngo_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <fieldset style="height: 20px; width: 198px;">
                    <asp:RadioButton ID="PermanantRadio" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" GroupName="Test2" Text="Permanant" OnCheckedChanged="PermanantRadio_CheckedChanged" />
                    <asp:RadioButton ID="contactRadio" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" GroupName="Test2" Text="Contact" OnCheckedChanged="contactRadio_CheckedChanged" />
                </fieldset>
            </td>
            <td colspan="2">
                <asp:Button ID="Button2" runat="server" Text="Address label" OnClick="Button2_Click"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                <asp:Button ID="BtnSlip" runat="server" Text="Slip" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="BtnSlip_Click" Visible="False" />
            </td>
            <td>
                <asp:Label ID="Label3" Text="Report Type" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
                <asp:DropDownList ID="ddlreportTye" runat="server" AutoPostBack="true" Width="145px"
                    Font-Bold="True" Font-Names="Book Antiqua" CssClass="textbox1 ddlstyle ddlheight3"
                    OnSelectedIndexChanged="ddlreportTye_SelectedIndexChanged">
                    <asp:ListItem Value="0">Applied</asp:ListItem>
                    <asp:ListItem Value="1">Shortlist</asp:ListItem>
                    <asp:ListItem Value="2">Wait to Admitted</asp:ListItem>
                    <asp:ListItem Value="3">Admitted</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Select Any one student" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Small" ForeColor="#FF3300" Style="top: 192px;
                    left: 251px; position: absolute; height: 18px; width: 137px" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/image/Top Band-2.jpg" Width="1000px"
        Style="top: 296px; left: 0px; position: absolute; height: 15px">
        <br />
        <br />
        <asp:Label ID="lblnorec" runat="server" Text="No records Found" Visible="False" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="#FF3300"></asp:Label>
        <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
            Style="top: 19px; left: 4px; position: absolute; height: 21px; width: 219px"></asp:Label>
        &nbsp;&nbsp;
        <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
            Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 19px; left: 230px; position: absolute;
            height: 21px; width: 126px"></asp:Label>
        &nbsp;&nbsp;
        <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
            Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
            Height="24px" Width="58px" Style="top: 17px; left: 365px; position: absolute">
        </asp:DropDownList>
        &nbsp;&nbsp;
        <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
            AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 22px; left: 433px; position: absolute"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
            FilterType="Numbers" />
        &nbsp;&nbsp;
        <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
            Width="96px" Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 18px; left: 476px;
            position: absolute; height: 21px"></asp:Label>
        &nbsp;&nbsp;
        <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
            OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Height="17px" Style="top: 19px; left: 579px; position: absolute;
            width: 34px;"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
            FilterType="Numbers" />
        &nbsp;&nbsp;
        <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 22px; left: 628px; position: absolute;
            height: 21px; width: 303px"></asp:Label>
    </asp:Panel>
    <asp:Panel ID="Panel5" runat="server" Style="top: 407px; left: 0px; position: absolute;
        height: 3px; width: 960px">
        <center style="top: 0px; left: 0px; position: absolute; height: 15px; width: 960px">
            <div>
                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Height="200" Width="500" Visible="false" OnButtonCommand="fpspreadshow_Command"
                    HorizontalScrollBarPolicy="Never" VerticalScrollBarPolicy="Never">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </div>
        </center>
        <div>
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Style="position: absolute;
                left: 228px; top: 343px" />
            <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="BtnSlip"
        CancelControlID="Button1" PopupControlID="Panel4" PopupDragHandleControlID="PopupHeader"
        Drag="true" BackgroundCssClass="ModalPopupBG">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel4" runat="server" Width="1000px" Height="620px" ScrollBars="Auto"
        BorderColor="Black" BorderStyle="Double" Style="display: none; height: 400; width: 700;">
        <div class="HellowWorldPopup" style="background-color: white;">
            <div class="PopupHeader" id="Div2" style="text-align: center; color: Blue; font-family: Book 

Antiqua; font-size: xx-large; font-weight: bold">
            </div>
            <div class="PopupBody">
            </div>
            <div class="Controls">
                <center>
                    <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="600" Width="900" HorizontalScrollBarPolicy="Never"
                        VerticalScrollBarPolicy="Never">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark" ButtonType="PushButton" ShowPDFButton="True">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" GridLineColor="White">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
            <asp:Button ID="Button1" runat="server" Text="Close" Style="margin-left: 50px;" />
            <br />
        </div>
    </asp:Panel>
    <%--<panel>
                      <FarPoint:FpSpread ID="FpSpread2" Width=400 Height=200 BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" runat=server><CommandBar ButtonShadowColor="ControlDark" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight" BackColor="Control"></CommandBar><Sheets><FarPoint:SheetView SheetName="Sheet1"></FarPoint:SheetView></Sheets></FarPoint:FpSpread>
                      </panel>--%>
</asp:Content>
