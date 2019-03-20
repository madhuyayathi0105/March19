<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="ReceiptJpr.aspx.cs" Inherits="ReceiptJpr" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title>Receipt Jeppiar</title>
    <link rel="Shortcut Icon" href="college/Left_Logo.jpeg" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
        <script type="text/javascript" language="javascript">

            function PrintDiv() {
                var panel = document.getElementById("<%=contentDiv.ClientID %>");
                var printWindow = window.open('', '', 'height=816,width=1000');
                printWindow.document.write('<html><head>');
                printWindow.document.write('<style> .classRegular { font-family:Arial; font-size:10px; } .classBold10 { font-family:Arial; font-size:12px; font-weight:bold;} .classBold12 { font-family:Arial; font-size:14px; font-weight:bold;} .classBold { font-family:Arial; font-size:10px; font-weight:bold;} </style>');
                printWindow.document.write('</head><body >');
                printWindow.document.write(panel.innerHTML);
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                setTimeout(function () {
                    printWindow.print();
                }, 500);
                return false;
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: green;">Receipt</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: 950px; height: 420px;">
                <br />
                <table class="maintablestyle" style="width: 900px; text-align: left;">
                    <tr>
                        <td style="width: 250px;">
                            <div>
                                <asp:RadioButton ID="rb_single" runat="server" Text="Single" GroupName="s1" OnCheckedChanged="rb_single_Change"
                                    Checked="true" AutoPostBack="true" />
                                <asp:RadioButton ID="rb_multiple" runat="server" Text="Multiple" GroupName="s1" OnCheckedChanged="rb_multiple_Change"
                                    AutoPostBack="true" />
                            </div>
                        </td>
                        <td style="width: 250px;">
                            <asp:RadioButtonList ID="rbl_PartFull" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="txt_rerollno_TextChanged">
                                <asp:ListItem Selected="True">Current Payment</asp:ListItem>
                                <asp:ListItem>Full Payment</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="width: 200px;">
                            &nbsp;
                        </td>
                        <td>
                            Date
                        </td>
                        <td>
                            <asp:TextBox ID="txt_rdate" runat="server" CssClass="textbox txtheight textbox2"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_rdate" runat="server"
                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <center>
                    <div id="div_Single" runat="server" visible="true">
                        <div style="width: 900px">
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                                OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_rerollno" runat="server" AutoPostBack="true" CssClass="textbox txtheight1 textbox1"
                                                OnTextChanged="txt_rerollno_TextChanged"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_rerollno"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rerollno"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:TextBox ID="TextBox2" runat="server" Style="display: none" CssClass="textbox txtheight4 textbox1"
                                                onchange="return checkrno(this.value)" onkeyup="return checkrno(this.value)"
                                                onblur="return get(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                            <span style="color: Red;">*</span> <span id="Span1"></span>
                                        </td>
                                        <td>
                                            Name
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txt_rename" runat="server" ReadOnly="true" CssClass="textbox txtheight5 textbox1"></asp:TextBox>
                                            <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_rename"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rename"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>--%>
                                        </td>
                                        <td>
                                        </td>
                                        <td rowspan="3">
                                            <asp:Image ID="image3" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 100px;
                                                width: 100px;" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Batch
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_rebatch" runat="server" CssClass="txtheight textbox textbox1">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txt_recolg" runat="server" CssClass="textbox txtheight5 textbox1">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <%--Semester--%>
                                            <asp:TextBox ID="txt_resem" Visible="false" runat="server" CssClass="txtheight textbox textbox1"
                                                Width="30px">

                                            </asp:TextBox>
                                            <%--<asp:UpdatePanel ID="UPpanel_sem" runat="server">
                                            <ContentTemplate>--%>
                                            <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox txtheight1" Width="110px"
                                                ReadOnly="true" placeholder="Semester/Year" onfocus="return myFunction1(this)"></asp:TextBox>
                                            <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_sem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_sem_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupce_sem" runat="server" TargetControlID="txt_sem"
                                                PopupControlID="panel_sem" Position="Bottom">
                                            </asp:PopupControlExtender>
                                            <%--</ContentTemplate>
                                            <Triggers> </Triggers>
                                        </asp:UpdatePanel>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_redegree" runat="server" CssClass="txtheight textbox textbox1">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txt_redept" runat="server" CssClass="textbox txtheight5 textbox1">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            Section&nbsp;&nbsp;&nbsp;
                                            <asp:TextBox ID="txt_resec" runat="server" CssClass="txtheight textbox textbox1"
                                                Width="30px">
                                            </asp:TextBox>
                                            <asp:Label ID="lblsem" runat="server" Visible="false" Text="Semester"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                        <%--*******2nd div*****--%>
                        <div style="float: left; width: 900px;">
                            <center>
                                <div style="border-radius: 10px; border: 1px solid Gray; width: 800px; height: 200px;
                                    overflow: auto;">
                                    <%-- <div style="float: right; padding-right: 20px;">
                                <asp:LinkButton ID="lnkbtn_viewhistory" runat="server" Visible="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Blue" CausesValidation="False"
                                    OnClick="lnkbtn_viewhistory_Click">Fees Paid History</asp:LinkButton>
                            </div>
                            <br />--%>
                                    <div style="height: 170px; overflow: auto;">
                                        <asp:GridView ID="gridView3" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                            OnRowDataBound="gridView3_OnRowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("Sno") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:CheckBox ID="cb_Sel" runat="server" />
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Year/Semester" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_yearsem" runat="server" Text='<%#Eval("YearSem") %>'></asp:Label>
                                                            <asp:Label ID="lbl_feecat" runat="server" Visible="false" Text='<%#Eval("FeeCategory") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_hdr" runat="server" Text='<%#Eval("Header") %>'>
                                                        </asp:Label>
                                                        <asp:Label ID="lbl_hdrid" runat="server" Visible="false" Text='<%#Eval("HeaderFk") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_lgr" runat="server" Text='<%#Eval("Ledger") %>'>
                                                        </asp:Label>
                                                        <asp:Label ID="lbl_lgrid" runat="server" Visible="false" Text='<%#Eval("LedgerFk") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_feeamt" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_totamt" runat="server" Text='<%#Eval("FeeAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Concess" runat="server" Text='<%#Eval("Concession") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_paid" runat="server" Text='<%#Eval("Paid") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_bal" runat="server" Text='<%#Eval("Balance") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div>
                                        <table id="tblgrid3" runat="server" visible="false">
                                            <tr>
                                                <td>
                                                    Total :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_grid3_tot" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td>
                                                    Paid :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_grid3_paid" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td>
                                                    Balance :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_grid3_bal" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <table>
                                    <tr>
                                        <%--<td>
                                <asp:CheckBox ID="cb_Narrration" runat="server" Text="Narration" />
                                <asp:TextBox ID="txt_Narration" runat="server" CssClass="textbox textbox1 txtheight4" Width="350px"></asp:TextBox>
                                </td>--%>
                                        <td>
                                            <asp:Button ID="btn_Print" runat="server" CssClass="textbox textbox1 btn2" Text="Print"
                                                Visible="false" OnClick="btn_Print_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </div>
                    <%--***********end of Single receipt div*********--%>
                    <div id="div_Multiple" runat="server" visible="false">
                        <div style="width: 900px">
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_semrcpt" runat="server" Text="Semester/Year"></asp:Label>
                                            <asp:DropDownList ID="ddl_semrcpt" runat="server" CssClass="textbox  ddlheight2">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblstudrcpt" runat="server" Text="Total No.of Students"></asp:Label>
                                            <asp:TextBox ID="txt_tostudentsrcpt" runat="server" CssClass="textbox txtheight"
                                                MaxLength="8" Style="text-align: right;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_tostudentsrcpt"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnrcptRoll" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                                                OnClick="btn_roll_Click" />
                                            <asp:Button ID="btn_print2" Visible="false" Text="Print" runat="server" CssClass="textbox textbox1 btn2"
                                                OnClick="btn_print2_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </div>
                    <%--***********end of Multiple receipt div*********--%>
                </center>
            </div>
        </center>
        <%--  ******popup window******--%>
        <center>
            <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight1 ">
                <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 460px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 500px; width: 950px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green;">Select The Student</span></div>
                    </center>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_batch1" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_batch1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_stream" runat="server" Text="Stream"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Updp_strm" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_strm" runat="server" CssClass="textbox txtheight" ReadOnly="true"
                                            onfocus="return myFunction1(this)"></asp:TextBox>
                                        <asp:Panel ID="panel_strm" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Width="150px">
                                            <asp:CheckBox ID="cb_strm" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_strm_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_strm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_strm_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="Popupce_strm" runat="server" TargetControlID="txt_strm"
                                            PopupControlID="panel_strm" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree2" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight">--Select--</asp:TextBox>
                                        <asp:Panel ID="pdegree" runat="server" Width="150px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_degree2" runat="server" OnCheckedChanged="cb_degree2_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_degree2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree2_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_degree2"
                                            PopupControlID="pdegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_branch2" runat="server" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1 txtheight">--Select--</asp:TextBox>
                                        <asp:Panel ID="pbranch" runat="server" Width="250px" Height="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_branch1" runat="server" OnCheckedChanged="cb_branch1_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_branch1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch1_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_branch2"
                                            PopupControlID="pbranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_sec2" runat="server" Text="Section"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8sec" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sec2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1 txtheight">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlsec2" runat="server" Width="120px" Height="80px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_sec2" runat="server" OnCheckedChanged="cb_sec2_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_sec2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec2_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_sec2"
                                            PopupControlID="pnlsec2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_rollno3" runat="server" Text="Roll No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rollno3" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                    Height="20px" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_rollno3"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno3"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" Text="Go" OnClick="btn_go_Click" CssClass="textbox btn1 textbox1"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <div>
                        <asp:Label ID="lbl_errormsg" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                    <div>
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" ShowHeaderSelection="false"
                            BorderWidth="0px" Width="750px" Style="overflow: auto; height: 250px; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            OnUpdateCommand="Fpspread1_Command">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <br />
                    <center>
                        <div>
                            <asp:Button ID="btn_studOK" runat="server" CssClass="textbox btn2 textbox1" Text="Ok"
                                OnClick="btn_studOK_Click" />
                            <asp:Button ID="btn_exitstud" runat="server" CssClass="textbox btn2 textbox1" Text="Exit"
                                OnClick="btn_exitstud_Click" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <%-- Pop Alert--%>
        <center>
            <div id="imgAlert" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                        <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_alertclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <%-- New Print div--%>
        <div style="height: 1px; width: 1px; overflow: auto;">
            <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false">
            </div>
        </div>
    </body>
    </html>
</asp:Content>
