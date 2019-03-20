<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWizardMod/MasterWizard.master"
    AutoEventWireup="true" CodeFile="Degree Information.aspx.cs" Inherits="Degree_Information" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Degree Information</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
                margin-top: 10px;">Degree Information</span>
            <br />
            <br />
        </div>
    </center>
    <center>
        <div class="maintablestyle" style="width: 1000px; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; text-align: left;">
            <table cellpadding="0px" cellspacing="0px" style="height: 100%; width: 103%; margin: 0px;
                margin-bottom: 10px; margin-top: 10px;">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblcollege" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="25px" Width="250px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="25px" Width="60px" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                        <%--<asp:Label ID="lbldegCode" runat="server" Visible="true"/>--%>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddldegree" Height="24px" Width="88px" AutoPostBack="True"
                            Font-Bold="True" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="300px"
                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                            Font-Size="Medium" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnGo_Click" Style="width: auto; height: auto;" CssClass="textbox textbox1" />
                    </td>
                    <td>
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnAdd_Click" Style="width: auto; height: auto;"
                            CssClass="textbox textbox1" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Label ID="errmsg" runat="server" ForeColor="Red" Visible="False" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
    </center>
    <center>
        <br />
        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
        <asp:GridView runat="server" ID="gviewDegree" AutoGenerateColumns="false" BorderStyle="Double"
            OnRowCreated="gviewDegree_OnRowCreated" CssClass="grid-view" GridLines="Both"
            OnSelectedIndexChanged="gviewDegree_OnSelectedIndexChanged" Font-Names="Book Antique"
            ShowFooter="false" BackColor="AliceBlue">
            <Columns>
                <asp:TemplateField HeaderText="Note" Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblsec" Text='<%#Eval("Noofsec") %>' />
                        <asp:Label runat="server" ID="lblsubdiv" Text='<%#Eval("Noofsubdiv") %>' />
                        <asp:Label runat="server" ID="lbldegcod" Text='<%#Eval("Degregcode") %>' />
                        <asp:Label runat="server" ID="lblstrtyer" Text='<%#Eval("Startyear") %>' />
                        <asp:Label runat="server" ID="lblreg" Text='<%#Eval("Regulation") %>' />
                        <asp:Label runat="server" ID="lblsurren" Text='<%#Eval("IsSurren") %>' />
                        <asp:Label runat="server" ID="lblaccstatus" Text='<%#Eval("AccStatus") %>' />
                        <asp:Label runat="server" ID="lbltxtsurren" Text='<%#Eval("TxtSurren") %>' />
                        <asp:Label runat="server" ID="lblgradesys" Text='<%#Eval("Txtgrade") %>' />
                        <asp:Label runat="server" ID="lblbatchyrnew" Text='<%#Eval("batchyrnew") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <center>
                            <asp:Label runat="server" ID="lblsno" Text='<%#Container.DataItemIndex+1 %>' /></center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Code">
                    <ItemTemplate>
                        <center>
                            <asp:Label runat="server" ID="lblcode" Text='<%#Eval("Code") %>' /></center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblname" Text='<%#Eval("Name") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="System">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblsys" Text='<%#Eval("System") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Duration">
                    <ItemTemplate>
                        <center>
                            <asp:Label runat="server" ID="lbldur" Text='<%#Eval("Duration") %>' /></center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acronym">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblacr" Text='<%#Eval("Acronym") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="No Of Seats">
                    <ItemTemplate>
                        <center>
                            <asp:Label runat="server" ID="lblnoofseat" Text='<%#Eval("Noofseat") %>' /></center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="First Year Non-Semester">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblsem" Text='<%#Eval("Firstsem") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Large" />
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
            <RowStyle ForeColor="#333333" />
        </asp:GridView>
    </center>
    <div id="divEnterDegreeDetails" runat="server" visible="false" style="height: 70em;
        z-index: 100; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
        top: 8%; left: 0px;">
        <center>
            <div id="divDegreeDetail" runat="server" class="table" style="background-color: White;
                border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-left: auto;
                margin-right: auto; width: 970px; height: auto; z-index: 1000; border-radius: 5px;">
                <center>
                    <span style="top: 20px; bottom: 20px; text-align: center; color: Green; font-size: large;
                        position: relative; font-weight: bold;">Enter Degree Details </span>
                </center>
                <table style="margin: 0px; margin-bottom: 10px; margin-top: 25px; margin-left: -283px;
                    position: relative; width: auto;">
                    <tr>
                        <td>
                        <asp:Label ID="lbldegCode" ForeColor="Red" runat="server" Visible="false"/>
                            <asp:Label runat="server" ID="lbldegbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdegbatch" runat="server" CssClass="textbox textbox1" Height="15px"
                                Width="70px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" MaxLength="4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filterextenderToBatch" runat="server" TargetControlID="txtdegbatch"
                                FilterType="Numbers" ValidChars=".">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblCourse" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DegddlCourse" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="25px" Width="180px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblDegBranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DegddlBranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="25px" Width="180px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblAcronym" Text="Acronym" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label><span style="color: Red;">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAcronym" runat="server" CssClass="textbox textbox1" Height="15px"
                                Width="70px" Style="font-family: 'Book Antiqua';text-transform:uppercase;" MaxLength="10" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox><br />
                            <span style="color: Teal;">Eg:CS For Computer Science</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblDegRegCode" Text="Degree Register Code" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label><span style="color: Red;">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtregcode" runat="server" CssClass="textbox textbox1" Height="15px"
                                Width="70px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua" MaxLength="4"
                                Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="filtertxtbox1" runat="server" TargetControlID="txtregcode" FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblDegYerIntro" Text="Year Of Introducation" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label><span style="color: Red;">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtYerIntro" runat="server" CssClass="textbox textbox1" Height="15px"
                                Width="70px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua" MaxLength="4"
                                Font-Size="Medium"></asp:TextBox>
                                   <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtYerIntro" FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblexmsys" Text="Exam System" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:RadioButton ID="semRadio" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Semester" Checked="true" />
                            <asp:CheckBox ID="chkNonsem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="First Year Non Semester System" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:CheckBox runat="server" ID="chkSurrendseat" OnCheckedChanged="Surrendseat_Onclick"
                                AutoPostBack="true" Text="Surrendered Seat For Single Window System" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" CssClass="font" />
                            <asp:TextBox ID="txtSeatSurrend" runat="server" CssClass="textbox textbox1" Height="15px"
                                Width="70px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" MaxLength="3"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filterextenderSeatSurrend" runat="server" TargetControlID="txtSeatSurrend"
                                FilterType="Numbers" ValidChars=".">
                            </asp:FilteredTextBoxExtender>
                            <asp:Label runat="server" ID="lblSeatSurrend" Text="% of Seats Surrendered" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table style=" margin-left:560px; margin-top:-260px">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblNoSeats" Text="Total No Of Seats" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label><span style="color: Red;">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoSeats" runat="server" CssClass="textbox textbox1" Height="15px"
                                Width="70px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" MaxLength="3"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filterextenderNoseats" runat="server" TargetControlID="txtNoSeats"
                                FilterType="Numbers" ValidChars=".">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblNoSeaction" Text="Total No Of Sections" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoSeaction" runat="server" CssClass="textbox textbox1" Height="15px"
                                Width="70px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" MaxLength="2"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filterextenderNoSection" runat="server" TargetControlID="txtNoSeaction"
                                FilterType="Numbers" ValidChars=".">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblTotSubDivision" Text="Total No Of Subdivision" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubDiv" runat="server" CssClass="textbox textbox1" Height="15px"
                                Width="70px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" MaxLength="2"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filterextenderSubDiv" runat="server" TargetControlID="txtSubDiv"
                                FilterType="Numbers" ValidChars=".">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblReg" Text="Regulation" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReg" runat="server" CssClass="textbox textbox1" Height="15px"
                                Width="70px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" MaxLength="4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filterextenderReg" runat="server" TargetControlID="txtReg"
                                FilterType="Numbers" ValidChars=".">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblDegduration" Text="Duration" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtduration" runat="server" CssClass="textbox textbox1" Height="15px"
                                Width="70px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" MaxLength="2"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filterextenderduration" runat="server" TargetControlID="txtduration"
                                FilterType="Numbers" ValidChars=".">
                            </asp:FilteredTextBoxExtender>
                            <asp:Label runat="server" ID="lblSem" Text="Semester(s)" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblAccredition" Text="Accredition Status" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccredition" runat="server" CssClass="textbox textbox1" Height="15px"
                                Width="70px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="margin-left: -375px; margin-top: 95px;">
                    <%-- margin-top: -466px;"--%>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkGrade" runat="server" AutoPostBack="true" Text="Grade System"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkGrade_Onclick" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="importRadio" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" GroupName="Grade" Text="Importing Through External Database" />
                        </td>
                        <td>
                            <asp:RadioButton ID="entryRadio" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" GroupName="Grade" Text="Setting For Manual Entry" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_add_row" runat="server" Text="Add Row in Grid" Font-Bold="true"
                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            <asp:DropDownList ID="ddl_addrow" runat="server" Font-Size="Medium" Font-Bold="true"
                                Font-Names="Book Antiqua">
                                <asp:ListItem Value="NewRow">New Row</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="Btn_AddNewRow" runat="server" Text="Add" Font-Bold="true" Font-Size="Medium"
                                Font-Names="Book Antiqua" OnClick="Btn_AddNewRow_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <asp:GridView runat="server" ID="gview" AutoGenerateColumns="false" BorderStyle="Double"
                                    CssClass="grid-view" GridLines="Both" Font-Names="Book Antique" ShowFooter="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label runat="server" ID="lblsno" Text='<%#Container.DataItemIndex+1 %>' /></center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="From Mark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtfrmmark" runat="server" CssClass="textbox textbox1" Height="15px"
                                                    Text='<%#Eval("frmMark") %>' Width="80px" Style="font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="filterextenderfrmmark" runat="server" TargetControlID="txtfrmmark"
                                                    FilterType="numbers,custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Mark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txttomark" runat="server" CssClass="textbox textbox1" Height="15px"
                                                    Text='<%#Eval("toMark") %>' Width="80px" Style="font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="filterextendertomark" runat="server" TargetControlID="txttomark"
                                                    FilterType="numbers,custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mark Grade">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtmarkgrade" runat="server" CssClass="textbox textbox1" Height="15px"
                                                    Text='<%#Eval("markGrade") %>' Width="90px" Style="font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="filterextendermarkgrade" runat="server" TargetControlID="txtmarkgrade"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="+">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Grade Point">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtgradePoint" runat="server" CssClass="textbox textbox1" Height="15px"
                                                    Text='<%#Eval("gradePoint") %>' Width="90px" Style="font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua" MaxLength="3" Font-Size="Medium"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="filterextendergradepoint" runat="server" TargetControlID="txtgradePoint"
                                                    FilterType="numbers,custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Large" />
                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                    <RowStyle ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                </asp:GridView>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btndeleteGrade" runat="server" Text="Delete Grade" Font-Bold="true" Font-Size="Medium"
                                Font-Names="Book Antiqua" OnClick="Btn_btndeleteGrade_Click" />
                        </td>
                    </tr>
                </table>
                <table style="margin-bottom: 10px; margin-top: 5px; position: relative;">
                    <tr>
                        <td>
                            <asp:Label ID="lblPopDegErr" runat="server" Text=" " CssClass="font" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div id="divDegStuff" style="margin: 0px; padding: 2px; text-align: left;">
                    <center>
                        <table style="margin-bottom: 10px; margin-top: 5px; position: relative;">
                            <tr>
                                <td>
                                    <asp:Button ID="btnPopUpdateDeg" runat="server" Visible="false" Text="Update" CssClass="textbox textbox1"
                                        Font-Names="Book Antiqua" Font-Bold="True" Font-Size="Medium" Style="width: auto;
                                        height: auto;" OnClick="btnPopUpdateDeg_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnPopSaveDeg" runat="server" Text="Save" CssClass="textbox textbox1"
                                        Font-Names="Book Antiqua" OnClick="btnPopSaveDeg_Click" Font-Bold="True" Font-Size="Medium"
                                        Style="width: auto; height: auto;" />
                                </td>
                                <td>
                                    <asp:Button ID="btnPopDeleteDeg" runat="server" Text="Delete" CssClass="textbox textbox1"
                                        Font-Bold="True" OnClick="btnPopDeleteDeg_Click" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                        height: auto;" Visible="True" />
                                </td>
                                <td>
                                    <asp:Button ID="btnPopExitDeg" runat="server" Text="Exit" CssClass="textbox textbox1"
                                        Font-Names="Book Antiqua" Font-Bold="True" Font-Size="Medium" Style="width: auto;
                                        height: auto;" OnClick="btnPopExitDeg_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </div>
        </center>
    </div>
</asp:Content>
