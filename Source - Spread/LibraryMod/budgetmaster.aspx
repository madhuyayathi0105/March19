<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="budgetmaster.aspx.cs" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" Inherits="LibraryMod_budgetmaster" %>
    <%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function frelig() {

            document.getElementById('<%= btnaddbudget .ClientID%>').style.display = 'block';
            document.getElementById('<%=btnsubbudget.ClientID%>').style.display = 'block';
        }
        function QuantityChange() {
            var book = 0.0;
            var book1 = 0.0;
            var book2 = 0.0;
            var jour = 0.0;
            var jour1 = 0.0;
            var jour2 = 0.0;
            var non = 0.0;
            var non1 = 0.0;
            var non2 = 0.0;
            var tobud = 0.0;
            var lblAmt = document.getElementById("<%=Txtbooks1.ClientID %>");
            var lblAmt4 = document.getElementById("<%=Txtbook2.ClientID %>");
            var lblAmt5 = document.getElementById("<%=Txtbook3.ClientID %>");
            if (lblAmt4.value.trim() != "") {
                book2 = parseFloat(lblAmt4.value);
                //alert(book2);
            }

            if (lblAmt.value.trim() != "") {
                book = parseFloat(lblAmt.value);
            }
            book1 = book - book2;
            document.getElementById("<%=Hidden3.ClientID %>").value = book1;
            lblAmt5.value = book1;

            var lblAmt1 = document.getElementById("<%=Txtjou1.ClientID %>");
            if (lblAmt1.value.trim() != "") {
                jour = parseFloat(lblAmt1.value);
            }
            var lblAmt6 = document.getElementById("<%=Txtjou2.ClientID %>");
            var lblAmt7 = document.getElementById("<%=Txtjou3.ClientID %>");
            if (lblAmt6.value.trim() != "") {
                jour2 = parseFloat(lblAmt6.value);
            }
            jour1 = jour - jour2;
            document.getElementById("<%=Hidden4.ClientID %>").value = jour1;
            lblAmt7.value = jour1;

            var lblAmt2 = document.getElementById("<%=Txtnobok1.ClientID %>");
            if (lblAmt2.value.trim() != "") {
                non = parseFloat(lblAmt2.value);
            }
            var lblAmt8 = document.getElementById("<%=Txtnobok2.ClientID %>");
            var lblAmt9 = document.getElementById("<%=Txtnobok3.ClientID %>");
            if (lblAmt8.value.trim() != "") {
                non2 = parseFloat(lblAmt8.value);
            }
            non1 = non - non2;
            document.getElementById("<%=Hidden5.ClientID %>").value = non1;
            lblAmt9.value = non1;
            var lblAmt10 = document.getElementById("<%=Txtbal.ClientID %>");
            var txtbalan = jour1 + book1 + non1;
            document.getElementById("<%=Hidden2.ClientID %>").value = txtbalan;
            lblAmt10.value = jour1 + book1 + non1;
            var lblAmt3 = document.getElementById("<%=Txtbudgetamt.ClientID %>");
            tobud = jour + book + non;
            lblAmt3.value = tobud;
            document.getElementById("<%=Hidden1.ClientID %>").value = tobud;

            var idval = "";
            var empty = "";
            var id = "";
            var value1 = "";
            id = document.getElementById("<%=ddlbudgethead.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlbudgethead.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlbudgetdept.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlbudgetdept.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=Txtbudgetamt.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=Txtbudgetamt.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtbudget.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtbudget.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }

        }
        function QuantityChange1() {
            var book = 0.0;
            var jour = 0.0;
            var non = 0.0;
            var tobud = 0.0;
            var lblAmt = document.getElementById("<%=Txtbook2.ClientID %>");
            if (lblAmt.value.trim() != "") {
                book = parseFloat(lblAmt.value);
            }

            var lblAmt1 = document.getElementById("<%=Txtjou1.ClientID %>");
            if (lblAmt1.value.trim() != "") {
                jour = parseFloat(lblAmt1.value);
            }
            var lblAmt2 = document.getElementById("<%=Txtnobok1.ClientID %>");
            if (lblAmt2.value.trim() != "") {
                non = parseFloat(lblAmt2.value);
            }
            var lblAmt3 = document.getElementById("<%=Txtbudgetamt.ClientID %>");
            tobud = jour + book + non;
            lblAmt3.value = tobud;
        }
        function QuantityChange2() {
            var book = 0.0;
            var jour = 0.0;
            var non = 0.0;
            var tobud = 0.0;
            var lblAmt = document.getElementById("<%=Txtbooks1.ClientID %>");
            if (lblAmt.value.trim() != "") {
                book = parseFloat(lblAmt.value);
            }

            var lblAmt1 = document.getElementById("<%=Txtjou1.ClientID %>");
            if (lblAmt1.value.trim() != "") {
                jour = parseFloat(lblAmt1.value);
            }

            var lblAmt2 = document.getElementById("<%=Txtnobok1.ClientID %>");
            if (lblAmt2.value.trim() != "") {
                non = parseFloat(lblAmt2.value);
            }
            var lblAmt3 = document.getElementById("<%=Txtbudgetamt.ClientID %>");
            tobud = jour + book + non;
            lblAmt3.value = tobud;
        }

    </script>
    <style type="text/css">
        .fontbold
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        
        .fontnormal
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
    </style>
    <style>
        .fontblack
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: Black;
        }
        .fontcolorb
        {
            color: Green;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <center>
            <span class="fontstyleheader" style="color: green; margin: 0px; margin-bottom: 10px;
                margin-top: 10px; position: relative;">Budget Master</span></center>
        <center>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="maintablestyle" style="margin: 0px; margin-bottom: 0px; margin-top: 8px;
                        font-family: Book Antiqua; font-weight: bold; position: relative;" width="auto">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="170px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                    <asp:ListItem Text="All"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Library" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddllib" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="170px" AutoPostBack="True" OnSelectedIndexChanged="ddllib_SelectedIndexChanged">
                                    <asp:ListItem Text="All"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbldepartment" runat="server" Text="Department" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldepartment" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="170px" AutoPostBack="True" OnSelectedIndexChanged="ddldepartment_SelectedIndexChanged">
                                    <asp:ListItem Text="All"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblhead" runat="server" Text="Head" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlhead" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="170px" AutoPostBack="True" OnSelectedIndexChanged="ddlhead_SelectedIndexChanged">
                                    <asp:ListItem Text="All"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="Chkaccno" runat="server" Text="" AutoPostBack="True" OnCheckedChanged="Chkaccno_CheckedChange" />
                                <asp:Label ID="lbldate" runat="server" Text="From Date" CssClass="commonHeaderFont"
                                    Font-Names=" Book antiqua">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fromdate" runat="server" Style="width: 142px;" onchange="return checkDate()"
                                    Enabled="false" CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbltodate" runat="server" Text="To Date" CssClass="commonHeaderFont"
                                    Font-Names=" Book antiqua">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate" runat="server" Style="width: 142px;" onchange="return checkDate()"
                                    Enabled="false" CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpGoAdd" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="Btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_Click" />
                                        <asp:ImageButton ID="btnadd" runat="server" ImageUrl="~/LibImages/Add.jpg" OnClick="btnadd_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
    </center>
    <br />
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                            <asp:GridView ID="Grdbudget" runat="server" ShowFooter="false" Width="900px" AutoGenerateColumns="true"
                                Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true" toGenerateColumns="true"
                                OnRowDataBound="Grdbudget_OnRowDataBound" AllowPaging="true" PageSize="500" OnPageIndexChanging="Grdbudget_OnPageIndexChanged"
                                OnRowCreated="Grdbudget_OnRowCreated" OnSelectedIndexChanged="Grdbudget_SelectedIndexChanged">
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="Grdbudget" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <br />
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="rptprint" runat="server" visible="true">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
                        Font-Size="Medium" CssClass="textbox txtheight2"></asp:TextBox>
                    <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                        OnClick="btnExcel_Click" />
                    <asp:ImageButton ID="btnprintmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                        OnClick="btnprintmaster_Click" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <div id="divPopAlertbudget" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 426px;"
                        OnClick="btn_popclose_Click" />
                    <br />
                    <center>
                        <div id="divPopAlertContentbudget" runat="server" style="background-color: White;
                            height: 470px; font-family: Book Antiqua; font-weight: bold; width: 900px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsgNEW" runat="server" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <div>
                                        <asp:Label ID="lblnonbook" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                                            position: relative;" Text="Budget Master" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
                                    </div>
                                    <tr>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lblbudgetcode" runat="server" Text="Budget Code:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbudget" runat="server" AutoPostBack="true" Width="170px" BackColor="#ffffcc"
                                                Enabled="false" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="From:" CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_fromdatebudget" runat="server" Style="width: 163px;" onchange="return checkDate()"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdatebudget"
                                                runat="server" Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="To:" CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_todatebudget" runat="server" Style="width: 100px;" onchange="return checkDate()"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_todatebudget" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lblbudgethead" runat="server" Text="Head" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnaddbudget" runat="server" Style="font-family: Book Antiqua; font-size: small;
                                                height: 31px; display: block; left: 419px; position: absolute; top: 125px; width: 27px;"
                                                OnClick="btnaddbudget_Click" Text="+" />
                                            &nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlbudgethead" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Style="width: 163px; margin-left: -12px;" AutoPostBack="True" OnSelectedIndexChanged="ddlbudgethead_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Button ID="btnsubbudget" runat="server" Style="font-family: Book Antiqua; font-size: small;
                                                height: 31px; display: block; left: 607px; position: absolute; top: 125px; width: 27px;"
                                                OnClick="btnsubbudget_Click" Text="-" />
                                        </td>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lbldept" runat="server" Text="Department" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlbudgetdept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="170px" AutoPostBack="True" OnSelectedIndexChanged="ddlbudgetdept_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lblbudgetamt" runat="server" Text="Budget Amount:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtbudgetamt" runat="server" AutoPostBack="true" Width="171px" BackColor="#ffffcc"
                                                Enabled="false" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblamt" runat="server" Text="Amount Spend:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtamt" runat="server" AutoPostBack="true" Width="161px" BackColor="#ffffcc"
                                                Enabled="false" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblbal" runat="server" Text="Balance Amount:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtbal" runat="server" AutoPostBack="true" Width="98px" BackColor="#ffffcc"
                                                Enabled="false" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <fieldset style="width: 603px; height: 165px;">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label4" runat="server" Text="Budget Amount" CssClass="commonHeaderFont"
                                        Font-Names=" Book antiqua">
                                    </asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label5" runat="server" Text="Amount Spend" CssClass="commonHeaderFont"
                                        Font-Names=" Book antiqua">
                                    </asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label6" runat="server" Text="Balance Amount" CssClass="commonHeaderFont"
                                        Font-Names=" Book antiqua">
                                    </asp:Label><br />
                                    <asp:Label ID="lblbk" runat="server" Text="Books:" CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                    </asp:Label>
                                    <asp:TextBox ID="Txtbooks1" runat="server" AutoPostBack="true" Style="width: 161px;
                                        margin-left: 10px;" CssClass="textbox txtheight2" onchange="return QuantityChange()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_mno" runat="server" TargetControlID="Txtbooks1"
                                        FilterType="numbers,custom" ValidChars=" .">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:TextBox ID="Txtbook2" runat="server" AutoPostBack="true" Width="161px" BackColor="#ffffcc"
                                        Enabled="false" CssClass="textbox txtheight2" onchange="return QuantityChange1()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="Txtbook2"
                                        FilterType="numbers,custom" ValidChars=" .">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:TextBox ID="Txtbook3" runat="server" AutoPostBack="true" Width="161px" BackColor="#ffffcc"
                                        Enabled="false" CssClass="textbox txtheight2" onchange="return QuantityChange2()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="Txtbook3"
                                        FilterType="numbers,custom" ValidChars=" .">
                                    </asp:FilteredTextBoxExtender>
                                    <br />
                                    <br />
                                    <asp:Label ID="lbljournal" runat="server" Text="Journal:" CssClass="commonHeaderFont"
                                        Font-Names=" Book antiqua">
                                    </asp:Label>
                                    <asp:TextBox ID="Txtjou1" runat="server" AutoPostBack="true" Width="161px" CssClass="textbox txtheight2"
                                        onchange="return QuantityChange()"></asp:TextBox><asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                            runat="server" TargetControlID="Txtjou1" FilterType="numbers,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                    <asp:TextBox ID="Txtjou2" runat="server" AutoPostBack="true" Width="161px" BackColor="#ffffcc"
                                        Enabled="false" CssClass="textbox txtheight2" onchange="return QuantityChange1()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="Txtjou2"
                                        FilterType="numbers,custom" ValidChars=" .">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:TextBox ID="Txtjou3" runat="server" AutoPostBack="true" Width="161px" BackColor="#ffffcc"
                                        Enabled="false" CssClass="textbox txtheight2" onchange="return QuantityChange2()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="Txtjou3"
                                        FilterType="numbers,custom" ValidChars=" .">
                                    </asp:FilteredTextBoxExtender>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblnonbookkk" runat="server" Text="Non Book:" CssClass="commonHeaderFont"
                                        Style="font-family: Book antiqua; margin-left: -17px;">
                                    </asp:Label>
                                    <asp:TextBox ID="Txtnobok1" runat="server" AutoPostBack="true" Width="161px" CssClass="textbox txtheight2"
                                        onchange="return QuantityChange()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txtnobok1"
                                        FilterType="numbers,custom" ValidChars=" .">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:TextBox ID="Txtnobok2" runat="server" AutoPostBack="true" Width="161px" BackColor="#ffffcc"
                                        Enabled="false" CssClass="textbox txtheight2" onchange="return QuantityChange1()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="Txtnobok2"
                                        FilterType="numbers,custom" ValidChars=" .">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:TextBox ID="Txtnobok3" runat="server" AutoPostBack="true" Width="161px" BackColor="#ffffcc"
                                        Enabled="false" CssClass="textbox txtheight2" onchange="return QuantityChange2()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="Txtnobok3"
                                        FilterType="numbers,custom" ValidChars=" .">
                                    </asp:FilteredTextBoxExtender>
                                </fieldset>
                                <br />
                                <asp:Label ID="lblremarks" runat="server" Text="Remarks:" CssClass="commonHeaderFont"
                                    Style="font-family: Book antiqua; margin-left: -309px;">
                                </asp:Label>
                                <asp:TextBox ID="Txtremarks" runat="server" AutoPostBack="true" Width="210px" CssClass="textbox txtheight2"></asp:TextBox>
                                <br />
                                <br />
                                <asp:UpdatePanel ID="UpButton" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btnsavebud" runat="server" Visible="false" ImageUrl="~/LibImages/save.jpg"
                                            OnClick="btnsavebud_Click" OnClientClick="return QuantityChange();" />
                                        <asp:ImageButton ID="Btnup" runat="server" Visible="false" ImageUrl="~/LibImages/update.jpg"
                                            OnClick="Btnup_Click" OnClientClick="return QuantityChange();" />
                                        <asp:ImageButton ID="Btndele" runat="server" Visible="false" ImageUrl="~/LibImages/delete.jpg"
                                            OnClick="Btndele_Click" OnClientClick="return QuantityChange();" />
                                        <asp:ImageButton ID="btnclosebud" runat="server" Visible="false" ImageUrl="~/LibImages/save (2).jpg"
                                            OnClick="btnclosebud_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <div id="Div1" runat="server" visible="false" style="height: 120px; z-index: 0px;
                    width: 0px; background-color: rgba(54, 25, 25, .2); position: absolute; margin-top: 115px;
                    left: 80px;">
                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: -389px;
                        border-radius: 10px; margin-left: -49px">
                        <center>
                            <table style="height: 100px; width: 120px">
                                <tr align="center">
                                    <td align="center" colspan="2">
                                        <asp:TextBox ID="txt_infra" runat="server" CssClass="textbox txtheight2" Visible="true"
                                            Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:ImageButton ID="btnsave" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="btnsave_Click" />
                                            <asp:ImageButton ID="btnexit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                OnClick="btnexit_Click" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <input type="hidden" runat="server" id="Hidden1" />
    <input type="hidden" runat="server" id="Hidden2" />
    <input type="hidden" runat="server" id="Hidden3" />
    <input type="hidden" runat="server" id="Hidden4" />
    <input type="hidden" runat="server" id="Hidden5" />
    <input type="hidden" runat="server" id="Hidden6" />
    <center>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 360px;
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
                                                <asp:ImageButton ID="btnerrclose" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btnerrclose_Click" />
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
    <%--Progress bar for add and go--%>
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
    <%--Progress bar for add and go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpButton">
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
