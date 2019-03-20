<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="COESubtypePartSettings.aspx.cs" Inherits="COESubtypePartSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Panel ID="header_Panel" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
            Style="width: 1240px; height: 21px">
            <center>
                <asp:Label ID="lbl_Header" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="White" Text="COE Sub-Subject Settings"></asp:Label>
            </center>
        </asp:Panel>
        <table>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblCollege" runat="server" Text="College" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCollege" Width="100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblmonthYear1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Year And Month"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYear1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    OnSelectedIndexChanged="ddlYear1_SelectedIndexChanged" Font-Size="Medium" Width="60px"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMonth1" runat="server" CssClass="font" OnSelectedIndexChanged="ddlMonth1_SelectedIndexChanged"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="60px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldegree1" runat="server" CssClass="font" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="101px" OnSelectedIndexChanged="ddldegree1_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbranch1" runat="server" CssClass="font" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="160px" OnSelectedIndexChanged="ddlbranch1_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Sem"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsem1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="90px" OnSelectedIndexChanged="ddlsem1_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblsubtype" runat="server" Text="Subject Type" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsubtype" AutoPostBack="true" Width="200px" runat="server"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSubject" AutoPostBack="true" Width="407px" runat="server"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="No.of.Sub-Subject" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoPart" Visible="true" Width="45px" runat="server" Font-Bold="True"
                                    Font-Names="Book Antiqua" MaxLength="2" Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtNoPart"
                                    FilterType="numbers,custom" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnviewre" runat="server" Text="Go" OnClick="btnviewre_Click" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="width: 1240px;
            height: 20px;">
        </asp:Panel>
    </center>
    <asp:Label ID="lblerr1" runat="server" Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
        Font-Size="Medium" Visible="false"></asp:Label>
    <br />
    <center>
        <asp:Label ID="lblMin" runat="server" Text="Min Mark: " Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="false"></asp:Label>
        <asp:Label ID="lblminMark" runat="server" Font-Bold="True" ForeColor="Blue" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="false"></asp:Label>
        <asp:Label ID="lblMax" runat="server" Text="Max Mark: " Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="false"></asp:Label>
        <asp:Label ID="lblMaxMark" runat="server" Font-Bold="True" ForeColor="Blue" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="false"></asp:Label>
      
    </center>
    <center>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
            HeaderStyle-BackColor="#0CA6CA" BackColor="White">
            <Columns>
                <asp:TemplateField HeaderText="Part">
                    <ItemTemplate>
                        <asp:Label ID="lblPart" runat="server" Text='<%# Eval("PartName") %>' />
                    </ItemTemplate>
                    <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#F8B7B3" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub-Subject Name">
                    <ItemTemplate>
                        <asp:TextBox ID="txtgMarks" runat="server" Text='<%# Eval("Part") %>'></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Max Mark">
                    <ItemTemplate>
                        <asp:TextBox ID="txtMaxMark" runat="server" Text='<%# Eval("maxMark") %>'></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </center>
    <br />
    <center>
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="textbox btn2" Width="80px"
            Height="30px" Visible="false" OnClick="btnSave_Click" BackColor="#76D7C4" />
    </center>
</asp:Content>
