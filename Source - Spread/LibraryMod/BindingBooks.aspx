<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="BindingBooks.aspx.cs"
    MasterPageFile="~/LibraryMod/LibraryMaster.master" Inherits="LibraryMod_BindingBooks" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="content2" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="scriptmanager" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <span class="fontstyleheader" style="color: Green;">Bind Books</span>
        </center>
    </div>
    <div>
        <center>
            <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold;
                margin-top: 10px; margin-left: 0px; margin-bottom: 10px; padding: 6px;">
                <tr>
                    <td>
                        <asp:Label ID="lblclg" runat="server" Text="College" Style="margin-left: 3px; width: 80px;"></asp:Label>
                        <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                            AutoPostBack="true" Style="margin-left: 55px; width: 155px;" OnSelectedIndexChanged="ddlclg_selectedindexchange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbllibrary" runat="server" Text="Library" Style="margin-left: 4px;
                            width: 80px;"></asp:Label>
                        <asp:DropDownList ID="ddllibrary" runat="server" Style="margin-left: 53px; width: 150px;"
                            OnSelectedIndexChanged="ddllib_selectedindexchange" CssClass="textbox ddlstyle ddlheight3">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSerialno" runat="server" Text="Serial No" Style="margin-left: 3px;
                            width: 80px;"></asp:Label>
                        <asp:TextBox ID="txtserialno" runat="server" Enabled="false" Style="margin-left: 1px;
                            width: 123px;" CssClass="textbox txtheight2"></asp:TextBox>
                        <asp:Button ID="btnserialno" runat="server" Text="?" Style="margin-left: -6px; width: 30px;
                            height: 32px" Font-Names="Book Antiqua" OnClick="btnserialno_onclick" />
                    </td>
                    <td>
                        <asp:Label ID="lblbindorderdate" runat="server" Text="Binding Order Date" Style="margin-left: 10px;
                            width: 80px"></asp:Label>
                        <asp:TextBox ID="txtbindorderdt" runat="server" Height="15px" Width="120px" onchange="return checkdate()"
                            CssClass="textbox txtheight2"></asp:TextBox>
                        <asp:CalendarExtender ID="calender1" runat="server" TargetControlID="txtbindorderdt"
                            Format="MM/dd/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblexpecteddate" runat="server" Text="Expected Date" Style="margin-left: 3px;
                            width: 80px;"></asp:Label>
                        <asp:TextBox ID="txtexpdate" runat="server" Height="15px" Width="145px" Style="margin-left: 2px;"
                            onchange="return checkdate()" CssClass="textbox txtheight2"></asp:TextBox>
                        <asp:CalendarExtender ID="calender2" runat="server" TargetControlID="txtexpdate"
                            Format="MM/dd/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblcompany" runat="server" Text="Company Name" Style="margin-left: -2px;
                            width: 80px;"></asp:Label>
                        <asp:TextBox ID="txtcompany" Enabled="false" runat="server" Style="margin-left: -1px;
                            height: 15px; width: 141px;" CssClass="textbox txtheight2"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtIndianpriceFilteredTextBoxExtender" runat="server"
                            TargetControlID="txtcompany" FilterType="LowercaseLetters">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblemailid" runat="server" Text="Email Id" Style="margin-left: 0px;
                            width: 80px"></asp:Label>
                        <asp:TextBox ID="txtemailid" Enabled="false" runat="server" Style="margin-left: 11px;
                            width: 150px" CssClass="textbox txtheight2"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblphnoe" runat="server" Text="Phone No" Style="margin-left: 11px;
                            width: 80px;"></asp:Label>
                        <asp:TextBox ID="txtphone" Enabled="false" runat="server" Style="margin-left: 70px;
                            width: 119px" CssClass="textbox txtheight2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="updatepanel3" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblAddress" runat="server" Text="Address" Style="margin-left: 3px;
                                    width: 80px;"></asp:Label>
                                <asp:TextBox ID="txtaddress" TextMode="MultiLine" Enabled="false" runat="server"
                                    Style="margin-left: 50px; width: 146px;" CssClass="textbox txtheight2"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbltype" runat="server" Text="Type" Style="margin-left: 6px; width: 80px;"></asp:Label>
                        <asp:RadioButtonList ID="rbltype" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                            ForeColor="Black" Style="margin-left: 51px; width: 184px; margin-top: -23px"
                            OnSelectedIndexChanged="rbl_typeSelectedindex">
                            <asp:ListItem Selected="True">Periodicals</asp:ListItem>
                            <asp:ListItem>Books</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnBook" runat="server" ImageUrl="~/LibImages/books select.jpg"
                            Style="margin-left: 7px;" OnClick="btnBook_onclick" />
                        <asp:ImageButton ID="BtnPeriodical" runat="server" ImageUrl="~/LibImages/Select periodicals.jpg"
                            Style="margin-left: 7px;" OnClick="BtnPeriodical_onclick" />
                        <asp:ImageButton ID="btnbind" runat="server" ImageUrl="~/LibImages/bind.jpg" Style="margin-left: 7px;"
                            OnClick="btnbind_onclick" Enabled="false" />
                    </td>
                    <td>
                        <asp:ImageButton ID="btnreturnbind" runat="server" ImageUrl="~/LibImages/bind return.jpg"
                            Style="margin-left: 7px;" OnClick="btnbindreturn_onclick" Enabled="false" />
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <center>
        <div id="divserialno" runat="server" visible="false" style="height: 70px; z-index: 100;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0px;">
            <center>
                <div id="divserialn" runat="server" class="table" style="background-color: White;
                    border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-left: auto;
                    margin-right: auto; width: 500px; height: auto; z-index: 1000; border-radius: 5px;">
                    <center>
                        <span style="top: 10px; bottom: 20px; text-align: center; color: Green; font-size: large;
                            position: relative; font-weight: bold;">Select Serial No</span>
                    </center>
                    <table style="margin: 10px; margin-bottom: 10px; margin-top: -154px; margin-left: 28px;
                        position: relative; width: 382px; height: 382px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblserialno1" runat="server" Text="Serial No" Style="margin-left: 30px;
                                    width: 80px;"></asp:Label>
                                <asp:TextBox ID="txtserialno1" runat="server" Enabled="true" Style="margin-left: 5px;
                                    width: 150px" CssClass="textbox txtheight2"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="btngoserial" runat="server" ImageUrl="~/LibImages/GoWhite.jpg"
                                    Style="margin-left: 5px;" OnClick="btngoserial_click" />
                            </td>
                        </tr>
                    </table>
                    <div id="div1" runat="server" visible="false">
                        <table style="margin: 10px; margin-bottom: 13px; margin-top: -187px; margin-left: 16px;
                            position: relative; width: 382px; height: 382px;">
                            <tr>
                                <td>
                                    <center>
                                        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                        <asp:GridView ID="gridview1" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                            Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="50"
                                            Width="300px" OnSelectedIndexChanged="gridview1_onselectedindexchanged" OnRowCreated="gridview1_OnRowCreated">
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
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="btnok" runat="server" Visible="false" ImageUrl="~/LibImages/ok.jpg"
                                    OnClick="btnOk_click" />
                                <asp:ImageButton ID="btnexit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                    OnClick="btnexit_click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
    </center>
    <center>
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
                                    <center>
                                        <asp:ImageButton ID="btnPopAlertClose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                            OnClick="btnPopAlertClose_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <div id="divtable" runat="server" visible="false">
        <center>
            <table>
                <tr>
                    <td>
                        <center>
                            <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                            <asp:GridView ID="gridview2" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="500"
                                Width="1067px">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                            </asp:Label></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkenbl" runat="server" AutoPostBack="True" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                            </asp:GridView>
                            <center>
                                <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                                </asp:Label></center>
                            <br />
                            <br />
                            <br />
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
                                    <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                                </center>
                            </div>
                        </center>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <center>
        <div id="divselectbook" runat="server" visible="false" style="height: 70px; z-index: 100;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0px;">
            <center>
                <div id="divselectbook1" runat="server" class="table" style="background-color: White;
                    border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-left: auto;
                    margin-right: auto; width: 864px; height: auto; z-index: 1000; border-radius: 5px;">
                    <center>
                        <span style="top: 10px; bottom: 20px; text-align: center; color: Green; font-size: large;
                            position: relative; font-weight: bold;">Select Books</span>
                    </center>
                    <table style="margin: 10px; margin-bottom: 10px; margin-top: -89px; margin-left: 28px;
                        position: relative; width: 744px; height: 267px;">
                        <tr>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Text="Department" Style="margin-left: -16px;
                                    width: 80px;"></asp:Label>
                                <asp:DropDownList ID="ddldept" runat="server" Style="margin-left: -2px; width: 150px;"
                                    OnSelectedIndexChanged="ddldept_selectedindex" CssClass="textbox ddlstyle ddlheight3">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsearchby" runat="server" Text="Search By" Style="margin-left: -8px;
                                    width: 80px;"></asp:Label>
                                <asp:DropDownList ID="ddlsearch" runat="server" Style="margin-left: 0px; width: 150px;"
                                    OnSelectedIndexChanged="ddlsearch_selectedindex" AutoPostBack="true" CssClass="textbox ddlstyle ddlheight3">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Book Title</asp:ListItem>
                                    <asp:ListItem>Author</asp:ListItem>
                                    <asp:ListItem>Access Number</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtsearch" Visible="false" runat="server" Style="margin-left: -4px;
                                    <asp:TextBox ID="txtsearch" Visible="false" runat="server" Style="margin-left: -4px;
                                    width: 150px;" CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:ImageButton ID="btnselectbooks" runat="server" ImageUrl="~/LibImages/GoWhite.jpg"
                                    Style="margin-left: 22px;" OnClick="btnselectbooks_click" />
                                <asp:ImageButton ID="btnex" runat="server" ImageUrl="~/LibImages/save (2).jpg" Style="margin-left: -1px;"
                                    OnClick="btnex_click" />
                            </td>
                        </tr>
                    </table>
                    <div id="div2" runat="server" visible="true">
                        <table style="margin: 10px; margin-bottom: 10px; margin-top: -111px; margin-left: 28px;
                            position: relative; width: 811px; height: 382px;">
                            <tr>
                                <td>
                                    <center>
                                        <asp:HiddenField ID="HiddenField2" runat="server" Value="-1" />
                                        <asp:GridView ID="gridview3" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                            Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="50"
                                            Width="800px">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                                        </asp:Label></center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkenbl" runat="server" AutoPostBack="True" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                        </asp:GridView>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:ImageButton ID="btnselbokok" runat="server" Visible="false" ImageUrl="~/LibImages/ok.jpg"
                                            OnClick="btnselbokok_click" />
                                        <asp:ImageButton ID="btnselbokexit" runat="server" Visible="false" ImageUrl="~/LibImages/save (2).jpg"
                                            OnClick="btnselbokexit_click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="divselectperiodicals" runat="server" visible="false" style="height: 70px;
            z-index: 100; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
            top: 0%; left: 0px;">
            <center>
                <div id="divselperiodicals" runat="server" class="table" style="background-color: White;
                    border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-left: auto;
                    margin-right: auto; width: 864px; height: auto; z-index: 1000; border-radius: 5px;">
                    <center>
                        <span style="top: 10px; bottom: 20px; text-align: center; color: Green; font-size: large;
                            position: relative; font-weight: bold;">Select Periodicals</span>
                    </center>
                    <table style="margin: 10px; margin-bottom: 10px; margin-top: -99px; margin-left: 28px;
                        position: relative; width: 744px; height: 267px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblperiodicalsearch" runat="server" Text="Search By" Style="margin-left: 191px;
                                    width: 80px;"></asp:Label>
                                <asp:DropDownList ID="ddlpersearch" runat="server" Style="margin-left: -1px; width: 150px;"
                                    OnSelectedIndexChanged="ddlpersearch_selectedindex" AutoPostBack="true" CssClass="textbox ddlstyle ddlheight3">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Acc No</asp:ListItem>
                                    <asp:ListItem>Journal Title</asp:ListItem>
                                    <asp:ListItem>Department</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtpersearch" Visible="false" runat="server" Style="margin-left: -6px;
                                    width: 150px;" CssClass="textbox txtheight2"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="btnpergo" runat="server" ImageUrl="~/LibImages/GoWhite.jpg"
                                    Style="margin-left: 1px;" OnClick="btnpergo_click" />
                                <asp:ImageButton ID="btnexit3" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                    Style="margin-left: 1px;" OnClick="btnexit3_click" />
                            </td>
                        </tr>
                        <br />
                        <br />
                    </table>
                    <div id="divperspread" runat="server" visible="true">
                        <table style="margin: 10px; margin-bottom: 10px; margin-top: -145px; margin-left: 28px;
                            position: relative; width: 811px; height: 382px;">
                            <tr>
                                <br />
                                <br />
                                <td>
                                    <center>
                                        <asp:HiddenField ID="HiddenField3" runat="server" Value="-1" />
                                        <asp:GridView ID="gridview4" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                            Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="50"
                                            OnSelectedIndexChanged="gridview4_onselectedindexchanged" OnRowCreated="gridview4_OnRowCreated"
                                            Width="800px">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                                        </asp:Label></center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkenbl" runat="server" AutoPostBack="True" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                        </asp:GridView>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:ImageButton ID="btnperok" Visible="false" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                            OnClick="btnperok_click" />
                                        <asp:ImageButton ID="btnperexit" runat="server" Visible="false" ImageUrl="~/LibImages/save (2).jpg"
                                            OnClick="btnperexit_click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
