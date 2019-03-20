<%@ Page Title="" Language="C#" MasterPageFile="~/SMSMOD/SMSSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SmsGroupCreation.aspx.cs" Inherits="SmsGroupCreation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=btnAdd.ClientID %>').click(function () {
                $('#<%=divaddtype.ClientID %>').show();
                $('#<%=txtdesc.ClientID %>').val('');
                return false;
            });
            $('#<%=btnDel.ClientID %>').click(function () {
                var rptText = $('#<%=ddlreport.ClientID %>').find('option:selected').text();
                if (rptText.trim() != null && rptText != "Select") {
                    var msg = confirm("Are you sure you want to delete this report type?");
                    if (msg)
                        return true;
                    else
                        return false;
                }
                else {
                    alert("Please select any one report type!");
                    return false;
                }
            });

            $('#<%=btnexittype.ClientID %>').click(function () {
                $('#<%=divaddtype.ClientID %>').hide();
                return false;
            });

            $('#<%=btnaddtype.ClientID %>').click(function () {
                var txtval = $('#<%=txtdesc.ClientID %>').val();
                if (txtval == null || txtval == "") {
                    alert("Please enter the report type!");
                    return false;
                }
            });
        });

         
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <span class="fontstyleheader" style="color: Green;">Sms Group Creation</span>
    </center>
    <br />
    <center>
        <table class="maintablestyle">
            <tr>
                <td>
                    <asp:Label ID="lblcollege" runat="server" Text="College Name" Width="120px" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                        AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Height="25px" Width="150px">
                    </asp:DropDownList>
                </td>
                <td colspan="6">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblrptype" Text="Group Name" Font-Bold="True" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btnAdd" runat="server" Text="+" Style="height: 30px; width: 40px;" /><%--OnClick="btnAdd_OnClick"--%>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlreport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnDel" runat="server" Text="-" Style="height: 30px; width: 40px;"
                                        OnClick="btnDel_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbldept" runat="server" Text="Department" Style="" Width="90px" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                                Style="height: 20px; width: 180px; margin-right: 15px;">---Select---</asp:TextBox>
                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Height="400" Width="208">
                                <asp:CheckBox ID="CheckBox1" runat="server" Text="SelectAll" AutoPostBack="true"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                    OnCheckedChanged="CheckBox1_CheckedChanged" Checked="false" />
                                <asp:CheckBoxList ID="CheckBoxList1" runat="server" Font-Size="Small" AutoPostBack="True"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Width="350px" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged"
                                    Height="37px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="TextBox1"
                                PopupControlID="Panel1" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Staff Type" Style="font-family: Book Antiqua;
                        font-size: medium; font-weight: bold;"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtstafftype" runat="server" CssClass="Dropdown_Txt_Box" Height="20px"
                                ReadOnly="true" Width="180px" Style="height: 20px; width: 100px;" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="Panel3" runat="server" BackColor="White" CssClass="multxtpanel" Height="200px"
                                ScrollBars="Auto" Width="179px" Style="font-family: 'Book Antiqua'">
                                <asp:CheckBox ID="Chkboxstafftype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnCheckedChanged="Chkboxstafftype_CheckedChanged" Text="Select All"
                                    AutoPostBack="True" />
                                <asp:CheckBoxList ID="Chhliststafftype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Width="350px" OnSelectedIndexChanged="Chhliststafftype_SelectedIndexChanged"
                                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Height="58px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txtstafftype"
                                PopupControlID="Panel3" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lbldesignation" runat="server" Text="Designation" Style="display: inline-block;
                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                        width: 90px;"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtdesignation" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" Width="180px" Style="font-size: medium; font-weight: bold; height: 20px;
                                width: 180px; font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pdesignation" runat="server" BackColor="White" CssClass="multxtpanel"
                                Height="400px" ScrollBars="Auto" Width="350px" Style="font-family: 'Book Antiqua'">
                                <asp:CheckBox ID="chkdesignation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnCheckedChanged="chkdesignation_CheckedChanged" Text="Select All"
                                    AutoPostBack="True" />
                                <asp:CheckBoxList ID="chklstdesignation" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Width="350px" OnSelectedIndexChanged="chklstdesignation_SelectedIndexChanged"
                                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Height="58px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtdesignation"
                                PopupControlID="pdesignation" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Text="GO" Style="height: 30px; width: 40px;"
                        OnClick="btnstaffgo_Click" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" Visible="true" OnClick="btnSave_Click"
                        Style="height: 30px; width: 50px; background-color: Green;" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <%-- <div id="divGrid" runat="server" visible="false">
    
            <asp:CheckBox ID="chkSelectAll" runat="server" style="text-align:left ;" Text="select All" onchange="return SelLedgers();"/>
            <asp:GridView ID="gdReport" runat="server" AutoGenerateColumns="false">--%>
        <%-- ondatabound="gdattrpt_ondatabound" onrowdatabound="gdreport_onrowdatabound"--%>
        <%--<Columns>
                    <asp:TemplateField HeaderText="Sno" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                        HeaderStyle-Width="">
                        <ItemTemplate>
                            <center>
                                <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Sno") %>'></asp:Label>
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                        HeaderStyle-Width="">--%>
        <%-- <HeaderTemplate>
                       
                    </HeaderTemplate>--%>
        <%--   <ItemTemplate>
                            <center>
                                <asp:CheckBox ID="cbselect" runat="server" />
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                        HeaderStyle-Width="">
                        <ItemTemplate>
                            <asp:Label ID="staffCode" runat="server" Text='<%#Eval("staff_code") %>'></asp:Label>
                            <asp:Label ID="lblappno" runat="server" Text='<%#Eval("appno") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                        HeaderStyle-Width="">
                        <ItemTemplate>
                            <asp:Label ID="staffname" runat="server" Text='<%#Eval("staff_name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                        HeaderStyle-Width="">
                        <ItemTemplate>
                            <asp:Label ID="stafftype" runat="server" Text='<%#Eval("stftype") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff-MobileNo" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                        HeaderStyle-Width="">
                        <ItemTemplate>
                            <center>
                                <asp:Label ID="MobileNo" runat="server" Text='<%#Eval("per_mobileno") %>'></asp:Label>
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff-EmailId" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                        HeaderStyle-Width="">
                        <ItemTemplate>
                            <asp:Label ID="Email" runat="server" Text='<%#Eval("email") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="GroupName" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                        HeaderStyle-Width="">
                        <ItemTemplate>
                            <asp:Label ID="GroupName" runat="server" Text='<%#Eval("sms_groupCode") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>--%>
        <%--</center>--%>
        <%--report type name enter text box--%>
        <%--OnCellClick="Cell_Click"
            OnPreRender="Fpspread1_render" OnButtonCommand="FpSpread1_ButtonCommand"--%>
        <div id="print" runat="server" visible="false">
            <asp:Label ID="lblvalidation1" runat="server" Text="Please Enter Your Report Name"
                Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red" Style="display: none;"></asp:Label>
            <asp:Label ID="lblrptname" runat="server" Visible="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Visible="true" Width="180px" onkeypress="display()"
                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                InvalidChars="/\">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnExcel" runat="server" Visible="true" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                Height="32px" CssClass="textbox textbox1" />
            <asp:Button ID="btnprintmasterhed" runat="server" Visible="true" Font-Names="book antiqua"
                Font-Size="medium" Text="print" OnClick="btnprintmaster_click" Height="32px"
                Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
            <Insproplus:printmaster runat="server" ID="printcontrolhed" Visible="false" />
        </div>
        </br>
        <FarPoint:FpSpread ID="FpSpread1" runat="server" CssClass="spreadborder" ShowHeaderSelection="false"
            Visible="false" OnButtonCommand="FpSpread1_ButtonCommand">
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <div id="divaddtype" runat="server" style="height: 100%; z-index: 10000; background-color: rgba(54, 25, 25, .2);
            position: absolute; display: none; top: 5px; left: 5px; width: 100%;">
            <%----%>
            <center>
                <div id="panel_description11" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 567px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <%----%>
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbldesc" runat="server" Text="Description" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txtdesc" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnaddtype" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 30px; width: 50px;" OnClick="btnaddtype_Click" />
                                <asp:Button ID="btnexittype" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 30px; width: 50px;" /><%--OnClick="btnexittype_Click"--%>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
</asp:Content>
