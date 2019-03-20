<%@ Page Title="About Us" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About" EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <style type="text/css">
            .spstyle
            {
                height: 100;
                width: 1000;
                cursor: pointer;
            }
            </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <p>
        <html>
        <title></title>
        <style type="text/css">
            .spstyle
            {
                height: 100;
                width: 1000;
                cursor: hand;
            }
            .flright
            {
                float: right;
            }
            .style1
            {
                border-width: 0;
                background-color: transparent;
                width: 250;
            }
            .styles
            {
                font-size: small;
            }
            .linkbtn
            {
                text-align: right;
                width: 156px;
            }
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
            }
            
            .cpimage
            {
                float: right;
                vertical-align: middle;
                background-color: transparent;
            }
            .style4
            {
                width: 198px;
            }
            .style5
            {
                width: 164px;
            }
            .style10
            {
                width: 108px;
            }
            #style11
            {
                width: 300px;
            }
            #style12
            {
                width: 100px;
            }
            .style16
            {
                width: 158px;
                height: 13px;
            }
            .style17
            {
                width: 177px;
                height: 20px;
            }
            .style21
            {
                width: 185px;
            }
            .style275
            {
                border-width: 0;
                background-color: transparent;
                width: 250;
                height: 64px;
            }
            .style276
            {
                border-width: 0;
                background-color: transparent;
                width: 225px;
                height: 64px;
            }
            .style277
            {
                width: 108px;
                height: 64px;
            }
            .style278
            {
                width: 185px;
                height: 32px;
            }
            .style279
            {
                border-width: 0;
                background-color: transparent;
                width: 210px;
                height: 32px;
            }
            .style280
            {
                height: 32px;
            }
            .style281
            {
                border-width: 0;
                background-color: transparent;
                width: 250;
                height: 32px;
            }
            #btn
            {
                background-color: transparent;
            }
            .style288
            {
                width: 209px;
            }
            .style290
            {
                height: 20px;
            }
            .style291
            {
                border-width: 0;
                background-color: transparent;
                width: 198px;
            }
            .style295
            {
                height: 26px;
            }
            .style296
            {
                width: 209px;
                height: 26px;
            }
            .style298
            {
                height: 8px;
            }
            .style299
            {
                width: 209px;
                height: 8px;
            }
            .style302
            {
                height: 13px;
            }
            .style303
            {
                width: 210px;
                height: 8px;
            }
            .style304
            {
                width: 210px;
                height: 26px;
            }
            .style305
            {
                width: 310px;
            }
            .style329
            {
                height: 13px;
                width: 134px;
            }
            .style332
            {
                border-width: 0;
                background-color: transparent;
                width: 210px;
            }
            .HeaderCSS
            {
                color: Black;
                background-color: #33bdef;
                border: 1px solid #0688fa;
                font-size: medium; /* border:solid 1px salmon; */
                font-family: Book Antiqua;
                text-align: left;
                height: 25px;
                cursor: pointer;
            }
            .HeaderSelectedCSS
            {
                color: white;
                background-color: #4470bd;
                border: 1px solid #4e6096;
                font-family: Times New Roman;
                font-weight: bold;
                font-size: medium; /* font-style:italic;  */
                text-align: left;
                height: 25px;
                cursor: pointer;
            }
            stylenew
            {
                font-family: Book Antiqua;
                font-size: medium;
                font-weight: bold;
            }
            .style333
            {
                width: 284px;
            }
            .style335
            {
                width: 591px;
            }
            .style336
            {
                width: 256px;
                height: 28px;
            }
        </style>
        <script>
        

        </script>
        <body>
            <script type="text/javascript">
                function pageLoad() {
                    $find('DropDownExtender1')._dropWrapperHoverBehavior_onhover();
                    $find('DropDownExtender1').unhover = VisibleMe;

                }

                function VisibleMe() {
                    $find('DropDownExtender1')._dropWrapperHoverBehavior_onhover();
                }

                function ExcelClick() {
                    var txtval = document.getElementById("<%=txtexcelname.ClientID %>").value;
                    if (txtval == "") {
                        alert("Please Enter Report Name");
                    }
                }
            </script>
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <center>
                    <span class="fontstyleheader" style="color: green;">Universal Report</span></center>
                <center>
                    <div>
                        <center>
                            <table id="myCol" class="maintablestyle" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcollege" runat="server" Text="College Name" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbcollege" runat="server" Height="19px" Width="163px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Label ID="lblcol" Visible="False" runat="server" Text="Label" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Red"></asp:Label>
                        </center>
                        <table>
                            <tr>
                                <td>
                                </td>
                                <td class="style335">
                                    <div id="Div15" runat="server" class="linkbtn">
                                        <asp:LinkButton ID="LinkButtoncol" runat="server" Font-Size="X-Small" Visible="false"
                                            Width="66px" Height="16px" OnClick="LinkButtoncol_Click" Font-Names="Book Antiqua">Remove All</asp:LinkButton>
                                        <br />
                                    </div>
                                    <asp:PlaceHolder ID="PlaceHoldercollege" Visible="false" runat="server"></asp:PlaceHolder>
                                    <asp:Panel ID="Panelcollege" runat="server" Height="93px" ScrollBars="Auto" Width="169px"
                                        BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px">
                                        <asp:CheckBox ID="cbcollege" Text="Select All" runat="server" OnCheckedChanged="cbcollege_CheckedChanged"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small" AutoPostBack="True" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <br />
                                        <asp:CheckBoxList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:DropDownExtender ID="DropDownExtender3" TargetControlID="tbcollege" DropDownControlID="Panelcollege"
                                        runat="server">
                                    </asp:DropDownExtender>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnsearch">
                            <asp:Accordion runat="server" ID="Accordion1" HeaderCssClass="HeaderCSS" HeaderSelectedCssClass="HeaderSelectedCSS"
                                ContentCssClass="cpBody" Width="1100px" BorderColor="White" Style="margin-right: 22px">
                                <Panes>
                                    <asp:AccordionPane runat="server" ID="AccordionPane1">
                                        <Header>
                                            <span style="margin-left: 12px;">Registration Details</span></Header>
                                        <Content>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" Text="Application No" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            Width="118px" Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlappno" runat="server" Height="20px" Width="120px" OnSelectedIndexChanged="ddlappno_SelectedIndexChanged"
                                                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp; &nbsp;
                                                        <asp:TextBox ID="tbappno" Height="16px" Width="120" runat="server" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="Admission No" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddladmno" runat="server" Height="20px" Width="150px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddladmno_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp; &nbsp;
                                                        <asp:TextBox ID="tbadmno" Height="16" Width="115" runat="server" ClientIDMode="Inherit"
                                                            MaxLength="16" Enabled="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblreg" runat="server" Text="Reg No" Font-Size="Medium" Font-Bold="True"
                                                            Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlregno" runat="server" Height="20px" Width="120px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlregno_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp; &nbsp;
                                                        <asp:TextBox ID="tbregno" Height="16" Width="120" MaxLength="16" runat="server" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbregno"
                                                            FilterType="Numbers,LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblrno" runat="server" Text="Roll No" Font-Size="Medium" Font-Bold="True"
                                                            Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlrollno" runat="server" Height="20px" Width="150px" OnSelectedIndexChanged="ddlrollno_SelectedIndexChanged"
                                                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp;&nbsp;
                                                        <asp:TextBox ID="tbrollno" Height="16" Width="120" MaxLength="16" runat="server"
                                                            ClientIDMode="Inherit" Enabled="False" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbrollno"
                                                            FilterType="Numbers,LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblstudname" runat="server" Text="Student Name" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlstudname" runat="server" Height="20px" Width="120px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlstudname_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp; &nbsp;
                                                        <asp:TextBox ID="tbstudname" Height="16px" Width="121px" runat="server" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftbesname" runat="server" TargetControlID="tbstudname"
                                                            FilterType="Custom, LowercaseLetters,UppercaseLetters" ValidChars="." />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label9" runat="server" Text="Admitted Date" Font-Size="Medium" Font-Bold="True"
                                                            Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label51" runat="server" Text="From" Font-Size="Medium" Font-Bold="True"
                                                            Font-Names="Book Antiqua"></asp:Label>
                                                        <asp:TextBox ID="tbfromadmdt" Height="16px" Width="100" runat="server" AutoPostBack="True"
                                                            OnTextChanged="tbfromadmdt_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="tbfromadmdt"
                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','-'" />
                                                        <asp:CalendarExtender ID="CalendarExtender2" Format="d-MM-yyyy" TargetControlID="tbfromadmdt"
                                                            runat="server">
                                                        </asp:CalendarExtender>
                                                        <asp:Label ID="Label50" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                        <asp:TextBox ID="tbtoadmdt" Height="16px" Width="100" runat="server" AutoPostBack="True"
                                                            OnTextChanged="tbtoadmdt_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="tbtoadmdt"
                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','-'" />
                                                        <asp:CalendarExtender ID="CalendarExtender6" Format="d-MM-yyyy" TargetControlID="tbtoadmdt"
                                                            runat="server">
                                                        </asp:CalendarExtender>
                                                        <asp:Label ID="Labeldatead" runat="server" ForeColor="Red" Text="Enter from Date first"
                                                            Visible="False" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label8" Width="124px" runat="server" Text="Applied Date" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label52" runat="server" Text="From" Font-Size="Medium" Font-Bold="True"
                                                            Font-Names="Book Antiqua"></asp:Label>
                                                        <asp:TextBox ID="tbfromappdt" Width="100px" runat="server" Height="16px" AutoPostBack="True"
                                                            OnTextChanged="tbfromappdt_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" TargetControlID="tbfromappdt"
                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','-'" />
                                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="tbfromappdt" Format="d-MM-yyyy"
                                                            runat="server">
                                                        </asp:CalendarExtender>
                                                        <asp:Label ID="Label49" runat="server" Font-Size="Medium" Text="To" Font-Bold="True"
                                                            Font-Names="Book Antiqua"></asp:Label>
                                                        <asp:TextBox ID="tbtoappdt" Width="100px" runat="server" Height="16px" OnTextChanged="tbtoappdt_TextChanged"
                                                            AutoPostBack="True" ForeColor="Black" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" TargetControlID="tbtoappdt"
                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','-'" />
                                                        <asp:CalendarExtender ID="CalendarExtender5" Format="d-MM-yyyy" TargetControlID="tbtoappdt"
                                                            runat="server">
                                                        </asp:CalendarExtender>
                                                        <asp:Label ID="Labeldateap" runat="server" ForeColor="Red" Text="Enter from Date first"
                                                            Visible="False" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblhostel" runat="server" Text="Hostler/Dayscholar" Font-Bold="true"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="stylenew"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlhosday" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label12" runat="server" Text="ReferredBy" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbdirect" runat="server" Text="Direct" GroupName="reference"
                                                            AutoPostBack="True" OnCheckedChanged="rbdirect_CheckedChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbdirect" runat="server" Height="19px" ReadOnly="true" Width="80px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div id="Div19" runat="server" class="linkbtn" style="margin-left: -45px; margin-top: 12px;
                                                            position: absolute;">
                                                            <asp:LinkButton ID="LinkButtondirect" runat="server" Font-Size="X-Small" Visible="false"
                                                                Width="199px" Height="16px" OnClick="LinkButtondirect_Click" Font-Names="Book Antiqua">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHolderdirect" runat="server"></asp:PlaceHolder>
                                                        <asp:DropDownExtender ID="DropDownExtender4" runat="server" DropDownControlID="pdirect"
                                                            DynamicServicePath="" Enabled="true" TargetControlID="tbdirect">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton1" runat="server" Text="Staff Name" GroupName="reference"
                                                            AutoPostBack="True" OnCheckedChanged="rbstaffname_CheckedChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbstaffname" runat="server" Height="19px" ReadOnly="true" Width="100px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div id="Div20" runat="server" class="linkbtn" style="margin-left: -45px; margin-top: 12px;
                                                            position: absolute;">
                                                            <asp:LinkButton ID="LinkButtonstaff" runat="server" Font-Size="X-Small" Visible="false"
                                                                Width="199px" Height="16px" OnClick="LinkButtonstaff_Click" Font-Names="Book Antiqua">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHolderstaff" runat="server"></asp:PlaceHolder>
                                                        <asp:DropDownExtender ID="DropDownExtender2" runat="server" DropDownControlID="pstaffname"
                                                            DynamicServicePath="" Enabled="true" TargetControlID="tbstaffname">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbothers" runat="server" Text="Others" GroupName="reference"
                                                            AutoPostBack="True" OnCheckedChanged="rbothers_CheckedChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlconsultant" AutoPostBack="true" OnSelectedIndexChanged="ddlconsultant_SelectedIndexChanged"
                                                            runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="tbothers" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="pdirect" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="109px" ScrollBars="Vertical" Width="197px">
                                                            <asp:CheckBoxList ID="cbldirect" runat="server" Font-Size="Medium" Width="157px"
                                                                AutoPostBack="True" OnSelectedIndexChanged="cbldirect_SelectedIndexChanged" Height="34px"
                                                                Font-Bold="True" Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="pstaffname" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="109px" ScrollBars="Vertical" Width="197px">
                                                            <asp:CheckBoxList ID="cblstaffname" runat="server" Font-Size="Medium" Width="182px"
                                                                AutoPostBack="True" OnSelectedIndexChanged="cblstaffname_SelectedIndexChanged"
                                                                Height="38px" Font-Bold="True" Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </Content>
                                    </asp:AccordionPane>
                                    <asp:AccordionPane runat="server" ID="AccordionPane2">
                                        <Header>
                                            <span style="margin-left: 12px;">Personal Details</span></Header>
                                        <Content>
                                            <div id="Div16" runat="server" class="btn">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label6" Width="161px" runat="server" Text="Student Date of Birth"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"></asp:Label>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label47" runat="server" Text="From" Font-Size="Medium" Font-Bold="True"
                                                            Font-Names="Book Antiqua"></asp:Label>
                                                        &nbsp;
                                                        <asp:TextBox ID="tbfromdob" runat="server" Height="16px" Width="75px" AutoPostBack="True"
                                                            OnTextChanged="tbfromdob_TextChanged" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        &nbsp;
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="tbfromdob"
                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','-'" />
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="d-MM-yyyy" TargetControlID="tbfromdob">
                                                        </asp:CalendarExtender>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label48" runat="server" Font-Size="Medium" Text="To" Font-Bold="True"
                                                            Font-Names="Book Antiqua"></asp:Label>
                                                        <asp:TextBox ID="tbtodob" runat="server" Height="16px" Width="75px" AutoPostBack="True"
                                                            OnTextChanged="tbtodob_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:TextBox><asp:Label ID="Labeldatedob" runat="server" Text="Enter from Date"
                                                                Visible="False" ForeColor="Red" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" TargetControlID="tbtodob"
                                                            FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','-'" />
                                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="d-MM-yyyy" TargetControlID="tbtodob">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                        <asp:Label ID="Label10" runat="server" Font-Size="Medium" Text="Father Name" Width="104px"
                                                            Height="16px" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style17">
                                                        <asp:DropDownList ID="ddlfname" runat="server" Height="20px" Width="150px" OnSelectedIndexChanged="ddlfname_SelectedIndexChanged"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                    </td>
                                                    <td class="style290">
                                                        <asp:TextBox ID="tbfname" runat="server" Height="16px" Width="150" OnTextChanged="tbfname_TextChanged"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="tbfname"
                                                            FilterType="Custom, LowercaseLetters,UppercaseLetters" ValidChars="." />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label23" runat="server" Text="Mother Name" Width="152px" Font-Size="Medium"
                                                            Height="22px" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlmname" runat="server" Height="20px" Width="150px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlmname_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbmname" runat="server" Height="16px" Width="150px" Style="margin-top: 4px"
                                                            Enabled="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="tbmname"
                                                            FilterType="Custom, LowercaseLetters,UppercaseLetters" ValidChars="." />
                                                    </td>
                                                    <td class="style329">
                                                        <asp:Label ID="Label29" runat="server" Font-Size="Medium" Text="Guardian Name" Width="123px"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                        &nbsp;
                                                    </td>
                                                    <td class="style16">
                                                        <br />
                                                        <asp:DropDownList ID="ddlgname" runat="server" AutoPostBack="True" Height="20px"
                                                            OnSelectedIndexChanged="ddlgname_SelectedIndexChanged" Width="150px" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td class="style302">
                                                        <asp:TextBox ID="tbgname" runat="server" Enabled="False" Style="margin-top: 6px"
                                                            Height="16px" Width="150px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="tbgname"
                                                            FilterType="Custom, LowercaseLetters,UppercaseLetters" ValidChars="." />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td class="style275">
                                                        <asp:Label ID="Label13" runat="server" Text="Seat Type" Width="75px" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style276">
                                                        <asp:TextBox ID="tbseattype" runat="server" Height="16px" Width="80px" ReadOnly="true"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div id="Div11" runat="server" class="linkbtn" style="left: -45px; position: absolute;">
                                                            <asp:LinkButton ID="LinkButtonseattype" runat="server" Font-Size="X-Small" OnClick="LinkButtonseattype_Click"
                                                                Visible="false" Width="100px" Height="16px">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHolderseattype" runat="server"></asp:PlaceHolder>
                                                        <asp:Panel ID="pseattype" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="109px" ScrollBars="Auto" Width="130px">
                                                            <asp:CheckBox ID="CheckBoxseat" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxseat_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblseattype" runat="server" Font-Size="Medium" Width="98px"
                                                                OnSelectedIndexChanged="cblseattype_SelectedIndexChanged" AutoPostBack="True"
                                                                Font-Bold="True" Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:DropDownExtender ID="ddeseattype" runat="server" DropDownControlID="pseattype"
                                                            DynamicServicePath="" Enabled="true" TargetControlID="tbseattype">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                    <td class="style277">
                                                        <asp:Label ID="Label14" runat="server" Text="Blood Group" Width="112px" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style275">
                                                        <asp:TextBox ID="tbblood" runat="server" Height="20px" OnTextChanged="tbblood_TextChanged"
                                                            ReadOnly="true" Width="135px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div id="Div12" runat="server" class="linkbtn">
                                                            <asp:LinkButton ID="LinkButtonblood" runat="server" Font-Size="X-Small" OnClick="LinkButtonblood_Click"
                                                                Visible="false" Width="120px">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHolderblood" runat="server"></asp:PlaceHolder>
                                                        <asp:Panel ID="pblood" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="112px" ScrollBars="Vertical" Width="134px">
                                                            <asp:CheckBox ID="CheckBoxblood" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxblood_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblblood" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblblood_SelectedIndexChanged" Width="98px" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:DropDownExtender ID="ddeblood" runat="server" DropDownControlID="pblood" DynamicServicePath=""
                                                            Enabled="true" TargetControlID="tbblood">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style5">
                                                        <asp:Label ID="Label15" runat="server" Text="Caste" Width="200px" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style4">
                                                        <asp:TextBox ID="tbcaste" runat="server" Height="19px" ReadOnly="true" Width="129px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <div id="castediv" runat="server" class="linkbtn">
                                                            <asp:LinkButton ID="LinkButtoncaste" runat="server" Font-Size="X-Small" OnClick="LinkButtoncaste_Click"
                                                                Visible="false" Width="120px">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHoldercaste" runat="server"></asp:PlaceHolder>
                                                        <asp:Panel ID="pcaste" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="150px" ScrollBars="Vertical" Width="139px">
                                                            <asp:CheckBox ID="CheckBoxcaste" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxcaste_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblcaste" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblcaste_SelectedIndexChanged" Width="98px" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:DropDownExtender ID="ddecaste" runat="server" DropDownControlID="pcaste" DynamicServicePath=""
                                                            Enabled="true" TargetControlID="tbcaste">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                    <td class="style10">
                                                        <asp:Label ID="Label17" runat="server" Text="Religion" Width="75px" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style1">
                                                        <asp:TextBox ID="tbreligion" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <div id="Div1" runat="server" class="linkbtn">
                                                            <asp:LinkButton ID="LinkButtonreligion" runat="server" Font-Size="X-Small" OnClick="LinkButtonreligion_Click"
                                                                Visible="false" Width="120px">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHolderreligion" runat="server"></asp:PlaceHolder>
                                                        <asp:Panel ID="preligion" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="112px" ScrollBars="Vertical" Width="134px">
                                                            <asp:CheckBox ID="CheckBoxreligion" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxreligion_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblreligion" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblreligion_SelectedIndexChanged" Width="98px" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:DropDownExtender ID="ddereligion" runat="server" DropDownControlID="preligion"
                                                            DynamicServicePath="" Enabled="true" TargetControlID="tbreligion">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                </tr>
                                                <%--Community,Region--%>
                                                <tr>
                                                    <td class="style5">
                                                        <asp:Label ID="Label16" runat="server" Text="Community" Width="200px" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style291">
                                                        <asp:TextBox ID="tbcomm" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div id="Div2" runat="server" class="linkbtn">
                                                            <asp:LinkButton ID="LinkButtoncomm" runat="server" Font-Size="X-Small" OnClick="LinkButtoncomm_Click"
                                                                Visible="false" Width="120px">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHoldercomm" runat="server"></asp:PlaceHolder>
                                                        <asp:Panel ID="pcomm" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="118px" ScrollBars="Vertical" Width="135px">
                                                            <asp:CheckBox ID="CheckBoxcomm" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxcomm_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblcomm" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblcomm_SelectedIndexChanged" Width="98px" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:DropDownExtender ID="ddecomm" runat="server" DropDownControlID="pcomm" DynamicServicePath=""
                                                            Enabled="true" TargetControlID="tbcomm">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                    <td class="style10">
                                                        <asp:Label ID="Label18" runat="server" Text="Region" Width="75px" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style1">
                                                        <asp:TextBox ID="tbregion" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div id="Div3" runat="server" class="linkbtn">
                                                            <asp:LinkButton ID="LinkButtonregion" runat="server" Font-Size="X-Small" OnClick="LinkButtonregion_Click"
                                                                Visible="false" Width="120px">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHolderregion" runat="server"></asp:PlaceHolder>
                                                        <asp:Panel ID="pregion" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="112px" ScrollBars="Vertical" Width="134px">
                                                            <asp:CheckBox ID="CheckBoxregion" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxregion_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblregion" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblregion_SelectedIndexChanged" Width="98px" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:DropDownExtender ID="dderegion" runat="server" DropDownControlID="pregion" DynamicServicePath=""
                                                            Enabled="true" TargetControlID="tbregion">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                </tr>
                                                <%--mother tongue--%>
                                                <tr>
                                                    <td class="style5">
                                                        <asp:Label ID="Label19" runat="server" Text="Mother Tongue" Width="200px" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style291">
                                                        <asp:TextBox ID="tbmtongue" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div id="Div4" runat="server" class="linkbtn">
                                                            <asp:LinkButton ID="LinkButtonmtongue" runat="server" Font-Size="X-Small" OnClick="LinkButtonmtongue_Click"
                                                                Visible="false" Width="120px">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHoldermtongue" runat="server"></asp:PlaceHolder>
                                                        <asp:Panel ID="pmtongue" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="112px" ScrollBars="Vertical" Width="134px">
                                                            <asp:CheckBox ID="CheckBoxmtongue" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxmtongue_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblmtongue" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblmtongue_SelectedIndexChanged" Width="98px" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:DropDownExtender ID="ddemtongue" runat="server" DropDownControlID="pmtongue"
                                                            DynamicServicePath="" Enabled="true" TargetControlID="tbmtongue">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                    <td class="style10">
                                                        <asp:Label ID="Label20" runat="server" Text="Father Mobile No" Width="132px" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbfmobno" runat="server" Width="200" MaxLength="10" AutoPostBack="True"
                                                            OnTextChanged="tbfmobno_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="tbfmobno"
                                                            FilterType="Numbers" />
                                                        <br />
                                                        <asp:Label ID="lblfmobno" runat="server" ForeColor="Red" Text="Enter valid Mobile No"
                                                            Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <%--father occupation--%>
                                                <tr>
                                                    <td class="style5">
                                                        <asp:Label ID="Label21" runat="server" Text="Father Occupation" Width="137px" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style291">
                                                        <asp:TextBox ID="tbfoccu" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div id="Div5" runat="server" class="linkbtn">
                                                            <asp:LinkButton ID="LinkButtonfoccu" runat="server" Font-Size="X-Small" OnClick="LinkButtonfoccu_Click"
                                                                Visible="false" Width="120px">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHolderfoccu" runat="server"></asp:PlaceHolder>
                                                        <asp:Panel ID="pfoccu" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="112px" ScrollBars="Vertical" Width="134px">
                                                            <asp:CheckBox ID="CheckBoxfoccu" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxfoccu_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblfoccu" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblfoccu_SelectedIndexChanged" Width="98px" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:DropDownExtender ID="ddefoccu" runat="server" DropDownControlID="pfoccu" DynamicServicePath=""
                                                            Enabled="true" TargetControlID="tbfoccu">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                    <td class="style10">
                                                        <asp:Label ID="Label22" runat="server" Text="Father office Mobile No" Width="179px"
                                                            Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua" Height="19px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbfoffno" runat="server" Width="200" MaxLength="10" AutoPostBack="True"
                                                            OnTextChanged="tbfoffno_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="tbfoffno"
                                                            FilterType="Numbers" />
                                                        <br />
                                                        <asp:Label ID="lblfoffno" runat="server" ForeColor="Red" Text="Enter Valid Mobile No"
                                                            Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <%--father qualification--%>
                                                <tr>
                                                    <td class="style5">
                                                        <asp:Label ID="Label24" runat="server" Text="Father Qualification" Width="200px"
                                                            Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style291">
                                                        <asp:TextBox ID="tbfqual" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div id="Div6" runat="server" class="linkbtn">
                                                            <asp:LinkButton ID="LinkButtonfqual" runat="server" Font-Size="X-Small" OnClick="LinkButtonfqual_Click"
                                                                Visible="false" Width="120px">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHolderfqual" runat="server"></asp:PlaceHolder>
                                                        <asp:Panel ID="pfqual" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="112px" ScrollBars="Vertical" Width="134px">
                                                            <asp:CheckBox ID="CheckBoxfqual" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxfqual_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblfqual" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblfqual_SelectedIndexChanged" Width="98px" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:DropDownExtender ID="ddefqual" runat="server" DropDownControlID="pfqual" DynamicServicePath=""
                                                            Enabled="true" TargetControlID="tbfqual">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                    <td class="style10">
                                                        <asp:Label ID="Label25" runat="server" Text="Mother Mobile No" Width="200px" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbmmobno" runat="server" Width="200" MaxLength="10" AutoPostBack="True"
                                                            OnTextChanged="tbmmobno_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="tbmmobno"
                                                            FilterType="Numbers" />
                                                        <br />
                                                        <asp:Label ID="lblmmobno" runat="server" ForeColor="Red" Text="Enter Valid Mobile No"
                                                            Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <%--mother occupation--%>
                                                <tr>
                                                    <td class="style5">
                                                        <asp:Label ID="Label26" runat="server" Text="Mother Occupation" Width="200px" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style291">
                                                        <asp:TextBox ID="tbmoccu" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div id="Div7" runat="server" class="linkbtn">
                                                            <asp:LinkButton ID="LinkButtonmoccu" runat="server" Font-Size="X-Small" OnClick="LinkButtonmoccu_Click"
                                                                Visible="false" Width="120px">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHoldermoccu" runat="server"></asp:PlaceHolder>
                                                        <asp:Panel ID="pmoccu" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="112px" ScrollBars="Vertical" Width="134px">
                                                            <asp:CheckBox ID="CheckBoxmoccu" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxmoccu_CheckedChanged" Font-Bold="True"
                                                                Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblmoccu" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblmoccu_SelectedIndexChanged" Width="98px" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:DropDownExtender ID="ddemoccu" runat="server" DropDownControlID="pmoccu" DynamicServicePath=""
                                                            Enabled="true" TargetControlID="tbmoccu">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                    <td class="style10">
                                                        <asp:Label ID="Label27" runat="server" Text="Mother Office Mobile No" Width="200px"
                                                            Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbmoffno" runat="server" Width="200" MaxLength="10" AutoPostBack="True"
                                                            OnTextChanged="tbmoffno_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="tbmoffno"
                                                            FilterType="Numbers" />
                                                        <br />
                                                        <asp:Label ID="lblmoffno" runat="server" ForeColor="Red" Text="Enter Valid Mobile No"
                                                            Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <%--mother qualification--%>
                                                <tr>
                                                    <td class="style5">
                                                        <asp:Label ID="Label28" runat="server" Text="Mother Qualification" Width="200px"
                                                            Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style291">
                                                        <asp:TextBox ID="tbmqual" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div id="Div8" runat="server" class="linkbtn">
                                                            <asp:LinkButton ID="LinkButtonmqual" runat="server" Font-Size="X-Small" OnClick="LinkButtonmqual_Click"
                                                                Visible="false" Width="120px">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHoldermqual" runat="server"></asp:PlaceHolder>
                                                        <asp:Panel ID="pmqual" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" Height="112px" ScrollBars="Vertical" Width="134px">
                                                            <asp:CheckBox ID="CheckBoxmqual" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxmqual_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblmqual" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblmqual_SelectedIndexChanged" Width="98px" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:DropDownExtender ID="ddemqual" runat="server" DropDownControlID="pmqual" DynamicServicePath=""
                                                            Enabled="true" TargetControlID="tbmqual">
                                                        </asp:DropDownExtender>
                                                    </td>
                                                    <td class="style10">
                                                        <asp:Label ID="Label30" runat="server" Text="Student Mobile No" Width="200px" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbstudmobno" runat="server" MaxLength="10" AutoPostBack="True" Height="20px"
                                                            Width="200px" OnTextChanged="tbstudmobno_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="tbstudmobno"
                                                            FilterType="Numbers" />
                                                        <br />
                                                        <asp:Label ID="lblsmobno" runat="server" ForeColor="Red" Text="Enter Valid Mobile No"
                                                            Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </Content>
                                    </asp:AccordionPane>
                                    <asp:AccordionPane runat="server" ID="AccordionPane3">
                                        <Header>
                                            <span style="margin-left: 12px;">Contact Details</span></Header>
                                        <Content>
                                            <div id="Div17" runat="server" class="btn">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                            <table>
                                                <tr>
                                                    <td class="style298">
                                                        <asp:Label ID="Label31" Width="200px" runat="server" Text="Student Personal E-Mail Id"
                                                            Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style298">
                                                        <asp:DropDownList ID="ddlpemailid1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlpemailid1_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style303">
                                                        &nbsp;
                                                    </td>
                                                    <td class="style299">
                                                    </td>
                                                    <td class="style298">
                                                        <asp:Label ID="Label34" Width="125px" runat="server" Text="Permanent Street" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style298">
                                                        <asp:DropDownList ID="ddlpstreet1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlpstreet1_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style298">
                                                        <asp:DropDownList ID="ddlpstreet" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="ddlpstreet_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style298">
                                                        <asp:TextBox ID="tbpstreet" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="tbpstreet"
                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style295">
                                                        <asp:Label ID="Label35" Width="185px" runat="server" Text="Permanent City / Taluk"
                                                            Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style295">
                                                        <asp:DropDownList ID="ddlpcity1" Width="200" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlpcity1_SelectedIndexChanged"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style304">
                                                        <asp:DropDownList ID="ddlpcity" Width="150" runat="server" AutoPostBack="True" Visible="False"
                                                            OnSelectedIndexChanged="ddlpcity_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style296">
                                                        <asp:TextBox ID="tbpcity" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="tbpcity"
                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                    <td class="style295">
                                                        <asp:Label ID="Label37" Width="146px" runat="server" Text="Permanent Country" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style295">
                                                        <asp:DropDownList ID="ddlpcountry1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlpcountry1_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style295">
                                                        <asp:DropDownList ID="ddlpcountry" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="ddlpcountry_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style295">
                                                        <asp:TextBox ID="tbpcountry" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="tbpcountry"
                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label39" Width="145px" runat="server" Text="Permanent District" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlpdistrict1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlpdistrict1_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style305">
                                                        <asp:DropDownList ID="ddlpdistrict" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="ddlpdistrict_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style288">
                                                        <asp:TextBox ID="tbpdistrict" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label44" Width="120px" runat="server" Text="Contact District" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlcdistrict1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlcdistrict1_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlcdistrict" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="ddlcdistrict_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbcdistrict" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="tbcdistrict"
                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label45" Width="151px" runat="server" Text="Contact Country" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlccountry1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlccountry1_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style305">
                                                        <asp:DropDownList ID="ddlccountry" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="ddlccountry_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style288">
                                                        <asp:TextBox ID="tbccountry" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="tbccountry"
                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label36" Width="120px" runat="server" Text="Contact street" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlcstreet1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlcstreet1_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlcstreet" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="ddlcstreet_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbcstreet" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="tbcstreet"
                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label38" Width="185px" runat="server" Text="Contact city / Taluk"
                                                            Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlccity1" Width="200" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlccity1_SelectedIndexChanged"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style305">
                                                        <asp:DropDownList ID="ddlccity" Width="150" runat="server" AutoPostBack="True" Visible="False"
                                                            OnSelectedIndexChanged="ddlccity_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style288">
                                                        <asp:TextBox ID="tbccity" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="tbccity"
                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label40" Width="133px" runat="server" Text="Guardian District" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlgdistrict1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlgdistrict1_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlgdistrict" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="ddlgdistrict_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbgdistrict" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="tbgdistrict"
                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label46" Width="153px" runat="server" Text="Guardian Country" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlgcountry1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlgcountry1_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style305">
                                                        <asp:DropDownList ID="ddlgcountry" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="ddlgcountry_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style288">
                                                        <asp:TextBox ID="tbgcountry" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="tbgcountry"
                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label11" Width="120px" runat="server" Text="Guardian City" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlgcity1" Width="200" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlgcity1_SelectedIndexChanged"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlgcity" Width="150" runat="server" AutoPostBack="True" Visible="False"
                                                            OnSelectedIndexChanged="ddlgcity_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbgcity" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label58" Width="120px" runat="server" Text="Guardian Street" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlgstreet1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlgstreet1_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlgstreet" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="ddlgstreet_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbgstreet" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="tbgstreet"
                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label41" Width="120px" runat="server" Text="Permanent State" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlpstate1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlpstate1_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlpstate" Width="150" runat="server" AutoPostBack="True" Visible="False"
                                                            OnSelectedIndexChanged="ddlpstate_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbstatep" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label32" Width="153px" runat="server" Text="Contact State" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlcstate1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlcstate1_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style305">
                                                        <asp:DropDownList ID="ddlcstate" Width="150" runat="server" AutoPostBack="True" Visible="False"
                                                            OnSelectedIndexChanged="ddlcstate_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="style288">
                                                        <asp:TextBox ID="tbstatec" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="tbgcountry"
                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label33" Width="120px" runat="server" Text="Guardian State" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlgstate1" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlgstate1_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlgstate" Width="150" runat="server" AutoPostBack="True" Visible="False"
                                                            OnSelectedIndexChanged="ddlgstate_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbstateg" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <%--Start Aruna add pin and address  =========--%>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label53" Width="120px" runat="server" Text="Permanent Address" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_padress" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="drp_padress_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_padress1" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="drp_padress1_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_padress" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label54" Width="120px" runat="server" Text="Contact Address" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_cadress" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="drp_cadress_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_cadress1" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="drp_cadress1_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_cadress" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label57" Width="120px" runat="server" Text="Guardian Address" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_gadress" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="drp_gadress_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_gadress1" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="drp_gadress1_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_gadress" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label59" Width="120px" runat="server" Text="Permanent Pincode" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_ppincode" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="drp_ppincode_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_ppincode1" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="drp_ppincode1_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_ppincode" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label60" Width="120px" runat="server" Text="Contact Pincode" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_cpincode" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="drp_cpincode_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_cpincode1" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="drp_cpincode1_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_cpincode" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label61" Width="120px" runat="server" Text="Guardian Pincode" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_gpincode" Width="200" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="drp_gpincode_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drp_gpincode1" Width="150" runat="server" AutoPostBack="True"
                                                            Visible="False" OnSelectedIndexChanged="drp_gpincode1_SelectedIndexChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_gpincode" Width="100" runat="server" Visible="False" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <%--End ======================================--%>
                                            </table>
                                            <br />
                                            <br />
                                            <br />
                                        </Content>
                                    </asp:AccordionPane>
                                    <asp:AccordionPane runat="server" ID="AccordionPane4">
                                        <Header>
                                            <span style="margin-left: 12px;">Course Details</span></Header>
                                        <Content>
                                            <div id="Div18" runat="server" class="btn">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                            <table>
                                                <tr>
                                                    <td class="style21">
                                                        <asp:Label ID="Label1" Width="144px" runat="server" Text="Student Batch Year" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                        &nbsp;<asp:Label ID="Label55" runat="server" Font-Size="Medium" Height="20px" Text="From"
                                                            Width="37px" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style305">
                                                        <asp:DropDownList ID="ddlbatchyrfrm" runat="server" Font-Bold="True" Height="21px"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" Width="60px">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="Label56" runat="server" Font-Size="Medium" Height="21px" Text="To"
                                                            Width="19px" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                        &nbsp;<asp:DropDownList ID="ddlbatchyrto" runat="server" Height="21px" Width="60px"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_stuDegree" runat="server" Font-Size="Medium" Text="Student Degree"
                                                            Width="120px" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style1">
                                                        <asp:TextBox ID="tbdegree" runat="server" Height="15px" Width="138px" ReadOnly="true"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div class="linkbtn" id="Div10" runat="server">
                                                            <asp:LinkButton ID="LinkButtondegree" Font-Size="X-Small" Visible="false" runat="server"
                                                                Width="120px" OnClick="LinkButtondegree_Click">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHolderdegree" runat="server"></asp:PlaceHolder>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style21">
                                                    </td>
                                                    <td class="style305">
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td class="style1">
                                                        <asp:Panel ID="pdegree" runat="server" Height="112px" Width="134px" BorderColor="Black"
                                                            BorderStyle="Solid" BorderWidth="2px" BackColor="White" ScrollBars="Vertical">
                                                            <asp:CheckBox ID="CheckBoxdegree" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxdegree_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cbldegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                Width="98px" OnSelectedIndexChanged="cbldegree_SelectedIndexChanged" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <asp:DropDownExtender ID="ddedegree" Enabled="true" DynamicServicePath="" DropDownControlID="pdegree"
                                                    TargetControlID="tbdegree" runat="server">
                                                </asp:DropDownExtender>
                                                <tr>
                                                    <td class="style278">    

                                                        <asp:Label ID="Label3" Width="200px" runat="server" Text="Student Branch" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style279">
                                                        <asp:TextBox ID="tbbranch" runat="server" Height="20px" Width="135px" ReadOnly="true"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div class="linkbtn" id="Div9" runat="server">
                                                            <asp:LinkButton ID="LinkButtonbranch" Font-Size="X-Small" Visible="false" runat="server"
                                                                Width="120px" OnClick="LinkButtonbranch_Click">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHolderbranch" runat="server"></asp:PlaceHolder>
                                                    </td>
                                                    <td class="style280">
                                                        <asp:Label ID="Label42" Width="200px" runat="server" Text="Student Section" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style281">
                                                        <asp:TextBox ID="tbsection" runat="server" Height="20px" Width="135px" ReadOnly="true"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div class="linkbtn" id="Div13" runat="server">
                                                            <asp:LinkButton ID="LinkButtonsection" Font-Size="X-Small" Visible="false" runat="server"
                                                                Width="120px" OnClick="LinkButtonsec_Click">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHoldersection" runat="server"></asp:PlaceHolder>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style21">
                                                    </td>
                                                    <td class="style332">
                                                        <asp:Panel ID="pbranch" runat="server" Height="112px" Width="134px" BorderColor="Black"
                                                            BorderStyle="Solid" BorderWidth="2px" BackColor="White" ScrollBars="Vertical">
                                                            <asp:CheckBox ID="CheckBoxbranch" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxbranch_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                Width="98px" OnSelectedIndexChanged="cblbranch_SelectedIndexChanged" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                    <asp:DropDownExtender ID="ddebranch" Enabled="true" DynamicServicePath="" DropDownControlID="pbranch"
                                                        TargetControlID="tbbranch" runat="server">
                                                    </asp:DropDownExtender>
                                                    <td>
                                                    </td>
                                                    <td class="style1">
                                                        <asp:Panel ID="psection" runat="server" CssClass="multxtpanel" Height="112px" Width="134px"
                                                            BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" BackColor="White" ScrollBars="Vertical">
                                                            <asp:CheckBox ID="CheckBoxsection" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxsection_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblsection" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                Width="98px" OnSelectedIndexChanged="cblsection_SelectedIndexChanged" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <asp:DropDownExtender ID="ddesection" Enabled="true" DynamicServicePath="" DropDownControlID="psection"
                                                    TargetControlID="tbsection" runat="server">
                                                </asp:DropDownExtender>
                                                <tr>
                                                    <td class="style21">
                                                        <asp:Label ID="lbl_stuSemOrT" Width="200px" runat="server" Text="Student Semester"
                                                            Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td class="style332">
                                                        <asp:TextBox ID="tbsem" runat="server" Height="20px" Width="135px" ReadOnly="true"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <br />
                                                        <div class="linkbtn" id="Div14" runat="server">
                                                            <asp:LinkButton ID="LinkButtonsemester" Font-Size="X-Small" Visible="false" runat="server"
                                                                Width="120px" OnClick="LinkButtonsem_Click">Remove All</asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <asp:PlaceHolder ID="PlaceHoldersemester" runat="server"></asp:PlaceHolder>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbllaststudied" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Last Studied School/College Name"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddllaststudied" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" runat="server">
                                                        </asp:DropDownList>
                                                        <td>
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td class="style21">
                                                    </td>
                                                    <td class="style332">
                                                        <asp:Panel ID="psem" runat="server" Height="112px" Width="134px" BorderColor="Black"
                                                            BorderStyle="Solid" BorderWidth="2px" BackColor="White" ScrollBars="Vertical">
                                                            <asp:CheckBox ID="CheckBoxsem" Text="Select All" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" OnCheckedChanged="CheckBoxsem_CheckedChanged" Visible="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" />
                                                            <asp:CheckBoxList ID="cblsem" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                OnSelectedIndexChanged="cblsem_SelectedIndexChanged" Width="98px" Font-Bold="True"
                                                                Font-Names="Book Antiqua">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <asp:DropDownExtender ID="DropDownExtender1" Enabled="true" DynamicServicePath=""
                                                    DropDownControlID="psem" TargetControlID="tbsem" runat="server">
                                                </asp:DropDownExtender>
                                            </table>
                                        </Content>
                                    </asp:AccordionPane>
                                    <asp:AccordionPane runat="server" ID="AccordionPane5">
                                        <Header>
                                            <span style="margin-left: 12px;">Column Order</span>
                                            <%--<span id="expandCollapse_span" style="float:right;margin-right:12px;font-size:20px;font-weight:bold;">+</span>--%>
                                        </Header>
                                        <Content>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="CheckBoxselect" Text="Select All" runat="server" Font-Size="Small"
                                                            AutoPostBack="True" OnCheckedChanged="CheckBoxselect_CheckedChanged" Font-Bold="True"
                                                            Font-Names="Book Antiqua" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton
                                                            ID="LinkButtonsremove" CssClass="flright" Font-Size="X-Small" runat="server"
                                                            Width="59px" OnClick="LinkButtonsremove_Click" Height="16px">Remove  All</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <asp:TextBox ID="tborder" Visible="false" ReadOnly="true" Width="1000" TextMode="MultiLine"
                                                        CssClass="style1" AutoPostBack="true" runat="server">
                                                    </asp:TextBox>
                                                </tr>
                                                <tr>
                                                    <asp:CheckBoxList ID="cblsearch" AutoPostBack="true" CssClass="styles" RepeatColumns="6"
                                                        RepeatDirection="Horizontal" runat="server" OnSelectedIndexChanged="cblsearch_SelectedIndexChanged1"
                                                        Height="275px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small">
                                                        <asp:ListItem>Application No</asp:ListItem>
                                                        <asp:ListItem>Admission No</asp:ListItem>
                                                        <asp:ListItem>Roll No</asp:ListItem>
                                                        <asp:ListItem>Register No</asp:ListItem>
                                                        <asp:ListItem>Student Name</asp:ListItem>
                                                        <asp:ListItem>Gender</asp:ListItem>
                                                        <asp:ListItem>Student Applied date</asp:ListItem>
                                                        <asp:ListItem>Student Admitted Date</asp:ListItem>
                                                        <asp:ListItem>Student Date of Birth</asp:ListItem>
                                                        <asp:ListItem>Father Name</asp:ListItem>
                                                        <asp:ListItem>Mother Name</asp:ListItem>
                                                        <asp:ListItem>Guardian Name</asp:ListItem>
                                                        <asp:ListItem>Seat Type</asp:ListItem>
                                                        <asp:ListItem>Blood Group</asp:ListItem>
                                                        <asp:ListItem>Caste</asp:ListItem>
                                                        <asp:ListItem>Religion</asp:ListItem>
                                                        <asp:ListItem>Community</asp:ListItem>
                                                        <asp:ListItem>Region</asp:ListItem>
                                                        <asp:ListItem>Mother Tongue</asp:ListItem>
                                                        <asp:ListItem>Father Occupation</asp:ListItem>
                                                        <asp:ListItem>Father Qualification</asp:ListItem>
                                                        <asp:ListItem>Father Mobile No.</asp:ListItem>
                                                        <asp:ListItem>Father Office Mobile No</asp:ListItem>
                                                        <asp:ListItem>Mother Occupation</asp:ListItem>
                                                        <asp:ListItem>Mother Qualification</asp:ListItem>
                                                        <asp:ListItem>Mother Mobile No</asp:ListItem>
                                                        <asp:ListItem>Mother Office Mobile No</asp:ListItem>
                                                        <asp:ListItem>Student Mobile No</asp:ListItem>
                                                        <asp:ListItem>Student Personal EMail Id</asp:ListItem>
                                                        <asp:ListItem>Permanent City</asp:ListItem>
                                                        <asp:ListItem>Permanent District</asp:ListItem>
                                                        <asp:ListItem>Permanent Country</asp:ListItem>
                                                        <asp:ListItem>Permanent Street</asp:ListItem>
                                                        <asp:ListItem>Contact City</asp:ListItem>
                                                        <asp:ListItem>Contact District</asp:ListItem>
                                                        <asp:ListItem>Contact Country</asp:ListItem>
                                                        <asp:ListItem>Contact Street</asp:ListItem>
                                                        <asp:ListItem>Guardian city</asp:ListItem>
                                                        <asp:ListItem>Guardian District</asp:ListItem>
                                                        <asp:ListItem>Guardian Country</asp:ListItem>
                                                        <asp:ListItem>Guardian Street</asp:ListItem>
                                                        <asp:ListItem>Student Batch Year</asp:ListItem>
                                                        <asp:ListItem>Degree</asp:ListItem>
                                                        <asp:ListItem>Department</asp:ListItem>
                                                        <asp:ListItem>Semester</asp:ListItem>
                                                        <asp:ListItem>Direct Reference</asp:ListItem>
                                                        <asp:ListItem>Referred by Staff</asp:ListItem>
                                                        <asp:ListItem>Referred by Others</asp:ListItem>
                                                        <asp:ListItem>Hostler/Day Scholar</asp:ListItem>
                                                        <asp:ListItem>Last Studied School/College Name</asp:ListItem>
                                                        <asp:ListItem>Permanent State</asp:ListItem>
                                                        <asp:ListItem>Contact State</asp:ListItem>
                                                        <asp:ListItem>Guardian State</asp:ListItem>
                                                        <asp:ListItem>Remarks</asp:ListItem>
                                                        <asp:ListItem>College</asp:ListItem>
                                                        <asp:ListItem>SSLC Percentage</asp:ListItem>
                                                        <asp:ListItem>Year of Completion SSLC</asp:ListItem>
                                                        <asp:ListItem>HSC  Percentage</asp:ListItem>
                                                        <asp:ListItem>Year of Completion HSC</asp:ListItem>
                                                        <asp:ListItem>Diploma Percentage</asp:ListItem>
                                                        <asp:ListItem>Year of Completion Diploma</asp:ListItem>
                                                        <asp:ListItem>UG Percentage</asp:ListItem>
                                                        <asp:ListItem>Year of Completion UG</asp:ListItem>
                                                        <asp:ListItem>Permanent Address</asp:ListItem>
                                                        <asp:ListItem>Contact Address</asp:ListItem>
                                                        <asp:ListItem>Guardian Address</asp:ListItem>
                                                        <asp:ListItem>Permanent Pincode</asp:ListItem>
                                                        <asp:ListItem>Contact Pincode</asp:ListItem>
                                                        <asp:ListItem>Guardian Pincode</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </tr>
                                            </table>
                                        </Content>
                                    </asp:AccordionPane>
                                </Panes>
                            </asp:Accordion>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnsearch" runat="server" Text="Search" OnClick="btnsearch_Click"
                                Style="height: 26px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                CausesValidation="False" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="clear" runat="server" Text="Clear All" OnClick="clear_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Height="28px" Width="82px" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </asp:Panel>
                        <br />
                        <asp:Label ID="lblnorec" runat="server" Text="There are no records matched" ForeColor="Red"
                            Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <br />
                        <br />
                        <center>
                            <asp:Panel ID="Panelpage" runat="server" Height="37px">
                                <table class="maintablestyle" style="background-color: transparent; border-color: transparent;
                                    border-style: none; box-shadow: none;">
                                    <%--class="style336"--%>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Buttontotal" runat="server" Text="Label" Width="208px" Font-Bold="True"
                                                Font-Size="Medium" Visible="False" Font-Names="Book Antiqua" Height="17px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblrecord" runat="server" Visible="False" Font-Bold="True" Text="No of Records per Page"
                                                Font-Names="Book Antiqua" Width="180" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                                                Font-Bold="True" Visible="False" Width="60px" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Height="18px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="50px"
                                                AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" TargetControlID="TextBoxother"
                                                FilterType="Numbers" runat="server">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblother" runat="server" Text="Select" Visible="False" ForeColor="Red"
                                                Width="200" Style="font-weight: 400" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                                                Width="100px" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBoxpage" runat="server" Style="margin-bottom: 0px" Width="44px"
                                                Visible="False" AutoPostBack="True" OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="TextBoxpage"
                                                FilterType="Numbers" />
                                        </td>
                                        <td>
                                            <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Small" Height="16px" Width="333px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <FarPoint:FpSpread ID="FpSpread1" runat="server"  CssClass="spstyle" OnPreRender="FpSpread1_SelectedIndexChanged"
                                            OnCellClick="FpSpread1_CellClick" Height="221px" CommandBar-ButtonType="PushButton"
                                            CommandBar-ShowPDFButton="True" ShowHeaderSelection="false">
                                            <%--Width="798px"--%>
                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ShowPDFButton="true" ButtonType="PushButton"
                                                ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Report Name"></asp:Label>
                                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" runat="server" TargetControlID="txtexcelname"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnexcel" runat="server" Text="Export Excel" OnClick="btnexcel_Click"
                                            OnClientClick="return ExcelClick();" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Font-Bold="true" />
                                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                        <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <br />
                        <br />
                    </div>
                </center>
            </div>
        </body>
        </html>
    </p>
</asp:Content>
