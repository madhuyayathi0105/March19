<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master" AutoEventWireup="true" CodeFile="AdmissionPrint.aspx.cs" Inherits="AdmissionPrint" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
 <script type="text/javascript">
     function display() {
         document.getElementById("<%=lblsmserror.ClientID %>").innerHTML = "";
     }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <center>
                <br />
                <div>
                    <span class="fontstyleheader" style="color: green;">Admission Print Format</span></div>
             
            </center>

            <table id="Table1" class="maintablestyle" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="lbl_collegename" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_collegename" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged"
                            Height="29px" Width="202px" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                           <asp:Label ID="lblStr" Text="Stream" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddltype" runat="server" Width="132px" Height="30px" Enabled="false"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="type_Change"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">Batch</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Width="70px" Height="30px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="batch_SelectedIndexChange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">Education
                            Level</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddledulevel" runat="server" Width="70px" Height="30px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="edulevel_SelectedIndexChange">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbldeg" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_degree" runat="server" ReadOnly="true" Width="190px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 txtheight">---Select---</asp:TextBox>
                                <asp:Panel ID="paneldegree" runat="server" Height="300px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cbdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbdegree_Changed" />
                                    <asp:CheckBoxList ID="cbldegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnSelectedIndexChanged="cbldegree_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_degree"
                                    PopupControlID="paneldegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cbldegree" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                       <asp:Label ID="lblBran" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_department" runat="server" ReadOnly="true" Width="122px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 txtheight">---Select---</asp:TextBox>
                                <asp:Panel ID="paneldepartment" runat="server" Height="300px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cbdepartment1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbdepartment_Changed" />
                                    <asp:CheckBoxList ID="cbldepartment" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="cbldepartment_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_department"
                                    PopupControlID="paneldepartment" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">From
                            Date</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfrmdate" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                        <asp:CalendarExtender ID="calfrmdate" runat="server" TargetControlID="txtfrmdate"
                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold;">To Date</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txttodate" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                        <asp:CalendarExtender ID="caltodate" runat="server" TargetControlID="txttodate" CssClass="cal_Theme1 ajax__calendar_active"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Text="GO" Font-Bold="true" Font-Names="Book Antiqua"
                            CssClass="textbox textbox1 btn1" OnClick="btngo_click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="mainpgeerr" runat="server" Text="" Visible="false" Font-Bold="true"
                Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Red"></asp:Label>
            <br />
            <br />
            <div id="sp_div" runat="server">
                <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="1px" Width="778px" Height="600px" Style="margin-left: 2px;"
                    class="spreadborder" OnButtonCommand="Fpspread_command" ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </div>
            <br />
            <br />
            <asp:Button ID="btncoverprint" runat="server" Text="Cover Print" Visible="false"
                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 btn3" Height="30px"
                OnClick="btncoverprint_click" />
            <asp:Button ID="btninsurprnt" runat="server" Text="Insurance Print" Visible="false"
                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 btn3"  Height="30px" 
                OnClick="btninsurprnt_click" />
            <br />
            <br />
            <div id="rprint" runat="server" visible="false">
                <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                    Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
                <asp:TextBox ID="txtexcel" onkeypress="display()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnexcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" CssClass="textbox textbox1 btn3" Text="Export Excel" OnClick="btnexcel_Click"   Height="30px"  />
                <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn3"   Height="30px"  />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </div>
            <br />
        </div>
    </center>
</asp:Content>

