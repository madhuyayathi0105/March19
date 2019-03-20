<%@ Page Title="Student Batch Year Updation" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="BatchYearUpdation.aspx.cs" Inherits="BatchYearUpdation"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green">Student Batch Year Updation</span>
        </div>
        <div style="font-family: Book Antiqua; height: auto; width: auto; padding: 10px;
            margin: 0px; margin-top: 15px; margin-bottom: 15px;">
            <table style="font-family: Book Antiqua; height: auto; padding: 10px; width: auto;
                margin: 0px;" class="maintablestyle">
                <tr>
                    <td>
                        <span>Browse Excel File</span>
                    </td>
                    <td>
                        <asp:FileUpload ID="fuImportExcel" runat="server" ToolTip="Browse Only Excel File" />
                    </td>
                    <td>
                        <asp:Button ID="btnUpload" runat="server" Text="Import" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnUpload_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divImport" runat="server" visible="false">
            <table>
                <tr>
                    <td colspan="3">
                        <asp:GridView ID="gvImport" runat="server" AutoGenerateColumns="false" Width="800px"
                            HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" AllowPaging="false"
                            BackColor="white" CssClass="spreadborder" Style="border-color: #000000;" OnRowDataBound="gvImport_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Register No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegNo" runat="server" ForeColor="Black" Text='<%# Eval("Reg_No") %>'
                                            Visible="true"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="180px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Batch Year">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBatchYear" runat="server" Text='<%# Eval("Batch_Year") %>' Visible="true"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Visible="true"></asp:Label>
                                        <asp:Label ID="lblStatusCode" Visible="false" runat="server" Text='<%# Eval("StatusCode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" BorderColor="Black" VerticalAlign="Middle" Width="130px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnUpdate_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <center>
        <div id="divNotSave" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px; height: 100em;">
            <asp:ImageButton ID="imgbtnClose" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 61px; margin-left: 405px;"
                OnClick="imgbtnClose_Click" />
            <center>
                <div id="divNotInserted" runat="server" style="background-color: White; height: 400px;
                    width: 840px; border: 5px solid #0CA6CA; border-top: 5px solid #0CA6CA; margin-top: 72px;
                    border-radius: 10px;">
                    <asp:Label ID="lbl_upload_suc" runat="server" Visible="false" ForeColor="Blue"></asp:Label>
                    <br />
                    <asp:Label ID="lblNotSave" Visible="true" runat="server" Style="color: Red;" Font-Bold="true"
                        Font-Size="Medium"></asp:Label>
                    <div style="height: 345px; width: 700px; overflow: auto;">
                        <asp:TextBox ID="txtNotSave" TextMode="MultiLine" runat="server" Style="height: 334px;
                            overflow: auto;" Visible="true" Width="650px" ForeColor="Blue" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </center>
        </div>
        <div id="divPopAlert" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px; height: 100em;">
            <center>
                <div id="pnlAlert" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <br />
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" Visible="true" runat="server" Text="" Style="color: Red;"
                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btnPopAlertClose_Click" Text="OK" runat="server" />
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
