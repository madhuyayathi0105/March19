<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ReAdmissionReport.aspx.cs" Inherits="StudentMod_ReAdmissionReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function PrintPanel() {
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>SchoolCompartmentWise</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green; font-size: x-large;">Readmission
                Process Report</span>
        </div>
        <br />
    </center>
    <div>
        <asp:Label ID="lbl_clgT" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lbl_degreeT" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lbl_branchT" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lbl_semT" runat="server" Visible="false"></asp:Label>
        <center>
            <asp:Panel ID="pnlContents" runat="server" Visible="false">
                <style type="text/css" media="print">
                    @page
                    {
                        size: A3 portrait;
                        margin: 0.5cm;
                    }
                    @media print
                    {
                        .printclass
                        {
                            display: table;
                        }
                        thead
                        {
                            display: table-header-group;
                        }
                        tfoot
                        {
                            display: table-footer-group;
                        }
                        #header
                        {
                            position: fixed;
                            top: 0px;
                            left: 0px;
                        }
                        #footer
                        {
                            position: fixed;
                            bottom: 0px;
                            left: 0px;
                        }
                        #printable
                        {
                            position: relative;
                            bottom: 30px;
                            height: 300;
                        }
                    
                    }
                    @media screen
                    {
                        thead
                        {
                            display: block;
                        }
                        tfoot
                        {
                            display: block;
                        }
                    }
                </style>
            </asp:Panel>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lbl_clgname" runat="server" Text="College"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox1" runat="server" AutoPostBack="true"
                            Width="215px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_batch" runat="server" CssClass="ddlheight textbox1" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_degree" Text="Degree" Width="89px" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                    Width="165px" ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="cb_degree_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_degree"
                                    PopupControlID="p3" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                    ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="180px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="cb_branch_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch"
                                    PopupControlID="p4" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <%--<td>
                        <asp:Label ID="lbl_org_sem" Text="Semester" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox textbox1 txtheight3" ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_sem"
                                    PopupControlID="Panel11" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblFormat" runat="server" Text="Type"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlAppFormat" CssClass="ddlheight textbox1" runat="server"
                            Width="123px">
                        </asp:DropDownList>
                    </td>--%>
                    <td>
                        <asp:Label ID="lblStudent" runat="server" Text="Category"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlCatogery" CssClass="ddlheight1 textbox1" runat="server"
                            AutoPostBack="true" Width="123px" OnSelectedIndexChanged="ddlCatogery_SelectedIndexChanged">
                            <asp:ListItem Value="1">Discontinue</asp:ListItem>
                            <asp:ListItem Value="2">Prolong Absent</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        From Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtfromdate" runat="server" AutoPostBack="true" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                        <asp:CalendarExtender ID="calendetextenfordatext" TargetControlID="txtfromdate" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <td>
                        To Date
                    </td>
                    <td>
                        <asp:TextBox ID="txttodate" runat="server" AutoPostBack="true" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttodate" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="btn_go" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                            OnClick="btn_go_OnClick" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <center>
                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Style="height: 370px; overflow: auto; background-color: White;
                    border-radius: 10px; box-shadow: 0px 0px 8px #999999" ShowHeaderSelection="false"
                    Visible="false" > <%-- OnUpdateCommand="FpSpread1_Command"--%>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </center>
            <br />
        </center>
        <%--<asp:Button ID="btn_print" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Print" OnClick="btn_print_Click" Height="32px" Style="margin-top: 10px;"
                                CssClass="textbox textbox1" Width="60px" />
                                 <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />--%>
        <center>
            <div id="print" runat="server" visible="false">
                <asp:Label ID="lblvalidation1" runat="server" Text="Please Enter Your Report Name"
                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red" Style="display: none;"></asp:Label>
                <asp:Label ID="lblrptname" runat="server" Visible="false" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Visible="true" Width="180px" onkeypress="display()"
                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                    InvalidChars="/\">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnExcel" runat="server" Visible="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                    Height="32px" CssClass="textbox textbox1" />
                <asp:Button ID="btnprintmasterhed" runat="server" Visible="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Print Setting" OnClick="btnprintmaster_Click" Height="32px"
                    Style="margin-top: 10px;" CssClass="textbox textbox1" Width="100px" />
                <asp:Button ID="btn_print" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                    Text="Print" OnClick="btn_print_Click" Height="32px" Style="margin-top: 10px;"
                    CssClass="textbox textbox1" Width="60px" />
                <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
            </div>
        </center>
        <%--<center>
            <div id="pop_readmissiondet" runat="server" visible="false" style="height: 48em;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute;
                top: 0; left: 0;">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 13px; margin-left: 163px;"
                    OnClick="imagebtnpopclose4_Click" />
                <br />
                <div style="background-color: White; height: auto; width: 350px; border: 5px solid #0CA6CA;
                    border-top: 5px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <span style="color: Green;" class="fontstyleheader ">Readmission</span>
                    </center>
                    <br />
                    <table>
                        <tr>
                            <td>
                                Batch Year
                            </td>
                            <td>
                                <asp:DropDownList ID="RA_ddlbatch" CssClass="ddlheight textbox1" runat="server" Width="123px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Semester
                            </td>
                            <td>
                                <asp:DropDownList ID="RA_ddlsem" CssClass="ddlheight textbox1" runat="server" Width="123px">
                                    <asp:ListItem Selected="True" Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Remark
                            </td>
                            <td>
                                <asp:TextBox ID="RA_txtremark" runat="server" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Date
                            </td>
                            <td>
                                <asp:TextBox ID="RA_txtdate" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                    Format="dd/MM/yyyy" TargetControlID="RA_txtdate">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                    </table>
                </div>
            </div>
        </center>--%>
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
</asp:Content>
