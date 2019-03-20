<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CAMRange.aspx.cs" Inherits="CAM" %>
    <%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style20
        {
            top: 277px;
            left: -28px;
            position: absolute;
            width: 997px;
            height: 16px;
        }
        .style21
        {
            height: 79px;
        }
        .style23
        {
            width: 134px;
            height: 20px;
        }
        .style24
        {
            margin-top: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <html>
    <body>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">CAM R4-CAM Subject Range Analysis</span>
        </center>
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Panel ID="Panel1" class="maintablestyle" runat="server" Style="" BorderColor="Black"
            BorderWidth="1px" Height="65px" Width="1008px">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="True" Height="25px"
                            Style="margin-left: 15px;" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="25px"
                            Width="115px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Style="margin-left: 11px;"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Height="25px"
                            OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Width="234px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSem" runat="server" AutoPostBack="True" Height="25px" OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged"
                            Width="44px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                            Height="25px" Width="42px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSubject" runat="server" AutoPostBack="True" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 25px; width: 100px;"
                            OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Test"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddltest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnSelectedIndexChanged="ddltest_SelectedIndexChanged" Style="margin-left: 21px;"
                            AutoPostBack="True" CssClass="style24" Height="25px" Width="115px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged"
                            Text="Range" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:Panel ID="Panel4" runat="server" Visible="False" BorderColor="Black" BorderWidth="1px"
                            Style="padding-left: 7px; margin-top: -3px;" Height="26px" Width="150px">
                            <asp:Label ID="fromlbl" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Small"></asp:Label>
                            &nbsp
                            <asp:TextBox ID="fromtext" runat="server" Height="16px" Width="20px" Style="margin-top: 2px;"
                                MaxLength="3"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="fromtext"
                                FilterType="Numbers" />
                            <asp:Label ID="Tolbl" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Small"></asp:Label>
                            &nbsp;<asp:TextBox ID="Totext" runat="server" Width="20px" Height="16px" OnTextChanged="Totext_TextChanged"
                                MaxLength="3"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Totext"
                                FilterType="Numbers" />
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Text="Print Master Setting"
                            Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnPrint_Click"
                            Width="151px" />
                    </td>
                    <td>
                        <asp:Label ID="lblConvert_Value" runat="server" Text="MarkConversion Value" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txtConvert_Value" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="17px" Width="58px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkonesubject" runat="server" AutoPostBack="true" Text=" Result for individual subject"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkonesubject_checkedchanged" />
                    </td>
                    <td>
                        <asp:Button ID="btgGO" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="Button1_Click" Text="Go" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <table>
            <tr>
                <td colspan="9">
                    <asp:Label ID="lblnofrmto" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="#FF3300" Text="Fill From and To" Visible="False"></asp:Label>
                    <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Text="There are no Records Found" Visible="False">
                    </asp:Label>
                    <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Visible="False"
                        Font-Names="Book Antiqua"></asp:Label>
                    <asp:Label ID="lblrecord" runat="server" Visible="False" Font-Bold="True" Text="     Records Per Page"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                        Visible="False">
                    </asp:DropDownList>
                    <asp:TextBox ID="TextBoxother" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="TextBoxother_TextChanged"
                        Visible="false" Width="34px"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                        FilterType="Numbers" />
                    <asp:Label ID="lblpage" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Page Search" Visible="False"></asp:Label>
                    <asp:TextBox ID="TextBoxpage" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="TextBoxpage_TextChanged"
                        Visible="False" Width="34px"></asp:TextBox>
                    &nbsp;
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="TextBoxpage"
                        FilterType="Numbers" />
                    <asp:Label ID="LabelE" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <asp:RadioButton ID="RadioHeader" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in All Pages" />
                    <asp:RadioButton ID="Radiowithoutheader" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in 1st Page" />
                </td>
            </tr>
        </table>
        <center>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                <asp:GridView ID="gview" runat="server" ShowHeader="false" >
                <Columns>
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#333333" />
                <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Black" />
                <PagerStyle BackColor="#336666"  HorizontalAlign="Center" />
                <RowStyle  ForeColor="#333333" />
                <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <table>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblrptname" runat="server" Width="120px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                        <asp:Button ID="Button1" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                             <NEW:NEWPrintMater runat="server" ID="Printcontrol1" Visible="false" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </td>
                </tr>
            </table>
        </center>
    </body>
    </html>
</asp:Content>
