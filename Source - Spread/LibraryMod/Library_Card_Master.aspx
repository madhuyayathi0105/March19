<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Library_Card_Master.aspx.cs" Inherits="LibraryMod_Library_Card_Master" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function frelig() {
            document.getElementById('<%=btnAddCardCatogery.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnDeleteCardCatogery.ClientID%>').style.display = 'block';
        }

        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=grdLibCardMas.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 0; i < (tbl.rows.length); i++) {
                var chkSelectid = document.getElementById('MainContent_grdLibCardMas_selectchk_' + i.toString());

                if (chkSelAll.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }

        }
    </script>
    <style type="text/css">
        .fontbold
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        
        .fontnormal
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        
        .fontblack
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: Black;
        }
        .fontcolorb
        {
            color: Green;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div>
                    <asp:Label ID="Label4" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                        position: relative;" Text="Library Card Master" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
                </div>
                <div style="height: 170px; width: 978px; margin: 0px; margin-top: 15px; font-family: Book Antiqua;
                    font-weight: bold; margin-bottom: 15px; position: relative; text-align: left;"
                    class="maintablestyle">
                    <table style="color: Black;">
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="lblclg" runat="server" Text="<b>College</b>"></asp:Label>
                                <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddl_collegename_OnSelectedIndexChanged" Style="width: 195px;
                                    height: 26px; margin-left: 48px" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:Label ID="lblLibrary" runat="server" Text="<b>Library</b>" Style="margin-left: 3px;"> </asp:Label>
                                <asp:DropDownList ID="ddlLibrary" runat="server" Style="width: 145px; height: 26px;
                                    margin-left: 14px;" CssClass="textbox ddlstyle ddlheight3">
                                    <asp:ListItem Text="--SELECT--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="lblBookType" runat="server" Text="<b>Book Type</b>" Style="margin-left: 10px;"> </asp:Label>
                                <asp:DropDownList ID="ddlBookType" runat="server" Width="135px" Height="26px" CssClass="textbox ddlstyle ddlheight3">
                                    <asp:ListItem Text="--SELECT--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <fieldset id="FieldsetStudentorstaff" runat="server" style="border: 2px solid #ffffff;
                                    height: 5px; margin-left: 8px; width: 148px;">
                                    <asp:RadioButton ID="rbStudent" runat="server" Text="Student" GroupName="Rbgrp" AutoPostBack="true"
                                        OnCheckedChanged="rbStudent_OnCheckedChanged" />
                                    <asp:RadioButton ID="rbStaff" runat="server" Text="Staff" GroupName="Rbgrp" AutoPostBack="true"
                                        OnCheckedChanged="rbStaff_OnCheckedChanged" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="lblCardCategory" runat="server" Text="<b>Card Catogery</b>"></asp:Label>
                                <asp:Button ID="btnAddCardCatogery" runat="server" Font-Size="Medium" OnClick="btnAddCardCatogery_OnClick"
                                    Style="font-size: Medium; position: absolute; margin-left: -3px;" Text="+" />
                                <asp:DropDownList ID="ddl_CardCatogery" runat="server" Font-Size="Medium" AutoPostBack="true"
                                    Style="position: absolute; margin-left: 31px; height: 26px" CssClass="textbox ddlstyle ddlheight3">
                                    <asp:ListItem Text="--SELECT--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Button ID="btnDeleteCardCatogery" runat="server" Font-Size="Medium" OnClick="btnDeleteCardCatogery_OnClick"
                                    Style="position: absolute; margin-left: 165px;" Text="-" />
                                <asp:CheckBox ID="cb_BookBank" runat="server" Style="margin-left: 195px;" OnCheckedChanged="cb_BookBank_OnCheckedChanged"
                                    AutoPostBack="true" Text="<b>Book Bank</b>" />
                                <asp:DropDownList ID="ddl_BookBank" runat="server" Enabled="false" Style="width: 110px;
                                    height: 26px; margin-left: 6px;" CssClass="textbox ddlstyle ddlheight3">
                                    <asp:ListItem Text="--SELECT--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="lblBatchYr" runat="server" Text="Batch" Style="margin-left: 10px;"></asp:Label>
                                <asp:TextBox ID="txt_BatchYear" runat="server" CssClass="textbox txtheight2" Style="height: 17px;
                                    width: 127px; margin-left: 40px;" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="120px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_BatchYear" runat="server" ForeColor="Black" Text="<b>Select All</b>"
                                        AutoPostBack="true" OnCheckedChanged="cb_BatchYear_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_BatchYear" runat="server" ForeColor="Black" AutoPostBack="true"
                                        OnSelectedIndexChanged="cbl_BatchYear_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_BatchYear"
                                    PopupControlID="p3" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpGo" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btn_MainGo" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-left: 8px;"
                                            OnClick="btn_MainGo_OnClick" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                                <asp:TextBox ID="txt_Department" runat="server" CssClass="textbox txtheight2" Style="height: 16px;
                                    width: 182px; margin-left: 15px;" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="300px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_Department" runat="server" ForeColor="Black" Text="Select All"
                                        AutoPostBack="true" OnCheckedChanged="cb_Department_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_Department" runat="server" ForeColor="Black" AutoPostBack="true"
                                        OnSelectedIndexChanged="cbl_Department_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_Department"
                                    PopupControlID="Panel1" Position="Bottom">
                                </asp:PopupControlExtender>
                                <asp:Label ID="lblStaffCategory" runat="server" Text="Staff Category" Style="margin-left: 2px;"></asp:Label>
                                <asp:TextBox ID="txt_StaffCatogery" runat="server" CssClass="textbox txtheight2"
                                    Width="100px" Height="16px" margin-left="-5px" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="130px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_StaffCatogery" runat="server" ForeColor="Black" Text="Select All"
                                        AutoPostBack="true" OnCheckedChanged="cb_StaffCatogery_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_StaffCatogery" runat="server" ForeColor="Black" AutoPostBack="true"
                                        OnSelectedIndexChanged="cbl_StaffCatogery_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_StaffCatogery"
                                    PopupControlID="Panel2" Position="Bottom">
                                </asp:PopupControlExtender>
                                <asp:Label ID="lblStaffType" runat="server" Text="Staff Type" Style="margin-left: 10px;"></asp:Label>
                                <asp:TextBox ID="txt_StaffType" runat="server" CssClass="textbox txtheight2" Style="height: 16px;
                                    width: 126px; margin-left: 6px;" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="130px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_StaffType" runat="server" ForeColor="Black" Text="<b>Select All</b>"
                                        AutoPostBack="true" OnCheckedChanged="cb_StaffType_checkedchange" /><%--OnCheckedChanged="cb_StaffType_checkedchange"--%>
                                    <asp:CheckBoxList ID="cbl_StaffType" ForeColor="Black" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="cbl_StaffType_SelectedIndexChanged">
                                        <%--OnSelectedIndexChanged="cbl_StaffType_SelectedIndexChanged"--%>
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_StaffType"
                                    PopupControlID="Panel3" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkbtn_FinePerWeek" runat="server" Text="<b>FinePerWeek</b>"
                                    Style="margin-left: 8px;" ForeColor="Blue" Width="90px" OnClick="lnkbtn_FinePerWeek_OnClick"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:Label ID="lbl_NoofCards" runat="server" Text="<b>No Of Cards</b>"></asp:Label>
                                <asp:TextBox ID="txt_NoofCards" runat="server" Style="width: 40px; margin-left: 12px;
                                    height: 16px;" CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:Label ID="lblNoofDays" runat="server" Text="<b>No Of Days</b>" Style="margin-left: 5px;"></asp:Label>
                                <asp:TextBox ID="txt_NoofDays" runat="server" Width="40px" Height="16px" CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:CheckBox ID="cb_FinePerWeek" runat="server" Text="<b>Fine Per Week</b>" Style="margin-left: 5px;"
                                    AutoPostBack="true" OnCheckedChanged="cb_FinePerWeek_CheckedChanged" />
                                <asp:TextBox ID="txt_FinePerWeek" runat="server" Width="40px" Height="16px" CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:Label ID="lbl_NoofDaysReferal" runat="server" Text="<b>No. Of Days(Ref)</b>"
                                    Style="margin-left: 5px;"></asp:Label>
                                <asp:TextBox ID="txt_NoofDaysReferal" runat="server" Width="40px" Height="16px" CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:Label ID="lbl_NoofRenewalCount" runat="server" Text="<b>No. Of Renewal Count</b>"
                                    Style="margin-left: 5px;"></asp:Label>
                                <asp:TextBox ID="txt_NoofRenewalCount" runat="server" Width="40px" Height="16px"
                                    CssClass="textbox txtheight2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7" runat="server" align="right">
                                <asp:LinkButton ID="lnkbtn_SetRenewal" runat="server" Text="<b>Set Renewal</b>" OnClick="lnkbtn_SetRenewal_OnClick"
                                    Width="105px" Style="margin-left: 10px;" ForeColor="Blue"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpGenerate" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btn_GenerateCard" runat="server" Style="margin-left: 8px;" ImageUrl="~/LibImages/generate card.jpg"
                                            OnClick="btn_GenerateCard_OnClick" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <div id="PNewCardCatogery" runat="server" visible="false" style="height: 100%; z-index: 10000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="DivCard" runat="server" visible="false" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 35px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <table>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblNewCardCatogery" runat="server" Text="Card Catogery" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt_CardCatogery" runat="server" Width="200px" Style="margin-left: 13px"
                                            Font-Bold="True" Font-Size="Medium" CssClass="textbox txtheight2"></asp:TextBox><%--TextMode="MultiLine"--%>
                                    </td>
                                </tr>
                                <tr>
                                    <asp:UpdatePanel ID="UpButtonAdd" runat="server">
                                        <ContentTemplate>
                                            <td align="center">
                                                <asp:Button ID="btn_NewCardCatogerySave" runat="server" Text="Add" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="textbox btn1 textbox1" OnClick="btn_NewCardCatogerySave_Click" />
                                                <asp:Button ID="btn_NewCardcatogeryExit" runat="server" Text="Exit" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="textbox btn1 textbox1" OnClick="btn_NewCardcatogeryExit_Click" />
                                            </td>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </tr>
                            </table>
                        </div>
                    </center>
                    <br />
                    <br />
                    <asp:Label ID="lblErrNewCardCatoger" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </div>
                <%-- </asp:Panel>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <div id="divPopAlertNEW" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsgNEW" runat="server" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpdatePanelbtn4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btnPopAlertCloseNEW" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                            OnClick="btnPopAlertClose_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--PopUp for Renewal--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <div id="PSetRenewal" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 343px;"
                        OnClick="imagebtnRenewpopclose_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 500px; width: 750px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; font-family: Book Antiqua; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Set Renewal</span></div>
                        </center>
                        <br />
                        <table class="maintablestyle" style="width: 615px;">
                            <tr>
                                <td>
                                    <%--<fieldset id="Fieldset2Renewal" runat="server">--%>
                                    <asp:Label ID="lblBatchYrNEW" runat="server" Text="Batch"></asp:Label>
                                    <asp:TextBox ID="txt_BatchYearNEW" runat="server" CssClass="textbox txtheight2" Width="70px"
                                        Height="15px" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="p3NEW" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="90px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_BatchYearNEW" runat="server" ForeColor="Black" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cb_BatchYearNEW_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_BatchYearNEW" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_BatchYearNEW_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender18NEW" runat="server" TargetControlID="txt_BatchYearNEW"
                                        PopupControlID="p3NEW" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:Label ID="lblDepartmentNEW" runat="server" Text="Department"></asp:Label>
                                    <asp:TextBox ID="txt_DepartmentNEW" runat="server" CssClass="textbox txtheight2"
                                        Width="70px" Height="15px" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1NEW" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="150px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_DepartmentNEW" runat="server" ForeColor="Black" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cb_DepartmentNEW_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_DepartmentNEW" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_DepartmentNEW_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1NEW" runat="server" TargetControlID="txt_DepartmentNEW"
                                        PopupControlID="Panel1NEW" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:Label ID="lblStaffCategoryNEW" runat="server" Text="Staff Category"></asp:Label>
                                    <asp:TextBox ID="txt_StaffCatogeryNEW" runat="server" CssClass="textbox txtheight2"
                                        Width="70px" Height="15px" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel2NEW" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="90px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_StaffCatogeryNEW" runat="server" ForeColor="Black" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cb_StaffCatogeryNEW_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_StaffCatogeryNEW" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_StaffCatogeryNEW_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2NEW" runat="server" TargetControlID="txt_StaffCatogeryNEW"
                                        PopupControlID="Panel2NEW" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpRenewGo" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btn_RenewalGoClick" runat="server" ImageUrl="~/LibImages/Go.jpg"
                                                OnClick="btn_RenewalGoClick_OnClick" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <div id="DivRenew" runat="server" visible="false" style="width: 500px; background-color: White;
                                border-radius: 10px;">
                                <asp:GridView ID="GrdRenew" Width="600px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                    Font-Names="Book Antiqua" toGenerateColumns="true" ShowHeaderWhenEmpty="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                </asp:GridView>
                            </div>
                        </center>
                        <div id="DivAddRenew" runat="server" visible="false" style="width: 600px; background-color: White;
                            border-radius: 10px;">
                            <asp:HiddenField ID="HiddenFieldRenew" runat="server" />
                            <asp:GridView ID="GrdAddRenew" Width="600px" runat="server" ShowFooter="false" AutoGenerateColumns="false"
                                Font-Names="Book Antiqua" toGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                OnRowDataBound="GrdAddRenew_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Renewal From" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_RenewFrom" runat="server" CssClass="textbox txtheight" Text='<%#Eval("RenewFrom") %>'
                                                    Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderRenewFrom" runat="server" TargetControlID="txt_RenewFrom"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Renewal To" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_RenewTo" runat="server" OnTextChanged="textboxValidationRenew"
                                                    CssClass="  textbox txtheight" Text='<%#Eval("RenewTo") %>' Height="15px" Width="100px"
                                                    Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderRenewTo" runat="server" TargetControlID="txt_RenewTo"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Renewal Days" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_RenewDays" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("RenewDays") %>'
                                                    Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderRenewDays" runat="server" TargetControlID="txt_RenewDays"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <asp:UpdatePanel ID="UpRenewButton" runat="server">
                            <ContentTemplate>
                                <asp:ImageButton ID="btnRenewalAddRow" Visible="false" runat="server" ImageUrl="~/LibImages/Add row.jpg"
                                    OnClick="btnRenewAddRow_OnClick" />
                                <asp:ImageButton ID="btnRenewalDelRow" Visible="false" runat="server" ImageUrl="~/LibImages/Delete row.jpg"
                                    OnClick="btnRenewDeleteRow_OnClick" />
                                <asp:ImageButton ID="btnRenewalSave" runat="server" ImageUrl="~/LibImages/save.jpg"
                                    OnClick="btnRenewalSave_OnClick" />
                                <asp:ImageButton ID="btnRenewalClose" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                    OnClick="btnRenewalClose_OnClick" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--PopUp for Fine--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
            <ContentTemplate>
                <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: fixed; margin-top: 30px; margin-left: 343px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 500px; width: 700px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; font-family: Book Antiqua; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Fine Per Week</span></div>
                        </center>
                        <br />
                        <table class="maintablestyle" style="width: 615px;">
                            <tr>
                                <td>
                                    <%--<fieldset id="Fieldset2ReFineal" runat="server">--%>
                                    <asp:Label ID="lblBatchYrFine" runat="server" Text="Batch"></asp:Label>
                                    <asp:TextBox ID="txt_BatchYearFine" runat="server" CssClass="textbox txtheight2"
                                        Width="70px" Height="15px" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="p3Fine" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="150px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_BatchYearFine" runat="server" ForeColor="Black" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cb_BatchYearFine_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_BatchYearFine" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_BatchYearFine_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender18Fine" runat="server" TargetControlID="txt_BatchYearFine"
                                        PopupControlID="p3Fine" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:Label ID="lblDepartmentFine" runat="server" Text="Department"></asp:Label>
                                    <asp:TextBox ID="txt_DepartmentFine" runat="server" CssClass="textbox  textbox1 txtheight3"
                                        Width="70px" Height="15px" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1Fine" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="300px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_DepartmentFine" runat="server" ForeColor="Black" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cb_DepartmentFine_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_DepartmentFine" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_DepartmentFine_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1Fine" runat="server" TargetControlID="txt_DepartmentFine"
                                        PopupControlID="Panel1Fine" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:Label ID="lblStaffCategoryFine" runat="server" Text="Staff Category"></asp:Label>
                                    <asp:TextBox ID="txt_StaffCatogeryFine" runat="server" CssClass="textbox  textbox1 txtheight3"
                                        Width="70px" Height="15px" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel2Fine" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="200px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_StaffCatogeryFine" runat="server" ForeColor="Black" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cb_StaffCatogeryFine_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_StaffCatogeryFine" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_StaffCatogeryFine_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2Fine" runat="server" TargetControlID="txt_StaffCatogeryFine"
                                        PopupControlID="Panel2Fine" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <%--                            </fieldset>--%>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpFineGo" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btn_FineGoClick" runat="server" ImageUrl="~/LibImages/Go.jpg"
                                                OnClick="btn_FineGoClick_OnClick" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="DivFine" runat="server" visible="false" style="width: 600px; background-color: White;
                            border-radius: 10px;">
                            <asp:GridView ID="grdFine" Width="600px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="Book Antiqua" toGenerateColumns="true" ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                        <div id="DivAddFine" runat="server" visible="false" style="width: 600px; background-color: White;
                            border-radius: 10px;">
                            <asp:HiddenField ID="HdnSelectedRowIndex" runat="server" />
                            <asp:GridView ID="grdAddFine" Width="600px" runat="server" ShowFooter="false" AutoGenerateColumns="false"
                                Font-Names="Book Antiqua" toGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                OnRowDataBound="GvUsersRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Day From" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_DayFrom" runat="server" CssClass="textbox txtheight" Text='<%#Eval("DayFrom") %>'
                                                    Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderDayFrom" runat="server" TargetControlID="txt_DayFrom"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Day To" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_DayTo" runat="server" OnTextChanged="textboxValidation" CssClass="  textbox txtheight"
                                                    Text='<%#Eval("DayTo") %>' Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderDayTo" runat="server" TargetControlID="txt_DayTo"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fine Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_FineAmount" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("FineAmount") %>'
                                                    Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderFineAmount" runat="server" TargetControlID="txt_FineAmount"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                        <asp:UpdatePanel ID="UpFineButton" runat="server">
                            <ContentTemplate>
                                <asp:ImageButton ID="btnAddRow" Visible="false" runat="server" ImageUrl="~/LibImages/Add row.jpg"
                                    OnClick="btnAddRow_OnClick" />
                                <asp:ImageButton ID="btnDeleteRow" Visible="false" runat="server" ImageUrl="~/LibImages/Delete row.jpg"
                                    OnClick="btnDeleteRow_OnClick" />
                                <asp:ImageButton ID="btnFineSave" runat="server" ImageUrl="~/LibImages/save.jpg"
                                    OnClick="btnFineSave_OnClick" />
                                <asp:ImageButton ID="btnFineClose" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                    OnClick="btnFineClose_OnClick" /></ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="grdAddFine" />
            </Triggers>
        </asp:UpdatePanel>
        <%-- </asp:Panel>--%>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
            <ContentTemplate>
                <span style="padding-right: 100px; margin-left: -260px; margin-top: 3px;">
                    <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                        onchange="return SelLedgers();" />
                </span>
                <div id="divspread" runat="server" visible="false" style="width: 950px; height: auto;
                    background-color: White; border-radius: 10px;">
                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="grdLibCardMas" Width="1000px" runat="server" ShowFooter="false"
                        ShowHeader="true" AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="true"
                        ShowHeaderWhenEmpty="true" OnRowDataBound="grdLibCardMas_RowDataBound" OnRowCreated="grdLibCardMas_OnRowCreated"
                        OnSelectedIndexChanged="grdLibCardMas_SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="allchk" runat="server" Text="Select All"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="selectchk" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="grdLibCardMas" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <br />
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
            <ContentTemplate>
                <div id="print" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        ForeColor="Red" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                        InvalidChars="/\">
                    </asp:FilteredTextBoxExtender>
                    <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                        OnClick="btnExcel_Click" />
                    <asp:ImageButton ID="btnprintmasterhed" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                        OnClick="btnprintmaster_Click" />
                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/LibImages/delete.jpg"
                        OnClick="btndelete_Click" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmasterhed" />
                <asp:PostBackTrigger ControlID="btnDelete" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <%--PopUp For CellClick--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel28" runat="server">
            <ContentTemplate>
                <div id="DivCellClick" runat="server" class="popupstyle" visible="false" style="height: 50em;
                    z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 10px; left: 0;">
                    <center>
                        <div id="DivFineCellClick" runat="server" style="background-color: White; height: 210px;
                            width: 290px; margin-top: 250px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                            border-radius: 10px;">
                            <table style="width: 280px;">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkCellFine" runat="server" AutoPostBack="true" Text="Fine Update"
                                            OnCheckedChanged="chkCellFine_OnCheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkCellNoOfDays" runat="server" AutoPostBack="true" Text="No Of Days"
                                            OnCheckedChanged="chkCellNoOfDays_OnCheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table style="width: 245px;">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="RbPerDay" runat="server" Enabled="false" Text="Per Day" AutoPostBack="true"
                                                        OnCheckedChanged="RbPerDay_OnCheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblFinePerDay" runat="server" Text="Fine" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                    <asp:TextBox ID="txt_CellClickFine" runat="server" Enabled="false" CssClass="  textbox txtheight"
                                                        Style="height: 15px; width: 60px; text-align: right;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="filterextenderFineCell" runat="server" TargetControlID="txt_CellClickFine"
                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:RadioButton ID="RbPerWeek" runat="server" Enabled="false" Text="Per Week" AutoPostBack="true"
                                                            OnCheckedChanged="RbPerWeek_OnCheckedChanged" />
                                                    </td>
                                                </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table style="width: 245px;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblNoDaysCell" runat="server" Text="No.Of Days" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                    <asp:TextBox ID="txt_NoDaysCell" runat="server" Enabled="false" CssClass="  textbox txtheight"
                                                        Style="height: 15px; width: 60px; text-align: right; margin-left: 48px;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNoDaysCell" runat="server"
                                                        TargetControlID="txt_NoDaysCell" FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblNoDayRef" runat="server" Text="No.Of Days(Ref)" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                    <asp:TextBox ID="txt_NoDayRef" runat="server" Enabled="false" CssClass="  textbox txtheight"
                                                        Style="height: 15px; width: 60px; text-align: right; margin-left: 15px;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNoDayRef" runat="server"
                                                        TargetControlID="txt_NoDayRef" FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <br />
                                <tr>
                                    <asp:UpdatePanel ID="UpcellClickFine" runat="server">
                                        <ContentTemplate>
                                            <td colspan="2">
                                                <center>
                                                    <asp:ImageButton ID="BtnCellClikOk" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                        OnClick="BtnCellClikOk_OnClick" />
                                                    <asp:ImageButton ID="BtnCellClikExit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                        OnClick="BtnCellClikExit_OnClick" />
                                                </center>
                                            </td>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--Card Generation Popup--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
            <ContentTemplate>
                <div id="Surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Label ID="LblCancel" runat="server" Text="Do You Want To Generate Card" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpSureYes" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnSure_yes" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                            width: 65px;" OnClick="btnSure_yes_Click" Text="yes" runat="server" />
                                                        <asp:Button ID="btnSure_no" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                            width: 65px;" OnClick="btnSure_no_Click" Text="no" runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID='btnSure_yes' />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel16" runat="server">
            <ContentTemplate>
                <div id="ProgreeDiv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="proDiv" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="UP_progress" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_progress" runat="server" Style="height: 20px; width: 100px;"
                                                        ReadOnly="true"></asp:TextBox>
                                                    <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 110px;
                                                        height: 200px;">
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="pce_progress" runat="server" TargetControlID="txt_progress"
                                                        PopupControlID="panel_progress" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--Pop For Delete --%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel17" runat="server">
            <ContentTemplate>
                <div id="SureDivDelete" runat="server" visible="false" style="height: 310%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div4" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblFineadd" runat="server" Text="Are you sure to delete selected row"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpSureDivDel" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btnAdd_yes" runat="server" ImageUrl="~/LibImages/yes.jpg" OnClick="btn_DeleteYes_Click" />
                                                        <asp:ImageButton ID="btnAdd_no" runat="server" ImageUrl="~/LibImages/no.jpg" OnClick="btn_DeleteNo_Click" />
                                                    </ContentTemplate>
                                                     <Triggers>
                                                        <asp:PostBackTrigger ControlID='btnAdd_yes' />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
            <ContentTemplate>
                <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 360px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpdatePanelbtn9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btn_errorclose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                            OnClick="btn_errorclose_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--Progress bar for go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
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
    <%--Progress bar for UpGenerate--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpGenerate">
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
    <%--Progress bar for UpRenewGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpRenewGo">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for UpRenewButton--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpRenewButton">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for UpFineGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpFineGo">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress5"
            PopupControlID="UpdateProgress5">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for UpFineButton--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="UpFineButton">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender6" runat="server" TargetControlID="UpdateProgress6"
            PopupControlID="UpdateProgress6">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for UpSureYes--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="UpSureYes">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender7" runat="server" TargetControlID="UpdateProgress7"
            PopupControlID="UpdateProgress7">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for UpSureDivDel--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="UpSureDivDel">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender8" runat="server" TargetControlID="UpdateProgress8"
            PopupControlID="UpdateProgress8">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for UpButtonAdd--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="UpButtonAdd">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender9" runat="server" TargetControlID="UpdateProgress9"
            PopupControlID="UpdateProgress9">
        </asp:ModalPopupExtender>
    </center>
    <%--Progress bar for UpcellClickFine--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="UpcellClickFine">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender10" runat="server" TargetControlID="UpdateProgress10"
            PopupControlID="UpdateProgress10">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
