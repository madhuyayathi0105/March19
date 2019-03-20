<%@ Page Title="" Language="C#" MasterPageFile="~/HostelMod/hostelsite.master" AutoEventWireup="true"
    CodeFile="Hostel_Attendance_Manual_Report.aspx.cs" Inherits="Hostel_Attendance_Manual_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <center>
                <div>
                    <asp:Label ID="header" runat="server" Text="Hostel Attendance Report" CssClass="fontstyleheader"
                        ForeColor="Green"></asp:Label>
                    <br />
                </div>
    </center>
    
    <br />
     <div class="maindivstyle maindivstylesize">
        <br />
        <center>
            <table class="maintablestyle"  style="margin: 0px; margin-bottom: 0px; margin-top: 10px; position: relative;width: 873px;">
                <tr>
                    <td colspan="10">
                        <fieldset style="width: 700px; height: 35px;">
                            <asp:RadioButton ID="rdbhostel" runat="server" Text="Hostel/Study Hours Attendance" OnCheckedChanged="rdbhostel_CheckedChange"
                                GroupName="Attendance" AutoPostBack="true"  />
                            <asp:RadioButton ID="rdbmess" runat="server" Text="Mess Attendance" GroupName="Attendance"
                                OnCheckedChanged="rdbmess_CheckedChange" AutoPostBack="true" />
                            
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblhostel" runat="server" Text="Hostel" CssClass="commonHeaderFont"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Hostel" runat="server" CssClass="textbox1  ddlheight1"  OnSelectedIndexChanged="ddl_Hostel_SelectedIndexChanged"  AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblAttendance" runat="server" Text="From Date" CssClass="commonHeaderFont"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_attandance" runat="server" CssClass="textbox  txtheight" AutoPostBack="true"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_attandance" runat="server"
                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                        </asp:CalendarExtender>
                    </td>
                     <td>
                        <asp:Label ID="lblAttendance_to" runat="server" Text="To Date" CssClass="commonHeaderFont"></asp:Label>
                    </td>
                     <td>
                        <asp:TextBox ID="txt_attandance_to" runat="server" CssClass="textbox  txtheight" AutoPostBack="true"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_attandance_to" runat="server"
                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                    <td>
                        <asp:Label ID="Lblsession" runat="server" Text="Session" CssClass="commonHeaderFont"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsession" runat="server" CssClass="textbox1  ddlheight1">
                            <asp:ListItem Value="0">Morning</asp:ListItem>
                            <asp:ListItem Value="1">Evening</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                          <td>
                        <asp:Label ID="Label2" Text="Building Name" runat="server" Width="52px" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="drbbuilding"  runat="server" CssClass="textbox textbox1 ddlheight1" Visible="false"
                                            AutoPostBack="true" OnSelectedIndexChanged="drbbuilding_SelectedIndexChanged">
                                        </asp:DropDownList>                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                      <td>
                        <asp:Label ID="lbl_floorname" Text="Floor" runat="server" Width="52px" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upp1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_floorname" runat="server" Visible="false" CssClass="textbox textbox1"
                                    Width="82px" ReadOnly="true" Height="20px" >--Select--</asp:TextBox>
                                <asp:Panel ID="pflrnm" runat="server" Visible="false" CssClass="multxtpanel" Width="155px"
                                    Height="250px">
                                    <asp:CheckBox ID="cb_floorname" runat="server" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_floorname_CheckedChange" />
                                    <asp:CheckBoxList ID="cbl_floorname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_floorname_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupExt4" runat="server" TargetControlID="txt_floorname"
                                    PopupControlID="pflrnm" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                      <td >
                        <asp:Label ID="Lblroom" runat="server" Text="Room" CssClass="commonHeaderFont" Visible="false"></asp:Label>
                        
                                </td>
                                <td>
                                 <asp:UpdatePanel ID="updatepanel_room" runat="server" Visible="false">
                            <ContentTemplate>
                                    <asp:TextBox ID="txt_room" runat="server" CssClass="textbox  txtheight2" ReadOnly="true" >--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_room" runat="server" Width="128px" CssClass="multxtpanel multxtpanleheight">
                                        <asp:CheckBox ID="cb_room" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_room_CheckedChanged"  />
                                        <asp:CheckBoxList ID="cbl_room" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_room_SelectedIndexChanged" >
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_room" 
                                        PopupControlID="panel_room" Position="Bottom">
                                    </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    </tr>
                    <tr>
                       <td>
                        <asp:Label ID="Label1" runat="server" Text="Status" Visible="false" CssClass="commonHeaderFont"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlstatus" runat="server" CssClass="textbox1  ddlheight1"  Visible="false"  AutoPostBack="true">
                        
                        </asp:DropDownList>
                    </td>

                    <td>
                        <asp:Label ID="Lblstatus" runat="server" Text="Roll No" CssClass="commonHeaderFont"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_status" runat="server" CssClass="textbox1  ddlheight1" OnSelectedIndexChanged="ddl_status_SelectedIndexChanged"   AutoPostBack="true">
                         <asp:ListItem>Roll No</asp:ListItem>
                                       
                                      
                                        <asp:ListItem>Hostel Id</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                   

                    <td>
                   <center>
            <asp:Button ID="btn_go" runat="server" CssClass="fontbold" Width="56px" Height="27px" Text="Go"
                OnClick="Go_Click" />
        </center></td>
                </tr>
            </table>
        </center>
        <br />
        <br />
        
         <center>
                  <div>
                                <FarPoint:FpSpread ID="Fpspread6" runat="server" Visible="true" AutoPostBack="true"
                                    BorderWidth="0px" Style="overflow: auto; height: 400px; border: 0px solid #999999;
                                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                </center>

                  <center>
                    <div id="rptprint1" runat="server" visible="false">
                        <br />
                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <%--  --%>
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" onkeypress="display1()" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                            Height="31px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="31px"
                            CssClass="textbox textbox1" />
                        
                        <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                    </div>
                    <br />
                </center>
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
                                            OnClick="btnerrclose_Click" Text="ok" runat="server" />
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