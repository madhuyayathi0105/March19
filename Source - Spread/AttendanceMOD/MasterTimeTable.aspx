<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="MasterTimeTable.aspx.cs" Inherits="MasterTimeTable" %>
    <%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('<%=lblexer.ClientID %>').innerHTML = "";
        }
        function display1() {
            document.getElementById('<%=lblexer.ClientID %>').innerHTML = "";
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="Label2" runat="server" Text="Master  Time Table" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
    </center>
    <br />
    <center>
        <asp:UpdatePanel ID="Upanel1" runat="server">
            <ContentTemplate>
                <table class="maintablestyle" style="width: 700px; height: 40px; background-color: #0CA6CA;">
                    <tr>
                        <td>
                            <asp:Label ID="Lblclg" runat="server" Text="College" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlclg" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Width="200px" CausesValidation="True"
                                OnSelectedIndexChanged="ddlclg_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" CausesValidation="True"
                                OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                                Font-Bold="True" Font-Names="Book Antiqua" Width="100px" Font-Size="Medium" CausesValidation="True"
                                OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" Width="245px"
                                OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UPGo" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Go" OnClick="btnGo_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <br />
    <asp:UpdatePanel ID="Upanel2" runat="server">
        <ContentTemplate>
            <center>
                <asp:Label ID="lblexer" runat="server" Font-Size="Medium" Font-Bold="true" ForeColor="Red"></asp:Label>
            </center>
            <br />
            <center>
                <asp:GridView ID="gview" runat="server" ShowHeader="false" OnRowDataBound="gview_OnRowDataBound"
                    OnDataBound="gview_OnDataBound" Width="1000">
                    <Columns>
                    </Columns>
                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                    <FooterStyle BackColor="White" ForeColor="#333333" />
                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                    <RowStyle ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                </asp:GridView>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:UpdatePanel ID="Upanel3" runat="server">
        <ContentTemplate>
            <center>
                <asp:Label ID="Lblreport" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcl" runat="server" onkeypress="display()" Height="20px" Width="180px"
                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btnxcl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    OnClick="btnxcl_click" Font-Size="Medium" />
                <asp:Button ID="btnprnt" runat="server" Text="Print" Font-Names="Book Antiqua" Font-Size="Medium"
                    Font-Bold="true" OnClick="btnprnt_Click" />
                    <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                <insproplus:printmaster runat="server" id="Printmaster1" visible="false" />
                <asp:Label ID="lblerr" runat="server" Font-Size="Medium" Font-Bold="true" ForeColor="Red"></asp:Label>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:UpdatePanel ID="Upanel4" runat="server">
        <ContentTemplate>
            <center>
                <asp:GridView ID="gviewstaff" runat="server" ShowHeader="false" OnRowDataBound="gviewstaff_OnRowDataBound"
                    OnDataBound="gviewstaff_OnDataBound" Width="900">
                    <Columns>
                    </Columns>
                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                    <FooterStyle BackColor="White" ForeColor="#333333" />
                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                    <RowStyle ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                </asp:GridView>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:UpdatePanel ID="Upanel5" runat="server">
        <ContentTemplate>
            <center>
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display1()" Height="20px"
                    Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:TextBox>
                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnxl_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
                    Font-Size="Medium" Font-Bold="true" OnClick="btnprintmaster_Click" />
                    <NEW:NEWPrintMater runat="server" ID="NEWPrintMater2" Visible="false" />
                <insproplus:printmaster runat="server" id="Printcontrol" visible="false" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--</div>--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UPGo">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
