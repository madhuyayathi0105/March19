<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Transport_fees.aspx.cs" Inherits="Transport_fees" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblvalidation').innerHTML = "";
        }
    </script>
    <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 25px;
            width: 174px;
        }
        .style2
        {
            height: 25px;
            width: 100px;
        }
        .style3
        {
            height: 25px;
            width: 100px;
        }
        .style4
        {
            width: 800px;
        }
        .style5
        {
            width: 985px;
        }
        .style6
        {
            text-align: center;
            width: 4000px;
        }
        .style7
        {
            width: 82px;
        }
        .style8
        {
            height: 35px;
            width: 90px;
        }
        .style9
        {
            height: 29px;
            width: 40px;
            text-align: center;
        }
        .style10
        {
            height: 25px;
            width: 127px;
        }
    </style>
    <script type="text/javascript">

        function validation() {
            var error = "";

            var ddlbranch = document.getElementById("<%=ddlBranch.ClientID %>");
            var ddlfrmmonth = document.getElementById("<%=ddlfrommonth.ClientID %>");
            var ddltomonth = document.getElementById("<%=ddlToMonth.ClientID %>");
            var go = document.getElementById("<%=ButtonGo.ClientID %>");
            var rdid = document.getElementById("<%=rdbderee.ClientID %>");
            var routeid = document.getElementById("<%=rdbroute.ClientID %>");
            var veh_id = document.getElementById("<%=txt_vehicleid.ClientID %>");
            var r_id = document.getElementById("<%=txt_routeid.ClientID %>");
            if (rdid.checked == true) {
                if (ddlbranch.value == "--Select--") {
                    error += "Please Select Branch \n";
                }

            }
            if (routeid.checked == true) {
                if (veh_id.value == "---Select---") {
                    error += "Please Select Vehicle ID \n";
                }
                if (r_id.value == "---Select---") {
                    error += "Please Select Route ID \n";
                }
            }

            if (ddlfrmmonth.value == "--Select--") {

                error += "Please Select From Month \n";

            }
            if (ddltomonth.value == "--Select--") {
                error += "Please Select To Month \n";
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
    <div>
        <center>
            <br />
        </center>
    </div>
    <div style="width: 985px; height: 30px; left: 20px; background-color: #008080;">
        <table class="style5">
            <tr>
                <center>
                    <td class="style6">
                        <span style="font-size: large; color: White; font-weight: bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            Transport Fees Report</span>
                    </td>
                </center>
                <%--<td class="style7">
                    <asp:LinkButton ID="lnk_back" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx">Back</asp:LinkButton>
                </td>
                <td class="style7">
                    <asp:LinkButton ID="lnk_home" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx">Home</asp:LinkButton>
                </td>
                <td class="style7">
                    <asp:LinkButton ID="lnk_logout" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" ForeColor="White" OnClick="lnk_logout_Click">Logout</asp:LinkButton>
                </td>--%>
            </tr>
        </table>
    </div>
    <div style="width: 985px; height: 135px; background-color: #219DA5;">
        <center>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblclg" runat="server" Text="Header" Visible="false" Style="font-family: Book Antiqua;
                            font-size: large; color: White; font-weight: 700;"></asp:Label>
                            <span style="font-family: Book Antiqua; font-size: large; color: White; font-weight: 700;">
                            Header</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlHeader" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style1" AutoPostBack="true" OnSelectedIndexChanged="ddlHeader_selectchange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: large; color: White; font-weight: 700;">
                            Ledger</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLedger" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style1" AutoPostBack="true" OnSelectedIndexChanged="ddlLedger_selectchange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: large; color: White; font-weight: 700;">
                            Batch</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            AutoPostBack="true" CssClass="style3" OnSelectedIndexChanged="ddlBatch_selectchange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Style="font-family: Book Antiqua;
                            font-size: large; color: White; font-weight: 700;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style2" AutoPostBack="true" OnSelectedIndexChanged="ddlDegree_selectchange">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Style="font-family: Book Antiqua;
                            font-size: large; color: White; font-weight: 700;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style1" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_selectchange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        
                    </td>
                    <td colspan="5">
                        <asp:DropDownList ID="ddlfrommonth" runat="server" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style2" AutoPostBack="true" OnSelectedIndexChanged="ddlfrommonth_selectchange">
                        </asp:DropDownList>
                        
                        <asp:DropDownList ID="ddlfrmyear" runat="server" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style3" OnSelectedIndexChanged="ddlfrmyear_selectchange">
                        </asp:DropDownList>
                        
                        <asp:DropDownList ID="ddlToMonth" runat="server" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style2" AutoPostBack="true" OnSelectedIndexChanged="ddlToMonth_selectchange">
                        </asp:DropDownList>
                        
                        <asp:DropDownList ID="ddltoyear" runat="server" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style3" OnSelectedIndexChanged="ddltoyear_selectchange">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: large; color: White; font-weight: 700;">
                            Status</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlstatus" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style10" AutoPostBack="true" OnSelectedIndexChanged="ddlstatus_selectchange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: large; color: White; font-weight: 700;">
                            From Date</span>
                    </td>
                    <%-- <asp:DropDownList ID="ddlpaidfrmdate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style2" OnSelectedIndexChanged="ddlpaidfrmdate_selectchange">
                        </asp:DropDownList>--%>
                    <td>
                        <asp:TextBox ID="txtpaidfrmdate" runat="server" Font-Names="Book Antiqua" AutoPostBack="true"
                            Font-Size="Medium" Width="100px" OnTextChanged="txtpaidfrmdate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtpaidfrmdate" runat="server"
                            Format="d/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: large; color: White; font-weight: 700;">
                            To Date</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtpaidtodate" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px" OnTextChanged="txtpaidtodate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtpaidtodate" runat="server"
                            Format="d/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="ButtonGo" CssClass="btnapprove2" AutoPostBack="true" runat="server"
                            Font-Bold="true" Style="width: 40px; height: 27px" Text="Go" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Black" BackColor="DarkGray" OnClientClick="return validation()"
                            OnClick="ButtonGo_Click" />
                            </ContentTemplate>
                </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:RadioButton ID="rdbderee" runat="server" Text="Degree wise" Checked="true" AutoPostBack="true"
                            Style="font-family: Book Antiqua; font-size: large; color: White; font-weight: 700;"
                            GroupName="same" OnCheckedChanged="rdbdegree_Change" />
                        <asp:RadioButton ID="rdbroute" runat="server" Text="Route wise" AutoPostBack="true"
                            Style="font-family: Book Antiqua; font-size: large; color: White; font-weight: 700;"
                            GroupName="same" OnCheckedChanged="rdbroute_Change" />
                        <span id="vehicleidspan" runat="server" visible="false" style="font-family: Book Antiqua;
                            font-size: large; color: White; font-weight: 700;">Vehicle ID</span>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_vehicleid" runat="server" ReadOnly="true" Font-Bold="True" Visible="false"
                                    Font-Names="Book Antiqua" Font-Size="medium" CssClass="Dropdown_Txt_Box" Width="135px"
                                    Style="top: 220px; left: 380px; position: absolute;">---Select---</asp:TextBox>
                                <asp:Panel ID="panelvehicleid" runat="server" Width="280px" Visible="false" CssClass="multxtpanel multxtpanleheight">
                                    <asp:CheckBox ID="cb_vehicleid" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_vehicleid_CheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_vehicleid" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_vehicleid_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_vehicleid"
                                    PopupControlID="panelvehicleid" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <span id="routeidspan" runat="server" visible="false" style="font-family: Book Antiqua;
                            font-size: large; color: White; font-weight: 700;">Route ID</span>
                        <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_routeid" runat="server" ReadOnly="true" Width="135px" Font-Bold="True"
                                    Visible="false" Font-Names="Book Antiqua" Font-Size="medium" CssClass="Dropdown_Txt_Box"
                                    Style="top: 220px; left: 670px; position: absolute;">---Select---</asp:TextBox>
                                <asp:Panel ID="panelrouteid" runat="server" Width="280px" Visible="false" CssClass="multxtpanel multxtpanleheight">
                                    <asp:CheckBox ID="cb_routeid" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_routeid_CheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_routeid" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_routeid_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_routeid"
                                    PopupControlID="panelrouteid" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <br />
    <asp:Label ID="lblmsg" runat="server" Text="No Records Found" ForeColor="Red" Font-Bold="true"
        Visible="false" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
    <br />
    <center>
        <%-- <asp:GridView ID="gridview1" runat="server" CssClass="style5" Font-Names="Book Antiqua"
            Font-Size="medium" AutoGenerateColumns="false" OnPreRender="gridview1_OnPreRender"
            GridLines="Both">
            <Columns>
                <asp:TemplateField HeaderText="S.No" ItemStyle-VerticalAlign="Middle">
                    <ItemTemplate>
                        <asp:Label ID="lblsno" runat="server" Text='<%#Eval("SNo") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="20px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Roll No" ItemStyle-Width="100px" ItemStyle-VerticalAlign="Middle">
                    <ItemTemplate>
                        <asp:Label ID="lblRoll" runat="server" Text='<%#Eval("Roll No") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Register No" ItemStyle-Width="100px" ItemStyle-VerticalAlign="Middle">
                    <ItemTemplate>
                        <asp:Label ID="lblReg" runat="server" Text='<%# Eval("Register Number") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Student Name">
                    <ItemTemplate>
                        <asp:Label ID="lblstudname" runat="server" Text='<%#Eval("Student Name") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Month" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <asp:Label ID="lblmonth" runat="server" Text='<%#Eval("Month") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Demand" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <asp:Label ID="lblDemand" runat="server" Text='<%#Eval("Demand") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Paid" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <asp:Label ID="lblpaid" runat="server" Text='<%#Eval("Paid") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Balance" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <asp:Label ID="lblbal" runat="server" Text='<%#Eval("Balance") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <asp:Label ID="lblstatus" runat="server" Text='<%#Eval("status") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="80px" HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>--%>
        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderWidth="1px">
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
    </center>

        </ContentTemplate>
    </asp:UpdatePanel>
    <table>
        <tr>
            <td>
            
                <asp:Label ID="lblvalidation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="Red" Text="" Visible="false"></asp:Label>
                   
            </td>
        </tr>
        <tr>
            </ContentTemplate>
         </asp:UpdatePanel>
            <td>
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name" ></asp:Label>
                <asp:TextBox ID="txt_excel" runat="server" Width="120px"  Height="25px"
                    Font-Bold="true" Font-Size="Medium" onkeypress="display()" Font-Names="Book Antiqua"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_excel"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+\}{][':;?,.">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="g2btnexcel" runat="server" OnClick="btnexcel1_OnClick" 
                    Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </td>
            <td>
                <asp:Button ID="g2btnprint" runat="server" OnClick="g1btnprint1_OnClick" 
                    Text="Print" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
            </td>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
        </tr>
    </table>
            </ContentTemplate>
         </asp:UpdatePanel>
     <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
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
</asp:Content>
