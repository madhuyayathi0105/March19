<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="book_circulation_report.aspx.cs"
    Inherits="LibraryMod_book_circulation_report" MasterPageFile="~/LibraryMod/LibraryMaster.master" %>
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
                <span class="fontstyleheader" style="color: Green;">Book Circulation Report </span>
            </div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="updatepanel2" runat="server">
            <ContentTemplate>
                <center>
                    <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                        margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                        <tr>
                            <asp:UpdatePanel ID="updatepanel22" runat="server">
                                <ContentTemplate>
                                    <td>
                                        <asp:Label ID="lblCollege" runat="server" Text="College" Width="79px">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Style="width: 180px; margin-left: 33px;" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbllibrary" runat="server" Text="Library" Width="57px" Style="margin-left: 11px;">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="8">
                                        <asp:Label ID="lbldepart" runat="server" Text="Department" Width="87px" Style="margin-left: 16px;"></asp:Label>
                                        <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddldept_selectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                        <tr>
                            <asp:UpdatePanel ID="updatepanel1" runat="server">
                                <ContentTemplate>
                                    <td>
                                        <asp:CheckBox ID="cbdate1" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbdate1_OnCheckedChanged" />
                                        <asp:Label ID="lbl_fromdate" runat="server" Text="From:" Width="78px" Style="margin-left: 8px;"></asp:Label>
                                        <asp:TextBox ID="txt_fromdate1" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                            Style="margin-left: 2px;" onchange="return checkDate()"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_fromdate1" runat="server"
                                            Format="yyyy-MM-dd" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_todate" runat="server" Text="To:" Width="55px" Style="margin-left: 13px;"></asp:Label>
                                        <asp:TextBox ID="txt_todate1" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                            onchange="return checkDate()"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todate1" runat="server"
                                            Format="yyyy-MM-dd" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td colspan="1">
                                        <asp:Label ID="lblsearchby" runat="server" Text="Search By:" CssClass="commonHeaderFont"
                                            Width="82px" Style="margin-left: 17px;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblStatus" runat="server" Visible="true" RepeatDirection="Horizontal"
                                            AutoPostBack="true" ForeColor="Black" Width="209px" Style="margin-left: -5px;">
                                            <asp:ListItem Selected="True">ByAccessNo</asp:ListItem>
                                            <asp:ListItem>ByTitle</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblaccno" runat="server" Visible="false" Text="AccessNumber" Width="114px"></asp:Label>
                                <asp:TextBox ID="txtaccess" runat="server" Visible="false" CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:Button ID="btnaccessno" runat="server" CssClass="textbox btn2" Text="?" OnClick="btnaccessno_Click"
                                    Visible="false" Width="45px" Height="28px" />
                                <asp:Label ID="lblcriteria" runat="server" Text="Criteria" Width="83px" Visible="false">
                       
                                </asp:Label>
                                <asp:DropDownList ID="ddlcriteria" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="width: 150px; margin-left: 30px;" AutoPostBack="true" Visible="false"
                                    OnSelectedIndexChanged="ddllcriteria_SelectedIndexChanged">
                                    <asp:ListItem>Start With</asp:ListItem>
                                    <asp:ListItem>Like</asp:ListItem>
                                    <asp:ListItem>Exactly</asp:ListItem>
                                    <asp:ListItem>End With</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbltitle" runat="server" Text="Title" Width="56px" Visible="false"
                                    Style="margin-left: 13px;">
                                </asp:Label>
                                <%--<asp:Panel ID="pnltitle" runat="server" CssClass="multxtpanel" Style="width: 151px; height: 22px; overflow: auto; margin: 0px; padding: 0px; color: Black;">--%>
                                <asp:DropDownList ID="ddltitle" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                    Width="150px" AutoPostBack="false" OnSelectedIndexChanged="ddltitle_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:TextBox ID="txttit" runat="server" Visible="false" Style="width: 144px;" CssClass="textbox txtheight2"></asp:TextBox>
                                <%--</asp:Panel>--%>
                            </td>
                            <td>
                                <asp:Label ID="lblauthor" Visible="false" runat="server" Text="Author" Width="85px"
                                    Style="margin-left: 20px;">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlauthor" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="150px" AutoPostBack="false" Visible="false" OnSelectedIndexChanged="ddlauthor_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtauth" runat="server" Visible="false" Style="width: 145px;" CssClass="textbox txtheight2"></asp:TextBox>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upGo" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="Btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="updatepanel3" runat="server">
        <ContentTemplate>
            <div id="divtable" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <center>
                                <asp:GridView ID="grdManualExit" runat="server" ShowFooter="false"
                                    Font-Names="Book Antiqua"  AllowPaging="true" PageSize="10" ShowHeader="true"
                                    OnSelectedIndexChanged="grdManualExit_OnSelectedIndexChanged" OnPageIndexChanging="grdManualExit_OnPageIndexChanged"
                                    Width="980px" Style="margin-left: 247px;">
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                </asp:GridView>
                            </center>
                            <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                            </asp:Label></center>
                            <div id="div_report" runat="server" visible="false">
                                <center>
                                    <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                        CssClass="textbox textbox1 txtheight5" onkeypress="return ClearPrint1()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:ImageButton ID="btn_Excel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                                        OnClick="btnExcel_Click" />
                                    <asp:ImageButton ID="btn_printmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                        OnClick="btn_printmaster_Click" />
                                     <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                </center>
                            </div>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
         <Triggers>
                <asp:PostBackTrigger ControlID="btn_Excel" />
                <asp:PostBackTrigger ControlID="btn_printmaster" />
            </Triggers>
    </asp:UpdatePanel>
    <center>
        <asp:UpdatePanel ID="updatepanel4" runat="server">
            <ContentTemplate>
                <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%; right: 0%;">
                    <center>
                        <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="updatepanelbtn2" runat="server">
                                                <ContentTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="btnPopAlertClose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                            OnClick="btnPopAlertClose_Click" />
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
        <asp:UpdatePanel ID="updatepanel5" runat="server">
            <ContentTemplate>
                <div id="divaccess" runat="server" visible="false" style="height: 70em; z-index: 100;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0px;">
                    <br />
                    <center>
                        <div id="divaccessno" runat="server" class="table" visible="false" style="background-color: White;
                            border: 5px solid #0CA6CA; font-family: Book Antiqua; border-top: 25px solid #0CA6CA;
                            margin-left: auto; margin-right: auto; width: 880px; height: auto; z-index: 1000;
                            border-radius: 5px;">
                            <center>
                                <span style="top: 10px; bottom: 20px; text-align: center; color: Green; font-size: large;
                                    position: relative; font-weight: bold;">Select Access Number </span>
                            </center>
                            <table style="margin: 10px; margin-bottom: 10px; margin-top: 10px; margin-left: 5px;
                                position: relative; width: 612px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsearch" runat="server" Text="Search By:" Width="82px" Visible="true">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlsearch" runat="server" Visible="true" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlsearch_SelectedIndexChanged">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Title</asp:ListItem>
                                            <asp:ListItem>Author</asp:ListItem>
                                            <asp:ListItem>Access Number</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtsearch" runat="server" Visible="true" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upAccGo" runat="server">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="btngoaccess" runat="server" ImageUrl="~/LibImages/GoWhite.jpg"
                                                    OnClick="btngoaccess_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <div id="div1acc" runat="server" visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="GridView1" runat="server" ShowFooter="false" AutoGenerateColumns="false"
                                                Font-Names="Book Antiqua" toGenerateColumns="true" AllowPaging="true" PageSize="50"
                                                OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnPageIndexChanging="GridView1_OnPageIndexChanged"
                                                Width="867px" Style="margin-left: auto;">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Acc_No">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkview" runat="server" Text='<%#Eval("Acc_No") %>' ForeColor="Black"
                                                                OnClick="view_click" Font-Underline="false"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Title">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lnkview1" runat="server" Text='<%#Eval("Title") %>' ForeColor="Black"
                                                                OnClick="view_click" Font-Underline="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Author">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lnkview2" runat="server" Text='<%#Eval("Author") %>' ForeColor="Black"
                                                                OnClick="view_click" Font-Underline="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle BackColor="#0CA6CA" ForeColor="White" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            </br>
                            <table>
                                <tr>
                                    <asp:UpdatePanel ID="updatepanelbtn3" runat="server">
                                        <ContentTemplate>
                                            <td>
                                                <asp:ImageButton ID="btnok" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btn_okk1_Click" />
                                                <asp:ImageButton ID="btnex" runat="server" ImageUrl="~/LibImages/save (2).jpg" OnClick="btn_ex_Click" />
                                            </td>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for upGo--%>
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
    <%--progressBar for upAccGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upAccGo">
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
