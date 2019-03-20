<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="DespatchOfAnswerPackets.aspx.cs" Inherits="DespatchOfAnswerPackets" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" Runat="Server">
 <style type="text/css">
        .fontbold
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        .fontnormal
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        .printclass
        {
            display: none;
        }
    </style>
    <style type="text/css">
        .fontStyle
        {
            font-size: medium;
            font-weight: bolder;
            font-style: oblique;
            padding: 5px;
        }
        .fontStyle1
        {
            font-size: medium;
            font-style: oblique;
            padding: 3px;
            color: Blue;
        }
        .commonHeaderFont
        {
            font-size: medium;
            color: Black;
            font-family: 'Book Antiqua';
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        function PrintPanel1() {
            var panel = document.getElementById("<%=pnlContent1.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            var spF1College = document.getElementById('<%=spF1College.ClientID%>');
            spF1College.style.display = "block";
            var spF1Date = document.getElementById('<%=spF1Date.ClientID%>');
            spF1Date.style.display = "block";
            var spHead = document.getElementById('<%=spHead.ClientID%>');
            spHead.style.display = "block";
            var dateoedel = document.getElementById('<%=dateoedel.ClientID%>');
            dateoedel.style.display = "block";
            var spsign = document.getElementById('<%=spsign.ClientID%>');
            spsign.style.display = "block";
            
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>DespatchOfAnswerPackets</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            spF1College.style.display = "none";
            spF1Date.style.display = "none";
            spHead.style.display = "none";
            spsign.style.display = "none";
            dateoedel.style.display = "none";
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">Despatch Of Answer Packets </span>
    </center>
    <br />
     <div class="maindivstyle maindivstylesize">
     <br />
     <center>
        <table class="maintablestyle" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative;">
            <tr>
                
               <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont"
                            AssociatedControlID="txtCollege"></asp:Label>
                            </td>
                            <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlCollege" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtCollege" Visible="true" Width="104px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlCollege" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="280px">
                                        <asp:CheckBox ID="chkCollege" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkCollege_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblCollege" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblCollege_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupExtCollege" runat="server" TargetControlID="txtCollege"
                                        PopupControlID="pnlCollege" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                 <td>
                    <asp:Label ID="lblYear" runat="server" Style="" Width="34px" Text="Year"
                        CssClass="fontbold"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" Style="" runat="server" CssClass="fontbold" Width="60px"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblMonth" runat="server" Style="" Width="51px" Text="Month"
                        CssClass="fontbold"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" Style="" CssClass="fontbold" Width="65px"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                    </asp:DropDownList>
                    </td>
                    <td>
                    <asp:Label ID="lblDate" runat="server" Text="Date" Style="" CssClass="fontbold"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDate" runat="server" Style="" CssClass="fontbold" Width="101px"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlDate_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSession" runat="server" Text="Session" CssClass="fontbold" Style=""></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSession" runat="server" Style="" CssClass="fontbold" Width="90px"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged" >
                       
                    </asp:DropDownList>
                    
                </td>
                 <td>
                   <asp:Button ID="btngo" runat="server" Text="GO" Style="" CssClass="fontbold" Onclick="btngo_Click"/>
                </td>
                </tr>
                </table>
                <br />
                <br />
                     <asp:Label ID="lblerror" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
        position: relative;" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
        Width="800px" Font-Bold="true" ForeColor="Red"></asp:Label>
                </center>
                
         <div id="divFormat1" runat="server" visible="true">
        <center>
            <asp:Panel ID="pnlContent1" runat="server" Visible="true">
                <style type="text/css" media="print">
                    @page
                    {
                        size: A3 portrait;
                        margin: 0.5cm;
                    }
                    @media print
                    {
                        .printclass
                        {
                            display: table;
                        }
                        thead
                        {
                            display: table-header-group;
                        }
                        tfoot
                        {
                            display: table-footer-group;
                        }
                        #header
                        {
                            position: fixed;
                            top: 0px;
                            left: 0px;
                        }
                        #footer
                        {
                            position: fixed;
                            bottom: 0px;
                            left: 0px;
                        }
                        #printable1
                        {
                            position: relative;
                            bottom: 30px;
                            height: 300;
                            width: 100%;
                        }
                    
                    }
                    @media screen
                    {
                        thead
                        {
                            display: block;
                        }
                        tfoot
                        {
                            display: block;
                        }
                    }
                </style>
                <div id="printable1">
                    <table width="100%">
                        <thead>
                            <tr>
                                <th colspan="2">
                                    <div>
                                        <table class="printclass" style="width: 100%; font-weight: bold; font-family: Book Antiqua;
                                            font-size: medium; margin-top: 20px;">
                                            <tr>
                                                <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">

                                                    <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                                                        Width="100px" Height="100px" />
 
                                                </td>
                                                <td align="center">
                                                    <span id="spF1College" runat="server" style="font-size: 25px;"></span>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="center" colspan="2">
                                                    <span id="spcategory" runat="server" style="font-size: 14px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <span id="Spdegree" runat="server" style="font-size: 14px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <span id="spF1Date" runat="server" style="font-size: 14px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <span id="spHead" runat="server" style="font-size: 14px;"></span>
                                                </td>
                                            </tr>
                                            
                                       
                                </th>
                            </tr>
                        </thead>
                         
                                   
                          <tr>
                                <td colspan="2" align="center">
                                <center>
                        <FarPoint:FpSpread ID="Fpspread" runat="server" Visible="true" BorderWidth="0px"
                                        BorderStyle="Solid" BorderColor="Black"  CssClass="spreadborder" ActiveSheetViewIndex="0" >
                    <Sheets>
                        <FarPoint:SheetView  PageSize="100" SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                </center>
                </td>
                </tr>
                 <thead>
                            <tr>
                                <th colspan="2">
                                    <div>
                                        <table class="printclass" style="width: 100%; font-weight: bold; font-family: Book Antiqua;
                                            font-size: medium;">
                <tr>
                    <td align="left" colspan="6">
                       <span id="dateoedel" runat="server"  style="font-size: 13px;" ></span>
                     </td>
                  </tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>

<tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>

<tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>


                  <tr>

                                                <td align="left" colspan="2">
                                                    <span id="spsign" runat="server" style="font-size: 13px;"></span>
                                                </td>
                                            
                                                <td align="right" colspan="2">
                                                    <span id="spsignchif" runat="server" style="font-size: 13px;"></span>
                                                </td>
                                            </tr>
                                             <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>

<tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>

<tr></tr>
                  <tr></tr>
                  <tr></tr>
                  <tr></tr>
                                             <tr>
                                                <td align="left" colspan="2">
                                                    <span id="spnbun" runat="server" style="font-size: 13px;"></span>
                                                </td>
                                                 <td align="right" colspan="2">
                                                    <span id="spsig" runat="server" style="font-size: 13px;"></span>
                                                </td>
                                            </tr>
                                             </th>
                            </tr>
                        </thead>           
                </table>
                </div>
                </asp:Panel>
                </div>
                 <center>
            <asp:Button ID="btn_directprint" runat="server" CssClass="fontbold" Width="100px"
                Text="Direct Print"  OnClientClick="return PrintPanel1();"  />
        </center>
                

</asp:Content>

