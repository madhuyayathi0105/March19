<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="graduityeligibility.aspx.cs" Inherits="graduityeligibility" %>

<%@ Register Src="~/Usercontrols/GridPrintMaster.ascx" TagName="GridPrintMaster"
    TagPrefix="InsproplusGrid" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <%-- <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">
      
    </script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <br />
            <center>
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: #008000">Set Staff Gratuity</span></div>
                    </center>
                    <asp:UpdatePanel ID="up1" runat="server">
                        <ContentTemplate>
                            <div class="maindivstyle" style="width: 1000px; height: auto;">
                                <br />
                                <div>
                                    <center>
                                        <table class="maintablestyle" width="400px">
                                            <tr align="center">
                                                <td>
                                                    <asp:Label ID="lblcollege" runat="server" Text="College"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox1 ddlheight3" Width="250px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_Change">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        </br>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:checkbox ID="cbgross" runat="server" Checked="false" AutoPostBack="true" OnCheckedChanged="cbgross_changed" Text="Gross Pay" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium;" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_include" runat="server" Text="Common" Style="font-weight: bold;
                                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtcommon" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                                Enabled="true" Style="font-weight: bold; width: 120px; font-family: book antiqua;
                                                                font-size: medium;">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel1" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                                                Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                                                position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                                                height: 105px; width: 128px;">
                                                                <asp:CheckBox ID="cbcommon" runat="server" Text="Select All" OnCheckedChanged="cbcommon_CheckedChange"
                                                                    AutoPostBack="true" />
                                                                <asp:CheckBoxList ID="cblcommon" runat="server" OnSelectedIndexChanged="cblcommon_SelectedIndexChange"
                                                                    AutoPostBack="true">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtcommon"
                                                                PopupControlID="Panel1" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_allow" runat="server" Text="Allowance" Style="font-weight: bold;
                                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_allow" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                                Enabled="true" Style="font-weight: bold; width: 120px; font-family: book antiqua;
                                                                font-size: medium;">--Select--</asp:TextBox>
                                                            <asp:Panel ID="P6" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                                border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                                box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                                                width: 128px;">
                                                                <asp:CheckBox ID="cb_allow" runat="server" Text="Select All" OnCheckedChanged="cb_allow_CheckedChange"
                                                                    AutoPostBack="true" />
                                                                <asp:CheckBoxList ID="cbl_allow" runat="server" OnSelectedIndexChanged="cbl_allow_SelectedIndexChange"
                                                                    AutoPostBack="true">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_allow"
                                                                PopupControlID="P6" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_sel" runat="Server" Text="Go" OnClick="btn_selection" CssClass="textbox textbox1 btn2" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </center>
                                    <center>
                                        <asp:UpdatePanel ID="upgo" runat="server">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                                <div id="div1" runat="server" visible="false" style="border-radius: 10px; overflow: auto;">
                                                    <asp:GridView ID="grdgratuity" Width="300px" runat="server" ShowFooter="false" AutoGenerateColumns="false"
                                                        HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" toGenerateColumns="false"
                                                        ShowHeaderWhenEmpty="true" OnRowDataBound="grdgratuity_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Gratuity Calculation Heads">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="gratuity" runat="server" Text='<%#Eval("gratuityCal") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="grdgratuity" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </center>
                                    </br>
                                    </br>

                                    <center>
                                    <asp:Button ID="btn_Set" runat="server" Visible="false" Text="Set" OnClick="btn_setclick" CssClass="textbox textbox1 btn2"  />
                                    </center>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </center>
        </div>
    </body>
</asp:Content>
