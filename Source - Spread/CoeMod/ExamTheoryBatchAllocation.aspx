<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ExamTheoryBatchAllocation.aspx.cs" Inherits="ExamTheoryBatchAllocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Exam Theory/Lab
            Batch Allocation</span>
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
                                    <asp:Label ID="lblExamYear" runat="server" Text="Exam Year" CssClass="commonHeaderFont"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlExamYear" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged"
                                            AutoPostBack="True" Width="106px" Height="30px">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblExamMonth" runat="server" Text="Month" CssClass="commonHeaderFont"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlExamMonth" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlExamMonth_SelectedIndexChanged"
                                            AutoPostBack="True" Width="106px" Height="30px">
                                        </asp:DropDownList>
                                    </div>
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
                                    <asp:Label ID="lblDegree" runat="server" CssClass="commonHeaderFont" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddldegree1" runat="server" CssClass="font" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Width="101px" OnSelectedIndexChanged="ddldegree1_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblBranch" runat="server" CssClass="commonHeaderFont" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <%--<asp:UpdatePanel ID="upnlDegree" runat="server">
                                            <ContentTemplate>--%>
                                        <asp:TextBox ID="txtDegree" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                            ReadOnly="true">-- Select --</asp:TextBox>
                                        <asp:Panel ID="pnlDegree" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="280px">
                                            <asp:CheckBox ID="chkDegree" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblDegree" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                            PopupControlID="pnlDegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                        <%--   </ContentTemplate>
                                        </asp:UpdatePanel>--%>
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
                                    <asp:Label ID="lblSem" runat="server" CssClass="commonHeaderFont" Text="Sem"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlsem" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                            AutoPostBack="True" Width="76px" Height="25px">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblSubject" runat="server" CssClass="commonHeaderFont" Text="Subjects"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlSubject" AutoPostBack="true" Width="307px" runat="server"
                                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged"
                                            Font-Size="Medium">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblSubSubject" runat="server" CssClass="commonHeaderFont" Text="Sub-Subject"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:DropDownList ID="ddlSubSubject" AutoPostBack="true" Width="157px" runat="server"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblNopart" runat="server" CssClass="commonHeaderFont" Text="No.of Batch"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoPart" Visible="true" Width="45px" runat="server" Font-Names="Book Antiqua"
                                        MaxLength="2" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtNoPart"
                                        FilterType="numbers,custom" ValidChars="">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnGo" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnGo_Click" Text="Go" Width="70px" />
                                </td>
                                <td>
                                    <asp:Button ID="btnView" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnView_Click" Text="View" Width="80px" />
                                </td>
                                <td>
                                    <asp:Label ID="lblMaxMark" runat="server" CssClass="commonHeaderFont" Visible="false"></asp:Label>
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
        <asp:GridView ID="GridPart" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
            width: auto; font-size: 14px" Font-Names="Times New Roman" AutoGenerateColumns="false">
            <HeaderStyle BackColor="#009999" ForeColor="White" />
            <AlternatingRowStyle Height="20px" />
            <Columns>
                <asp:TemplateField HeaderText="Batch" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblPartNo" runat="server" Text='<%# Eval("PartName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:TextBox ID="txtappldate" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                            Width="100px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" Format="dd/MM/yyyy" TargetControlID="txtappldate"
                            runat="server">
                        </asp:CalendarExtender>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Session">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlSession" runat="server" CssClass="commonHeaderFont" Width="70px">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Time">
                    <ItemTemplate>
                        <asp:TextBox ID="txtTime" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                            Width="100px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Internal">
                    <ItemTemplate>
                        <asp:TextBox ID="txtInternal" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                            Width="100px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="External">
                    <ItemTemplate>
                        <asp:TextBox ID="txtExternal" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                            Width="100px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Skill Assistant">
                    <ItemTemplate>
                        <asp:TextBox ID="txtSkillAss" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                            Width="100px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lab Assistant">
                    <ItemTemplate>
                        <asp:TextBox ID="txtLabAss" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                            Width="100px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <asp:Button ID="btnLab" runat="server" Text="Select" Style="height: 26px; left: 445px;
                            font-weight: 700;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            OnClick="btnLabl_click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div style="margin-left: 750px">
            <asp:Button ID="Button1" runat="server" Text="GO" Style="height: 26px; left: 445px;
                font-weight: 700;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                Visible="false" OnClick="Button1_Click" />
        </div>
    </center>
    <center>
        <div class="maindivstyle" id="divRange" runat="server" visible="false" align="center"
            style="border-radius: 7px; width: 500px; height: 35px;">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="From" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txt_frange" CssClass="textbox textbox1 txtheight" runat="server"
                            MaxLength="4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_frange"
                            FilterType="Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="To" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txt_trange" CssClass="textbox textbox1 txtheight" runat="server"
                            MaxLength="4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_trange"
                            FilterType="Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblBatchSel" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlBatchSel" runat="server" CssClass="commonHeaderFont" Width="100px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="Btn_range" runat="server" Text="Select" OnClick="Btn_range_Click"
                            CssClass="textbox1 textbox btn2" Font-Bold="true" Font-Names="Book Antiqua" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <br />
    <center>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
            HeaderStyle-BackColor="#0CA6CA" BackColor="White">
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <asp:Label ID="lblQNo" runat="server" Text="<%# Container.DisplayIndex+1 %>" />
                    </ItemTemplate>
                    <ItemStyle Width="60px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Roll No">
                    <ItemTemplate>
                        <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("Roll_no") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reg No">
                    <ItemTemplate>
                        <asp:Label ID="lblRegNo" runat="server" Text='<%# Eval("Reg_no") %>'></asp:Label>
                        <asp:Label ID="lblAppNo" runat="server" Text='<%# Eval("App_no") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblExamCode" runat="server" Text='<%# Eval("exam_code") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Student Name">
                    <ItemTemplate>
                        <asp:Label ID="lblStud" runat="server" Text='<%# Eval("stud_name") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="250px" HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subject Name">
                    <ItemTemplate>
                        <asp:Label ID="lblSub" runat="server" Text='<%# Eval("subject_name") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="250px" HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("edate") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Session">
                    <ItemTemplate>
                        <asp:Label ID="lblSess" runat="server" Text='<%# Eval("ExamSession") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Batch">
                    <ItemTemplate>
                        <asp:Label ID="lblStud" runat="server" Text='<%# Eval("Batch") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </center>
    <center>
        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
            HeaderStyle-BackColor="#0CA6CA" BackColor="White">
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <asp:Label ID="lblQNo" runat="server" Text="<%# Container.DisplayIndex+1 %>" />
                    </ItemTemplate>
                    <ItemStyle Width="60px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Roll No">
                    <ItemTemplate>
                        <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("Roll_no") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reg No">
                    <ItemTemplate>
                        <asp:Label ID="lblRegNo" runat="server" Text='<%# Eval("Reg_no") %>'></asp:Label>
                        <asp:Label ID="lblAppNo" runat="server" Text='<%# Eval("App_no") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblExamCode" runat="server" Text='<%# Eval("exam_code") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Student Name">
                    <ItemTemplate>
                        <asp:Label ID="lblStud" runat="server" Text='<%# Eval("stud_name") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="250px" HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Batch">
                    <ItemTemplate>
                        <asp:TextBox ID="txtgMarks" runat="server" Text='<%# Eval("Batch") %>' ReadOnly="true"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </center>
    <br />
    <center>
        <asp:Button ID="btnSave" runat="server" Text="Save Batch" CssClass="textbox btn2"
            Width="120px" Height="30px" Visible="false" OnClick="btnSave_Click" BackColor="#76D7C4" />
        <%--  <asp:Button ID="btnSave" CssClass="textbox textbox1" runat="server" Style="width: auto;
            height: auto; font-family: 'Book Antiqua'; font-weight: bold; font-size: medium;"
            Text="Save" Visible="false" OnClick="btnSave_Click" />--%>
    </center>
    <center>
        <asp:Panel ID="panel3" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
            BorderWidth="2px" Style="left: 30%; top: 35%; right: 30%; position: absolute;
            overflow: auto; z-index: 3;" Height="480px" Width="715px">
            <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                font-size: Small; font-weight: bold">
                <br />
                <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                    left: 200px">
                    Select Staff Incharge
                </caption>
                <br />
                <asp:RadioButton ID="rbInternal" runat="server" Text="Internal" GroupName="User" />
                <asp:RadioButton ID="rbExternal" runat="server" Text="External" GroupName="User" />
                <asp:RadioButton ID="rbLabAss" runat="server" Text="Lab Assistant" GroupName="User" />
                <asp:RadioButton ID="rblSkillAss" runat="server" Text="Skill Assistant" GroupName="User" />
                <asp:CheckBox ID="chkExtOnly" runat="server" Text="External Only" />
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList1" runat="server" Width="150px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldepratstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="LblCategory" runat="server" Text="Staff Category"></asp:Label>
                            <asp:TextBox ID="txt_Category" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                Width="135px" Font-Bold="True" Visible="true">---Select---</asp:TextBox>
                            <asp:Panel ID="panel_Category" runat="server" CssClass="multxtpanel" Height="250px"
                                Width="355px" Style="text-align: left;">
                                <asp:CheckBox ID="cb_Category" runat="server" OnCheckedChanged="cb_Category_CheckedChanged"
                                    Text="Select All" AutoPostBack="True" TextAlign="Right" Style="text-align: left;" />
                                <asp:CheckBoxList ID="cbl_Category" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Category_SelectedIndexChanged"
                                    TextAlign="Right">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_Category"
                                PopupControlID="panel_Category" Position="Bottom">
                            </asp:PopupControlExtender>
                            <asp:Button ID="BtnCategory" runat="server" Text="Go" Font-Bold="true" Font-Size="Medium"
                                Font-Names="Book Antiqua" OnClick="BtnCategory_Click" Width="53px" />
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
                <div id="div7" runat="server" style="overflow: auto; border: 1px solid Gray; width: 460px;
                    height: 280px;">
                    <asp:GridView runat="server" ID="gviewstaff" AutoGenerateColumns="false" Style="height: 300;
                        width: 460px; overflow: auto;">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <%#Container.DisplayIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <center>
                                        <asp:Label ID="allchk" runat="server" Text="Select"></asp:Label></center>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="selectchk1" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Staff Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblstaff" runat="server" Text='<%#Eval("Staff_Code") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Staff Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblname" runat="server" Text='<%#Eval("Staff_Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                        <RowStyle ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                    </asp:GridView>
                    <fieldset style="position: absolute; left: 345px; visibility: visible; top: 426px;
                        width: 140px; height: 2px;">
                        <asp:Button runat="server" ID="btnstaffadd" AutoPostBack="True" Text="Ok" Font-Bold="true"
                            OnClick="btnstaffadd_Click" Style="width: 75px; top: 2px; position: absolute;
                            left: 2px;" />
                        <asp:Button runat="server" ID="btnexit" AutoPostBack="True" Text="Exit" Font-Bold="true"
                            OnClick="btnexit_Click" Style="width: 75px; top: 2px; position: absolute; left: 85px;" />
                    </fieldset>
                </div>
        </asp:Panel>
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
</asp:Content>
