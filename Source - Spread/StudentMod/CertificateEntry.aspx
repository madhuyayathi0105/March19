<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CertificateEntry.aspx.cs" Inherits="StudentMod_CertificateEntry" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Student Updated Details</span></div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <div>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_str1" runat="server" Text="Stream"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_stream" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_stream" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_stream" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_stream" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="cb_stream_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_stream" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_stream_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pce_stream" runat="server" TargetControlID="txt_stream"
                                                                PopupControlID="panel_stream" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    Batch
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_batch" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                                                PopupControlID="panel_batch" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_degree" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                                                PopupControlID="panel_degree" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="Up_dept" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                                                PopupControlID="panel_dept" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_go" Text="Go" CssClass=" textbox btn1" runat="server" OnClick="btn_go_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnsave" runat="server" CssClass="textbox btn2" Visible="false" Text="Save"
                                                        OnClick="btn_Save_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </center>
        <center>
            <div id="divGrid" runat="server" style="width: auto; height: auto; overflow: auto;
                ackground-color: White; border-radius: 0px;">
                <span style="padding-right: 100px; margin-left: -460px;">
                    <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                        onchange="return SelLedgers();" />
                </span>
                <asp:GridView ID="grid_Details" runat="server" AutoGenerateColumns="false" GridLines="Both"
                    Style="width: auto" OnRowDataBound="gridReport_OnRowDataBound">
                    <%--OnDataBound="grid_Details_DataBound" OnRowDataBound="grid_Details_OnRowDataBound"--%>
                  
                    <Columns>
                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_rs" runat="server" Width="60px" Text='<%#Eval("Sno") %>'></asp:Label>
                                        <asp:Label ID="lbl_sno" runat="server" Visible="false" Text='<%#Eval("appno") %>'>
                                    </asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Roll No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_rn" runat="server" Style="width:auto;" Text='<%#Eval("Roll No") %>'></asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reg No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_res" runat="server" Style="width:auto;" Text='<%#Eval("Reg No") %>'></asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Admission No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_an" runat="server" Style="width:auto;" Text='<%#Eval("Admission No") %>'></asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Student Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_name" runat="server" Style="width:auto;" Text='<%#Eval("Student Name") %>'></asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="YearSession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_YearSession" runat="server" Style="width:auto;" Text='<%#Eval("YearSession") %>'></asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                           <asp:TemplateField HeaderText="TotalMark" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_TotalMark" runat="server" Style="width:auto;" Text='<%#Eval("TotalMark") %>'></asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="12th Certificate No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:TextBox ID="txt_CertificateNo" runat="server" CssClass="textbox ddlheight3"
                                        Width="110px" Visible="true" Text='<%#Eval("12th Certificate No") %>'></asp:TextBox>
                                    <asp:Label ID="lbl_CertificateNo" runat="server" Visible="false" Style="width:auto;" Text='<%#Eval("12th Certificate No") %>'>
                                    </asp:Label>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </center>
    </div>
</asp:Content>
