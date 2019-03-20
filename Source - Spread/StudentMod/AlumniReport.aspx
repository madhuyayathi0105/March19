<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AlumniReport.aspx.cs" Inherits="StudentMod_AlumniReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <link href="Styles/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function che(id) {
            var value = id.checked;
            var id_Value = document.getElementById("<%=txt_degree.ClientID %>");
            var second_id = document.getElementById("<%=txt_department.ClientID %>");
            if (value == true) {
                id_Value.disabled = false;
                second_id.disabled = false;
                return true;
            }
            else {
                id_Value.disabled = true;
                second_id.disabled = true;
                return true;
            }
        }
        function justclick(id) {
            var counter = 0;
            var value = id.checked;
            if (value == true) {
                var chk = document.getElementById("<%=cbldegree.ClientID %>");
                var checkbox = chk.getElementsByTagName("input");
                for (var i = 0; i < checkbox.length; i++) {
                    if (checkbox[i].checked) {
                        counter++;
                    }
                }
                alert(counter);
            }
            else {

            }

        }
    </script>
    <style type="text/css">
        .textbox
        {
            border: 1px solid #c4c4c4;
            height: 20px;
            width: 160px;
            font-size: 13px;
            text-transform: capitalize;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        .textbox1:hover
        {
            outline: none;
            border: 1px solid #7bc1f7;
            box-shadow: 0px 0px 8px #7bc1f7;
            -moz-box-shadow: 0px 0px 8px #7bc1f7;
            -webkit-box-shadow: 0px 0px 8px #7bc1f7;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="height: auto; width: 1024px; margin-left: auto; margin-right: auto;">
        <div style="width: 100%; height: 20px; text-align: right;">
            <center>
                <span style="font-weight: bold; font-size: large; color: Green;">Alumni Report</span>
            </center>
            <asp:LinkButton ID="lnkback" runat="server" PostBackUrl="~/Default_login.aspx" Text="Back"></asp:LinkButton>
            <asp:LinkButton ID="lnkhome" runat="server" PostBackUrl="~/Default_login.aspx" Text="Home"></asp:LinkButton>
            <asp:LinkButton ID="lnklogout" runat="server" Text="Logout" OnClick="lnk_logout"></asp:LinkButton>
        </div>
        <br />
        <br />
        <center>
            <div style="height: 90px; width: 95%; border: 1px solid lightblue; -webkit-border-radius: 10px;
                -moz-border-radius: 10px; border-radius: 10px; padding: 10px; margin: 0 auto;">
                <table id="Maintable" runat="server" style="position: absolute; margin-left: 0px;
                    margin-top: 10px; line-height: 35px; width: 850px;">
                    <tr>
                        <td>
                            <asp:RadioButton ID="cbapply" runat="server" Font-Size="14px" Text="Attending Student"
                                GroupName="same" />
                        </td>
                        <td>
                            <asp:RadioButton ID="cbnotapply" runat="server" Text="Not Attending Student" GroupName="same" />
                        </td>
                        <td>
                            <span>Stream</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltype" runat="server" Width="120px" Height="30px" OnSelectedIndexChanged="type_Change"
                                AutoPostBack="true" CssClass="textbox textbox1">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <span>Education Level</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddledulevel" runat="server" Width="120px" Height="30px" AutoPostBack="true"
                                OnSelectedIndexChanged="edulevel_SelectedIndexChange" CssClass="textbox textbox1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cbdegreewise" runat="server" Text="Degree" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" runat="server" ReadOnly="true" Enabled="false" Font-Bold="True"
                                        Width="105px" Style="position: absolute; left: 100px; top: 45px;" Font-Names="Book Antiqua"
                                        Font-Size="medium" CssClass="Dropdown_Txt_Box1 textbox textbox1">---Select---</asp:TextBox>
                                    <asp:Panel ID="paneldegree" runat="server" Height="300px" Width="155px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cbdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbdegree_Changed" />
                                        <asp:CheckBoxList ID="cbldegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbldegree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="paneldegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cbldegree" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <%--<asp:CheckBox ID="cbdepartment" runat="server" Text="Department" Style="position: absolute;
                            left: 594px; top: 207px;" />--%>
                            <span style="position: absolute; left: 230px; top: 45px;">Department </span>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_department" runat="server" Enabled="false" ReadOnly="true" Font-Bold="True"
                                        Width="145px" Style="position: absolute; left: 330px; top: 45px;" Font-Names="Book Antiqua"
                                        Font-Size="medium" CssClass="Dropdown_Txt_Box1 textbox textbox1">---Select---</asp:TextBox>
                                    <asp:Panel ID="paneldepartment" runat="server" Height="300px" Width="225px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cbdepartment1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbdepartment_Changed" />
                                        <asp:CheckBoxList ID="cbldepartment" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="cbldepartment_SelectedIndexChanged" Font-Bold="True"
                                            Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_department"
                                        PopupControlID="paneldepartment" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_fromdate" runat="server" Text="From" Style="position: absolute; left: 550px; top: 40px;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px; position: absolute; left: 600px; top: 45px;" 
                                onchange="return checkDate()"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="lbl_todate" runat="server" Text="To" Style="position: absolute; left: 700px; top: 40px;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px; position: absolute; left: 740px; top: 45px;" onchange="return checkDate()"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Button ID="btn_go" runat="server" Text="Go" OnClick="Click" Height="30px" Width="50px"
                                CssClass="textbox textbox1" Style="position: absolute; left: 840px; top: 45px;" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div>
                <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                    HeaderStyle-BackColor="#FFB608">
                </asp:GridView>
            </div>
            <br />
            <br />
            <div>
                <asp:Button ID="btnexcel" runat="server" Text="Export Excel" Visible="false" OnClick="btn_excelClcik"
                    Height="30px" Width="90px" CssClass="textbox textbox1" />
                <asp:Button ID="btn_pdf" runat="server" Text="Print PDF" Visible="false" OnClick="pdf_Click"
                    Height="30px" Width="90px" CssClass="textbox textbox1" />
            </div>
        </center>
    </div>
</asp:Content>
