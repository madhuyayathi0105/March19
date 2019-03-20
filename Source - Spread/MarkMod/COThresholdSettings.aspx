<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="COThresholdSettings.aspx.cs" Inherits="COThresholdSettings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
     <style type="text/css">
        .GridDock
        {
            overflow-x: auto;
            overflow-y: hidden;
            height:366px;;
            padding: 0 0 0 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <style type="text/css">
        .style1
        {
            width: 890px;
            background-color: Teal;
        }
        .style2
        {
            width: 818px;
        }
        .style3
        {
            width: 150px;
        }
        .cursorptr
        {
            cursor: pointer;
        }
    </style>
    <br />
    <center>
        <asp:Label ID="lblhead" runat="server" Text="Subjectwise CO Threshold Setting and Avg Report"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
    <br />
    <center>
        <table class="maintablestyle" style="width: 700px; height: 44px; background-color: #0CA6CA;">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="21px" Width="180px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="21px" Width="69px" AutoPostBack="True" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="21px" Width="127px" AutoPostBack="True" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="21px" Width="187px" AutoPostBack="True" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="21px" Width="55px" AutoPostBack="True" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="21px" Width="187px" AutoPostBack="True" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="btngo" runat="server" Text="GO" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnClick="GO_Click" />
                                        <asp:Button ID="btnSaveSettings" runat="server" Text="Save" CssClass="textbox  btn2"
                                            Width="60px" BackColor="#81C13F" ForeColor="White" OnClick="btnSaveSettings_OnClick" />
                                    </td>
                                </tr>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    <br />
    <center>
    <div id="divspreadpopup" runat="server" visible="false" class="GridDock" style="width:1300px; margin-top:15px;">
    <table ><tr align="center"><td align="center" >
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
            HeaderStyle-BackColor="#0CA6CA" BackColor="White" OnRowDataBound="RowDataBound"
            OnDataBound="OnDataBound" Width="1300px">
            <%-- OnDataBound="gridTimeTable_OnDataBound"--%>
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DisplayIndex+1 %>" />
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CO Name">
                    <ItemTemplate>
                        <asp:Label ID="lblCoName" runat="server" Text='<%#Eval("template") %>'></asp:Label>
                        <asp:Label ID="CoId" runat="server" Text='<%# Eval("masterno") %>' Visible="false" />
                    </ItemTemplate>
                    <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#F8B7B3" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Descriptions">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" style="width: 246px; height: 28px;"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle Height="10px" Width="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Knowledge">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlknw" runat="server" Width="120px">
                        </asp:DropDownList>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Target">
                    <ItemTemplate>
                        <asp:TextBox ID="txtTarget" runat="server" Width="80px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtTarget"
                            FilterType="numbers,custom" ValidChars=".">
                        </asp:FilteredTextBoxExtender>
                    </ItemTemplate>
                    <ItemStyle Width="80px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Threshold">
                    <ItemTemplate>
                        <asp:TextBox ID="txtThreshold" runat="server"  Width="80px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtThreshold"
                            FilterType="numbers,custom" ValidChars=".">
                        </asp:FilteredTextBoxExtender>
                    </ItemTemplate>
                    <ItemStyle Width="80px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                  <asp:TemplateField HeaderText=" Average Attainment %">
                    <ItemTemplate>
                        <asp:Label ID="lblAvgAtt" runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Attainment % based on Threshold">
                    <ItemTemplate>
                        <asp:Label ID="lblAtt" runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" Relative attainment % based on Avg Attainment">
                    <ItemTemplate>
                        <asp:Label ID="lblRelAvgAtt" runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PO1" >
                <ItemTemplate>
                 <asp:DropDownList ID="ddlpo1" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO2" >
                <ItemTemplate>
                  <asp:DropDownList ID="ddlpo2" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO3" >
                <ItemTemplate>
                  <asp:DropDownList ID="ddlpo3" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO4" >
                <ItemTemplate>
                  <asp:DropDownList ID="ddlpo4" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO5">
                <ItemTemplate>
                  <asp:DropDownList ID="ddlpo5" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO6">
                <ItemTemplate>
                 <asp:DropDownList ID="ddlpo6" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO7">
                <ItemTemplate>
                  <asp:DropDownList ID="ddlpo7" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO8">
                <ItemTemplate>
                  <asp:DropDownList ID="ddlpo8" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO9">
                <ItemTemplate>
                  <asp:DropDownList ID="ddlpo9" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO10">
                <ItemTemplate>
                  <asp:DropDownList ID="ddlpo10" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO11" >
                <ItemTemplate>
                  <asp:DropDownList ID="ddlpo11" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO12" >
                <ItemTemplate>
                  <asp:DropDownList ID="ddlpo12" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO13" >
                <ItemTemplate>
                 <asp:DropDownList ID="ddlpo13" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO14">
                <ItemTemplate>
                  <asp:DropDownList ID="ddlpo14" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PO15">
                <ItemTemplate>
                 <asp:DropDownList ID="ddlpo15" runat="server" Width="120px"></asp:DropDownList>
                </ItemTemplate>
                <ItemStyle Width="80" HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </td></tr></table>
        </div>
    </center>
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
                                    <asp:Label ID="lblaltermsgs" runat="server" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
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
