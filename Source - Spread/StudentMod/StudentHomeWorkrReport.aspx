<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentHomeWorkrReport.aspx.cs" Inherits="StudentMod_StudentHomeWorkrReport" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Student Home Work Report</title>
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 109px;
        }
        .style2
        {
            width: 189px;
        }
        .style3
        {
            width: 419px;
        }
        .style4
        {
            width: 100px;
        }
        .style7
        {
            width: 84px;
        }
        .style9
        {
            width: 83px;
        }
        .style10
        {
            width: 301px;
        }
        .style11
        {
            width: 314px;
        }
        .txt
        {
        }
        .style12
        {
            width: 77px;
        }
        .style13
        {
            width: 326px;
        }
        .style14
        {
            width: 299px;
        }
        .style15
        {
            width: 297px;
        }
        .style16
        {
            width: 309px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
                margin-top: 10px;">Home Work Status Report</span>
            <br />
            <br />
        </div>
    </center>
    <br />
    <center>
        <asp:UpdatePanel ID="Upanel1" runat="server">
            <ContentTemplate>
                <div class="maintablestyle" style="width: 1000px; margin: 0px; margin-bottom: 10px;
                    margin-top: 10px; text-align: left;">
                    <table cellpadding="0px" cellspacing="0px" style="height: 100%; width: 103%; margin: 0px;
                        margin-bottom: 10px; margin-top: 10px;">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblcollege" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                    Width="250px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="25px" Width="60px" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddldegree" Height="24px" Width="88px" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="300px"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblduration" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlduration" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged"
                                    AutoPostBack="True" Height="25px" Width="47px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="25px" Width="49px" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <div style="position: relative;">
                                    <asp:UpdatePanel ID="upbranch" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtsubject" runat="server" ReadOnly="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Font-Bold="true" CssClass="textbox  txtheight2">-- Select --</asp:TextBox>
                                            <asp:Panel ID="psubject" runat="server" CssClass="multxtpanel" Height="100px" Width="250px">
                                                <asp:CheckBox ID="chksubject" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chksubject_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblsubject" runat="server" Font-Size="Medium" Font-Bold="True"
                                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstsubject_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popExtbranch" runat="server" TargetControlID="txtsubject"
                                                PopupControlID="psubject" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                            <td class="style12">
                                <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td class="style9">
                                <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="20px" Width="75px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtFromDate_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="txtFromDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                                    ValidChars="/" runat="server" TargetControlID="txtFromDate">
                                </asp:FilteredTextBoxExtender>
                                <asp:CalendarExtender ID="calfromdate" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td class="style39">
                                <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td class="style7">
                                <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Height="20px" Width="75px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtToDate_TextChanged"
                                    AutoPostBack="True"></asp:TextBox><%-- --%>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom,Numbers"
                                    ValidChars="/" runat="server" TargetControlID="txtToDate">
                                </asp:FilteredTextBoxExtender>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpGo" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" Font-Bold="True" OnClick="btnGo_Click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="width: auto; height: auto;"
                                            CssClass="textbox textbox1" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:UpdatePanel ID="Upanel3" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                            <asp:GridView ID="gview" runat="server" BorderStyle="Double" CssClass="grid-view"
                                GridLines="Both" OnRowCreated="gviewOnRowCreated" ShowFooter="false" ShowHeader="false"
                                OnSelectedIndexChanged="gview_OnSelectedIndexChanged" Font-Names="Book Antique"
                                Style="width: 100%; height: 100px; overflow: auto">
                                <Columns>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                <RowStyle ForeColor="#333333" />
                                <%--<selectedrowstyle backcolor="#339966" font-bold="True" />--%>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="gview" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="Upanel2" runat="server">
            <ContentTemplate>
                <asp:Label ID="errlbl" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Height="20px"></asp:Label>
                <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Height="16px"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <div id="divPopSpread" runat="server" visible="false" style="height: 220em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divPopSpreadContent" runat="server" class="table" style="background-color: White;
                    border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-left: auto;
                    margin-right: auto; width: 970px; height: auto; z-index: 1000; border-radius: 5px;">
                    <asp:ImageButton ID="btnClose" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                        Style="height: 30px; width: 30px; margin-top: -2%; margin-left: 465px; position: absolute;"
                        OnClick="btnclosespread_OnClick" />
                    <center>
                        <center style="height: 30px; font-family: Book Antiqua; font-weight: bold; color: Navy">
                        </center>
                        <asp:GridView ID="GridView1" runat="server" BorderStyle="Double" CssClass="grid-view"
                            GridLines="Both" ShowFooter="false" AutoGenerateColumns="false" Font-Names="Book Antique"
                            Style="width: 60%; height: 100px; overflow: auto">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblsno" runat="server" Text='<%#Container.DisplayIndex+1 %>' /></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Roll No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno" runat="server" Text='<%#Eval("RollNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Name") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                            <RowStyle ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                        </asp:GridView>
                    </center>
                </div>
            </center>
        </div>
        <br />
        <br />
        <asp:UpdatePanel ID="Upanelprint" runat="server">
            <ContentTemplate>
                <center>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnxl_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
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
