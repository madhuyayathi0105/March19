<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="ExamHallMaster.aspx.cs" Inherits="ExamHallMaster" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="scrptmngr" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="pnlPageload" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="pnlPageload">
                <ProgressTemplate>
                    <div class="CenterPB" style="height: 40px; width: 40px;">
                        <img src="images/progress2.gif" height="180px" width="180px" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                PopupControlID="UpdateProgress1">
            </asp:ModalPopupExtender>
              <center> <br /> <asp:Label ID="Label7" runat="server" Text="Exam Hall Master Priority" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
           <br />
        
            <asp:Label ID="errmsg" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"
                Font-Names="Book Antiqua"></asp:Label>
          <table>
                <tr>
                    <td>
                        <asp:Label ID="lbltype" runat="server" Font-Bold="true" Text="Mode" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                        <asp:DropDownList ID="ddltype" runat="server" Width="128px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <farpoint:fpspread id="sprdHallMaster" runat="server" verticalscrollbarpolicy="Always"
                            horizontalscrollbarpolicy="Always" onupdatecommand="sprdHallMaster_UpdateCommand"
                            onbuttoncommand="sprdHallMaster_ButtonCommand">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </farpoint:fpspread>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbltots" runat="server" Font-Bold="True" Text="Total Selected Seats :"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <asp:Label ID="lbltotseats" runat="server" Font-Bold="True" Text="0" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:Button ID="btnreset" runat="server" Text="Reset" OnClick="btnreset_Click" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 26px" />
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 26px" />
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

