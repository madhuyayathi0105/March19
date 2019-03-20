<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="DepartmentwiseCummulative Salary.aspx.cs" Inherits="DepartmentwiseCummulative_Salary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=grdPanel.ClientID %>");
            var SpanHdr = document.getElementById("<%=RptHead.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>PF STATEMENT</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<form>');
            printWindow.document.write('<br/>');
            printWindow.document.write('<center>');
            printWindow.document.write(SpanHdr.innerHTML);
            printWindow.document.write('</center>');
            printWindow.document.write('<br/>');
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
    <style type="text/css">
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
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
        
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
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
    </style>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <center>
        <div style="top: 80px; position: absolute;">
            <div>
                <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="position: absolute;
                    width: 995px; height: 21px; margin-bottom: 0px; top: 8px; left: -30px;">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label2" runat="server" Text="Departmentwise Cummulative Salary" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White" Style="color: White;
                        font-family: Book Antiqua; font-size: medium; font-weight: bold; position: absolute;
                        left: 400px;"></asp:Label>
                    &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;
                    <br />
                    <br />
                    <br />
                </asp:Panel>
            </div>
        </div>
        <br />
        <br />
        <br />
        <asp:Panel ID="pnldemond" runat="server" BorderColor="Black" BorderWidth="2px" BackColor="#0CA6CA"
            Width="966px">
            <table style="height: 51px">
                <tr>
                    <td>
                        <asp:Label ID="Label13" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Year"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cblbatchyear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td class="style405">
                        <asp:Label ID="lblmonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Month"></asp:Label>
                    </td>
                    <td class="style315">
                        <asp:DropDownList ID="cblmonthfrom" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                            <asp:ListItem Value="1">January</asp:ListItem>
                            <asp:ListItem Value="2">February</asp:ListItem>
                            <asp:ListItem Value="3">March</asp:ListItem>
                            <asp:ListItem Value="4">April</asp:ListItem>
                            <asp:ListItem Value="5">May</asp:ListItem>
                            <asp:ListItem Value="6">June</asp:ListItem>
                            <asp:ListItem Value="7">July</asp:ListItem>
                            <asp:ListItem Value="8">August</asp:ListItem>
                            <asp:ListItem Value="9">September</asp:ListItem>
                            <asp:ListItem Value="10">October</asp:ListItem>
                            <asp:ListItem Value="11">November</asp:ListItem>
                            <asp:ListItem Value="12">December</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbldept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Department"></asp:Label>
                    </td>
                    <td>
                        <div id="castediv" runat="server">
                            <asp:TextBox ID="tbseattype" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">---Select---</asp:TextBox>
                            <br />
                        </div>
                        <asp:Panel ID="pseattype" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="2px" Height="300px" ScrollBars="Vertical" Width="350px">
                            <asp:CheckBox ID="chkselect" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkselect_CheckedChanged"
                                Text="Select All" />
                            <asp:CheckBoxList ID="cbldepttype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Width="258px" OnSelectedIndexChanged="cbldepttype_SelectedIndexChanged" Height="102px"
                                Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="ddeseattype" runat="server" TargetControlID="tbseattype"
                            PopupControlID="pseattype" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Category"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbblood" runat="server" Height="20px" ReadOnly="true" Width="135px"
                            Style="font-family: 'Book Antiqua'; margin-top: 0px;" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">---Select---</asp:TextBox>
                        <br />
                        <asp:Panel ID="pblood" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="2px" Height="200px" ScrollBars="Auto" Width="200px" Style="font-family: 'Book Antiqua';
                            margin-left: 58px;">
                            <asp:CheckBox ID="chkcategory" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkcategory_CheckedChanged"
                                Text="Select All" Checked="True" />
                            <asp:CheckBoxList ID="cblcategory" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Width="158px" OnSelectedIndexChanged="cblcategory_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:DropDownExtender ID="ddeblood" runat="server" DropDownControlID="pblood" DynamicServicePath=""
                            Enabled="true" TargetControlID="tbblood">
                        </asp:DropDownExtender>
                    </td>
                    <td>
                        <asp:Label ID="txtstafftype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Staff Type"></asp:Label>
                    </td>
                    <td>
                        <div id="Div3" runat="server" class="linkbtn">
                            <asp:TextBox ID="txtsstafftype" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">---Select---</asp:TextBox>
                            <br />
                        </div>
                        <asp:PlaceHolder ID="pstafftypes" runat="server"></asp:PlaceHolder>
                        <asp:Panel ID="pstafftype" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="2px" ScrollBars="Vertical" Style="font-family: 'Book Antiqua'; margin-left: 47px;">
                            <asp:CheckBox ID="chksstafftype" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cblsstafftype_CheckedChanged"
                                Text="Select All" Checked="True" />
                            <asp:CheckBoxList ID="chksstafftypelist" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Width="167px" OnSelectedIndexChanged="cblsstafftype_SelectedIndexChanged" Font-Bold="True"
                                Font-Names="Book Antiqua">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:DropDownExtender ID="DropDownExtender3" runat="server" DropDownControlID="pstafftype"
                            DynamicServicePath="" Enabled="true" TargetControlID="txtsstafftype">
                        </asp:DropDownExtender>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Allowance"></asp:Label>
                    </td>
                    <td>
                        <div id="Div1" runat="server" class="linkbtn">
                            <asp:TextBox ID="txtallowance" runat="server" Height="20px" ReadOnly="true" Width="100px"
                                Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">---Select---</asp:TextBox>
                            <br />
                        </div>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                        <asp:Panel ID="Pallowance" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="2px" ScrollBars="Vertical" Style="font-family: 'Book Antiqua'; margin-left: 47px;">
                            <asp:CheckBox ID="chkallowance" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cblallowance_CheckedChanged"
                                Text="Select All" Checked="True" />
                            <asp:CheckBoxList ID="cblallowance" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Width="167px" OnSelectedIndexChanged="cblallowance_SelectedIndexChanged" Font-Bold="True"
                                Font-Names="Book Antiqua">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:DropDownExtender ID="DropDownExtender1" runat="server" DropDownControlID="Pallowance"
                            DynamicServicePath="" Enabled="true" TargetControlID="txtallowance">
                        </asp:DropDownExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblde" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            Text="Deduction"></asp:Label>
                    </td>
                    <td>
                        <div id="Div2" runat="server" class="linkbtn">
                            <asp:TextBox ID="txtdeduction" runat="server" Height="20px" ReadOnly="true" Width="100px"
                                Style="font-family: 'Book Antiqua'; margin-bottom: 0px;" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">---Select---</asp:TextBox>
                            <br />
                        </div>
                        <asp:PlaceHolder ID="PlaceHolderded" runat="server"></asp:PlaceHolder>
                        <asp:Panel ID="Pdeduction" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="2px" ScrollBars="Vertical" Width="200px" Style="font-family: 'Book Antiqua';
                            margin-left: 58px;">
                            <asp:CheckBox ID="Chkdeduction" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="Chkdeduction_CheckedChanged"
                                Text="Select All" Checked="True" />
                            <asp:CheckBoxList ID="cbldeduction" runat="server" Font-Size="Medium" AutoPostBack="True"
                                Width="167px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbldeduction_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:DropDownExtender ID="DropDownExtender2" runat="server" DropDownControlID="Pdeduction"
                            DynamicServicePath="" Enabled="true" TargetControlID="txtdeduction">
                        </asp:DropDownExtender>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rbtype" runat="server" AutoPostBack="true" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtype_SelectedIndexChanged">
                            <asp:ListItem Value="0" Selected="True">General</asp:ListItem>
                            <asp:ListItem Value="1">Category</asp:ListItem>
                            <asp:ListItem Value="2">Staff</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkdept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Dept" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkpf" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="PF Order" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkgroup" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Include Group" AutoPostBack="true" OnCheckedChanged="chkgroup_CheckedChanged" />
                    </td>
                    <td>
                        <asp:LinkButton ID="LinkButton1" runat="server" Visible="false" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="LinkButton1_Click">Group</asp:LinkButton>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkgrouptotal" Text="Group Total" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="GO" OnClick="btndemond_go_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
            Width="1030px" Style="position: absolute; left: 0px;">
        </asp:Panel>
        <br />
        <asp:Label ID="lblnorec" runat="server" Text="There are no Records Found" ForeColor="Red"
            Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
        <br />
        <br />
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <FarPoint:FpSpread ID="fpsalarydemond" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Height="295px" Width="1000px" ShowHeaderSelection="false">
                    <CommandBar BackColor="Control" ButtonType="PushButton" ButtonFaceColor="Control"
                        ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium"></asp:Label>
        <asp:TextBox ID="txtxl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium"></asp:TextBox>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        <br />
        <br />
        <span id="RptHead" runat="server" class="fontstyleheader" style="color: Green; font-size: medium;">
        </span>
        <br />
        <br />
        <asp:Panel ID="grdPanel" runat="server" Visible="false">
            <asp:GridView ID="grdPF" runat="server" Visible="false" AutoGenerateColumns="true"
                GridLines="Both" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Names="Book Antiqua"
                HeaderStyle-Font-Size="Medium" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#0CA6CA"
                HeaderStyle-ForeColor="Black">
            </asp:GridView>
            <br />
            <br />
            <asp:GridView ID="grdSummary" runat="server" Visible="false" Width="400px" AutoGenerateColumns="true"
                GridLines="Both" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Names="Book Antiqua"
                HeaderStyle-Font-Size="Medium" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#0CA6CA"
                HeaderStyle-ForeColor="Black">
            </asp:GridView>
        </asp:Panel>
        <br />
        <br />
        <asp:Button ID="btnExport" runat="server" Style="font-family: Book Antiqua; font-weight: bold;"
            Text="Export To PDF" Visible="false" OnClientClick=" return PrintPanel()" />
        <br />
        <div id="poppernew" runat="server" visible="false" class="popupstyle popupheight1 ">
            <asp:Panel ID="Panel3" runat="server" Visible="false" CssClass="modalPopup" Style="background-color: lightgray;
                margin-left: 90px; border-style: solid; border-width: 1px; left: 20px; height: 700;
                width: 780px;">
                <div>
                    <table>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="FpSpread1" runat="server" OnCellClick="FpSpread1_CellClick">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark" ButtonType="PushButton">
                                        <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                    <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                                        Font-Size="X-Large">
                                    </TitleInfo>
                                </FarPoint:FpSpread>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="TextBox1" runat="server" Height="27px" Width="150px" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </div>
                                <div>
                                    <asp:DropDownList ID="ddlstream" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </div>
                                <div style="height: 10px;">
                                </div>
                                <div>
                                    <asp:Button ID="Button1" runat="server" Text="Group" OnClick="Btn_group_Click" Height="27px"
                                        Width="47px" />
                                </div>
                                <div style="height: 10px;">
                                </div>
                                <div>
                                    <asp:Button ID="Btn_Move" runat="server" Text=">" OnClick="Btn_Move_Click" Height="27px"
                                        Width="31px" />
                                </div>
                                <div style="height: 10px;">
                                </div>
                                <div>
                                    <asp:Button ID="Btn_Moveall" runat="server" Visible="false" Text=">>" Height="25px"
                                        Width="31px" OnClick="Btn_Moveall_Click" />
                                </div>
                                <div style="height: 30px;">
                                    <asp:Button ID="btn_Remove" runat="server" Text="<" Height="25px" Width="31px" OnClick="btn_Remove_Click" />
                                </div>
                                <div>
                                    <asp:Button ID="Btn_Removeall" runat="server" Text="<<" OnClick="Btn_Removeall_Click"
                                        Height="25px" Width="31px" />
                                </div>
                            </td>
                            <td>
                                <FarPoint:FpSpread ID="FpSpread2" runat="server" OnCellClick="FpSpread2_CellClick">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark" ButtonType="PushButton">
                                        <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                    <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                                        Font-Size="X-Large">
                                    </TitleInfo>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="margin-left: 280px;">
                    <asp:Button ID="Btn_ok" runat="server" Text="Save" Width="75px" OnClick="Btn_ok_Click" />
                    <asp:Button ID="Btn_cancel" runat="server" Text="Cancel" Width="75px" OnClick="Btn_cancel_Click" />
                </div>
            </asp:Panel>
        </div>
    </center>
</asp:Content>
