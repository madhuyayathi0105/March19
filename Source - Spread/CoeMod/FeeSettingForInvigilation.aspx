<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="FeeSettingForInvigilation.aspx.cs" Inherits="CoeMod_FeeSettingForInvigilation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <span class="fontstyleheader" style="color: Green;">Fees Setting For Exam Invigilation</span>
        </center>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="width: 1000px; font-family: Book Antiqua; font-weight: bold; height: auto">
                    <table class="maintablestyle" style="height: auto; margin-top: 10px; margin-bottom: 10px;
                        padding: 6px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="width: 160px;" Height="" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblYear" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Exam Year"></asp:Label>
                                <asp:DropDownList ID="ddlYear1" runat="server" CssClass="textbox ddlheight" OnSelectedIndexChanged="ddlYear1_SelectedIndexChanged"
                                    Width="107px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblmonth" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Month"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMonth1" runat="server" CssClass="textbox ddlheight" OnSelectedIndexChanged="ddlMonth1_SelectedIndexChanged"
                                    Style="width: 150px;" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="LblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Degree" runat="server" CssClass="textbox txtheight2" Style="height: 17px;
                                    width: 127px;" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="PnlDegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="155px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_Degree" runat="server" ForeColor="Black" Text="<b>Select All</b>"
                                        AutoPostBack="true" OnCheckedChanged="cb_Degree_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_Degree" runat="server" ForeColor="Black" AutoPostBack="true"
                                        OnSelectedIndexChanged="cbl_Degree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_Degree"
                                    PopupControlID="PnlDegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblDept" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Dept" runat="server" CssClass="textbox txtheight2" Style="height: 17px;
                                    width: 149px;" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="PnlDept" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="155px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_Dept" runat="server" ForeColor="Black" Text="<b>Select All</b>"
                                        AutoPostBack="true" OnCheckedChanged="cb_Dept_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_Dept" runat="server" ForeColor="Black" AutoPostBack="true"
                                        OnSelectedIndexChanged="cbl_Dept_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_Dept"
                                    PopupControlID="PnlDept" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblBatchYr" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_BatchYear" runat="server" CssClass="textbox txtheight2" Style="height: 17px;
                                    width: 100px; margin-left: 10px;" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="PnlBatch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="155px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_BatchYear" runat="server" ForeColor="Black" Text="<b>Select All</b>"
                                        AutoPostBack="true" OnCheckedChanged="cb_BatchYear_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_BatchYear" runat="server" ForeColor="Black" AutoPostBack="true"
                                        OnSelectedIndexChanged="cbl_BatchYear_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_BatchYear"
                                    PopupControlID="PnlBatch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblsubtype" runat="server" Text="Subject Type" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Subtype" runat="server" CssClass="textbox txtheight2" Style="height: 17px;
                                    width: 139px;" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="PnlSubtype" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="155px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_SubType" runat="server" ForeColor="Black" Text="<b>Select All</b>"
                                        AutoPostBack="true" OnCheckedChanged="cb_SubType_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_SubType" runat="server" ForeColor="Black" AutoPostBack="true"
                                        OnSelectedIndexChanged="cbl_SubType_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_Subtype"
                                    PopupControlID="PnlSubtype" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Subject" runat="server" CssClass="textbox txtheight2" Style="height: 17px;
                                    width: 127px;" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="PnlSubj" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="155px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_Subject" runat="server" ForeColor="Black" Text="<b>Select All</b>"
                                        AutoPostBack="true" OnCheckedChanged="cb_Subject_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_Subject" runat="server" ForeColor="Black" AutoPostBack="true"
                                        OnSelectedIndexChanged="cbl_Subject_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_Subject"
                                    PopupControlID="PnlSubj" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblSubSubject" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Sub-Subject"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_SubSubject" runat="server" CssClass="textbox txtheight2" Style="height: 17px;
                                    width: 149px;" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="PnlSubSubject" runat="server" BackColor="White" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="155px"
                                    Style="position: absolute;">
                                    <asp:CheckBox ID="cb_SubSubject" runat="server" ForeColor="Black" Text="<b>Select All</b>"
                                        AutoPostBack="true" OnCheckedChanged="cb_SubSubject_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_SubSubject" runat="server" ForeColor="Black" AutoPostBack="true"
                                        OnSelectedIndexChanged="cbl_SubSubject_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_SubSubject"
                                    PopupControlID="PnlSubSubject" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="LblCat" runat="server" Text="Category">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Category" runat="server" CssClass="textbox txtheight2" Style="height: 17px;
                                    width: 100px; margin-left: 10px;" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="Pnlcategory" runat="server" BackColor="White" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="155px"
                                    Style="position: absolute;">
                                    <asp:CheckBox ID="cb_Category" runat="server" ForeColor="Black" Text="<b>Select All</b>"
                                        AutoPostBack="true" OnCheckedChanged="cb_Category_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_Category" runat="server" ForeColor="Black" AutoPostBack="true"
                                        OnSelectedIndexChanged="cbl_Category_SelectedIndexChanged">
                                        <asp:ListItem Text="Internal Examiner" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="External Examiner" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Lab Assistant" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Assitant Examiner(Skilled)" Value="3"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_Category"
                                    PopupControlID="Pnlcategory" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpGoAdd" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" CssClass="textbox btn"
                                            Style="width: 80px;" />
                                        <asp:Button ID="btnAddRow" runat="server" Text="Add Row" OnClick="btnAddRow_OnClick"
                                            CssClass="textbox btn" Style="width: 80px;" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="DivFees" runat="server" visible="false" style="width: 600px; background-color: White;
                    border-radius: 10px;">
                    <asp:HiddenField ID="HdnSelectedRowIndex" runat="server" />
                    <asp:GridView ID="grdFeesSetting" Width="600px" runat="server" ShowFooter="false"
                        AutoGenerateColumns="false" Font-Names="Book Antiqua" toGenerateColumns="false"
                        ShowHeaderWhenEmpty="true" OnDataBound="grdFeesSetting_DataBound" OnRowDataBound="grdFeesSetting_OnRowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <center>
                                        <asp:DropDownList ID="ddlHeader" runat="server" CssClass="textbox ddlheight" Width="300px"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:Label ID="lbl_Header" runat="server" Visible="false" Text='<%#Eval("header") %>'>
                                        </asp:Label>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <center>
                                        <asp:TextBox ID="txt_Amount" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("Amount") %>'
                                            Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                    <asp:FilteredTextBoxExtender ID="filterextenderAmount" runat="server" TargetControlID="txt_Amount"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                    </asp:GridView>
                    <br />
                    <br />
                    <asp:UpdatePanel ID="UpSave" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_OnClick" CssClass="textbox btn"
                                Style="width: 80px;" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for Go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGoAdd">
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
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpSave">
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
</asp:Content>
