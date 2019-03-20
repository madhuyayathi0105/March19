<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="DummyNumberReport.aspx.cs" Inherits="DummyNumberReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    function display() {
        document.getElementById('MainContent_lblvalidation1').innerHTML = "";
    }
    </script>
    <style type="text/css">
        .font
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <br />
   <center>
        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green" Text="Dummy Number Report"></asp:Label></center>
        
    <br />
   <center>
            <table style="width:700px; height:70px; background-color:#0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Style=" font-family: 'Book Antiqua';" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                    <div style="position:relative">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" CssClass="dropdown"
                                    Width="120px" Style=" font-family: 'Book Antiqua';"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">--Select--</asp:TextBox>
                                <asp:Panel ID="pdegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua'; overflow-y: scroll;">
                                    <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                    PopupControlID="pdegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel></div>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                    <div style="position:relative">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Width="120px" Style=" font-family: 'Book Antiqua';"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">--Select--</asp:TextBox>
                                <asp:Panel ID="pbranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua'; overflow-y: scroll;">
                                    <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Height="58px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                                    PopupControlID="pbranch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel></div>
                    </td>
                    <td>
                        <asp:Label ID="lblMonthandYear" runat="server" CssClass="font" Text="Month & Year" Width="125px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="font" Width="100px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="font" Width="100px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Lbldate" runat="server" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" Width="100px" OnSelectedIndexChanged="ddldate_SelectedIndexChanged"
                            TabIndex="3">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblsession" runat="server" Text="Session" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsession" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" Width="50px" OnSelectedIndexChanged="ddlsession_SelectedIndexChanged">
                            <asp:ListItem>F.N</asp:ListItem>
                            <asp:ListItem>A.N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsubject" runat="server" Text="Subject" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsubject" runat="server" CssClass="font" AutoPostBack="True"
                            Width="300px" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged" TabIndex="4">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbserial" runat="server" OnCheckedChanged="rdbserial_Change"
                            AutoPostBack="true" CssClass="font" Text="Serial" GroupName="Same" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbRandam" runat="server" CssClass="font" OnCheckedChanged="rdbRandam_Change"
                            AutoPostBack="true" Text="Random" GroupName="Same" />
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" CssClass="font" Text="Go" OnClick="btngo_Click" />
                    </td>
                </tr>
            </table>
        </center>
    
    
    <br />
    <asp:Label ID="errorlable" runat="server" ForeColor="Red" Visible="false" CssClass="font"></asp:Label>
    <br />
    <div>
        <center>
            <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Width="800px" CommandBar-Visible="false">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </center>
    </div>
    <br />
    <div id="rptprint" runat="server" style="margin-left: 152px;" visible="false">
        <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
        <br />
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
            OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <asp:Button ID="btndummynoprint" runat="server" Text="Dummy Number Print" OnClick="btndummynoprint_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
    </div>
</asp:Content>

