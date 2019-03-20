<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="UserCreation.aspx.cs"
    Inherits="UserCreation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <title>User Creation</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <asp:Panel ID="Panel1" runat="server">
            <center>
                <asp:Label ID="lblpage" runat="server" Text="User Creation" Font-Size="20px" Font-Names="Book Antiqua"
                    Font-Bold="True" ForeColor="Green"></asp:Label>
            </center>
        </asp:Panel>
        <br />
        <fieldset style="height: auto; width: auto; border-color: Black; border-width: 4px;">
            <table style="text-align: left">
                <tr>
                    <td>
                        <asp:Label ID="lblcollege" runat="server" Text="Institution" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Width="400px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblusername" runat="server" Text="User Name" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtusername" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Width="400px" MaxLength="50"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtusername"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnuse" runat="server" Text="?" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" OnClick="btnuse_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblfullname" runat="server" Text="Name" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfullname" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Width="400px" MaxLength="100"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtfullname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbldescription" runat="server" Text="Descrption" Font-Size="Medium"
                            Font-Names="Book Antiqua" Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtdescription" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Width="400px" MaxLength="100"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtdescription"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblpassword" runat="server" Text="Password" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtpassword" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Width="400px" TextMode="Password" MaxLength="50"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtpassword"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblphone" runat="server" Text="Phone No" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Width="112px"></asp:Label>
                        <asp:TextBox ID="txtphone" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Width="100px" MaxLength="10"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtphone"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblconpassword" runat="server" Text="Conform Password" Font-Size="Medium"
                            Font-Names="Book Antiqua" Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtconpassword" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Width="400px" ControlToValidate="txtconpassword" TextMode="Password"
                            MaxLength="50"></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkfin" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Text="Is Finance User" />
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Password Not Matching"
                            ControlToCompare="txtpassword" ControlToValidate="txtconpassword" ForeColor="Red"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblcounter" runat="server" Text="Counter No" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtcounter" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Width="400px" MaxLength="100"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtcounter"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblcounname" runat="server" Text="Counter Name" Font-Size="Medium"
                            Font-Names="Book Antiqua" Font-Bold="True" Width="112px"></asp:Label>
                        <asp:TextBox ID="txtcouname" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Width="100px" MaxLength="100"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtcouname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkstaff" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Text="Is Staff" AutoPostBack="true" OnCheckedChanged="chkstaff_CheckedChnage" />
                    </td>
                    <td>
                        <asp:Label ID="lblstaffname" runat="server" Text="Staff Name" Font-Size="Medium"
                            Font-Names="Book Antiqua" Font-Bold="True" Width="112px"></asp:Label><asp:TextBox
                                ID="txtstaffname" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                Font-Bold="True" Width="100px" Enabled="false"></asp:TextBox>
                        <asp:Button ID="btnaddstaff" runat="server" Text="?" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" OnClick="btnaddstaff_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkinvenuser" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Text="Inventory User" />
                    </td>
                    <td>
                        <asp:Button ID="btninveadd" runat="server" Text="?" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkOtpConfirm" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Text="OTP Confirmation" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkgroupuser" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="True" Text="Group User" />
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblgroup" runat="server" Text="Group Id" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Font-Bold="True" Width="80px"></asp:Label>
                                <asp:TextBox ID="txtgroup" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="130px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pgroup" runat="server" Style="border: solid 1px #000000; overflow-y: scroll;
                                    background-color: #f0f8ff; font-size: 11px; font-family: Calibri, Arial, Helvetica;
                                    line-height: normal;">
                                    <asp:CheckBox ID="chkgroup" runat="server" Font-Bold="True" OnCheckedChanged="chkgroup_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsgroup" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsgroup_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtgroup"
                                    PopupControlID="pgroup" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btngroupcreate" runat="server" Text="Group Create / Update " Font-Size="Medium"
                            Font-Names="Book Antiqua" Font-Bold="True" OnClick="btngroupcreate_Click" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="errmsg" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                Font-Bold="True" ForeColor="Red"></asp:Label>
            <br />
            <asp:Button ID="btnclear" runat="server" Text="Clear" Font-Size="Medium" Font-Names="Book Antiqua"
                Font-Bold="True" OnClick="btnclear_Click" />
            <asp:Button ID="btnsave" runat="server" Text="Save" Font-Size="Medium" Font-Names="Book Antiqua"
                Font-Bold="True" OnClick="btnsave_Click" />
            <asp:Button ID="btnrestpass" runat="server" Text="Reset Password" Font-Size="Medium"
                Font-Names="Book Antiqua" Font-Bold="True" OnClick="btnrestpass_Click" />
            <asp:Button ID="btndelete" runat="server" Text="Delete" Font-Size="Medium" Font-Names="Book Antiqua"
                Font-Bold="True" OnClick="btndelete_Click" />
        </fieldset>
        <asp:Panel ID="Puser" runat="server" Style="height: 100em; z-index: 1000; width: 100%;
            background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
            <center>
                <br />
                <br />
                <br />
                <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: Book Antiqua;
                    font-size: medium; font-weight: bold; width: 540px; background-color: #F0F8FF;
                    border: 1px solid black;">
                    <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                        left: 200px">
                        Select User
                    </caption>
                    <br />
                    <table style="text-align: left">
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkincludegroup" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Font-Bold="True" Text="Include Group User" AutoPostBack="true" OnCheckedChanged="chkincludegroup_Checked" />
                            </td>
                            <td>
                                <asp:Label ID="lblusersearch" runat="server" Text="Group Id" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtusersearch" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Font-Bold="True" Width="150px" AutoPostBack="true" OnTextChanged="chkincludegroup_Checked"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtusersearch"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <FarPoint:FpSpread ID="Fpuser" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="300" Width="550" HorizontalScrollBarPolicy="Never"
                        VerticalScrollBarPolicy="AsNeeded">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <asp:Label ID="lblperrmsg" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                        Font-Bold="True" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Button ID="btnuserok" runat="server" Text="Ok" Font-Size="Medium" Font-Names="Book Antiqua"
                        Font-Bold="True" OnClick="btnuserok_Click" />
                    <asp:Button ID="btnuseexit" runat="server" Text="Exit" Font-Size="Medium" Font-Names="Book Antiqua"
                        Font-Bold="True" OnClick="btnuseexit_Click" />
                </div>
            </center>
        </asp:Panel>
        <asp:Panel ID="PnStaff" runat="server" Style="height: 100em; z-index: 1000; width: 100%;
            background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
            <center>
                <br />
                <br />
                <br />
                <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: Book Antiqua;
                    font-size: medium; font-weight: bold; width: 700px; background-color: #F0F8FF;
                    border: 1px solid black;">
                    <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                        left: 200px">
                        Select Staff Incharge
                    </caption>
                    <br />
                    <br />
                    <table style="text-align: left">
                        <tr>
                            <td>
                                <asp:Label ID="lblDepartment" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldepratstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblsearchby" runat="server" Text="Staff By" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true">
                                    <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                    <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_search"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <FarPoint:FpSpread ID="fsstaff" runat="server" ActiveSheetViewIndex="0" Height="300"
                        Width="800" VerticalScrollBarPolicy="AsNeeded" BorderWidth="0.5" Visible="False">
                        <CommandBar BackColor="Control" ButtonType="PushButton">
                            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <asp:Label ID="errstaff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lblstaffcodehide" runat="server" Visible="false"></asp:Label>
                    <br />
                    <asp:Button runat="server" ID="btnstaffadd" Text="Ok" OnClick="btnstaffadd_Click"
                        Width="75px" Font-Bold="True" />
                    <asp:Button runat="server" ID="btnexitpop" Text="Exit" OnClick="exitpop_Click" Width="75px"
                        Font-Bold="True" />
            </center>
            </div>
        </asp:Panel>
        <asp:Panel ID="PnGroup" runat="server" Style="height: 100em; z-index: 1000; width: 100%;
            background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
            <center>
                <br />
                <br />
                <br />
                <div class="PopupHeaderrstud2" id="Div2" style="text-align: center; font-family: Book Antiqua;
                    font-size: medium; font-weight: bold; width: 540px; background-color: #F0F8FF;
                    border: 1px solid black;">
                    <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                        left: 200px">
                        Select User
                    </caption>
                    <br />
                    <table style="text-align: left">
                        <tr>
                            <td>
                                <asp:Label ID="lblgroupload" runat="server" Text="College" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlgroupna" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Font-Bold="True" Width="400px" AutoPostBack="true" OnSelectedIndexChanged="ddlgroupna_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblgroupname" runat="server" Text="Group Name" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtgroupname" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Font-Bold="True" Width="400px" MaxLength="50"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtgroupname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblgroupdesc" runat="server" Text="Description" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtgroupdesc" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Font-Bold="True" Width="400px" MaxLength="100"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtgroupdesc"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lblgrouperr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:CheckBox ID="chkgropedit" runat="server" Text="Edit" Font-Size="Medium" Font-Names="Book Antiqua"
                        Font-Bold="True" />
                    <asp:Button runat="server" ID="btngroupsave" Text="Save" OnClick="btngroupsave_Click"
                        Width="75px" Font-Bold="True" />
                    <asp:Button runat="server" ID="btngroupdelete" Text="Delete" OnClick="btngroupdelete_Click"
                        Width="75px" Font-Bold="True" />
                    <asp:Button runat="server" ID="btngroupexit" Text="Exit" OnClick="btngroupexit_Click"
                        Width="75px" Font-Bold="True" />
                </div>
            </center>
        </asp:Panel>
    </center>
</asp:Content>
