<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="journalGrid.aspx.cs" Inherits="journalGrid" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .div
        {
            left: 0%;
            top: 0%;
        }
        .watermark
        {
            color: #999999;
        }
        .table2
        {
            border: 1px solid #0CA6CA;
            border-radius: 10px;
            background-color: #0CA6CA;
            box-shadow: 0px 0px 8px #7bc1f7;
        }
        .popsty3
        {
            height: 600px;
            width: 700px;
            border: 5px solid #0CA6CA;
            border-top: 30px solid #0CA6CA;
            border-radius: 10px;
        }
    </style>
    <body onkeydown="return(event.keyCode!=13)">
        <script type="text/javascript">


            function display1() {
                document.getElementById('<%=lblerror.ClientID %>').innerHTML = "";
            }
        
    
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <asp:Label ID="Label1" runat="server" Style="color: Green; font-family: Book Antiqua;
                            font-size: x-large; font-weight: bold;" Text="Journal"></asp:Label>
                        <br />
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div style="height: auto; width: 980px;">
                <%--class="maindivstyle"--%>
                <br />
                <table class="maintablestyle" width="970px">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_college" Height="25px" runat="server" CssClass="textbox textbox1 ddlheight2"
                                OnSelectedIndexChanged="ddl_college_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_stream" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_stream" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto; margin-left: 30px;">
                                        <asp:CheckBox ID="cb_stream" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_stream_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_stream" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_stream_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_stream"
                                        PopupControlID="pbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_edulevel" runat="server" Text="Education Level" Width="110px"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_edulevel" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: auto;
                                        width: 150px;">
                                        <asp:CheckBox ID="cb_edulevel" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_edulevel_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_edulevel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_edulevel_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_edulevel"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_batch" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_batch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_batch"
                                        PopupControlID="Panel2" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_course" runat="server" Text="Course"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_course" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_course" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_course_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_course" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_course_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_course"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_dept" runat="server" Text="Department"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_dept" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_dept_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_dept"
                                        PopupControlID="Panel4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_seat" runat="server" Text="Seat Type"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_seat" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_seat" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_seat_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_seat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_seat_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_seat"
                                        PopupControlID="Panel5" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_type" runat="server" Text="Type"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_type" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_type" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_type_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_type" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_type_SelectedIndexChanged">
                                            <%-- <asp:ListItem Value="1">Regular</asp:ListItem>
                                        <asp:ListItem Value="3">Lateral</asp:ListItem>
                                        <asp:ListItem Value="2">Transfer</asp:ListItem>--%>
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_type"
                                        PopupControlID="Panel6" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_stutype" runat="server" Text="Student Type"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_stutype" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel7" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_stutype" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_stutype_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_stutype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_stutype_SelectedIndexChanged">
                                            <asp:ListItem>Day Scholar</asp:ListItem>
                                            <asp:ListItem>Hostler</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_stutype"
                                        PopupControlID="Panel7" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_header" runat="server" Text="Header"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_header" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel8" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_header" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_header_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_header" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_header_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_header"
                                        PopupControlID="Panel8" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_ledger" runat="server" Text="Ledger"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_ledger" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel9" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_ledger" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_ledger_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_ledger" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_ledger_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_ledger"
                                        PopupControlID="Panel9" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_sem" runat="server" Text="Semester"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sem" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel10" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_sem"
                                        PopupControlID="Panel10" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <tr>
                            <%--  <td>
                        <asp:Label ID="lbl_sltdet" runat="server" 
                            Text="Select Details" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel12" runat="server" Visible="false">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_sltdet" runat="server" Height="15px" CssClass="textbox  txtheight2" 
                                    ReadOnly="true"  >--Select--</asp:TextBox>
                                <asp:Panel ID="Panel11" runat="server" CssClass="multxtpanel" Height="200px" Width="100px">
                                    <asp:CheckBox ID="cb_sltdet" runat="server" 
                                        Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="cbl_sltdet" runat="server" AutoPostBack="True" >
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_sltdet"
                                    PopupControlID="Panel11" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                            --%>
                            <td>
                                <asp:Label ID="lbl_type1" Text="FeeAllot Type" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_type" runat="server" OnSelectedIndexChanged="ddl_type_SelectedIndexChanged"
                                    CssClass="textbox textbox1 ddlheight2" AutoPostBack="true">
                                    <asp:ListItem Value="0">General</asp:ListItem>
                                    <asp:ListItem Value="1">Individual(Applied)</asp:ListItem>
                                    <asp:ListItem Value="2">Individual(Admitted)</asp:ListItem>
                                    <%--  <asp:ListItem Value="3">Individual(Both)</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_detre" runat="server" Text="Deduction Reason" CssClass="textbox"></asp:Label>
                            </td>
                            <td colspan="3" style="border: 1px solid #c4c4c4; padding: 4px 4px 4px 4px; border-radius: 4px;
                                -moz-border-radius: 4px; -webkit-border-radius: 4px; box-shadow: 0px 0px 8px #d9d9d9;
                                -moz-box-shadow: 0px 0px 8px #d9d9d9; -webkit-box-shadow: 0px 0px 8px #d9d9d9;">
                                <asp:Button ID="btn_plus_detre" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                    CommandName="jai" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_plus_detre_Click" />
                                <asp:DropDownList ID="ddl_detre" CssClass="textbox ddlheight2" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:Button ID="btn_minus_detre" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_minus_detre_Click" />
                            </td>
                            <td>
                                <asp:Label ID="lblsearchddl" Text="Search" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsearch" runat="server" CssClass="textbox textbox1 ddlheight2"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Value="1">First Graduate</asp:ListItem>
                                    <asp:ListItem Value="2">Tuition Fee weiver</asp:ListItem>
                                    <asp:ListItem Value="3">Post Metric Scholarship</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Religion" runat="server" Text="Religion"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_religion" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panelreli" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: auto;">
                                            <asp:CheckBox ID="cb_religion" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_religion_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_religion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_religion_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_religion"
                                            PopupControlID="panelreli" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_Community" runat="server" Text="Community"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_community" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panelcomm" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: auto;">
                                            <asp:CheckBox ID="cb_community" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_community_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_community" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_community_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txt_community"
                                            PopupControlID="panelcomm" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2">
                                <%-- <asp:Label ID="lbl_roll" Text="Reg No" runat="server"></asp:Label>--%>
                                <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                    OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged" Height="28px">
                                </asp:DropDownList>
                                <asp:TextBox ID="txt_roll" runat="server" Height="15px" OnTextChanged="txt_roll_OnTextChanged"
                                    AutoPostBack="true" CssClass="textbox txtheight4"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_roll"
                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getroll" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>
                                <br />
                                <asp:Label ID="lblNameSrc" runat="server" Text="Name "></asp:Label>
                                <asp:TextBox ID="txt_name" runat="server" placeholder="Name" CssClass="textbox txtheight4"
                                    Height="15px" Width="201px" OnTextChanged="txt_name_Changed" AutoPostBack="true"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetName" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td colspan="2">
                                <fieldset style="box-shadow: 0px 0px 8px #d9d9d9; -moz-box-shadow: 0px 0px 8px #d9d9d9;
                                    -webkit-box-shadow: 0px 0px 8px #d9d9d9; border: 1px solid #c4c4c4;">
                                    <asp:LinkButton ID="lnkfine" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Style="margin-left: 30px;" Font-Size="Large" ForeColor="Blue" CausesValidation="False"
                                        OnClick="lnkfine_click">Fine Settings</asp:LinkButton>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="Disability"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtdisa" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnldisa" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                        height: auto;">
                                                        <asp:CheckBox ID="cbdisa" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbdisa_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cbldisa" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldisa_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender15" runat="server" TargetControlID="txtdisa"
                                                        PopupControlID="pnldisa" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td colspan="3">
                                            <asp:CheckBox ID="cb_include" runat="server" Visible="false" Text="Include Already Alloted Students" />
                                            <asp:CheckBox ID="cb_up" runat="server" Visible="false" Text="Update From CommonFees" />
                                        </td>
                                        <td colspan="3">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Scholarship
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnplusMulSclReason" runat="server" Text="+" CssClass="textbox btn textbox1"
                                                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplusMulSclReason_OnClick" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_MulSclReason" runat="server" CssClass="textbox ddlheight2">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnminusMulSclReason" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                            Font-Names="Book Antiqua" CssClass="textbox btn textbox1" OnClick="btnminusMulSclReason_OnClick" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_click" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="textbox btn1" Width="60px"
                                                            BackColor="LightGreen" OnClick="btnsave_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkview" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Large" ForeColor="Blue" CausesValidation="False" OnClick="view_click">View Details</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                </table>
                <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
                <div>
                    <br />
                    <center>
                        <asp:Panel ID="pheaderfilter" runat="server" CssClass="table2" Height="22px" Width="850px"
                            Style="margin-top: -0.1%;">
                            <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                            <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                            <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageAlign="Right" />
                        </asp:Panel>
                    </center>
                    <br />
                </div>
                <center>
                    <asp:Panel ID="pcolumnorder" runat="server" CssClass="table2" Width="850px">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                        Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                    <asp:TextBox ID="tborder" Visible="false" Width="837px" TextMode="MultiLine" CssClass="style1"
                                        AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                        Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                        RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                        <asp:ListItem Enabled="false">Mode</asp:ListItem>
                                        <asp:ListItem Enabled="false" Value="FeeAmount">Fee Amount</asp:ListItem>
                                        <asp:ListItem Value="DeductAmout">Deduction</asp:ListItem>
                                        <asp:ListItem Value="DeductReason">Deduction Reason</asp:ListItem>
                                        <asp:ListItem Enabled="false" Value="TotalAmount">Total</asp:ListItem>
                                        <asp:ListItem Value="RefundAmount">Refund</asp:ListItem>
                                        <asp:ListItem Value="DueDate">Pay Start Date</asp:ListItem>
                                        <%--<asp:ListItem Value="FineAmount">Fine</asp:ListItem>
                                    <asp:ListItem Value="PayStartDate">Due Date</asp:ListItem>--%>
                                        <asp:ListItem>Scholarship</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
                <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                    CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                    TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                    ExpandedImage="~/images/down.jpeg">
                </asp:CollapsiblePanelExtender>
                <center>
                    <asp:Label Style="color: Red;" ID="lblerr" Visible="false" Text="Record Not Found"
                        runat="server"></asp:Label>
                </center>
                <br />
                <%-- <center>--%>
                <div id="divGridI" runat="server" style="width: 980px;">
                    <div id="divGridII" runat="server" style="width: 960px; height: 500px; overflow: auto;">
                        <asp:GridView ID="gridLedgeDetails" runat="server" OnRowDataBound="OnRowDataBound"
                            OnDataBound="gridLedgeDetails_DataBound">
                        </asp:GridView>
                    </div>
                </div>
                <%-- </center>--%>
                <center>
                    <%-- <asp:UpdatePanel ID="upd1" runat="server">
                    <ContentTemplate>--%>
                    <asp:Panel ID="pnlupdate" runat="server" Visible="false" Style="top: 376px; border-color: Black;
                        background-color: lightyellow; border-style: solid; border-width: 0.5px; left: 446px;
                        position: absolute; width: 360px; height: 443px;">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            <asp:Label ID="lblmonthwise" runat="server" Visible="true" Text="Monthwise Allotment"
                                Font-Bold="true" Font-Size="Large" Font-Names="Book Antiqua" Style="margin-left: 6px;"></asp:Label>
                        </caption>
                        <asp:Panel ID="Panel20" runat="server" Style="top: 48px; border-color: Black; background-color: lightyellow;
                            border-style: solid; border-width: 0.5px; left: 4px; position: absolute; width: 330px;
                            height: 334px;">
                            <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="0.5" autopostback="true" ClientAutoCalculation="true" ShowHeaderSelection="false"
                                OnUpdateCommand="FpSpread3_Command">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true" GridLineColor="Black">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </asp:Panel>
                        <asp:Button ID="btnexi" runat="server" Text="Exit" OnClick="btnexi_Click" Style="top: 411px;
                            left: 155px; position: absolute; height: 27px; width: 88px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </asp:Panel>
                    <%--     </ContentTemplate>
                </asp:UpdatePanel>--%>
                </center>
                <%--Popup Scholarships - Multiple--%>
                <%--  <div id="divMulSchlolar" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">--%>
                <center>
                    <%--  <asp:UpdatePanel ID="upMulSchlolar" runat="server">
                    <ContentTemplate>--%>
                    <asp:Panel ID="divMulSchlolar" runat="server" Visible="false" Style="top: 376px;
                        border-color: Black; background-color: lightyellow; border-style: solid; border-width: 0.5px;
                        left: 446px; position: absolute; width: 300px; height: 443px;">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            <asp:Label ID="Label2" runat="server" Visible="true" Text="Multiple Scholarship"
                                Font-Bold="true" Font-Size="Large" Font-Names="Book Antiqua" Style="margin-left: 6px;"></asp:Label>
                        </caption>
                        <asp:Panel ID="Panel12" runat="server" Style="top: 48px; border-color: Black; background-color: lightyellow;
                            border-style: solid; border-width: 0.5px; left: 4px; position: absolute; width: 280px;
                            height: 334px;">
                            <FarPoint:FpSpread ID="FpSchloar" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="0.5" autopostback="true" ClientAutoCalculation="true" ShowHeaderSelection="false"
                                OnUpdateCommand="FpSchloar_Command">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true" GridLineColor="Black">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </asp:Panel>
                        <asp:Button ID="btnExitScholar" runat="server" Text="Exit" OnClick="btnExitScholar_Click"
                            Style="top: 411px; left: 120px; position: absolute; height: 27px; width: 88px"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </asp:Panel>
                    <%--   </ContentTemplate>
                </asp:UpdatePanel>--%>
                </center>
                <%--</div>--%>
            </div>
            <center>
                <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                    <center>
                        <div id="panel_addreason" runat="server" visible="false" class="table" style="background-color: White;
                            height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <table style="line-height: 30px">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_addreason" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_addreason" runat="server" Width="200px" CssClass="textbox textbox1"
                                            onkeypress="display1()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="line-height: 35px">
                                        <asp:Button ID="btn_addreason" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1 btn2" OnClick="btn_addreason_Click" />
                                        <asp:Button ID="btn_exitreason" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1 btn2" OnClick="btn_exitaddreason_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100em; z-index: 1000;
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
                <div id="imgDiv1" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div4" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblconfirm" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnyes" CssClass=" textbox textbox1 btn2 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnyes_Click" Text="Yes" runat="server" />
                                                <asp:Button ID="btnno" CssClass=" textbox textbox1 btn2 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnno_Click" Text="No" runat="server" />
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
                <div id="popfine" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                        width: 30px; position: absolute; margin-top: 12px; margin-left: 376px;" OnClick="imagepopclose_click" />
                    <br />
                    <center>
                        <div style="height: 700px; width: 800px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                            border-radius: 10px; background-color: White;">
                            <fieldset style="height: 680px;">
                                <legend class="fontstyleheader" style="color: Green;">Fine Settings</legend>
                                <br />
                                <table cellpadding="10">
                                    <tr>
                                        <td colspan="4">
                                            <asp:RadioButton ID="rbfine" runat="server" Checked="true" Text="Fine" GroupName="fn"
                                                AutoPostBack="true" OnCheckedChanged="rbfine_Changed" />
                                            <asp:RadioButton ID="rbreadd" runat="server" Text="Re-Admission" GroupName="fn" AutoPostBack="true"
                                                OnCheckedChanged="rbreadd_Changed" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblheadfine" runat="server" Text="Header" />
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updheader" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtheadfine" runat="server" Height="15px" CssClass="textbox txtheight2"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlfinehead" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                        height: auto;">
                                                        <asp:CheckBox ID="cbheadfine" runat="server" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="cbheadfine_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblheadfine" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblheadfine_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="txtheadfine"
                                                        PopupControlID="pnlfinehead" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_ledgerfine" runat="server" Text="Ledger" />
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updledgefine" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtledgerfine" runat="server" Height="15px" CssClass="textbox txtheight2"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlledge" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                        height: auto;">
                                                        <asp:CheckBox ID="cbledgefine" runat="server" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="cbledgefine_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblledgefine" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblledgefine_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender14" runat="server" TargetControlID="txtledgerfine"
                                                        PopupControlID="pnlledge" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="10" id="tblfine" runat="server" visible="false">
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <table cellpadding="10">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rb_common" runat="server" Text="Common" GroupName="fine" AutoPostBack="true"
                                                                OnCheckedChanged="rb_common_OnCheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_fine" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rb_perday" runat="server" Text="Per Day" GroupName="fine" AutoPostBack="true"
                                                                OnCheckedChanged="rb_perday_OnCheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rb_perweek" runat="server" Text="Per Week" GroupName="fine"
                                                                AutoPostBack="true" OnCheckedChanged="rb_perweek_OnCheckedChanged" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                        <td colspan="2">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbldue" runat="server" Text="Due Date"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txt_due" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                        <asp:CalendarExtender ID="caldueext" runat="server" TargetControlID="txt_due" CssClass="cal_Theme1 ajax__calendar_active"
                                                            Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblreadd" runat="server" visible="false">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Amount"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtreeaddAmt" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Due Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtreeadddt" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtreeadddt"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                                <center>
                                    <table id="tblperweek" runat="server" visible="false">
                                        <tr>
                                            <td>
                                                <center>
                                                    <br />
                                                    <asp:Label ID="lblsprerr" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                    <FarPoint:FpSpread ID="Fpspreadfine" runat="server" Visible="false" Style="overflow: auto;"
                                                        CssClass="spreadborder" ShowHeaderSelection="false" ActiveSheetViewIndex="0"
                                                        OnUpdateCommand="Fpspreadfine_Command" OnPreRender="Fpspreadfine_render">
                                                        <Sheets>
                                                            <FarPoint:SheetView SheetName="Sheet1">
                                                            </FarPoint:SheetView>
                                                        </Sheets>
                                                    </FarPoint:FpSpread>
                                                </center>
                                            </td>
                                            <td>
                                                <fieldset id="field" runat="server" style="box-shadow: 0px 0px 8px #d9d9d9; -moz-box-shadow: 0px 0px 8px #d9d9d9;
                                                    -webkit-box-shadow: 0px 0px 8px #d9d9d9; border: 1px solid #c4c4c4;">
                                                    <div id="btnaddrowdiv" runat="server" visible="true">
                                                        <asp:Button ID="btnaddrow" runat="server" Text="Add Row" OnClick="btnaddrow_click"
                                                            CssClass="textbox textbox1 btn2" />
                                                    </div>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <br />
                                <center>
                                    <div>
                                        <asp:Button ID="btnsavefine" runat="server" Text="Save" OnClick="btnsavefine_click"
                                            CssClass="textbox textbox1 btn2" />
                                        <asp:Button ID="btnexitfine" runat="server" Text="Exit" OnClick="btnexitfine_click"
                                            CssClass="textbox textbox1 btn2" />
                                        <asp:Button ID="btnWeekFindDel" runat="server" Text="Delete" Visible="false" OnClick="btnWeekFindDel_click"
                                            CssClass="textbox textbox1 btn2" />
                                    </div>
                                </center>
                            </fieldset>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="alertfine" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="divfine" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblfine" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnfineclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnfineclose_click" Text="Ok" runat="server" />
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
                <div id="divview" runat="server" visible="false" style="height: 120em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="div1" runat="server" class="table" style="background-color: White; height: 500px;
                            width: 840px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 30px;
                            border-radius: 10px;">
                            <asp:ImageButton ID="imgbtn3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: -32px; margin-left: 400px;"
                                OnClick="imagebtnorder_Click" />
                            <center>
                                <center>
                                    <span class="fontstyleheader" style="color: Green;">Student Details</span>
                                </center>
                                <center>
                                    <div id="studentdetail" runat="server" visible="false" style="width: 820px; height: 620px;">
                                        <center>
                                            <center>
                                                <asp:Label ID="lblerr1" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                                <FarPoint:FpSpread ID="FpSpreadstud" runat="server" Visible="false" Style="overflow: auto;
                                                    margin-top: 10px;" CssClass="spreadborder" ShowHeaderSelection="false" ActiveSheetViewIndex="0"
                                                    OnUpdateCommand="FpSpreadstud_Command">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                                <FarPoint:FpSpread ID="FpSpreadstud2" runat="server" Visible="false" Style="overflow: auto;"
                                                    CssClass="spreadborder" ShowHeaderSelection="false" ActiveSheetViewIndex="0"
                                                    OnCellClick="FpSpreadstud2_CellClick" OnPreRender="FpSpreadstud2_SelectedIndexChanged">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                                <FarPoint:FpSpread ID="FpSpreadstud3" runat="server" Visible="false" Style="overflow: auto;"
                                                    CssClass="spreadborder" ShowHeaderSelection="false" ActiveSheetViewIndex="0"
                                                    OnCellClick="FpSpreadstud3_CellClick" OnPreRender="FpSpreadstud3_SelectedIndexChanged">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                            </center>
                                            <br />
                                            <div id="Div3" runat="server" visible="true">
                                                <asp:Button ID="btnok1" runat="server" OnClick="btnok1_Click" CssClass="textbox textbox1 btn2"
                                                    Text="Ok" />
                                                <asp:Button ID="btnexitstud" runat="server" OnClick="btnexitstud_Click" CssClass="textbox textbox1 btn2"
                                                    Text="Exit" />
                                            </div>
                                        </center>
                                    </div>
                                </center>
                            </center>
                        </div>
                    </center>
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
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 100000;
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
        </center>
    </body>
    </html>
</asp:Content>
