<%@ Page Title="C.I.A Mark Entry" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Internalnew.aspx.cs" Inherits="internalnew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblexcelerror').innerHTML = "";
        }
        function display1() {
            document.getElementById('<%=lblexcelerror.ClientID %>').innerHTML = "";
        }
        function get(txt1, mxmark2, mimisubjectmark) {
            var sst = document.getElementById(txt1).value;
            if (sst > mxmark2 || sst < mimisubjectmark) {
                alert('Please Enter Correct Mark' + '(Mark Must be Between -1 and ' + mxmark2 + ' or A|AA|AAA|a|aa|aaa)');
                document.getElementById(txt1).value = '';
            }
        }
        function get(txt2, mxmark2, mimisubjectmark) {
            var sst = document.getElementById(txt2).value;
            if (sst > mxmark2 || sst < mimisubjectmark) {
                alert('Please Enter Correct Mark' + '(Mark Must be Between -1 and ' + mxmark2 + ' or A|AA|AAA|a|aa|aaa)');
                document.getElementById(txt2).value = '';
            }
        }
        function get(txt3, mxmark2, mimisubjectmark) {
            var sst = document.getElementById(txt3).value;
            if (sst > mxmark2 || sst < mimisubjectmark) {
                alert('Please Enter Correct Mark' + '(Mark Must be Between -1 and ' + mxmark2 + ' or A|AA|AAA|a|aa|aaa)');
                document.getElementById(txt3).value = '';
            }
        }
        function get(txt4, mxmark2, mimisubjectmark) {
            var sst = document.getElementById(txt4).value;
            if (sst > mxmark2 || sst < mimisubjectmark) {
                alert('Please Enter Correct Mark' + '(Mark Must be Between -1 and ' + mxmark2 + ' or A|AA|AAA|a|aa|aaa)');
                document.getElementById(txt4).value = '';
            }
        }
        function get(txt5, mxmark2, mimisubjectmark) {
            var sst = document.getElementById(txt5).value;
            if (sst > mxmark2 || sst < mimisubjectmark) {
                alert('Please Enter Correct Mark' + '(Mark Must be Between -1 and ' + mxmark2 + ' or A|AA|AAA|a|aa|aaa)');
                document.getElementById(txt5).value = '';
            }
        }
        function get(txt6, mxmark2, mimisubjectmark) {
            var sst = document.getElementById(txt6).value;
            if (sst > mxmark2 || sst < mimisubjectmark) {
                alert('Please Enter Correct Mark' + '(Mark Must be Between -1 and ' + mxmark2 + ' or A|AA|AAA|a|aa|aaa)');
                document.getElementById(txt6).value = '';
            }
        }
        function get(txt7, mxmark2, mimisubjectmark) {
            var sst = document.getElementById(txt7).value;
            if (sst > mxmark2 || sst < mimisubjectmark) {
                alert('Please Enter Correct Mark' + '(Mark Must be Between -1 and ' + mxmark2 + ' or A|AA|AAA|a|aa|aaa)');
                document.getElementById(txt7).value = '';
            }
        }
        function CheckAllsub(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=gvatte.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[1].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
        function checkMark(mark) {
            var m = mark;
            //            var re = new RegExp("@^(A)?$|^(AA)?$|^(AAA)?$|^(a)?$|^(aa)?$|^(aaa)?$|^-?\d+?$|^-?$");
            var re = new RegExp("@^(A)?$|^(AA)?$|^(AAA)?$|^(a)?$|^(aa)?$|^(aaa)?$");
            if (re.test(m)) {
                return 1;
            } else {
                return 0;
            }
        }
     
    </script>
    <style>
        .style8
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
            top: 190px;
            left: 0px;
        }
        .style10
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
            width: 960px;
            height: 17px;
        }
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 15px;
            margin-top: 15px; position: relative;">C.I.A Mark Entry</span>
    </center>
    <center>
        <table class="maintablestyle" style="margin: 0px; margin-bottom: 15px; margin-top: 15px;
            position: relative;">
            <tr>
                <td>
                    <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="height: 21px; width: 100px;"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBatch" runat="server" Height="20px" Width="59px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" Font-Bold="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                        OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Width="74px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                        Width="190px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px;">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSemYr" runat="server" Text="Sem" Font-Bold="True" Visible="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 20px; width: 33px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Visible="true"
                        OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="height: 21px; width: 44px;">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSec" Visible="false" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlSec_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="height: 21px; width: 47px;">
                    </asp:DropDownList>
                    <asp:UpdatePanel ID="UpnlSec" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSec" Width="79px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                            <asp:Panel ID="pnlSec" runat="server" CssClass="multxtpanel" Style="width: auto;
                                height: 100px; overflow: auto;" Width="120px">
                                <asp:CheckBox ID="chkSec" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSec_CheckedChanged" />
                                <asp:CheckBoxList ID="cblSec" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSec_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="popExtSec" runat="server" TargetControlID="txtSec"
                                PopupControlID="pnlSec" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:DropDownList ID="ddlactivity" runat="server" Visible="false" Style="margin-left: 10px;
                        width: 146px; background-color: #E1E3E4" AutoPostBack="true" OnSelectedIndexChanged="ddlactivity_SelectedIndexChanged"
                        CssClass="fontcomman">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Names="Book Antiqua"
                        ForeColor="Black" Font-Size="Medium" Style="height: 30px; width: 40px" />
                </td>
            </tr>
        </table>
    </center>
    <asp:Label ID="lblErrorMsg" runat="server" Text="" Style="font-family: Book Antiqua;
        color: Red; font-size: medium; font-weight: bold; height: 20px; width: 263px;
        margin: 0px; margin-bottom: 15px; margin-top: 15px; position: relative;"></asp:Label>
    <div id="show1" runat="server" style="margin: 0px; margin-bottom: 20px;">
        <center>
            <asp:Panel ID="pHeaderEntry" runat="server" CssClass="style8" Height="16px" Width="949px">
                <center>
                    <asp:Label ID="Labelpersonal" Text="Subject Details" runat="server" Font-Size="Medium"
                        Font-Bold="True" Font-Names="Book Antiqua" />
                    <asp:Image ID="Imagepersonal" runat="server" AlternateText="" ImageAlign="Right"
                        CssClass="cpimage" ImageUrl="../images/right.jpeg" />
                </center>
            </asp:Panel>
        </center>
        <asp:Panel ID="pnlEntry" runat="server">
            <center>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Visible="false" Height="307" Width="800" OnButtonCommand="FpSpread1_OnButtonCommand"
                            ShowHeaderSelection="false">
                            <CommandBar BackColor="Control" ButtonType="PushButton" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" AutoPostBack="false" GridLineColor="Black">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                        <asp:GridView ID="gvatte" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            Font-Bold="True" Font-Size="Medium" Style="font-family: Book Antiqua; border: 1px solid black;"
                            BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" ">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkallsubject" runat="server" onclick="CheckAllsub(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chksubject" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Batch Year">
                                    <ItemTemplate>
                                        <asp:Label ID="lblbatch" runat="server" Text='<%# Eval("Batch_Year") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subject Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsubj_name" runat="server" Text='<%# Eval("subject_name") %>'></asp:Label>
                                        <asp:Label ID="lblsub_ac" runat="server" Visible="false" Text='<%# Eval("acronym") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="350px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subject Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsubcode" runat="server" Text='<%# Eval("subject_code") %>'></asp:Label>
                                        <asp:Label ID="lblsubno" runat="server" Visible="false" Text='<%# Eval("subject_no") %>'></asp:Label>
                                        <asp:Label ID="lblmaxmark" runat="server" Visible="false" Text='<%# Eval("maxtotal") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Black" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="Black" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Button ID="btnok" runat="server" Text="OK" Visible="false" Font-Names="Book Antiqua"
                    Font-Size="Medium" Width="60px" OnClick="btnok_Click1" />
            </center>
        </asp:Panel>
        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlEntry"
            CollapseControlID="pHeaderEntry" ExpandControlID="pHeaderEntry" AutoExpand="true"
            AutoCollapse="false" TextLabelID="Labelpersonal" CollapsedSize="0" ImageControlID="Imagepersonal"
            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
        </asp:CollapsiblePanelExtender>
    </div>
    <div id="show2" runat="server" style="margin: 0px; margin-bottom: 20px;">
        <div id="posalign" runat="server">
            <center>
                <asp:Panel ID="pHeaderSettings" runat="server" CssClass="style8" Height="16px" Width="949px">
                    <center>
                        <asp:Label ID="Label4" Text="Student Details" runat="server" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua" />
                        <asp:Image ID="Image2" runat="server" AlternateText="" ImageAlign="Right" CssClass="cpimage"
                            ImageUrl="../images/right.jpeg" />
                    </center>
                </asp:Panel>
            </center>
        </div>
        <asp:Panel ID="pnlSettings" runat="server" BorderColor="Black">
            <center>
                <asp:Label ID="lblLockErr" runat="server" Text="" Style="font-family: Book Antiqua;
                    color: Red; font-size: medium; font-weight: bold; height: 20px; width: 263px;
                    margin: 0px; margin-bottom: 15px; margin-top: 10px;"></asp:Label>
                <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                    CssClass="pos" Style="font-family: Book Antiqua; font-size: 14; font-weight: bold;"
                    BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="AsNeeded" HorizontalScrollBarPolicy="AsNeeded"
                    ShowHeaderSelection="false">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <asp:GridView ID="gvmarkentry" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvmarkentry_RowDataBound"
                    BackColor="White" Font-Bold="True" Font-Size="Medium" Style="font-family: Book Antiqua;
                    border: 1px solid black; margin: 0px; margin-top: 10px; margin-bottom: 10px;"
                    BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Roll No">
                            <ItemTemplate>
                                <asp:Label ID="lblroll" runat="server" Text='<%# Eval("roll_no") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Student Name">
                            <ItemTemplate>
                                <asp:Label ID="lblstud_name" runat="server" Text='<%# Eval("stud_name") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblsc1" runat="server" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtm1" runat="server" Width="50px" Style="text-align: center; border: 0px;
                                    background-color: skyblue; font-size: medium; font-weight: normal;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtm1"
                                    FilterType="Numbers,Custom" FilterMode="ValidChars" ValidChars="-1234567890A">
                                </asp:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" BackColor="skyblue" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" ">
                            <ItemTemplate>
                                <asp:Label ID="lblsc2" runat="server" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtm2" runat="server" Width="50px" Style="text-align: center; border: 0px;
                                    background-color: turquoise; font-size: medium; font-weight: normal;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtm2"
                                    FilterType="Numbers,Custom" FilterMode="ValidChars" ValidChars="-1234567890A">
                                </asp:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" BackColor="turquoise" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" ">
                            <ItemTemplate>
                                <asp:Label ID="lblsc3" runat="server" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtm3" runat="server" Width="50px" Style="text-align: center; border: 0px;
                                    background-color: skyblue; font-size: medium; font-weight: normal;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtm3"
                                    FilterType="Numbers,Custom" FilterMode="ValidChars" ValidChars="-1234567890A">
                                </asp:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" BackColor="skyblue" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" ">
                            <ItemTemplate>
                                <asp:Label ID="lblsc4" runat="server" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtm4" runat="server" Width="50px" Style="text-align: center; border: 0px;
                                    background-color: turquoise; font-size: medium; font-weight: normal;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtm4"
                                    FilterType="Numbers,Custom" FilterMode="ValidChars" ValidChars="-1234567890A">
                                </asp:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" BackColor="turquoise" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" ">
                            <ItemTemplate>
                                <asp:Label ID="lblsc5" runat="server" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtm5" runat="server" Width="50px" Style="text-align: center; border: 0px;
                                    background-color: skyblue; font-size: medium; font-weight: normal;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtm5"
                                    FilterType="Numbers,Custom" FilterMode="ValidChars" ValidChars="-1234567890A">
                                </asp:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" BackColor="skyblue" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" ">
                            <ItemTemplate>
                                <asp:Label ID="lblsc6" runat="server" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtm6" runat="server" Width="50px" Style="text-align: center; border: 0px;
                                    background-color: turquoise; font-size: medium; font-weight: normal;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtm6"
                                    FilterType="Numbers,Custom" FilterMode="ValidChars" ValidChars="-1234567890A">
                                </asp:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" BackColor="turquoise" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" ">
                            <ItemTemplate>
                                <asp:Label ID="lblsc7" runat="server" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtm7" runat="server" Width="50px" Style="text-align: center; border: 0px;
                                    background-color: skyblue; font-size: medium; font-weight: normal;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtm7"
                                    FilterType="Numbers,Custom" FilterMode="ValidChars" ValidChars="-1234567890A">
                                </asp:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" BackColor="skyblue" Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Black" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <RowStyle ForeColor="Black" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                </asp:GridView>
                <asp:Label ID="lblexcelerror" runat="server" ForeColor="Red" Visible="false" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Kindly Enter report name"
                    Style="font-family: Book Antiqua; color: Red; font-size: medium; font-weight: bold;
                    height: 20px; width: 263px; margin: 0px; margin-bottom: 15px; margin-top: 10px;"></asp:Label>
                <br />
                <asp:Button ID="btnfpspread1save" runat="server" Text="Save" CssClass="fontcomman"
                    Width="60px" OnClick="btnfpspread1save_Click1" />
                <asp:Button ID="btnfpspread1delete" runat="server" Text="Delete" CssClass="fontcomman"
                    Width="90px" OnClick="btnfpspread1delete_Click1" />
                <asp:Label ID="lblrptname" runat="server" Width="120px" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display1()" Height="20px"
                    Width="150px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                <asp:Button ID="btnPrint" runat="server" Text="Print PDF" OnClick="btnPrint_Click"
                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Style="width: auto;
                    height: auto;" CssClass="textbox textbox1" />
                <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                <FarPoint:FpSpread ID="fpmarkimport" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                    CssClass="stylefp" ShowHeaderSelection="false">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </center>
            <style>
                .fontcomman
                {
                    font-family: Book Antiqua;
                    font-size: medium;
                    font-weight: bold;
                }
                .pos
                {
                    position: relative;
                    margin-top: 20px;
                }
            </style>
            <center>
                <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                    <tr>
                        <td style="width: auto;">
                            <asp:FileUpload runat="server" ID="fpmarkexcel" Visible="true" Font-Names="Book Antiqua"
                                Font-Bold="True" Font-Size="Medium" />
                        </td>
                        <td>
                            <asp:Button ID="btn_import" Text="Import" runat="server" Visible="true" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_importex" />
                        </td>
                    </tr>
                </table>
            </center>
        </asp:Panel>
        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="pnlSettings"
            CollapseControlID="pHeaderSettings" ExpandControlID="pHeaderSettings" AutoCollapse="false"
            AutoExpand="true" TextLabelID="Labelpersonal" CollapsedSize="0" ImageControlID="Image2"
            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
        </asp:CollapsiblePanelExtender>
    </div>
    <center>
        <div id="divConfirm" runat="server" visible="false" style="height: 300em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divConfirmation" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblConfirnMsg" runat="server" Text="Do You Want To Save marks?" Style="color: Red;"
                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    <asp:Label ID="lblSaveorDelete" Visible="false" runat="server" Text="1" Style="color: Red;"
                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnConfirnYes" CssClass=" textbox btn1 textbox1" Style="height: auto;
                                            width: auto;" OnClick="btnConfirnYes_Click" Text="Yes" runat="server" />
                                        <asp:Button ID="btnConfirnNo" CssClass=" textbox btn1 textbox1" Style="height: auto;
                                            width: auto;" OnClick="btnConfirnNo_Click" Text="No" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 400em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" CssClass="textbox textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btnPopAlertClose_Click" Text="Ok" runat="server" />
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
