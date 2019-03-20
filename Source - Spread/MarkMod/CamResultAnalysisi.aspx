<%@ Page Title="Internal Result Analysis" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CamResultAnalysisi.aspx.cs" Inherits="CamResultAnalysisi" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
        
        .style1
        {
            width: 122px;
        }
        
        .printclass
        {
            display: none;
        }
        .marginSet
        {
            margin: 0px;
            padding: 0px;
        }
        .headerDisp
        {
            font-size: 25px;
            font-weight: bold;
        }
        .headerDisp1
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        @media print
        {
            #divMainContents
            {
                display: block;
            }
            .printclass
            {
                display: block;
                font-family: Book Antiqua;
            }
            .noprint
            {
                display: none;
            }
        }
        @media screen,print
        {
        
        }
        @page
        {
            size: A4;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblerror').innerHTML = "";
        }
        function PrintPanel() {
            var panel = document.getElementById("<%=divMainContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <center>
                    <asp:Label ID="Label4" runat="server" Text="Internal Result Analysis" CssClass="fontstyleheader"
                        Style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;"
                        ForeColor="Green"></asp:Label>
                    <table style="width: 700px; height: 70px; background-color: #0CA6CA; margin: 0px;
                        margin-bottom: 10px; margin-top: 10px;">
                        <tr>
                            <td>
                                <asp:Label ID="LblCollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="120Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="120Px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="120Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="120Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="LblSem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            CssClass="arrow" Style="width: 60px" Font-Size="Medium" Width="120Px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10" align="left">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Sec">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Font-Bold="True"
                                                Visible="true" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged" CssClass="arrow"
                                                Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black" Height="21px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbltest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Test">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddltest" runat="server" AutoPostBack="true" Font-Bold="True"
                                                Visible="true" CssClass="arrow" OnSelectedIndexChanged="ddltest_SelectedIndexChanged"
                                                Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black" Width="120px"
                                                Height="21px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTop" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Top">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTop" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Style="width: 33px;" MaxLength="2" onkeydown="return jsDecimals(event);"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtTop"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Width="40px" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text=" ">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="Buttongo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnClick="Buttongo_Click" Text="Go" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIncludeAbsent" Checked="false" runat="server" Text="Include Absent in Pass Pecentage"
                                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
                <center>
                    <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                        Width="959px" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
                        <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                            Font-Bold="True" Font-Names="Book Antiqua" />
                        <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                            ImageAlign="Right" />
                    </asp:Panel>
                    <asp:Panel ID="pbodyfilter" runat="server" CssClass="cpBody" Width="952px">
                        <center>
                            <asp:CheckBoxList ID="chklscolumn" runat="server" Font-Size="Medium" AutoPostBack="True"
                                OnSelectedIndexChanged="chklscolumn_SelectedIndexChanged" Font-Bold="True" RepeatColumns="5"
                                RepeatDirection="Horizontal" Font-Names="Book Antiqua">
                                <asp:ListItem Text="S.No"></asp:ListItem>
                                <asp:ListItem Text="Subject Code"></asp:ListItem>
                                <asp:ListItem Text="Subject Name"></asp:ListItem>
                                <asp:ListItem Text="Staff Name"></asp:ListItem>
                                <asp:ListItem Text="Subject Type"></asp:ListItem>
                                <asp:ListItem Text="Student Strength"></asp:ListItem>
                                <asp:ListItem Text="Appear"></asp:ListItem>
                                <asp:ListItem Text="Pass"></asp:ListItem>
                                <asp:ListItem Text="Fail"></asp:ListItem>
                                <asp:ListItem Text="Pass Percentage"></asp:ListItem>
                                <asp:ListItem Text="Appear After Retest"></asp:ListItem>
                                <asp:ListItem Text="Pass After Retest"></asp:ListItem>
                                <asp:ListItem Text="Fail After Retest"></asp:ListItem>
                                <asp:ListItem Text="Pass Percentage After Retest"></asp:ListItem>
                                <asp:ListItem Text="Remarks"></asp:ListItem>
                                <asp:ListItem Text="Total Number of Students"></asp:ListItem>
                                <asp:ListItem Text="Total Number of Girl Students"></asp:ListItem>
                                <asp:ListItem Text="Total Number of Boy Students"></asp:ListItem>
                                <asp:ListItem Text="Total Number of Girl Hostel Students"></asp:ListItem>
                                <asp:ListItem Text="Total Number of Boys Hostel Students"></asp:ListItem>
                                <asp:ListItem Text="Total Number of Girl Day Scholar Students"></asp:ListItem>
                                <asp:ListItem Text="Total Number of Boys Day Scholar Students"></asp:ListItem>
                                <asp:ListItem Text="Total Number of Students Failed In One Subject"></asp:ListItem>
                                <asp:ListItem Text="Total Number of Students Failed In Twos Subject"></asp:ListItem>
                                <asp:ListItem Text="Total Number of Students Failed In 3 & Above Subject"></asp:ListItem>
                            </asp:CheckBoxList>
                        </center>
                    </asp:Panel>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pbodyfilter"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
                        ExpandedImage="../images/down.jpeg">
                    </asp:CollapsiblePanelExtender>
                    <asp:Label ID="lblerror" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true" ForeColor="Red" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                        position: relative;"></asp:Label>
                    <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                        <tr>
                            <td colspan="5">
                                <div id="divMainContents" runat="server" style="display: table; margin: 0px; height: auto;
                                    margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
                                    <table class="printclass" style="width: 98%; height: auto; margin: 0px; padding: 0px;">
                                        <tr>
                                            <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                                                <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                                                    Width="100px" Height="100px" />
                                            </td>
                                            <th class="marginSet" align="center" colspan="6">
                                                <span id="spCollegeName" class="headerDisp" runat="server"></span>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th class="marginSet" align="center" colspan="6">
                                                <span id="spAddr" class="headerDisp1" runat="server"></span>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th class="marginSet" align="center" colspan="6">
                                                <span id="spReportName" class="headerDisp1" runat="server"></span>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td class="marginSet" colspan="3" align="center">
                                                <span id="spDegreeName" class="headerDisp1" runat="server"></span>
                                            </td>
                                            <td class="marginSet" colspan="3" align="right">
                                                <span id="spSem" class="headerDisp1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="marginSet" colspan="3" align="left">
                                                <span id="spProgremme" class="headerDisp1" runat="server"></span>
                                            </td>
                                            <td class="marginSet" colspan="3" align="right">
                                                <span id="spSection" class="headerDisp1" runat="server"></span>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                        HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true"
                                        OnRowDataBound="Showgrid_OnRowDataBound">
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="flow" runat="server" Text="STAFF PERFORMANCE" Font-Bold="true" ForeColor="Black"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px; margin-bottom: 10px;
                                    margin-top: 10px;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Chart ID="Chart1" runat="server" Width="800px" Visible="false" Style="margin: 0px;
                                    margin-bottom: 10px; margin-top: 10px;">
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
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblexcel" Text="Report Name" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                                                Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="_() {}[]">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnexcel" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true"
                                                runat="server" OnClick="btnexcel_Click" Text="Export Excel" />
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btnprint" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                            <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                            <asp:Button ID="btndirectprt" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
            <asp:PostBackTrigger ControlID="btnprint" />
        </Triggers>
    </asp:UpdatePanel>
    <%--progressBar for Upbook_go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel_go">
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
    <%--progressBar for Sem--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel_sem">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
