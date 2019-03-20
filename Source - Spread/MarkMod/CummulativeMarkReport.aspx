<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master" AutoEventWireup="true" CodeFile="CummulativeMarkReport.aspx.cs" Inherits="CummulativeMarkReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript">
     function display() {
         document.getElementById('MainContent_lblvalidation').innerHTML = "";
     }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager><br />
    <center>
         <asp:Label ID="lbl" runat="server" Text="Cumulative Mark & Grade Report" Font-Bold="true"
                    Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
    </center><br />
   <center>
            <table style="width:1000px; height:90px; background-color:#0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="lblschool" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="School"></asp:Label>
                            </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddschool" runat="server" Width="246px" Height="25px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddschool_OnSelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblyear" runat="server" Font-Bold="True" Font-Names="Book Antiqua" 
                            Font-Size="Medium" Text="Year"></asp:Label></td>
                    <td>
                        <asp:DropDownList ID="dropyear" runat="server" Width="59px" Height="25px" Font-Bold="True"
                            OnSelectedIndexChanged="dropyear_SelectedIndexChanged" Font-Names="Book Antiqua"
                           Font-Size="Medium" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblschooltype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"  Font-Size="Medium" Text="School Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddschooltype" runat="server" Width="75px" Height="25px" AutoPostBack="true"
                            OnSelectedIndexChanged="dropschooltype_SelectedIndexChanged" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblstandard" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Standard" ></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddstandard" runat="server" Width="120px" Height="25px" AutoPostBack="true"
                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddstandard_SelectedIndexChanged" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                   
                   
                </tr>
                <tr>
                 <td>
                        <asp:Label ID="lblterm" runat="server" Font-Bold="True" Font-Names="Book Antiqua"  Font-Size="Medium" Text="Term"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="dropterm" runat="server" Width="35px" Height="25px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="dropterm_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Sec" ></asp:Label>
                        </td>
                    <td>
                        <asp:DropDownList ID="dropsec" runat="server" Width="44px" Height="25px"  Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" >
                        </asp:DropDownList>
                    </td>
                    <td>
                       <asp:Label ID="Label2" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Text="From Date" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfromdate" runat="server" Width="80px" AutoPostBack="True"  Font-Bold="True" OnTextChanged="txtfromdate_TextChanged"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="txtfromdate"
                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','/'" />
                        <asp:CalendarExtender ID="CalendarExtender22" runat="server" Format="d/MM/yyyy" TargetControlID="txtfromdate">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Text="To Date" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txttodate" runat="server" Width="80px" AutoPostBack="True"  OnTextChanged="txttodate_TextChanged" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" TargetControlID="txttodate"
                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','/'" />
                        <asp:CalendarExtender ID="CalendarExtender23" runat="server" Format="d/MM/yyyy" TargetControlID="txttodate">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua"  Font-Size="Medium" ForeColor="Black" OnClientClick="return validation()"
                            Text="Go" OnClick="btngo_Click" />
                    </td>
                </tr>
            </table>
        </center>
    <br />
    <asp:Label ID="lblerrormsg" runat="server" Text="" Width="302px" Style="position: absolute;
        left: 15px; top: 277px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
        Visible="true" ForeColor="#FF3300"></asp:Label>
    <br />
    <asp:GridView ID="reportgrid1" Visible="false" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
        HeaderStyle-BackColor="Teal" AlternatingRowStyle-CssClass="gvAltRow" HeaderStyle-CssClass="gvHeader"
        OnRowDataBound="reportgrid1_RowDataBound" OnDataBound="reportgrid1_DataBound"
        Style="margin-top: -4px; width: 945px; margin-left: -28px;">
    </asp:GridView>
    <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderWidth="2px">
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread>
    <table>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblvalidation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="Red" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name" Visible="false"></asp:Label>
                <asp:TextBox ID="txt_excel" runat="server" Width="120px" Visible="false" Height="25px"
                    Font-Bold="true" Font-Size="Medium" onkeypress="display()" Font-Names="Book Antiqua"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_excel"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+\}{][':;?,.">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="g1btnexcel" runat="server" OnClick="g1btnexcel_OnClick" Visible="false"
                    Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
            </td>
            <td>
                <asp:Button ID="g1btnprint" runat="server" OnClick="g1btnprint_OnClick" Visible="false"
                    Text="Print" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>

