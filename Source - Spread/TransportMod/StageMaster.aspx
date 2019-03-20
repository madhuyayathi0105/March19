<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StageMaster.aspx.cs" Inherits="StageMaster" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script type="text/javascript">
     function fStage() {
         document.getElementById('<%=ceradd.ClientID%>').style.display = 'block';
         document.getElementById('<%=cerremove.ClientID%>').style.display = 'block';
     }
    </script>
      <style type="text/css">
        ody, input
        {
            font-family: Tahoma;
            font-size: 11px;
        }
        
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 500px;
            min-height: 100px;
            max-height: 150px;
            top: 100px;
            left: 150px;
        }
        
        .topHandle
        {
            background-color: #97bae6;
        }
        .floatr
        {
            border-style: solid;
            border-color: Black;
            border-width: thin;
            float: right;
        }
        
        
        .tabl
        {
            empty-cells: show;
            border-style: solid;
            border-color: Gray;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: normal;
        }
        .tablfont
        {
            empty-cells: show;
            border-style: solid;
            border-color: Gray;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
        }
        .tabl3
        {
            empty-cells: hide;
            border-style: solid;
            border-color: Black;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
            text-align: left;
        }
        .tabl5
        {
            border-style: solid;
            border-color: Black;
            border-width: thin;
            text-align: left;
        }
        .tabl1
        {
            empty-cells: show;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
        }
        .linkbtn
        {
            border-color: White;
            border-style: none;
            background-color: transparent;
            cursor: pointer;
        }
        .HeaderSelectedCSS
        {
            color: white;
            background-color: #719DDB;
            font-weight: bold;
            font-size: medium; /* font-style:italic;  */
        }
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .style2
        {
            width: 118px;
        }
        .style4
        {
            width: 43px;
        }
        .stylefp
        {
            cursor: pointer;
        }
        .style5
        {
            width: 185px;
        }
        .style55
        {
            width: 25px;
        }
        .style27
        {
            width: 25px;
        }
        .style25
        {
            width: 200px;
        }
        .style251
        {
            width: 125px;
        }
        .style6
        {
            width: 528px;
        }
        .style12
        {
            width: 200px;
        }
        .style22
        {
            width: 122px;
        }
        .style24
        {
            width: 30px;
        }
        
        .font
        {
            font-size: Small;
            font-family: MS Sans Serif;
        }
        .HeaderCSS
        {
            color: white;
            background-color: #719DDB;
            font-size: medium; /* border:solid 1px salmon; */
            font-weight: bold;
        }
        .cpBody
        {
            background-color: #DCE4F9; /*font: normal 11px auto Verdana, Arial;
            border: 1px gray;               
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
            width:720;*/
        }
        .accordion
        {
            width: 400px;
        }
        
        .accordionHeader
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #2E4d7B;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionHeaderSelected
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #5078B3;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionContent
        {
            background-color: White;
            border: 1px dashed #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
   
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <table style="width: 946px">
        <tr>
            <td align="left">
                <asp:Panel ID="pnl4" runat="server" BackImageUrl="~/image/Top Band-2.jpg" Height="20px"
                    Style="margin-left: 0px; top: 75px; left: -23px; width: 1018px; position: absolute;">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                   
                    <asp:Label ID="Label31" runat="server" Text="Stage Master" Font-Bold="true" Font-Names="MS Sans Serif"
                        Font-Size="Medium" ForeColor="White"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <%--<asp:LinkButton ID="LinkButton1" runat="server" Font-Names="MS Sans Serif" Font-Size="Small"
                                ForeColor="White" Font-Bold="true" PostBackUrl="~/StageMaster.aspx">Home</asp:LinkButton>
                            &nbsp; &nbsp;
                            <asp:LinkButton ID="LinkButton3" runat="server" Font-Names="MS Sans Serif" Font-Size="Small"
                                ForeColor="White" Font-Bold="true" PostBackUrl="~/Default_login.aspx">Back</asp:LinkButton>
                            &nbsp; &nbsp;
                            <asp:LinkButton ID="lb2" runat="server" OnClick="lb2_Click" Font-Bold="true" Font-Names="MS Sans Serif"
                                Font-Size="Small" ForeColor="White">Logout</asp:LinkButton>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <table class="tabl" style="width: 726px; margin-left:10px;">
        <tr>
            <td>
                <asp:Label ID="lblvehicletype" runat="server" Font-Bold="true" CssClass="font" Text="District"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlvehicletypeview" runat="server" Font-Bold="true" CssClass="font"
                    Width="122px" AutoPostBack="True" OnSelectedIndexChanged="ddlvehicletypeview_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lbltypeview" runat="server" Font-Bold="true" CssClass="font" Text="Stage Name"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddltypeview" runat="server" Font-Bold="true" CssClass="font"
                    Width="122px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnMainGo" runat="server" Text="Go" Font-Bold="True" OnClick="btnMainGo_Click"
                    Font-Names="Book Antiqua" Font-Size="Medium" />
            </td>
            <td>
                <table class="tabl" width="250">
                    <tr>
                        <td>
                            <asp:Label ID="Label131" runat="server" Text="District" Font-Bold="true" Font-Names="MS Sans Serif"
                                Font-Size="Small"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="ceradd" runat="server" Text="+" Font-Names="MS Sans Serif" Font-Size="Small"
                                Height="21px" OnClick="ceradd_Click" Style="display: none;" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcertificate" runat="server" Height="20" Width="100" Font-Names="MS Sans Serif"
                                Font-Size="Small">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="cerremove" runat="server" Text="-" Font-Names="MS Sans Serif" Font-Size="Small"
                                Height="21px" OnClick="cerremove_Click" Style="display: none;" />
                        </td>
                        <td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Panel ID="Panelceradd" runat="server" Visible="False" Style="width: 225px; height: 70px;
        top: 156px; left: 499px; right: 300px; position: absolute;" BorderStyle="Solid"
        BorderWidth="1px" BackColor="#CCCCCC" Font-Names="MS Sans Serif" Font-Size="Small">
        <center>
            <caption runat="server" id="Caption6" style="height: 10px; top: 10px; font-variant: Small-caps">
                District</caption>
            <br />
            <asp:TextBox ID="tbaddcer" Width="210px" Height="14px" runat="server"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" TargetControlID="tbaddcer"
                InvalidChars="1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,^,&,*,(,),<,>,?,|,_,+,/,\,:,;,',[,],{,},`,~"
                runat="server" FilterMode="InvalidChars">
            </asp:FilteredTextBoxExtender>
            <br />
            <asp:Button ID="addcernew" Width="70px" runat="server" Text="Add" OnClick="addcernew_Click"
                Font-Names="MS Sans Serif" Font-Size="Small" Height="25px" />
            &nbsp;
            <asp:Button ID="exitcernew" Width="50px" runat="server" Text="Exit" OnClick="exitcernew_Click"
                Font-Names="MS Sans Serif" Font-Size="Small" Height="25px" />
        </center>
    </asp:Panel>
    <asp:Label ID="Labelvalidation" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
        Font-Bold="true" Font-Size="Small" Style="position: absolute; top: 594px;"></asp:Label>
    <center>
        <br />
        <table class="tabl">
            <tr>
                <td>
                    <center>
                        <asp:Button ID="btnremoverowfee" runat="server" OnClick="addremoveStage" Text="Remove"
                            Visible="false" Style="top: 180px; left: 652px; position: absolute;" />
                        <asp:Button ID="btnaddrowfee" runat="server" OnClick="addrowStage" Font-Bold="true"
                            Text="Add Stage" Style="top: 120px; left: 876px; position: absolute;" />
                        <br />
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                            Font-Size="3pt" Font-Bold="true" Visible="false"></asp:Label>
                        <br />
                        <FarPoint:FpSpread ID="FpSpreadstage" runat="server" BorderColor="Black" BorderStyle="Solid"
                            OnButtonCommand="FpSpreadstage_ButtonCommand" OnUpdateCommand="FpSpreadstage_UpdateCommand"
                            BorderWidth="1px" Height="391px" Width="1000" HorizontalScrollBarPolicy="Never"
                            VerticalScrollBarPolicy="Never">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                            <ClientEvents EditStopped="FpSpreadstage_ActiveCellChanged" />
                        </FarPoint:FpSpread>
                    </center>
                </td>
            </tr>
        </table>
    </center>
    <div style="text-align: center;">
        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="ButtonsaveRoute_Click"
            Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False"
            ForeColor="Black" Width="60px" Height="25px" />
        <asp:Button ID="Buttondelete" runat="server" Text="Delete" OnClick="Buttondelete_Click"
            Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False"
            ForeColor="Black" Width="70px" Height="25px" Enabled="False" />
        <asp:Button ID="Btnprint" runat="server" Text="Print" OnClick="ButtonPrint_Click"
            Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False"
            ForeColor="Black" Width="70px" Height="25px" Enabled="False" />
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
    </div>
     </ContentTemplate>
        </asp:UpdatePanel>
  
</asp:Content>
