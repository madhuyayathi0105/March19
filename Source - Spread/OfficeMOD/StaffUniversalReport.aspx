<%@ Page Title="About Us" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StaffUniversalReport.aspx.cs" Inherits="StaffUniversalReport"
    EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
        <body>
            <script type="text/javascript">
                function pageLoad() {
                    $find('DropDownExtender1')._dropWrapperHoverBehavior_onhover();
                    $find('DropDownExtender1').unhover = VisibleMe;

                }

                function VisibleMe() {
                    $find('DropDownExtender1')._dropWrapperHoverBehavior_onhover();
                }

            </script>
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <center>
                    <span class="fontstyleheader" style="color: green;">Staff Universal Report</span></center>
                </center>
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
                    <center>
                        <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnsearch">
                            <asp:Accordion runat="server" ID="Accordion1" HeaderCssClass="HeaderCSS" HeaderSelectedCssClass="HeaderSelectedCSS"
                                ContentCssClass="cpBody" Width="1100px" BorderColor="White" Style="margin-right: 22px">
                                <Panes>
                                    <asp:AccordionPane runat="server" ID="AccordionPane1">
                                        <Header>
                                            <span style="margin-left: 12px;">Registration Details</span>
                                        </Header>
                                        <Content>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" Text="Application No" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            Width="118px" Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlappno" runat="server" Height="20px" Width="125px" OnSelectedIndexChanged="ddlappno_SelectedIndexChanged"
                                                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp; &nbsp;
                                                        <asp:TextBox ID="tbappno" Height="16px" Width="100" runat="server" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblstaffname" runat="server" Text="Staff Name" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlstaffname" runat="server" Height="20px" Width="120px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlstudname_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp; &nbsp;
                                                        <asp:TextBox ID="tbstaffname" Height="16px" Width="121px" runat="server" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftbesname" runat="server" TargetControlID="tbstaffname"
                                                            FilterType="Custom, LowercaseLetters,UppercaseLetters" ValidChars="." />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="Department Name" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddldeptname" runat="server" Height="20px" Width="125px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddl_dept_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp; &nbsp;
                                                        <asp:TextBox ID="txtdept" Height="16px" Width="100px" runat="server" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtdept"
                                                            FilterType="Custom, LowercaseLetters,UppercaseLetters" ValidChars="." />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Lbl_desig" runat="server" Text="Designation Name" Font-Size="Medium"
                                                            Font-Names="Book Antiqua" Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_desig" runat="server" Height="20px" Width="121px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddl_desig_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp; &nbsp;
                                                        <asp:TextBox ID="txt_desig" Height="16px" Width="121px" runat="server" Enabled="False"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_desig"
                                                            FilterType="Custom, LowercaseLetters,UppercaseLetters" ValidChars="." />
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
                                                        <asp:TextBox ID="tbfromappdt" Width="75px" runat="server" Height="16px" AutoPostBack="True"
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
                                                </tr>
                                            </table>
                                        </Content>
                                    </asp:AccordionPane>
                                    <asp:AccordionPane runat="server" ID="AccordionPane2">
                                        <Header>
                                            <span style="margin-left: 12px;">Personal Details</span>
                                        </Header>
                                        <Content>
                                            <div id="Div16" runat="server" class="btn">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </div>
                                            <center>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label6" Width="161px" runat="server" Text="Staff Date of Birth" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Font-Bold="True"></asp:Label>
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
                                                            <asp:DropDownList ID="ddlfname" runat="server" Height="20px" Width="150px" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlfname_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium">
                                                            </asp:DropDownList>
                                                            &nbsp;
                                                        </td>
                                                        <td class="style290">
                                                            <asp:TextBox ID="tbfname" runat="server" Height="16px" Width="150" Enabled="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="tbfname"
                                                                FilterType="Custom, LowercaseLetters,UppercaseLetters" ValidChars="." />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style10">
                                                            <asp:Label ID="Label20" runat="server" Text="Mobile No" Width="132px" Font-Size="Medium"
                                                                Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tbfmobno" runat="server" Width="130" MaxLength="10" AutoPostBack="True"
                                                                OnTextChanged="tbfmobno_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="tbfmobno"
                                                                FilterType="Numbers" />
                                                            <br />
                                                            <asp:Label ID="lblfmobno" runat="server" ForeColor="Red" Text="Enter valid Mobile No"
                                                                Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                                                        </td>
                                                        <td class="style10">
                                                            <asp:Label ID="lbl_MaritalStatus" runat="server" Text="Marital Status" Width="132px"
                                                                Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                        </td>
                                                        <td class="style17">
                                                            <asp:DropDownList ID="ddl_maritalstatus" runat="server" AutoPostBack="true" Height="20px"
                                                                Width="150px" OnSelectedIndexChanged="ddl_maritalstatus_SelectedIndexChanged"
                                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                                            </asp:DropDownList>
                                                            &nbsp;
                                                        </td>
                                                        <td class="style290">
                                                            <asp:TextBox ID="txt_maritalstatus" runat="server" Height="16px" Width="150" Enabled="false"
                                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_maritalstatus"
                                                                FilterType="Custom, LowercaseLetters,UppercaseLetters" ValidChars="." />
                                                            <br />
                                                        </td>
                                                    </tr>
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
                                                        <td class="style277">
                                                            <asp:Label ID="Label14" runat="server" Text="Blood Group" Width="112px" Font-Size="Medium"
                                                                Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                        </td>
                                                        <td class="style275">
                                                            <asp:TextBox ID="tbblood" runat="server" Height="20px" ReadOnly="true" Width="135px"
                                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
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
                                                </table>
                                            </center>
                                        </Content>
                                    </asp:AccordionPane>
                                    <asp:AccordionPane runat="server" ID="AccordionPane3">
                                        <Header>
                                            <span style="margin-left: 12px;">Contact Details</span>
                                        </Header>
                                        <Content>
                                            <div id="Div17" runat="server" class="btn">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </div>
                                            <table>
                                                <tr>
                                                    <td class="style298">
                                                        <asp:Label ID="Label31" Width="200px" runat="server" Text="Staff E-Mail Id" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
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
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="tbstatec"
                                                            FilterType="LowercaseLetters,UppercaseLetters" />
                                                    </td>
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
                                                </tr>
                                            </table>
                                        </Content>
                                    </asp:AccordionPane>
                                    <asp:AccordionPane runat="server" ID="AccordionPane4">
                                        <Header>
                                            <span style="margin-left: 12px;">Experience Details</span>
                                        </Header>
                                        <Content>
                                            <div id="Div18" runat="server" class="btn">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </div>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblexp" runat="server" Text="Year Of Experience" Font-Size="Medium"
                                                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlyearofexp" runat="server" Height="20px" Width="120px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlyearofexp_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        &nbsp;&nbsp; &nbsp;
                                                        <asp:TextBox ID="txtyearofexp" Height="16" Width="120" MaxLength="16" runat="server"
                                                            Enabled="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtyearofexp"
                                                            FilterType="Numbers,LowercaseLetters,UppercaseLetters" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </Content>
                                    </asp:AccordionPane>
                                    <asp:AccordionPane runat="server" ID="AccordionPane5">
                                        <Header>
                                            <span style="margin-left: 12px;">Column Order</span>
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
                                                        <asp:ListItem>Staff Name</asp:ListItem>
                                                        <asp:ListItem>Department</asp:ListItem>
                                                        <asp:ListItem>Designation</asp:ListItem>
                                                        <asp:ListItem>Staff Applied date</asp:ListItem>
                                                        <asp:ListItem>Staff Date of Birth</asp:ListItem>
                                                        <asp:ListItem>Marital Status</asp:ListItem>
                                                        <asp:ListItem>Father Name</asp:ListItem>
                                                        <asp:ListItem>Blood Group</asp:ListItem>
                                                        <asp:ListItem>Staff Mobile No</asp:ListItem>
                                                        <asp:ListItem>Caste</asp:ListItem>
                                                        <asp:ListItem>Religion</asp:ListItem>
                                                        <asp:ListItem>Community</asp:ListItem>
                                                        <asp:ListItem>Staff EMail Id</asp:ListItem>
                                                        <asp:ListItem>Permanent City</asp:ListItem>
                                                        <asp:ListItem>Permanent District</asp:ListItem>
                                                        <asp:ListItem>Permanent State</asp:ListItem>
                                                        <asp:ListItem>Contact City</asp:ListItem>
                                                        <asp:ListItem>Contact District</asp:ListItem>
                                                        <asp:ListItem>Contact State</asp:ListItem>
                                                        <asp:ListItem>Contact Pincode</asp:ListItem>
                                                        <asp:ListItem>Year of Experience</asp:ListItem>
                                                    </asp:CheckBoxList>
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
                    </center>
                    <br />
                    <asp:Label ID="lblnorec" runat="server" Text="There are no records matched" ForeColor="Red"
                        Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <br />
                    <br />
                    <center>
                        <asp:Panel ID="Panelpage" runat="server" Height="37px">
                            <table class="maintablestyle" style="background-color: transparent; border-color: transparent;
                                border-style: none; box-shadow: none;">
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
                        <center>
                            <div>
                                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnexcel" runat="server" Text="Export Excel" OnClick="btnexcel_Click"
                                    OnClientClick="return ExcelClick();" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="true" Visible="false" />
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Visible="false" />
                                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                            </div>
                        </center>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <FarPoint:FpSpread ID="FpSpread1" runat="server" CssClass="stylefp" OnPreRender="FpSpread1_SelectedIndexChanged"
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
                        <br />
                        <br />
                    </center>
                </div>
            </div>
        </body>
        </html>
    </p>
</asp:Content>
