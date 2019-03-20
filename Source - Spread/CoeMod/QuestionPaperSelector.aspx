<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="QuestionPaperSelector.aspx.cs" Inherits="CoeMod_QuestionPaperSelector" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Question Paper Selector's Subject
                    Allotment</span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                <div>
                    <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                        margin-bottom: 10px; padding: 6px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown commonHeaderFont"
                                    Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblexammonth" runat="server" CssClass="commonHeaderFont" Text="ExamMonth">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlExamMonth" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlExamMonth_SelectedIndexChanged"
                                    AutoPostBack="True" Width="80px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblexamyear" runat="server" CssClass="commonHeaderFont" Text="ExamYear">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlExamYear" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged"
                                    AutoPostBack="True" Width="80px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Subject Search
                            </td>
                            <td>
                                <asp:TextBox ID="txt_subject" placeholder="Subject Search" runat="server" CssClass="textbox textbox1 txtheight3"
                                    TabIndex="1" onkeyup="return displayNormal(this);" onkeypress="return enterkeyvoid(event)"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="subject" runat="server" DelimiterCharacters="" Enabled="True"
                                    ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                    CompletionSetCount="10" ServicePath="" TargetControlID="txt_subject" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="txtsearchpan"
                                    OnClientPopulating="subject_OnClientPopulating">
                                </asp:AutoCompleteExtender>
                                <asp:FilteredTextBoxExtender ID="subjectExtender" runat="server" TargetControlID="txt_subject"
                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" .">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="font" OnClick="btnGo_Click"
                                    Style="width: auto; height: auto;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkSendEMail" runat="server" Text="Send EMail" Checked="false" />
                                <asp:CheckBox ID="cbsendSMS" runat="server" Text="Send SMS" Checked="false" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px; margin-bottom: 15px;
                        margin-top: 10px;"></asp:Label>
                </div>
            </div>
            <div id="Panel2" runat="server" height="18px" style="margin-left: 0px; width: 950px;">
                <asp:Label ID="lblmsgcredit" runat="server" ForeColor="Green" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
                <asp:Label ID="Label2" runat="server" ForeColor="Green" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="/" Visible="false"></asp:Label>
                <asp:Label ID="lblmsgused" runat="server" ForeColor="Green" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>

            </div>
            <br />
         
            <div id="divspread" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <FarPoint:FpSpread ID="Fpuser" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                Style="overflow: auto; border: 0px solid #999999; border-radius: 10px; background-color: White;
                                box-shadow: 0px 0px 8px #999999;" class="spreadborder" OnButtonCommand="Fpuser_OnButtonCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btn_Save" runat="server" Text="Save and Send" OnClick="btnSave_Click"
                    Visible="false" />
            </div>
        </center>
    </div>
    <div id="divAlterFreeStaffDetails" runat="server" visible="false" style="height: 160em;
        z-index: 2000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
        top: 0; left: 0px;">
        <center>
            <div id="divAlterFreeStaff" runat="server" class="table" style="background-color: White;
                height: auto; width: 85%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                top: 5%; left: 5%; right: 5%; position: fixed; border-radius: 10px;">
                <center>
                    <span style="font-family: Book Antiqua; font-size: 20px; font-weight: bold; color: Green;
                        margin: 0px; margin-bottom: 20px; margin-top: 15px; position: relative;">Available
                        Staff List</span>
                </center>
                <div>
                    <asp:Label ID="lblAlterDate" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblAlterHour" runat="server" Text="" Visible="false"></asp:Label>
                    <table>
                        <tr>
                            <asp:RadioButton ID="Internal" runat="server" Text="Internal Staff" AutoPostBack="true"
                                OnCheckedChanged="Internal_CheckedChanged" Checked="false" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" GroupName="Pass" Width="150px" />
                            <asp:RadioButton ID="External" runat="server" Text="External Staff" AutoPostBack="true"
                                OnCheckedChanged="External_CheckedChanged" Checked="false" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" GroupName="Pass" Width="150px" />
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblAlterFreeCollege" Text="College" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAlterFreeCollege" runat="server" OnSelectedIndexChanged="ddlAlterFreeCollege_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblAlterFreeDepartment" Text="Department" runat="server"></asp:Label>
                                <asp:DropDownList ID="ddlAlterFreeDepartment" Width="100px" runat="server" IndexChanged="ddlAlterFreeDepartment_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSearchBy" runat="server" Text="Staff By"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAlterFreeStaff" runat="server" Width="200px" OnSelectedIndexChanged="ddlAlterFreeStaff_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                    <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAlterFreeStaffSearch" runat="server" OnTextChanged="txtAlterFreeStaffSearch_TextChanged"
                                    Width="200px" AutoPostBack="True"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btn_Search" Width="50px" runat="server" Text="Search" CssClass="textbox btn1"
                                    OnClick="btnSearch_clickNEw" />
                            </td>
                        </tr>
                    </table>
                </div>
                <center>
                    <div id="divspreadpopup" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <FarPoint:FpSpread ID="fpstaff" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                        Style="overflow: auto; border: 0px solid #999999; border-radius: 10px; background-color: White;
                                        box-shadow: 0px 0px 8px #999999;" class="spreadborder">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td id="smscontent">
                                    SMS
                                </td>
                                <td>
                                    <textarea id="textarea_smscontent" runat="server" cols="40" rows="5"> </textarea>
                                </td>
                                <td id="emailcontent">
                                    EMAIL
                                </td>
                                <td>
                                    <textarea id="textarea_emailcontent" runat="server" cols="40" rows="5">  </textarea>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    Syllabus
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileupload" runat="server" Font-Bold="true" Enabled="true" Style="width: auto;
                                        height: auto" />
                                    <asp:LinkButton ID="lnk_Syllabus" runat="server" OnClick="lnk_Syllabus_Click" Visible="false"
                                        Style="color: Green; width: auto; height: auto"></asp:LinkButton>
                                </td>
                                <td>
                                    Model QuestionPaper
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileupload1" runat="server" Font-Bold="true" Enabled="true" Style="width: auto;
                                        height: auto" />
                                    <asp:LinkButton ID="lnk_model" runat="server" OnClick="lnk_model_Click" Visible="false"
                                        Style="color: Green; width: auto; height: auto"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:Button ID="btnSelectStaff" runat="server" Text="Ok" OnClick="btnSelectStaff_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnFreeStaffExit" runat="server" Text="Exit" OnClick="btnFreeStaffExit_Click" />
                                </td>
                            </tr>
                            <%-- <tr>
                                <td>
                                    <asp:LinkButton ID="lnk_Syllabus" runat="server" OnClick="lnk_Syllabus_Click" Visible="false" Style="color: Green;
                                        width: auto; height: auto"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnk_model" runat="server" OnClick="lnk_model_Click"  Visible="false" Style="color: Green;
                                        width: auto; height: auto"></asp:LinkButton>
                                </td>
                            </tr>--%>
                        </table>
                    </div>
                </center>
            </div>
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
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
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
