<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Library_Master.aspx.cs" Inherits="LibraryMod_Library_Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <link href="~/Styles/css/Style.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <asp:Label ID="lblMainError" runat="server" CssClass="font" ForeColor="Red"></asp:Label>
    <asp:Label ID="lblerrordate" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
        Font-Size="5pt" Visible="false"></asp:Label>
    <br />
    <center>
        <span class="fontstyleheader" style="color: Green; font-weight: bold; margin: 0px;
            margin-bottom: 15px; margin-top: 10px;">Library Master </span>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table class="maintablestyle" style="width: 718px; height: auto; font-family: Book Antiqua;
                    font-weight: bold; padding: 6px; margin: 0px; margin-bottom: 15px; margin-top: 10px;">
                    <tr>
                        <td>
                            <asp:Label ID="Label16" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCollege1" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege1_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Library"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="Txtlibrary" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                Width="160px" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="Panel1" runat="server" Width="280px" CssClass="multxtpanel multxtpanleheight">
                                <asp:CheckBox ID="cblibrary" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnCheckedChanged="cblibrary_CheckedChanged" Text="Select All"
                                    AutoPostBack="True" />
                                <asp:CheckBoxList ID="cbllibrary" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    OnSelectedIndexChanged="cbllibrary_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="Txtlibrary"
                                PopupControlID="Panel1" Position="Bottom">
                            </asp:PopupControlExtender>
                        </td>
                        <asp:UpdatePanel ID="UpGoAdd" runat="server">
                            <ContentTemplate>
                                <td>
                                    <asp:ImageButton ID="btnMainGo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btnMainGo_Click" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/LibImages/Add.jpg" OnClick="btnAdd_Click" />
                                </td>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="lblerrmainapp" runat="server" Text="" Visible="false" ForeColor="Red"
            CssClass="font"></asp:Label>
    </center>
    <br />
    <center>
        <asp:UpdatePanel ID="upGrid" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                <asp:GridView ID="GrdLibMaster" runat="server" ShowFooter="false" Width="900px" AutoGenerateColumns="true"
                    ShowHeader="false" Font-Names="Book Antiqua" toGenerateColumns="true" OnRowDataBound="GrdLibMaster_OnRowDataBound"
                    OnRowCreated="GrdLibMaster_OnRowCreated" OnSelectedIndexChanged="GrdLibMaster_onselectedindexchanged">
                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <div style="height: 20px;">
    </div>
    <div style="text-align: center;">
        <asp:Label ID="lbprint" runat="server" Visible="false" Style="color: Red;"></asp:Label>
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" onkeypress="return keyvalue(this)"
            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium"></asp:TextBox>
        <asp:ImageButton ID="btn_excel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
            OnClick="btn_excel_Click" />
        <asp:ImageButton ID="btnprintmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg"
            OnClick="btnprintmaster_Click" />
        <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
    </div>
    <center>
        <asp:UpdatePanel ID="updatepanel54" runat="server">
            <ContentTemplate>
                <div id="divTarvellerEntryDetails" runat="server" visible="false" style="height: 70em;
                    z-index: 100; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0%; left: 0px;">
                    <center>
                        <%--left: 15%; right: 15%; position: absolute;--%>
                        <div id="divTarvellerEntry" runat="server" class="table" style="background-color: White;
                            border: 5px solid #0CA6CA; border-top: 20px solid #0CA6CA; margin-left: auto;
                            margin-right: auto; width: 990px; height: 987px; z-index: 1000; border-radius: 5px;
                            font-family: Book Antiqua; font-weight: bold;">
                            <asp:Label ID="lbl_clgT" runat="server" Visible="false"></asp:Label>
                            <center>
                                <span style="top: 20px; bottom: 20px; text-align: center; color: Green; font-size: large;
                                    position: relative; font-weight: bold;">Library Information </span>
                            </center>
                            <center>
                                <asp:Panel ID="Panel5" runat="server" Style="border-color: Gray; border-style: solid;
                                    font-family: Book Antiqua; font-weight: bold; width: 978px; height: 929px; margin-top: 20px;
                                    margin-bottom: 0px; margin-right: 58px; margin-left: 3px;">
                                    <center>
                                        <table class="tabl" style="top: 200px; position: absolute; border-color: Gray; border-width: thin;
                                            margin-left: 10px; margin-top: -80px; height: 150px;">
                                            <tr>
                                                <td>
                                                    <span style="color: Red;">*</span>
                                                    <asp:Label ID="libname" runat="server" Text="Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtname" runat="server" CssClass="textbox txtheight2" Style="margin-right: 29px;
                                                                width: 200px;"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color: Red;">*</span>
                                                    <asp:Label ID="Label2" runat="server" Text="Librarian"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpBtnLibr" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="Txtlibrarian" runat="server" CssClass="textbox txtheight2" Style="margin-right: 29px;
                                                                width: 200px;"></asp:TextBox>
                                                            <asp:Button ID="Btnlibrarian" runat="server" OnClick="Btnlibrarian_Click" Text="?"
                                                                Height="27px" Width="28px" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color: Red;">*</span>
                                                    <asp:Label ID="Label3" runat="server" Text="Location"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpBtnLoc" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="Txtlocation" runat="server" CssClass="textbox txtheight2" Style="margin-right: 29px;
                                                                width: 200px;"></asp:TextBox>
                                                            <asp:Button ID="btnlocation" runat="server" OnClick="Btnlocation_Click" Text="?"
                                                                Height="27px" Width="28px" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color: Red;">*</span>
                                                    <asp:Label ID="Label4" runat="server" Text="Fine Collection"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rblibrary" Text="Library" runat="server" AutoPostBack="True"
                                                        GroupName="ApplyType" Checked="true" />
                                                    <asp:RadioButton ID="rboffice" Text="Office" runat="server" GroupName="ApplyType"
                                                        AutoPostBack="True" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label5" runat="server" Text="Automatic Access No Generation"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Panel ID="Panel6" runat="server" Style="border-color: Gray; border-style: solid;
                                                        border-width: thin; width: 675px; height: 411px;">
                                                        <br />
                                                        <fieldset style="width: 647px; height: auto;">
                                                            <table cellspacing="7">
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="cb_books" runat="server" Text="Books" AutoPostBack="true" OnCheckedChanged="cb_books_CheckedChanged"
                                                                            Style="margin-left: -20px;" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbgeneral" Text="General" runat="server" AutoPostBack="True"
                                                                            Style="margin-left: -3px;" OnCheckedChanged="rbgeneral_CheckedChanged" GroupName="BookType"
                                                                            Checked="true" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblacronym1" runat="server" Text="Acronym" Visible="false" Style="margin-left: 10px;"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtacronym1" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                            Width="90px"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblstartnum1" runat="server" Visible="false" Text="Start Number"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtstartnum1" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                            Width="90px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbcategorywist" Text="Category Wise" runat="server" AutoPostBack="True"
                                                                            OnCheckedChanged="rbcategorywist_CheckedChanged" GroupName="BookType" Checked="true"
                                                                            Style="margin-left: -3px;" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblacronym2" runat="server" Visible="false" Text="Acronym" Style="margin-left: 10px;"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtacronym2" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                            Width="90px"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblstartnum2" runat="server" Visible="false" Text="Start Number"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtstartnum2" Visible="false" runat="server" CssClass="textbox txtheight2"
                                                                            Width="90px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                        <table cellspacing="15">
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cbbackvolume" runat="server" Text="Backvolume" AutoPostBack="true"
                                                                        OnCheckedChanged="cbbackvolume_CheckedChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblacronym3" runat="server" Text="Acronym" Visible="false"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtacronym3" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                        Width="90px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblstartnum3" runat="server" Visible="false" Text="Start Number"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtstartnum3" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                        Width="90px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cbperiodicalsmaster" runat="server" Text="Periodicals Master" AutoPostBack="true"
                                                                        OnCheckedChanged="cbperiodicalsmaster_CheckedChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblacronym4" runat="server" Text="Acronym" Visible="false"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtacronym4" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                        Width="90px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblstartnum4" runat="server" Visible="false" Text="Start Number"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtstartnum4" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                        Width="90px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cbnonbook" runat="server" Text="Nonbook Materials" AutoPostBack="true"
                                                                        OnCheckedChanged="cbnonbook_CheckedChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblacronym5" runat="server" Text="Acronym" Visible="false"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtacronym5" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                        Width="90px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblstartnum5" runat="server" Visible="false" Text="Start Number"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtstartnum5" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                        Width="90px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cbstandardmaster" runat="server" Text="Standard Master" AutoPostBack="true"
                                                                        OnCheckedChanged="cbstandardmaster_CheckedChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblacronym6" runat="server" Text="Acronym" Visible="false"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtacronym6" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                        Width="90px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblstartnum6" runat="server" Visible="false" Text="Start Number"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtstartnum6" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                        Width="90px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cbproceedings" runat="server" Text="Proceedings Master" AutoPostBack="true"
                                                                        OnCheckedChanged="cbproceedings_CheckedChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblacronym7" runat="server" Text="Acronym" Visible="false"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtacronym7" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                        Width="90px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblstartnum7" runat="server" Visible="false" Text="Start Number"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtstartnum7" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                        Width="90px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cbprojectmaster" runat="server" Text="Project Master" AutoPostBack="true"
                                                                        OnCheckedChanged="cbprojectmaster_CheckedChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblacronym8" runat="server" Text="Acronym" Visible="false"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtacronym8" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                        Width="90px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblstartnum8" runat="server" Visible="false" Text="Start Number"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtstartnum8" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                                        Width="90px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblcategoryofinward" runat="server" Text="Category of Inward Material">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="Btnattplus" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                        Height="23px" Style="height: 23px; display: none; left: 507px; position: absolute;
                                                        top: 169px; width: 27px;" Text="+" />
                                                    <asp:DropDownList ID="ddlcategoryofinward" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlcategoryofinward_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="Btnacch" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                        Height="22px" Style="height: 23px; display: none; left: 640px; position: absolute;
                                                        top: 169px; width: 27px;" Text="-" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblcategoryofbook" runat="server" Text="Category of Book Issue">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel30" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlcategoryofbook" runat="server" CssClass="textbox ddlstyle ddlheight3">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cbbookbank" runat="server" Text="Book Bank" AutoPostBack="true" />
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:CheckBox ID="cballstudents" runat="server" Text="All Students" AutoPostBack="true" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cbfixedduedate" runat="server" Text="Fixed Due Date" AutoPostBack="true"
                                                        OnCheckedChanged="cbfixedduedate_CheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtfixedduedate" Enabled="false" runat="server" CssClass="textbox txtheight2"
                                                        onchange="return checkDate()"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtfixedduedate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cbfinecalculation" runat="server" Text="Fine Calculation" AutoPostBack="true"
                                                        OnCheckedChanged="cbfinecalculation_CheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtfinecalculationfrom" Enabled="false" runat="server" CssClass="textbox txtheight2"
                                                        onchange="return checkDate()"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfinecalculationfrom"
                                                        runat="server" Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                    <asp:TextBox ID="txtfinecalculationto" Enabled="false" runat="server" CssClass="textbox txtheight2"
                                                        onchange="return checkDate()"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtfinecalculationto"
                                                        runat="server" Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cbbookissueclosedatestudents" runat="server" Text="Book Issue Close Date-Students"
                                                        AutoPostBack="true" OnCheckedChanged="cbbookissueclosedatestudents_CheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtbookissueclosedatestudents" Enabled="false" runat="server" CssClass="textbox txtheight2"
                                                        onchange="return checkDate()"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txtbookissueclosedatestudents"
                                                        runat="server" Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cbbookissueclosedatestaff" runat="server" Text="Book Issue Close Date-Staff"
                                                        AutoPostBack="true" OnCheckedChanged="cbbookissueclosedatestaff_CheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtbookissueclosedatestaff" Enabled="false" runat="server" CssClass="textbox txtheight2"
                                                        onchange="return checkDate()"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txtbookissueclosedatestaff"
                                                        runat="server" Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cballowallcollegestudandstaff" runat="server" Text="Allow all college Students & Staffs"
                                                        AutoPostBack="true" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                    <center>
                                        <table class="tablfont" style="top: 500px; margin-left: 465px; margin-top: 280px;
                                            position: absolute; width: 179px; height: 28px; border-color: Gray; margin-top: 457px;">
                                            <tr>
                                                <asp:UpdatePanel ID="UpSaveUpdate" runat="server">
                                                    <ContentTemplate>
                                                        <td>
                                                            <asp:ImageButton ID="btnnew" runat="server" ImageUrl="~/LibImages/Add new (2).jpg"
                                                                OnClick="btnnew_Click" Enabled="true" />
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="Btnsave" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="Btnsave_Click"
                                                                Enabled="false" />
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="Btnupdate" runat="server" ImageUrl="~/LibImages/update.jpg"
                                                                OnClick="Btnupdate_Click" Enabled="false" />
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btndelete" runat="server" ImageUrl="~/LibImages/delete.jpg"
                                                                OnClick="btndelete_Click" Enabled="true" />
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btn_exit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                                OnClick="btn_exit_Click" />
                                                        </td>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </tr>
                                        </table>
                                        <center>
                                            <asp:UpdatePanel ID="UpdatePanel50" runat="server">
                                                <ContentTemplate>
                                                    <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                                                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                                        left: 0px;">
                                                        <center>
                                                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                                                width: 338px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
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
                                                                                    <asp:ImageButton ID="btn_alertclose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                                                        OnClick="btn_alertclose_Click" />
                                                                                    <asp:ImageButton ID="btn_yes" runat="server" ImageUrl="~/LibImages/yes.jpg" OnClick="btn_yes_Click"
                                                                                        Visible="false" />
                                                                                    <asp:ImageButton ID="btn_No" runat="server" ImageUrl="~/LibImages/no (2).jpg" OnClick="btn_No_Click"
                                                                                        Visible="false" />
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
                                    </center>
                                </asp:Panel>
                                <asp:UpdatePanel ID="UpdatePanel53" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="panel3" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                                            BorderWidth="2px" Style="left: 30%; top: 10%; right: 30%; position: absolute;
                                            z-index: 3;" Height="690px" Width="700px">
                                            <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                                                font-size: Small; font-weight: bold">
                                                <br />
                                                <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                                                    left: 200px">
                                                    <asp:Label ID="Lbltittle" runat="server" Text="Select Staff Incharge"></asp:Label>
                                                </caption>
                                                <br />
                                                <br />
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblcollege" runat="server" Text="College"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlcollege" runat="server" Width="150px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                                                AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddldepratstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged"
                                                                AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblsearchby" runat="server" Text="Staff By"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                                                AutoPostBack="true">
                                                                <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                                                <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                                                AutoPostBack="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <div>
                                                    <asp:GridView ID="GrdStaff" runat="server" ShowFooter="false" Style="font-family: Book Antiqua;
                                                        width: 650px; border-collapse: collapse; margin-left: 25px;" AutoGenerateColumns="true"
                                                        ShowHeaderWhenEmpty="true" toGenerateColumns="true" AllowPaging="true" PageSize="20"
                                                        OnSelectedIndexChanged="GrdStaff_OnSelectedIndexChanged" OnPageIndexChanging="GrdStaff_OnPageIndexChanged">
                                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:GridView ID="GrdLocation" runat="server" ShowFooter="false" Style="font-family: Book Antiqua;
                                                        width: 650px; border-collapse: collapse; margin-left: 25px;" AutoGenerateColumns="true"
                                                        ShowHeaderWhenEmpty="true" toGenerateColumns="true" AllowPaging="true" PageSize="20"
                                                        OnPageIndexChanging="GrdLocation_OnPageIndexChanged">
                                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <fieldset style="position: absolute; left: 345px; visibility: visible; width: 140px;
                                                        height: 2px;">
                                                        <asp:ImageButton ID="btnstaffadd" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btnstaffadd_Click"
                                                            Style="width: 75px; top: 2px; position: absolute; left: 2px;" />
                                                        <asp:ImageButton ID="btnexit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                            OnClick="btnexit_Click" Style="width: 75px; top: 2px; position: absolute; left: 85px;" />
                                                    </fieldset>
                                                </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for Go And Add--%>
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
    <%--progressBar for Common for save,update--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpSaveUpdate">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for Common for UpBtnLibr--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpBtnLibr">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for Common for UpBtnLoc--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpBtnLoc">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress5"
            PopupControlID="UpdateProgress5">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
