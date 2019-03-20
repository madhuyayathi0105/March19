<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master" AutoEventWireup="true" CodeFile="bonafide1.aspx.cs" Inherits="bonafidekongu" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    function flg() {

        document.getElementById('<%=btnaddr.ClientID%>').style.display = 'block';
        document.getElementById('<%=btndelr.ClientID%>').style.display = 'block';
    }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager><br />
       <center>
            <asp:Label ID="lbl_Header" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="Green" Text="Bonafide"></asp:Label></center>
        <br />
       
            <table style="width:1000px; height:110px; background-color:#0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="lblclg" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlclg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px" OnSelectedIndexChanged="ddlclg_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="116px" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbldeg" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldeg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Width="116px"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged" Width="100px"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsem" runat="server" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                    
                    </td>                   
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblsec" runat="server" Text="Section" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbltype" runat="server" Text="Type" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddltype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                            <asp:ListItem>Day Scholar</asp:ListItem>
                            <asp:ListItem>Hostler</asp:ListItem>
                            <asp:ListItem>Both</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbonafide" runat="server" Text="Bonafide Type" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbonafide" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="116px" OnSelectedIndexChanged="ddlbonafide_change"
                            AutoPostBack="true">
                            <asp:ListItem>General</asp:ListItem>
                            <asp:ListItem>Passport</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblreason" runat="server" Text="Reason" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                         <asp:Button ID="btnaddr" runat="server" Text="+" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnaddr_click" Style="display: none;" />   
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlreason" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btndelr" runat="server" Text="-" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btndelr_click" Style="display:none;"/>
                    </td>
                    <td>
                   <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btngo_click"/>
                    </td>
                </tr>
            </table>
        <asp:Panel ID="Panel9" runat="server" Visible="false" Style="border-color: Black; border-style: solid; border-width: 0.5px; width: 303px; height: 75px; left: 699px; position: absolute;">
            <asp:TextBox ID="txtadd" runat="server" Width="273px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"></asp:TextBox>
            <asp:Button ID="btnadd1" runat="server" Text="Add" OnClick="btnadd1_Click" Style="height: 26px; width: 88px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"/>
            <asp:Button ID="btnexit1" runat="server" Text="Exit" OnClick="btnexit1_Click" Style=" height: 26px; width: 88px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
        </asp:Panel>
        <asp:Label ID="lblerr" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="false"></asp:Label>
            <br />
        <table align="center">
            <tr>
                <td>
                    <FarPoint:FpSpread ID="Fpspread" runat="server" OnUpdateCommand="Fpspread_UpdateCommand">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" AutoPostBack="false" GridLineColor="Black">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btngenerate" runat="server" Text="Generate" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btngenerate_click" />
                </td>
            </tr>
        </table>
    </body>
</asp:Content>

