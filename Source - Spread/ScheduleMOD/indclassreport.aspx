<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="indclassreport.aspx.cs" Inherits="NewAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <%--<head runat="server">
   <title>Alternate Shedule Change</title>--%>
    <link href="Styles/AttendanceStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #clsbtn
        {
            height: 26px;
            width: 72px;
        }
        
        .style2
        {
            height: 30px;
        }
        .style3
        {
            width: 184px;
            height: 30px;
        }
        
        .style4
        {
            width: 327px;
        }
        .style7
        {
            width: 116px;
        }
        .txt
        {
        }
        .style8
        {
            height: 30px;
            width: 156px;
        }
        .style9
        {
            width: 156px;
        }
        .style10
        {
            width: 184px;
        }
        
        .style11
        {
            width: 1001px;
        }
        
        .style12
        {
            width: 132px;
        }
        
        .style13
        {
            width: 95px;
        }
        .style1
        {
            width: 135px;
        }
    </style>
    <%--</head>--%>
    <body oncontextmenu="return false">
        <br />
        <center>
            <asp:Label ID="lblhead" runat="server" Text="Individual Class Report" CssClass="fontstyleheader"
                ForeColor="Green"></asp:Label></center>
        <asp:ScriptManager ID="ScriptManager2" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
        <table style="width: 900px; height: 79px;" class="maintablestyle ">
            <tr>
                <td>
                    <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <div style="position: relative">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbatch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbatch" runat="server" Height="250px" Width="110px" CssClass="MultipleSelectionDDL"
                                    BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chkbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="chkbatch_CheckedChanged" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklstbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                    PopupControlID="pbatch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
                </td>
                <td>
                    <div style="position: relative">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdegree" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                    ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pdegree" runat="server" Height="250px" CssClass="MultipleSelectionDDL"
                                    Width="110px" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="chkdegree_CheckedChanged" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                    PopupControlID="pdegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Width="90px" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <div style="position: relative">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                    ReadOnly="true" Width="180px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbranch" runat="server" Height="400px" Width="350px" CssClass="MultipleSelectionDDL"
                                    BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="chkbranch_CheckedChanged" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                                    PopupControlID="pbranch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <div style="position: relative">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtsection" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                    ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="psection" runat="server" Height="125px" Width="110px" CssClass="MultipleSelectionDDL"
                                    BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chksection" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="chksection_CheckedChanged" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklstsection" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklstsection_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsection"
                                    PopupControlID="psection" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <div style="position: relative">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtsubject" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="psubject" runat="server" Height="250px" CssClass="MultipleSelectionDDL"
                                    BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chksubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="chksubject_CheckedChanged" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklssubject" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklssubject_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtsubject"
                                    PopupControlID="psubject" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblFromdate" runat="server" Text="FromDate" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="25px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="75px" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtFromDate"
                        FilterType="Custom,Numbers" ValidChars="/" />
                    <asp:CalendarExtender ID="calfromdate" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="lbltodate" runat="server" Text="ToDate" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" CssClass="txt" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" runat="server" Height="25px" Width="75px" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtToDate"
                        FilterType="Custom,Numbers" ValidChars="/" />
                    <asp:CalendarExtender ID="CalToDate" TargetControlID="txtToDate" Format="d/MM/yyyy"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td class="style2">
                    <asp:Button ID="btnGo" runat="server" Text="Go" Style="height: 26px; font-weight: 700;"
                        OnClick="btnGo_Click" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
            </tr>
        </table>
        <div style="position: relative">
            <table>
                <tr>
                    <td class="style9">
                        <asp:Label ID="fmlbl" runat="server" Text="Select from date" ForeColor="Red" Font-Bold="True"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td class="style10">
                        <asp:Label ID="tolbl" runat="server" Text="Select to date" ForeColor="Red" Font-Bold="True"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="diffdate" runat="server" Text="To date should be greater than from date"
                            Font-Bold="True" Font-Names="Book Antiqua" ForeColor="Red"></asp:Label><%--
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                    </td>
                </tr>
            </table>
        </div>
        <table>
            <tr>
                <td>
                    <%--<asp:Panel ID="Panel5" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="width: 1030px; height: 18px; background-image: url('Menu/Top%20Band-2.jpg');">
                                <br />
                            </asp:Panel>--%>
                </td>
            </tr>
            <%--  <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>--%>
            <tr>
                <td class="style11">
                    <asp:Label ID="errlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red"></asp:Label>
                    <%-- //-------------------------spread.---------------------%>
                    <FarPoint:FpSpread ID="classreport" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="700" Width="650">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="SpdInfo">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <%--  //--------------------------------------------------%>
                </td>
            </tr>
            <tr>
                <td class="style11">
                    <asp:Panel ID="colorpnl" runat="server">
                        <asp:TextBox ID="TextBox1" runat="server" BackColor="LightPink" Height="21px" Width="29px"
                            Enabled="false"></asp:TextBox>
                        <asp:Label ID="Label2" runat="server" Text="Alternate Schedule"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="TextBox2" runat="server" BackColor="LightSeaGreen" Height="21px"
                            Width="29px" Enabled="false"></asp:TextBox>
                        <asp:Label ID="Label4" runat="server" Text="Semester Schedule"></asp:Label>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <center>
                    <td>
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnxl_Click" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                        <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                        <asp:Label ID="norecordlbl" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                    </td>
                </center>
            </tr>
        </table>
        </center>
        </div>
        <%-- </form> --%>
    </body>
    </html>
</asp:Content>
