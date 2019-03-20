<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master" AutoEventWireup="true" CodeFile="certificateissues.aspx.cs" Inherits="certificateissues" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .style1
        {
            height: 238px;
        }
        .font
        {
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblerrex').innerHTML = "";

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <asp:ScriptManager ID="ScriptManager1" runat="server" /><br />
 <center>
  <asp:Label ID="Label31" runat="server" Text="Certificate Issue" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Green"></asp:Label>
                        </center>    <br />  
                        <center>
        <table style="width:900px; height:110px; background-color:#0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label></td>
                        <td>
                    <asp:DropDownList ID="ddlcoollege" runat="server" Height="23px" Width="139px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlcoollege_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label></td>
                        <td>
                        <div style="position:relative;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="tbbatch" runat="server" Font-Bold="True" ReadOnly="true" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--" Style="width: 106px; height: 20px; " AutoPostBack="True" OnTextChanged="tbbatch_TextChanged"></asp:TextBox>
                            <asp:Panel ID="pbatch" runat="server" CssClass="MultipleSelectionDDL" Height="200"
                                Width="110" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                                <asp:CheckBox ID="Chkbatch" runat="server" Text="Select All" AutoPostBack="true"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                    OnCheckedChanged="Chkbatch_CheckedChanged"/>
                                <asp:CheckBoxList ID="chklbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklbatch_SelectedIndexChanged"
                                    Height="37px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="tbbatch"
                                PopupControlID="pbatch" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label></td>
                        <td>
                        <div style="position:relative;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="tbdegree" runat="server" Font-Bold="True" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                                Style="width: 106px; height: 20px;" AutoPostBack="True" OnTextChanged="tbdegree_TextChanged"></asp:TextBox>
                            <asp:Panel ID="pdegree" runat="server" CssClass="MultipleSelectionDDL" Height="200"
                                Width="120" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                                <asp:CheckBox ID="chkdegree" runat="server" Text="Select All" AutoPostBack="true"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                    OnCheckedChanged="chkdegree_CheckedChanged" Checked="false" />
                                <asp:CheckBoxList ID="chkldegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="chkldegree_SelectedIndexChanged"
                                    Height="37px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="tbdegree"
                                PopupControlID="pdegree" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel></div>
                </td>
                <td>
                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label></td>
                        <td>
                        <div style="position:relative;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="tbbranch" runat="server" Font-Bold="True" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                                Style="width: 106px; height: 20px; " AutoPostBack="True" OnTextChanged="tbbranch_TextChanged"></asp:TextBox>
                            <asp:Panel ID="pbranch" runat="server" CssClass="MultipleSelectionDDL" Height="200"
                                Width="220px" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                                <asp:CheckBox ID="chkbranch" runat="server" Text="Select All" AutoPostBack="true"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                    OnCheckedChanged="chkbranch_CheckedChanged" Checked="false" />
                                <asp:CheckBoxList ID="chklbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklbranch_SelectedIndexChanged"
                                    Height="37px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="tbbranch"
                                PopupControlID="pbranch" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel></div>
                </td>
                <td>
                    <asp:Label ID="lblsemester" runat="server" Text="Semester" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label></td>
                        <td>
                        <div style="position:relative">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="tbsem" runat="server" Font-Bold="True" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                                Style="width: 83px;height: 20px;" AutoPostBack="True" OnTextChanged="tbsem_TextChanged"></asp:TextBox>
                            <asp:Panel ID="psem" runat="server" CssClass="MultipleSelectionDDL" Height="180"
                                Width="100" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                                <asp:CheckBox ID="chksem" runat="server" Text="Select All" AutoPostBack="true" Font-Bold="True"
                                    ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chksem_CheckedChanged"
                                    Checked="false" />
                                <asp:CheckBoxList ID="chklsem" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsem_SelectedIndexChanged"
                                    Height="37px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="tbsem"
                                PopupControlID="psem" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel></div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblsec" runat="server" Text="Section" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label></td>
                    <td>
                        <div style="position:relative;">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="tbsec" runat="server" Font-Bold="True" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                                Style="width: 106px;height: 20px; " AutoPostBack="True"></asp:TextBox>
                            <asp:Panel ID="psec" runat="server" CssClass="MultipleSelectionDDL" Height="105px"
                                Width="125" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                                <asp:CheckBox ID="chksec" runat="server" Text="Select All" AutoPostBack="true" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chksec_CheckedChanged"
                                    Checked="false" />
                                <asp:CheckBoxList ID="chklsec" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsec_SelectedIndexChanged"
                                    Height="37px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="tbsec"
                                PopupControlID="psec" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel></div>
                </td>
                <td>
                    <asp:Label ID="lblrollno" runat="server" Text="Roll No" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label></td>
                        <td>
                    <asp:TextBox ID="tbrollno" runat="server" Font-Bold="True" Style="height: 19px; width: 106px;"
                        Font-Names="Book Antiqua" Font-Size="Medium" Text=" "></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbltype" runat="server" Text="Type" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label></td>
                    <td>
                    <div style="position:relative;">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="tbtype" runat="server" Font-Bold="True" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                                Style="width: 106px; height: 20px'" AutoPostBack="True"></asp:TextBox>
                            <asp:Panel ID="ptype" runat="server" CssClass="MultipleSelectionDDL" Height="129px"
                                Width="125" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                                <asp:CheckBox ID="chktype" runat="server" Text="Select All" AutoPostBack="true" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chktype_CheckedChanged"
                                    Checked="false" />
                                <asp:CheckBoxList ID="chkltype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="chkltype_SelectedIndexChanged"
                                    Height="37px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="tbtype"
                                PopupControlID="ptype" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel></div>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Text="GO" Font-Size="Medium" OnClick="btngo_Click" />
                </td>
            </tr>
        </table>
    </center>
    <br />
    <asp:Label ID="lblmessage" runat="server" Text="" ForeColor="Red" Visible="False"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <%--<FarPoint:FpSpread ID="Fpspread1" runat="server" AutoPostBack="true" OnCellClick="Fpspread_CellClick"  OnPreRender="Fpspread_PreRender">
                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                            ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>--%>
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" AutoPostBack="false" BorderColor="Black"
                            CssClass="cur" BorderStyle="Solid" BorderWidth="1px" OnButtonCommand="Fpspread1_Command"
                            OnPreRender="Fpspread1_PreRender" OnCellClick="Fpspread1_CellClick" Height="300"
                            Width="623" Visible="False">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black" AutoPostBack="false">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Button ID="btnissue" runat="server" Text="Issue" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnissue_Click" />
                <asp:TextBox ID="txtfoc" runat="server" Height="0px" Width="0px" Style="opacity: 0;"></asp:TextBox>
                <%-- <asp:ModalPopupExtender ID="mpemsgboxupdate" runat="server" TargetControlID="hfupdate"
          PopupControlID="pnlmsgboxupdate1"    >
         </asp:ModalPopupExtender>
         <asp:HiddenField  runat="server" ID="hfupdate" />--%>
                <br />
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnxl_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
                    Font-Size="Medium" Font-Bold="true" OnClick="btnprintmaster_Click" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblmessage1" runat="server" Text="" ForeColor="Red" Visible="False"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlmsgboxupdate1" runat="server" CssClass="modalPopup" BorderColor="Black"
        BorderWidth="1Px" BackColor="#ffffcc" Style="top: 334px; position: fixed; left: 250px;">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblissu" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Issue"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlissu" runat="server" Height="23px" Width="161px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblissudate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Issue Date"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtissuedate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="" Style="height: 19px; width: 97px;">
                    </asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtissuedate">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="lblissutime" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Issue Time"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlintimehh" runat="server" AutoPostBack="true" CssClass="font"
                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="26px" OnSelectedIndexChanged="ddlintimehh_SelectedIndexChanged"
                        Width="52px">
                        <asp:ListItem>HH</asp:ListItem>
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                    <asp:DropDownList ID="ddlintimemm" runat="server" AutoPostBack="true" CssClass="font"
                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="26px" OnSelectedIndexChanged="ddlintimemm_SelectedIndexChanged"
                        Width="56px">
                        <asp:ListItem>MM</asp:ListItem>
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                    <asp:DropDownList ID="ddlintimeses" runat="server" AutoPostBack="true" CssClass="font"
                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="26px" Width="52px">
                        <asp:ListItem>AM</asp:ListItem>
                        <asp:ListItem>PM</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblissueperson" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Issue Person"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtissueper" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="" Style="height: 20px;"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnstaff" runat="server" Text="?" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnstaff_click" />
                </td>
                <td>
                    <asp:TextBox ID="txtstaff_co" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="" Style="opacity: 0; height: 0; width: 0;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnok" runat="server" Text="OK" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnok_click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btncncl" runat="server" Text="Cancel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btncncl_click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panel8" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
        BorderWidth="2px" Style="background-color: AliceBlue; border-color: Black; border-width: 2px;
        border-style: solid; position: fixed; width: 520px; height: 440px; left: 250px;
        top: 99px;">
        <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
            font-size: Small; font-weight: bold">
            <br />
            <asp:Label ID="Label19" runat="server" Text=" Staff List" Style="width: 150px; position: absolute;
                left: 166px; top: 4px;"></asp:Label>
            <%-- <caption style="top: 20px; border-style: solid; border-color: Black; position: absolute;
                        left: 200px">
                        Staff List
                    </caption>--%>
            <br />
            <br />
            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="College" Style="width: 150px; position: absolute;
                                    left: -41px; top: 30px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" Width="150px" Style="width: 150px;
                                    position: absolute; left: 70px; top: 30px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDepartment" runat="server" Text="Department" Style="width: 150px;
                                    position: absolute; left: 237px; top: 30px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldepratstaff" runat="server" AutoPostBack="true" Width="150px"
                                    OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged" Style="width: 150px;
                                    position: absolute; left: 360px; top: 30px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="Staff Type" Style="width: 150px; position: absolute;
                                    left: -41px; top: 65px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_stftype" runat="server" Width="150px" OnSelectedIndexChanged="ddl_stftype_SelectedIndexChanged"
                                    AutoPostBack="true" Style="width: 150px; position: absolute; left: 70px; top: 65px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label21" runat="server" Text="Designation" Style="width: 150px; position: absolute;
                                    left: 237px; top: 65px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_design" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddl_design_SelectedIndexChanged" Style="width: 150px;
                                    position: absolute; left: 360px; top: 65px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblsearchby" runat="server" Text="Staff By"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                    <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="width: 510px; position: absolute; top: 95px;">
                        <FarPoint:FpSpread ID="fsstaff" runat="server" ActiveSheetViewIndex="0" Height="300"
                            Width="510" VerticalScrollBarPolicy="AsNeeded" BorderWidth="0.5" Visible="False"
                            OnUpdateCommand="fsstaff_UpdateCommand" OnCellClick="fsstaff_CellClick">
                            <CommandBar BackColor="Control" ButtonType="PushButton">
                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <fieldset style="width: 160px; position: absolute; padding: 4px 1em 19px; left: 328px;
                height: 9px; top: 388px;">
                <asp:Button runat="server" ID="btnstaffadd" OnClick="btnstaffadd_Click" Width="75px" />
                <asp:Button runat="server" ID="btnexitpop" Text="Exit" OnClick="exitpop_Click" Width="75px" />
            </fieldset>
    </asp:Panel>
</asp:Content>

