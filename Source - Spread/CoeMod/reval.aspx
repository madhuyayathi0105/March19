<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="reval.aspx.cs" Inherits="reval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

 <style type="text/css">
        .style1
        {
            width: 100px;
            height: 25px;
        }
        .style2
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript">

        function validation() {
            var error = "";

            var exammth = document.getElementById("<%=exammnth.ClientID %>");
            var ddlyr = document.getElementById("<%=ddlyear.ClientID %>");
            var ddltyp = document.getElementById("<%=ddltyp.ClientID %>");
            //0alert(ddlyr.value);
            if (exammth.value == "0") {
                error += "Please Select Exam Month \n";

            }
            if (ddlyr.value == "0") {

                error += "Please Select Year \n";

            }
            if (ddltyp.value == "0") {

                error += "Please Select Type \n";
            }
            if (error != "") {

                alert(error);
                return false;
            }
            else {
                return true;
            }

        }

        //        function validation1() {
        //            var error = "";
        //            
        //            
        //            if (error != "") {
        //                alert(error);
        //                return false;
        //            }
        //            else {
        //                return true;
        //            }

        //        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
        <br />
            <center>
            <asp:Label ID="lbl_msg" runat="server"  Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green" Text="Revaluation/Retotal Application" ></asp:Label>
           
            </center>
            <br />
           
                <table style="width: 800px; height: 50px; background-color: #0CA6CA;">
                    <tr>
                        <td>
                             <asp:Label ID="lblcollege" runat="server" Text="Exam Month" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="exammnth" runat="server" AutoPostBack="true" CssClass="style1"
                                Font-Names="Book Antiqua" Font-Size="medium" OnSelectedIndexChanged="exammnth_OnSelectedIndexChanged" Font-Bold="True"  >
                            </asp:DropDownList>
                        </td>
                        <td>
                             <asp:Label ID="Label1" runat="server" Text="Year" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlyear" runat="server" AutoPostBack="true" CssClass="style1"
                                Font-Names="Book Antiqua" Font-Size="medium" OnSelectedIndexChanged="ddlyear_OnSelectedIndexChanged" Font-Bold="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblty" runat="server" Text="Type"  Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true" Visible="true"></asp:Label> 
                                </td>
                                <td>
                            <asp:DropDownList ID="ddltyp" runat="server" CssClass="style1" Font-Names="Book Antiqua" AutoPostBack="true"
                                Font-Size="medium" Width="165px" Visible="true" OnSelectedIndexChanged="ddltyp_OnSelectedIndexChanged" Font-Bold="true">
                                <asp:ListItem Value="0">---Select---</asp:ListItem>
                                <asp:ListItem Value="1">Photocopy Report</asp:ListItem>
                                <asp:ListItem Value="2">Retotaling Report</asp:ListItem>
                                <asp:ListItem Value="3">Revaluation Report</asp:ListItem>
                                <asp:ListItem Value="4">Supplementary Report</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbldop" runat="server" Text="Last Date" Font-Bold="True" 
                                Font-Names="Book Antiqua" Font-Size="medium">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdop" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="75px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtdop" Format="dd/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Button ID="butgo" runat="server" Text="Go" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="38px" Height="30px"
                                OnClientClick="return validation()" OnClick="butgo_Click" />
                        </td>
                    </tr>
                </table>
         
        </center>
    </div>
    
    
    <center>
        <asp:GridView ID="gridview1" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
            Font-Size="medium" Style="height: 70px; width: 960px;" OnRowDataBound="gridview_databoud"
            OnRowCommand="gridview2" OnDataBound="OnDataBound">
            <HeaderStyle BackColor="#3DAF98" Height="38px" Font-Size="medium" ForeColor="White"
                Font-Names="Book Antiqua" />
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <center>
                            <asp:Label ID="gd_sno" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Year">
                    <ItemTemplate>
                        <center>
                            <asp:Label ID="gd_yr" runat="server" Text='<%#Eval("Branch") %>'></asp:Label>
                        </center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Degree">
                    <ItemTemplate>
                        <center>
                            <asp:Label ID="gd_degcr" runat="server" Visible="true" Text='<%#Eval("Course_Name") %>'></asp:Label>
                        </center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Branch">
                    <ItemTemplate>
                        <asp:Label ID="gd_branch" runat="server" Visible="true" Text='<%#Eval("department") %>'></asp:Label>
                        <asp:Label ID="lbldegree_code" runat="server" Visible="false" Text='<%#Eval("Degree") %>'></asp:Label>
                        <%--             <asp:Label ID="gd_branchcode" runat="server" Visible="false" Text='<%#Eval("Dept_Code") %>'></asp:Label>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Semester">
                    <ItemTemplate>
                        <center>
                            <%--<asp:Label ID="gd_sem1" runat="server" Visible="true" Text='<%#Eval("Semester") %>'></asp:Label>--%>
                            <asp:Label ID="gd_sem" runat="server" Visible="true" Text='<%#Eval("Current_Semester") %>'></asp:Label>
                        </center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Student">
                    <ItemTemplate>
                        <center>
                            <asp:Label ID="gd_totstd" runat="server" Visible="true" Text='<%#Eval("total") %>'></asp:Label>
                        </center>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </center>
    
    <center>
        <asp:GridView ID="gridviewrow" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
            Font-Size="medium" Style="height: 80px; width: 600px;">
            <HeaderStyle BackColor="#3DAF98" Height="38px" Font-Size="medium" ForeColor="White"
                Font-Names="Book Antiqua" />
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <center>
                            <asp:Label ID="gd_sno" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </center>
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Register Number">
                    <ItemTemplate>
                        <asp:Label ID="lblreg" runat="server" Visible="true" Text='<%#Eval("Reg_No") %>'></asp:Label>
                        <asp:Label ID="lblroll" runat="server" Visible="false" Text='<%#Eval("Roll_No") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="120px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Student Name">
                    <ItemTemplate>
                        <asp:Label ID="lblstu" runat="server" Visible="true" Text='<%#Eval("Student_Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Select">
                    <HeaderTemplate>
                        <center>
                            <asp:CheckBox ID="cbselectall" ItemStyle-VerticalAlign="Top" CssClass="gridCB" runat="server"
                                Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="cbselectall_change">
                            </asp:CheckBox>
                        </center>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <center>
                            <asp:CheckBox ID="cbSelect" CssClass="gridCB" Font-Names="Book Antiqua" Font-Size="Medium"
                                runat="server"></asp:CheckBox>
                        </center>
                    </ItemTemplate>
                    <ItemStyle Width="30px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </center>
    
    
    <center>
        <table style="margin-top: -3px; margin-left: -160px;">
            <tr>
                <%-- <td>
                    <asp:Label ID="lblty" runat="server" Text="Type" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true" Visible="false"></asp:Label>
                    <asp:DropDownList ID="ddltyp" runat="server" CssClass="style1" Font-Names="Book Antiqua"
                        Font-Size="medium" Width="165px" Visible="false">
                        <asp:ListItem Value="0">---Select---</asp:ListItem>
                        <asp:ListItem Value="1">Photocopy Report</asp:ListItem>
                        <asp:ListItem Value="2">Retotaling Report</asp:ListItem>
                        <asp:ListItem Value="3">Revaluation Report</asp:ListItem>
                        <asp:ListItem Value="4">Supplementary Report</asp:ListItem>
                    </asp:DropDownList>
                </td>--%>
            </tr>
        </table>
        <table style="margin-top: -32px; margin-left: 160px;">
        
        
            <tr>
                <td>
                    <asp:Button ID="butgen" runat="server" Font-Bold="true" Visible="false" Text="Generate"
                        Style="width: 93px; height: 28px;" OnClick="butgen_Click" />
                </td>
            </tr>
        </table>
    </center>
</asp:Content>

