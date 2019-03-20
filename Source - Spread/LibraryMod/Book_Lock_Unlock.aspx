<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Book_Lock_Unlock.aspx.cs" Inherits="LibraryMod_Book_Lock_Unlock" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();

            if (postBackElement.id == 'btn_Lock')
                $get('UpdateProgress1').style.display = 'block';
        }
        function EndRequest(sender, args) {
            if (postBackElement.id == 'btn_Lock')
                $get('UpdateProgress1').style.display = 'none';
        }

    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Book Lock/Unlock</span></div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 990px; height: auto">
                <div>
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <center>
                                            <div style="width: 1000px; font-family: Book Antiqua; font-weight: bold; height: auto">
                                                <table class="maintablestyle" style="height: auto; width: auto; margin-left: -16px;
                                                    margin-top: 10px; margin-bottom: 10px; padding: 6px;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblclg" runat="server" Text="College:">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="208px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbllibrary" runat="server" Text="Library:">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_dept" runat="server" Text="Department:">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_dept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="153px" AutoPostBack="True" OnSelectedIndexChanged="ddl_dept_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_Acron" runat="server" Text="Acronym:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Txt_BookAcr" runat="server" CssClass="textbox txtheight2" Style="width: 90px;
                                                                margin-left: -11px;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:RadioButtonList ID="rblbook" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                                OnSelectedIndexChanged="rblbook_Selected">
                                                                <asp:ListItem Value="0" Text="Locked" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Unlocked"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_type" runat="server" Text="Type:">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_type" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddl_type_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:CheckBox ID="Chk_AccNo" runat="server" AutoPostBack="true" Text="Access No:"
                                                                OnCheckedChanged="Chk_AccNo_CheckedChanged" />
                                                            From
                                                            <asp:TextBox ID="txt_from" runat="server" Enabled="false" Width="65px" CssClass="textbox txtheight2"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_from"
                                                                FilterType="Numbers">
                                                            </asp:FilteredTextBoxExtender>
                                                            To
                                                            <asp:TextBox ID="txt_to" runat="server" Enabled="false" Width="65px" CssClass="textbox txtheight2"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_to"
                                                                FilterType="Numbers">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpGo" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="Btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-top: 10px;"
                                                                        OnClick="btngo_Click" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </center>
        <br />
        <br />
        <center>
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="grdBookLock" runat="server" ShowFooter="false" AutoGenerateColumns="false"
                        Font-Names="book antiqua" togeneratecolumns="false" AllowPaging="true" PageSize="100"
                        OnPageIndexChanging="grdBookLock_onpageindexchanged" OnRowDataBound="grdBookLock_RowDataBound"
                        Width="900px">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>">
                                    </asp:Label></center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkenbl" runat="server" AutoPostBack="True" Style="text-align:center;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Access No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="120px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_accessno" runat="server" Text='<%#Eval("Access No") %>' Width="80px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Title" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_title" runat="server" Text='<%#Eval("Title") %>' Style="text-align: left;"
                                            Width="250px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Author" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_author" runat="server" Text='<%#Eval("Author") %>' Style="text-align: left;"
                                            Width="250px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Publisher" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_publish" runat="server" Text='<%#Eval("Publisher") %>' Style="text-align: left;"
                                            Width="200px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name of the student" HeaderStyle-BackColor="#0CA6CA"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_name" runat="server" Text='<%#Eval("Nameofthestudent") %>' Style="text-align: left;"
                                            Width="200px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Department Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_depname" runat="server" Text='<%#Eval("DepartmentName") %>' Style="text-align: left;"
                                            Width="200px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reason" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <center>
                                        <asp:TextBox ID="lbl_reason" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("Reason") %>'
                                            Height="24px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                   
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />
            <asp:UpdatePanel ID="UpCommon" runat="server">
                <ContentTemplate>
                    <div id="rptprint" runat="server" visible="false">
                        <asp:CheckBox ID="chkOldSearch" runat="server" AutoPostBack="true" Text="Include Old Search"
                            OnCheckedChanged="chkOldSearch_CheckedChanged" />
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                            Font-Size="Medium" CssClass="textbox txtheight2"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                            Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Width="60px" CssClass="textbox textbox1 btn2" />
                        <asp:Button ID="btn_Lock" Width="85px" runat="server" Style="margin-left: 35px; margin-top: 2px;"
                            CssClass="textbox btn2" Text="Lock" Visible="false" OnClick="btn_Lock_Click"
                            BackColor="LightGreen" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                    </div>
                </ContentTemplate>
                   <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
            </Triggers>

            </asp:UpdatePanel>
        </center>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <div id="Divalert" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblalertmsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnerrclose1" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnerrclose1_Click" Text="Ok" runat="server" />
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
    <%--progressBar for GO--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
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
    <%--progressBar for UpCommon--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpCommon">
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
