<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="LibraryIdcardgeneration.aspx.cs" Inherits="LibraryMod_LibraryIdcardgeneration" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
                <span class="fontstyleheader" style="color: Green;">Library ID Generation</span></div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
            <ContentTemplate>
                <center>
                    <div>
                        <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 5px;
                            margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rbllib" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                        OnSelectedIndexChanged="rbllib_Selected">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
            <ContentTemplate>
                <center>
                    <div id="studentpop" runat="server" visible="false" style="z-index: 1000; width: 100%;
                        position: absolute; top: 6; left: 0px;">
                        <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 5px;
                            margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblbatch" runat="server" Text="Batch" CssClass="commonHeaderFont">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlBatch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="75px" AutoPostBack="true" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldegree" runat="server" Text="Degree" CssClass="commonHeaderFont">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddldegree" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="75px" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblbranch" runat="server" Text="Branch" CssClass="commonHeaderFont">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlbranch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblsem" runat="server" Text="Semester" CssClass="commonHeaderFont">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlsem" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="60px" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblsection" runat="server" Text="Section" CssClass="commonHeaderFont">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlsection" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="75px" AutoPostBack="true" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <fieldset style="border-radius: 10px; width: 410px; height: -1px;">
                                        <legend>Search Condition</legend>
                                        <div>
                                            <asp:Label ID="lblsearchby" runat="server" Text="Search By"></asp:Label>
                                            <asp:DropDownList ID="ddlrollno" runat="server" AutoPostBack="True" CssClass="textbox1 ddlheight1"
                                                OnSelectedIndexChanged="ddlrollno_SelectedIndexChanged">
                                                <asp:ListItem Value="0"> All</asp:ListItem>
                                                <asp:ListItem Value="1">Roll No</asp:ListItem>
                                                <asp:ListItem Value="2">Student Name</asp:ListItem>
                                                <asp:ListItem Value="3">Adm No</asp:ListItem>
                                                <asp:ListItem Value="4">Gender</asp:ListItem>
                                                <asp:ListItem Value="5">Lib ID</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblsearchcontent" runat="server" Text="Search Content"></asp:Label>
                                            <asp:TextBox ID="txtsearchcontent" runat="server" Enabled="false" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Height="20px" Width="86px" Style="" OnTextChanged="txtsearchcontent_TextChanged"
                                                AutoPostBack="True"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getsearch" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearchcontent"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:DropDownList ID="ddlsearchcontnet" runat="server" Visible="false" CssClass="dropdown commonHeaderFont"
                                                Width="116px" AutoPostBack="true">
                                                <asp:ListItem> Male</asp:ListItem>
                                                <asp:ListItem> Female</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </fieldset>
                                </td>
                                <td colspan="2">
                                    <fieldset style="border-radius: 10px;">
                                        <legend>Student Mode</legend>
                                        <div>
                                            <asp:RadioButtonList ID="rblstudent" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnSelectedIndexChanged="rblstudent_Selected">
                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="1">Transfer</asp:ListItem>
                                                <asp:ListItem Value="2">Regular</asp:ListItem>
                                                <asp:ListItem Value="3">Lateral</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </fieldset>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpGo" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="studentgo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="studentgoClick" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel16" runat="server">
            <ContentTemplate>
                <div id="divtable" runat="server" visible="false" style="width: 900px; overflow: auto;
                    background-color: White; border-radius: 10px; margin-top: 190px;">
                    <asp:GridView ID="grdStudent" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                        ShowHeader="true" Font-Names="book antiqua" togeneratecolumns="true" OnSelectedIndexChanged="grdStudent_onselectedindexchanged"
                        Width="840px">
                        <HeaderStyle BackColor="#0ca6ca" ForeColor="Black" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel17" runat="server">
            <ContentTemplate>
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
                                            <asp:UpdatePanel ID="UpdatePanelbtn2" runat="server">
                                                <ContentTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="btnerrclose" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btnerrclose_Click" />
                                                    </center>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
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
    <center>
        <asp:UpdatePanel ID="UpdatePanel18" runat="server">
            <ContentTemplate>
                <div id="div2" runat="server" visible="false">
                    <table style="font-family: Book Antiqua; font-weight: bold">
                        <tr>
                            <td>
                                <asp:Label ID="lblacr" runat="server" Text="Acronym:"></asp:Label>
                                <asp:CheckBox ID="cbacr" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbacr_OnCheckedChanged" />
                                <asp:TextBox ID="txtacr" runat="server" Font-Names="Book Antiqua" Enabled="false"
                                    Font-Size="Medium" Height="20px" Width="86px" AutoPostBack="True"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox1" runat="server" MaxLength="15" Font-Names="Book Antiqua"
                                    Enabled="true" Font-Size="Medium" Height="20px" Width="86px" Style="" OnTextChanged="txtacr_TextChanged"
                                    AutoPostBack="True"></asp:TextBox><asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                        runat="server" TargetControlID="TextBox1" FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                    </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Button ID="btndefault" Text="Default" Enabled="false" Width="63px" CssClass=" textbox btn1"
                                    runat="server" OnClientClick="return valid2()" OnClick="btndefaultClick" />
                            </td>
                            <td>
                                <asp:Label ID="lblstart" runat="server" Text="Serial Start With:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtstart" runat="server" MaxLength="3" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="20px" Width="86px" OnTextChanged="txtstart_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtstart"
                                    FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblsize" runat="server" Text="Serial Size:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtsize" runat="server" MaxLength="1" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="20px" Width="86px" OnTextChanged="txtsize_TextChanged"
                                    AutoPostBack="True"></asp:TextBox><asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                        runat="server" TargetControlID="txtsize" FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpGeneration" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btngen" Text="Generate" Width="63px" CssClass=" textbox btn1" runat="server"
                                            OnClientClick="return valid2()" OnClick="btngenClick" />
                                        <asp:Button ID="btmnogen" Text="Re-Generate" Width="82px" CssClass=" textbox btn1"
                                            runat="server" OnClientClick="return valid2()" OnClick="btmnogenClick" />
                                        <asp:ImageButton ID="btnsave" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="btnsaveClick" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblnoofstud" runat="server" Text="No of Students"></asp:Label>
                                <asp:TextBox ID="txtnoofstud" runat="server" Enabled="false" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="20px" Width="86px" OnTextChanged="txtnoofstud_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
            <ContentTemplate>
                <div id="print2" runat="server" visible="false">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblvalidation3" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                            <asp:Label ID="lblrptname2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname2" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:ImageButton ID="btnExcel2" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                                OnClick="btnExcel_Click2" />
                            <asp:ImageButton ID="btnprintmasterhed2" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                OnClick="btnprintmaster_Click2" />
                            <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExcel2" />
                            <asp:PostBackTrigger ControlID="btnprintmasterhed2" />
                            <asp:PostBackTrigger ControlID="Printcontrolhed2" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
        <ContentTemplate>
            <center>
                <div id="staffpop" runat="server" visible="false" style="z-index: 1000; width: 100%;
                    position: absolute; top: 6; left: 0px;">
                    <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                        margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                        <tr>
                            <td>
                                <asp:Label ID="lblcollege1" runat="server" Text="College" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege1" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="223px" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege1_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Text="Department" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="213px">
                                </asp:DropDownList>
                                <td>
                                    <asp:UpdatePanel ID="UpstaffGo" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="staffgo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="staffgoClick1" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel20" runat="server">
        <ContentTemplate>
            <center>
                <asp:GridView ID="grdStaff" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                    ShowHeader="true" Font-Names="book antiqua" togeneratecolumns="true" OnSelectedIndexChanged="grdStaff_onselectedindexchanged"
                    Width="840px">
                    <HeaderStyle BackColor="#0ca6ca" ForeColor="white" />
                </asp:GridView>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <br />
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel21" runat="server">
            <ContentTemplate>
                <div id="div3" runat="server" visible="false">
                    <table style="font-family: Book Antiqua; font-weight: bold">
                        <tr align="center">
                            <td>
                                <asp:Label ID="lblstaffacr" runat="server" Text="Acronym:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtstaffacr" runat="server" Font-Names="Book Antiqua" MaxLength="15"
                                    Font-Size="Medium" Height="20px" Width="86px" OnTextChanged="txtstaffacr_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblstaffstart" runat="server" Text=" Starts With:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtstaffstart" runat="server" Font-Names="Book Antiqua" MaxLength="3"
                                    Font-Size="Medium" Height="20px" Width="86px" OnTextChanged="txtstaffstart_TextChanged"
                                    AutoPostBack="True"></asp:TextBox><asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                        runat="server" TargetControlID="txtstaffstart" FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblstaffsize" runat="server" Text="Serial Size:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtstaffsize" runat="server" Font-Names="Book Antiqua" MaxLength="1"
                                    Font-Size="Medium" Height="20px" Width="86px" OnTextChanged="txtstaffsize_TextChanged"
                                    AutoPostBack="True"></asp:TextBox><asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                        runat="server" TargetControlID="txtstaffsize" FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpStaffGen" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnstaffgen" Text="Generate" Width="63px" CssClass=" textbox btn1"
                                            runat="server" OnClientClick="return valid2()" OnClick="btnstaffgenClick" />
                                        <asp:ImageButton ID="btnsave1" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="btnsave1Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel22" runat="server">
            <ContentTemplate>
                <div id="Div4" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div1" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label3" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanelbtn6" runat="server">
                                                <ContentTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="Button6" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="Button1_Click" />
                                                    </center>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
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
    <center>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="print3" runat="server" visible="false">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                        <ContentTemplate>
                            <asp:Label ID="Label1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                            <asp:Label ID="Label2" runat="server" Visible="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Report Name"></asp:Label>
                            <asp:TextBox ID="TextBox2" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                                OnClick="btnExcel2_Click2" />
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                OnClick="btnprintmaster2_Click2" />
                            <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="ImageButton1" />
                            <asp:PostBackTrigger ControlID="ImageButton2" />
                            <asp:PostBackTrigger ControlID="NEWPrintMater1" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for Go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upGo">
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
    <%--progressBar for StudentGeneration--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpGeneration">
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
    <%--progressBar for StaffGeneration--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpStaffGen">
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
</asp:Content>
