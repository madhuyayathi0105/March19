<%@ Page Title="Student Rank & Topper List" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="student_rank_topperlist.aspx.cs" Inherits="student_rank_topperlist" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function checkvalidate() {
            var checkvalidation = document.getElementById('<%=txtbatch.ClientID%>').value;
            var checkvalidation1 = document.getElementById('<%=txtdegree.ClientID%>').value;
            var checkvalidation2 = document.getElementById('<%=txtbranch.ClientID%>').value;
            if (checkvalidation == "---Select---") {
                alert("Please Select Batch");
                return false;
            }
            else if (checkvalidation1 == "---Select---") {
                alert("Please Select Degree");
                return false;
            }
            else if (checkvalidation2 == "---Select---") {
                alert("Please Select Branch");
                return false;
            }
            else {
                return true;
            }
        }
        function display() {
            document.getElementById('MainContent_errmsg').innerHTML = "";
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 152px;
        }
        .style2
        {
            width: 100px;
        }
        .style4
        {
            width: 25px;
        }
        .style6
        {
            width: 133px;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            top: 50px;
            left: 150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="Label5" CssClass="fontstyleheader" runat="server" Font-Bold="True"
            ForeColor="Green" Text="Student Rank & Topper List" Style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;"></asp:Label>
        <table style="width: 700px; height: auto; padding: 5px; background-color: #0CA6CA;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="coll" runat="server" Text="College" font-name="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="true" ForeColor="Black" with="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlclg" runat="server" AutoPostBack="true" Width="240px" Font-Names="Book Antiqua"
                                    Font-Bold="true" OnSelectedIndexChanged="ddlclg_click" Font-Size="Medium" Height="25px ">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbach" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <div style="position: relative">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Style="height: 20px; width: 80px">---Select---</asp:TextBox>
                                            <asp:Panel ID="pbatch" runat="server" CssClass="MultipleSelectionDDL" Width="125px"
                                                Height="300px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                ScrollBars="Vertical">
                                                <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Width="100px" Height="200px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                                PopupControlID="pbatch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="chklsbatch" />
                                        </Triggers>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="chkbatch" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <div style="position: relative">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtdegree" runat="server" CssClass="Dropdown_Txt_Box" Style="font-family: 'Book Antiqua'"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="20px" ReadOnly="true"
                                                Width="80px">---Select---</asp:TextBox>
                                            <asp:Panel ID="pdegree" runat="server" CssClass="MultipleSelectionDDL" Width="150px"
                                                Height="300px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                ScrollBars="Vertical">
                                                <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                                <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Width="100px" Height="200px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                                PopupControlID="pdegree" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="chklstdegree" />
                                        </Triggers>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="chkdegree" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <div style="position: relative">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pbranch" runat="server" Width="400px" Height="400px" CssClass="MultipleSelectionDDL"
                                                BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
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
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="chklstbranch" />
                                        </Triggers>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="chkbranch" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="rdsem" runat="server" AutoPostBack="true" OnCheckedChanged="rdsem_CheckedChanged"
                                    GroupName="top" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Text="Sem Wise Topper" Width="162px" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdover" runat="server" AutoPostBack="true" OnCheckedChanged="rrdover_CheckedChanged"
                                    Font-Bold="true" GroupName="top" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Text="Over All Topper" Width="162px" />
                            </td>
                            <td>
                                <asp:Label ID="lblyear" runat="server" Text="Exam Year" Font-Bold="True" ForeColor="Black"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlyear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="25px" Width="61px" AutoPostBack="True" OnSelectedIndexChanged="ddlyear_Selectedindex">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblmonth" runat="server" Text="Exam Month" Font-Bold="True" ForeColor="Black"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlmonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="25px" Width="61px" AutoPostBack="True" OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbtoporbelow" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbtoporbelow_selectedindexchanged"
                                    Font-Size="Medium" Width="150px">
                                    <asp:ListItem Selected="True" Value="0">Top</asp:ListItem>
                                    <asp:ListItem Value="1">Below</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Top" Font-Bold="True" ForeColor="Black"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txttop" Font-Bold="true" runat="server" Width="50px" Font-Names="Book Antiqua"
                                    Font-Size="Medium" MaxLength="3"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FTX2" runat="server" TargetControlID="txttop" FilterType="Numbers" />
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" Text="Go" Width="50px" Height="28px" Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btngo_Click" OnClientClick="return checkvalidate()" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    <table style="text-align: left; margin: 0px; margin-bottom: 10px; margin-top: 10px;
        position: relative;">
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Small" Style="margin-left: 0px; top: 210px;
                    left: -4px;"></asp:Label>
                <asp:Label ID="lblother" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblnorec" runat="server" Text="No Records Found" ForeColor="Red" Font-Bold="True"
        Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px; margin-bottom: 10px;
        margin-top: 10px; position: relative;"></asp:Label>
    <center>
        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="1px" Width="1052" OnButtonCommand="FpSpread1_ButtonCommand" Style="margin: 0px;
            margin-bottom: 10px; margin-top: 10px; position: relative;">
            <CommandBar BackColor="" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark" ButtonType="PushButton">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="True">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
        <br />
        <asp:Label ID="lblreptname" runat="server" Text="Report Name" font-name="Book Antiqua"
            Visible="false" Font-Size="Medium" Font-Bold="true" Width="100px"></asp:Label>
        <asp:TextBox ID="txtreptname" runat="server" Font-Bold="True" Visible="false" Font-Names="Book Antiqua"
            Font-Size="Medium" onkeypress="display()" Width="130px"></asp:TextBox>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
    </center>
    <asp:HiddenField runat="server" ID="hfdelete" />
    <asp:ModalPopupExtender ID="mpgetamount" runat="server" TargetControlID="hfdelete"
        PopupControlID="pnlstudemark" BackgroundCssClass="ModalPopupBG">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlstudemark" runat="server" CssClass="modalPopup" Style="display: none;
        height: 500px; width: 620px;">
        <table>
            <tr>
                <td>
                    <FarPoint:FpSpread ID="Fpstudentmark" runat="server" BorderColor="Black" BorderStyle="Solid"
                        ActiveSheetViewIndex="0" currentPageIndex="0" BorderWidth="1px" ScrollBarDarkShadowColor="White">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </td>
            </tr>
            <tr>
                <td>
                    <center>
                        <asp:Button ID="btnclosepanel" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnexitpanel_Click" />
                        <asp:Button ID="printmarks" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btn_printmarks" />
                    </center>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
