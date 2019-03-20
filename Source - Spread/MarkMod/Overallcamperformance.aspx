<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" EnableEventValidation="false" CodeFile="Overallcamperformance.aspx.cs"
    Inherits="Overallcamperformance" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            function display() {

                document.getElementById('MainContent_lblerrexcel').innerHTML = "";
                document.getElementById('MainContent_errmsg').innerHTML = "";
            }
        </script>
        <style type="text/css" media="screen">
            .floats
            {
                height: 26px;
            }
            .CenterPB
            {
                position: absolute;
                left: 50%;
                top: 50%;
                margin-top: -20px;
                margin-left: -20px;
                width: auto;
                height: auto;
            }
        </style>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="20px"
            Style="width: 1169px">
            <center>
                <asp:Label ID="Label1" runat="server" Text="CR14 - Overall College Best Performance"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White"></asp:Label>
            </center>
        </asp:Panel>
        <asp:Panel ID="Panel1" runat="server" Height="69px" BackColor="LightBlue" BorderColor="Black"
            BorderStyle="Solid" ClientIDMode="Static" Width="806px" BorderWidth="1px" Style="">
            <asp:UpdatePanel ID="Upanel3" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 18px; width: 44px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="200px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                    AutoPostBack="True" Style="">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Style="height: 20px; width: 42px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtbatch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Height="200px">
                                    <asp:CheckBox ID="chkbatch" runat="server" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsbatch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                    PopupControlID="pbatch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Style="height: 21px; width: 56px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtdegree" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Height="200px">
                                    <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" OnCheckedChanged="chkdegree_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsdegree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdegree"
                                    PopupControlID="pdegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Style="height: 21px; width: 56px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtbranch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbranch" runat="server" CssClass="multxtpanel" Height="200px">
                                    <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" OnCheckedChanged="chkbranch_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsbranch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                    PopupControlID="pbranch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text=" Test" Style="width: 31px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged1"
                                    Height="21px" Style="width: 171px;" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbltop" runat="server" Text="Top" Style="height: 24px; width: 57px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txttop" runat="server" MaxLength="3" Style="width: 50px;" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="markfilter" runat="server" FilterType="Numbers"
                                    TargetControlID="txttop">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                <ContentTemplate>
                                    <td>
                                        <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Style="text-align: center;"
                                            Text="Go" Width="40px" Height="28px" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" />
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                    </table>
                    <asp:Label ID="errmsg" runat="server" Text="Label" Style="color: Red;" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <asp:Panel ID="Panel3" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
                        Style="width: 1169px;">
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <center>
                <table>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <tr>
                                <td>
                                    <center>
                                        <asp:GridView ID="gridview1" runat="server" ShowHeader="false" ShowFooter="false"
                                            AutoGenerateColumns="true" Font-Names="book antiqua" togeneratecolumns="true"
                                            AllowPaging="true" PageSize="50" OnPageIndexChanging="gridview1_onpageindexchanged">
                                           <%-- <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label runat="server" ID="lblsno" Text='<%#Eval("sno") %>' /></center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Roll No">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblroll" Text='<%#Eval("rollno") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reg No">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblreg" Text='<%#Eval("regno") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Student Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblnme" Text='<%#Eval("sname") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Student Type">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblstyp" Text='<%#Eval("stype") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Marks">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label runat="server" ID="lbltot" Text='<%#Eval("total") %>' /></center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Percentage">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label runat="server" ID="lblper" Text='<%#Eval("percentage") %>' /></center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cut Off Mark">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label runat="server" ID="lblcut" Text='<%#Eval("cutof") %>' /></center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Branch">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblbra" Text='<%#Eval("branch") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                          <%--  </Columns>--%>
                                        </asp:GridView>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblerrexcel" runat="server" Text="Label" ForeColor="Red" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
                                        Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnxl_Click" />
                                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                </td>
                            </tr>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnxl" />
                            <asp:PostBackTrigger ControlID="btnprintmaster" />
                        </Triggers>
                    </asp:UpdatePanel>
                </table>
            </center>
        </asp:Panel>
        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
          <%--progressBar for GO--%>
        <center>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel_go">
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
    </body>
</asp:Content>
