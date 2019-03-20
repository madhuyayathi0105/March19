<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="performance_analysis.aspx.cs" Inherits="performance_analysis"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lbl_err').innerHTML = "";
        }
    </script>
    <style type="text/css">
        .head
        {
            background-color: Teal;
            font-family: Book Antiqua;
            font-size: medium;
            color: black;
            top: 165px;
            position: absolute;
            font-weight: bold;
            width: 950px;
            height: 25px;
            left: 15px;
        }
        .mainbatch
        {
            background-color: #3AAB97;
            width: 950px;
            position: absolute;
            height: 50px;
            top: 190px;
            left: 15px;
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: black;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="lbl_head" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
            ForeColor="Green" Font-Size="Large" Text="Performance Analysis of Internal Assessment"></asp:Label></center>
    <body>
        <br />
        <center>
            <table style="width: 700px; height: 70px; background-color: #0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="Iblbatch" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                            runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="updpan_batch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_batch" CssClass="Dropdown_Txt_Box" Font-Size="Medium" Font-Names="Book Antiqua"
                                        Font-Bold="true" Width="100px" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        CssClass="multxtpanel" Width="114px" Font-Size="Medium" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="125px">
                                        <asp:CheckBox ID="Chk_batch" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="Chlk_batchchanged" />
                                        <asp:CheckBoxList ID="Chklst_batch" Font-Bold="true" Font-Size="Medium" runat="server"
                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="Chlk_batchselected">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupbatch" runat="server" TargetControlID="txt_batch"
                                        PopupControlID="pbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="Ibldegree" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                            Font-Size="Medium" Text="Degree"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="updpan_degree" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                                        Font-Bold="true" Font-Size="Medium" runat="server" ReadOnly="true" Width="126px">--Select--</asp:TextBox>
                                    <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Width="128px" Font-Bold="true"
                                        Font-Size="Medium" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" ScrollBars="Vertical" Height="125px">
                                        <asp:CheckBox ID="chk_degree" Font-Bold="true" runat="server" Font-Size="Medium"
                                            Text="Select All" AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="checkDegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="Chklst_degree" Font-Bold="true" Font-Size="Medium" runat="server"
                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cheklist_Degree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupdegree" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="pdegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="Iblbranch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                            Font-Size="Medium" Text="Branch"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="updpan_branch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_branch" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                        runat="server" ReadOnly="true" Width="125px" Font-Size="Medium">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Width="400px" Font-Names="Book Antiqua"
                                        Font-Bold="true" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                        ScrollBars="Vertical" Height="125px">
                                        <asp:CheckBox ID="chk_branch" runat="server" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" Text="Select All" OnCheckedChanged="chk_branchchanged"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklst_branch" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                            runat="server" OnSelectedIndexChanged="chklst_branchselected" AutoPostBack="True">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupbranch" runat="server" TargetControlID="txt_branch"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblTestname" runat="server" Text="Test Name " font-name="Book Antiqua"
                            Font-Size="Medium" Width="100px" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txttest" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="True"
                                        ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="---Select---"
                                        Width="110px"></asp:TextBox>
                                    <asp:Panel ID="pnltest" runat="server" CssClass="multxtpanel" Height="150px" Width="200px"
                                        BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                                        <asp:CheckBox ID="chktest" runat="server" Text="SelectAll" AutoPostBack="True" Font-Bold="True"
                                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chktest_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="chklsttest" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsttest_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <br />
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txttest"
                                        PopupControlID="pnltest" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <asp:UpdatePanel ID="UpdatePanelgo" runat="server">
                        <ContentTemplate>
                            <td>
                                <asp:Button ID="btngo" runat="server" Font-Names="Book Antiqua" Text="Go" OnClick="btngo1"
                                    Font-Size="Medium" Font-Bold="true" />
                            </td>
                        </ContentTemplate>
                    </asp:UpdatePanel> 
                </tr>
            </table>
        </center>
        <br />
        <asp:Label ID="lbl_err" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium"
            Font-Names="Book Antiqua"></asp:Label>
        <br />
        <center>
            <asp:GridView ID="gridperfomance" ShowHeader="false" runat="server" AutoGenerateColumns="true" Width="800px"
                ShowHeaderWhenEmpty="true" Font-Names="book antiqua" OnRowDataBound="OnRowDataBound">
                <Columns>
                </Columns>
                <HeaderStyle BackColor="#008080" ForeColor="White" />
            </asp:GridView>
            <br />
            <asp:Chart ID="Chart1" runat="server" Width="1600px" Visible="true">
                <Series>
                    <asp:Series Name="Series1" IsValueShownAsLabel="true" ChartArea="ChartArea1" ChartType="Column">
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                        <AxisY LineColor="White">
                            <LabelStyle Font="Trebuchet MS, 8.25pt" />
                            <MajorGrid LineColor="#e6e6e6" />
                            <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                        </AxisY>
                        <AxisX LineColor="White">
                            <LabelStyle Font="Trebuchet MS, 8.25pt" />
                            <MajorGrid LineColor="#e6e6e6" />
                            <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                        </AxisX>
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
            <br />
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+|\}{][':;?><,./">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnxl_Click" />
            <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnmasterprint_Click" />
            <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
        </center>
            <%--progressBar for GO--%>
        <center>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelgo">
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
