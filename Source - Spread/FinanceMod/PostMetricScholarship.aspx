<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="PostMetricScholarship.aspx.cs" Inherits="PostMetricScholarship" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <style type="text/css">
        .printclass
        {
            display: none;
        }
        .grid-view
        {
            padding: 0;
            margin: 0;
            border: 1px solid #333;
            font-family: "Verdana, Arial, Helvetica, sans-serif, Trebuchet MS";
            font-size: 0.9em;
        }
        
        .grid-view tr.header
        {
            color: white;
            background-color: #0CA6CA;
            height: 30px;
            vertical-align: middle;
            text-align: center;
            font-weight: bold;
            font-size: 15px;
        }
        
        .grid-view tr.normal
        {
            color: black;
            background-color: #FDC64E;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.alternate
        {
            color: black;
            background-color: #D59200;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.normal:hover, .grid-view tr.alternate:hover
        {
            background-color: white;
            color: black;
            font-weight: bold;
        }
        
        .grid_view_lnk_button
        {
            color: Black;
            text-decoration: none;
            font-size: large;
        }
        .lbl
        {
            font-family: Book Antiqua;
            font-size: 30px;
            font-weight: bold;
            color: Green;
            text-align: center;
            font-style: italic;
        }
        .hdtxt
        {
            font-family: Book Antiqua;
            font-size: large;
            font-weight: bold;
        }
        .FixedHeader
        {
            position: absolute;
            font-weight: bold;
        }
    </style>
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
        <script>
            function leavereason(id) {
                var value1 = id.value;

                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_reason.ClientID %>");
                    idval.style.display = "block";

                }
                else {
                    var idval = document.getElementById("<%=txt_reason.ClientID %>");
                    idval.style.display = "block";
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Post Metric Scholarship</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <table class="maintablestyle">
                        <tr>
                            <td colspan="10">
                                <fieldset style="height: 60px;">
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rblMode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                    OnSelectedIndexChanged="rblMode_Selected">
                                                    <asp:ListItem Text="Entry" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Report"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true"
                                                    Width="350px">
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="2" id="tdno" runat="server" visible="false">
                                                <asp:Label ID="lblsclNo" runat="server" Text="Scholarship No"></asp:Label>
                                                <%--  </td>
                                            <td>--%>
                                                <asp:TextBox ID="txtsclNo" runat="server" Enabled="false" Style="color: Green; width: 100px;"
                                                    CssClass="txtheight3 txtcaps"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtsclNo"
                                                    FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Scholarship" runat="server" Text="Scholarship Type" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Scholarship" runat="server" CssClass="textbox  txtheight2" Style="height: 15px;
                                                    width: 135px;" ReadOnly="true" Visible="false">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel4" runat="server" Visible="false" CssClass="multxtpanel" Style="width: 200px;
                                                    height: auto;">
                                                    <asp:CheckBox ID="cb_Scholarship" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_Scholarship_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_Scholarship" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Scholarship_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_Scholarship"
                                                    PopupControlID="Panel4" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </td>
                                            <td colspan="3" id="tdReport" runat="server" visible="false">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label7" runat="server" Text="Date"></asp:Label>
                                                            <asp:TextBox ID="txtfrom" runat="server" Style="height: 20px; width: 75px;"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtfrom" runat="server"
                                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label8" runat="server" Text="To"></asp:Label>
                                                            <asp:TextBox ID="txtto" runat="server" Style="height: 20px; width: 75px;"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtto" runat="server"
                                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnReport" runat="server" CssClass="textbox btn2" Width="56px" Text="Go"
                                                                OnClick="btnReport_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10" id="tdentry" runat="server" visible="false">
                                <fieldset>
                                    <table>
                                        <tr>
                                            <td colspan="8">
                                                <%--  <td>--%>
                                                <%--     <td>--%>
                                                <%--</td>--%>
                                                <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                                    OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txt_rollno" runat="server" CssClass="txtheight3 txtcaps" OnTextChanged="txt_rollno_OnTextChanged"
                                                    AutoPostBack="true"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txt_rollno"
                                                    FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                                <%--</td>--%>
                                                <asp:Label ID="Label1" runat="server" Text="Header"></asp:Label>
                                                <%--  </td>
                                        <td>--%>
                                                <asp:DropDownList ID="ddlheader" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    OnSelectedIndexChanged="ddlheader_SelectedIndexChanged" AutoPostBack="true" Width="200px">
                                                </asp:DropDownList>
                                                <%-- </td>
                                        <td>--%>
                                                <asp:Label ID="Label2" runat="server" Text="Ledger"></asp:Label>
                                                <%--  </td>
                                        <td>--%>
                                                <asp:DropDownList ID="ddlledger" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    Width="200px">
                                                </asp:DropDownList>
                                                <%--</td>
                                        <td>--%>
                                                <asp:Label ID="Label3" runat="server" Text="Feecategory"></asp:Label>
                                                <asp:Label ID="Label6" Visible="false" runat="server"></asp:Label>
                                                <%-- </td>
                                        <td>--%>
                                                <asp:DropDownList ID="ddlfeecat" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbldate" runat="server" Text="Date"></asp:Label>
                                                <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="Amount"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtamt" runat="server" Width="80px" CssClass="txtheight3 txtcaps"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txtamt"
                                                    FilterType="Custom,Numbers" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="Reason"></asp:Label>
                                            </td>
                                            <%-- <td>
                                            <asp:DropDownList ID="ddlreason" runat="server" Width="200px" CssClass="textbox ddlstyle ddlheight3"
                                                OnSelectedIndexChanged="ddlreason_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>--%>
                                            <td>
                                                <%--<asp:DropDownList ID="ddlreason" Width="100px" CssClass="ddlheight3 textbox textbox1"
                                                runat="server" onchange="leavereason(this)" onfocus="return myFunction(this)">
                                            </asp:DropDownList>--%>
                                                <asp:TextBox ID="txt_reason" runat="server" Width="400px" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_reason"
                                                    FilterType="UppercaseLetters,lowercaseLetters,Custom" ValidChars=" *$%@!-.">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Scholarship
                                            </td>
                                            <td colspan="2">
                                                <asp:Button ID="btnplusMulSclReason" runat="server" Text="+" CssClass="textbox btn textbox1"
                                                    Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplusMulSclReason_OnClick" />
                                                <asp:DropDownList ID="ddl_MulSclReason" runat="server" CssClass="textbox ddlheight2">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnminusMulSclReason" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" CssClass="textbox btn textbox1" OnClick="btnminusMulSclReason_OnClick" />
                                            </td>
                                            <td>
                                                Paymode
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RbNeft" Checked="true" runat="server" RepeatDirection="Horizontal">
                                                </asp:RadioButton>Neft &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="LblNeftno" runat="server"
                                                    Text="Neft No"></asp:Label>
                                                <asp:TextBox ID="TxtNeftNo" runat="server" Width="150px" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_roll" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                                                    OnClick="btn_roll_Click" />
                                                <asp:Label ID="lblroll" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbldisp" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnadd" runat="server" CssClass="textbox btn2" Width="56px" Text="Save"
                                                    OnClick="btnadd_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <asp:GridView ID="gdstuddet" runat="server" AutoGenerateColumns="false" GridLines="Both"
                            CssClass="grid-view" BackColor="WhiteSmoke" Style="width: auto;" OnRowDataBound="gdstuddet_OnRowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Sno") %>'>
                                            </asp:Label>
                                            <asp:Label ID="lblappno" Visible="false" runat="server" Text='<%#Eval("appno") %>'>
                                            </asp:Label>
                                        </center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Roll No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblroll" runat="server" Text='<%#Eval("rollno") %>'>
                                            </asp:Label>
                                        </center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reg No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblreg" runat="server" Text='<%#Eval("regno") %>'>
                                            </asp:Label>
                                        </center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Admission No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbladd" runat="server" Text='<%#Eval("addno") %>'>
                                            </asp:Label>
                                        </center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblname" runat="server" Text='<%#Eval("Name") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Degree" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldeg" runat="server" Text='<%#Eval("degree") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remove" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Button ID="btnremove" runat="server" Text="Remove" OnClick="btnremove_Click" />
                                            <%--OnClick="btnremove_Click"--%>
                                        </center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="right" />
                                </asp:TemplateField>
                            </Columns>
                            <%--OnRowDataBound="gdattrpt_OnRowDataBound"--%>
                        </asp:GridView>
                    </div>
                    <div>
                        <asp:GridView ID="gdReport" runat="server" Visible="false" AutoGenerateColumns="true"
                            GridLines="Both" CssClass="grid-view" BackColor="WhiteSmoke" Style="width: auto;"
                            OnRowDataBound="gdReport_OnRowDataBound">
                        </asp:GridView>
                    </div>
                </div>
            </center>
        </div>
        <center>
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
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
                                <asp:Label ID="lbl_stream" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_strm" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_strm_OnIndexChange">
                                </asp:DropDownList>
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
                                <span class="challanLabel">
                                    <p>
                                        Section</p>
                                </span>
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
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
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
                        <tr runat="server" id="trFuParNot" visible="false">
                            <td colspan="5">
                            </td>
                            <td colspan="8" style="text-color: white; text-align: right;">
                                <asp:CheckBox ID="cbFirstGrad" runat="server" BackColor="#EE9090" Checked="true"
                                    Text="First Graduate" />
                                <asp:CheckBox ID="cbFpaid" runat="server" BackColor="#90EE90" Checked="true" Text="Fully Paid" /><asp:CheckBox
                                    ID="cbPpaid" runat="server" BackColor="#FFB6C1" Checked="true" Text="Partially Paid" />
                                <asp:CheckBox ID="cbNpaid" runat="server" BackColor="White" Checked="true" Text="Not Paid" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <asp:Label ID="lbl_errormsg" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                    <div>
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" ShowHeaderSelection="false"
                            BorderWidth="0px" Width="830px" Style="overflow: auto; height: 250px; border: 0px solid #999999;
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
            <div style="height: 1px; width: 1px; overflow: auto;">
                <div id="Div3" runat="server" style="height: 710px; width: 1344px;" visible="false">
                </div>
            </div>
        </center>
        <%------popup window1-----%>
        <%-- ***********imgdiv*******--%>
        <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 10000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="panel_description" runat="server" visible="false" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_description11" runat="server" Text="Scholarship Reason" Font-Bold="true"
                                    Font-Size="Large" ForeColor="Green"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txt_description11" runat="server" Width="200px" Style="font-family: 'Book Antiqua';
                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1 textbox1" OnClick="btndescpopadd_Click" />
                                <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1 textbox1" OnClick="btndescpopexit_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
        <%--************--%>
        <%--************--%>
        <%--  **********end of popup**********--%>
        <%--Delete Confirmation Popup --%>
        <center>
            <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                        <asp:Label ID="lbl_sure" runat="server" Text="Do You Want To Delete Selected Scholarship?"
                                            Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yes" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                width: 65px;" OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_no" CssClass=" textbox textbox1 btn1 " Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureno_Click" Text="no" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <div id="Div1" runat="server" visible="false" style="height: 100%; z-index: 100000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="panel_erroralert" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_erroralert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_erroralert" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btnerrexit_Click" Text="OK" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </body>
    </html>
</asp:Content>
