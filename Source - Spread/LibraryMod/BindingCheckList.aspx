<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="BindingCheckList.aspx.cs"
    Inherits="LibraryMod_BindingCheckList" MasterPageFile="~/LibraryMod/LibraryMaster.master" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <link href="Styles/css/Registration.css" rel="Stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="scriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <span class="fontstyleheader" style="color: Green;">Binding Check List</span>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <center>
                    <table class="maintablestyle" style="margin-left: 0px; font-family: Book Antiqua;
                        font-weight: bold; margin-bottom: 10px; margin-top: 10px; padding: 6px; height: auto;">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="updatepanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblclg" runat="server" Style="margin-left: -1px; width: 80px;" Text="College"></asp:Label>
                                        <asp:DropDownList ID="ddlclg" runat="server" Style="margin-left: 20px; width: 173px;"
                                            OnSelectedIndexChanged="ddlclg_OnSelectedChanged" AutoPostBack="true" CssClass="textbox ddlstyle ddlheight3">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lbllibrary" runat="server" Style="margin-left: -24px; width: 80px;"
                                            Text="Library"></asp:Label>
                                        <asp:DropDownList ID="ddllibrary" runat="server" Style="margin-left: 5px; width: 172px;"
                                            OnSelectedIndexChanged="ddllibrary_OnSelectedChanged" CssClass="textbox ddlstyle ddlheight3">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lbllib" runat="server" Style="margin-left: -38px; width: 80px;" Text="Book Type"></asp:Label>
                                        <asp:DropDownList ID="ddllib" runat="server" Style="margin-left: 2px; width: 170px;"
                                            OnSelectedIndexChanged="ddllib_OnSelectedChanged" CssClass="textbox ddlstyle ddlheight3">
                                            <asp:ListItem>Book</asp:ListItem>
                                            <asp:ListItem>Periodicals</asp:ListItem>
                                            <asp:ListItem>Project Book</asp:ListItem>
                                            <asp:ListItem>Non Book Material</asp:ListItem>
                                            <asp:ListItem>Question Bank</asp:ListItem>
                                            <asp:ListItem>Back Volume</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lbldate" runat="server" Style="margin-left: -52px; width: 80px;" Text="Date"></asp:Label>
                                        <asp:TextBox ID="Txtdate" runat="server" Style="margin-left: 5px; width: 110px;"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:CalendarExtender ID="Calender" runat="server" TargetControlID="Txtdate" Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpGo" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btnGo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_OnClick" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="updatepanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltextbox" runat="server" Style="margin-left: -3px; width: 80px"
                                            Text="File Name"></asp:Label>
                                        <asp:TextBox ID="textbox" runat="server" Style="margin-left: 1px; width: 200px" CssClass="textbox txtheight2"></asp:TextBox></ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="conditional">
                                    <ContentTemplate>
                                        <asp:FileUpload ID="fileupload1" runat="server" Style="margin-left: -1px; width: 165px;
                                            height: 26px" CssClass="textbox" />
                                          <asp:ImageButton ID="btnimport" runat="server" ImageUrl="~/LibImages/Import.jpg"
                                            Style="margin-left: -2px;" OnClick="btnimport_OnClick" />
                                     
                                     
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnimport" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel5" runat="server">
                                    <ContentTemplate>
                                        <fieldset id="bindbinreturn" runat="server" style="height: 22px; width: 213px; margin-left: 22px">
                                            <asp:RadioButtonList ID="rblbind" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                Style="margin-left: -2px; width: 230px" OnSelectedIndexChanged="rblbind_OnSelectedIndexedChanged">
                                                <asp:ListItem Selected="True">Binding</asp:ListItem>
                                                <asp:ListItem>Binding Return</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upSave" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/LibImages/save-Recovered.jpg"
                                            OnClick="btnSave_OnClick" Visible="false" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divtable" runat="server" visible="false">
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <center>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <asp:GridView ID="gridview1" runat="server" ShowFooter="false" AutoGenerateColumns="true" ShowHeader="true"
                                        Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="500"
                                        OnSelectedIndexChanged="gridview1_onselectedindexchanged" Width="980px" OnPageIndexChanging="gridview1_onpageindexchanged">
                                      
                                        <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                    </asp:GridView>
                                    <center>
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
                                           <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                                        </center>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
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
                                            <asp:UpdatePanel ID="updatepanelbtn4" runat="server">
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
    <%--progressBar for upSave--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upSave">
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
