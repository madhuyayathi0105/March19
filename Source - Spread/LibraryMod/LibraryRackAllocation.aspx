<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="LibraryRackAllocation.aspx.cs" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" Inherits="LibraryMod_LibraryRackAllocation" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">


        function display() {
            document.getElementById('MainContent_errmsg').innerHTML = "";
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();

            if (postBackElement.id == 'btngo' || postBackElement.id == 'BtnAddRack' || postBackElement.id == 'BtnRackEntryGo' || postBackElement.id == 'BtnRackSave' || postBackElement.id == 'BtnRackUpdate' || postBackElement.id == 'BtnRackDelete' || postBackElement.id == 'BtnPosSave' || postBackElement.id == 'BtnPosDel' || postBackElement.id == 'BtnPosUpdate')
                $get('UpdateProgress1').style.display = 'block';
        }
        function EndRequest(sender, args) {
            if (postBackElement.id == 'btngo' || postBackElement.id == 'BtnAddRack' || postBackElement.id == 'BtnRackEntryGo' || postBackElement.id == 'BtnRackSave' || postBackElement.id == 'BtnRackUpdate' || postBackElement.id == 'BtnRackDelete' || postBackElement.id == 'BtnPosSave' || postBackElement.id == 'BtnPosDel' || postBackElement.id == 'BtnPosUpdate')
                $get('UpdateProgress1').style.display = 'none';
        }        
    </script>
    <style type="text/css">
        .fontbold
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        
        .fontnormal
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
    </style>
    <style>
        .fontblack
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: Black;
        }
        .fontcolorb
        {
            color: Green;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <asp:Label ID="Label4" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                position: relative;" Text="Rack Master" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
        </div>
        <div style="color: black; font-family: Book Antiqua; font-weight: bold; height: 50px;
            width: 800px; margin: 0px; margin-top: 15px; margin-bottom: 15px; position: relative;
            text-align: left;" class="maintablestyle">
            <table style="margin-top: 8px;">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddl_collegename_OnSelectedIndexChanged" Style="width: 170px;
                                    height: 27px;" AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lbllibrary" runat="server" Text="Library">
                                </asp:Label>
                                <asp:DropDownList ID="ddlLibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="width: 170px; height: 27px;" AutoPostBack="True">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lbl_RackNo" runat="server" Text="Rack No"></asp:Label>
                                <asp:TextBox ID="txt_RackNo" runat="server" MaxLength="50" Style="width: 80px; height: 17px;"
                                    CssClass="textbox txtheight2"> </asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftxt_RackNo" runat="server" TargetControlID="txt_RackNo"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="   ">
                                </asp:FilteredTextBoxExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpGoAdd" runat="server">
                            <ContentTemplate>
                                <asp:ImageButton ID="btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_Click" />
                                <asp:ImageButton ID="BtnAddRack" runat="server" ImageUrl="~/LibImages/Add.jpg" OnClick="BtnAddRack_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="UpdatePanel36" runat="server">
            <ContentTemplate>
                <div id="divspread" runat="server" visible="false" style="width: 1000px; overflow: auto;
                    background-color: White; border-radius: 10px;">
                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="GrdRackMaster" runat="server" ShowFooter="false" Width="700px" ShowHeader="true"
                        AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true"
                        toGenerateColumns="true" AllowPaging="true" PageSize="200" OnPageIndexChanging="GrdRackMaster_OnPageIndexChanged"
                        OnRowCreated="GrdRackMaster_OnRowCreated" OnRowDataBound="GrdRackMaster_OnRowDataBound"
                        OnSelectedIndexChanged="GrdRackMaster_SelectedIndexChanged">
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                       
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="GrdRackMaster" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel44" runat="server">
            <ContentTemplate>
                <div id="print" runat="server" visible="false">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="Red" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                                    OnClick="btnExcel_Click" />
                                <asp:ImageButton ID="btnprintmasterhed" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                    OnClick="btnprintmaster_Click" />
                                      <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
               <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmasterhed" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <%--PopUp for RackMaster Entry--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel43" runat="server">
            <ContentTemplate>
                <div id="popwindow_RackEntry" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 460px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; font-family: Book Antiqua; font-weight: bold;
                        height: 500px; width: 950px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                        border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Rack Master Entry</span>
                            </div>
                        </center>
                        <br />
                        <table class="maintablestyle" style="width: 800px;">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="Lbllib" runat="server" Text="Library Name">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlMaster_Lib" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                AutoPostBack="True" Style="width: 150px; height: 27px; margin-left: 6px;">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblNoofRack" runat="server" Text="Total No Of Racks"></asp:Label>
                                            <asp:TextBox ID="txt_TotalRack" MaxLength="3" runat="server" Style="width: 140px;
                                                height: 15px;" CssClass="textbox txtheight2"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftext_TotalRack" runat="server" TargetControlID="txt_TotalRack"
                                                FilterType="numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblRackAcr" runat="server" Text="Rack Acronym"></asp:Label>
                                            <asp:TextBox ID="txt_RackAcr" runat="server" MaxLength="50" Style="width: 140px;
                                                height: 15px;" CssClass="textbox txtheight2" OnTextChanged="txtRackAcr_OnTextChanged"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftext_RackAcr" runat="server" TargetControlID="txt_RackAcr"
                                                FilterType="UppercaseLetters,LowercaseLetters" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="LblRackStNo" runat="server" Text="Rack Start No"></asp:Label>
                                            <asp:TextBox ID="txt_RackstNo" runat="server" MaxLength="5" Style="width: 140px;
                                                height: 15px; margin-left: 32px;" CssClass="textbox txtheight2"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftext_RackstNo" runat="server" TargetControlID="txt_RackstNo"
                                                FilterType="numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpRackEntryGo" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="BtnRackEntryGo" runat="server" ImageUrl="~/LibImages/Go.jpg"
                                                OnClick="BtnRackEntryGo_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="divSpreadRack" runat="server" visible="false" style="width: 800px; background-color: White;
                            border-radius: 10px;">
                            <asp:GridView ID="GrdRackEntry" runat="server" ShowFooter="false" Width="700px" AutoGenerateColumns="false"
                                Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true" toGenerateColumns="false"
                                AllowPaging="true" PageSize="10" OnRowDataBound="GrdRackEntry_OnRowDataBound">
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rack Number" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="150px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_GrdRckNo" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("GrdRckNo") %>'
                                                    Height="15px" Width="60px" MaxLength="7" Style="text-align: right;"></asp:TextBox></center>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rack Max Capacity" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_GrdRckMaxCap" runat="server" CssClass="  textbox txtheight"
                                                    Text='<%#Eval("GrdRckMaxCap") %>' MaxLength="7" Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderGrdRckMaxCap" runat="server" TargetControlID="txt_GrdRckMaxCap"
                                                FilterType="Numbers,Custom">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No Of Shelves in Rack" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_GrdNoOfShlfinRck" runat="server" CssClass="  textbox txtheight"
                                                    Text='<%#Eval("GrdNoOfShlfinRck") %>' MaxLength="7" Height="15px" Width="60px"
                                                    Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderGrdNoOfShlfinRck" runat="server" TargetControlID="txt_GrdNoOfShlfinRck"
                                                FilterType="Numbers,Custom">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Available Copy" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_AvailableCopy" runat="server" Text='<%#Eval("AvailableCopy") %>'
                                                    Style="text-align: right;" Width="60px"></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Available Copy" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_GrdRckAvlCopy" runat="server" CssClass="  textbox txtheight"
                                                    Text='<%#Eval("GrdRckAvlCopy") %>' Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderGrdRckAvlCopy" runat="server" TargetControlID="txt_GrdRckAvlCopy"
                                                FilterType="Numbers,Custom">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="selectchk" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add Shelf" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_AddShelf" Text="Shelf" OnClick="btn_AddShelf_click" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpRackButtons" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="BtnRackSave" runat="server" Enabled="false" ImageUrl="~/LibImages/save.jpg"
                                                OnClick="BtnRackSave_Click" />
                                            <asp:ImageButton ID="BtnRackUpdate" runat="server" Enabled="false" ImageUrl="~/LibImages/update.jpg"
                                                OnClick="BtnRackUpdate_Click" />
                                            <asp:ImageButton ID="BtnRackDelete" runat="server" Enabled="false" ImageUrl="~/LibImages/delete.jpg"
                                                OnClick="BtnRackDelete_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--PopUp for Shelf Entry--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel37" runat="server">
            <ContentTemplate>
                <div id="DivShelfEntry" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 460px;"
                        OnClick="imagebtnShlfpopclose_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; font-weight: bold; font-family: Book Antiqua;
                        height: 500px; width: 950px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                        border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Shelf Entry</span>
                            </div>
                        </center>
                        <br />
                        <table class="maintablestyle" style="width: 800px;">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblShlfRackNo" runat="server" Text="Rack No"></asp:Label>
                                            <asp:TextBox ID="txt_ShlfRackNo" runat="server" CssClass="textbox txtheight2" Style="width: 100px;
                                                height: 15px; margin-left: 49px;"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblShlfRackMax" runat="server" Text="Rack Max Capacity"></asp:Label>
                                            <asp:TextBox ID="txt_ShlfRackMax" runat="server" CssClass="textbox txtheight2" Style="width: 100px;
                                                height: 15px; margin-left: 3px;"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblShlfNoOfRack" runat="server" Text="No. Of shelves In Rack"></asp:Label>
                                            <asp:TextBox ID="txt_NORinShlf" runat="server" Style="width: 100px; height: 15px;"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblShlfAcr" runat="server" Text="Shelf Acronym"></asp:Label>
                                            <asp:TextBox ID="txt_ShlfAcr" runat="server" Style="width: 100px; height: 15px;"
                                                CssClass="textbox txtheight2" OnTextChanged="txt_ShlfAcr_OnTextChanged"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_ShlfAcr"
                                                FilterType="UppercaseLetters,LowercaseLetters" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblShlfStNo" runat="server" Text="Shelf Start Number"></asp:Label>
                                            <asp:TextBox ID="txt_ShlfStNo" runat="server" Style="width: 100px; height: 15px;"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_ShlfStNo"
                                                FilterType="Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpShelfGo" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="BtnShlfGo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="BtnShlfGo_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="divShelfSpread" runat="server" visible="false" style="width: 800px; overflow: auto;
                            background-color: White; border-radius: 10px;">
                            <asp:GridView ID="GrdShelfEntry" runat="server" ShowFooter="false" Width="700px"
                                AutoGenerateColumns="false" Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true"
                                toGenerateColumns="false" AllowPaging="true" PageSize="10" OnRowDataBound="GrdShelfEntry_OnRowDataBound">
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shelf Number" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="150px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_GrdShlfNo" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("GrdShlfNo") %>'
                                                    Height="15px" Width="60px" MaxLength="7" Style="text-align: right;"></asp:TextBox></center>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shelf Max Capacity" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_GrdShlfMaxCap" runat="server" CssClass="  textbox txtheight"
                                                    Text='<%#Eval("GrdShlfMaxCap") %>' MaxLength="7" Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderGrdShlfMaxCap" runat="server" TargetControlID="txt_GrdShlfMaxCap"
                                                FilterType="Numbers,Custom">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No Of Position in Shelf" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_GrdNoOfPosinShlf" runat="server" CssClass="  textbox txtheight"
                                                    Text='<%#Eval("GrdNoOfPosinShlf") %>' MaxLength="7" Height="15px" Width="60px"
                                                    Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderGrdNoOfPosinShlf" runat="server" TargetControlID="txt_GrdNoOfPosinShlf"
                                                FilterType="Numbers,Custom">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Available Copy" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_AvailableCopy" runat="server" Text='<%#Eval("AvailableCopy") %>'
                                                    Style="text-align: right;" Width="60px"></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Available Copy" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_GrdRckAvlCopy" runat="server" CssClass="  textbox txtheight"
                                                    Text='<%#Eval("GrdRckAvlCopy") %>' Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderGrdRckAvlCopy" runat="server" TargetControlID="txt_GrdRckAvlCopy"
                                                FilterType="Numbers,Custom">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="selectchk" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Positon" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_AddPos" Text="Add Positon" OnClick="btn_AddPos_click" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpShlfButtons" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="BtnShlfSave" runat="server" Enabled="false" ImageUrl="~/LibImages/save.jpg"
                                                OnClick="BtnShlfSave_Click" />
                                            <asp:ImageButton ID="BtnShlfDel" runat="server" Enabled="false" ImageUrl="~/LibImages/delete.jpg"
                                                OnClick="BtnShlfDel_Click" />
                                            <asp:ImageButton ID="BtnShlfUpdate" runat="server" Enabled="false" ImageUrl="~/LibImages/update.jpg"
                                                OnClick="BtnShlfUpdate_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <%-- </asp:Panel>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--PopUp for Position Entry--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel38" runat="server">
            <ContentTemplate>
                <div id="DivPosEntry" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 460px;"
                        OnClick="imagebtnPospopclose_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 500px; font-weight: bold; font-family: Book Antiqua;
                        width: 950px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Position Entry</span>
                            </div>
                        </center>
                        <br />
                        <table class="maintablestyle" style="width: 850px;">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblPosShelf" runat="server" Text="Shelf No"></asp:Label>
                                            <asp:TextBox ID="Txt_PosShlfNo" runat="server" Style="width: 100px; height: 15px;
                                                margin-left: 67px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblPosSMC" runat="server" Text="Shelf Max Capacity"></asp:Label>
                                            <asp:TextBox ID="Txt_PosShlfSMC" runat="server" Style="width: 100px; height: 15px;
                                                margin-left: 21px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblNOPShlf" runat="server" Text="No. Of Position In Shelf"></asp:Label>
                                            <asp:TextBox ID="Txt_NOPShlf" runat="server" Style="width: 100px; height: 15px;"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="LblPosAcr" runat="server" Text="Position Acronym"></asp:Label>
                                            <asp:TextBox ID="txt_PosAcr" runat="server" Style="width: 100px; height: 15px;" CssClass="textbox txtheight2"
                                                OnTextChanged="txt_PosAcr_OnTextChanged"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_PosAcr"
                                                FilterType="UppercaseLetters,LowercaseLetters" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="LblPosStNo" runat="server" Text="Position Start Number"></asp:Label>
                                            <asp:TextBox ID="txt_PosStNo" runat="server" Style="width: 100px; height: 15px;"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_PosStNo"
                                                FilterType="Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpPosGo" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="BtnPosGo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="BtnPosGo_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="divSpreadPos" runat="server" visible="false" style="width: 800px; overflow: auto;
                            background-color: White; border-radius: 10px;">
                            <asp:GridView ID="GrdPosEntry" runat="server" ShowFooter="false" Width="700px" AutoGenerateColumns="false"
                                Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true" toGenerateColumns="false"
                                AllowPaging="true" PageSize="10">
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Position Number" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="150px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_GrdPosNo" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("GrdPosNo") %>'
                                                    Height="15px" Width="60px" MaxLength="7" Style="text-align: right;"></asp:TextBox></center>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Max Capacity" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="200px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_GrdPosMaxCap" runat="server" CssClass="  textbox txtheight"
                                                    Text='<%#Eval("GrdPosMaxCap") %>' MaxLength="7" Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextenderGrdPosMaxCap" runat="server" TargetControlID="txt_GrdPosMaxCap"
                                                FilterType="Numbers,Custom">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Available Copy" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_AvailableCopy" runat="server" Text='<%#Eval("AvailableCopy") %>'
                                                    Style="text-align: right;" Width="60px"></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                            <%--<FarPoint:FpSpread ID="SpreadPositionEntry" runat="server" Visible="true" BorderStyle="Solid"
                                BorderWidth="0px" Width="800px" Style="overflow: auto; border: 0px solid #999999;
                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                class="spreadborder">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>--%>
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpPosButtons" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="BtnPosSave" runat="server" Enabled="false" ImageUrl="~/LibImages/save.jpg"
                                                OnClick="BtnPosSave_Click" />
                                            <asp:ImageButton ID="BtnPosDel" runat="server" Enabled="false" ImageUrl="~/LibImages/delete.jpg"
                                                OnClick="BtnPosDel_Click" />
                                            <asp:ImageButton ID="BtnPosUpdate" runat="server" Enabled="false" ImageUrl="~/LibImages/update.jpg"
                                                OnClick="BtnPosUpdate_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <%-- </asp:Panel>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel39" runat="server">
            <ContentTemplate>
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
                                                <asp:UpdatePanel ID="UpdatePanel32" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                            width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
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
    <%--Pop For Delete --%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel40" runat="server">
            <ContentTemplate>
                <div id="SureDivDeleteRack" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Label ID="LblDel" runat="server" Text="Are You Sure to Delete this Rack?" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpDelRackYes" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnDeleteRack_yes" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                            width: 65px;" OnClick="btn_DeleteRackYes_Click" Text="yes" runat="server" />
                                                        <asp:Button ID="btnDelRack_no" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                            width: 65px;" OnClick="btn_DeleteRackNo_Click" Text="no" runat="server" />
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
    <%--Pop For Deleteshelf --%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel41" runat="server">
            <ContentTemplate>
                <div id="SureDivDeleteShelf" runat="server" visible="false" style="height: 100%;
                    z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0; left: 0px;">
                    <center>
                        <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblShelf" runat="server" Text="Are You Sure to Delete this Rack?"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpDelShlefYes" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btn_DeleteShelfYes" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                            width: 65px;" OnClick="btn_DeleteShelfYes_Click" Text="yes" runat="server" />
                                                        <asp:Button ID="btn_DeleteShelfNo" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                            width: 65px;" OnClick="btn_DeleteShelfNo_Click" Text="no" runat="server" />
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
    <%--Pop For Delete Position  --%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel42" runat="server">
            <ContentTemplate>
                <div id="SureDivDeletePosition" runat="server" visible="false" style="height: 100%;
                    z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0; left: 0px;">
                    <center>
                        <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblPos" runat="server" Text="Are You Sure to Delete this Position?"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpDelPosYes" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btn_DeletePosYes" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                            width: 65px;" OnClick="btn_DeletePosYes_Click" Text="yes" runat="server" />
                                                        <asp:Button ID="btn_DeletePosNo" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                            width: 65px;" OnClick="btn_DeletePosNo_Click" Text="no" runat="server" />
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
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpRackEntryGo">
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
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpRackButtons">
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
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpShelfGo">
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
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpShlfButtons">
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
    <center>
        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="UpPosGo">
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
    <center>
        <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="UpPosButtons">
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
    <center>
        <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="UpDelRackYes">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender8" runat="server" TargetControlID="UpdateProgress8"
            PopupControlID="UpdateProgress8">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress9" runat="server" AssociatedUpdatePanelID="UpDelShlefYes">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender9" runat="server" TargetControlID="UpdateProgress9"
            PopupControlID="UpdateProgress9">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress10" runat="server" AssociatedUpdatePanelID="UpDelPosYes">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender10" runat="server" TargetControlID="UpdateProgress10"
            PopupControlID="UpdateProgress10">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
