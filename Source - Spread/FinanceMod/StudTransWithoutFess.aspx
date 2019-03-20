<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudTransWithoutFess.aspx.cs" Inherits="StudTransWithoutFess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptMrgr" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: auto; height: auto">
                <div>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rbmode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                    OnSelectedIndexChanged="rbmode_Selected">
                                    <asp:ListItem Text="Trasnfer" Value="0" Selected="True"></asp:ListItem>
                                    <%--  <asp:ListItem Text="Report" Value="1"></asp:ListItem>--%>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="diventry" runat="server" visible="false">
                        <table>
                            <tr>
                                <td colspan="2">
                                    <div id="div1" style="width: 500px; float: left;">
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" Text="College"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox ddlheight5" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                                            OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_roll" runat="server" CssClass="textbox txtheight4 textbox1"
                                                            OnTextChanged="txt_roll_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_roll"
                                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="panelbackground">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:TextBox ID="txt_reg" runat="server" Style="display: none" CssClass="textbox txtheight4 textbox1"
                                                            onchange="return checkrno(this.value)" onkeyup="return checkrno(this.value)"
                                                            onblur="return get(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                                        <span style="color: Red;">*</span> <span id="rnomsg"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_name" runat="server" CssClass="textbox txtheight6 textbox1"
                                                            onblur="getname(this.value)"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftext_name" runat="server" TargetControlID="txt_name"
                                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                                            ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                            CompletionSetCount="10" ServicePath="" TargetControlID="txt_name" CompletionListCssClass="autocomplete_completionListElement"
                                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                                        </asp:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Date of Transfer
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="Updp_date" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txt_date" runat="server" CssClass="textbox txtheight2 textbox2"
                                                                    Width="100px"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </td>
                                <td>
                                    <div id="div2" style="width: auto; display: none;">
                                        <asp:Image ID="image2" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 110px;
                                            width: 100px;" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%--student detail from  div--%>
                                    <div style="width: 450px;">
                                        <fieldset style="height: 260px; width: 370px; border: 1px solid #999999;">
                                            <legend>From</legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_colg" runat="server" CssClass="txtheight5 txtcaps">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_str1" runat="server" Text="Stream"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_strm" runat="server" CssClass="txtheight3 txtcaps">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Batch
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_batch" runat="server" CssClass="txtheight txtcaps">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_degree" runat="server" CssClass="txtheight txtcaps">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_dept" runat="server" CssClass="txtheight5 txtcaps">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_sem" runat="server" CssClass="txtheight txtcaps">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Section
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_sec" runat="server" CssClass="txtheight txtcaps">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%--Seat Type--%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_seattype" Visible="false" runat="server" CssClass="txtheight txtcaps">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                </td>
                                <td>
                                    <%--change to another section--%>
                                    <div>
                                        <fieldset id="todivnotAdmit" runat="server" style="height: 260px; width: 370px; border: 1px solid #999999;">
                                            <legend>To</legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblclgs" runat="server" Text="College"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_colg" runat="server" CssClass="textbox ddlheight5" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddl_colg_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_str2" runat="server" Text="Stream"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_strm" runat="server" CssClass="textbox ddlheight4" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddl_strm_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Batch
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_batch" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbldegs" runat="server" Text="Degree"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_degree" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddl_degree_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbldepts" runat="server" Text="Department"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_dept" runat="server" CssClass="textbox ddlheight5" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddl_dept_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblsems" runat="server" Text="Semester"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_sem" runat="server" CssClass="textbox ddlheight1" Width="80px"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_sem_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Section
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_sec" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddl_sec_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%-- Seat Type--%>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_seattype" Visible="false" runat="server" CssClass="textbox ddlheight1"
                                                            Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddl_seattype_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%-- Roll No--%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_roll_no" Visible="false" runat="server" CssClass="txtheight2 txtcaps"
                                                            MaxLength="25" OnTextChanged="txt_roll_noNotApp_TextChanged" AutoPostBack="true">
                                                        </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_roll_no"
                                                            FilterType="UppercaseLetters,LowercaseLetters,Numbers">
                                                        </asp:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <br />
                                    <asp:Button ID="btntransfer" runat="server" CssClass="textbox btn btn2" Text="Transfer"
                                        OnClick="btntransfer_Click" Style="font-family: Book Antiqua; font-size: medium;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="divalert" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div8" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbalert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnalert" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnalert_Click" Text="ok" runat="server" />
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
