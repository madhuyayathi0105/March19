<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentTimeTable.aspx.cs" Inherits="StudentTimeTable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 370px;
        }
    </style>
    <script type="text/javascript">
        //
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();
            }
        }

        function EndRequestHandler(sender, args) {
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.hide();
            }
        }
    </script>
    <style type="text/css">
        .floats
        {
            height: 26px;
        }
        .CenterPB
        {
            position: absolute;
            left: 50%;
            top: 50%;
            margin-top: -20px;
            margin-left: -20px;
            width: auto;
            height: auto;
        }
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 500px;
            min-height: 100px;
            max-height: 150px;
            top: 100px;
            left: 150px;
        }
        .modalPopup1
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 700px;
            min-height: 100px;
            max-height: 250px;
            overflow: scroll;
            top: 100px;
            left: 150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green" Text="Semester Time Table"></asp:Label>
    </center>
    <br />
    <asp:UpdatePanel ID="udptimetable" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="udptimetable">
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
            <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                PopupControlID="UpdateProgress1">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlmsgboxupdate" runat="server" CssClass="modalPopup1" Style="display: none;
                height: 200; width: 400; left: auto; top: 30px">
                <table width="100%">
                    <tr class="topHandle">
                        <td colspan="2" align="left" runat="server" id="tdCaption">
                            <asp:Label ID="lblCaption" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                                Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 90px" valign="middle" align="center">
                            <asp:Image ID="imgInfo" runat="server" ImageUrl="~/Info-48x48.png" />
                        </td>
                        <td valign="middle" align="left">
                            <asp:Label ID="Label1" Text="Do You want to Allow?" runat="server" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnuupd" runat="server" Text="Make Combine Class" OnClick="btnupOk_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                            <asp:Button ID="btnuCancel" runat="server" Text="Cancel" OnClick="btnupCancel_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:ModalPopupExtender ID="mpemsgboxupdate" runat="server" TargetControlID="hfupdate"
                PopupControlID="pnlmsgboxupdate">
            </asp:ModalPopupExtender>
            <asp:HiddenField runat="server" ID="hfupdate" />
            <center>
                <table style="width: 900px; height: 70px; background-color: #0CA6CA;">
                    <td>
                        <asp:Label ID="lblcolg" runat="server" Text="College " Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcolg" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="collook_load">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbatch" runat="server" Text="Batch " Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Width="80px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Text="Degree " Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Width="80" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Text="Branch " Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td id="tdbranch" runat="server" colspan="2">
                        <asp:DropDownList ID="ddlbranch" runat="server" Width="220px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsem" runat="server" Width="50px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblsec" runat="server" Text="Sec " Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsec" runat="server" Width="50px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbltimetable" runat="server" Text="Time Table Name" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Width="150"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltimetable" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddltimetable_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="tdTime" runat="server">
                            <asp:TextBox ID="txttimetable" runat="server" Font-Bold="true" Font-Names="Book Antiqua"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txttimetable"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" -()_">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Label ID="lbldate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdate" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                AutoPostBack="true" Width="80px" OnTextChanged="txtdate_TextChanged"></asp:TextBox>
                            <asp:CalendarExtender ID="CalToDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtdate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btngo_Click" />
                        </td>
                    </tr>
                </table>
            </center>
            <br />
            <center>
                <asp:Label ID="errmsg" ForeColor="Red" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </center>
            <br />
            <center>
                <%--<div style="height: 400px; width: 900px; overflow: auto;">--%>
                <center>
                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="gview" runat="server" ShowHeader="false" Height="400" OnRowDataBound="gview_OnRowDataBound"
                        OnSelectedIndexChanged="gview_onselectedindexchanged" OnRowCreated="OnRowCreated"
                        Width="900">
                        <Columns>
                        </Columns>
                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                        <RowStyle ForeColor="#333333" />
                        <%--<SelectedRowStyle BackColor="#339966" Font-Bold="True" />--%>
                    </asp:GridView>
                </center>
                <%--</div>--%>
                <asp:Button ID="btnsave" Text="Save" runat="server" OnClick="btnsave_Click" Font-Bold="true"
                    Font-Names="Book Antiqua" />
                <asp:Button ID="btndelete" Text="Clear" runat="server" OnClick="btndelete_Click"
                    Font-Bold="true" Font-Names="Book Antiqua" Visible="False" />
                <asp:Button ID="btnclassadvisor" runat="server" Text="Class Advisor" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnclassadvisor_Click"
                    Visible="False" />
                <br />
                <br />
                <table style="text-align: left;">
                    <tr>
                        <td>
                            <asp:Label ID="lblday" runat="server" Text="Day :" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblday1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbltimings" runat="server" Text="Timings" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblfromtime" runat="server" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbltotime" runat="server" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <asp:GridView ID="gviewstaff" runat="server" ShowHeader="false" Width="1000">
                        <Columns>
                        </Columns>
                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                        <RowStyle ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                    </asp:GridView>
                </center>
                <div id="advdiv" runat="server" style="height: auto; overflow: auto;">
                    <asp:GridView ID="gviewclassadv" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lblclssno" runat="server" Text='<%#Container.DataItemIndex+1 %>' /></center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Staff Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblclsstafnme" runat="server" Text='<%#Eval("stfnme") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Staff Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblclsstfcode" runat="server" Text='<%#Eval("stfcode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remove">
                                <ItemTemplate>
                                    <asp:Button ID="btnrmve" Text="Remove" OnClick="advRemoveOnClick" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                        <RowStyle ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                    </asp:GridView>
                </div>
                <br />
                <br />
                <table>
                    <tr>
                        <td style="width: 100px;">
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                        </td>
                        <td style="width: 180px;">
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td style="width: 115px;">
                            <asp:Button ID="btnexcel" runat="server" OnClick="btnexcel_Click" Text="Export Excel"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                        </td>
                        <%--<td style="width:62px;">
                            <asp:Button ID="btnprint" runat="server" Text="Print" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true" />
                            
                            </td>--%>
                        <td style="width: 60px;">
                            <asp:Button ID="btnPDF" runat="server" Text="Print" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true" OnClick="btnPDF_Click" />
                            <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </td>
                    </tr>
                </table>
                <center>
                    <div id="div2" runat="server" visible="false" style="height: 400em; z-index: 2000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div9" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label9" runat="server" Text="Do You Want Remove Subject" Style="color: Red;"
                                                    Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="Btnyes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                        OnClick="btnyes_Click" Text="Yes" runat="server" />
                                                    <asp:Button ID="Btnnoremove" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                        OnClick="Btnnoremove_Click" Text="No" runat="server" />
                                                    <asp:Button ID="Btnalter" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                        OnClick="Btnalter_Click" Text="Alter" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <asp:Panel ID="treepanel" runat="server" BorderStyle="Dotted" BorderColor="ActiveBorder"
                    Height="600px">
                    <center>
                        <div id="spcellClickPopup" runat="server" visible="false" style="height: 50em; z-index: 2000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: fixed; top: 0;
                            left: 0;">
                            <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 38px; margin-left: 500px;"
                                OnClick="spcellClickPopupclose_Click" />
                            <br />
                            <br />
                            <div class="subdivstyle" style="background-color: White; overflow: auto; width: 1050px;
                                height: 550px;" align="center">
                                <br />
                                <table>
                                    <tr>
                                        <td style="height: 400px; margin: 0px;">
                                            <asp:TreeView ID="subjtree" runat="server" AutoPostBack="true" BackColor="White"
                                                Height="300px" Width="300px" SelectedNodeStyle-ForeColor="Red" HoverNodeStyle-BackColor="LightBlue"
                                                OnSelectedNodeChanged="subjtree_SelectedNodeChanged" Font-Names="Book Antiqua"
                                                Font-Size="Small" ForeColor="Black" Style="overflow: scroll; border: 1px solid black;
                                                height: 300px; margin: 0px; margin-top: 10px; width: 300px;">
                                            </asp:TreeView>
                                        </td>
                                        <td style="width: 500px; height: 400px; margin: 0px;">
                                            <table>
                                                <tr style="width: auto; height: auto; margin: 0px; margin-bottom: 10px; margin-top: 8px;">
                                                    <td>
                                                        <asp:Label ID="lblmulstaff" runat="server" Text="For Mulitple Staff Selection Only"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtmulstaff" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                            Width="100px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <asp:Panel ID="pmulstaff" runat="server" CssClass="multxtpanel" BackColor="White"
                                                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                                            Height="180px" Width="220px">
                                                            <asp:CheckBox ID="chkmulstaff" runat="server" Font-Bold="True" OnCheckedChanged="chkmulstaff_ChekedChange"
                                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                            <asp:CheckBoxList ID="chkmullsstaff" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chkmullsstaff_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtmulstaff"
                                                            PopupControlID="pmulstaff" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnmulstaff" runat="server" Text="Ok" OnClick="btnmulstaff_Click"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:GridView ID="gview1" runat="server" OnDataBound="gview1_OnDataBound" AutoGenerateColumns="false"
                                                            Width="565">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Subject Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsubnme" runat="server" Text='<%#Eval("subnme") %>' />
                                                                        <asp:Label ID="lblsubtag" runat="server" Visible="false" Text='<%#Eval("subtag") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Staff Name">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txt_staffname" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                                            Width="200px" Font-Bold="True" Visible="true">---Select---</asp:TextBox>
                                                                        <asp:Panel ID="panel_Category" runat="server" CssClass="multxtpanel" Height="150px"
                                                                            Width="200px" Style="text-align: left;">
                                                                            <asp:CheckBox ID="cb_staffname" runat="server" Text="Select All" AutoPostBack="True"
                                                                                TextAlign="Right" OnCheckedChanged="cb_Category_CheckedChanged" Style="text-align: left;" />
                                                                            <asp:CheckBoxList ID="cbl_staffname" runat="server" AutoPostBack="True" TextAlign="Right"
                                                                                OnSelectedIndexChanged="cbl_staffname_SelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_staffname"
                                                                            PopupControlID="panel_Category" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Staff Name" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlstaffnme" Width="240" AutoPostBack="true" runat="server">
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="lblddltag" runat="server" Visible="false" Text='<%#Eval("ddltag") %>' />
                                                                        <asp:Label ID="lblddlvalue" runat="server" Visible="false" Text='<%#Eval("ddlvalue") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Remove">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btnremove" OnClick="RemoveOnClick" runat="server" Text="Remove" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                                            <FooterStyle BackColor="White" ForeColor="#333333" />
                                                            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                            <RowStyle ForeColor="#333333" />
                                                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:GridView ID="gridSelTT" runat="server" HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="false"
                                                            Width="300px">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Day/Hour">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDay" runat="server" Text='<%#Eval("Day") %>'></asp:Label>
                                                                        <asp:Label ID="lblDayVal" runat="server" Text='<%#Eval("DayVal") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="1">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlH1" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                            <asp:ListItem></asp:ListItem>
                                                                            <asp:ListItem Value="1">Allot</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="2">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlH2" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                            <asp:ListItem></asp:ListItem>
                                                                            <asp:ListItem Value="1">Allot</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="3">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlH3" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                            <asp:ListItem></asp:ListItem>
                                                                            <asp:ListItem Value="1">Allot</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="4">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlH4" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                            <asp:ListItem></asp:ListItem>
                                                                            <asp:ListItem Value="1">Allot</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="5">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlH5" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                            <asp:ListItem></asp:ListItem>
                                                                            <asp:ListItem Value="1">Allot</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="6">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlH6" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                            <asp:ListItem></asp:ListItem>
                                                                            <asp:ListItem Value="1">Allot</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="7">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlH7" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                            <asp:ListItem></asp:ListItem>
                                                                            <asp:ListItem Value="1">Allot</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="8">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlH8" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                            <asp:ListItem></asp:ListItem>
                                                                            <asp:ListItem Value="1">Allot</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="9" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlH9" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                            <asp:ListItem></asp:ListItem>
                                                                            <asp:ListItem Value="1">Allot</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="10" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlH10" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                            <asp:ListItem></asp:ListItem>
                                                                            <asp:ListItem Value="1">Allot</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                        </td>
                                        <td class="style12">
                                            <asp:CheckBox ID="chkappend" runat="server" Text="Append to the schedule List" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="blue" Visible="False" />
                                            <asp:CheckBox ID="chk_multisubj" runat="server" OnCheckedChanged="chk_multisubj_CheckedChanged"
                                                Text="Multiple Staffs" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                ForeColor="blue" AutoPostBack="true" Visible="False" />
                                            <asp:TextBox ID="txtmultisubj" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                Width="100px" Style="top: 1342px; left: 765px; position: absolute; font-family: 'Book Antiqua';"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                            <asp:Panel ID="pnlmultisubj" runat="server" Height="400px" Width="300px" CssClass="MultipleSelectionDDL">
                                                <asp:CheckBoxList ID="chklistmultisubj" runat="server" CssClass="font" Font-Bold="True"
                                                    OnSelectedIndexChanged="chklistmultisubj_selectedindetxchange" AutoPostBack="true"
                                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtmultisubj"
                                                PopupControlID="pnlmultisubj" Position="Bottom">
                                            </asp:PopupControlExtender>
                                            <asp:Button ID="btnOk" runat="server" Text="Ok" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Height="25px" Width="75px" BackColor="ControlLight" OnClick="btnOk_Click"
                                                Visible="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="lblErrMsg" ForeColor="Red" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </center>
                </asp:Panel>
                <asp:Panel ID="panelstaff" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="false" BorderWidth="2px" Style="left: 150px; top: 90px; position: absolute;"
                    Height="480px" Width="655px">
                    <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            Select Staff Incharge
                        </caption>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblcollege" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <%--sankar add--%>
                                    <asp:DropDownList ID="ddlcollege" runat="server" Width="150px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldepratstaff" runat="server" Width="150px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged">
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
                        <div id="gviewdiv" style="height: 300px; width: 650px; overflow: auto;">
                            <asp:GridView ID="gviewstaffdetail" runat="server" Width="650" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text='<%#Eval("sno") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Staff Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstafnme" runat="server" Text='<%#Eval("stfnme") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Staff Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstfcode" runat="server" Text='<%#Eval("stfcode") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                <RowStyle ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                            </asp:GridView>
                        </div>
                        <fieldset style="width: 160px; height: 20px; position: absolute; top: 429px; left: 468px;">
                            <asp:Button runat="server" ID="btnstaffadd" Text="Ok" OnClick="btnstaffadd_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="75px" />
                            <asp:Button runat="server" ID="btnexitpop" Text="Exit" OnClick="exitpop_Click" Width="75px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                        </fieldset>
                </asp:Panel>
            </center>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPDF" />
        </Triggers>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
        </Triggers>
        <%--<Triggers>
                <asp:AsyncPostBackTrigger ControlID="Fptimetable" />
            </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
