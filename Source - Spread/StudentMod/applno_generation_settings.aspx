<%@ Page Title="" Language="C#" MasterPageFile="~/studentmod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="applno_generation_settings.aspx.cs" Inherits="applno_generation_settings"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <link href="../css/Registration.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .modalPopup
        {
            background: rgba(54, 25, 25, .2);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function validate() {
                var empty = true;
                var batch = document.getElementById("<%=txtclgbatch.ClientID %>").value;
                var applval = document.getElementById("<%=txtclgappl.ClientID %>").value;
                var applsize = document.getElementById("<%=txtclgadmis.ClientID %>").value;

                if (batch == "") {
                    batch = document.getElementById("<%=txtclgbatch.ClientID %>");
                    batch.style.borderColor = 'Red';
                    empty = false;
                }
                else {
                    if (batch.length != 4) {
                        batch = document.getElementById("<%=txtclgbatch.ClientID %>");
                        batch.style.borderColor = 'Red';
                        empty = false;
                    }
                }

                if (applval == "") {
                    applval = document.getElementById("<%=txtclgappl.ClientID %>");
                    applval.style.borderColor = 'Red';
                    empty = false;
                }
                if (applsize == "") {
                    applsize = document.getElementById("<%=txtclgadmis.ClientID %>");
                    applsize.style.borderColor = 'Red';
                    empty = false;
                }

                if (empty == false) {
                    return false;
                }
            }

            function validatedeg() {
                var empty = true;
                var batch = document.getElementById("<%=txt_batch.ClientID %>").value;
                var applval = document.getElementById("<%=txt_serialstartwith.ClientID %>").value;
                var applsize = document.getElementById("<%=txt_serialsize.ClientID %>").value;
                if (batch == "") {
                    batch = document.getElementById("<%=txt_batch.ClientID %>");
                    batch.style.borderColor = 'Red';
                    empty = false;
                }
                else {
                    if (batch.length != 4) {
                        batch = document.getElementById("<%=txt_batch.ClientID %>");
                        batch.style.borderColor = 'Red';
                        empty = false;
                    }
                }

                if (applval == "") {
                    applval = document.getElementById("<%=txt_serialstartwith.ClientID %>");
                    applval.style.borderColor = 'Red';
                    empty = false;
                }
                if (applsize == "") {
                    applsize = document.getElementById("<%=txt_serialsize.ClientID %>");
                    applsize.style.borderColor = 'Red';
                    empty = false;
                }

                if (empty == false) {
                    return false;
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <span style="color: Green; top: 80px; left: 39%; position: absolute; font-weight: bold;
                    font-size: large;">Application Number Generation Settings </span>
            </center>
            <%-- <asp:LinkButton ID="Linkbtn_exit" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" Style="top: 150px; left: 80%; position: absolute;" ForeColor="Blue"
                PostBackUrl="~/Student.aspx" CausesValidation="False">Back</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="LinkButton3" Style="top: 150px; left: 84%; position: absolute;"
                runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Blue"
                PostBackUrl="~/Default_login.aspx" CausesValidation="False">Home</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="lb2" Style="top: 150px; left: 89%; position: absolute;" runat="server"
                Visible="true" OnClick="lb2_Click" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" ForeColor="Blue" CausesValidation="False">Logout</asp:LinkButton>--%>
        </div>
        <div class="center" style="position: relative; height: 130em; border-left: 1px solid;
            border-right: 1px solid; border-style: solid; top: 40px; width: 95%; border-color: Gray;">
            <div id="step1" runat="server">
                <div>
                    <center>
                        <span style="color: Green; font-weight: bold; visibility: hidden;">SELECT COLLEGE AND
                            COURSE INFORMATION</span>
                        <%--<asp:Label ID="lblselectcollege" runat="server" Font-Bold="true" ForeColor="Green"
                        Text="Select College" Width="300px" Font-Size="Large" Height="35px"></asp:Label>--%>
                    </center>
                    <span style="visibility: hidden;">SELECT COLLEGE</span>
                    <asp:DropDownList ID="ddlcollege" runat="server" Width="500px" Height="30px" Visible="false"
                        CssClass="textbox">
                    </asp:DropDownList>
                    <%-- <div style="text-align: right; width: 90%;">
                        <asp:CheckBox ID="cbinstruction" runat="server" OnCheckedChanged="cbinstruction_Click"
                            AutoPostBack="true" Font-Bold="true" onchange="show(this)" Font-Size="Medium"
                            ForeColor="Green" Text="Instruction" />
                    </div>--%>
                </div>
                <table>
                    <tr>
                        <td>
                            <fieldset style="height: 11px;">
                                <asp:RadioButtonList ID="rbSelMode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                    OnSelectedIndexChanged="rbSelMode_Selected">
                                    <asp:ListItem Text="Degree" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="College" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Education level" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </fieldset>
                        </td>
                        <td colspan="2">
                            <fieldset style="height: 11px;">
                                <asp:RadioButton ID="rdb_applicationno" runat="server" GroupName="rr" Text="Application No Generation"
                                    AutoPostBack="true" OnCheckedChanged="rdb_applicationno_CheckedChanged" Checked="true" />
                                <%-- </td>
                    <td>--%>
                                <asp:RadioButton ID="rdb_admissionno" runat="server" GroupName="rr" Text="Admission No Generation"
                                    AutoPostBack="true" OnCheckedChanged="rdb_admissionnoCheckedChanged" />
                            </fieldset>
                        </td>
                        <td>
                            <fieldset style="height: 11px;" id="fdChkSeat" runat="server">
                                <asp:CheckBox ID="cbSeatType" runat="server" AutoPostBack="true" OnCheckedChanged="cbSeatType_OnCheckedChanged"
                                    Text="Seat Type" />
                            </fieldset>
                        </td>
                        <td>
                         <fieldset style="height: 11px;" id="Fieldsetincludeclg" runat="server" visible="false">
                                <asp:CheckBox ID="cb_includeclg" runat="server"
                                    Text="Include College" AutoPostBack="true" OnCheckedChanged="cb_includeclg_Changed" />
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <div id="sub" runat="server" visible="false">
                    <div style="width=100%; height: 28px; background-color: Brown;">
                        <span style="color: White; font-size: large; font-weight: bold;">Select College Type</span>
                    </div>
                    <center>
                        <%--<asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="Large" ForeColor="Brown"
                        Text="Select Education Level"></asp:Label>--%>
                    </center>
                    <br />
                    <center>
                        <%--degreewise--%>
                        <asp:GridView ID="typegrid" runat="server" Visible="false" AutoGenerateColumns="False"
                            OnRowDataBound="typegrid_OnRowDataBound" OnDataBound="typebound" OnRowCommand="gridMembersList_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stream / Shift">
                                    <ItemTemplate>
                                        <asp:Label ID="typelnk1" runat="server" CssClass="pont" ForeColor="Blue" Text='<%# Eval("collname") %>'></asp:Label>
                                        <asp:Label ID="typeextendlilnk" runat="server" CssClass="pont" Visible="false" ForeColor="Blue"
                                            Text='<%# Eval("college_code") %>' Font-Underline="true"></asp:Label>
                                        <%--<asp:LinkButton ID="typelnk1" runat="server" Text='<%# Eval("type") %>' Visible="false"></asp:LinkButton>
                                    <asp:LinkButton ID="typeextendlilnk" runat="server" Text=""></asp:LinkButton>--%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Instruction" Visible="false">
                                    <ItemTemplate>
                                        <%--<asp:LinkButton ID="lnk_typeinstruction" runat="server" Text="Click here"></asp:LinkButton>--%>
                                        <asp:Label ID="lnk_typeinstruction" runat="server" CssClass="pont" ForeColor="Blue"
                                            Text="Click here" Font-Underline="true"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <%--collegewise--%>
                        <asp:GridView ID="gdclgwise" runat="server" Visible="false" AutoGenerateColumns="False"
                            OnSelectedIndexChanged="gdclgwise_SelectedIndexChanege" OnDataBound="gdclgwise_OnDataBound"
                            OnRowDataBound="gdclgwise_OnRowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsnos" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stream / Shift">
                                    <ItemTemplate>
                                        <asp:Label ID="lblclgname" runat="server" CssClass="pont" ForeColor="Blue" Text='<%# Eval("collname") %>'></asp:Label>
                                        <asp:Label ID="lblclgcode" runat="server" CssClass="pont" Visible="false" ForeColor="Blue"
                                            Text='<%# Eval("college_code") %>' Font-Underline="true"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Starting No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblnotgent" runat="server" Text="Not Generated" ForeColor="Red"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Generation">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="applylnk" runat="server" CommandName="App" Text="Generate"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <%--Education level wise--%>
                        <asp:GridView ID="grideduwise" runat="server" Visible="false" AutoGenerateColumns="False"
                            OnSelectedIndexChanged="grideduwise_SelectedIndexChanege" OnDataBound="grideduwise_OnDataBound"
                            OnRowDataBound="grideduwise_OnRowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsnos" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Education Level">
                                    <ItemTemplate>
                                        <asp:Label ID="lblclgname" runat="server" CssClass="pont" ForeColor="Blue" Text='<%# Eval("Edu_Level") %>'></asp:Label>
                                        <asp:Label ID="lblclgcode" runat="server" CssClass="pont" Visible="false" ForeColor="Blue"
                                            Text='<%# Eval("Edu_Level") %>' Font-Underline="true"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Starting No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblnotgent" runat="server" Text="Not Generated" ForeColor="Red"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Generation">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="applylnk" runat="server" CommandName="App" Text="Generate"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </center>
                    <br />
                </div>
                <br />
                <div id="divedu" runat="server" visible="true">
                    <div style="width: 100%; height: 25px; background-color: Brown;">
                        <span style="color: White; font-size: large; font-weight: bold;">Select Education Level
                            Settings</span>
                    </div>
                    <center>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    Education Level
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degreename" runat="server" Height="22px" Width="180px" CssClass="textbox textbox1 txtheight"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel7" runat="server" BackColor="White" BorderColor="Black" BorderWidth="1px"
                                                CssClass="multxtpanel" Height="250px" Width="180px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_degreename" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_degreename_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_degreename" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degreename_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_degreename"
                                                PopupControlID="Panel7" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btn_degreename" runat="server" Width="70px" Height="30px" Text="Save"
                                        OnClick="btn_degreename_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <div id="coursediv" runat="server" visible="false">
                        <div style="width: 100%; height: 25px; background-color: Brown;">
                            <span style="color: White; font-size: large; font-weight: bold;">Select Education Level</span>
                        </div>
                        <br />
                        <center>
                            <asp:GridView ID="grid_edulevel" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="grid_edulevel_SelectedIndexChanege"
                                OnRowDataBound="grid_edulevel_OnRowDataBound" OnDataBound="eduleveldatabound"
                                OnRowCommand="grid_edulevel_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Education Level">
                                        <ItemTemplate>
                                            <asp:Label ID="edulevellink" runat="server" Visible="false" ForeColor="Blue" CssClass="pont"
                                                Text='<%# Eval("Edu_Level") %>'></asp:Label>
                                            <asp:Label ID="link_addvalue" runat="server" Text="" CssClass="pont" ForeColor="Blue"
                                                Font-Underline="true"></asp:Label>
                                            <%-- <asp:LinkButton ID="edulevellink" runat="server" Visible="false" CommandName="Education"
                                        Text='<%# Eval("Edu_Level") %>'></asp:LinkButton>
                                    <asp:LinkButton ID="link_addvalue" runat="server" CommandName="Education" Text=""></asp:LinkButton>--%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Instruction" Visible="false">
                                        <ItemTemplate>
                                            <%-- <asp:LinkButton ID="lnk_instruction" runat="server" Text="Click here"></asp:LinkButton>--%>
                                            <asp:Label ID="lnk_instruction" runat="server" CssClass="pont" Text="Click here"
                                                ForeColor="Blue" Font-Underline="true"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </center>
                    </div>
                    <br />
                    <div id="sub2" runat="server" visible="false">
                        <div style="width: 100%; height: 25px; background-color: Brown;">
                            <span style="color: White; font-size: large; font-weight: bold;">Application Number
                                Generation</span>
                        </div>
                        <br />
                        <center>
                            <asp:Label ID="lblscltype" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblstandard" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:GridView ID="Course_gird" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="course_gird_SelectedIndexChanege"
                                OnDataBound="OnDataBound" OnRowDataBound="OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Course Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcoursename" runat="server" Text='<%# Eval("Course_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldeptname" runat="server" Text='<%# Eval("Dept_Name") %>'></asp:Label>
                                            <asp:Label ID="lbldeptcode" runat="server" Visible="false" Text='<%# Eval("Dept_Code") %>'></asp:Label>
                                            <asp:Label ID="lblMode" runat="server" Visible="false" Text='<%# Eval("mode") %>'></asp:Label>
                                            <asp:Label ID="lbltextcode" runat="server" Visible="false" Text='<%# Eval("textcode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="350px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Starting No">
                                        <ItemTemplate>
                                            <%--<asp:LinkButton ID="lnkgno" runat="server" CommandName="App" Text="Generate"></asp:LinkButton>--%>
                                            <asp:Label ID="lblnotgenerate" runat="server" Text="Not Generated" ForeColor="Red"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Generation">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkapply" runat="server" CommandName="App" Text="Generate"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Degree code" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldegreecode" runat="server" Text='<%# Eval("Degree_Code") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="350px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </center>
                    </div>
                </div>
            </div>
            <br />
            <asp:CheckBox ID="showreport" runat="server" Text="Show Report" Font-Bold="true"
                Font-Size="Large" ForeColor="Green" OnCheckedChanged="Showreport_Changed" AutoPostBack="true" />
            <asp:UpdatePanel ID="reportpanel" runat="server">
                <ContentTemplate>
                    <div id="divrpt" runat="server" visible="false">
                        <div id="Reportstep" runat="server" visible="false">
                            <div style="width=100%; height: 28px; background-color: Brown;">
                                <span style="color: White; font-size: large; font-weight: bold;">Applicaiton Number
                                    Generation Report</span>
                            </div>
                            <br />
                            <asp:GridView ID="ReportGrid" runat="server" AutoGenerateColumns="false" Visible="false"
                                OnRowDataBound="report_Databound">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#93a31d">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" ForeColor="Brown" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type" HeaderStyle-BackColor="#93a31d">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcoursename" runat="server" Text='<%# Eval("type") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Education Level" HeaderStyle-BackColor="#93a31d">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldeptname" runat="server" Text='<%# Eval("Edu_Level") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="350px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department Name" HeaderStyle-BackColor="#93a31d">
                                        <ItemTemplate>
                                            <%--<asp:LinkButton ID="lnkgno" runat="server" CommandName="App" Text="Generate"></asp:LinkButton>--%>
                                            <asp:Label ID="deptlable" runat="server" Text='<%#Eval("coursename")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Application Number Acronym" HeaderStyle-BackColor="#93a31d">
                                        <ItemTemplate>
                                            <%-- <asp:LinkButton ID="lnkapply" runat="server" Text='<%#Eval("appcode") %>'></asp:LinkButton>--%>
                                            <asp:Label ID="lblapplicationacr" runat="server" Text='<%#Eval("appcode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Application Number Start Digit" HeaderStyle-BackColor="#93a31d">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstartdigit" runat="server" Text='<%# Eval("app_startwith") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="350px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Application Number Size" HeaderStyle-BackColor="#93a31d">
                                        <ItemTemplate>
                                            <asp:Label ID="lblappsize" runat="server" Text='<%# Eval("app_serial") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="350px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Application Number College Acronym" HeaderStyle-BackColor="#93a31d">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcollacr" runat="server" Text='<%# Eval("app_acr") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="350px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Application Number Department Acronym" HeaderStyle-BackColor="#93a31d">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldeptacr" runat="server" Text='<%# Eval("app_dept_acr") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="350px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Application Number Other Acronym" HeaderStyle-BackColor="#93a31d">
                                        <ItemTemplate>
                                            <asp:Label ID="lblotheracr" runat="server" Text='<%# Eval("app_other_acr") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="350px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Application Number Last Modify Date" HeaderStyle-BackColor="#93a31d">
                                        <ItemTemplate>
                                            <asp:Label ID="lblmodifydate" runat="server" Text='<%# Eval("Modifydate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="350px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <center>
                                <asp:Button ID="export" runat="server" CssClass="textbox textbox1 type" Width="90px"
                                    Height="30px" Text="Print PDF" OnClick="export_Click" />
                            </center>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="export" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <center>
            <div id="step2" runat="server" visible="false" style="height: 80em; z-index: 1000;
                width: 100%; background-color: White; position: absolute; top: 120px; left: 0;">
                <div id="Div1" runat="server" visible="true" style="top: 13.5px; width: 70%; height: 460px;
                    background-color: White; border: 5px solid Brown;">
                    <center>
                        <div style="width=100%; height: 28px; background-color: Brown; text-align: center;">
                            <span style="color: White; font-size: large; font-weight: bold;">Enter Information</span>
                        </div>
                        <br />
                        <asp:UpdatePanel ID="partupdate" runat="server">
                            <ContentTemplate>
                                <div style="width: 100%; background-color: White;">
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span>Batch</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_batch" CssClass="textbox textbox1" runat="server" Width="100px"
                                                    onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<asp:DropDownList ID="ddldegreename" CssClass="textbox3 textbox1" runat="server"
                                    onblur="blurFunction(this)" onfocus="myFunction(this)" Style="width: 160px;">
                                </asp:DropDownList>--%>
                                                <asp:TextBox ID="txt_degree" CssClass="textbox textbox1" runat="server" Width="100px"
                                                    onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldept" runat="server" Text="Branch"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<asp:DropDownList ID="ddlbranch" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction(this)" Style="width: 160px;">
                                </asp:DropDownList>--%>
                                                <asp:TextBox ID="txt_branch" CssClass="textbox textbox1" runat="server" Width="200px"
                                                    onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span>Pervious Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_perviousdate" CssClass="textbox textbox1" runat="server" Width="100px"
                                                    onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_perviousdate"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span>Modify Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_modifydate" CssClass="textbox textbox1" runat="server" Width="100px"
                                                    onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                                <asp:CalendarExtender ID="calender1" runat="server" TargetControlID="txt_modifydate"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbcollegeacr" runat="server" AutoPostBack="true" OnCheckedChanged="cbcollegeacr_Change" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblclgacr" runat="server" Text="College Acronym"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_collegeacr" CssClass="textbox textbox1" runat="server" Enabled="false"
                                                    Width="100px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_collegeacr"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbdeptacr" runat="server" AutoPostBack="true" OnCheckedChanged="cbdeptacr_Change" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblclgs" runat="server" Text="College" Visible="false"></asp:Label>
                                                <asp:Label ID="lbldeptacr" runat="server" Text="Department Acronym"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_deptacr" CssClass="textbox textbox1" runat="server" Enabled="false"
                                                    Width="100px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_deptacr"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbothracr" runat="server" AutoPostBack="true" OnCheckedChanged="cbothracr_Change" />
                                            </td>
                                            <td>
                                                <span>Other Acronym</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_otheracr" CssClass="textbox textbox1" runat="server" Enabled="false"
                                                    Width="100px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_otheracr"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="CheckBox3" Visible="false" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblapplno" runat="server" Text="Application Number Starts With"></asp:Label>
                                                <%-- <span>Application Number Starts With</span>--%>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_serialstartwith" CssClass="textbox textbox1" runat="server"
                                                    Style="text-align: right;" Width="100px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_serialstartwith"
                                                    FilterType="Numbers" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="CheckBox4" Visible="false" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblapplsize" runat="server" Text="Application Number Size"></asp:Label>
                                                <%-- <span>Application Number Size</span>--%>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_serialsize" CssClass="textbox textbox1" runat="server" Width="100px"
                                                    MaxLength="1" Style="text-align: right;" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                                <asp:Label ID="lblSeatMode" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblSeatTextcode" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <div style="text-align: center;">
                                        <asp:Button ID="btngenerate" CssClass="textbox textbox1 type" runat="server" Style="width: 100px;
                                            height: 35px;" Text="Generate" OnClientClick="return validatedeg()" OnClick="Generate_Click" />
                                        <asp:Button ID="btncancel" CssClass="textbox textbox1 type" runat="server" Style="width: 100px;
                                            height: 35px;" Text="Cancel" OnClick="Cancel_Click" />
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btngenerate" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </center>
                </div>
            </div>
            <%-- <asp:HiddenField runat="server" ID="hfdelete" />
        <asp:ModalPopupExtender ID="mpemsgboxdelete" runat="server" BackgroundCssClass="modalPopup"
            TargetControlID="hfdelete" PopupControlID="step2">
        </asp:ModalPopupExtender>--%>
            <%--collegewise setting--%>
            <div id="divclg" runat="server" visible="false" style="height: 80em; z-index: 1000;
                width: 100%; background-color: White; position: absolute; top: 120px; left: 0;">
                <div id="Div3" runat="server" visible="true" style="top: 13.5px; width: 70%; height: 460px;
                    background-color: White; border: 5px solid Brown;">
                    <center>
                        <div style="width=100%; height: 28px; background-color: Brown; text-align: center;">
                            <span style="color: White; font-size: large; font-weight: bold;">Enter Information</span>
                        </div>
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div style="width: 100%; background-color: White;">
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span>Batch</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtclgbatch" CssClass="textbox textbox1" runat="server" Width="100px"
                                                    onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span>Pervious Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtclgpdate" CssClass="textbox textbox1" runat="server" Width="100px"
                                                    onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtclgpdate"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span>Modify Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtclgmdate" CssClass="textbox textbox1" runat="server" Width="100px"
                                                    onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtclgmdate"
                                                    Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbclgacr" runat="server" AutoPostBack="true" OnCheckedChanged="cbclgacr_Change" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="College" Visible="false"></asp:Label>
                                                <asp:Label ID="Label3" runat="server" Text="College Acronym"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtclgacr" CssClass="textbox textbox1" runat="server" Enabled="false"
                                                    Width="100px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtclgacr"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbclgother" runat="server" AutoPostBack="true" OnCheckedChanged="cbclgother_Change" />
                                            </td>
                                            <td>
                                                <span>Other Acronym</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtclgother" CssClass="textbox textbox1" runat="server" Enabled="false"
                                                    Width="100px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtclgother"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbclgappl" Visible="false" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text="Application Number Starts With"></asp:Label>
                                                <%-- <span>Application Number Starts With</span>--%>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtclgappl" CssClass="textbox textbox1" runat="server" Style="text-align: right;"
                                                    Width="100px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtclgappl"
                                                    FilterType="Numbers" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbclgadmis" Visible="false" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text="Application Number Size"></asp:Label>
                                                <%-- <span>Application Number Size</span>--%>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtclgadmis" CssClass="textbox textbox1" runat="server" Width="100px"
                                                    MaxLength="1" Style="text-align: right;" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <div style="text-align: center;">
                                        <asp:Button ID="btnclgGent" CssClass="textbox textbox1 type" runat="server" Style="width: 100px;
                                            height: 35px;" Text="Generate" OnClientClick="return validate()" OnClick="btnclgGent_Click" />
                                        <asp:Button ID="btnclgCan" CssClass="textbox textbox1 type" runat="server" Style="width: 100px;
                                            height: 35px;" Text="Cancel" OnClick="btnclgCan_Click" />
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnclgGent" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </center>
                </div>
            </div>
            <%-- <asp:HiddenField runat="server" ID="HiddenField1" />
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="modalPopup"
            TargetControlID="HiddenField1" PopupControlID="divclg">
        </asp:ModalPopupExtender>--%>
            <%--   end--%>
    </body>
    </html>
</asp:Content>
