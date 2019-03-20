<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ExamQuesSettings.aspx.cs" Inherits="ExamQuesSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Exam Questions
            Settings</span>
        <div style="width: 100%; margin: 0px; margin-bottom: 10px; margin-top: 10px;" visible="true">
            <table class="maintablestyle" style="height: auto; width: auto;">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCollege" runat="server" Text="College" Style="height: 18px; width: 10px"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlCollege" runat="server" Width="182px" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"
                                        AutoPostBack="True" Height="30px" Style="">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblBatch" runat="server" Text="Batch" CssClass="commonHeaderFont"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                            AutoPostBack="True" Width="106px" Height="30px">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblDegree" runat="server" CssClass="commonHeaderFont" Text="Degree"
                                        AssociatedControlID="txtDegree"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="upnlDegree" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtDegree" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                    ReadOnly="true">-- Select --</asp:TextBox>
                                                <asp:Panel ID="pnlDegree" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                    Width="140px">
                                                    <asp:CheckBox ID="chkDegree" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                        AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cblDegree" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                                    PopupControlID="pnlDegree" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblBranch" runat="server" CssClass="commonHeaderFont" Text="Branch"
                                        AssociatedControlID="txtBranch"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="upnlBranch" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtBranch" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                    ReadOnly="true">-- Select --</asp:TextBox>
                                                <asp:Panel ID="pnlBranch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                    Width="450px">
                                                    <asp:CheckBox ID="chkBranch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                        AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cblBranch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                                    PopupControlID="pnlBranch" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblSem" runat="server" CssClass="commonHeaderFont" Text="Sem"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlsem" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                            AutoPostBack="True" Width="76px" Height="30px">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSubType" runat="server" Text="Type" CssClass="commonHeaderFont"
                                        AssociatedControlID="txttype"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txttype" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                    ReadOnly="true">-- Select --</asp:TextBox>
                                                <asp:Panel ID="Panel2" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                    Width="280px">
                                                    <asp:CheckBox ID="CheckBox1" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                        AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged" Checked="true" />
                                                    <asp:CheckBoxList ID="CheckBoxList1" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged">
                                                        <%-- <asp:ListItem Text="Theory" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Practical" Value="1" Selected="True"></asp:ListItem>--%>
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txttype"
                                                    PopupControlID="Panel2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                                <%-- <asp:DropDownList ID="ddlSubType" runat="server" CssClass="commonHeaderFont" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlSubType_SelectedIndexChanged" Width="100px">
                                        <asp:ListItem Text="Theory" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Practical" Value="1"></asp:ListItem>
                                    </asp:DropDownList>--%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblSubject" runat="server" CssClass="commonHeaderFont" Text="Subjects"
                                        AssociatedControlID="txtSubject"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtSubject" Visible="true" Width="100px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                    ReadOnly="true">-- Select --</asp:TextBox>
                                                <asp:Panel ID="Panel1" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                    Width="380px">
                                                    <asp:CheckBox ID="chkSubject" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                        AutoPostBack="True" OnCheckedChanged="chkSubject_CheckedChanged" />
                                                    <asp:CheckBoxList ID="CblSubject" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="CblSubject_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtSubject"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblTest" runat="server" Text="Test" CssClass="commonHeaderFont" AssociatedControlID="ddlTest"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="upnlTest" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlTest" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged"
                                                    AutoPostBack="True" Width="126px" Height="30px">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblNopart" runat="server" CssClass="commonHeaderFont" Text="No.of Part"
                                        AssociatedControlID="txtSubject"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoPart" Visible="true" Width="45px" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" MaxLength="2" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtNoPart"
                                        FilterType="numbers,custom" ValidChars="">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnGo" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnGo_Click" Text="Go" Width="70px" />
                                </td>
                                <td>
                                    <asp:Button ID="btnReport" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnReport_Click" Text="Report" Width="70px" />
                                </td>
                                <td>
                                    <asp:Label ID="lblMaxMark" runat="server" CssClass="commonHeaderFont" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbInternal" runat="server" GroupName="rpt" Text="Internal" AutoPostBack="true"
                                        OnCheckedChanged="rbInternal_OnCheckedChanged"  Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbExternal" runat="server" GroupName="rpt" Text="External" AutoPostBack="true"
                                        OnCheckedChanged="rbExternal_OnCheckedChanged"  Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" CssClass="commonHeaderFont" Text="Exam Month & Year"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                            AutoPostBack="True" Width="76px" Height="30px">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="commonHeaderFont" Width="76px"
                                            Height="30px">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Label ID="lblErrmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Visible="False" Font-Size="Medium" ForeColor="#CC0000"></asp:Label>
        <br />
        <center>
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="GridPart" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                        width: auto; font-size: 14px" Font-Names="Times New Roman" AutoGenerateColumns="false">
                                        <HeaderStyle BackColor="#009999" ForeColor="White" />
                                        <AlternatingRowStyle Height="20px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Part No" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPartNo" runat="server" Text='<%# Eval("PartName") %>' />
                                                    <asp:Label ID="lblPart" runat="server" Text='<%# Eval("PartNo") %>' Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No.of Question">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNoQ" runat="server" MaxLength="2" Text='<%# Eval("Qno") %>'></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtNoQ"
                                                        FilterType="numbers,custom" ValidChars="">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <center>
                                        <asp:Button ID="Button1" runat="server" Text=">>" Style="height: 26px; left: 445px;
                                            font-weight: 700;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Visible="false" OnClick="Button1_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="GridView2" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                        width: auto; font-size: 14px" Font-Names="Times New Roman" OnDataBound="GridView2_OnDataBound"
                                        AutoGenerateColumns="false">
                                        <HeaderStyle BackColor="#009999" ForeColor="White" />
                                        <AlternatingRowStyle Height="20px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Part No" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPartNo" runat="server" Text='<%# Eval("PartName") %>' />
                                                    <asp:Label ID="lblPart" runat="server" Text='<%# Eval("PartNo") %>' Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No.of Question">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNoQ" runat="server" Text='<%# Eval("NO_Ques") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Q.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQNo" runat="server" Text="<%# Container.DisplayIndex+1 %>" />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sub Division 1">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNoQ1" runat="server" MaxLength="2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtNoQ1"
                                                        FilterType="numbers,custom" ValidChars="">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <center>
                                        <asp:Button ID="Button2" runat="server" Text=">>" Style="height: 26px; left: 445px;
                                            font-weight: 700;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Visible="false" OnClick="Button2_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="GridView3" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                        width: auto; font-size: 14px" Font-Names="Times New Roman" OnDataBound="GridView3_OnDataBound"
                                        AutoGenerateColumns="false">
                                        <HeaderStyle BackColor="#009999" ForeColor="White" />
                                        <AlternatingRowStyle Height="20px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Part No" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPartNo" runat="server" Text='<%# Eval("PartName") %>' />
                                                    <asp:Label ID="lblPart" runat="server" Text='<%# Eval("PartNo") %>' Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No.of Question">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNoQ" runat="server" Text='<%# Eval("NO_Ques") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Q.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQNo" runat="server" Text='<%# Eval("Qno") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sub Division 1">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSub1" runat="server" Text='<%# Eval("Sub1") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sub Division 2">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNoQ2" runat="server" MaxLength="2"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtNoQ2"
                                                        FilterType="numbers,custom" ValidChars="">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <center>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </center>
    <br />
    <center>
        <asp:Button ID="Button3" runat="server" Text="OK" Style="height: 26px; left: 445px;
            font-weight: 700;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
            Visible="false" OnClick="Button3_Click" />
    </center>
    <br />
    <center>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
            HeaderStyle-BackColor="#0CA6CA" BackColor="White" OnDataBound="OnDataBound" >
            <%-- OnDataBound="gridTimeTable_OnDataBound"--%>
            <Columns>
                <asp:TemplateField HeaderText="Part">
                    <ItemTemplate>
                        <asp:Label ID="lblPartgNO" runat="server" Text='<%#Eval("PartName") %>'></asp:Label>
                        <asp:Label ID="lblPart" runat="server" Text='<%# Eval("PartNo") %>' Visible="false" />
                    </ItemTemplate>
                    <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#F8B7B3" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="No.of Qes">
                    <ItemTemplate>
                        <asp:Label ID="txtgNoQ" runat="server" Text='<%# Eval("NO_Ques") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Q.No">
                    <ItemTemplate>
                        <asp:Label ID="lblgQno" runat="server" Text='<%# Eval("Qno") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub Division 1">
                    <ItemTemplate>
                        <asp:Label ID="lblSub1" runat="server" Text='<%# Eval("Sub1") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub Division 2">
                    <ItemTemplate>
                        <asp:Label ID="lblSub2" runat="server" Text='<%# Eval("Sub2") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Course Outcome">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlgUnit" runat="server" Width="120px">
                        </asp:DropDownList>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Marks">
                    <ItemTemplate>
                        <asp:TextBox ID="txtgMarks" runat="server" MaxLength="3"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtgMarks"
                            FilterType="numbers,custom" ValidChars=".">
                        </asp:FilteredTextBoxExtender>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </center>
    <br />
    <table>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" Style="height: 26px; left: 445px;
                    font-weight: 700; margin-left: 900px;" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Visible="false" OnClick="btnSave_Click" />
            </td>
        </tr>
    </table>
    <center>
        <asp:GridView ID="GridReport" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
            width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" Visible="false"
            BackColor="AliceBlue" OnRowDataBound="RowDataBound" OnDataBound="GridReport_OnDataBound">
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"
                            OnClick="lnkAttMark11" ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                        <%--<asp:Label ID="lblSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>--%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Degree">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblDegInfo" runat="server" Text='<%# Eval("DegInfo") %>' OnClick="lnkAttMark11"
                            ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subject Code">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblSubCode" runat="server" Text='<%# Eval("Subject Code") %>'
                            OnClick="lnkAttMark11" ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                        <asp:Label ID="lblSubNo" runat="server" Text='<%# Eval("SubjectNo") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblCriteria" runat="server" Text='<%# Eval("CriteriaNo") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subject Name">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblSubjectName" runat="server" Text='<%# Eval("Subject Name") %>'
                            OnClick="lnkAttMark11" ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Part No">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblNOPart" runat="server" Text='<%# Eval("PartNo") %>' OnClick="lnkAttMark11"
                            ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Q.No">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblQNo" runat="server" Text='<%# Eval("Qno") %>' OnClick="lnkAttMark11"
                            ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub Devision 1">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblSub1" runat="server" Text='<%# Eval("sub1") %>' OnClick="lnkAttMark11"
                            ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="50px" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub Devision 2">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblSub2" runat="server" Text='<%# Eval("sub2") %>' OnClick="lnkAttMark11"
                            ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="50px" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Course Outcome">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblCo" runat="server" Text='<%# Eval("Course Outcome") %>' OnClick="lnkAttMark11"
                            ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Marks">
                    <ItemTemplate>
                        <asp:LinkButton ID="lblMark" runat="server" Text='<%# Eval("mark") %>' OnClick="lnkAttMark11"
                            ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
        </asp:GridView>
    </center>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <div id="cannot_insert_div" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px; height: 100em;">
        <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
            Style="height: 30px; width: 30px; position: absolute; margin-top: 61px; margin-left: 1005px;"
            OnClick="btn_Exit_Click1" />
        <center>
            <div id="cannot_insert" runat="server" style="background-color: White; height: 400px;
                width: 840px; border: 5px solid #0CA6CA; border-top: 5px solid #0CA6CA; margin-top: 72px;
                border-radius: 10px;">
                <asp:Label ID="lbl_upload_suc" runat="server" Visible="false" ForeColor="Blue"></asp:Label>
                <br />
                <asp:Label ID="lbl_cannotsave" Visible="false" runat="server" Style="color: Red;"
                    Font-Bold="true" Font-Size="Medium"></asp:Label>
                <div style="height: 345px; width: 700px; overflow: auto;">
                    <asp:TextBox ID="lbl_cannotinsert" TextMode="MultiLine" runat="server" Style="height: 334px;
                        overflow: auto;" Visible="false" Width="650px" ForeColor="Blue" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </center>
    </div>
    <div id="divPopSpread" runat="server" visible="false" style="height: 220em; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <asp:ImageButton ID="btnClose" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
            Style="height: 30px; width: 30px; margin-top: 11%; margin-left: 1200px; position: absolute;"
            OnClick="btnclosespread_OnClick" />
        <center>
            <div id="divPopSpreadContent" runat="server" class="table" style="background-color: White;
                height: auto; width: 72%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                left: 15%; right: 39%; top: 5%; padding: 5px; position: absolute; border-radius: 10px;">
                <center style="height: 30px; font-family: Book Antiqua; font-weight: bold; color: Navy">
                    <asp:Label ID="lblSuName" runat="server"></asp:Label></center>
                <center>
                    <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
                        HeaderStyle-BackColor="#0CA6CA" BackColor="White" OnDataBound="GridView4_OnDataBound">
                        <%-- OnDataBound="gridTimeTable_OnDataBound"--%>
                        <Columns>
                            <asp:TemplateField HeaderText="Part">
                                <ItemTemplate>
                                    <asp:Label ID="lblPartgNO" runat="server" Text='<%#Eval("PartName") %>'></asp:Label>
                                    <asp:Label ID="lblPart" runat="server" Text='<%# Eval("PartNo") %>' Visible="false" />
                                </ItemTemplate>
                                <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#F8B7B3" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No.of Qes">
                                <ItemTemplate>
                                    <asp:Label ID="txtgNoQ" runat="server" Text='<%# Eval("NO_Ques") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Q.No">
                                <ItemTemplate>
                                    <asp:Label ID="lblMasterNo" runat="server" Text='<%# Eval("MasterID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblgQno" runat="server" Text='<%# Eval("Qno") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sub Division 1">
                                <ItemTemplate>
                                    <asp:Label ID="lblSub1" runat="server" Text='<%# Eval("Sub1") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sub Division 2">
                                <ItemTemplate>
                                    <asp:Label ID="lblSub2" runat="server" Text='<%# Eval("Sub2") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Course Outcome">
                                <ItemTemplate>
                                    <asp:Label ID="lblCo" runat="server" Text='<%# Eval("CourseOutComeNo") %>' Visible="false"></asp:Label>
                                    <asp:DropDownList ID="ddlgUnit" runat="server" Width="120px">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Marks">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtgMarks" runat="server" Text='<%# Eval("Mark") %>' MaxLength="3"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtgMarks"
                                        FilterType="numbers,custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </center>
                <center>
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" Style="height: 26px; left: 445px;
                        font-weight: 700; margin-left: 900px;" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnUpdate_Click" />
                </center>
            </div>
        </center>
    </div>
</asp:Content>
