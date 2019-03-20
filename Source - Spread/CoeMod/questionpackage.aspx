<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true"
    CodeFile="questionpackage.aspx.cs" Inherits="questionpackage" EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblnorec').innerHTML = "";
        }
    </script>
      <script type="text/javascript">
          function PrintPanel() {
              var panel = document.getElementById("<%=pnlContents.ClientID %>");
              var printWindow = window.open('', '', 'height=842,width=1191');
              printWindow.document.write('<html');
              printWindow.document.write('<head><title>Seating Arrangement</title>');
              printWindow.document.write('</head><body >');
              printWindow.document.write('<form>');
              printWindow.document.write(panel.innerHTML);
              printWindow.document.write(' </form>');
              printWindow.document.write('</body></html>');
              printWindow.document.close();
              setTimeout(function () {
                  printWindow.print();
              }, 500);
              return false;
          }
    </script>
    <script type="text/javascript">
        function validation() {

            var ddlYear = document.getElementById("<%=ddlYear.ClientID %>");
            var ddlMonth = document.getElementById("<%=ddlMonth.ClientID %>");
            var ddlDate = document.getElementById("<%=ddlDate.ClientID %>");
            var ddlSession = document.getElementById("<%=ddlSession.ClientID %>");
            var ddlhall = document.getElementById("<%=ddlhall.ClientID %>");
            var btnView1 = document.getElementById("<%=btnView1.ClientID %>");
            if ((ddlYear.value == 0)) {
                alert("Please Select The Year");
                return false;
            }
            if ((ddlMonth.value == 0)) {
                alert("Please Select The Month");
                return false;
            }
            if ((ddlDate.value == 0)) {
                alert("Please Select The Date");
                return false;
            }

            if ((ddlSession.value == 0)) {
                alert("Please Select The Session");
                return false;
            }
            if ((ddlhall.value == 0)) {
                alert("Please Select The Hall");
                return false;
            }

        }
    </script>
    <asp:ScriptManager ID="scrptmngr" runat="server">
    </asp:ScriptManager>
    <contenttemplate>
    <br />
    <center>
            <span class="fontstyleheader" style="color:Green;">Question Paper Packing</span>
            <br />
            <br />
            <table class="maintablestyle">
                <tr>
                 <td>
                    <asp:Label ID="lbl_collegename" Text="College"  runat="server"></asp:Label>
                
                     <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                    OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                  </td>
                    <td>
                <asp:Label ID="lbltype" runat="server" Text="Type" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
     
                <asp:DropDownList ID="ddltype" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Width="101px" OnSelectedIndexChanged="ddltype_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
                    <td>
                        <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" OnCheckedChanged="CheckBox1_click"
                            Font-Size="Medium" />
                        <asp:Label ID="Label1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Format1"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="True" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" OnCheckedChanged="CheckBox2_click"
                            Font-Size="Medium" />
                        <asp:Label ID="Label2" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Format2"></asp:Label>
                    </td>
                     <td>
                        <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="True" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" OnCheckedChanged="CheckBox3_click"
                            Font-Size="Medium" />
                        <asp:Label ID="Label3" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Format3"></asp:Label>
                    </td>
                     <td>
                        <asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="True" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" OnCheckedChanged="CheckBox4_click"
                            Font-Size="Medium" />
                        <asp:Label ID="Label4" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Format4"></asp:Label>
                    </td>
                     <td>
                        <asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="True" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" OnCheckedChanged="CheckBox5_click"
                            Font-Size="Medium" />
                        <asp:Label ID="Label5" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Format5"></asp:Label>
                    </td>
                    <td></td>
                   
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMonth" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Month And Year"></asp:Label>
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="60px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="60px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                   
                    <td>
                        <asp:Label ID="lbldate" runat="server" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlDate" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="101px" OnSelectedIndexChanged="ddldate_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsession" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Session"></asp:Label>
                        <asp:DropDownList ID="ddlSession" runat="server" CssClass="font" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Width="90px" OnSelectedIndexChanged="ddlsession_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblhall" runat="server" Text="Hall" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlhall" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="90px" OnSelectedIndexChanged="ddlhall_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnView" runat="server" Text="GO" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnView_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnView1" runat="server" Text="GO" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClientClick="return validation()" OnClick="btnView1_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                     <td>
                        <asp:Button ID="btnView4" runat="server" Text="GO" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClientClick="return validation()" OnClick="btnView4_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                     <td>
                        <asp:Button ID="btnView3" runat="server" Text="GO" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClientClick="return validation()" OnClick="btnView3_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                     <td>
                        <asp:Button ID="btnView2" runat="server" Text="GO" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClientClick="return validation()" OnClick="btnView2_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblerror" runat="server" CssClass="font" ForeColor="Red" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            </center>
            <br />
    <%--     <center>--%>
     <asp:Panel ID="pnlContents" runat="server" Visible="true" Style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">
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
                      #printable
                    {
                        position: relative;
                        bottom: 30px;
                        height: 300;
                    }
                }
               </style>
             <div id="printable">
            <table  class="printclass" style="width: 100%; font-weight: bold; font-family: Book Antiqua;
                                        font-size: medium; margin-top: 20px;">
                <tr>
                    <td>
                        <FarPoint:FpSpread ID="fpspread" runat="server" OnUpdateCommand="fpspread_OnUpdateCommand"
                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Height="550" Width="940"
                            HorizontalScrollBarPolicy="Never" Style="font-family: Book Antiqua; font-size: medium;
                            font-weight: bold;" VerticalScrollBarPolicy="Never" ShowHeaderSelection="false">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" Visible="false">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </td>
                    <td>
                            <FarPoint:FpSpread ID="fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Width="600" Visible="false">
                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark" ButtonType="PushButton">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                    </td>
                     <td>
                     <center>
                <FarPoint:FpSpread ID="fpspread2" runat="server" OnUpdateCommand="fpspread2_OnUpdateCommand"
                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Height="550" Width="940"
                    HorizontalScrollBarPolicy="Never" Style="font-family: Book Antiqua; font-size: medium;
                    font-weight: bold;" VerticalScrollBarPolicy="Never">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark" Visible="false">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                </center>
            </td>
             <td>
                <center>
                    <FarPoint:FpSpread ID="fpspread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Width="600" Visible="false">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark" ButtonType="PushButton">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
            </td>
                </tr>
            </table>
            </div>
            </asp:Panel>
            <%--</center>--%>


            <table >
                <tr>
                    <td>
                        <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="No Record(s) Found" Visible="False">
                        </asp:Label>
                    </td>
                </tr>
            </table>
            <center>
            <asp:Label ID="lblrptname" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Visible="false"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"
                onkeypress="display()"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Export To Excel" Width="127px" Visible="false" OnClick="btnExcel_Click" />
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
                Font-Size="Medium" Font-Bold="true" Visible="false" OnClick="btnprintmaster_Click" />
            <asp:Button ID="btn_directprint" runat="server" Visible="false" CssClass="fontbold"
            Width="100px" Text="Direct Print" OnClientClick="return PrintPanel();" />
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </center>
            <table style="margin-left: 0px; left: 450px; position: absolute;">
                <tr>
                    <td align="center">
                        <asp:Button ID="btngenerate" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="false" OnClick="btngenerate_click" Text="Print" />
                    </td>
                    
                </tr>
            </table>
        </contenttemplate>
    <triggers>
            <asp:PostBackTrigger ControlID="btngenerate" />
        </triggers>
        
</asp:Content>
