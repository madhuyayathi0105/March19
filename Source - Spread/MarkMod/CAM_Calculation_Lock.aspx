<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CAM_Calculation_Lock.aspx.cs" Inherits="CAM_Calculation_Lock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .sty
        {
            font-size: medium;
            font-family: Book Antiqua;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">CAM Calculation Lock</span>
        </center>
        <br />
        <table class="maintablestyle" style="text-align: left;">
            <tr>
                <td class="style1">
                    <asp:Label ID="Label1" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td class="style1">
                    <asp:DropDownList ID="ddlcollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="120px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="style1">
                    <asp:Label ID="lblbach" runat="server" Text="Batch" Style="font-family: 'Book Antiqua';"
                        Font-Bold="True" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td class="style1">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div style="position: relative;">
                                <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Width="120px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbatch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                    PopupControlID="pbatch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="style1">
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Style="font-family: 'Book Antiqua';"
                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td class="style1">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div style="position: relative;">
                                <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" CssClass="dropdown"
                                    Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
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
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="style1">
                    <asp:Label ID="lblbranch" runat="server" Style="font-family: 'Book Antiqua';" Text="Branch"
                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td class="style2">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">---Select---</asp:TextBox>
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
                    </asp:UpdatePanel>
                </td>
                <td class="style1">
                    <asp:Button ID="btngo" runat="server" Height="30px" CssClass="dropdown" Text="Go"
                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Style="" OnClick="btngo_Click" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="errmsg" runat="server" ForeColor="Red" Visible="false"></asp:Label>
    </center>
    <br />
    <div>
    <center>
    <asp:GridView ID="Gridview1" runat="server" style="margin-top:15px; margin-bottom:15px;width:auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" OnRowDataBound="gridview1_OnRowDataBound" OnDataBound="gridview1_DataBound" BackColor="AliceBlue"  >
    <Columns>
    <asp:TemplateField HeaderText="S.No">
    <ItemTemplate>
    <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
    </ItemTemplate>
    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Batch">
    <ItemTemplate>
    <asp:Label ID="lblbatch" runat="server" Text='<%#Eval("batch") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Degree">
    <ItemTemplate>
     
    <asp:Label ID="lbldegree" runat="server" Text='<%#Eval("degree") %>'></asp:Label>
    </ItemTemplate>
     <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Department">
    <ItemTemplate>
    <asp:Label ID="lbldegcode" runat="server" Text='<%#Eval("degree_code") %>' Visible="false"></asp:Label>
    <asp:Label ID="lbldepartment" runat="server" Text='<%#Eval("department") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Sem">
    <ItemTemplate>
    <asp:Label ID="lblsem" runat="server" Text='<%#Eval("Sem") %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
    </asp:TemplateField>
      <asp:TemplateField HeaderText="Day">
    <ItemTemplate>
    <asp:Label ID="lblday" runat="server" Text='<%#Eval("day") %>' Visible="false" ></asp:Label>
    <asp:DropDownList ID="ddlday" runat="server" OnSelectedIndexChanged="ddlday_OnSelectedIndexChanged"></asp:DropDownList>
    </ItemTemplate>
    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Month">
    <ItemTemplate>
    <asp:Label ID="lblmonth" runat="server" Text='<%#Eval("month") %>' Visible="false" ></asp:Label>
    <asp:DropDownList ID="ddlmonth" runat="server"  OnSelectedIndexChanged="ddlmonth_OnSelectedIndexChanged"></asp:DropDownList>
    </ItemTemplate>
    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Year">
    <ItemTemplate>
    <asp:Label ID="lblyear" runat="server" Text='<%#Eval("year") %>' Visible="false"></asp:Label>
    <asp:DropDownList ID="ddlyear" runat="server"   OnSelectedIndexChanged="ddlyear_OnSelectedIndexChanged"></asp:DropDownList>
    </ItemTemplate>
    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
    </asp:TemplateField>
    </Columns>
    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />    
    </asp:GridView>
    </center>
    </div>
    <br />
    <div>
        <center>
            <asp:Button ID="btnsave" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                Font-Bold="true" Text="Save" Visible="false" OnClick="btnsave_Click" />
            <asp:Button ID="btnreset" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                Font-Bold="true" Text="Reset" Visible="false" OnClick="btnreset_Click" />
        </center>
    </div>

     <center>
        <div id="divpopalter" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divpopaltercontent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblaltermsgs" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                            <td align="center">
                                        <asp:Button ID="btnokcl" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnokclk_Click"
                                            Text="OK" runat="server" />
                                </td>
                               
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
