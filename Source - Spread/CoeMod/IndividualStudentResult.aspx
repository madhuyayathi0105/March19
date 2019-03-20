<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="IndividualStudentResult.aspx.cs" Inherits="CoeMod_IndividualStudentResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function display() {
            document.getElementById('MainContent_errmsg').innerHTML = "";


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="lblhead" runat="server" Text="Individual Student wise Marks/Grade Report" class="fontstyleheader"
            Style="color: #008000; font-size: x-large"></asp:Label>
    </center>
    <div>
        <center>
            <table cellpadding="0px" cellspacing="0px" style="width: 900px; height: 50px; background-color: #0CA6CA;"
                class="table">
                <tr>
                    <td style="padding: 10px; width: 50px;">
                        <asp:Label ID="Label13" runat="server" Text="College" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td style="width: 10px;">
                        <asp:DropDownList ID="ddlcollege" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_indexChanged"
                            CssClass="textbox ddlstyle ddlheight3" Width="300px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 25px; width: 25px;">
                        <asp:DropDownList ID="ddlrollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlrollno_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 5px; width: 25px;">
                        <asp:TextBox ID="txt_rollno" runat="server" AutoPostBack="true" CssClass="textbox txtheight4 textbox1"
                            OnTextChanged="txt_rollno_TextChanged"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_rollno"
                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                        </asp:FilteredTextBoxExtender>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td style="padding-left: 12px;">
                        <asp:Label ID="lblsemester" runat="server" Text="Semester" Visible="true" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:UpdatePanel ID="semUpdatePanel" runat="server" Visible="true">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_sem" runat="server" CssClass="font" Width="122px" Font-Names="Book Antiqua"
                                    Font-Size="Medium">--Select--</asp:TextBox>
                                <asp:Panel ID="semPanel" runat="server" CssClass="MultipleSelectionDDL" Style="font-family: 'Book Antiqua';
                                    position: absolute;" Font-Names="Book Antiqua" Height="230px" Width="124px" BackColor="AliceBlue">
                                    <asp:CheckBox ID="chkSem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="chkSem_checkedchanged" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklSem" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklSem_selectedchanged" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_sem"
                                    PopupControlID="semPanel" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="padding-left: 18px; padding-right: 10px;">
                        <asp:Button ID="btnGo" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                            OnClick="btnGo_Click" Font-Size="Medium" Height="28px" Width="45px" />
                    </td>
                </tr>
            </table>
        </center>
        <br />
        <br />
        <br />
        <center>
            <div id="divSpreadDet" runat="server" visible="false" style="overflow: auto; border: 1px solid Gray;
                width: 750px; border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="false" BorderStyle="Solid"
                    OnCellClick="spreadDet_CellClick" BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999;
                    border-radius: 10px;">
                    <Sheets>
                        <%--OnPreRender="spreadDet_SelectedIndexChanged"--%>
                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Cyan">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <div style="margin-top: 15px;">
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnxl_Click" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Visible="true" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
                <br />
            </div>
            <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium"></asp:Label>
        </center>
    </div>
</asp:Content>
