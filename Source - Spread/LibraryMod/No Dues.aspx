<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="No Dues.aspx.cs" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" Inherits="LibraryMod_No_Dues" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function PrintDiv() {
            var panel = document.getElementById("<%=contentDiv.ClientID %>");
            var printWindow = window.open('', '', 'height=auto,width=685');
            printWindow.document.write('<html');
            printWindow.document.write('<head> <style type="text/css"> p{ font-size: x-small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
            printWindow.document.write('</head><body>');
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
        function QuantityChange1(objRef) {
            var grdvw = document.getElementById("<%=grdNoDues.ClientID %>");
            var grid = document.getElementById('<%=grdNoDues.ClientID%>');
            var ddl = document.getElementById('MainContent_grdNoDues_selectall_0');

            if (ddl.checked == true) {
                for (var i = 1; i < grid.rows.length; i++) {
                    var ddl_select = document.getElementById('MainContent_grdNoDues_select_' + i.toString());
                    ddl_select.checked = true;
                }
            }
            else {
                for (var i = 1; i < grid.rows.length; i++) {
                    var ddl_select = document.getElementById('MainContent_grdNoDues_select_' + i.toString());
                    ddl_select.checked = false;
                }
            }
        }
        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=grdNoDues.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 1; i < (tbl.rows.length); i++) {
                var chkSelectid = document.getElementById('MainContent_grdNoDues_selectchk_' + i.toString());

                if (chkSelAll.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <center>
        <span class="fontstyleheader" style="color: green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">No Dues</span></center>
    <center>
        <div style="color: Black; font-family: Book Antiqua; font-weight: bold; height: 123px;
            width: 937px; margin: 0px; margin-top: 10px; margin-bottom: 15px; position: relative;
            text-align: left; font-family: Book Antiqua; font-weight: bold" class="maintablestyle">
            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Lblcollege" runat="server" Text="College" CssClass="commonHeaderFont"
                                    Visible="true"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Visible="true" Style="width: 140px; height: 30px;" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                            <%----%>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="Lbllib" runat="server" Text="Library Name" CssClass="commonHeaderFont"
                                    Visible="true"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddllib" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Visible="true" Style="width: 140px; height: 30px;" AutoPostBack="True">
                                            <%--OnSelectedIndexChanged="ddltransfrom_SelectedIndexChanged"--%>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="Chkbatch" runat="server" Text="Batch" AutoPostBack="True" OnCheckedChanged="Chkbatch_CheckedChange" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Visible="True" Style="width: 140px; height: 30px;" AutoPostBack="True" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 206px; height: 11px;">
                                    <asp:RadioButton ID="rdbstudent" runat="server" Text="Student" Checked="true" GroupName="Move"
                                        AutoPostBack="true" />
                                    <asp:RadioButton ID="rdbstaff" runat="server" Text="Staff" GroupName="Move" AutoPostBack="true" />
                                    <asp:RadioButton ID="rdbboth" runat="server" Text="Both" GroupName="Move" AutoPostBack="true" />
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lblcourse" runat="server" Text="Course" CssClass="commonHeaderFont"
                                    Visible="True"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlcourse" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Visible="True" Style="width: 140px; height: 30px;" AutoPostBack="True" OnSelectedIndexChanged="ddlcourse_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="Lbldept" runat="server" Text="Department" CssClass="commonHeaderFont"
                                    Visible="True"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Visible="True" Style="width: 140px; height: 30px;" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblnodues" runat="server" Text="No Dues" Style="margin-left: 7px;"
                                    CssClass="commonHeaderFont" Visible="true"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlnodues" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Visible="true" Style="width: 140px; height: 30px;" AutoPostBack="True">
                                            <%--OnSelectedIndexChanged="ddltransfrom_SelectedIndexChanged"--%>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="Lblstatus" runat="server" Text="Status" Style="margin-left: 5px;"
                                    CssClass="commonHeaderFont" Visible="true"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlstatus" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Visible="true" Style="width: 140px; height: 30px;" AutoPostBack="True">
                                            <%--OnSelectedIndexChanged="ddltransfrom_SelectedIndexChanged"--%>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="Cboldsearch" runat="server" Text="From" AutoPostBack="True" OnCheckedChanged="Cboldsearch_CheckedChange" />
                                        <asp:TextBox ID="txt_from" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                            Width=" 65px" Visible="true" Enabled="false"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_from" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                        <asp:Label ID="Label2" runat="server" Text="To" CssClass="commonHeaderFont" Visible="true"></asp:Label>
                                        <asp:TextBox ID="Txtto" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                            Width=" 65px" Visible="true" Enabled="false"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="Txtto" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="5" align="right">
                                <asp:UpdatePanel ID="UpGoAdd" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="Btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-top: 10px;"
                                            OnClick="Go_Click" />
                                        <asp:ImageButton ID="Btnadd" runat="server" ImageUrl="~/LibImages/Add.jpg" Style="margin-top: 10px;"
                                            OnClick="add_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </center>
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <span style="padding-right: 100px; margin-left: -260px; margin-top: 3px;">
                    <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                        onchange="return SelLedgers();" Style="margin-left: -356px;" />
                </span>
                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                <asp:GridView ID="grdNoDues" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                    ShowHeader="true" Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true"
                    PageSize="100" OnSelectedIndexChanged="grdNoDues_onselectedindexchanged" OnPageIndexChanging="grdNoDues_onpageindexchanged"
                    OnRowCreated="grdNoDues_OnRowCreated" OnRowDataBound="grdNoDues_RowDataBound"
                    Width="1100px">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                 <asp:Label ID="lbl_sel" runat="server" Text="Select" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="selectchk" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="grdNoDues" />
            </Triggers>
        </asp:UpdatePanel>
        <center>
            <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                <ContentTemplate>
                    <div id="print" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        ForeColor="Red" Text="" Visible="false"></asp:Label>
                                    <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
                                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                                        OnClick="btnExcel_Click" />
                                    <asp:ImageButton ID="btnprintmasterhed" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                        OnClick="btnprintmaster_Click" />
                                    <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExcel" />
                    <asp:PostBackTrigger ControlID="btnprintmasterhed" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
        <%--     ------------------------ no  dues-----------------------------------------%>
        <center>
            <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                <ContentTemplate>
                    <div id="nodues" runat="server" class="popupstyle popupheight1" visible="false">
                        <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 257px; margin-left: 850px;"
                            OnClick="btn_Question_Bank_popup_Click" />
                        <br />
                        <br />
                        <div style="background-color: White; font-family: Book Antiqua; font-weight: bold;
                            height: 650px; width: 910px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                            border-radius: 10px;">
                            <center>
                                <span class="fontstyleheader" style="color: Green;">No Dues</span>
                            </center>
                            <table>
                                <tr>
                                    <td>
                                        <center>
                                            <table style="height: auto; background-color: bisque; margin-left: -1px; margin-top: 10px;
                                                margin-bottom: 10px; padding: 6px; width: 895px">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Lbllibrary" runat="server" Text="Library" CssClass="commonHeaderFont">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddllibrary" runat="server" Style="width: 129px;" AutoPostBack="true"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                    <%--OnSelectedIndexChanged="ddlcollege_sts_SelectedIndexChanged"--%>
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Lbluserentry" runat="server" Text="UserEntry" CssClass="commonHeaderFont">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddluserentry" runat="server" Style="width: 142px;" AutoPostBack="true"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <fieldset style="width: 144px; height: 18px;">
                                                            <asp:RadioButton ID="rdbstu" runat="server" Text="Student" Checked="true" GroupName="Move12"
                                                                OnCheckedChanged="rdbstu_CheckedChange" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rdbstaff_stu" runat="server" Text="Staff" GroupName="Move12"
                                                                AutoPostBack="true" OnCheckedChanged="rdbstu_CheckedChange" />
                                                        </fieldset>
                                                    </td>
                                                    <td rowspan="3">
                                                        <asp:Image ID="image2" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 110px;
                                                            width: 130px;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1">
                                                        <asp:Label ID="Lblroll" runat="server" Text="Roll No" CssClass="commonHeaderFont">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_rollno" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                                                    OnTextChanged="txt_rollno_TextChanged" MaxLength="15" Visible="true" Width="119px"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td colspan="1">
                                                        <asp:Label ID="Lblname" runat="server" Text="Name" CssClass="commonHeaderFont">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtname" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                                                    Enabled="false" Visible="true" Width="133px"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1">
                                                        <asp:Label ID="lblsem" runat="server" Text="Semester" CssClass="commonHeaderFont">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtsem" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                                                    Enabled="false" Visible="true" Width="118px"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td colspan="1">
                                                        <asp:Label ID="Lbldept1" runat="server" Text="Department" CssClass="commonHeaderFont">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtdept" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                                                    Enabled="false" Visible="true" Width="133px"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan='2'>
                                                        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                            <ContentTemplate>
                                                                <asp:CheckBox ID="Chkissued" runat="server" Text="No Dues Issued" AutoPostBack="True"
                                                                    OnCheckedChanged="Chkissued_CheckedChange" />
                                                                <%--OnCheckedChanged="Chkbatch_CheckedChange" --%>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Lblissuedate" runat="server" Text="Issued Date" CssClass="commonHeaderFont"
                                                            Visible="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtissued" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                                                    Visible="true" Enabled="false" Width="132px"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtissued" runat="server"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label ID="Lblremark" runat="server" Text="Remarks" CssClass="commonHeaderFont">
                                                        </asp:Label>
                                                        <asp:TextBox ID="Txt_Remarks" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                                            Visible="true" Width="200px"></asp:TextBox>
                                                    </td>
                                                    <%--<td>
                                                    <asp:Button ID="btn_sts_Rack_Go" Style="width: 70px; height: 30px; margin-left: 31px;"
                                                        runat="server" CssClass="textbox btn2" Text="Go" />--%><%--OnClick="btn_sts_Rack_Go_Click"--%>
                                                </tr>
                                            </table>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <center>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div id="DivDueList" runat="server" visible="false" style="width: 900px; overflow: auto;
                                            background-color: White; border-radius: 10px;">
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                            <asp:GridView ID="grdNoDuesForm" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                                Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="50"
                                                Width="980px">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                                            </asp:Label></center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                            <br />
                            <center>
                                <asp:UpdatePanel ID="Upsave" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btnsave" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save.jpg"
                                            OnClick="save_Click" />
                                        <asp:ImageButton ID="btnupdate" runat="server" Font-Bold="true" ImageUrl="~/LibImages/update (2).jpg"
                                            OnClick="update_Click" />
                                        <asp:ImageButton ID="btnprintletter" runat="server" Font-Bold="true" ImageUrl="~/LibImages/Print White.jpg"
                                            OnClick="btnprintletter_Click" />
                                        <asp:ImageButton ID="btnexit" runat="server" Font-Bold="true" ImageUrl="~/LibImages/save (2).jpg"
                                            OnClick="btnexit_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <asp:UpdatePanel ID="UpdatePanel19" runat="server">
            <ContentTemplate>
                <div style="height: 1px; width: 1px; overflow: auto;">
                    <div id="contentDiv" runat="server" style="height: auto; width: 900px;" visible="false">
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%-- Popup for Books Due--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel21" runat="server">
            <ContentTemplate>
                <div id="DivBooksDue" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div6" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblBookDue" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:ImageButton ID="BtnBooksDueYes" runat="server" ImageUrl="~/LibImages/yes.jpg"
                                                    OnClick="btnBooksDueYes_Click" />
                                                <asp:ImageButton ID="BtnBooksDueNo" runat="server" ImageUrl="~/LibImages/no.jpg"
                                                    OnClick="btnBooksDueNo_Click" />
                                                <%--<asp:Button ID="BtnIssueYesAgain" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btnIssueYes_Click" Text="Yes" runat="server" />--%>
                                                <%--<asp:Button ID="BtnIssueNoAgain" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btnIssueNo_Click" Text="No" runat="server" />--%>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%-- Popup for Print--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel24" runat="server">
            <ContentTemplate>
                <div id="DivSurePrint" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblSurePrint" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/LibImages/yes.jpg"
                                                    OnClick="btnSurePrintYes_Click" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/LibImages/no.jpg" OnClick="btnSurePrintNo_Click" />
                                                <%--<asp:Button ID="BtnIssueYesAgain" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btnIssueYes_Click" Text="Yes" runat="server" />--%>
                                                <%--<asp:Button ID="BtnIssueNoAgain" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btnIssueNo_Click" Text="No" runat="server" />--%>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--    ------------------------------------------end no  dues-----------------------------%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel25" runat="server">
            <ContentTemplate>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 100px;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 50px;
                    left: 0px;">
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:ImageButton ID="btnerrclose" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btnerrclose_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for Add--%>
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
    <%--progressBar for save--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Upsave">
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
