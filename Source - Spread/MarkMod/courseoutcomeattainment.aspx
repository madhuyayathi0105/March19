<%@ Page Language="C#" AutoEventWireup="true" CodeFile="courseoutcomeattainment.aspx.cs"
    Inherits="MarkMod_courseoutcomeattainment" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green;">Course Outcome Attainment Report</span>
    </center>
    <br />
    <div>
       <%-- <asp:UpdatePanel ID="upn" runat="server">
            <ContentTemplate>--%>
                <center>
                    <table class="maintablestyle" style="margin-left: 0px; height: 73px; width: 1017px;
                        margin-bottom: 0px;">
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 18px; width: 44px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="201px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Style="margin-left: -21px;" Font-Size="Medium"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Height="25px" Width="69px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldegree" runat="server" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Width="100px" Height="25px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Width="225px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                    Style="margin-left: -18px;" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Height="25px" Width="52px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="9">
                                <table>
                                    <tr>
                                        <%--<td class="style8">
                                    <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" style="margin-left: 23px;"></asp:Label>
                                </td>--%>
                                        <%-- <td class="style25">
                                    <asp:DropDownList ID="ddlsection" runat="server" AutoPostBack="True" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="62px" style="margin-left: 16px;" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>--%>
                                        <td>
                                            <asp:Label ID="lblcourse" runat="server" Text="CourseOutCome" Width="61px" Font-Bold="True"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin-left: 10px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlconame" runat="server" Font-Bold="True" OnSelectedIndexChanged="ddlconame_OnselectedIndexedChanged"
                                                Width="100px" Style="margin-left: 83px;" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsubject" runat="server" Text="Subject" Width="61px" Font-Bold="True"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin-left: 2px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_subject" runat="server" OnSelectedIndexChanged="ddl_subject_OnSelectedIndexChanged"
                                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Style="margin-left: 8px; height: 25px; width: 185px;">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbltest" runat="server" Text="CO Criteria Name" Font-Bold="True" ForeColor="Black"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="149px"></asp:Label>
                                        </td>
                                        <%--<td>
                                    <asp:DropDownList ID="ddltest" runat="server" OnSelectedIndexChanged="ddltest_OnSelectedIndexChanged" width=" 147px">
                                    </asp:DropDownList>
                                </td>--%>
                                        <td>
                                            <asp:UpdatePanel ID="Upnltest" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txttest" Height="20px" Width="150px" runat="server" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="paneltest" runat="server" CssClass="multxtpanel" Height="200px" Width="150px">
                                                        <asp:CheckBox ID="chktest" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chktest_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cbltest" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbltest_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popupExttest" runat="server" TargetControlID="txttest"
                                                        PopupControlID="paneltest" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="style15">
                                            <%--<asp:UpdatePanel ID="upgo" runat="server">
                                                <ContentTemplate>--%>
                                                    <asp:Button ID="btngo" runat="server" Text="Generate" OnClick="btngo_Click" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" /><%--</ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    <div>
       <%-- <asp:UpdatePanel ID="upgd" runat="server">
            <ContentTemplate>--%>
                <center>
                    <table>
                        <tr align="center">
                            <td align="center">
                                <asp:GridView ID="GridView1" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                    width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="true" ShowHeader="false"
                                    OnRowDataBound="GridView1_OnRowDataBound">
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                           <%--  <asp:Chart ID="Chart1" runat="server" Height="300px" Width="400px" Visible="false">--%>
                               <asp:Chart ID="Chart1" runat="server" Width="800px" Visible="false" Font-Names="Book Antiqua"
                                    EnableViewState="true" Font-Size="Medium" Style="margin: 0px; margin-bottom: 10px;
                                    margin-top: 10px;">
                                     <Titles>
                                        <asp:Title Docking="Bottom" Text="Grade">
                                        </asp:Title>
                                        <asp:Title Docking="Left" Text="Student Count">
                                        </asp:Title>
                                    </Titles>
                                   <%-- <Titles>
                                        <asp:Title ShadowOffset="2" Name="Items" />
                                    </Titles>--%>
                                    <Legends>
                                        <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default"
                                            LegendStyle="Row" />
                                    </Legends>
                                    <Series>
                                        <asp:Series Name="Grade"  />
                                    </Series>
                                
                                     <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                                        <AxisY LineColor="Black">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                            <MajorGrid LineColor="Brown" />
                                            <MinorGrid Enabled="false" LineColor="AliceBlue" />
                                        </AxisY>
                                        <AxisX LineColor="Black">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                            <MajorGrid LineColor="Brown" />
                                            <MinorGrid Enabled="false" LineColor="AliceBlue" />
                                        </AxisX>
                                    </asp:ChartArea>
                                </ChartAreas>
                                </asp:Chart>
                            </td>
                        </tr>
                        <tr>
                        <td align="center">
                            <asp:GridView ID="GridViewchart" runat="server" Style="margin: 0px; margin-bottom: 10px;
                                margin-top: 10px;" Width="645px" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" BackColor="AliceBlue" OnRowDataBound="GridViewchart_OnRowDataBound">
                                 <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" />
                            </asp:GridView>
                        </td>
                    </tr>
                        <tr align="center">
                            <td align="center">
                                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                    Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Visible="false"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ .">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnxl_Click" Visible="false" />
                                <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnmasterprint_Click" Visible="false" />
                                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:Label ID="lbl_err" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </center>
           <%-- </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btnmasterprint" />
            </Triggers>
        </asp:UpdatePanel>--%>
    </div>
    <center>
       <%-- <asp:UpdatePanel ID="upok" runat="server">
            <ContentTemplate>--%>
                <div id="div4" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div5" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                            right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label4" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnaltok" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnaltok_Click"
                                                Text="Ok" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>
   <%-- <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upgo">
            <ProgressTemplate>
                <div style="height: 40px; width: 150px;">
                    <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                    <br />
                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                        Processing Please Wait...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>--%>
    
</asp:Content>
