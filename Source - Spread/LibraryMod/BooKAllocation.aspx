<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="BooKAllocation.aspx.cs" Inherits="LibraryMod_BooKAllocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <span class="fontstyleheader" style="color: green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">Book Allocation</span>
        <br />
        <center>
            <asp:UpdatePanel ID="updatepanel1" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <table class="maintablestyle" style="margin: 0px; font-family: Book Antiqua; margin-bottom: 0px;
                                    position: relative; width: 800px; height: 230px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Lblcollege" runat="server" Text="College" Visible="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Visible="true" Width="140px" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbllibrary" runat="server" Text="Library" CssClass="commonHeaderFont"
                                                Visible="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_library" runat="server" Style="margin-left: -71px" CssClass="textbox ddlstyle ddlheight3"
                                                Visible="true" Width="121px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbldept" runat="server" Text="Dept" CssClass="commonHeaderFont" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddldeptm" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Visible="false" Width="120px" 
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Lblmove" runat="server" Text="MoveTo:" CssClass="commonHeaderFont"
                                                Visible="true"></asp:Label>
                                        </td>
                                        <td colspan="11">
                                            <fieldset style="width: 504px; height: 17px;">
                                                <asp:RadioButton ID="rdbrack" runat="server" Text="Rack" OnCheckedChanged="rdbrack_CheckedChange"
                                                    GroupName="Move" AutoPostBack="true" />
                                                <asp:RadioButton ID="rdbrackto" runat="server" Text="Rack To Rack" GroupName="Move"
                                                    OnCheckedChanged="rdbrackto_CheckedChange" AutoPostBack="true" />
                                                <asp:RadioButton ID="rdbLibrary" runat="server" Text="Library" GroupName="Move" OnCheckedChanged="rdbLibrary_CheckedChange"
                                                    AutoPostBack="true" Checked="true" />
                                                <asp:RadioButton ID="rdbissue" runat="server" Text="Dept Issue" GroupName="Move"
                                                    AutoPostBack="true" OnCheckedChanged="rdbissue_CheckedChange" />
                                                <asp:RadioButton ID="rdbreturn" runat="server" Text="Dept Return" GroupName="Move"
                                                    OnCheckedChanged="rdbreturn_CheckedChange" AutoPostBack="true" />
                                                <asp:RadioButton ID="rdbtrans" runat="server" Text="Dept Transfer" GroupName="Move"
                                                    AutoPostBack="true" OnCheckedChanged="rdbtrans_CheckedChange" Visible="false" />
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Rack" CssClass="commonHeaderFont" Visible="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlrack2" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Visible="true" Width="120px" OnSelectedIndexChanged="ddlrack2_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Shelf" CssClass="commonHeaderFont" Style="margin-left: 0px"
                                                Visible="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlself1" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Visible="true" Style="width: 95px; margin-left: -103px" OnSelectedIndexChanged="ddlshelf1_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblposit" runat="server" Text="Position" Style="margin-left: -100px"
                                                CssClass="commonHeaderFont" Visible="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlposi" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Visible="true" Style="width: 111px; margin-left: -99px;" OnSelectedIndexChanged="ddlposi_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblpla" runat="server" Text="Place" Style="margin-left: -12px" CssClass="commonHeaderFont"
                                                Visible="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlpla" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Visible="true" Style="margin-left: -39px" Width="98px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" Text="SearchBy "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlbook" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Visible="true" Width="120px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_booksearch" runat="server" CssClass="textbox  txtheight9" Width="114px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Lblacc" runat="server" Text="From" Style="margin-left: 0px" Visible="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtfromaccno" runat="server" CssClass="textbox txtheight2" Width="56px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Lbltoacc" runat="server" Text="To" Visible="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txttoaccno" runat="server" CssClass="textbox txtheight2" Width='56px'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="BookType" Visible="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlsearchbook" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Visible="true" Width="120px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="1px">
                                            <
                                            <asp:CheckBox ID="cbref" runat="server" Style="margin-left: -21px" Text="Reference"
                                                AutoPostBack="True" />
                                        </td>
                                        <td colspan="3px">
                                            <asp:CheckBox ID="Cboldsearch" runat="server" Style="margin-left: -53px" Text="Include Old search"
                                                AutoPostBack="True" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblcate" runat="server" Text="Category" Style="margin-left: -98px"
                                                Visible="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Visible="true" Width="90px" Style="margin-left: -101px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updCommonGo" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="BtnGo" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-left: -91px"
                                                        Visible="True" OnClick="Go_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="Lblfrom" runat="server" Text="From" Visible="true" Style="margin-left: 0px;"></asp:Label>
                                            <asp:TextBox ID="Txtfromacc" runat="server" CssClass="textbox txtheight2" Width="59px"></asp:TextBox>
                                            <asp:Label ID="Lblto" runat="server" Text="To" Visible="true"></asp:Label>
                                            <asp:TextBox ID="Txttoacc" runat="server" CssClass="textbox txtheight2" Width="59px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upGo" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="Button1" runat="server" ImageUrl="~/LibImages/Go.jpg" Visible="True"
                                                        OnClick="btn1_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <%--</fieldset>--%>
                            </td>
                            <td>
                                <%-- <fieldset style="margin: 0px; padding: 0px; height: auto; width: 484px; float: left;
                            border-color: #fff0;">--%>
                                <table class="maintablestyle" style="margin: 0px; font-family: Book Antiqua; margin-bottom: 0px;
                                    position: relative; width: 500px; height: 230px; margin-left: 1px" border="0">
                                    <tr id="col4" runat="server">
                                        <td>
                                            <asp:Label ID="Lblmoveto1" runat="server" Text="MoveToLibrary" Visible="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlmoveto" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Visible="true" Width="120px" OnSelectedIndexChanged="ddlmovetorack_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Lblmoveself" runat="server" Text="MoveToShelf"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlshelf" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="120px" OnSelectedIndexChanged="ddlshelf_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Tr1" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Lblposi" runat="server" Text="MovePosition"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlposition" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="120px" OnSelectedIndexChanged="ddlposition_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Lblplace" runat="server" Text="Place" CssClass="commonHeaderFont"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlplace" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="120px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="col" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Lblmaxcap1" runat="server" Text="MaxCapacity"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ddlmaxcap1" runat="server" CssClass="textbox txtheight2" Width="55px"
                                                BackColor="#ffffcc" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Lblmaxcap" runat="server" Text="MaxCapacity" CssClass="commonHeaderFont"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ddlmaxcap" runat="server" CssClass="textbox txtheight2" Width="55px"
                                                BackColor="#ffffcc" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="MaxCapacity"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox1" runat="server" CssClass="textbox txtheight2" Width="55px"
                                                BackColor="#ffffcc" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="col1" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="LblBooksav" runat="server" Text="Available" CssClass="commonHeaderFont"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ddlbooksav" runat="server" CssClass="textbox txtheight2" Width="55px"
                                                BackColor="#ffffcc" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblBooks" runat="server" Text="Available"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ddlbooks" runat="server" CssClass="textbox txtheight2" Width="55px"
                                                BackColor="#ffffcc" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label15" runat="server" Text="Available" CssClass="commonHeaderFont"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox txtheight2" Width="50px"
                                                BackColor="#ffffcc" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="col6" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Lbltracsdate" runat="server" Text="Trans Date" CssClass="commonHeaderFont"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_transdate" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                                Width="100px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_transdate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UptransGo" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="Btngo1" runat="server" ImageUrl="~/LibImages/ok1.jpg" OnClick="Btngo1_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr id="col99" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Location" Visible="true"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="Btnadd" runat="server" Text="+" Style="width: 32px; height: 29px;
                                                margin-left: -35px;" Visible="True" OnClick="Btnadd_Click" />
                                            <asp:DropDownList ID="ddlreason" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Visible="true" Width="98px" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:Button ID="Btnsub" runat="server" Text="-" Style="width: 32px; height: 29px;"
                                                Visible="True" OnClick="Btnsub_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <div id="Div1" runat="server" visible="false" style="height: 120px; z-index: 0px;
                                    width: 0px; background-color: rgba(54, 25, 25, .2); position: absolute; margin-top: 115px;
                                    left: 80px;">
                                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: -131px;
                                        border-radius: 10px; margin-left: 613px">
                                        <center>
                                            <table style="height: 100px; width: 120px">
                                                <tr align="center">
                                                    <td align="center" colspan="2">
                                                        <asp:TextBox ID="txt_infra" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                                            Visible="true" Width="100px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:ImageButton ID="btnsave" runat="server" ImageUrl="~/LibImages/save.jpg" Visible="True"
                                                                OnClick="btnsave_Click" />
                                                    </td>
                                                    <td>
                                                        <center>
                                                            <asp:ImageButton ID="btnexit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                                Visible="True" OnClick="btnexit_Click" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </div>
                                <%-- </fieldset>--%>
                            </td>
                        </tr>
                        <tr>
                            <%--<td colspan="2">
                                <table>
                                    <tr>--%>
                            <td>
                                <center>
                                    <div id="BookSDiv" visible="true" runat="server" style="width: 800px; overflow: auto;
                                        height: 300px; overflow: auto;">
                                        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                        <asp:GridView ID="grdBooks" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                            Font-Names="book antiqua" togeneratecolumns="true" ShowHeaderWhenEmpty="true"
                                            AllowPaging="true" PageSize="1000" OnPageIndexChanging="grdBooks_onpageindexchanged"
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
                                    </div>
                                </center>
                            </td>
                            <td>
                                <center>
                                    <div id="TransBookDiv" runat="server" visible="true" style="width: 500px; overflow: auto;
                                        height: 300px;">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                        <asp:GridView ID="grdTranBooks" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                            Font-Names="book antiqua" togeneratecolumns="true" ShowHeaderWhenEmpty="true"
                                            AllowPaging="true" PageSize="1000" OnPageIndexChanging="grdTranBooks_onpageindexchanged"
                                            Width="400px">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_sno1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                                        </asp:Label></center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkenbl1" runat="server" AutoPostBack="True" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                        </asp:GridView>
                                    </div>
                                </center>
                            </td>
                            <%-- </tr>
                                </table>
                            </td>--%>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="link_status" Text="View Rack Status Monitor" Font-Name="Book Antiqua"
                                    Font-Size="11pt" OnClick="link_status_Click" runat="server" Width="192px" />
                                <center>
                                    <asp:UpdatePanel ID="upNext" runat="server">
                                        <ContentTemplate>
                                            <fieldset style="margin: 0px; padding: 0px; height: auto; width: 550px; border-color: #f000;
                                                float: left;">
                                                <asp:ImageButton ID="Button4" runat="server" ImageUrl="~/LibImages/Next 2.jpg" Visible="true"
                                                    OnClick="btntrans_Click" />
                                                <asp:ImageButton ID="Button5" runat="server" ImageUrl="~/LibImages/back 2.jpg" Visible="false"
                                                    OnClick="btntran_Click" />
                                                <asp:ImageButton ID="Button7" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                    Visible="false" />
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </center>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--  ----------------------Start Status-------------------------------%>
            <asp:UpdatePanel ID="updatepanel2" runat="server">
                <ContentTemplate>
                    <div id="DivStatus" runat="server" class="popupstyle popupheight1" visible="false"
                        style="height: 300em; width: auto;">
                        <br />
                        <div style="background-color: White; height: 600px; width: 840px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px; margin-left: 80px; margin-top: 80px">
                            <asp:ImageButton ID="ImageButton4" Visible="true" runat="server" Width="40px" Height="40px"
                                ImageUrl="~/images/close.png" Style="height: 30px; width: 30px; position: absolute;
                                margin-top: -32px; margin-left: 400px;" OnClick="btn_Question_Bank_popup_Click" />
                            <br />
                            <center>
                                <span class="fontstyleheader" style="color: #008000;">Rack Status Monitor</span>
                            </center>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <center>
                                                <div>
                                                    <table class="maintablestyle" style="height: auto; margin-left: 103px; margin-top: 10px;
                                                        margin-bottom: 10px; padding: 6px; width: 745px">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label5" runat="server" Text="College" CssClass="commonHeaderFont">
                                                                </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlstat_college" runat="server" Style="width: 204px; height: 30px;
                                                                    margin-left: 72px;" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_sts_SelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label212" runat="server" Text="Library" CssClass="commonHeaderFont">
                                                                </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddllibrary_sts" runat="server" Style="width: 204px; height: 30px;
                                                                    margin-left: 31px;" AutoPostBack="true" OnSelectedIndexChanged="ddllibrary_sts_SelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="1">
                                                                <asp:Label ID="Label6" runat="server" Text="Rack Number" CssClass="commonHeaderFont">
                                                                </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlsts_rackno" runat="server" Style="width: 204px; height: 30px;
                                                                    margin-left: 75px;" AutoPostBack="true" OnSelectedIndexChanged="ddlrack_sts_SelectedIndexChanged"
                                                                    CssClass="textbox ddlstyle ddlheight3">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpStStatus" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:ImageButton ID="btn_sts_Rack_Go" runat="server" ImageUrl="~/LibImages/Go.jpg"
                                                                            Style="margin-left: 31px;" OnClick="btn_sts_Rack_Go_Click" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <fieldset id="Fieldset1" runat="server" style="width: 103px; height: 4px; background-color: Red;
                                                                margin-left: 156px;">
                                                                <asp:Label ID="Label7" runat="server" Text="CompletetyFilled"></asp:Label>
                                                            </fieldset>
                                                        </td>
                                                        <td>
                                                            <fieldset id="Fieldset6" runat="server" enabled="false" style="width: 103px; height: 4px;
                                                                background-color: Green; margin-left: 27px;">
                                                                <asp:Label ID="Label8" runat="server" Text="PartiallyFilled"></asp:Label>
                                                            </fieldset>
                                                        </td>
                                                        <td>
                                                            <fieldset id="Fieldset7" runat="server" enabled="false" style="width: 103px; height: 4px;
                                                                background-color: Yellow; margin-left: 27px;">
                                                                <asp:Label ID="Label9" runat="server" Text="No Shelf Entry"></asp:Label>
                                                            </fieldset>
                                                        </td>
                                                        <%-- <td>
                                            <asp:Label Text="SH->Shelf" runat="server" Style="font-style: italic"></asp:Label>
                                        </td>--%>
                                                    </tr>
                                                </table>
                                                <table style="margin-left: 103px; margin-top: 10px;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label10" Text="SH->Shelf" runat="server" Style="font-style: italic"></asp:Label>
                                                            <asp:Label ID="Label11" Text="AVAIL->Available Copies" runat="server" Style="font-style: italic"></asp:Label>
                                                            <asp:Label ID="Label12" Text="TOT->Maximum Copies" runat="server" Style="font-style: italic"></asp:Label>
                                                            <asp:Label ID="Label13" Text="IM->Category Of Inward Material" runat="server" Style="font-style: italic"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <br />
                                                <FarPoint:FpSpread ID="RackFpSpread" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="1px">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                                <br />
                                                <br />
                                                <div id="rptprint" runat="server" visible="false">
                                                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                                                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                                                        Font-Size="Medium" CssClass="textbox textbox1"></asp:TextBox>
                                                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                                                        Text="Export To Excel" Width="127px" />
                                                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                                        Width="60px" CssClass="textbox textbox1 btn2" />
                                                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                                </div>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%-- ------------------------End Status------------------------------%>
            <center>
                <asp:UpdatePanel ID="updatepanel3" runat="server">
                    <ContentTemplate>
                        <div id="alertpopwindow" runat="server" visible="false" style="height: 120px z-index: 100px;
                            width: 23px; background-color: rgba(54, 25, 25, .2); position: absolute; top: 50px;
                            left: 0px;">
                            <center>
                                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 129px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 360px;
                                    margin-left: 386px; border-radius: 10px;">
                                    <center>
                                        <table style="height: 100px;">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:UpdatePanel ID="UpClose" runat="server">
                                                            <ContentTemplate>
                                                                <asp:ImageButton ID="btnerrclose" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btnerrclose_Click" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
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
            <center>
                <asp:UpdatePanel ID="updatepanel4" runat="server">
                    <ContentTemplate>
                        <div id="DivIssue" runat="server" visible="false" style="height: 100%; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="Div6" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                    border-radius: 10px;">
                                    <center>
                                        <table style="height: 100px; width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="LblIssuesName" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:UpdatePanel ID="UpTransferYes" runat="server">
                                                            <ContentTemplate>
                                                                <asp:ImageButton ID="BtnIssueYesAgain" runat="server" ImageUrl="~/LibImages/yes.jpg"
                                                                    OnClick="btnIssueYes_Click" />
                                                                <asp:ImageButton ID="BtnIssueNoAgain" runat="server" ImageUrl="~/LibImages/no.jpg"
                                                                    OnClick="btnIssueNo_Click" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
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
    </center>
    <%--progressBar for Go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updCommonGo">
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
    <%--progressBar for SelectRangeGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upGo">
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
    <%--progressBar for TransGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UptransGo">
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
    <%--progressBar for UpStStatus--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpStStatus">
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
    <%--progressBar for UpTransferYes--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpTransferYes">
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
    <%--progressBar for UpClose--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="UpClose">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender6" runat="server" TargetControlID="UpdateProgress6"
            PopupControlID="UpdateProgress6">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for upNext--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="upNext">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender7" runat="server" TargetControlID="UpdateProgress7"
            PopupControlID="UpdateProgress7">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
