<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master" AutoEventWireup="true"
    CodeFile="inv_StockDetails.aspx.cs" Inherits="inv_StockDetails" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title>Hostel</title>
        <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
        <script type="text/javascript">
            function Test1() {
                var empty = "";
                var item = document.getElementById("<%=txt_item.ClientID %>").value;
                var quant = document.getElementById("<%=txt_quant.ClientID %>").value;

                if (item.trim() == "") {
                    dep = document.getElementById("<%=txt_item.ClientID %>");
                    dep.style.borderColor = 'Red';
                    empty = "E";

                }
                if (quant.trim() == "") {
                    dep = document.getElementById("<%=txt_quant.ClientID %>");
                    dep.style.borderColor = "red";
                    empty = "E";
                }

                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }

            function myFunction1(y) {
                y.style.borderColor = "#c4c4c4";
            }

            function display() {
                document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
            }

            function Test2() {
                var empty = "";
                var hosname = document.getElementById("<%=txt_hostelname.ClientID %>").value;
                var build = document.getElementById("<%=txt_building.ClientID %>").value;
                var floor = document.getElementById("<%=txt_floor.ClientID %>").value;
                var room = document.getElementById("<%=txt_room.ClientID %>").value;

                if (hosname.trim() == "--Select--") {
                    quantity = document.getElementById("<%=txt_hostelname.ClientID %>");
                    quantity.style.borderColor = 'Red';
                    empty = "E";

                }
                if (build.trim() == "--Select--") {
                    rateperunit = document.getElementById("<%=txt_building.ClientID %>");
                    rateperunit.style.borderColor = 'Red';
                    empty = "E";

                }
                if (floor.trim() == "--Select--") {
                    dep = document.getElementById("<%=txt_floor.ClientID %>");
                    dep.style.borderColor = 'Red';
                    empty = "E";

                }
                if (room.trim() == "--Select--") {
                    dep = document.getElementById("<%=txt_room.ClientID %>");
                    dep.style.borderColor = 'Red';
                    empty = "E";

                }

                if (empty.trim() != "") {

                    return false;
                }
                else {
                    return true;
                }
            }


        </script>
    </head>
    <body>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Room Stock Details</span></div>
                <br />
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="height: 700px; width: 1000px;">
                <%--maincontent--%>
                <center>
                    <div style="height: 700px;">
                        <br />
                        <table class="maintablestyle" style="width: 850px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_hostelname" runat="server" Text="Hostel Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPHostelName" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_hostelname" runat="server" Height="200px" Width="200px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_hostel" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_hostel_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_hostel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostel_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popHostelName" runat="server" TargetControlID="txt_hostelname"
                                                PopupControlID="panel_hostelname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_building" runat="server" Text="Building Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPBuilding" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_building" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_building" runat="server" Height="200px" Width="200px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_building" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_building_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_building" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_building_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popBuilding" runat="server" TargetControlID="txt_building"
                                                PopupControlID="panel_building" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_floor" runat="server" Text="Floor Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPFloor" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_floor" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_floor" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_floor" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_floor_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_floor" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_floor_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popFloor" runat="server" TargetControlID="txt_floor"
                                                PopupControlID="panel_floor" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_room" runat="server" Text="Room Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPRoom" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_room" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_room" runat="server" CssClass="multxtpanel" Height="250px" Width="130px">
                                                <asp:CheckBox ID="cb_room" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_room_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_room" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_room_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popRoom" runat="server" TargetControlID="txt_room"
                                                PopupControlID="panel_room" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                    <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox btn2"
                                        OnClick="btn_addnew_Click" />
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div>
                            <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </div>
                        <div id="divColOrder" runat="server" visible="false">
                            <%--     Collapse Panel start--%>
                            <asp:Panel ID="panel_headerfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="850px" Style="margin-top: -0.1%;">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="lbl_filter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="panel_columnorder" runat="server" CssClass="maintablestyle" Width="850px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_column_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -469px;"
                                                Visible="false" Width="111px" OnClick="lnk_columnorder_Click">Remove  All</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                            <asp:TextBox ID="txt_order" Visible="false" Width="830px" TextMode="MultiLine" CssClass="style1"
                                                AutoPostBack="true" runat="server" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cbl_columnorder" runat="server" Height="43px" AutoPostBack="true"
                                                Width="830px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cbl_columnorder_SelectedIndexChanged">
                                                <asp:ListItem Value="HostelName">Hostel Name</asp:ListItem>
                                                <asp:ListItem Value="Building_Name">Building Name</asp:ListItem>
                                                <asp:ListItem Value="Floor_Name">Floor Name</asp:ListItem>
                                                <asp:ListItem Value="Room_Name">Room Name</asp:ListItem>
                                                <asp:ListItem Value="Net_Connection">Net Connection</asp:ListItem>
                                                <asp:ListItem Value="item_count">Total no of Item</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="panel_columnorder"
                                CollapseControlID="panel_headerfilter" ExpandControlID="panel_headerfilter" Collapsed="true"
                                TextLabelID="lbl_filter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                                ExpandedImage="down.jpeg">
                            </asp:CollapsiblePanelExtender>
                            <%--     Collapse Panel End--%>
                        </div>
                        <br />
                        <div id="div1" runat="server" style="width: 950px; height: 360px; overflow: auto;
                            background-color: White; border-radius: 10px;">
                            <br />
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                                BorderWidth="1px" Width="900px" Style="overflow: auto; border: 0px solid #999999;
                                border-radius: 0px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                class="spreadborder" OnCellClick="FpSpread1_OnCellClick" OnPreRender="FpSpread1_Selectedindexchange">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please enter the report name"
                                Visible="false"></asp:Label>
                            <%-- Font-Names="Book Antiqua" Font-Size="Medium"--%>
                            <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                            <%--Font-Names="Book Antiqua" Font-Size="Medium"--%>
                            <asp:TextBox ID="txt_excelname" runat="server" CssClass="textbox textbox1 txtheight4"
                                onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=". ">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" CssClass="textbox"
                                Text="Export To Excel" Width="127px" Height="30px" />
                            <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                                Width="60px" Height="30px" CssClass="textbox" />
                            <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
                        </div>
                    </div>
                </center>
                <%--pop up add new--%>
                <center>
                    <div id="popwindow1" runat="server" visible="false" class="popupstyle popupheight1">
                        <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 450px;"
                            OnClick="btn_exit_Click" />
                        <br />
                        <br />
                        <div style="background-color: White; height: 550px; width: 920px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <center>
                                <div>
                                    <span class="fontstyleheader" style="color: Green;">Room Extra Items Entry</span></div>
                            </center>
                            <br />
                            <div style="width: 850px; height: 425px;" class="table">
                                <br />
                                <div style="float: left;">
                                    <table style="width: 350px; border: 1px black; left: 100px;">
                                        <tr>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rdbl_selection" runat="server" OnSelectedIndexChanged="rdbl_selection_OnSelectedIndexChanged"
                                                    RepeatDirection="Horizontal" TextAlign="Right" AutoPostBack="true">
                                                    <asp:ListItem Selected="True">Single</asp:ListItem>
                                                    <asp:ListItem>Multiple</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_hostelname2" runat="server" Text="Hostel Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_hostelname2" runat="server" CssClass="textbox  ddlheight3"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_hostelname2_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:UpdatePanel ID="UP2HostelName" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_hostelname2" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                            onfocus="return myFunction1(this)" Visible="false">--Select--</asp:TextBox>
                                                        <asp:Panel ID="panel_hostel2" runat="server" Height="200PX" Width="200PX" CssClass="multxtpanel"
                                                            Visible="false">
                                                            <asp:CheckBox ID="cb_hostel2" runat="server" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cb_hostel2_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_hostel2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostel2_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="pop2Hostel" runat="server" TargetControlID="txt_hostelname2"
                                                            PopupControlID="panel_hostel2" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <span style="color: Red;">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_building2" runat="server" Text="Building Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_building2" runat="server" CssClass="textbox ddlheight3"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_building2_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:UpdatePanel ID="UP2Building" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_building2" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                            onfocus="return myFunction(this)" Visible="false">--Select--</asp:TextBox>
                                                        <asp:Panel ID="panel_building2" runat="server" Height="200PX" Width="150PX" CssClass="multxtpanel"
                                                            Visible="false">
                                                            <asp:CheckBox ID="cb_building2" runat="server" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cb_building2_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_building2" runat="server" OnSelectedIndexChanged="cbl_building2_OnSelectedIndexChanged"
                                                                AutoPostBack="True">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="pop2Building" runat="server" TargetControlID="txt_building2"
                                                            PopupControlID="panel_building2" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <span style="color: Red;">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_floor2" runat="server" Text="Floor Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_floor2" runat="server" CssClass="textbox ddlheight3" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddl_floor2_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:UpdatePanel ID="UP2Floor" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_floor2" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                            onfocus="return myFunction1(this)" Visible="false">--Select--</asp:TextBox>
                                                        <asp:Panel ID="panel_floor2" runat="server" Height="200PX" Width="150PX" CssClass="multxtpanel"
                                                            Visible="false">
                                                            <asp:CheckBox ID="cb_floor2" runat="server" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cb_floor2_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_floor2" runat="server" OnSelectedIndexChanged="cbl_floor2_OnSelectedIndexChanged"
                                                                AutoPostBack="True">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="pop2Floor" runat="server" TargetControlID="txt_floor2"
                                                            PopupControlID="panel_floor2" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <span style="color: Red;">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_room2" runat="server" Text="Room Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_room2" runat="server" CssClass="textbox ddlheight3">
                                                </asp:DropDownList>
                                                <asp:UpdatePanel ID="Up2Room" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_room2" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                            onfocus="return myFunction1(this)" Visible="false">--Select--</asp:TextBox>
                                                        <asp:Panel ID="panel_room2" runat="server" Height="200PX" Width="150PX" CssClass="multxtpanel"
                                                            Visible="false">
                                                            <asp:CheckBox ID="cb_room2" runat="server" Text="Select All" AutoPostBack="True"
                                                                OnCheckedChanged="cb_room2_OnCheckedChanged" />
                                                            <asp:CheckBoxList ID="cbl_room2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_room2_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="pop2Room" runat="server" TargetControlID="txt_room2"
                                                            PopupControlID="panel_room2" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <span style="color: Red;">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_item" runat="server" Text="Item"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_item" runat="server" CssClass="textbox txtheight2" onfocus="return myFunction1(this)"
                                                    ReadOnly="true"></asp:TextBox>
                                                <asp:TextBox ID="txt_itemCode" runat="server" CssClass="textbox txtheight2" onfocus="return myFunction1(this)"
                                                    ReadOnly="true" Visible="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <span style="color: Red;">*</span>
                                                <asp:Button ID="btn_additem1" runat="server" CssClass="textbox btn1" Text="?" OnClick="btn_additem1_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_quant" runat="server" Text="Quantity"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_quant" runat="server" CssClass="textbox txtheight2" onfocus="return myFunction1(this)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server" TargetControlID="txt_quant"
                                                    FilterType="custom,Numbers" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <span style="color: Red;">*</span>
                                                <asp:Button ID="btn_additem2" runat="server" CssClass="textbox btn1" Text="Add" OnClientClick="return Test1()"
                                                    OnClick="btn_additem2_Click" Width="60px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                                <asp:Label ID="lbl_netcon" runat="server" Text="Net Connection"></asp:Label>
                                            </td>
                                            <td>
                                                <br />
                                                <asp:CheckBox ID="cb_netcon" runat="server" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <div align="right">
                                    <FarPoint:FpSpread ID="FpSpread2" runat="server" Width="470px" Height="300px" CssClass="spreadborder">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </div>
                            <br />
                            <div>
                                <center>
                                    <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="textbox btn2"
                                        OnClick="btn_update_Click" />
                                    <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                                        OnClick="btn_delete_Click" />
                                    <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="textbox btn2" OnClick="btn_save_Click" />
                                    <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit_Click" />
                                </center>
                            </div>
                            <br />
                        </div>
                    </div>
                </center>
                <%--pop up add new Itemscheck--%>
                <center>
                    <div id="popwindow2" runat="server" visible="false" class="popupstyle popupheight">
                        <asp:ImageButton ID="imagebtnpop2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 35px; margin-left: 396px;"
                            OnClick="btn_itemexit_Click" />
                        <br />
                        <br />
                        <center>
                            <div style="background-color: White; height: 550px; width: 816px; border: 5px solid #0CA6CA;
                                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                <br />
                                <center>
                                    <div>
                                        <span class="fontstyleheader" style="color: Green;">Select the Item</span></div>
                                </center>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_itemheader3" runat="server" Text="Item Header"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UP3ItemHeader" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_itemheader3" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                        Width="120px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel_itemheader3" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                        <asp:CheckBox ID="cb_itemheader3" runat="server" Width="100px" Text="Select All"
                                                            AutoPostBack="True" OnCheckedChanged="cb_itemheader3_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cbl_itemheader3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_itemheader3_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="pop3ItemHeader" runat="server" TargetControlID="txt_itemheader3"
                                                        PopupControlID="panel_itemheader3" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_itemname3" runat="server" Text="Item Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UP3ItemName" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_itemname3" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                        Width="120px" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel_itemname3" runat="server" Height="250px" Width="250px" CssClass="multxtpanel">
                                                        <asp:CheckBox ID="cb_itemname3" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="cb_itemname3_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cbl_itemname3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_itemname3_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="pop3ItemName" runat="server" TargetControlID="txt_itemname3"
                                                        PopupControlID="panel_itemname3" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Search By"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_type3" runat="server" CssClass="textbox ddlheight2" OnSelectedIndexChanged="ddl_type3_OnSelectedIndexChanged"
                                                Width="100px" AutoPostBack="True">
                                                <asp:ListItem Value="0">Item Name</asp:ListItem>
                                                <asp:ListItem Value="1">Item Code</asp:ListItem>
                                                <asp:ListItem Value="2">Item Header</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_searchby" Visible="false" runat="server" CssClass="textbox txtheight2"
                                                Style="top: 10px;" Width="120px" onfocus="return myFunction1(this)"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:TextBox ID="txt_searchitemcode" Visible="false" runat="server" CssClass="textbox txtheight2"
                                                Style="top: 10px;" Width="120px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getitemcode" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchitemcode"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:TextBox ID="txt_searchheadername" Visible="false" runat="server" CssClass="textbox txtheight2"
                                                Style="top: 10px;" Width="120px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getitemheader" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchheadername"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_go1" runat="server" Text="Go" CssClass="textbox btn1" OnClick="btn_go1_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div id="newdiv" runat="server" style="width: 750px; height: 390px; overflow: auto;"
                                    class="table">
                                    <br />
                                    <FarPoint:FpSpread ID="FpSpread3" runat="server" Width="700px" Height="300px" CssClass="spreadborder">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                    <br />
                                    <asp:Button ID="btn_itemok" Visible="false" runat="server" Text="Ok" OnClick="btn_itemok_Click"
                                        CssClass="textbox btn2" />
                                    <asp:Button ID="btn_itemexit" Visible="false" runat="server" Text="Exit" CssClass="textbox btn2"
                                        OnClick="btn_itemexit_Click" />
                                </div>
                        </center>
                    </div>
                </center>
                <%--    Error--%>
            </div>
            <div id="imgdiv2" runat="server" visible="false" class="popupstyle" style="height: 50em;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errclose" CssClass=" textbox btn2 comm" OnClick="btn_errclose_Click"
                                                Text="OK" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
            <center>
                <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_sure" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_yes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                                <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btn_sureno_Click" Text="no" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
