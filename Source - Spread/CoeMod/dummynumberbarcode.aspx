<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="dummynumberbarcode.aspx.cs" Inherits="dummynumberbarcode" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scrptmngr" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function OnGenerateSelectCheck() {
            var monId = document.getElementById("<%=ddlMonth.ClientID %>").value.trim();
            var yearId = document.getElementById("<%=ddlYear.ClientID %>").value.trim();
            if (monId != '' && yearId != '' && monId != '0' && yearId != '0') {
                var genType = document.getElementById("<%=ddlGenType.ClientID %>").value.trim();

                if (genType != "Common") {
                    var subVal = document.getElementById("<%=ddlsubject.ClientID %>").value.trim();
                    if (subVal != '' && subVal != '0') {
                        return true;
                    } else {
                        alert('Please Select Subject');
                        return false;
                    }
                }
                return true;
            }
            alert('Please Select Month & Year');
            return false;
        }
        function OnMapCheck() {
            var id = document.getElementById("<%=gridDummy.ClientID %>");
            if (id == null) {
                alert('No Mapped Numbers Selected');
                return false;
            }
            else {
                var len = id.rows.length;
                if (len > 0) {
                    return true;
                }
                else {
                    alert('No Mapped Numbers Selected');
                    return false;
                }
            }
        }
    </script>
    <center>
        <span class="fontstyleheader" style="font-size: large; color: Green;">Dummy Number Generation</span>
        <div class="maindivstyle">
            <table class="maintablestyle">
                <tr>
                    <td>
                        <%-- <asp:UpdatePanel ID="UP_college" runat="server">
                            <ContentTemplate>--%>
                        <asp:TextBox ID="txt_College" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                            placeholder="College"></asp:TextBox>
                        <asp:Panel ID="panel_college" runat="server" CssClass="multxtpanel">
                            <asp:CheckBox ID="cb_College" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                OnCheckedChanged="cb_College_CheckedChanged" />
                            <asp:CheckBoxList ID="cbl_College" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_College_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="popupce_sem" runat="server" TargetControlID="txt_College"
                            PopupControlID="panel_college" Position="Bottom">
                        </asp:PopupControlExtender>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </td>
                    <td>
                        <asp:Label ID="lblMonthandYear" runat="server" Text="Year & Month"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="textbox ddlheight" Width="60px"
                            OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="True" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="textbox ddlheight" Width="60px"
                            OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblGenType" runat="server" Text="Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGenType" runat="server" CssClass="textbox ddlheight" Width="100px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlGenType_IndexChange">
                            <asp:ListItem>Common</asp:ListItem>
                            <asp:ListItem>Subject Wise</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblGenMethod" runat="server" Text="Mode"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGenMethod" runat="server" CssClass="textbox ddlheight" Width="90px"
                            AutoPostBack="true">
                            <asp:ListItem>Serial</asp:ListItem>
                            <asp:ListItem>Random</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="Generatebtn" runat="server" CssClass="textbox btn" Width="100px"
                            Text="Generate" OnClick="btnGenerate_Click" OnClientClick="return OnGenerateSelectCheck();" />
                    </td>
                    <td>
                        <asp:Button ID="Viewbtn" runat="server" CssClass="textbox btn" Width="60px" Text="View"
                            OnClick="Viewbtn_Click" OnClientClick="return OnGenerateSelectCheck();" />
                    </td>
                    <td>
                        <asp:Button ID="btnDummyMap" runat="server" CssClass="textbox btn" Width="120px"
                            Text="Save Mapped" OnClick="btnDummyMap_Click" OnClientClick="return OnMapCheck();" />
                    </td>
                </tr>
                <tr id="trSubjectDet" runat="server" visible="false">
                    <td colspan="10">
                        <asp:Label ID="lblExDate" runat="server" Text="Exam Date"></asp:Label>
                        <asp:DropDownList ID="ddlExDate" runat="server" CssClass="textbox ddlheight" Width="100px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlExdate_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="lblsession" runat="server" Text="Session"></asp:Label>
                        <asp:DropDownList ID="ddlsession" runat="server" CssClass="textbox ddlheight" AutoPostBack="True"
                            Width="70px" OnSelectedIndexChanged="ddlsession_SelectedIndexChanged">
                            <asp:ListItem>Both</asp:ListItem>
                            <asp:ListItem>F.N</asp:ListItem>
                            <asp:ListItem>A.N</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblsubject" runat="server" Text="Subject"></asp:Label>
                        <asp:DropDownList ID="ddlsubject" runat="server" CssClass="textbox ddlheight" Width="400px"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblnorec" runat="server" Text="No Records Found" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Small" ForeColor="#FF3300" Visible="False"></asp:Label>
            <br />
            <asp:GridView ID="gridDummy" runat="server" AutoGenerateColumns="false" GridLines="Both"
                Visible="false" HeaderStyle-BackColor="#0CA6CA" Style="text-align: center;">
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <center>
                                <asp:Label ID="lblSerial" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                <asp:Label ID="lblClgCode" runat="server" Text='<%#Eval("CollegeCode") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblDegCode" runat="server" Text='<%#Eval("DegreeCode") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblBatchYear" runat="server" Text='<%#Eval("BatchYear") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblCurSem" runat="server" Text='<%#Eval("CurSem") %>' Visible="false"></asp:Label>
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dummy Number">
                        <ItemTemplate>
                            <center>
                                <asp:Label ID="lblDummyNo" runat="server" Text='<%#Eval("DummyNumber") %>'></asp:Label>
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Roll Number" Visible="false">
                        <ItemTemplate>
                            <center>
                                <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNumber") %>'></asp:Label>
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Registered Number">
                        <ItemTemplate>
                            <center>
                                <asp:Label ID="lblRegNo" runat="server" Text='<%#Eval("RegNumber") %>'></asp:Label>
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
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
