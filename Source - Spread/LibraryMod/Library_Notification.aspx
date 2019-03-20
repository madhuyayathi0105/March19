<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Library_Notification.aspx.cs" Inherits="LibraryMod_Library_Notification" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <span class="fontstyleheader" style="color: Green;">Library Notification</span><br />
    </center>
    <br />
    <div>
        <center>
            <div style="width: 900px; font-family: Book Antiqua; font-weight: bold; height: auto">
                <table class="maintablestyle" style="width: 518px; height: auto; font-family: Book Antiqua;
                    font-weight: bold; padding: 6px; margin: 0px; margin-bottom: 15px; margin-top: 10px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblst" runat="server" Text="From Date" Font-Bold="True" ForeColor="Black"
                                Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" Style="display: inline-block;
                                color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtstartdate" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="22px" Style="font-family: Book Antiqua; font-size: medium;
                                font-weight: bold; height: 22px; width: 100px;"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtstartdate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="ft1" runat="server" TargetControlID="txtstartdate"
                                FilterType="Custom,Numbers" ValidChars="/">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblet" runat="server" Text="To Date" Width="60px" Font-Bold="True"
                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="display: inline-block;
                                color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                width: 60px;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtenddate" runat="server" Width="80px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="22px" Style="font-family: Book Antiqua; font-size: medium;
                                font-weight: bold; height: 22px;"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="d/MM/yyyy" TargetControlID="txtenddate">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtenddate"
                                FilterType="Custom,Numbers" ValidChars="/">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnMainGo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btnMainGo_Click" />
                        </td>
                        <td>
                            <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/LibImages/Add.jpg" OnClick="btnAdd_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </center>
    </div>
    <br />
    <center>
       
        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
        <asp:GridView ID="grdLibNotification" runat="server" ShowFooter="false" AutoGenerateColumns="true"
            Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="50"
            OnSelectedIndexChanged="grdLibNotification_onselectedindexchanged" OnPageIndexChanging="grdLibNotification_onpageindexchanged"
            OnRowCreated="grdLibNotification_OnRowCreated" Width="980px">
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                        </asp:Label></center>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
        </asp:GridView>
    </center>
    <br />
    <br />
    <div style="text-align: center;">
        <%-- <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" onkeypress="display()"
            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium"></asp:TextBox>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
        <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="#FF3300" Visible="False" Style="" CssClass="style50"></asp:Label>
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        <br />--%>
        <asp:Label ID="norecordlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Width="676px" Text=""></asp:Label>
    </div>
    <center>
        <div id="divTarvellerEntryDetails" runat="server" visible="false" style="height: 70em;
            z-index: 100; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
            top: 0%; left: 0px;">
            <center>
                <%--left: 15%; right: 15%; position: absolute;--%>
                <div id="divTarvellerEntry" runat="server" class="table" style="background-color: White;
                    border: 5px solid #0CA6CA; border-top: 20px solid #0CA6CA; margin-left: auto;
                    margin-right: auto; width: 830px; height: 600px; z-index: 1000; border-radius: 5px;">
                    <asp:Label ID="lbl_clgT" runat="server" Visible="false"></asp:Label>
                    <center>
                        <span style="top: 20px; bottom: 20px; text-align: center; color: Green; font-size: large;
                            position: relative; font-weight: bold;">Library Notification </span>
                    </center>
                    <br />
                    <br />
                    <asp:Label ID="errnote" runat="server" Style="" ForeColor="Red" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <%--<asp:UpdatePanel ID="UpdatedAdd" runat="server">
                    
                                    <ContentTemplate>  --%>
                    <center>
                        <table id="Tablenote" runat="server">
                            <tr>
                                <td>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblnotification" Text="Notification Feed" Font-Size="Large" Font-Names="Book Antiqua"
                                        runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsubject" Text="Subject" runat="server" Font-Bold="true" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: 90px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsubject" runat="server" border-color="black" Style="display: inline-block;
                                        color: Black; border-width: thin; border-color: Black; font-family: Book Antiqua;
                                        font-size: medium; font-weight: bold; width: 500px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblnote" Text="Notification" runat="server" Font-Bold="true" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: 90px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtnotification" TextMode="MultiLine" runat="server" MaxLength="4000"
                                        Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                        border-width: thin; border-color: Black; font-weight: bold; width: 500px; height: 300px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblfile" Text="Photos" runat="server" Font-Bold="true" Style="font-family: Book Antiqua;
                                        font-size: medium; font-weight: bold; margin-left: 0px; width: 90px" text-align="left"></asp:Label>
                                    <asp:FileUpload ID="fudfile" runat="server" />
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblattachements" Text="Attachements" runat="server" Font-Bold="true"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 90px;
                                        text-align: left" />
                                    <asp:FileUpload ID="fudattachemnts" runat="server" />
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <%----  <td>
                                        <asp:Button ID="btnnotfsave" OnClick="btnnotfsave_Click" Text="Notification Send"
                                            runat="server" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" />
                                    </td>---%>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <center>
                        <asp:Button ID="btnsend" OnClick="btnsend_Click" Text="Send" runat="server" Style="font-family: Book Antiqua;
                            font-size: medium; font-weight: bold;" />
                        <asp:Button ID="btndelete" Text="Delete" runat="server" OnClick="btndelete_Click"
                            Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" />
                        <asp:Button ID="btnexit" Text="Exit" runat="server" OnClick="btnexit_Click" Style="font-family: Book Antiqua;
                            font-size: medium; font-weight: bold;" />
                    </center>
                </div>
                <br />
            </center>
        </div>
    </center>
</asp:Content>
