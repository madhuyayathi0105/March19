<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true"
    CodeFile="DepartmentwiseArrearReport.aspx.cs" Inherits="DepartmentwiseArrearReport"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cpHeader
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
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 0px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
        
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
        .rblpassfail
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            position: relative;
            margin: 0px;
            padding: 0px;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function Validation() {

            var selectedvalue = $('#<%= rblPassorFailSublist.ClientID %> input:checked').val()
            if (document.getElementById("<%=txt_batch.ClientID%>").value == "---Select---") {
                alert("Please Select Batch");
                return false;
            }
            else if (document.getElementById("<%=txt_degree.ClientID%>").value == "---Select---") {
                alert("Please Select Degree");
                return false;
            }
            else if (document.getElementById("<%=txt_branch.ClientID%>").value == "---Select---") {
                alert("Please Select Department");
                return false;
            }
            else {
                if (selectedvalue == "1") {
                    if (document.getElementById("<%=txtarrearrange.ClientID%>").value == "") {
                        alert("Please Enter Arrear Range");
                        return false;
                    }
                }
            }
            return true;
        }

        function display() {
            document.getElementById('MainContent_lblreportmsg').innerHTML = "";
        }

        //Function to allow only numbers to textbox
        function validate(key) {
            //getting key code of pressed key
            var keycode = (key.which) ? key.which : key.keyCode;
            var phn = document.getElementById('txtarrearrange');
            //comparing pressed keycodes
            if (!(keycode == 8 || keycode == 46) && (keycode < 48 || keycode > 57)) {
                return false;
            }
            else {
                //Condition to check textbox contains ten numbers or not
                if (phn.value.length < 10) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }                        
    </script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.js">       
    </script>
    <script type="text/javascript">
        $("[id*=rblPassorFailSublist] input").live("click", function () {
            var selectedValue = $(this).val();
            var selectedText = $(this).next().html();
            if (selectedValue == "0") {
                $('#<%=lblarrearrange.ClientID %>').css('display', 'none');
                $('#<%=txtarrearrange.ClientID %>').css('display', 'none');
                $('#<%=ViewSpread.ClientID %>').css('display', 'none')
            }
            else if (selectedValue == "1") {
                $('#<%=lblarrearrange.ClientID %>').css('display', 'block');
                $('#<%=txtarrearrange.ClientID %>').css('display', 'block');
                $('#<%=ViewSpread.ClientID %>').css('display', 'none')
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <asp:Label ID="Label1" runat="server" Text="Arrear Count Wise Student List" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
        </center>
        <br />
        <center>
            <table style="width: 700px; height: 70px; background-color: #0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="lblbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Batch" Style="position: relative;"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_batch" runat="server" Font-Bold="True" ReadOnly="true" CssClass="Dropdown_Txt_Box"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--" Style="position: relative;
                                        height: 20px; width: 100px;">---Select---</asp:TextBox>
                                    <asp:Panel ID="pbat" runat="server" CssClass="MultipleSelectionDDL" Height="200"
                                        Width="125" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                        ScrollBars="Vertical">
                                        <asp:CheckBox ID="chk_batch" runat="server" Text="SelectAll" AutoPostBack="true"
                                            OnCheckedChanged="chk_batch_ChekedChanged" Font-Bold="True" ForeColor="Black"
                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                        <asp:CheckBoxList ID="chklst_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklst_batch_SelectedIndexChanged"
                                            Font-Bold="True" Font-Size="Medium" ForeColor="Black" Font-Names="Book Antiqua"
                                            Width="62px" Height="37px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_batch"
                                        PopupControlID="pbat" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="chklst_batch" />
                                </Triggers>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="chk_batch" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lbldeg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Degree" Style="font-family: Book Antiqua; font-size: medium;
                            font-weight: bold; position: relative;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_degree" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="position: relative;
                                    height: 20px; width: 100px;">---Select---</asp:TextBox>
                                <asp:Panel ID="Pdeg" runat="server" CssClass="MultipleSelectionDDL" Height="200"
                                    Width="125" BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                    ScrollBars="Vertical">
                                    <asp:CheckBox ID="chk_degree" runat="server" Text="SelectAll" AutoPostBack="true"
                                        OnCheckedChanged="chk_degree_ChekedChanged" Font-Bold="True" ForeColor="Black"
                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                    <asp:CheckBoxList ID="chklst_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklst_degree_SelectedIndexChanged"
                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Width="98px">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_degree"
                                    PopupControlID="Pdeg" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="chklst_degree" />
                            </Triggers>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="chk_degree" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Text="Department" Width="90px" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="display: inline-block;
                            color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                            width: 90px; position: relative;"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_branch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                    ReadOnly="true" Width="105px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Style="font-size: medium; font-weight: bold; height: 20px; font-family: 'Book Antiqua';
                                    position: relative;">---Select---</asp:TextBox>
                                <asp:Panel ID="pbranch" runat="server" Width="230px" CssClass="MultipleSelectionDDL"
                                    BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                    Height="150px">
                                    <asp:CheckBox ID="chk_branch" runat="server" Font-Bold="True" Width="150px" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chk_branch_ChekedChanged" />
                                    <asp:CheckBoxList ID="chklst_branch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklst_branch_SelectedIndexChanged" Width="150px" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_branch"
                                    PopupControlID="pbranch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="chklst_branch" />
                            </Triggers>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="chk_branch" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblsem" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                            Text="Sem" Style="position: relative;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsem" runat="server" Font-Size="Medium" AutoPostBack="true"
                            Width="55px" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged" Style="position: relative;">
                            <asp:ListItem Text="All"></asp:ListItem>
                            <asp:ListItem Text="1"></asp:ListItem>
                            <asp:ListItem Text="2"></asp:ListItem>
                            <asp:ListItem Text="3"></asp:ListItem>
                            <asp:ListItem Text="4"></asp:ListItem>
                            <asp:ListItem Text="5"></asp:ListItem>
                            <asp:ListItem Text="6"></asp:ListItem>
                            <asp:ListItem Text="7"></asp:ListItem>
                            <asp:ListItem Text="8"></asp:ListItem>
                            <asp:ListItem Text="9"></asp:ListItem>
                            <asp:ListItem Text="10"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rblPassorFailSublist" runat="server" CssClass="rblpassfail" AutoPostBack="true"
                            RepeatDirection="Horizontal" OnSelectedIndexChanged="rblPassorFailSublist_OnSelectedIndexChanged">
                            <asp:ListItem Selected="True" Text="Pass Subjects" Value="0"></asp:ListItem>
                            <asp:ListItem Selected="False" Text="Fail Subjects" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="margin: 0px; padding: 0px;">
                        <asp:Label ID="lblarrearrange" runat="server" Font-Bold="true" Font-Size="Medium"
                            Text="Arrear Range" Style="position: relative;"></asp:Label>
                    </td>
                    <td style="margin: 0px; padding: 0px;">
                        <asp:TextBox ID="txtarrearrange" runat="server" Height="20px" onkeypress="return validate(event)"
                            MaxLength="3" Width="40px" Style="font-family: 'Book Antiqua'; position: relative;"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Go" Width="41px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="color: Black; position: relative;" 
                            OnClick="btnGo_Click"></asp:Button>

                           <%-- OnClientClick="return Validation()"--%>
                    </td>
                </tr>
            </table>
        </center>
        <center>
            <div>
                <br />
                <div>
                    <asp:Label ID="lblerrormsg" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                </div>
                <br />
                <div id="ViewSpread" runat="server" style="display: none;">
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Width="900px" HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="AsNeeded"
                        ShowHeaderSelection="false">
                        <CommandBar BackColor="White" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark" Visible="false">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" AllowSort="false" GridLineColor="Black" BackColor="White"
                                SelectionBackColor="#CE5D5A" SelectionForeColor="White">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <center>
                        <asp:Label ID="lblreportmsg" runat="server" Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <br />
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnexcel" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnexcel_Click" />
                        <asp:Button ID="btnprint" runat="server" Text="Print" Visible="false" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" OnClick="btnprint_Click" />
                        <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
                    </center>
                </div>
            </div>
        </center>
    </div>
</asp:Content>
