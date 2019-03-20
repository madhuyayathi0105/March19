<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AcademicYearSettings.aspx.cs" Inherits="AcademicYearSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#<%=btnDel.ClientID %>').click(function () {
                    var value = confirm("Do you want continue?");
                    if (!value)
                        return false;
                });
            });
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <fieldset style="height: auto; width: 750px;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 350px;
                                        height: 120px;">
                                        <asp:CheckBox ID="cbclg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cbclg_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cblclg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblclg_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtclg"
                                        PopupControlID="pnlclg" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                OnSelectedIndexChanged="rblType_Selected">
                                <asp:ListItem Text="Academic Year Settings" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Odd Settings"></asp:ListItem>
                                <asp:ListItem Text="Even Settings"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
                            <asp:Button ID="btnAddNew" runat="server" Text="Add New" OnClick="btnAddNew_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </center>
        <center>
            <table>
                <tr>
                    <td>
                        <asp:GridView ID="gdReport" runat="server" Visible="false" AutoGenerateColumns="false"
                            GridLines="Both" Width="730px" OnDataBound="gdattrpt_OnDataBound" OnRowDataBound="gdReport_OnRowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Sno") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="College" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblclg" runat="server" Text='<%#Eval("collegeStr") %>'></asp:Label>
                                        <asp:Label ID="lblclgVal" runat="server" Visible="false" Text='<%#Eval("collegeVal") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Academic Year" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblacd" runat="server" Text='<%#Eval("lblAcdemic") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Batch" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblbatch" runat="server" Text='<%#Eval("batchYear") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Feecategory" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblSem" runat="server" Text='<%#Eval("semester") %>'></asp:Label>
                                            <asp:Label ID="lblSemVal" runat="server" Visible="false" Text='<%#Eval("semesterVal") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Update" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" /><%--OnClick="btnUpdate_Click"--%>
                                            <asp:Label ID="lblbutton" runat="server" Visible="false" Text='<%#Eval("button") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <center>
                <div id="divEdit" runat="server" visible="false" class="popupstyle popupheight1 "
                    style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2);
                    position: absolute; top: 0; left: 0px;">
                    <asp:ImageButton ID="imgSetting" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 9px; margin-left: 308px;" OnClick="imgSetting_Click" />
                    <br />
                    <div style="background-color: White; height: auto; width: 650px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px; overflow: auto;">
                        <br />
                        <span style="color: Green; font-size: large; font-weight: bold;">Academic Year Settings</span>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnAddRow" runat="server" Visible="false" Text="Add New" OnClick="btnAddRow_Click" />
                                </td>
                                <td>
                                <fieldset>
                                    <asp:RadioButtonList ID="rblTypeNew" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Academic Year Settings" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Odd Settings"></asp:ListItem>
                                        <asp:ListItem Text="Even Settings"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:GridView ID="gdSetting" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                        Width="530px" OnDataBound="gdSetting_OnDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sno" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Sno") %>'></asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Academic Year" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:DropDownList ID="ddlAcademic" runat="server" CssClass="textbox1 ddlheight1">
                                                        </asp:DropDownList>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Batch" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:DropDownList ID="ddlBatch" runat="server" CssClass="textbox1 ddlheight1">
                                                        </asp:DropDownList>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Feecategory" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" Style="height: 80px;">
                                                            <asp:CheckBoxList ID="cblSem" runat="server">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <%-- <asp:DropDownList ID="ddlFeecat" runat="server" CssClass="textbox1 ddlheight1">
                                                </asp:DropDownList>--%>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <table id="tblSave" runat="server" visible="false">
                            <tr>
                                <td>
                                    <asp:Button ID="btnRowOK" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnDel" runat="server" Text="Delete" OnClick="btnDel_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </center>
        </center>
    </body>
</asp:Content>
