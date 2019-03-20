<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="HR_Reconciliation.aspx.cs" Inherits="HR_Reconciliation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_Label1').innerHTML = "";
        }

        function validation() {

            var ddlfirst = document.getElementById("<%=ddlfrom.ClientID %>");
            var ddlsecond = document.getElementById("<%=ddlto.ClientID %>");
            var ddlthird = document.getElementById("<%=ddltoyear.ClientID %>");
            var go = document.getElementById("<%=Button1.ClientID %>");

            if ((ddlfirst.value == "---Select---") && (ddlsecond.value == "---Select---") && (ddlthird.value == "Select")) {
                alert("Please Select From Month and To Month and To Year");
                return false;
            }
            else if ((ddlfirst.value == "---Select---") && (ddlsecond.value == "---Select---")) {
                alert("Please Select From Month and To Month");
                return false;
            }
            else if ((ddlsecond.value == "---Select---") && (ddlthird.value == "Select")) {
                alert("Please Select To Month and  To Year");
                return false;
            }
            else if (ddlfirst.value == "---Select---") {

                alert("Please Select From Month");
                return false;
            }
            else if (ddlsecond.value == "---Select---") {
                alert("Please Select To Month");
                return false;
            }
            else if (ddlthird.value == "Select") {
                alert("Please Select To Year");
                return false;
            }

        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <br />
        </center>
        <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="top: 85px;
            left: -14px; position: absolute; width: 102%; height: 21px">
            <center>
                <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="White" Text="HR Reconciliation"></asp:Label>
            </center>
        </asp:Panel>
        <br />
        <div style="width: 985px; height: 158px; background-color: #0CA6CA;">
            <table>
                <tr>
                    <td>
                        <asp:RadioButton ID="Rbtformat1" Text="Format 1" Style="font-family: Book Antiqua;
                            font-size: large; color: Black; font-weight: 400;" GroupName="Reconsilation"
                            runat="server" OnCheckedChanged="Rbtformat1_OnCheckedChanged" AutoPostBack="true" />
                    </td>
                    <td>
                        <asp:RadioButton ID="Rbtformat2" Text="Format 2" Style="font-family: Book Antiqua;
                            font-size: large; color: Black; font-weight: 400;" GroupName="Reconsilation"
                            runat="server" OnCheckedChanged="Rbtformat2_OnCheckedChanged" AutoPostBack="true" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblfrommonth" runat="server" Text="From Month Year:" Style="font-family: Book Antiqua;
                            font-size: large; color: Black; font-weight: 400;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlfrom" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlfrom_selectchange">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlfromyear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlfromyear_selectchange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbtomonth" runat="server" Text="To Month Year:" Style="font-family: Book Antiqua;
                            font-size: large; color: Black; font-weight: 400;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlto" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlto_selectchange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddltoyear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Stream:" Style="font-family: Book Antiqua;
                            font-size: large; color: Black; font-weight: 400;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsteam" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            AutoPostBack="true" Height="25px" Width="121px" OnSelectedIndexChanged="ddlsteam_selectchange">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="Staff Type:" Style="font-family: Book Antiqua;
                            font-size: large; color: Black; font-weight: 400;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlstftype" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            AutoPostBack="true" Height="25px" Width="165px" OnSelectedIndexChanged="ddlstftype_selectchange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblstaffcategory" runat="server" Text="Staff Category" Style="font-family: Book Antiqua;
                            font-size: large; color: Black; font-weight: 400;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel_f1_category" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_f1_category" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="133px" Height="20px">---Select---</asp:TextBox>
                                <asp:Panel ID="pnl_f1_category" runat="server" Height="100px" Width="200px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="Cb_f1_category" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="cb_f1_Category_CheckedChanged" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklst_f1_category" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklst_f1_Category_SelectedIndexChanged" Font-Bold="True"
                                        Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="Pop_f1_category" runat="server" TargetControlID="txt_f1_category"
                                    PopupControlID="pnl_f1_category" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnk_btn_print" runat="server" Text="Print Settings" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="lnk_btn_print_click"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Go" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClientClick="return validation()" ForeColor="#000000" OnClick="Button1_Click" />
                    </td>
                </tr>
            </table>
            <table style="margin-top: -210px; position: relative;">
                <tr>
                    <td>
                        <asp:Label ID="lblclg" runat="server" Text="College" font-name="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" ForeColor="Black" Style="top: 220px; left: 4px; position: absolute;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" AutoPostBack="true" Width="172px"
                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="25px" Style="top: 220px;
                            left: 65px; position: absolute;" OnSelectedIndexChanged="ddlcollege_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbldeg" runat="server" Text="Department" font-name="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" ForeColor="Black" Style="top: 220px; left: 241px;
                            position: absolute;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdept" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="100px" Style="top: 220px; left: 339px; position: absolute; font-family: 'Book Antiqua';"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pdept" runat="server" Height="400px" Width="300px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chkdept" runat="server" Font-Bold="True" OnCheckedChanged="chkdept_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsdept" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsdept_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdept"
                                    PopupControlID="pdept" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbldept" runat="server" Text="Designation" font-name="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" Width="50px" ForeColor="Black" Style="top: 220px;
                            left: 445px; position: absolute;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdesign" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="100px" Style="top: 220px; left: 540px; position: absolute; font-family: 'Book Antiqua';"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pdesign" runat="server" Height="300px" Width="300px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chkdesign" runat="server" Font-Bold="True" OnCheckedChanged="chkdesign_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsdesign" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsdesign_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdesign"
                                    PopupControlID="pdesign" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="Label4" runat="server" Text="Staff Category" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Width="108px" Style="top: 220px;
                            left: 648px; position: absolute; font-family: 'Book Antiqua';"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel_Category" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_Category" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="133px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Style="top: 220px; left: 757px; position: absolute; font-family: 'Book Antiqua';">---Select---</asp:TextBox>
                                <asp:Panel ID="panel_Category" runat="server" Height="100px" Width="200px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chk_Category" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="chk_Category_CheckedChanged" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklst_Category" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklst_Category_SelectedIndexChanged" Font-Bold="True"
                                        Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_Category"
                                    PopupControlID="panel_Category" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblmon" runat="server" Text="Month" font-name="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="50px" ForeColor="Black" Style="top: 253px; left: 5px;
                            position: absolute;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlmonth" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                            Font-Bold="true" Font-Size="Medium" Height="25px" Style="top: 250px; left: 66px;
                            position: absolute;" OnSelectedIndexChanged="ddlmonth_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblyr" runat="server" Text="Year" font-name="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="50px" ForeColor="Black" Style="top: 253px; left: 174px;
                            position: absolute;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlyear" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                            Font-Bold="true" Font-Size="Medium" Height="25px" Style="top: 250px; left: 210px;
                            position: absolute;" OnSelectedIndexChanged="ddlyear_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Text="Go" Width="45px" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnClick="btngo_OnClick" Style="top: 250px; left: 272px;
                            position: absolute;" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <center>
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                ForeColor="Red" Font-Size="Medium" Text="No Record Were Found" Visible="False"></asp:Label>
        </center>
        <center>
            <asp:GridView ID="gridview1" runat="server" Font-Size="Medium" CssClass="style4"
                AutoGenerateColumns="false">
                <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="white" />
                <RowStyle HorizontalAlign="Center" BackColor="white" ForeColor="Black"></RowStyle>
                <Columns>
                    <asp:TemplateField HeaderText="S.No" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="lblsno" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                            <asp:Label ID="lblmonthnumber" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Visible="false" Text='<%#Eval("Number") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Select">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbselectall" ItemStyle-VerticalAlign="Top" CssClass="gridCB" runat="server"
                                Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="cbselectall_change">
                            </asp:CheckBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbSelect" CssClass="gridCB" Font-Names="Book Antiqua" Font-Size="Medium"
                                runat="server"></asp:CheckBox>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Month" ItemStyle-Width="100px" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="lblmonth" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text='<%#Eval("Months") %>'></asp:Label>
                            <asp:Label ID="lblmonthnum" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Visible="false" Text='<%#Eval("monthnum") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="80px" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Overall Salary" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="lbloverall" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text='<%#Eval("Overall Salary") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="100px" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Add" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="lbladd" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text='<%# Eval("Add") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="100px" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Less" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="lblless" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text='<%#Eval("Less") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="100px" HorizontalAlign="Right" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </center>
        <center>
            <div id="printpopup" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div5" runat="server" class="table" style="background-color: White; height: 130px;
                        width: 359px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: auto; width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_print" runat="server" Text="Footer Name" Style="color: Black;
                                            width: 165px;" Font-Bold="true" Font-Size="Medium" ></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_print" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                        <asp:Label ID="error" runat="server" Font-Bold="True" Font-Names="Book Antiqua" ForeColor="Red"
                                            Font-Size="Medium" Visible="false" Text="Saved Successfully"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btn_printSave" CssClass=" textbox1 btn2" Width="100px" OnClick="btnsavePrint_Click"
                                Text="Save" runat="server" />
                            <asp:Button ID="btn_printexit" CssClass=" textbox1 btn2" Width="100px" OnClick="btnexitPrint_Click"
                                Text="Exit" runat="server" />
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <br />
            <asp:Button ID="btngen" Visible="false" CssClass="style8" runat="server" Font-Bold="true"
                Font-Names="Book Antiqua" Font-Size="Medium" Text="Generate" OnClick="btngen_Click" />
        </center>
        <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
            Width="960px" BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Always"
            OnCellClick="fpspread_CellClick" OnPreRender="fpspread_SelectedIndexChanged"
            ShowHeaderSelection="false">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                Font-Names="Book Antiqua" ButtonShadowColor="ControlDark" ButtonType="PushButton"
                Visible="false">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <table>
            <tr>
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
                <td>
                    <asp:Button ID="Excel" runat="server" Text="Export Excel" Visible="false" Font-Size="Medium"
                        Font-Bold="true" OnClick="Excel_OnClick" Font-Names="Book Antiqua" />
                </td>
                <td>
                    <asp:Button ID="Print" runat="server" Text="Print" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true" OnClick="Print_OnClick" Visible="false" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </td>
            </tr>
        </table>
</asp:Content>
