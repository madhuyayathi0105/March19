<%@ Page Title="" Language="C#" MasterPageFile="~/RequestMOD/RequestSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CertificateRequest.aspx.cs" Inherits="RequestMOD_CertificateRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function bordercolorchange(x) {
            x.style.borderColor = "#c4c4c4";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green; font-size: x-large;">CERTIFICATE
                REQUEST</span>
        </div>
        <br />
    </center>
    <center>
        <div class="maindivstyle" style="width: 1000px;">
            <br />
            <table class="maindivstyle">
                <%--class="maintablestyle"--%>
                <tr>
                    <td>
                        <asp:Label ID="lbl_clgname" runat="server" Text="Institution Name"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox1" runat="server" AutoPostBack="true"
                            Width="175px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Certificate Name
                    </td>
                    <td>
                        <%-- <asp:Button ID="btn_plus1" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus1_Click" Style="float: left;" />--%>
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_certificate" runat="server" CssClass="textbox textbox1 txtheight4"
                                    ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel6" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Width="260px" Height="180px">
                                    <asp:CheckBox ID="cb_certificate" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="cb_certificate_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_certificate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_certificate_checkedchange">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_certificate"
                                    PopupControlID="Panel6" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%--<asp:Button ID="btn_minus1" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus1_Click" Style="float: right;
                            margin-top: -30px;" />--%>
                    </td>
                    <td>
                        <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="Upp3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox textbox1 txtheight3"
                                    ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="p2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px">
                                    <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="cb_batch_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_checkedchange">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_batch"
                                    PopupControlID="p2" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_edulev" Text="Education Level" Width="116px" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_edu" runat="server" CssClass="textbox  textbox1 txtheight4"
                                    ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_edu" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_edu_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_edu" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_edu_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_edu"
                                    PopupControlID="Panel1" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_degree" Text="Degree" Width="89px" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight4"
                                    ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="167px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="cb_degree_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_degree"
                                    PopupControlID="p3" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight4"
                                    ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="231px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="cb_branch_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch"
                                    PopupControlID="p4" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_semT" Text="Semester" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_feecatagory" CssClass="ddlheight3 textbox1" Width="170px"
                            runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Financial Year
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_finyear" CssClass="ddlheight3 textbox1" Width="170px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_searchtype" runat="server" CssClass="textbox  ddlheight1"
                            AutoPostBack="true" OnSelectedIndexChanged="ddl_searchtype_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_searchappno" runat="server" CssClass="textbox textbox1 txtheight1"
                            Width="160px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender02" runat="server" TargetControlID="txt_searchappno"
                            FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=".-/ ">
                        </asp:FilteredTextBoxExtender>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" Enabled="True"
                            ServiceMethod="getappfrom" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                            CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchappno" CompletionListCssClass="autocomplete_completionListElement"
                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground"
                            UseContextKey="true" OnClientPopulating="autoComplete3_OnClientPopulating" DelimiterCharacters="">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td>
                        <asp:Button ID="btn_go" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                            CssClass="textbox btn1" OnClick="btn_go_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lbl_error" ForeColor="Red" Visible="false" runat="server"></asp:Label>
            <br />
            <center>
                <center>
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Style="height: 370px; overflow: auto; background-color: White;
                        border-radius: 10px; box-shadow: 0px 0px 8px #999999" ShowHeaderSelection="false"
                        Visible="false" OnUpdateCommand="FpSpread1_Command">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                <br />
                <asp:Button ID="btn_request" runat="server" Text="Request" Font-Bold="True" Font-Names="Book Antiqua"
                    CssClass="textbox btn2" Visible="false" OnClick="btn_Request_Click" />
                <br />
            </center>
        </div>
    </center>
    <center>
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
    </center>
</asp:Content>
