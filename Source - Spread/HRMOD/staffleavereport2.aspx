<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="staffleavereport2.aspx.cs" Inherits="staffleavereport2"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
        }
    </script>
    <div>
        <center>
            <br />
            <div>
                <span class="fontstyleheader" style="color: #008000;">Staff Cumulative Leave Report</span>
            </div>
        </center>
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <table class="maintablestyle ">
                <tr>
                    <td>
                        <asp:Label ID="lbldept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Department"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbseattype" runat="server" CssClass="Dropdown_Txt_Box" Height="20px"
                            ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            Width="124px">---Select---</asp:TextBox>
                        <asp:Panel ID="pseattype" runat="server" CssClass="multxtpanel" Width="400px" Height="400">
                            <asp:CheckBox ID="chkselect" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Small" OnCheckedChanged="chkselect_CheckedChanged"
                                Text="Select All" />
                            <asp:CheckBoxList ID="cbldepttype" runat="server" OnSelectedIndexChanged="cbldepttype_SelectedIndexChanged"
                                AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <br />
                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="tbseattype"
                            PopupControlID="pseattype" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblcategory" runat="server" Font-Bold="True" Width="110px" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Category"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbblood" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120px">---Select---</asp:TextBox>
                        <br />
                        <asp:Panel ID="pblood" runat="server" CssClass="multxtpanel" Height="400" Width="300px">
                            <asp:CheckBox ID="chkcategory" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnCheckedChanged="chkcategory_CheckedChanged" Text="Select All"
                                AutoPostBack="True" />
                            <asp:CheckBoxList ID="cblcategory" runat="server" Font-Size="Medium" AutoPostBack="True"
                                OnSelectedIndexChanged="cblcategory_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="tbblood"
                            PopupControlID="pblood" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label13" runat="server" Font-Bold="True" Width="90px" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Leavetype"></asp:Label>
                    </td>
                    <td>
                        <%-- <asp:DropDownList ID="cblleavetype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="144px" OnSelectedIndexChanged="cblleavetype_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>--%>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtleave" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pleave" runat="server" Height="300px" Width="300px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chkleave" runat="server" Font-Bold="True" OnCheckedChanged="chkleave_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsleave" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsleave_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtleave"
                                    PopupControlID="pleave" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblto" runat="server" Font-Bold="True" Text="To" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:Label ID="lblyear" runat="server" Text="Year" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="Txtentryto" runat="server" Height="20px" Width="75px" Font-Bold="True"
                            Font-Names="Book Antiqua" OnTextChanged="Txtentryto_TextChanged"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txtentryto"
                            FilterType="Custom, Numbers" ValidChars="/" />
                        <asp:CalendarExtender ID="Txtentryto_CalendarExtender" runat="server" TargetControlID="Txtentryto"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="reqdateto" runat="server" ControlToValidate="Txtentryto"
                            ErrorMessage="Please enter the  to Date" ForeColor="Red" Style="top: 54px; left: 504px;
                            position: absolute; height: 16px; width: 161px"></asp:RequiredFieldValidator>
                        <asp:Label ID="lbldate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            ForeColor="Red" Visible="False"></asp:Label>
                        <asp:DropDownList ID="ddlyear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            AutoPostBack="true" Font-Size="Medium" Visible="False" OnSelectedIndexChanged="ddlyear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblda" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            Text="Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="Txtentryfrom" runat="server" Style="margin-top: 17px;" Width="75px"
                            Font-Bold="True" Font-Names="Book Antiqua" Enabled="False"></asp:TextBox>
                        <asp:CalendarExtender ID="Txtentryfrom_CalendarExtender" runat="server" TargetControlID="Txtentryfrom"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtmonth" runat="server" ReadOnly="true" Style="font-family: 'Book Antiqua';"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="Dropdown_Txt_Box"
                                    Visible="False" Width="120px">---Select---</asp:TextBox>
                                <br />
                                <asp:Panel ID="pmonth" runat="server" CssClass="multxtpanel" Width="300px" Height="300"
                                    Visible="False">
                                    <asp:CheckBoxList ID="chkmonth" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="321px" Height="345px" Font-Bold="True" Font-Names="Book Antiqua" Visible="False"
                                        OnSelectedIndexChanged="chkmonth_SelectedIndexChanged">
                                        <asp:ListItem Value="1">January</asp:ListItem>
                                        <asp:ListItem Value="2">Febrarury</asp:ListItem>
                                        <asp:ListItem Value="3">March</asp:ListItem>
                                        <asp:ListItem Value="4">April</asp:ListItem>
                                        <asp:ListItem Value="5">May</asp:ListItem>
                                        <asp:ListItem Value="6">June</asp:ListItem>
                                        <asp:ListItem Value="7">July</asp:ListItem>
                                        <asp:ListItem Value="8">Auguest</asp:ListItem>
                                        <asp:ListItem Value="9">September</asp:ListItem>
                                        <asp:ListItem Value="10">Octobar</asp:ListItem>
                                        <asp:ListItem Value="11">November</asp:ListItem>
                                        <asp:ListItem Value="12">December</asp:ListItem>
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtmonth"
                                    PopupControlID="pmonth" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblstaffname" runat="server" Text="Staff Name" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cbostaffname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="130px" OnSelectedIndexChanged="cbostaffname_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RadioButton ID="rdoyearlywise" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" GroupName="s" Text="Yearly Wise" AutoPostBack="True" OnCheckedChanged="rdoyearlywise_CheckedChanged"
                            Checked="True" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rdomonthlywise" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" GroupName="s" Text="Monthly wise" AutoPostBack="True" OnCheckedChanged="rdomonthlywise_CheckedChanged" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rdodaywise" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" GroupName="s" Text="Day wise" AutoPostBack="True" OnCheckedChanged="rdodaywise_CheckedChanged" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkstaffleave" runat="server" Font-Bold="True" Text="Leave Staff Only"
                            Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_includeprevious" runat="server" Font-Bold="True" Text="Include Previous Leave Balance"
                            Font-Names="Book Antiqua" AutoPostBack="true" OnCheckedChanged="chk_includeprevious_change"
                            Checked="false" Font-Size="Medium" Visible="false" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_includeoverall" runat="server" Font-Bold="True" Text="Without Overall Balance"
                            Font-Names="Book Antiqua" AutoPostBack="true" OnCheckedChanged="chk_includeoverall_change"
                            Checked="false" Font-Size="Medium" Visible="false" />
                    </td>
                    <td>
                        <asp:Button ID="Button1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            OnClick="btngo_Click" Text="GO" Font-Size="Medium" CssClass=" textbox btn1" OnClientClick="return validation()"
                            Style="width: 40px; height: 30px;" />
                    </td>
                </tr>
            </table>
        </center>
        <br />
        <center>
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                ForeColor="Red" Text="No Record Found" Visible="False"></asp:Label>
        </center>
        <br />
        <center>
            <%--<FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                            BorderWidth="0px" Width="600px" Style="overflow: auto; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            class="spreadborder" OnCellClick="FpSpread1_OnCellClick" OnPreRender="FpSpread1_Selectedindexchange">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>--%>
            <div id="div2" runat="server" visible="true" style="overflow: auto; background-color: White;
                border-radius: 10px;">
                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                <asp:GridView ID="grdleave" Width="900px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                    Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowDataBound="grdleave_RowDataBound"
                    OnRowCreated="grdleave_OnRowCreated" OnSelectedIndexChanged="grdleave_SelectedIndexChanged">
                    <%-- --%>
                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                </asp:GridView>
            </div>
            <%--  <FarPoint:FpSpread ID="fpsalary" Visible="false" runat="server" BorderColor="Black"
                OnPreRender="fpsalary_PreRender" OnCellClick="fpsalary_CellClick" BorderStyle="Solid"
                BorderWidth="1px" CssClass="spreadborder">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>--%>
            <%--delsi--%>
            <center>
                <div id="ViewReport" runat="server" visible="false" class="popupstyle popupheight">
                    <br />
                    <div class="subdivstyle" style="background-color: White; height: 668px; width: 1000px;">
                        <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: -37px; margin-left: 477px;"
                            OnClick="imagebtnpopclose1_Click" />
                        <br />
                        <center>
                            <asp:Label ID="lbl_staffLeave" runat="server" Font-Bold="true" Style="font-size: large;
                                color: Green;" Text="Staff Leave List"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <FarPoint:FpSpread ID="FpstaffLeave" runat="server" Visible="false" Width="600px"
                                Style="overflow: auto; height: 300px; border: 0px solid #999999; border-radius: 10px;
                                background-color: White; box-shadow: 0px 0px 8px #999999;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </center>
                    </div>
                </div>
            </center>
            <br />
            <div id="rptprint" runat="server" visible="false">
                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label>
                <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                    Width="180px" onkeypress="display()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                    Text="Export To Excel" Width="127px" Height="30px" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                    CssClass="textbox btn1" Height="30px" />
                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
            </div>
        </center>
        <br />
    </div>
</asp:Content>
