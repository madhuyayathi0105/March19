<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Attendance_certificate.aspx.cs" Inherits="StudentMod_Attendance_certificate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <script language="javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: #008000;">Attendance Certificate</span>
                    </div>
                    <br />
                </center>
                <div class="maindivstyle" style="height: auto; width: 1200px;">
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_collegename" Text="Institution Name" runat="server" CssClass="txtheight"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox1  ddlheight5"
                                    OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                    AutoPostBack="True" CssClass="textbox1  ddlheight3" Width="69px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldegree" runat="server" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                    AutoPostBack="True" CssClass="textbox1  ddlheight3">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbranch" runat="server" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                    AutoPostBack="True" CssClass="textbox1  ddlheight3" Width="271px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                    AutoPostBack="True" CssClass="textbox1  ddlheight3" Width="41px">
                                </asp:DropDownList>
                                <%--   </td>
                            <td>--%>
                                <asp:Label ID="lblsec" runat="server" Text="Section"></asp:Label>
                                <%-- </td>
                            <td>--%>
                                <asp:DropDownList ID="ddlsection" runat="server" AutoPostBack="True" Width="52px"
                                    CssClass="textbox1  ddlheight">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" CssClass="textbox btn1 " />
                            </td>
                            <td>
                                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                                    <asp:TextBox ID="txt_fromdate" Enabled="true" runat="server" onchange="return checkDate()"
                                                        Style="height: 20px; width: 67px;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                            </td>
                             <td>
                                                    <asp:Label ID="lbl_todate" runat="server" Text="To"></asp:Label>
                                                    <asp:TextBox ID="txt_todate" runat="server" Enabled="true" onchange="return checkDate()"
                                                        Style="height: 20px; width: 67px;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                            </td>
                           <%--  </tr>--%>
                      <%--      <tr>--%>
                            <td colspan="2">
                                Print Semester
                            </td>
                            <td colspan="3">
                                <div style="border: thin groove #C0C0C0;">
                                    <asp:CheckBoxList ID="cbl_printsem" runat="server" RepeatColumns="10">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                            <td>
                                <asp:Button ID="btn_Generate_cer" runat="server" Text="Generate Certificate" OnClick="btn_Generate_cer_Click"
                                    CssClass="textbox btn1" Width="125px" />
                            </td>
                           
                        </tr>
                    </table>
                    <br />
                    <div>
                        <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        <br />
                    </div>
                    <br />
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Width="917px" Height="360px" CssClass="spreadborder" Visible="false"
                        OnButtonCommand="Fpspread_command">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                            Width="180px" onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                            Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            CssClass="textbox btn1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                    <br />
                </div>
            </center>
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
        </div>
    </body>
    </html>
</asp:Content>
