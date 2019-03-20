<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="library Entry Correction.aspx.cs" Inherits="LibraryMod_library_Entry_Correction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <center>
        <span class="fontstyleheader" style="color: green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">Library Entry Correction</span></center>
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
            <ContentTemplate>
                <table class="maintablestyle" style="margin: 0px; font-family: Book Antiqua; font-weight: bold;
                    margin-bottom: 0px; margin-top: 8px; position: relative;" width="550px">
                    <tr>
                        <td>
                            <asp:Label ID="Lblcollege" runat="server" Text="College" Visible="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Visible="true" Width="252px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbltype" runat="server" Text="Type" Style="margin-left: 17px; margin-top: 2px"
                                Visible="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Visible="true" Width="139px" AutoPostBack="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                <%----%>
                                <%----%>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_from" runat="server" Visible="true" CssClass="textbox txtheight2"
                                Style="width: 185px; margin-left: 2px; margin-top: 2px;"></asp:TextBox>
                            <fieldset id="datefield" runat="server" visible="false" style="width: 279px; height: 26px;
                                margin-left: -15px; margin-top: 0px;">
                                <asp:CheckBox ID="chkredate" runat="server" AutoPostBack="true" OnCheckedChanged="chkredate_CheckedChanged" />
                                From
                                <asp:TextBox ID="txtfromdate" runat="server" Enabled="false" AutoPostBack="true"
                                    Width="80px" CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:CalendarExtender ID="calendetextenfordatext" TargetControlID="txtfromdate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                To
                                <asp:TextBox ID="txttodate" runat="server" Enabled="false" AutoPostBack="true" Width="80px"
                                    CssClass="textbox txtheight2"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttodate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </fieldset>
                        </td>
                        <td>
                            <asp:Label ID="Lbllang" runat="server" Text="Language" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddllang" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Width="108px" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="ddllang_SelectedIndexChanged"
                                Enabled="false">
                                <%--OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"--%>
                                <%----%>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpGo" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="Btnmess" runat="server" CssClass="fontbold" Style="width: 44px; height: 28px;
                                        margin-left: -46px; margin-top: 4px;" Visible="true" Text="Go" OnClick="Go_Click" /><%--OnClick="Go_Click"--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                        </td>
                        <td>
                            <asp:Label ID="Lblselectentry" runat="server" Style="width: 65px; margin-left: -364px;"
                                Text="SelectCorrectEntry" Visible="false"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlentry" runat="server" Style="width: 140px; margin-left: -400px;"
                                CssClass="textbox ddlstyle ddlheight3" Visible="false" AutoPostBack="True" OnSelectedIndexChanged="ddlentry_SelectedIndexChanged">
                                <%--OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"--%>
                                <%----%>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label_title" runat="server" Visible="false" Style="width: 65px; margin-left: -531px;"
                                Text="Title"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNew" runat="server" Visible="false" CssClass="textbox txtheight2"
                                Style="width: 200px; margin-left: -428px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" rowspan="4">
                            <fieldset style="width: 280px; height: 215px;">
                                <center>
                                    <asp:Label ID="Label4" runat="server" Text="Select Wrong Entries Here" Visible="true"></asp:Label>
                                </center>
                                <asp:Panel ID="pnlCollege" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                    Width="280px">
                                    <asp:CheckBoxList ID="cblCollege" runat="server" AutoPostBack="True">
                                        <%--OnSelectedIndexChanged="cblCollege_SelectedIndexChanged"--%>
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </fieldset>
                        </td>
                        <td>
                            <asp:CheckBox ID="Chkbytype" runat="server" Style="margin-left: -36px; margin-top: 2px"
                                Text="By Type" AutoPostBack="True" Checked="true" /><%--OnCheckedChanged="Chkbatch_CheckedChange"--%>
                        </td>
                        <td colspan="5">
                            <fieldset style="width: 341px; height: 26px; margin-left: -100px; margin-top: 0px;">
                                <asp:CheckBox ID="Chkaccno" runat="server" Text="Access No" AutoPostBack="True" OnCheckedChanged="Chkaccno_CheckedChange" /><%----%>
                                <asp:Label ID="Label3" runat="server" Text="From" Visible="true"></asp:Label>
                                <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                    Width="69px" Visible="true" Enabled="false"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftext_phno" runat="server" TargetControlID="TextBox2"
                                    FilterType="numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="Label1" runat="server" Text="To" Visible="true"></asp:Label>
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                    Width="69px" Visible="true" Enabled="false"></asp:TextBox><%--OnTextChanged="TextBox1_TextChanged"--%>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="TextBox1"
                                    FilterType="numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                            </fieldset>
                        </td>
                        <td colspan="2">
                            <fieldset style="width: 110px; height: 26px; margin-left: -35px;">
                                <asp:RadioButton ID="rdbyes" runat="server" Text="Yes" GroupName="yes" AutoPostBack="true" />
                                <asp:RadioButton ID="rdbno" runat="server" Text="No" GroupName="yes" AutoPostBack="true"
                                    Checked="true" />
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset style="width: 500px; height: 21px; margin-left: -37px; margin-top: -6px;">
                                <asp:RadioButton ID="rdbdept" runat="server" Text="Repalce Department" GroupName="Move"
                                    AutoPostBack="true" Checked="true" OnCheckedChanged="rdbprice_CheckedChange" />
                                <asp:RadioButton ID="Rdbref" runat="server" Text="Ref.Book" GroupName="Move" AutoPostBack="true"
                                    OnCheckedChanged="rdbprice_CheckedChange" />
                                <asp:RadioButton ID="rdbprice" runat="server" Text="Replace Price" GroupName="Move"
                                    AutoPostBack="true" OnCheckedChanged="rdbprice_CheckedChange" />
                                <asp:TextBox ID="txtprice" runat="server" CssClass="textbox  txtheight" AutoPostBack="true"
                                    Visible="false" Enabled="true"></asp:TextBox>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="Chklan" runat="server" Text="Title/Author Language" AutoPostBack="True"
                                OnCheckedChanged="Chklan_CheckedChange" Visible="false" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddllng" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Visible="false" Style="width: 100px; margin-left: -244px; margin-top: 2px;" Enabled="false"
                                AutoPostBack="True" OnSelectedIndexChanged="ddllng_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="Updel" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="Btndelete" runat="server" CssClass="fontbold" Style="width: 74px;
                                        height: 28px; margin-left: -40px; margin-top: 6px;" Visible="true" Text="Delete"
                                        OnClick="delete_Click" /><%--OnClick="Go_Click"--%>
                                    <asp:Button ID="btnreplace" runat="server" CssClass="fontbold" Style="width: 84px;
                                        height: 28px; margin-left: 2px; margin-top: 6px;" Visible="true" Text="Replace"
                                        OnClick="replace_Click" /><%--OnClick="Go_Click"--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <%-- <tr>
                </tr>
                <tr>
                </tr>--%>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%-------------------------------------------- alter msg---------------------------------------%>
        <asp:UpdatePanel ID="UpdatePanel16" runat="server">
            <ContentTemplate>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 100px;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 50px;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
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
                                                    OnClick="btnerrclose_Click" Text="ok" runat="server" />
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
    <%--progressBar for delete--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Updel">
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
    <%--progressBar for Go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpGo">
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
