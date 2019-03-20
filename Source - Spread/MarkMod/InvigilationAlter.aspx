<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="InvigilationAlter.aspx.cs" Inherits="MarkMod_InvigilationAlter" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
    <style type="text/css">
        .GridDock
        {
            overflow-x: hidden;
            overflow-y: auto;
            height:500px;
            padding: 0 0 0 0;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Invigilation Staff Alter</span></div>
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
                                                    <asp:Label ID="lbl_date" Text="Exam Date" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_date" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_date" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_date" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_date" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_date_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_date" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_date_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pce_date" runat="server" TargetControlID="txt_date"
                                                                PopupControlID="panel_date" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_hall" Text="Hall No" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_hall" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_hall" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_hall" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_hall" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_hall_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_hall" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hall_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pce_hall" runat="server" TargetControlID="txt_hall"
                                                                PopupControlID="panel_hall" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
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
          <asp:Label ID="lblXpos" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lblYpos" runat="server" Visible="false"></asp:Label>
        <br />
        <br />
        <center>
    
            <div id="showreport1" runat="server" visible="false">
                <table>
                   
                    <tr>
                    <td>
                     <asp:GridView ID="GridView1" runat="server" style="margin-bottom:15px;margin-top:15px; width:auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" OnRowDataBound="gridview1_OnRowDataBound" Width="500px">
        <Columns>
        <asp:TemplateField HeaderText="S.No">
        <ItemTemplate>
        <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
        </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Degree Details">
        <ItemTemplate>
        <asp:Label ID="lbldegdetail" runat="server" Text='<%# Eval("Degree_details") %>'></asp:Label>
        </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"  />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date" >
        <ItemTemplate>
        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("Date") %>'></asp:Label>     
        </ItemTemplate>
           <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Hall No">
        <ItemTemplate>
        <asp:Label ID="lblhallno" runat="server" Text= '<%# Eval("HallNo") %>'></asp:Label>
        <asp:Label ID="lblexmdate" runat="server" Text= '<%# Eval("examdate") %>' Visible="false"></asp:Label>
        <asp:Label ID="lblcriteriano" runat="server" Text= '<%# Eval("criteriano") %>' Visible="false"></asp:Label>
        </ItemTemplate>
          <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"  />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Session">
        <ItemTemplate>
        <asp:Label ID="lblAn" runat="server" Text='<%# Eval("Session") %>'  ></asp:Label>
        </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Subject Name">
        <ItemTemplate>
        <asp:Label ID="lblsubname" runat="server" Text='<%# Eval("SubjectName") %>'></asp:Label>
         <asp:Label ID="lblsubno" runat="server" Text='<%# Eval("subjectno") %>' Visible="false"></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Staff Name">
        <ItemTemplate>
        <asp:Label ID="lblstaffname" runat="server" Text='<%# Eval("StaffName") %>'></asp:Label>
        <asp:Label ID="lblstaffcode" runat="server" Text='<%# Eval("Staffcode") %>' Visible="false"></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Select Staff">
        <ItemTemplate>
        <asp:Button ID="btnstaff" runat="server" Text="Select Staff" OnClick="btnstaff_OnClick" />
        </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Alter Staff Name" Visible="false">
        <ItemTemplate>
        <asp:Label ID="lblalterstaff" runat="server" Text='<%# Eval("AlterStaffName") %>'></asp:Label>
        <asp:Label ID="lblalterstafcode" runat="server" Text='<%# Eval("AlterStaffcode") %>' Visible="false"></asp:Label>
        <asp:Label ID="lblalterstafappid" runat="server" Text='<%# Eval("AlterStaffapplid") %>' Visible="false"></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        </Columns>
         <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
        </asp:GridView>
                    </td>
                    </tr>
                </table>
                <center>
                    <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save_Click"
                        Visible="false" Style="text-align: center" />
                </center>
            </div>
        </center>
        <div id="divAlterFreeStaffDetails" runat="server" visible="false" style="height: 160em;
            z-index: 2000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
            top: 0; left: 0px;">
            <center>
                <div id="divAlterFreeStaff" runat="server" class="table" style="background-color: White;
                    height: auto; width: 50%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    top: 5%; left: 25%; right: 25%; position: fixed; border-radius: 10px;">
                    <center>
                        <span style="font-family: Book Antiqua; font-size: 20px; font-weight: bold; color: Green;
                            margin: 0px; margin-bottom: 20px; margin-top: 15px; position: relative;">Available Staff
                            List</span>
                    </center>
                    <div>
                        <asp:Label ID="lblAlterDate" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblAlterHour" runat="server" Text="" Visible="false"></asp:Label>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblAlterFreeCollege" Text="College" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAlterFreeCollege" runat="server" OnSelectedIndexChanged="ddlAlterFreeCollege_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblAlterFreeDepartment" Text="Department" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlAlterFreeDepartment" Width="100px" runat="server" IndexChanged="ddlAlterFreeDepartment_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSearchBy" runat="server" Text="Staff By"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAlterFreeStaff" runat="server" Width="200px" OnSelectedIndexChanged="ddlAlterFreeStaff_SelectedIndexChanged"
                                        >
                                        <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                        <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAlterFreeStaffSearch" runat="server" OnTextChanged="txtAlterFreeStaffSearch_TextChanged"
                                        Width="200px" ></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btn_Search" Width="50px" runat="server" Text="Search" CssClass="textbox btn1"
                                        OnClick="btnSearch_clickNEw" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <center>
                        <div id="divspreadpopup" runat="server" visible="false" class="GridDock" style="width:500px; margin-top:15px;">
                            <table>
                                <tr>
                                    <td>
                                         <asp:GridView ID="GridView2" runat="server" style="margin-bottom:15px;margin-top:15px; width:auto;" Font-Names="Times New Roman" AutoGenerateColumns="false"  Width="500px"  >
        <Columns>
        <asp:TemplateField HeaderText="S.No">
        <ItemTemplate>
        <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
        </ItemTemplate>
         <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Staff Name">
        <ItemTemplate>
        <asp:Label ID="lblsatfname" runat="server" Text='<%# Eval("StaffName") %>'></asp:Label>
        <asp:Label ID="lblstafcode" runat="server" Text='<%# Eval("staffcode") %>' Visible="false"></asp:Label>
        <asp:Label ID="lblapplid" runat="server" Text='<%# Eval("applid") %>' Visible="false"></asp:Label>
        </ItemTemplate>
         <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="130px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Exp" >
        <ItemTemplate>
        <asp:Label ID="lblexp" runat="server" Text='<%# Eval("exp") %>'></asp:Label>     
        </ItemTemplate>
           <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Select">
        <ItemTemplate>
       <asp:CheckBox ID="cbcheck" runat="server" />
        
        </ItemTemplate>
          <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
        </asp:TemplateField>
        </Columns>
         <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                          
                        </div>
                         
                    </center>
                     <asp:Button ID="btnSelectStaff" runat="server" Text="Ok" OnClick="btnSelectStaff_Click" />
                            <asp:Button ID="btnFreeStaffExit" runat="server" Text="Exit" OnClick="btnFreeStaffExit_Click" />
                </div>
            </center>
        </div>
    </div>
    <center>
        <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                    <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
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
