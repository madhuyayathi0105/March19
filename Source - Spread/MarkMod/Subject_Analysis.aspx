<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Subject_Analysis.aspx.cs" Inherits="Subject_Analysis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_Label1').innerHTML = "";
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
    <style type="text/css">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <html>
    <body>
        <asp:ScriptManager ID="Script1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <asp:Label ID="lbl" runat="server" Text="Subject Analysis Report" Font-Bold="true"
                Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
        </center>
        <br />
        <div>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <center>
                        <table style="width: 900px; height: 100px; background-color: #0CA6CA;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtbatch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                    Width="106px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Width="108px" Height="150px"
                                                    BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                                                    <asp:CheckBox ID="chckbatch" runat="server" OnCheckedChanged="checkBatch_CheckedChanged"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBoxList ID="chcklistbatch" runat="server" OnSelectedIndexChanged="cheklistBatch_SelectedIndexChanged"
                                                        Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="true">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbatch"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="chcklistbatch" />
                                            </Triggers>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="chckbatch" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="deg" runat="server" Text="Degree" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldeg" runat="server" AutoPostBack="true" Width="150px" Font-Names="Book Antiqua"
                                        Font-Bold="true" Font-Size="Medium" Height="25px " OnSelectedIndexChanged="ddldegselect">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="branch" runat="server" Text="Branch" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="degbranch" runat="server" AutoPostBack="true" Width="150px"
                                        Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="25px "
                                        OnSelectedIndexChanged="degbranchselect">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="sem" runat="server" Text="Semester" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlsem" runat="server" AutoPostBack="true" Width="150px" Font-Names="Book Antiqua"
                                                Font-Bold="true" Font-Size="Medium" Height="25px " OnSelectedIndexChanged="ddlsemselect">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="sec" runat="server" Text="Section" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TextBox3" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                    Width="106px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Width="108px" Height="150px"
                                                    BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                                                    <asp:CheckBox ID="Chksec" runat="server" OnCheckedChanged="Chksec_CheckedChanged"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBoxList ID="Cblsec" runat="server" OnSelectedIndexChanged="Cblsec_SelectedIndexChanged"
                                                        Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="true">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="TextBox3"
                                                    PopupControlID="Panel4" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="Cblsec" />
                                            </Triggers>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="Chksec" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="sub" runat="server" Text="Subject" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsubj" runat="server" AutoPostBack="true" Width="150px" Font-Names="Book Antiqua"
                                        Font-Bold="true" Font-Size="Medium" Height="25px " OnSelectedIndexChanged="ddlsubject">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="subtype" runat="server" Text="SeatType" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="--Select--" CssClass="Dropdown_Txt_Box" ReadOnly="true"></asp:TextBox>
                                                <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Height="150" Width="200px"
                                                    BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="SelectAll" AutoPostBack="True"
                                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        OnCheckedChanged="cbsubtyp_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" Font-Size="Small" AutoPostBack="True"
                                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblsubtyp_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <br />
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="TextBox1"
                                                    PopupControlID="Panel2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="CheckBoxList1" />
                                            </Triggers>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="CheckBox1" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="testtyp" runat="server" Text="TestType" font-name="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="--Select--" CssClass="Dropdown_Txt_Box" ReadOnly="true"></asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="200" Width="200px"
                                                    BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                                                    <asp:CheckBox ID="Cbtesttyp" runat="server" Text="SelectAll" AutoPostBack="True"
                                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        OnCheckedChanged="Cbtesttyp_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="Cbltesttyp" runat="server" Font-Size="Small" AutoPostBack="True"
                                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="Cbltesttyp_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <br />
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="TextBox2"
                                                    PopupControlID="Panel3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="Cbltesttyp" />
                                            </Triggers>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="Cbtesttyp" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="gobtn" runat="server" Style="font-weight: 700;" Text="Go" Width="60px"
                                                Height="30px" OnClick="gobtn_Click" OnClientClick="return validation()" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkhost" runat="server" AutoPostBack="True" Font-Bold="True" Width="250px"
                                        Text="Including Hosteler" Font-Names="Book Antiqua" OnCheckedChanged="chkhost1"
                                        Font-Size="Medium" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    ForeColor="Red" Font-Size="Medium" Text="No Records Were Found" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <center>
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
                    </center>
                    <br />
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Visible="False"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblreptname" runat="server" Text="Report Name" font-name="Book Antiqua"
                                        Visible="false" Font-Size="Medium" Font-Bold="true" Width="100px"></asp:Label>
                                </td>
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    ForeColor="Red" Text="No Record Were Found" Font-Size="Medium" Visible="False"></asp:Label>
                                <td>
                                    <asp:TextBox ID="txtreptname" runat="server" Font-Bold="True" Visible="false" Font-Names="Book Antiqua"
                                        Font-Size="Medium" onkeypress="display()" Width="130px"></asp:TextBox>
                                </td>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtreptname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                                <td>
                                    <asp:Button ID="Excel" runat="server" Text="Export Excel" Visible="false" Font-Size="Medium"
                                        Font-Bold="true" OnClick="Excel_OnClick" Font-Names="Book Antiqua" />
                                </td>
                                <td>
                                    <asp:Button ID="Print" runat="server" Text="Print" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true" OnClick="Print_OnClick" Visible="false" />
                                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                    <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="Excel" />
                    <asp:PostBackTrigger ControlID="Print" />
                    <asp:PostBackTrigger ControlID="btnPrint" />
                </Triggers>
            </asp:UpdatePanel>
            <%--progressBar for Go--%>
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
        </div>
    </body>
    </html>
</asp:Content>
