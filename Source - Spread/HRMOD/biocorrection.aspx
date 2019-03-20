<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="biocorrection.aspx.cs" Inherits="biocorrection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html>
    <body oncontextmenu="return false">
    </body>
    <br />
    <asp:Panel ID="Panel5" runat="server" BackImageUrl="~/bioimage/Biomatric_New.jpg"
        Height="137px" Width="1006px">
        <br />
    </asp:Panel>
    <table style="width: 649px; margin-top: 10px;">
        <tr>
            <td>
                <asp:Label ID="lblcollege" runat="server" Text="Collge Name" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Width="100px"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="cblcollege" runat="server" AutoPostBack="True" Font-Size="Medium"
                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblcollege_SelectedIndexChanged"
                    Width="130px">
                </asp:DropDownList>
            </td>
            <td class="style457">
                <asp:Image ID="Image19" runat="server" Height="20px" ImageUrl="~/bioimage/Date.jpg"
                    Width="91px" />
            </td>
            <td class="style315">
                <asp:Label ID="Label10" runat="server" Font-Bold="True" Text="From:" Style="font-family: 'Book Antiqua'"
                    Font-Names="Calibri" Font-Size="Medium"></asp:Label>
            </td>
            <td class="style461" colspan="3">
                <asp:TextBox ID="Txtentryfrom" runat="server" Style="margin-bottom: 0px" Height="16px"
                    Width="75px" Font-Bold="True" Font-Names="Book Antiqua"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="Txtentryfrom"
                    FilterType="Custom, Numbers" ValidChars="/" />
                <asp:CalendarExtender ID="Txtentryfrom_CalendarExtender" runat="server" TargetControlID="Txtentryfrom"
                    Format="dd/MM/yyyy">
                </asp:CalendarExtender>
                <asp:RequiredFieldValidator ID="regdate1" runat="server" ControlToValidate="Txtentryfrom"
                    ErrorMessage="Please enter the Date" ForeColor="#FF3300" Style="top: 157px; left: 344px;
                    position: absolute; height: 26px; width: 131px"></asp:RequiredFieldValidator>
            </td>
            <td class="style458">
                <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="To:" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </td>
            <td class="style158" colspan="3">
                <asp:TextBox ID="Txtentryto" runat="server" OnTextChanged="Txtentryto_TextChanged"
                    Height="17px" Width="75px" Font-Bold="True" Font-Names="Book Antiqua"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txtentryto"
                    FilterType="Custom, Numbers" ValidChars="/" />
                <asp:CalendarExtender ID="Txtentryto_CalendarExtender" runat="server" TargetControlID="Txtentryto"
                    Format="dd/MM/yyyy">
                </asp:CalendarExtender>
                <asp:RequiredFieldValidator ID="reqdateto" runat="server" ControlToValidate="Txtentryto"
                    ErrorMessage="Please enter the  to Date" ForeColor="Red" Style="top: 196px; left: 161px;
                    position: absolute; height: 16px; width: 161px"></asp:RequiredFieldValidator>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
            </td>
            <td class="style449">
                <asp:Label ID="lbldate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    ForeColor="Red" Visible="False"></asp:Label>
            </td>
            <td>
                <asp:Image ID="Image3" runat="server" ImageUrl="~/bioimage/Department.jpg" Height="20px"
                    Width="91px" />
            </td>
            <td>
                <div id="castediv" runat="server" class="linkbtn">
                    <asp:TextBox ID="tbseattype" runat="server" Height="16px" ReadOnly="true" Width="135px"
                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">---Select---</asp:TextBox>
                    <br />
                </div>
                <asp:Panel ID="pseattype" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="2px" Height="300px" ScrollBars="Vertical" Width="350px">
                    <asp:CheckBox ID="chkselect" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkselect_CheckedChanged"
                        Text="Select All" />
                    <asp:CheckBoxList ID="cbldepttype" runat="server" Font-Size="Medium" AutoPostBack="True"
                        Width="337px" OnSelectedIndexChanged="cbldepttype_SelectedIndexChanged" Height="262px"
                        Font-Bold="True" Font-Names="Book Antiqua">
                    </asp:CheckBoxList>
                </asp:Panel>
                <asp:DropDownExtender ID="ddeseattype" runat="server" DropDownControlID="pseattype"
                    DynamicServicePath="" Enabled="true" TargetControlID="tbseattype">
                </asp:DropDownExtender>
            </td>
            <td>
                <asp:Image ID="Image5" runat="server" ImageUrl="~/bioimage/Category.jpg" Height="20px"
                    Width="81px" Style="margin-bottom: 0px" />
            </td>
            <td>
                <asp:TextBox ID="tbblood" runat="server" Height="20px" OnTextChanged="tbblood_TextChanged"
                    ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua'; margin-left: 0px;
                    margin-bottom: 0px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                <br />
                <asp:PlaceHolder ID="PlaceHolderblood" runat="server"></asp:PlaceHolder>
                <asp:Panel ID="pblood" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="2px" Height="150px" ScrollBars="Auto" Width="190px" Style="font-family: 'Book Antiqua'">
                    <asp:CheckBox ID="chkcategory" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkcategory_CheckedChanged"
                        Text="Select All " />
                    <asp:CheckBoxList ID="cblcategory" runat="server" Font-Size="Medium" AutoPostBack="True"
                        Width="158px" OnSelectedIndexChanged="cblcategory_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                    </asp:CheckBoxList>
                </asp:Panel>
                <asp:DropDownExtender ID="ddeblood" runat="server" DropDownControlID="pblood" DynamicServicePath=""
                    Enabled="true" TargetControlID="tbblood">
                </asp:DropDownExtender>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Image ID="Image6" runat="server" ImageUrl="~/bioimage/Staff Name.jpg" Height="20px"
                    Width="91px" Style="margin-left: 0px" />
            </td>
            <td>
                <asp:DropDownList ID="cbostaffname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Width="135px" Font-Size="Medium" Height="20px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RadioButton ID="rdoall" runat="server" Checked="True" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" GroupName="s" Text="Register&amp;Unregister" />
            </td>
            <td>
                <asp:RadioButton ID="rdounreg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" GroupName="s" Text="UnregisterStaff" />
            </td>
            <td>
                <asp:RadioButton ID="rdoreg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" GroupName="s" Text="RegisterStaff" />
            </td>
            <td>
                <asp:Button ID="btn_go" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btn_go_Click" Text="Search" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:Panel ID="Panel8" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
        Width="1000px">
    </asp:Panel>
    <asp:Label ID="lblnorec" runat="server" Text="There are no Records Found" ForeColor="Red"
        Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:Label ID="lblselect" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="Red" Text="Please Select The staff" Visible="False"></asp:Label>
            </td>
            <td>
                <asp:Button ID="btngo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btngo_Click" Text="GO" Visible="False" />
            </td>
        </tr>
    </table>
    <br />
    <center>
        <asp:UpdatePanel ID="up_fpbiomatric" runat="server">
            <ContentTemplate>
                <FarPoint:FpSpread ID="fpbiomatric" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Height="600px" Width="500" OnUpdateCommand="fpbiomatric_UpdateCommand"
                    ShowHeaderSelection="false">
                    <CommandBar BackColor="Control" ShowPDFButton="true" ButtonType="PushButton" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:Button ID="btnsave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnsave_Click" Text="Save" Enabled="False" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
    </center>
    </div>
    </html>
</asp:Content>
