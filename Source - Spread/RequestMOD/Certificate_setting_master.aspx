<%@ Page Title="" Language="C#" MasterPageFile="~/RequestMOD/RequestSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Certificate_setting_master.aspx.cs" Inherits="Certificate_setting_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function Emptytextalert() {
            var id = document.getElementById("<%=txt_addgroup.ClientID %>");
            if (id.value == "" || id.value == "0") {
                id.style.borderColor = "red";
                document.getElementById("<%=lblerror.ClientID %>").value = "Enter the Certificate Name";
                return false;
            }
            else { id.style.borderColor = "#c4c4c4"; document.getElementById("<%=lblerror.ClientID %>").value = ""; }
        }
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
                SETTING MASTER</span>
        </div>
        <br />
    </center>
    <center>
        <div class="maindivstyle" style="width: 1000px;">
            <br />
            <table class="maindivstyle">
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
                    <td style="width: 208px">
                        <asp:Button ID="btn_plus1" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus1_Click" Style="float: left;" />
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_certificate" runat="server" CssClass="textbox textbox1 txtheight3"
                                    ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel6" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Width="250px" Height="180px">
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
                        <asp:Button ID="btn_minus1" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus1_Click" Style="float: right;
                            margin-top: -30px;" />
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
                        Header Name
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_Header" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight4">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel4" runat="server" Width="250px" Height="170px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cb_Header" runat="server" OnCheckedChanged="cb_Header_SelectedIndexChanged"
                                        Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="cbl_Header" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Header_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControer3" runat="server" TargetControlID="txt_Header"
                                    PopupControlID="Panel4" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        Ledger Name
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_Ledger" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight4">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel5" runat="server" Width="250px" Height="170px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cb_Ledger" runat="server" OnCheckedChanged="cb_Ledger_SelectedIndexChanged"
                                        Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="cbl_Ledger" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Ledger_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupContrder4" runat="server" TargetControlID="txt_Ledger"
                                    PopupControlID="Panel5" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td colspan="2">
                        <asp:CheckBox ID="cb_Feesdue" runat="server" Text="Fees Due" /><asp:CheckBox ID="cb_librarydue"
                            runat="server" Text="Library Due" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Financial Year
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_finyear" CssClass="ddlheight3 textbox1" Width="170px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                         <asp:Label ID="lbl_sem" Text="Semester" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_feecatagory" CssClass="ddlheight3 textbox1" Width="170px"
                            runat="server">
                        </asp:DropDownList>
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
                <asp:GridView ID="headerledgeramtGrid" runat="server" AutoGenerateColumns="false"
                    Width="800px" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Header Name">
                            <ItemTemplate>
                                <asp:Label ID="lbl_headername" runat="server" Text='<%# Eval("headername") %>'></asp:Label>
                                <asp:Label ID="lbl_headerfk" runat="server" Visible="false" Text='<%# Eval("headerfk") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ledger Name">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ledgername" runat="server" Text='<%# Eval("leadername") %>'></asp:Label>
                                <asp:Label ID="lbl_ledgerfk" runat="server" Visible="false" Text='<%# Eval("ledgerfk") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_amount" runat="server" Style="text-align: center;" Text='<%# Eval("amount") %>'
                                    Width="120px" CssClass="textbox"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_amount"
                                    FilterType="Custom,Numbers" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="130px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="btn_viewfees" runat="server" Text="View Fees" Font-Bold="True" Visible="false"
                    Font-Names="Book Antiqua" CssClass="textbox btn3" Height="30px" OnClick="btn_viewfees_Click"
                    Style="margin-top: 10px;" />
                <asp:GridView ID="certificatedetails_grid" runat="server" AutoGenerateColumns="false"
                    Width="950px" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" Style="margin-top: 10px;"
                    OnDataBound="certificate_databound">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Certificate Name">
                            <ItemTemplate>
                                <asp:Label ID="lbl_certificatename" runat="server" Text='<%# Eval("certificatename") %>'></asp:Label>
                                <asp:Label ID="lbl_certificatenameId" runat="server" Visible="false" Text='<%# Eval("certificatenameid") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Degree">
                            <ItemTemplate>
                                <asp:Label ID="lbl_degree" runat="server" Text='<%# Eval("degree") %>'></asp:Label>
                                <asp:Label ID="lbl_degreevalue" runat="server" Visible="false" Text='<%# Eval("degreevalue") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Header Name">
                            <ItemTemplate>
                                <asp:Label ID="lbl_headername" runat="server" Text='<%# Eval("headername") %>'></asp:Label>
                                <asp:Label ID="lbl_headerfk" runat="server" Visible="false" Text='<%# Eval("headerfk") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ledger Name">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ledgername" runat="server" Text='<%# Eval("ledgername") %>'></asp:Label>
                                <asp:Label ID="lbl_ledgerfk" runat="server" Visible="false" Text='<%# Eval("ledgerfk") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_amount" runat="server" Style="text-align: center;" Text='<%# Eval("amount") %>'
                                    Width="120px" CssClass="textbox"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_amount"
                                    FilterType="Custom,Numbers" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="130px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <div id="btn_div" runat="server" visible="false">
                    <asp:Button ID="btn_onlinefees" runat="server" Text="Apply Online Fees" Font-Bold="True"
                        Font-Names="Book Antiqua" CssClass="textbox btn3" Height="30px" OnClick="btn_onlinefees_Click" />
                    <asp:Button ID="btn_offlinefees" runat="server" Text="Apply Offline Fees" Font-Bold="True"
                        Font-Names="Book Antiqua" Visible="false" CssClass="textbox btn3" Height="30px"
                        OnClick="btn_offlinefees_Click" />
                </div>
                <br />
            </center>
        </div>
    </center>
    <center>
        <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
            <center>
                <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                    height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table style="line-height: 30px">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox textbox1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="line-height: 35px">
                                <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                    CssClass="textbox btn2" OnClientClick="return Emptytextalert()" OnClick="btn_addgroup_Click" />
                                <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
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
