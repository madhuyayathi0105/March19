<%@ Page Title="Internal Marks Updation" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="COEInternalMarksUpdate.aspx.cs" Inherits="COEInternalMarksUpdate" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .Header
        {
            font-weight: bold;
            text-align: center;
            font-size: 22px;
            color: Green;
            margin-top: 20px;
            margin-bottom: 20px;
            line-height: 3em;
        }
        .nv
        {
            text-transform: uppercase;
        }
        .noresize
        {
            resize: none;
        }
        .fontCommon
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: #000000;
        }
        .defaultHeight
        {
            width: auto;
            height: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="contentBody" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div style="width: 100%; height: auto;">
            <table>
                <thead>
                    <tr>
                        <td colspan="3">
                            <center>
                                <span class="Header">Internal Marks Updation</span>
                            </center>
                        </td>
                    </tr>
                </thead>
            </table>
            <center>
                <div class="maindivstyle" style="width: 100%; height: auto; width: -moz-max-content;">
                    <table class="maintablestyle" style="margin: 10px; width: auto; height: auto;">
                        <tbody>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSearchBy" runat="server" Text="Search By" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchBy" runat="server" OnSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true">
                                        <asp:ListItem Selected="True" Text="Reg.No" Value="0"></asp:ListItem>
                                        <asp:ListItem Selected="False" Text="Roll.No" Value="1"></asp:ListItem>
                                        <asp:ListItem Selected="False" Text="Admission No" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSearch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Reg. No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text=""></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblExamYear" runat="server" Text="Exam Year" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlExamYear" runat="server" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblExamMonth" runat="server" Text="Exam Month" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlExamMonth" runat="server" OnSelectedIndexChanged="ddlExamMonth_SelectedIndexChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnGo" runat="server" Visible="true" Width="44px" Height="26px" CssClass="textbox textbox1"
                                        Text="Go" Font-Bold="true" OnClick="btnGo_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <div id="divMainGrid" runat="server" visible="false" style="margin: 10px;">
                        <asp:GridView ID="gvInternalMarks" Visible="false" runat="server" AutoGenerateColumns="false"
                            GridLines="Both" Width="100px">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblSno" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Width="60px" Text='<%#Eval("Sno") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subject Code" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubjectCode" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Width="160px"></asp:Label>
                                        <%--Text='<%#Eval("Unit") %>' Text='<%#Eval("Topic_No") %>'--%>
                                        <asp:Label ID="lblSubjectNo" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Visible="false" Width="160px"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="85px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subject Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                        </center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="285px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Internal Mark" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                        </center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="65px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <FarPoint:FpSpread ID="FpSpreadInternalMarks" runat="server" BorderColor="Black"
                            BorderStyle="Solid" ShowHeaderSelection="false" EnableClientScript="true" BorderWidth="1px" Visible="true"
                            VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never" CssClass="stylefp">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                                    GroupBarText="Drag a column to group by that column." SelectionBackColor="#AE8D5A"
                                    SelectionForeColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <center>
                            <div id="divPrint" runat="server" visible="false" style="margin: 20px;">
                                <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                    Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                                    Height="35px" CssClass="textbox textbox1" />
                                <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                                    CssClass="textbox textbox1" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                                <asp:Button ID="btnSave" runat="server" Visible="true" Width="60px" Height="35px"
                                    CssClass="textbox textbox1" Text="Save" Font-Bold="true" OnClick="btnSave_Click" />
                                <%--<asp:Button ID="btnCondonationReport" runat="server" Text="Save Condonation" OnClick="btnCondonationReport_Click"
                        Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true"
                        Height="35px" CssClass="textbox textbox1" />--%>
                            </div>
                        </center>
                        <%--<div id="divPrint" runat="server" visible="false">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>--%>
                    </div>
                    <div id="divPopAlert" runat="server" visible="false" style="height: 400em; z-index: 2000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 200px; border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnPopAlertClose" CssClass="textbox textbox1" Style="height: 28px;
                                                        width: 65px;" OnClick="btnPopAlertClose_Click" Text="Ok" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
