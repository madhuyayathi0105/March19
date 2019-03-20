<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master" AutoEventWireup="true" CodeFile="logindetails.aspx.cs" Inherits="logindetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager><br />
    <center>
        <asp:Label ID="lbl" runat="server" Text="Login Usage Report" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Large" ForeColor="Green"></asp:Label>    
            </center>
            <br />
             <center>
            <table  style="width:900px; height:80px; background-color:#0CA6CA;">
                <tr>
                    <td>
                     <asp:Label ID="lblloginuser" runat="server" Text="Login User" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Large"/>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="--Select--"
                    Font-Size="Medium" OnTextChanged="TextBox1_TextChanged" CssClass="Dropdown_Txt_Box"
                    Style="height: 22px; width: 100px;"></asp:TextBox>
                <asp:Panel ID="pnlCustomers" runat="server" CssClass="MultipleSelectionDDL" Height="113px" Width="105" BackColor="White">
                    <asp:CheckBox ID="SelectAll" runat="server" AutoPostBack="True" Font-Bold="True" Visible="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="SelectAll_CheckedChanged"
                        Text="Select All" />
                    <asp:CheckBoxList ID="ddluser" runat="server" OnSelectedIndexChanged="ddluser_SelectedIndexChanged"
                        AutoPostBack="true" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium">
                        <asp:ListItem Value="0">Admin</asp:ListItem>
                        <asp:ListItem Value="1">Staff</asp:ListItem>
                        <asp:ListItem Value="2">Student</asp:ListItem>                        
                       
                    </asp:CheckBoxList>
                </asp:Panel>
                <br />
                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="TextBox1"
                    PopupControlID="pnlCustomers" Position="Bottom">
                </asp:PopupControlExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
                        <asp:DropDownList ID="lgn_usr" runat="server" Visible="false" CssClass="cmn drp">
                        </asp:DropDownList>
                    </td>
                    <td>
                    <asp:Label ID="lblfromdate" runat="server" Text="From Date" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Large" />
                    </td>
                    <td>
                        <asp:TextBox ID="tbstart_date" runat="server" AutoPostBack="true" OnTextChanged="tbstart_date_OnTextChanged"
                            CssClass="cmn drp"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="tbstart_date">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                       <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Large"/>
                    </td>
                    <td>
                        <asp:TextBox ID="tbend_date" runat="server" CssClass="cmn drp" AutoPostBack="true"
                            OnTextChanged="tbend_date_OnTextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="tbend_date">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="btn_go" runat="server" OnClick="btn_go_click" CssClass="cmn btn" Text="Go" />
                    </td>
                </tr>
            </table>
        </center>
    <br />
    <center> <%-- center added by poo--%>
   <asp:Label ID="lblerr" runat="server"   Font-Bold="True"  ForeColor="Red"
            Font-Names="Book Antiqua" Font-Size="Medium" Text="" ></asp:Label>
            </center>
    <center>
        <div>
          <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False"  Width="500px"
                CellPadding="3"  OnRowDataBound="rowbound" OnRowCommand="change"  OnDataBound="bindboundgv3"
                BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <asp:Label ID="lblsno" runat="server" Text='<%#Eval("SNo")%>' ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="User Name">
                        <ItemTemplate>
                            <asp:Label ID="user" runat="server"  Text='<%#Eval("User Name")%>'></asp:Label>
                            <asp:Label ID="code" runat="server" Visible="false" Text='<%#Eval("staff_code")%>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Last Login Date">
                        <ItemTemplate>
                            <asp:Label ID="lbldate" runat="server" Text='<%#Eval("Login Date")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Login Used Count">
                        <ItemTemplate>
                            <asp:Label ID="lblcount" runat="server" Text='<%#Eval("Login Used Count")%>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"  CssClass="cmn"  />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <RowStyle ForeColor="#000066" CssClass="cmn" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#00547E" />
            </asp:GridView>
            <br />
            <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False"  
                CellPadding="3" OnDataBound="bindbound" BackColor="White" Width="500px"  style=" line-height:20px;"
                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <asp:Label ID="gv4sno" runat="server" Text='<%#Eval("SNo")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="gv4dt" runat="server" Text='<%#Eval("Date")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Time">
                        <ItemTemplate>
                            <asp:Label ID="gv4time" runat="server" Text='<%#Eval("Time")%>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  VerticalAlign="Middle"  HorizontalAlign="Center"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Count">
                        <ItemTemplate>
                            <asp:Label ID="gv4count" runat="server" Text='<%#Eval("Count")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" CssClass="cmn" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <RowStyle ForeColor="#000066" CssClass="cmn" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#00547E" />
            </asp:GridView>
        </div>
    </center>
    <style>
        .cmn
        {
            font-family: 'Book Antiqua';
            font-weight: bold;
            font-size: medium;
             width:90px;
        }
        .lbl
        {
            color: White;
             margin-left: 20px;
        }
        .drp .btn
        {
            width: 120px;
        }
    </style>
</asp:Content>

