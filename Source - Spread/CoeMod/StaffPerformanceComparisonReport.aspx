<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="StaffPerformanceComparisonReport.aspx.cs" Inherits="CoeMod_StaffPerformanceComparisonReport" %>



   <%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .fontStyle
        {
            font-size: medium;
            font-weight: bolder;
            font-style: oblique;
            padding: 5px;
        }
        .fontStyle1
        {
            font-size: medium;
            font-style: oblique;
            padding: 3px;
            color: Blue;
        }
        .commonHeaderFont
        {
            font-size: medium;
            color: Black;
            font-family: 'Book Antiqua';
            font-weight: bold;
        }
        .defaultWidthHeight
        {
            width: auto;
            height: auto;
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>
    <center>
        <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Staff Performance Comparison Report </span>
        <div id="divSearch" runat="server" visible="true" class="maindivstyle" style="width: 100%;
            height: auto; margin: 0px; margin-bottom: 20px; margin-top: 10px; padding: 5px;
            position: relative;">
             <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                margin-bottom: 10px; padding: 6px;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont">
                        </asp:Label>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown commonHeaderFont"
                            Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                        </ContentTemplate>
                      </asp:UpdatePanel>
                    </td>
                  
                    <td>
                            <asp:Label ID="Label3" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        </td>
                             <td>
                           <asp:UpdatePanel ID="UP_batch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox txtheight2" ReadOnly="true"  Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_batch_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"/>
                                        <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                        PopupControlID="panel_batch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" CssClass="commonHeaderFont" Text="Degree">
                        </asp:Label>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                        <asp:DropDownList ID="ddlDegree" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged"
                            AutoPostBack="True" Width="80px">
                        </asp:DropDownList>
                        </ContentTemplate>
                      </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" CssClass="commonHeaderFont" Text="Branch">
                        </asp:Label>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                            AutoPostBack="True" Width="150px">
                        </asp:DropDownList>
                        </ContentTemplate>
                      </asp:UpdatePanel>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblexamyear" runat="server" CssClass="commonHeaderFont" Text="ExamYear">
                        </asp:Label>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                        <asp:DropDownList ID="ddlExamYear" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged"
                            AutoPostBack="True" Width="80px">
                        </asp:DropDownList>
                        </ContentTemplate>
                      </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblexammonth" runat="server" CssClass="commonHeaderFont" Text="ExamMonth">
                        </asp:Label>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                        <asp:DropDownList ID="ddlExamMonth" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlExamMonth_SelectedIndexChanged"
                            AutoPostBack="True" Width="80px">
                        </asp:DropDownList>
                        </ContentTemplate>
                      </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="font" OnClick="btnGo_Click"
                            Style="width: auto; height: auto;" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px; margin-bottom: 15px;
                margin-top: 10px;"></asp:Label>

                </div>
    </center>
    <center>
    <div id="divmain" runat="server">
    <center>
    <table>
    <tr align="center">
    <td align="center">
    <asp:GridView ID="gridview1" runat="server" style="margin-bottom:15px;margin-top:15px; width:1100px;" Font-Names="Times New Roman" AutoGenerateColumns="false" ShowHeader="false" OnRowDataBound="gridview1_OnRowDataBound" BackColor="AliceBlue">
    <Columns>
     <asp:TemplateField HeaderText="Sno">
                        <ItemTemplate>
                            <asp:Label ID="lblsno" runat="server"  Text='<%# Eval("sno") %>'></asp:Label>
                          
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="55px" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Subject Name">
                        <ItemTemplate>
                            <asp:Label ID="lblsubjectname" runat="server"  Text='<%# Eval("SubjectName") %>'></asp:Label>
                          
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="55px" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="staff Name1">
                        <ItemTemplate>
                            <asp:Label ID="lblstaffname1" runat="server"  Text='<%# Eval("staffName1") %>'></asp:Label>
                          
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="55px" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Pass1%">
                        <ItemTemplate>
                            <asp:Label ID="lblpercent1" runat="server"  Text='<%# Eval("Pass1%") %>'></asp:Label>
                          
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="55px" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="staff Name2">
                        <ItemTemplate>
                            <asp:Label ID="lblstaffname2" runat="server"  Text='<%# Eval("staffName2") %>'></asp:Label>
                          
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="55px" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Pass2%">
                        <ItemTemplate>
                            <asp:Label ID="lblpercent2" runat="server"  Text='<%# Eval("Pass2%") %>'></asp:Label>
                          
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="55px" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="staff Name3">
                        <ItemTemplate>
                            <asp:Label ID="lblstaffname3" runat="server"  Text='<%# Eval("staffName3") %>'></asp:Label>
                          
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="55px" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Pass3%">
                        <ItemTemplate>
                            <asp:Label ID="lblpercent3" runat="server"  Text='<%# Eval("Pass3%") %>'></asp:Label>
                          
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="55px" />
                    </asp:TemplateField>

    </Columns>
    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
    </asp:GridView>
    </td></tr>
     <tr  align="center"><td  align="center">
     <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                    Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ .">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnxl_Click" />
                <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnmasterprint_Click"  />
                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
    </td></tr>
    <tr><td>
     <br />
                <asp:Label ID="lbl_err" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium"
                    Font-Names="Book Antiqua"></asp:Label>
    </td></tr>
    
    </table>
    </center>
    </div>
    </center>

    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
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
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
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

