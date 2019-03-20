<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IndStaffUniversalReport.aspx.cs"
    EnableEventValidation="false" Inherits="IndStaffUniversalReport" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="Styles/Commoncss.css" rel="stylesheet" type="text/css" />
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
        .alter
        {
            top: 120;
        }
        .cpBody
        {
            background-color: transparent;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
        .pHeader
        {
            color: white;
            font-size: 11px;
            cursor: pointer;
            padding: 4px;
            font-style: italic;
            font-variant: small-caps;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
            width: 904px;
        }
        .pBody
        {
            background-color: #9DF0E8;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        
        .cpimage
        {
            vertical-align: middle;
            background-color: transparent;
        }
        #lab
        {
            float: right;
            background-color: transparent;
            position: fixed;
        }
        .style5
        {
            width: 289px;
        }
        .style6
        {
            width: 289px;
        }
        .style7
        {
            width: 289px;
        }
        .btstyle
        {
            background-color: transparent;
            font-size: larger;
            font-variant: normal;
            font-family: Arial;
            font-style: normal;
            border-width: 0;
        }
        .btstyle1
        {
            background-color: transparent;
            font-size: larger;
            font-family: Arial Black;
            font-variant: small-caps;
            font-style: normal;
            border-width: 0;
        }
        .style8
        {
            width: 306px;
        }
        BODY
        {
            background-image: url('image/Student/tail-middle.jpg');
            background-repeat: repeat-y;
            background-position: center;
        }
        .accordion
        {
            width: 400px;
        }
        
        .accordionHeader
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #2E4d7B;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionHeaderSelected
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #5078B3;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 6px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionContent
        {
            background-color: White;
            border: 1px dashed #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 10px;
            height: auto;
        }
    </style>
    <style>
        .myButton
        {
            -moz-box-shadow: 3px 4px 0px 0px #057fd0;
            -webkit-box-shadow: 3px 4px 0px 0px #057fd0;
            box-shadow: 3px 4px 0px 0px #057fd0;
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #c62d1f), color-stop(1, #019ad2));
            background: -moz-linear-gradient(top, #33bdef 5%, #019ad2 100%);
            background: -webkit-linear-gradient(top, #33bdef 5%, #019ad2 100%);
            background: -o-linear-gradient(top, #33bdef 5%, #019ad2 100%);
            background: -ms-linear-gradient(top, #33bdef 5%, #019ad2 100%);
            background: linear-gradient(to bottom, #33bdef 5%, #019ad2 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#33bdef', endColorstr='#019ad2',GradientType=0);
            background-color: #33bdef;
            -moz-border-radius: 18px;
            -webkit-border-radius: 18px;
            border-radius: 18px;
            border: 1px solid #057fd0;
            display: inline-block;
            cursor: pointer;
            color: #ffffff;
            font-family: Arial;
            font-size: 17px;
            padding: 7px 25px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #057fd0;
        }
        .myButton:hover
        {
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #019ad2), color-stop(1, #33bdef));
            background: -moz-linear-gradient(top, #019ad2 5%, #33bdef 100%);
            background: -webkit-linear-gradient(top, #019ad2 5%, #33bdef 100%);
            background: -o-linear-gradient(top, #019ad2 5%, #33bdef 100%);
            background: -ms-linear-gradient(top, #019ad2 5%, #33bdef 100%);
            background: linear-gradient(to bottom, #019ad2 5%, #33bdef 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#019ad2', endColorstr='#33bdef',GradientType=0);
            background-color: #019ad2;
        }
        .myButton:active
        {
            position: relative;
            top: 1px;
        }
    </style>
    <style>
        .cpHeader
        {
           -moz-box-shadow: 0px 10px 14px -7px #2a5993;
	        -webkit-box-shadow: 0px 10px 14px -7px #2a5993;
	        box-shadow: 0px 10px 14px -7px #2a5993;
	        background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #0078CA), color-stop(1, #0078CA));
	        background:-moz-linear-gradient(top, #0078CA 5%, #0078CA 100%);
	        background:-webkit-linear-gradient(top, #0078CA 5%, #0078CA 100%);
	        background:-o-linear-gradient(top, #0078CA 5%, #0078CA 100%);
	        background:-ms-linear-gradient(top, #0078CA 5%, #0078CA 100%);
	        background:linear-gradient(to bottom, #0078CA 5%, #0078CA 100%);
	        filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#0078CA', endColorstr='#0078CA',GradientType=0);
	        background-color:#0078CA;;
	        -moz-border-radius:8px;
	        -webkit-border-radius:8px;
	        border-radius:8px;
	        display:inline-block;
	        cursor:pointer;
	        color:#ffffff;
	        font-family:Arial;
	        font-size:20px;
	        font-weight:bold;
	        padding:6px 32px;
	        text-decoration:none;
	        text-shadow:0px 1px 0px #3d768a;
            height: 16px;
            margin: 0px 0px 8px 0px;
        }
        .cpHeader:hover
        {
          background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #2A5993), color-stop(1, #2A5993));
	        background:-moz-linear-gradient(top, #2A5993 5%, #2A5993 100%);
	        background:-webkit-linear-gradient(top, #2A5993 5%, #2A5993 100%);
	        background:-o-linear-gradient(top, #2A5993 5%, #2A5993 100%);
	        background:-ms-linear-gradient(top, #2A5993 5%, #2A5993 100%);
	        background:linear-gradient(to bottom, #2A5993 5%, #2A5993 100%);
	        filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#2A5993', endColorstr='#2A5993',GradientType=0);
	        background-color:#2A5993;
        }
        .cpHeader:active
        {
            position: relative;
            top: 1px;
        }
    </style>
    <style>
        #header_details
        {
            box-shadow: inset 0px 0px 15px 3px #23395e;
            border-radius: 17px;
            border: 1px solid #1f2f47;
            color: #ffffff;
            font-family: Arial;
            font-size: 15px;
            padding: 6px 13px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #263666;
            height: 50px;
        }
    </style>
    <style>
        #header_background
        {
            -moz-box-shadow: inset 0px 1px 0px 0px #bee2f9;
            -webkit-box-shadow: inset 0px 1px 0px 0px #bee2f9;
            box-shadow: inset 0px 1px 0px 0px #bee2f9;
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #0078bd), color-stop(1, #a7d1e8));
            background: -moz-linear-gradient(top, #0078bd 5%, #a7d1e8 100%);
            background: -webkit-linear-gradient(top, #0078bd 5%, #a7d1e8 100%);
            background: -o-linear-gradient(top, #0078bd 5%, #a7d1e8 100%);
            background: -ms-linear-gradient(top, #0078bd 5%, #a7d1e8 100%);
            background: linear-gradient(to bottom, #0078bd 5%, #a7d1e8 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#0078bd', endColorstr='#a7d1e8',GradientType=0);
            background-color: #0078bd;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #3866a3;
            display: inline-block;
            color: #193963;
            font-family: Arial;
            font-size: 28px;
            font-weight: bold;
            padding: 29px 66px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #7cacde;
        }
    </style>
</head>
<body>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            $(".btnS").hover(function () {
                $(this).css("background-color", "pink");
            }, function () {
                $(this).css("background-color", "white");
            });
        });
    </script>
    <script type="text/javascript">

       
    </script>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <center>
        <div id="header_background" style="width: 935px; padding: 10px 20px 20px 20px;">
            <table border="0">
                <tr id="header_details">
                    <td>
                        <center>
                            <asp:Label runat="server" Font-Bold="True" Font-Italic="False" Font-Underline="false"
                                ForeColor="White" Font-Names="Book Antiqua" Font-Size="XX-Large" ID="lblcol"
                                Width="700px"></asp:Label>
                        </center>
                    </td>
                    <td>
                        <div style="height: 22px; width: 180px;">
                            &nbsp;<asp:ImageButton ID="lnkback" runat="server" CausesValidation="False" Height="35px"
                                OnClick="lnkback_Click" Width="35px" ImageUrl="image/Student/backButton.png"
                                ToolTip="Back" />
                            <asp:ImageButton ID="lnkHome" runat="server" Width="35px" Style="height: 35px;" ImageUrl="image/Student/home.png"
                                ToolTip="Home" OnClick="lnkHome_Click" />
                            <asp:ImageButton ID="imgnotification" runat="server" Width="35px" Style="height: 35px;"
                                ImageUrl="image/Student/Notification.png" ToolTip="Notification" />
                            <asp:ImageButton ID="lblogout" runat="server" CausesValidation="False" Height="35px"
                                OnClick="lblogout_Click" Width="35px" ImageUrl="image/Student/logout.png" ToolTip="Logout" />
                        </div>
                    </td>
                </tr>
                <tr style="height: 250px;">
                    <td colspan="2">
                        <center>
                            <table border="0" width="900px">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Style="text-align: left;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="20px" Text="Staff Name"></asp:Label>
                                    </td>
                                    <td style="font-weight: bold; width: 2px;">
                                        :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblstaffname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="20px"></asp:Label>
                                    </td>
                                    <td colspan="3" rowspan="2" align="center">
                                        <asp:Image ID="Imagestudent" runat="server" Height="170px" Width="155px" Visible="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Style="vertical-align: middle;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="20px" Text="Department Name"></asp:Label>
                                    </td>
                                    <td style="font-weight: bold">
                                        :
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldept" runat="server" Style="vertical-align: middle;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="20px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_desig" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="20px" Text="Designaton"></asp:Label>
                                    </td>
                                    <td style="font-weight: bold">
                                        :
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldesig" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="20px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblstf_code" runat="server" Style="vertical-align: middle;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="20px" Text="Staff Code"></asp:Label>
                                    </td>
                                    <td style="font-weight: bold">
                                        :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblstaffcodeprint" runat="server" Style="vertical-align: middle;"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="20px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblstafftype" runat="server" Style="vertical-align: middle;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="20px" Text="Staff Type"></asp:Label>
                                    </td>
                                    <td style="font-weight: bold">
                                        :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblstafftypeprint" runat="server" Style="vertical-align: middle;"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="20px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <center>
        <div id="MainDiv" runat="server" style="top: 20px; width: 976px; position: relative">
            <div>
                <asp:Panel ID="pHeaderpersonal" Visible="true" runat="server" CssClass="cpHeader"
                    Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Personal Details</span>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pBodypersonal" runat="server" Visible="true" CssClass="cpBody">
                    <asp:Button ID="ImageButtonbio" runat="server" Visible="true" Text="Bio Data" OnClick="Buttonbiodata_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtoncontact" runat="server" Visible="true" Text="Contact Details"
                        OnClick="Buttoncontact_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <br />
                    <br />
                    <center>
                        <FarPoint:FpSpread ID="Fpspersonal" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" Height="200" Width="400" OnPreRender="Fpspersonal_SelectedIndexChanged">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <br />
                    <asp:CollapsiblePanelExtender ID="cpePersonal" runat="server" TargetControlID="pBodyPersonal"
                        CollapseControlID="pHeaderPersonal" ExpandControlID="pHeaderPersonal" Collapsed="true"
                        TextLabelID="Labelpersonal" CollapsedSize="0" ImageControlID="Imagepersonal"
                        CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
                    </asp:CollapsiblePanelExtender>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pHeaderattendance" Visible="true" runat="server" CssClass="cpHeader"
                    Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Staff Attendance</span>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pBodyattendance" runat="server" Visible="true" CssClass="cpBody">
                    <asp:Button ID="ImageButtonbiometric" runat="server" Visible="true" Text="Biometric Attendance"
                        OnClick="Buttonbiometricatt_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtongeneral" runat="server" Visible="true" Text="General Attendance"
                        OnClick="Buttongeneralatt_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbldatefrom" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="From" />
                                &nbsp; &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtfromdate" runat="server" Visible="false" Width="100px" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:TextBox>
                            </td>
                            <td>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="txtfromdate"
                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','/'" />
                                <asp:CalendarExtender ID="CalendarExtender22" runat="server" Format="d/MM/yyyy" TargetControlID="txtfromdate">
                                </asp:CalendarExtender>
                                &nbsp;&nbsp; &nbsp;
                                <asp:Label ID="lbltodate" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="To"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                                <asp:TextBox ID="txttodate" runat="server" Visible="false" Width="100px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" TargetControlID="txttodate"
                                    FilterType="Custom" ValidChars="'0','1','2','3','4','5','6','7','8','9','/'" />
                                <asp:CalendarExtender ID="CalendarExtender23" runat="server" Format="d/MM/yyyy" TargetControlID="txttodate">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                                <asp:Button ID="btn_go" runat="server" Visible="false" Text="Go" CssClass="myButton"
                                    OnClick="ButtonGo_Click" />
                                <asp:Button ID="btn_bio_go" runat="server" Visible="false" Text="Go" CssClass="myButton"
                                    OnClick="ButtonBioGo_Click" />
                            </td>
                        </tr>
                    </table>
                   
                    <br />
                    <asp:Label ID="lblError" runat="server" Font-Size="Medium" ForeColor="Red" Font-Bold="True"
                        Font-Names="Book Antiqua" Text="" Visible="false"></asp:Label>
                    <br />
                    
                    <center>
                        <FarPoint:FpSpread ID="Fpsattendence" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" >
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <br />
                </asp:Panel>
                <asp:CollapsiblePanelExtender ID="cpetimetable" runat="server" TargetControlID="pBodyattendance"
                    CollapseControlID="pHeaderattendance" ExpandControlID="pHeaderattendance" Collapsed="true"
                    TextLabelID="Labeltimetable" CollapsedSize="0" ImageControlID="Imagetimetable"
                    CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
            </div>
            <div>
                <asp:Panel ID="pHeaderstaffPerfomance" Visible="true" runat="server" CssClass="cpHeader"
                    Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Staff Perfomance</span>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pBodystaffperfomance" runat="server" Visible="true" CssClass="cpBody">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_FdatePer" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="From" />
                                &nbsp; &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="tbfrom" Width="100" runat="server" Height="19px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="tbfrom" Format="d/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                                &nbsp;&nbsp; &nbsp;
                                <asp:Label ID="lbl_TdatePer" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="To" />
                            </td>
                            <td>
                                &nbsp;
                                <asp:TextBox ID="tbto" Width="100" runat="server" Height="19px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender5" Format="d/MM/yyyy" TargetControlID="tbto"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbletype" Text="Exam Type" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                &nbsp; &nbsp;
                                <asp:DropDownList ID="ddlexam" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="100px">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem Value="1">Internal</asp:ListItem>
                                    <asp:ListItem Value="2">External</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <asp:Label ID="labeldatevalid" runat="server" Visible="false"  ForeColor="Red"></asp:Label>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <asp:Button ID="ImageButtonperfomance" runat="server" Visible="true" Text="Perfomance"
                        OnClick="Buttonperfomance_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <br />
                    <br />
                    <br />
                    <center>
                        <FarPoint:FpSpread ID="Fpspermomance" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="200" Visible="false">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <br />
                    <br />
                </asp:Panel>
                <asp:CollapsiblePanelExtender ID="cpeAttendence" runat="server" TargetControlID="pBodystaffperfomance"
                    CollapseControlID="pHeaderstaffPerfomance" ExpandControlID="pHeaderstaffPerfomance"
                    Collapsed="true" TextLabelID="LabelAttendence" CollapsedSize="0" ImageControlID="ImageAttendence"
                    CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
            </div>
        </div>
    </center>
    </form>
</body>
</html>
