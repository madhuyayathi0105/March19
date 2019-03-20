<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="hrsalaryincomepf.aspx.cs" Inherits="hrsalaryincomepf" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .style1
        {
            border-color: Black;
            height: 25px;
            width: 120px;
        }
        .style2
        {
            font-weight: normal;
            font-size: medium;
            border-color: Black;
        }
        
        
        .style3
        {
            border-color: Black;
            height: 25px;
            width: 150px;
        }
        .style5
        {
            height: 25px;
        }
        .style7
        {
            text-align: center;
        }
        .style8
        {
            left: 441px;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
        }
        function validation() {
            var error = "";

            var ddlfrmmnth = document.getElementById("<%=ddlfrommnth.ClientID %>");
            var ddltomonth = document.getElementById("<%=ddltomnth.ClientID %>");
            var ddltoyr = document.getElementById("<%=ddltoyr.ClientID %>");
            var txdept = document.getElementById("<%=TBDEPT1.ClientID %>");
            var txdesign = document.getElementById("<%=TBDESIGN2.ClientID %>");
            var st_name = document.getElementById("<%=TBST_NAME3.ClientID %>");
            var go = document.getElementById("<%=Button1.ClientID %>");

            if (ddlfrmmnth.value == "---Select---") {
                error += "Please Select From Month \n";

            }
            if (ddltomonth.value == "---Select---") {

                error += "Please Select To Month \n";

            }
            if (ddltoyr.value == "---Select---") {
                error += "Please Select To Year \n";
            }
            if (txdept.value == "--Select--") {

                error += "Please Select Department \n";
            }
            if (txdesign.value == "--Select--") {

                error += "Please Select Designation \n";

            }
            if (st_name.value == "--Select--") {

                error += "Please Select StaffName \n";

            }
            if (error != "") {

                alert(error);
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div style="width: 960px; height: 30px; background-color: Control;" class="style7">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <span style="font-size: large; color: Green; font-family: Book Antiqua; font-weight: bold;">
                    Income Tax Calculation & PF Settlement Report</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
            <center>
                <div style="width: 960px; height: 96px; background-color: #0CA6CA;" class="maintablestyle">
                    <table style="margin-left: 0px; margin-top: 0px; position: absolute; line-height: 20px;">
                        <tr>
                            <td>
                                <span style="font-family: Book Antiqua; font-size: medium; color: Black; font-weight: 600;">
                                    From Month&Year</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlfrommnth" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                    Font-Size="medium" CssClass="style5" Width="100px" OnSelectedIndexChanged="ddlfrommnth_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
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
                                <asp:DropDownList ID="ddlfrmyr" runat="server" AutoPostBack="true" CssClass="style5"
                                    Width="80px" Font-Names="Book Antiqua" Font-Size="medium" OnSelectedIndexChanged="ddlfrmyr_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span style="font-family: Book Antiqua; font-size: medium; color: Black; font-weight: 600;">
                                    To Month&Year</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltomnth" runat="server" AutoPostBack="true" CssClass="style5"
                                    Width="100px" Font-Names="Book Antiqua" Font-Size="medium" OnSelectedIndexChanged="ddltomnth_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
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
                                <asp:DropDownList ID="ddltoyr" runat="server" AutoPostBack="true" CssClass="style5"
                                    Width="80px" Font-Names="Book Antiqua" Font-Size="medium" OnSelectedIndexChanged="ddltoyr_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span style="font-family: Book Antiqua; font-size: medium; color: Black; font-weight: 600;">
                                    Department</span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="up1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="TBDEPT1" runat="server" CssClass="style2 style5 Dropdown_Txt_Box"
                                            Style="position: absolute; top: 2px; left: 735px;">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel7" runat="server" CssClass="multxtpanel" Width="220px" Height="300px">
                                            <asp:CheckBox ID="chkdept" runat="server" CssClass="style1 style2 style3" Font-Names="Book Antiqua"
                                                Font-Size="medium" AutoPostBack="true" Text="Select All" OnCheckedChanged="chkdept_CheckedChanged" />
                                            <asp:CheckBoxList ID="chklistdept" runat="server" CssClass="style2" Font-Names="Book Antiqua"
                                                Font-Size="medium" AutoPostBack="true" OnSelectedIndexChanged="chklistdept_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popctrl" runat="server" TargetControlID="TBDEPT1" PopupControlID="panel7"
                                            Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span style="font-family: Book Antiqua; font-size: medium; color: Black; font-weight: 600;">
                                    Designation</span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="up2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="TBDESIGN2" runat="server" CssClass="style2 style3 style5 Dropdown_Txt_Box"
                                            Style="position: absolute; top: 31px; left: 145px;">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel8" runat="server" CssClass="multxtpanel" Width="227px" Height="300px">
                                            <asp:CheckBox ID="chkdesign" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                Font-Size="medium" CssClass="style1 style2 style3" Text="Select All" OnCheckedChanged="chkdesign_CheckedChanged" />
                                            <asp:CheckBoxList ID="chklistdesign" runat="server" AutoPostBack="true" CssClass="style2"
                                                Font-Names="Book Antiqua" Font-Size="medium" OnSelectedIndexChanged="chklistdesign_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popctrl2" runat="server" PopupControlID="panel8" Position="Bottom"
                                            TargetControlID="TBDESIGN2">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <span style="font-family: Book Antiqua; font-size: medium; color: Black; font-weight: 600;">
                                    Staff Name</span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="up4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="TBST_NAME3" runat="server" CssClass="style2 style3 style5 Dropdown_Txt_Box"
                                            Style="position: absolute; top: 31px; left: 456px;">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel9" runat="server" CssClass="multxtpanel " Width="285px" Height="300px">
                                            <asp:CheckBox ID="chkstname" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                Font-Size="medium" CssClass="style1 style2 style3" Text="Select All" OnCheckedChanged="chkstname_CheckedChanged" />
                                            <asp:CheckBoxList ID="chkliststname" runat="server" AutoPostBack="true" CssClass="style2"
                                                Font-Names="Book Antiqua" Font-Size="medium" OnSelectedIndexChanged="chkliststname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popctrl3" runat="server" PopupControlID="panel9" Position="Bottom"
                                            TargetControlID="TBST_NAME3">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <span style="font-family: Book Antiqua; font-size: medium; color: Black; font-weight: 600;">
                                    Type</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltype" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                    CssClass="style5" Style="width: 163px;">
                                    <asp:ListItem Value="1">Income Tax</asp:ListItem>
                                    <asp:ListItem Value="2">PF</asp:ListItem>
                                    <asp:ListItem Value="3">Tax payable Income</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Staff Type
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updstafftype" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_stafftyp" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlstafftyp" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="200px">
                                            <asp:CheckBox ID="cb_stafftyp" runat="server" AutoPostBack="true" OnCheckedChanged="cb_stafftyp_CheckedChanged"
                                                Text="Select All" />
                                            <asp:CheckBoxList ID="cbl_stafftyp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stafftyp_selectedchanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" PopupControlID="pnlstafftyp"
                                            TargetControlID="txt_stafftyp" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                            </td>
                            
                            <td colspan="2">
                                <asp:Button ID="Button1" CssClass="btnapprove2" runat="server" Font-Bold="true" Style="width: 40px;
                                    height: 27px" Text="Go" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black"
                                    BackColor="DarkGray" OnClientClick="return validation()" OnClick="Click" />
                                      <asp:CheckBox ID="cb_relived" runat="server" AutoPostBack="true" Text="Include Relieved Staff"   OnCheckedChanged="cbrelived_checkchange" Style="font-weight: bold;" />
                            </td>
                        </tr>
                        <%-- <tr><td><asp:CheckBox ID="CbShowDiscription" runat="server" Text="Show Discription" Style="font-weight: bold;" /></td></tr>--%>
                    </table>
                </div>
                <div style="width: 946px; text-align: right;">
                    <asp:Button ID="Buttongen1" runat="server" Visible="false" Font-Bold="true" Style="width: 93px;
                        height: 28px;" Text="Generate" OnClick="butgen1_Click" />
                </div>
                <div>
                    <asp:Label ID="lbl" runat="server" Font-Bold="true" Font-Names="Book Antiqua" ForeColor="Red"></asp:Label>
                </div>
                <center>
                    <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        ForeColor="Red" Text="No Record Were Found" Visible="False"></asp:Label>
                    <asp:GridView ID="typegrid" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
                        Font-Size="medium" OnDataBound="gridbound" Style="height: 190px; width: 960px;">
                        <HeaderStyle BackColor="#0CA6CA" Height="38px" Font-Size="medium" ForeColor="White"
                            Font-Names="Book Antiqua" />
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lblsno" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="cbselectall" ItemStyle-VerticalAlign="Top" CssClass="gridCB" runat="server"
                                        AutoPostBack="true" OnCheckedChanged="cbselectall_change"></asp:CheckBox>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <center>
                                        <asp:CheckBox ID="gridCkBox1" runat="server" />
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="StaffCode">
                                <ItemTemplate>
                                    <asp:Label ID="lblstaff_code" runat="server" Visible="true" Text='<%#Eval("staff_code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="StaffName">
                                <ItemTemplate>
                                    <asp:Label ID="lblstaff" runat="server" Text='<%#Eval("Staff Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Department">
                                <ItemTemplate>
                                    <asp:Label ID="lbldept" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                    <asp:Label ID="lbldept_code" runat="server" Visible="false" Text='<%#Eval("Dept_code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Designation">
                                <ItemTemplate>
                                    <asp:Label ID="lbldesig" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                    <asp:Label ID="lbldesig_code" runat="server" Visible="false" Text='<%#Eval("desig_code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actual Salary">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lbltot" runat="server" Text='<%#Eval("Actual Salary") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Button ID="butgen" runat="server" Visible="false" Font-Bold="true" Text="Generate"
                        Style="width: 93px; height: 28px;" OnClick="butgen_Click" />
                    <%--28.02.18 barath--%>
                    <FarPoint:FpSpread ID="Fpspread2" Visible="false" runat="server" Height="1000px"
                        VerticalScrollBarPolicy="AsNeeded" CssClass="spreadborder">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
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
                            Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            CssClass="textbox btn1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </center>
                <center>
                    <div id="alertmessage" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_alerterror" Visible="false" runat="server" Text="" Style="color: Red;"
                                                    Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btn_errorclose" CssClass=" textbox btn2 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_errorclose_Click" Text="OK" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
</asp:Content>
