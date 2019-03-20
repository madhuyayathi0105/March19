<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AllotmentReport1.aspx.cs" Inherits="CoeMod_AllotmentReport1" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
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
                <span class="fontstyleheader" style="color: Green;">Question Paper Setter Report</span></div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <div>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                    <td>
                                                        <asp:Label ID="lblExamMonth" runat="server" CssClass="commonHeaderFont" Text="ExamMonth">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlExamMonth" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlExamMonth" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlExamMonth_SelectedIndexChanged" AutoPostBack="True" Width="80px">  
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblExamYear" runat="server" CssClass="commonHeaderFont" Text="ExamYear">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlExamYear" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlExamYear" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged"  AutoPostBack="True" Width="80px">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_go" Text="Go" CssClass=" textbox btn1" runat="server" OnClick="btn_go_Click" />
                                                    </td>
                                                </tr>
                                        </table>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </center>

         <asp:Label ID="lblXpos" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lblYpos" runat="server" Visible="false"></asp:Label>
        <br />
        <br />
      
        <center>
            <div id="showreport1" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <FarPoint:FpSpread ID="spreadDet1" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                Width="980px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder" OnCellClick="spreadDet1_OnCellClick" OnPreRender="spreadDet1_Selectedindexchange">
                             <%--  --%>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                       <tr>
                        <td>
                            <center>
                                <div id="print1" runat="server" visible="false">
                                    <asp:Label ID="lblvalidation2" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                    <asp:Label ID="lblrptname1" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname1" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                   <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>--%>
                                    <asp:Button ID="btnExcel1" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnExcel_Click1" Text="Export To Excel" Width="127px"
                                        Height="32px" CssClass="textbox textbox1" />
                                    <asp:Button ID="btnprintmasterhed1" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click1" Height="32px"
                                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrolhed1" Visible="false" />
                                </div>
                            </center>
                        </td>
                    </tr>
              
                </table>
            </div>
        </center>

          <center>
            <div id="showreport2" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <FarPoint:FpSpread ID="spreadDet2" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                Width="980px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder" OnButtonCommand="spreadDet2_OnButtonCommand">

                          <%--      OnCellClick="spreadDet2_OnCellClick"  OnPreRender="spreadDet2_Selectedindexchange"--%>
                               
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                <tr>
                        <td>
                            <center>
                                <div id="print2" runat="server" visible="false">
                                    <asp:Label ID="lblvalidation3" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                    <asp:Label ID="lblrptname2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname2" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                   <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>--%>
                                    <asp:Button ID="btnExcel2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnExcel_Click2" Text="Export To Excel" Width="127px"
                                        Height="32px" CssClass="textbox textbox1" />
                                    <asp:Button ID="btnprintmasterhed2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click2" Height="32px"
                                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrolhed2" Visible="false" />
                                </div>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
    </div>
</asp:Content>
