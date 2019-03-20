<%@ Page Title="" Language="C#" MasterPageFile="~/ChartMOD/ChartSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="staffcutoffchart.aspx.cs" Inherits="staffcutoffchart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function fnc(value, min, max) {
            if (parseInt(value) < 0 || isNaN(value))
                return 0;
            else if (parseInt(value) > 100)
                return "Number is greater than 100";
            else return value;
        }
  
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="lbl_Header" runat="server" CssClass="fontstyleheader" ForeColor="Green"
            Text="Staff CutOff Chart"></asp:Label></center>
    <br />
    <center>
        <table style="width: 900px; height: 70px;" class="maintablestyle ">
            <tr>
                <td>
                    <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="labbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbatch" runat="server" Height="22px" Width="78px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddldegree" runat="server" Height="22px" Width="90px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbldept" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddldept" runat="server" Height="22px" Width="150px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddldept_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsem" runat="server" Height="22px" Width="71px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DropDownList1" runat="server" Height="22px" Width="71px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbltest" runat="server" Text="Test" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DropDownList2" runat="server" Height="22px" Width="90px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblcutoff" runat="server" Text="Cut Off" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="60px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtcutoff" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="83px" onkeyup="this.value = fnc(this.value, 0, 100)"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="filtertextbox1" runat="server" FilterType="Numbers"
                        TargetControlID="txtcutoff">
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pndes1" runat="server" BorderColor="Black" BorderWidth="1px">
                        <asp:CheckBox ID="chkboxsms" runat="server" Text="SMS" OnCheckedChanged="chkboxsms_CheckedChangeds"
                            Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                            font-weight: bold;" Font-Names="Book Antiqua" />
                        <asp:CheckBox ID="chkboxmail" runat="server" Text="MAIL" OnCheckedChanged="chkboxmail_CheckedChanged"
                            Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                            font-weight: bold;" Font-Names="Book Antiqua" />
                        <asp:CheckBox ID="chknotification" runat="server" Text="Notification" AutoPostBack="true"
                            OnCheckedChanged="chknotification_CheckedChanged" Style="display: inline-block;
                            color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;" />
                    </asp:Panel>
                </td>
                <td colspan="3">
                    <asp:Panel ID="Panel4" runat="server" BorderColor="Black" BorderWidth="1px" Width="300px">
                        <asp:RadioButtonList ID="radiobutton1" runat="server" RepeatDirection="Horizontal"
                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" AutoPostBack="true"
                            OnSelectedIndexChanged="radiobutton1_selectedindexchanged">
                            <asp:ListItem Selected>CAM Wise</asp:ListItem>
                            <asp:ListItem>University Wise</asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua" OnClick="btngo_Click" />
                </td>
            </tr>
        </table>
    </center>
    <br />
    <table>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="Red" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
    <center>
        <asp:Panel ID="panel2" runat="server">
            <asp:Chart ID="datacam1" runat="server" Height="500px" Width="900px" OnClick="datacam1_Click"
                Visible="true">
                <Series>
                    <asp:Series Name="Series1" PostBackValue="#VALY">
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="0">
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="labheading" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                            Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <FarPoint:FpSpread ID="fpcammarkstaff" runat="server" Visible="false" OnUpdateCommand="fpcammarkstaff_UpdateCommand">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonType="PushButton"
                    ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark" Visible="false">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </asp:Panel>
    </center>
    <%--  <table>
                <tr>
                    <td>
                        <asp:Label ID="labpurpose" runat="server" Text="Purpose" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlpurpose" runat="server" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="false" OnSelectedIndexChanged="ddlpurpose_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <FarPoint:FpSpread ID="fpspreadpurpose" runat="server" Visible="false" OnCellClick="fpspreadpurpose_CellClick"
                            OnPreRender="fpspreadpurpose_SelectedIndexChanged">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonType="PushButton"
                                ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark" Visible="false">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </td>
                </tr>
            </table>--%>
    <%-- <asp:Panel ID="templatepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                Visible="false" BorderWidth="2px" Height="390px" Width="690px">
                <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: MS Sans Serif;
                    font-size: Small; font-weight: bold">
                    <table>
                        <caption>
                            <br />
                            <br />
                            <br />
                            <caption>
                                Message Template</caption>
                            <tr>
                                <td>
                                    <asp:Label ID="lblpurpose" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Black" Text="Purpose" Width="100px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btnplus" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnplus_Click" Text=" + " />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlpurposemsg" runat="server" AutoPostBack="True" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="200px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnminus" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnminus_Click" Text=" - " />
                                </td>
                            </tr>
                        </caption>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtpurposemsg" runat="server" TextMode="MultiLine" Height="200px"
                                    Width="680px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnsave_Click" Height=" 26px" Width=" 88px" />
                            </td>
                            <td>
                                <asp:Button ID="btnexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height=" 26px" Width=" 88px" OnClick="btnexit_Click" />
                            </td>
                        </tr>
                        <caption>
                            <br />
                            <br />
                            <br />
                            <tr>
                                <td>
                                    <asp:Label ID="lblerror" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red" Style="top: 340px; left: 5px; position: absolute;
                                        height: 21px" Width="676px"></asp:Label>
                                </td>
                            </tr>
                        </caption>
                    </table>
                </div>
            </asp:Panel>
    --%>
    <%-- <asp:Panel ID="purposepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                Visible="false" BorderWidth="2px" Height="100px" Width="300px">
                <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                    font-size: Small; font-weight: bold">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Purpose Type" Style="text-align: center;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblpurposecaption" runat="server" Text="Purpose" Style="font-size: medium;
                                    font-weight: bold; height: 22px; font-family: 'Book Antiqua'; position: absolute;
                                    top: 21px; left: 10px;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpurposecaption" runat="server" Style="font-size: medium; font-weight: bold;
                                    height: 22px; font-family: 'Book Antiqua';"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnpurposeadd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                    height: 26px;" OnClick="btnpurposeadd_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnpurposeexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                    height: 26px; width: 88px;" OnClick="btnpurposeexit_Click" />
                            </td>
                        </tr>
                    </table>
            </asp:Panel>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnaddtemplate" runat="server" Text="Add Template" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnaddtemplate_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btndeletetemplate" runat="server" Text="Delete Template" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btndeletetemplate_Click" />
                    </td>
                </tr>
            </table>--%>
    <center>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblerror" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtmessage" runat="server" TextMode="MultiLine" Height="200px" Width="500px"
                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnsms" runat="server" Text="Send" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnsms_Click" />
                </td>
                <%--<td>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                    </td>--%>
            </tr>
        </table>
    </center>
    <center>
        <table id="Tablenote" runat="server">
            <tr>
                <td>
                </td>
                <td>
                    <asp:Label ID="lblnotification" Text="Notification Feed" Font-Size="Medium" Font-Names="Book Antiqua"
                        runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblsubject" Text="Subject" runat="server" Font-Bold="true" Style="display: inline-block;
                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                        width: 90px;"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtsubject" runat="server" Style="display: inline-block; color: Black;
                        font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 500px;"></asp:TextBox>
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
                        font-weight: bold; width: 500px; height: 300px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblfile" Text="Photos" runat="server" Font-Bold="true" Style="font-family: Book Antiqua;
                        font-size: medium; font-weight: bold; width: 90px;"></asp:Label>
                </td>
                <td>
                    <asp:FileUpload ID="fudfile" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblattachements" Text="Attachements" runat="server" Font-Bold="true"
                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 90px;" />
                </td>
                <td>
                    <asp:FileUpload ID="fudattachemnts" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnnotfsave" OnClick="btnnotfsave_Click" Text="Notification Send"
                        runat="server" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" />
                </td>
            </tr>
        </table>
    </center>
    </ContentTemplate> </asp:UpdatePanel>
</asp:Content>
