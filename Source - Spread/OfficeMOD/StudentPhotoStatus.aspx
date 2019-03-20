<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentPhotoStatus.aspx.cs" Inherits="StudentPhotoStatus" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_errmsg').innerHTML = "";

        }
    </script>
    <style type="text/css">
        autocomplete_highlightedListItem
        {
            background-color: #ffff99;
            color: black;
            padding: 1px;
            width: 241px;
        }
        .autocomplete_completionListElement
        {
            margin: 0px !important;
            background-color: White;
            color: windowtext;
            border: buttonshadow;
            border-width: 0px;
            border-style: solid;
            cursor: 'default';
            height: 100px;
            font-family: Book Antiqua;
            font-size: small;
            text-align: left;
            list-style-type: none;
            padding-left: 1px;
            width: 430px;
            overflow: auto;
            overflow-x: hidden;
        }
    </style>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <asp:Label ID="Label5" runat="server" CssClass="fontstyleheader" ForeColor="Green"
                Text="Student's Photo Report"></asp:Label></center>
        <br />
        <center>
            <table style="width: 900px; height: 70px; background-color: #0CA6CA;" class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblbach" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbatch" runat="server" Height="18px" ReadOnly="true" Width="100px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                                        <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="100px" Height="200px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                        PopupControlID="pbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtdegree" runat="server" Height="18px" ReadOnly="true" Width="100px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pdegree" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="125px" Height="200px"
                                        ScrollBars="Vertical">
                                        <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="100px" Height="200px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdegree"
                                        PopupControlID="pdegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbranch" runat="server" Height="18px" ReadOnly="true" Width="100px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pbranch" runat="server" CssClass="MultipleSelectionDDL" Height="300px"
                                        BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical">
                                        <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                        <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            Height="58px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                        PopupControlID="pbranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtsec" runat="server" Height="18px" ReadOnly="true" Width="100px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="psec" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="125px">
                                        <asp:CheckBox ID="chksec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chksec_CheckedChanged" />
                                        <asp:CheckBoxList ID="chklssec" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            Height="58px" OnSelectedIndexChanged="chklstsec_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsec"
                                        PopupControlID="psec" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblcategory" runat="server" Text="Category" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtcategory" runat="server" Height="18px" ReadOnly="true" Width="100px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pcategory" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="110px">
                                        <asp:CheckBox ID="chkcategory" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Both" AutoPostBack="True" OnCheckedChanged="chkcategory_CheckedChanged" />
                                        <asp:CheckBoxList ID="chklscategory" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            Height="58px" OnSelectedIndexChanged="chklscategory_SelectedIndexChanged">
                                            <asp:ListItem Value="1">Student</asp:ListItem>
                                            <asp:ListItem Value="2">Father</asp:ListItem>
                                            <asp:ListItem Value="3">Mother</asp:ListItem>
                                            <asp:ListItem Value="4">Guardian</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtcategory"
                                        PopupControlID="pcategory" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblphoto" runat="server" Text="Photo" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtphoto" runat="server" Height="18px" ReadOnly="true" Width="80px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pphoto" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="120px">
                                        <asp:CheckBox ID="chkphoto" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Both" AutoPostBack="True" OnCheckedChanged="chkphoto_CheckedChanged" />
                                        <asp:CheckBoxList ID="chklsphoto" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            Height="58px" OnSelectedIndexChanged="chklsphoto_SelectedIndexChanged">
                                            <asp:ListItem Value="1">Available</asp:ListItem>
                                            <asp:ListItem Value="2">Not Available</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtphoto"
                                        PopupControlID="pphoto" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblfiltertype" runat="server" Text="Filter" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlfilter" AutoPostBack="true" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlselectindechanged" Width="100px"
                            Font-Size="Medium">
                            <asp:ListItem Text="Caste"> </asp:ListItem>
                            <asp:ListItem Text="Blood Group"> </asp:ListItem>
                            <asp:ListItem Text="Seat Type"> </asp:ListItem>
                            <asp:ListItem Text="Community"> </asp:ListItem>
                            <asp:ListItem Text="Permanent District"> </asp:ListItem>
                            <asp:ListItem Text="Permanent State"> </asp:ListItem>
                            <asp:ListItem Text="Permanent Country"> </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddltotal" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Width="150px" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btngo" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            runat="server" Text="Go" Width="50px" Height="28px" OnClick="btngo_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <FarPoint:FpSpread ID="Fpstudentphoto" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Height="400" Width="980" HorizontalScrollBarPolicy="Never"
                VerticalScrollBarPolicy="Never" OnButtonCommand="ButtonClickHandler">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br />
            <asp:Label ID="lblnorec" runat="server" Text="No Records Found" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
            <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
            <br />
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnxl_Click" />
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
            <br />
            <center>
                <asp:Panel ID="panelphoto" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="false" BorderWidth="2px" Style="left: 150px; top: 220px; position: absolute;"
                    Height="454px" Width="700px">
                    <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            <asp:Label ID="lblcaption" runat="server" Text="Photos Details" Font-Bold="True"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </caption>
                    </div>
                    <br />
                    <br />
                    <fieldset style="left: 25px; top: 30px; width: 280px; height: 130px; position: absolute;">
                        <asp:Label ID="Label1" runat="server" Text="Student Photo" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 50px;
                            top: 2px;"></asp:Label>
                        <asp:Image ID="imgstudp" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="fulstudp" runat="server" Style="position: absolute; left: 5px;
                            top: 130px;" />
                        <asp:Button ID="Btndownload" runat="server" Text="Download" Width="80px" Font-Bold="true"
                            OnClick="Btndownload_Click" Style="position: absolute; left: 225px; top: 100px;" />
                        <asp:Button ID="btnstuph" runat="server" Text="Ok" Width="75px" Font-Bold="true"
                            OnClick="btnstuph_Click" Style="position: absolute; left: 230px; top: 130px;" />
                    </fieldset>
                    <fieldset style="left: 350px; top: 30px; width: 280px; height: 130px; position: absolute;">
                        <asp:Label ID="Label2" runat="server" Text="Father Photo" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 50px;
                            top: 2px;"></asp:Label>
                        <asp:Image ID="imgfatp" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="fulfatp" runat="server" onchange="callme(this)" Style="position: absolute;
                            left: 5px; top: 130px;" />
                        <asp:Button ID="Btndownload1" runat="server" Text="Download" Width="80px" Font-Bold="true"
                            OnClick="Btndownload1_Click" Style="position: absolute; left: 225px; top: 100px;" />
                        <asp:Button ID="btnfaph" runat="server" Text="Ok" Width="75px" Font-Bold="true" OnClick="btnfaph_Click"
                            Style="position: absolute; left: 230px; top: 130px;" />
                    </fieldset>
                    <fieldset style="width: 280px; height: 130px; position: absolute; left: 25px; top: 200px;">
                        <asp:Label ID="Label3" runat="server" Text="Mother Photo" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 50px;
                            top: 2px;"></asp:Label>
                        <asp:Image ID="imgmotp" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="fulmp" runat="server" onchange="callme(this)" Style="position: absolute;
                            left: 5px; top: 130px;" />
                        <asp:Button ID="btndownload2" runat="server" Text="Download" Width="80px" Font-Bold="true"
                            OnClick="btndownload2_Click" Style="position: absolute; left: 225px; top: 100px;" />
                        <asp:Button ID="btnmotph" runat="server" Text="Ok" Width="75px" Font-Bold="true"
                            OnClick="btnmotph_Click" Style="position: absolute; left: 230px; top: 130px;" />
                    </fieldset>
                    <fieldset style="width: 280px; height: 130px; position: absolute; left: 350px; top: 200px;">
                        <asp:Label ID="Label4" runat="server" Text="Guardian Photo" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 50px;
                            top: 2px;"></asp:Label>
                        <asp:Image ID="imggurp" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="fulguar" runat="server" onchange="callme(this)" Style="position: absolute;
                            left: 5px; top: 130px;" />
                        <asp:Button ID="Btndownload3" runat="server" Text="Download" Width="80px" Font-Bold="true"
                            OnClick="Btndownload3_Click" Style="position: absolute; left: 225px; top: 100px;" />
                        <asp:Button ID="btngurph" runat="server" Text="Ok" Width="75px" Font-Bold="true"
                            OnClick="btngurph_Click" Style="position: absolute; left: 230px; top: 130px;" />
                    </fieldset>
                    <asp:Label ID="lblphotoerr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Style="position: absolute; left: 5px; top: 380px;"></asp:Label>
                    <fieldset style="width: 150px; height: 12px; position: absolute; left: 500px; top: 400px;">
                        <asp:Button ID="btnsave" runat="server" Text="Save" Width="75px" Font-Bold="true"
                            OnClick="btnsave_Click" Style="position: absolute; left: 5px; top: 7px;" />
                        <asp:Button ID="btnexit" runat="server" Text="Exit" Width="75px" Font-Bold="true"
                            OnClick="btnexit_Click" Style="position: absolute; left: 100px; top: 7px;" />
                    </fieldset>
                </asp:Panel>
            </center>
        </center>
    </body>
</asp:Content>
