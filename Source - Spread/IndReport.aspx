<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IndReport.aspx.cs" EnableEventValidation="false"
    Inherits="IndReport" %>

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
<%--<body background="image/Student/Top.jpg" >--%>
<%--background="image/Student/Top.jpg"--%>
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

        function display() {
            document.getElementById('MainContent_errmsg').innerHTML = "";


        }
        function make_blank() {
            document.form1.type.value = "";

        }

        function reason(id) {
            var value = id.value;
            if (value.trim().toUpperCase() == "OTHERS") {
                var idval = document.getElementById("<%=txt_ddlgatepassreson.ClientID %>");
                idval.style.display = "block";
            }
            else {
                var idval = document.getElementById("<%=txt_ddlgatepassreson.ClientID %>");
                idval.style.display = "none";
            }
        }

    </script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=divpersonal.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=400');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
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
                            <%--     &nbsp;<asp:LinkButton ID="lnkback" runat="server" CausesValidation="False" Height="19px"
                                Visible="true" OnClick="lnkback_Click" Width="40px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Small" ForeColor="#0000CC">Back</asp:LinkButton>--%>
                            <%--&nbsp;<asp:LinkButton ID="lnkHome" runat="server" CausesValidation="False" Height="19px"
                                OnClick="lnkHome_Click" Width="40px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Small" ForeColor="#0000CC">Home</asp:LinkButton>--%>
                            <%-- &nbsp;<asp:LinkButton ID="lblogout" runat="server" CausesValidation="False" Height="19px"
                            OnClick="lblogout_Click" Width="40px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Small" ForeColor="#0000CC">Logout</asp:LinkButton>--%>
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
                    <td>
                        <center>
                            <table border="0" width="700px" style="height: 200px;">
                                <%--width="700px" style="height: 200px;"--%>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Style="text-align: left;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="20px" Text="Student Name"></asp:Label>
                                    </td>
                                    <td style="font-weight: bold; width: 2px;">
                                        :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="20px"></asp:Label>
                                    </td>
                                    <%--   <td colspan="3" rowspan="2" align="center">
                                        <asp:Image ID="Imagestudent" runat="server" ImageUrl="~/image/Student/606.jpg" Height="70px"
                                            Width="65px" Visible="true" />
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRegNo" runat="server" Style="text-align: left;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="20px" Text="RollNo"></asp:Label>
                                    </td>
                                    <td style="font-weight: bold; width: 2px;">
                                        :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRegText" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="20px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Style="vertical-align: middle;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="20px" Text="Branch Name"></asp:Label>
                                    </td>
                                    <td style="font-weight: bold">
                                        :
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldegree" runat="server" Style="vertical-align: middle;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="20px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_semOrTerm" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="20px" Text="Semester"></asp:Label>
                                    </td>
                                    <td style="font-weight: bold">
                                        :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="20px"></asp:Label>
                                    </td>
                                    <td rowspan="2" align="right">
                                        <asp:Image ID="fatherphoto" runat="server" ImageUrl="~/image/Student/606.jpg" Height="70px"
                                            Width="65px" Visible="false" />
                                    </td>
                                    <td rowspan="2" align="center">
                                        <asp:Image ID="motherphoto" runat="server" ImageUrl="~/image/Student/606.jpg" Height="70px"
                                            Width="65px" Visible="false" />
                                    </td>
                                    <td rowspan="2" align="left">
                                        <asp:Image ID="gaurdianphoto" runat="server" ImageUrl="~/image/Student/606.jpg" Height="70px"
                                            Width="65px" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Style="vertical-align: middle;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="20px" Text="Batch Year"></asp:Label>
                                    </td>
                                    <td style="font-weight: bold">
                                        :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblyear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="20px"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="font-size: 10px; font-style: normal; font-names: Book Antiqua;">
                                    <td colspan="3">
                                    </td>
                                    <td align="right" style="padding-right: 20px">
                                        <asp:Label runat="server" ID="lbl_fatherPhoto" Visible="false"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label runat="server" ID="lbl_motherPhoto" Visible="false"></asp:Label>
                                    </td>
                                    <td align="left" style="padding-left: 10px">
                                        <asp:Label runat="server" ID="lbl_guardPhoto" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </td>
                    <td rowspan="2" style="width: 100px">
                        <asp:Image ID="Imagestudent" runat="server" ImageUrl="~/image/Student/606.jpg" Height="200px"
                            Width="150px" Visible="true" />
                    </td>
                    <%--<td rowspan="2" style="width: 100px">
                        <%--style="height: 185px; width: 156px;"
                        <br />
                       
                    </td>--%>
                </tr>
            </table>
        </div>
    </center>
    <%--   <table id="tblStudParents" runat="server" style="height: 150px; width: 460px;">
        <tr style="height: 150px; width: 460px;">
            <td>
                <asp:Image ID="fatherphoto" runat="server" Width="150px" Height="150px" />
            </td>
            <td>
                <asp:Image ID="motherphoto" runat="server" Width="150px" Height="150px" />
            </td>
            <td>
                <asp:Image ID="gaurdianphoto" runat="server" Width="150" Height="150px" />
            </td>
        </tr>
    </table>--%>
    <center>
        <div id="MainDiv" runat="server" style="top: 20px; width: 976px; position: relative">
            <%--Personal Details--%>
            <div>
                <asp:Panel ID="pHeaderpersonal" Visible="false" runat="server" CssClass="cpHeader"
                    Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Personal Details</span>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pBodypersonal" runat="server" Visible="false" CssClass="cpBody">
                    <%--style=" background-color: Yellow;"--%>
                    <asp:Button ID="ImageButtonbio" runat="server" Visible="false" Text="Bio Data" OnClick="Buttonbiodata_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtoncontact" runat="server" Visible="false" Text="Contact Details"
                        OnClick="Buttoncontact_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtoncertificate" runat="server" Visible="false" Text="Certificate Details"
                        OnClick="Buttoncertificate_Click" CssClass="myButton" />
                    <br />
                    <br />
                    <center>
                        <div id="divpersonal" runat="server">
                            <FarPoint:FpSpread ID="Fpspersonal" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Height="200" Width="400" OnPreRender="Fpspersonal_SelectedIndexChanged">
                              <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <asp:Button ID="btnPrint" runat="server" Visible="false" Text="Direct Print" OnClientClick="return PrintPanel();"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                        </div>
                    </center>
                    <br />
                    <asp:CollapsiblePanelExtender ID="cpePersonal" runat="server" TargetControlID="pBodyPersonal"
                        CollapseControlID="pHeaderPersonal" ExpandControlID="pHeaderPersonal" Collapsed="true"
                        TextLabelID="Labelpersonal" CollapsedSize="0" ImageControlID="Imagepersonal"
                        CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
                    </asp:CollapsiblePanelExtender>
                </asp:Panel>
            </div>
            <%--Time Table--%>
            <div>
                <asp:Panel ID="pHeadertimetable" Visible="false" runat="server" CssClass="cpHeader"
                    Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Time Table</span>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pBodytimetable" runat="server" Visible="false" CssClass="cpBody">
                    <asp:Button ID="ImageButtontodaytt" runat="server" Visible="false" Text="Today" OnClick="Buttontoday_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonsemtt" runat="server" Visible="false" Text="" OnClick="Buttonsem_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtoncamtt" runat="server" Visible="false" Text="CAM" OnClick="ButtonCAM_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonextt" runat="server" Visible="false" Text="External" OnClick="Buttonsemex_Click"
                        CssClass="myButton" />
                    <br />
                    <br />
                    <center>
                        <FarPoint:FpSpread ID="Fpstimetable" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="200" Width="400">
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
                <asp:CollapsiblePanelExtender ID="cpetimetable" runat="server" TargetControlID="pBodytimetable"
                    CollapseControlID="pHeadertimetable" ExpandControlID="pHeadertimetable" Collapsed="true"
                    TextLabelID="Labeltimetable" CollapsedSize="0" ImageControlID="Imagetimetable"
                    CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
            </div>
            <%--Marks--%>
            <div>
                <asp:Panel ID="pHeadermarks" Visible="false" runat="server" CssClass="cpHeader" Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Marks Details</span>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pBodymarks" runat="server" Visible="false" CssClass="cpBody">
                    <asp:Button ID="ImageButtonsubject" runat="server" Visible="false" Text="Subject"
                        OnClick="ButtonSubjects_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtoncam" runat="server" Visible="false" Text="CAM" OnClick="ButtonCAMmark_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButton1" runat="server" Visible="false" Text="Marks" OnClick="Buttonmarks_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonintm" runat="server" Visible="false" Text="Internal" OnClick="Buttoninmark_Click"
                        CssClass="myButton" />
                    <asp:Button ID="ImageButtonExm" runat="server" Visible="false" Text="External" OnClick="Buttonextmark_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonArrear" runat="server" Visible="false" Text="Arrear Papers"
                        OnClick="Buttonapaper_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonCamv" runat="server" Visible="false" Text="CAM" OnClick="Buttoncamreport_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnsubjectchooser" runat="server" Visible="false" Text="CAM" OnClick="btnsubjectchooser_Click"
                        CssClass="myButton" />
                    <br />
                    <br />
                    <div id="divMarks" runat="server">
                        <table>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblsemester" runat="server" Text="" Visible="false" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                    &nbsp; &nbsp;
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" Visible="false">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_vech" runat="server" CssClass="font" Width="122px" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium">--Select--</asp:TextBox>
                                            <asp:Panel ID="vehiclpan" runat="server" CssClass="MultipleSelectionDDL" Style="font-family: 'Book Antiqua';
                                                position: absolute;" Font-Bold="True" Font-Names="Book Antiqua" Height="230px"
                                                Width="124px" BackColor="AliceBlue">
                                                <asp:CheckBox ID="vehiclecheck" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="vehiclecheck_checkedchanged" />
                                                <asp:CheckBoxList ID="vehiclechecklist" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="vehiclechecklist_selectedchanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_vech"
                                                PopupControlID="vehiclpan" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlExamType" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Visible="false">
                                        <asp:ListItem>All</asp:ListItem>
                                        <asp:ListItem Value="0">CAM</asp:ListItem>
                                        <asp:ListItem Value="1">University</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button ID="btnmarkgo" runat="server" Text="Go" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" OnClick="btnmarkgo_Click" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <br />
                    <br />
                    <center>
                        <FarPoint:FpSpread ID="Fpsmarks" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" OnCellClick="Fpsmarks_CellClick" OnPreRender="Fpsmarks_PreRender"
                            Height="200">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnxl_Click" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Visible="false" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        <br />
                        <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </center>
                    <br />
                    <asp:Label ID="lblnorec" runat="server" Font-Bold="true" ForeColor="Red" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false"></asp:Label>
                    <br />
                    <asp:Button ID="btnsubjectchoosesave" runat="server" Font-Bold="true" Text="Save"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnsubjectchoosesave_Click" />
                    <%--Style="left: 820px; position: absolute;"--%>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblqa" Style="left: 10px; position: absolute;" runat="server" Font-Bold="true"
                                    Text="Type" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txttype" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                            Width="100px" Style="left: 55px; position: absolute; font-family: 'Book Antiqua';"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="ptype" runat="server" BackColor="AliceBlue">
                                            <asp:CheckBox ID="chktype" runat="server" Font-Bold="True" OnCheckedChanged="chktype_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklstype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstype_SelectedIndexChanged">
                                                <asp:ListItem Text="Questions"></asp:ListItem>
                                                <asp:ListItem Text="Answers"></asp:ListItem>
                                                <asp:ListItem Text="Attachements"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txttype"
                                            PopupControlID="ptype" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblmarks" Style="left: 185px; position: absolute;" runat="server"
                                    Font-Bold="true" Text="Marks" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlmarks" Style="left: 250px; position: absolute;" runat="server"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <br />
                    <FarPoint:FpSpread ID="FpQuestions" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="600" OnCellClick="FpQuestions_CellClick" FOnt_Bold="false"
                        Font_Name="Book Antiqua" OnPreRender="FpQuestions_PreRender">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <FarPoint:FpSpread ID="Fpquestionbank" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="600" Width="800" OnCellClick="Fpquestionbank_CellClick"
                        OnPreRender="Fpquestionbank_PreRender">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <asp:TreeView ID="subjecttree" runat="server" ShowCheckBoxes="Leaf" ExpandDepth="0"
                        ShowLines="true" ShowExpandCollapse="true" Visible="false">
                    </asp:TreeView>
                </asp:Panel>
                <%-- </center> </asp:Panel>--%>
                <asp:CollapsiblePanelExtender ID="cpemarks" runat="server" TargetControlID="pBodymarks"
                    CollapseControlID="pHeadermarks" ExpandControlID="pHeadermarks" Collapsed="true"
                    TextLabelID="Labelmarks" CollapsedSize="0" ImageControlID="Imagemarks" CollapsedImage="right.jpeg"
                    ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
            </div>
            <%--Lesson Status--%>
            <div>
                <asp:Panel ID="pHeaderlessonstatus" Visible="false" runat="server" CssClass="cpHeader"
                    Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Lesson Status Details</span>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pBodylessonstatus" runat="server" Visible="false" CssClass="cpBody">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="From" />
                                &nbsp; &nbsp;
                                <%--<asp:Image ID="Image1" Height="19px" runat="server" ImageUrl="~/image/Student/From.jpg" />--%>
                            </td>
                            <td>
                                <asp:TextBox ID="tbfrom" Width="100" runat="server" Height="19px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="tbfrom" Format="d-MM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                                &nbsp;&nbsp; &nbsp;
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="To" />
                                <%--  <asp:Image ID="Image2" runat="server" Height="19px" ImageUrl="~/image/Student/To.jpg" />--%>
                            </td>
                            <td>
                                &nbsp;
                                <asp:TextBox ID="tbto" Width="100" runat="server" Height="19px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender5" Format="d-MM-yyyy" TargetControlID="tbto"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <asp:Label ID="labeldatevalid" runat="server" Visible="false" Text="From " ForeColor="Red"></asp:Label>
                        </tr>
                    </table>
                    <br />
                    <asp:Button ID="ImageButtonconduct" runat="server" Visible="false" Text="Conducted"
                        OnClick="Buttonconduct_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonmis" runat="server" Visible="false" Text="Missed" OnClick="Buttonmis_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonyet" runat="server" Visible="false" Text="Yet" OnClick="Buttonyet_Click"
                        CssClass="myButton" />
                    <br />
                    <center>
                        <FarPoint:FpSpread ID="Fpslesson" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="200" Width="400">
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
                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="pBodylessonstatus"
                    CollapseControlID="pHeaderlessonstatus" ExpandControlID="pHeaderlessonstatus"
                    Collapsed="true" TextLabelID="Labellessonstatus" CollapsedSize="0" ImageControlID="Imagelessonstatus"
                    CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
            </div>
            <%--Attendance Details--%>
            <div>
                <asp:Panel ID="pHeaderAttendence" Visible="false" runat="server" CssClass="cpHeader"
                    Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Attendance Details</span>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pBodyAttendence" runat="server" Visible="false" CssClass="cpBody">
                    <asp:Button ID="ImageButtontodaya" runat="server" Visible="false" Text="Today" OnClick="Buttontodaya_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonweeka" runat="server" Visible="false" Text="Weekly" OnClick="Buttonweeka_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonsema" runat="server" Visible="false" Text="Semester" OnClick="Buttonsema_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonsemdate" runat="server" Visible="false" Text="Semester Date"
                        OnClick="Buttonsemdate_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonevents" runat="server" Visible="false" Text="Events" OnClick="Buttonevents_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonexamseat" runat="server" Visible="false" Text="Exam Date"
                        OnClick="Buttonexamseat_Click" CssClass="myButton" />
                    <br />
                    <br />
                    <br />
                    <%--  <center>--%>
                    <FarPoint:FpSpread ID="Fpsattendence" runat="server" BorderColor="Black" BorderStyle="Solid"
                        OnPreRender="Fpsattendence_SelectedIndexChanged" OnCellClick="Fpsattendence_CellClick"
                        BorderWidth="1px" Height="200" Visible="false">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <%-- </center>--%>
                    <br />
                    <br />
                    <FarPoint:FpSpread ID="Fpsematen" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="200" Width="400" Visible="false">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </asp:Panel>
                <asp:CollapsiblePanelExtender ID="cpeAttendence" runat="server" TargetControlID="pBodyAttendence"
                    CollapseControlID="pHeaderAttendence" ExpandControlID="pHeaderAttendence" Collapsed="true"
                    TextLabelID="LabelAttendence" CollapsedSize="0" ImageControlID="ImageAttendence"
                    CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
            </div>
            <%--Library Details--%>
            <div>
                <asp:Panel ID="pHeaderLibrary" Visible="false" runat="server" CssClass="cpHeader"
                    Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Library Details</span>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pBodyLibrary" runat="server" Visible="false" CssClass="cpBody">
                    <%--<asp:Button ID="ImageButtonopac" runat="server" Visible="false" Text="OPAC" OnClick="Buttonopac_Click"
                        CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;--%>
                    <asp:Button ID="ImageButtonlibcards" runat="server" Visible="false" Text="Library Cards"
                        OnClick="Buttonlibcards_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonbukt" runat="server" Visible="false" Text="Books Taken"
                        OnClick="Buttonbukt_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonremind" runat="server" Visible="false" Text="Reminder"
                        OnClick="Buttonremind_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonbukr" runat="server" Visible="false" Text="Books Return"
                        OnClick="Buttonbukr_Click" CssClass="myButton" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ImageButtonfine" runat="server" Visible="false" Text="Fine" OnClick="Buttonfine_Click"
                        CssClass="myButton" />
                    <br />
                    <br />
                    <br />
                    <center>
                        <FarPoint:FpSpread ID="Fpslibrary" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="200" Width="400">
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
                <asp:CollapsiblePanelExtender ID="cpeLibrary" runat="server" TargetControlID="pBodyLibrary"
                    CollapseControlID="pHeaderLibrary" ExpandControlID="pHeaderLibrary" Collapsed="true"
                    TextLabelID="LabelLibrary" CollapsedSize="0" ImageControlID="ImageLibrary" CollapsedImage="right.jpeg"
                    ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
            </div>
            <%--General Details--%>
            <div>
                <asp:Panel ID="pHeaderGeneral" Visible="false" runat="server" CssClass="cpHeader"
                    Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        General Details</span>
                </asp:Panel>
            </div>
            <asp:Panel ID="pBodyGeneral" runat="server" Visible="false" CssClass="cpBody">
                <asp:Button ID="ImageButtonfees" runat="server" Visible="false" Text="Fees" OnClick="Buttonfees_Click"
                    CssClass="myButton" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="ImageButtontransport" runat="server" Visible="false" Text="Transport"
                    OnClick="Buttontransport_Click" CssClass="myButton" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="ImageButtonfeedback" runat="server" Visible="false" Text="Feedback"
                    CssClass="myButton" OnClick="ImageButtonfeedback_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="ImageButton3" runat="server" Visible="false" Text="Notification"
                    OnClick="btnnotification_Click" CssClass="myButton" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="ImageButtonHostel" runat="server" Visible="false" Text="Hostel" OnClick="btnHostel_Click"
                    CssClass="myButton" />
                <br />
                <br />
                <center>
                    <asp:TextBox ID="feedtxt" Visible="false" runat="server" Height="138px" TextMode="MultiLine"
                        Width="828px"></asp:TextBox>
                    <%-- <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                    <ContentTemplate>
                    --%>
                    <div id="questiondiv" runat="server" visible="false">
                        <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                            <ContentTemplate>
                                <div id="feedback">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="App_no" runat="server" Visible="false" Width="200" Text=""></asp:Label>
                                                <asp:RadioButton ID="rb_Acad" Width="98px" runat="server" GroupName="same2" Text="Academic"
                                                    OnCheckedChanged="rb_Acad_CheckedChanged" AutoPostBack="true" Checked="true">
                                                </asp:RadioButton>
                                                <asp:RadioButton ID="rb_Gend" runat="server" Width="100px" GroupName="same2" Text="General"
                                                    OnCheckedChanged="rb_Gend_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <div>
                                    <center>
                                        <FarPoint:FpSpread ID="FpSpread4" runat="server" Visible="true" BorderStyle="Solid"
                                            BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false" OnCellClick="FpSpread4_OnCellClick"
                                            OnPreRender="FpSpread4_Selectedindexchange">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                        <br />
                                    </center>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lbl_Name" runat="server" Width="400px" ForeColor="green" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text=""></asp:Label>
                                    <center>
                                        <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="true" BorderStyle="Solid"
                                            BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false" OnButtonCommand="FpSpread2_OnButtonCommand">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </center>
                                </div>
                                <br />
                                <asp:Button ID="btn_save_fb" runat="server" Visible="true" Width="85px" Height="34px"
                                    CssClass="textbox textbox1" Text="Save" OnClick="btn_save_fb_Click" /><br />
                                <div id="rptprint1" runat="server" visible="false">
                                    <br />
                                    <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                    <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                        Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                                        Height="35px" CssClass="textbox textbox1" />
                                    <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                                        CssClass="textbox textbox1" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                                </div>
                                <asp:Button ID="btn_exit" runat="server" Visible="false" Width="68px" Height="32px"
                                    CssClass="textbox textbox1" Text="Exit" OnClick="btn_exit_Click" /><br />
                                <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                    left: 0px;">
                                    <center>
                                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 180px;
                                            border-radius: 10px;">
                                            <center>
                                                <table style="height: 100px; width: 100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lbl_alert1" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <center>
                                                                <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                    width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                                            </center>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </center>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <br />
                    <asp:Button ID="feedbtn" runat="server" Visible="false" Text="Submit" OnClick="feedbtn_Click" />
                    <br />
                    <%--<asp:RadioButtonList ID="radScrType" runat="server" Visible="false" OnSelectedIndexChanged="radScrType_changed"
                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" RepeatColumns="2"
                        AutoPostBack="true">
                        <asp:ListItem Selected="True" Text="Selection" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Report" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>--%>
                    <div id="btnsTrans" runat="server" visible="false" style="float: left;">
                        <asp:Button ID="btnSelect" runat="server" Text="Selection" CssClass="textbox btn2 btnS"
                            Width="100px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnSelect_Click" />
                        <asp:Button ID="btnReport" runat="server" Text="Report" CssClass="textbox btn2 btnS"
                            Width="100px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnReport_Click" />
                    </div>
                    <br />
                    <br />
                    <br />
                    <center>
                        <table id="tblSelScr" runat="server" visible="false">
                            <tr>
                                <td>
                                    <asp:Label ID="lblStageName" runat="server" Text="Select Stage" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStageScr" runat="server" CssClass="textbox1 ddlheight4">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="textbox1 btn2" BackColor="LightGreen"
                                        OnClick="btnAdd_Click" />
                                </td>
                                <td>
                                    <asp:Label ID="lblStgErr" runat="server" Visible="false" Text="Please Select Stage Name!"
                                        ForeColor="Red" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <asp:Label ID="feedlbl" runat="server" ForeColor="Green"></asp:Label>
                    <FarPoint:FpSpread ID="Fpsgeneral" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="200" Style="width: auto;" OnCellClick="Fpsgeneral_CellClick"
                        OnPreRender="Fpsgeneral_PreRender" ShowHeaderSelection="false">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <asp:Label ID="lblfeeerr" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <%--   <div id="divfee" runat="server"  style="overflow:auto;">--%>
                    <FarPoint:FpSpread ID="Fpspreadfee" Visible="false" runat="server" BorderColor="Black"
                        BorderStyle="Solid" BorderWidth="1px" Style="overflow: auto; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                        OnCellClick="Fpspreadfee_CellClick" OnPreRender="Fpspreadfee_PreRender" ShowHeaderSelection="false">
                        <%--  <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>--%>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <%-- </div>--%>
                    <asp:Label ID="lblfeestatus" runat="server" Text="" Visible="True" Font-Bold="True"
                        ForeColor="#3333FF" Font-Size="X-Large"></asp:Label>
                    <div id="divHostelInfo" runat="server" visible="false">
                        <FarPoint:FpSpread ID="FpHostel" runat="server" Visible="true" BorderStyle="Solid"
                            BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </center>
                <br />
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="cpeGeneral" runat="server" TargetControlID="pBodyGeneral"
                CollapseControlID="pHeaderGeneral" ExpandControlID="pHeaderGeneral" Collapsed="true"
                TextLabelID="LabelGeneral" CollapsedSize="0" ImageControlID="ImageGeneral" CollapsedImage="right.jpeg"
                ExpandedImage="down.jpeg">
            </asp:CollapsiblePanelExtender>
            <%--Online Examination--%>
            <div>
                <asp:Panel ID="Panel2" Visible="false" runat="server" CssClass="cpHeader" Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Online Examination</span>
                </asp:Panel>
            </div>
            <asp:Panel ID="Panel3" runat="server" Visible="false" CssClass="cpBody">
                <%--      <asp:Button ID="ImageButton2" runat="server" Text="Online Exam" OnClick="ImageButton2_Click"
                    CssClass="myButton" />--%>
                <asp:ImageButton ID="ImageButton2" runat="server" CssClass="btstyle" ImageUrl="~/image/student/Online Exam.jpg"
                    Height="70px" Width="180px" OnClick="ImageButton2_Click" />
                <asp:Panel ID="pgeneral" runat="server" Visible="True" Width="950px" BorderColor="#3867a3"
                    BackImageUrl="~\StudentImage\Box-1.jpg" BorderWidth="2px">
                    <table align="center">
                        <tr style="color: #1f2f47">
                            <td>
                                <asp:CheckBox ID="rdo_random" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                    Font-Size="Medium" Text="Random" OnCheckedChanged="rdo_random_CheckedChanged"
                                    AutoPostBack="true" />
                            </td>
                            <td style="width: 10px">
                            </td>
                            <td>
                                <table style="border: 1px solid black;">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rbo_subject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Subject" GroupName="a" OnCheckedChanged="rbo_subject_CheckedChanged"
                                                AutoPostBack="true" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbo_general" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="General" GroupName="a" AutoPostBack="True" OnCheckedChanged="rbo_general_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pradio" runat="server" Visible="False" Width="950px" BorderColor="#3867a3"
                    BorderWidth="2px" BackImageUrl="~\StudentImage\Box-1.jpg">
                    <table align="center">
                        <tr>
                            <td>
                                <asp:RadioButton ID="rdo_not_take" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Show Not Taken" GroupName="s" OnCheckedChanged="rdo_not_take_CheckedChanged"
                                    AutoPostBack="true" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdo_take" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Show Taken" GroupName="s" AutoPostBack="True" OnCheckedChanged="rdo_take_CheckedChanged" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdo_both" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Both" GroupName="s" AutoPostBack="True" Checked="True"
                                    OnCheckedChanged="rdo_both_CheckedChanged" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <br />
                <asp:Panel ID="ptest" runat="server" BorderColor="#3867a3" BorderWidth="2px" Width="950"
                    BackImageUrl="~\StudentImage\Box-1.jpg">
                    <center>
                        <FarPoint:FpSpread ID="fptest" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="200" Width="400" OnPreRender="fptest_SelectedIndexChanged"
                            OnCellClick="fptest_CellClick">
                            <CommandBar BackColor="Control" ButtonType="PushButton" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <center>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblwrong" runat="server" Text="Exam Already Taken" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red" Visible="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:UpdatePanel ID="updatepanel5" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pquestion" runat="server" BorderColor="Black" BorderWidth="2px" Width="950"
                                    BackImageUrl="~\StudentImage\Box-1.jpg">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblsno" runat="server" Text="S.NO:" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td bgcolor="White">
                                                <asp:Label ID="lbl_no_of_question" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" BackColor="White"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldisplaypatrn" runat="server" Font-Bold="True" Font-Size="Medium"
                                                    BackColor="White" ForeColor="Green" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblquestion" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Question:"></asp:Label>
                                            </td>
                                            <td bgcolor="White">
                                                <asp:Label ID="lblquset" runat="server" Font-Bold="True" Font-Size="Medium" BackColor="White"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblanswer" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Answer:"></asp:Label>
                                            </td>
                                            <td>
                                                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="1px" Height="200" Width="400" OnUpdateCommand="FpSpread1_UpdateCommand">
                                                    <CommandBar BackColor="Control" ButtonType="PushButton" Visible="false" ButtonHighlightColor="ControlLightLight"
                                                        ButtonShadowColor="ControlDark">
                                                    </CommandBar>
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                            </td>
                                        </tr>
                                    </table>
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btn_previous" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnClick="btn_previous_Click" Text="Previous" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_next" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnClick="btn_next_Click" Text="Next" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_save" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnClick="btn_save_Click" Text="Save" Enabled="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </center>
                </asp:Panel>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="Panel3"
                CollapseControlID="Panel2" ExpandControlID="Panel2" Collapsed="true" TextLabelID="LabelGeneral"
                CollapsedSize="0" ImageControlID="ImageGeneral" CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
            </asp:CollapsiblePanelExtender>
            <%--Student Conduct--%>
            <asp:Panel ID="Panel4" Visible="false" runat="server" CssClass="cpHeader" Style="width: 910px;">
                <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                    Student Conduct</span>
            </asp:Panel>
            <asp:Panel ID="pnl_student_conduct" Visible="false" runat="server">
                <center>
                    <FarPoint:FpSpread ID="Fp_student_conduct" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="200" Width="400">
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
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="pnl_student_conduct"
                CollapseControlID="Panel4" ExpandControlID="Panel4" Collapsed="true" TextLabelID="LabelGeneral"
                CollapsedSize="0" ImageControlID="ImageGeneral" CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
            </asp:CollapsiblePanelExtender>
            <%--    </asp:UpdatePanel> </center> </asp:Panel>--%>
            <%--*****************--%>
            <%--Questions--%>
            <div>
                <asp:Panel ID="Panelquestion" Visible="false" runat="server" CssClass="cpHeader"
                    Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Questions</span>
                </asp:Panel>
                <asp:Panel ID="Panel5" Visible="false" runat="server">
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblsubj" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Subjects" />
                            </td>
                            <td>
                                &nbsp; &nbsp; &nbsp;
                                <asp:DropDownList ID="ddlsubjects" runat="server" AutoPostBack="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlsubjects_SelectedIndexChanged"
                                    Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp; &nbsp; &nbsp;&nbsp;
                                <asp:Label runat="server" ID="lblcat" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Question category" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlcat" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    AutoPostBack="true" Font-Size="Medium" Width="80px" OnSelectedIndexChanged="ddlcat_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Panel runat="server" ID="treepanel" ScrollBars="Both" BackImageUrl="~/image/Student/Box.jpg"
                                    Visible="false">
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TreeView ID="TreeView2" runat="server" ForeColor="Black" Font-Size="Medium"
                                        SelectedNodeStyle-ForeColor="Red" Height="216px" Width="335px" OnSelectedNodeChanged="TreeView2_SelectedNodeChanged">
                                        <HoverNodeStyle ForeColor="Red" />
                                        <Nodes>
                                            <asp:TreeNode Expanded="True" SelectAction="Expand"></asp:TreeNode>
                                        </Nodes>
                                    </asp:TreeView>
                                </asp:Panel>
                            </td>
                            <td>
                                <FarPoint:FpSpread ID="spread_question" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    Visible="false" BorderWidth="1px" Height="600" Width="300" HorizontalScrollBarPolicy="AsNeeded"
                                    VerticalScrollBarPolicy="AsNeeded">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonType="PushButton" ButtonShadowColor="ControlDark">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName=" " AutoPostBack="true">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                        <%--------------------------------------18/6/12 PRABHA-----------------------------------------------%>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    OnPreRender="fpspread3prerender" OnCellClick="fpspread3_click" BorderWidth="1px"
                                    Height="200" Width="540" HorizontalScrollBarPolicy="Never" VerticalScrollBarPolicy="Never">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark" ButtonType="PushButton">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                        <%-------------------------------------%>
                    </table>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <br />
                    <br />
                    <center>
                    </center>
                </asp:Panel>
                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="Panel5"
                    CollapseControlID="Panelquestion" ExpandControlID="Panelquestion" Collapsed="true"
                    TextLabelID="LabelGeneral" CollapsedSize="0" ImageControlID="ImageGeneral" CollapsedImage="right.jpeg"
                    ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
                <%--***********************--%>
            </div>
            <%--Charts--%>
            <div id="chartctr" runat="server">
                <asp:Panel ID="Panel6" Visible="false" runat="server" CssClass="cpHeader" Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Charts</span>
                </asp:Panel>
                <asp:Panel ID="Panel7" Visible="false" runat="server">
                    <%--<asp:UpdatePanel ID="UPD" runat="server">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UPD">
                                <ProgressTemplate>
                                    <div class="CenterPB" style="height: 40px; width: 40px;">
                                        <img src="images/progress2.gif" height="180px" width="180px" />
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                                PopupControlID="UpdateProgress1">
                            </asp:ModalPopupExtender>--%>
                    <asp:Panel ID="pnlchart" runat="server" CssClass="cpBody">
                        <asp:Button ID="imgbutton" runat="server" Visible="false" Text="Charts" OnClick="imgbutton_Click"
                            CssClass="myButton" />
                    </asp:Panel>
                    <table>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel8" runat="server">
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Accordion ID="accordace" CssClass="accordion" HeaderCssClass="accordionHeader"
                                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                    runat="server" SelectedIndex="0" FadeTransitions="true" SuppressHeaderPostbacks="true"
                                    TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None"
                                    Style="border-color: #FFFFFF; height: auto; margin-left: 1px; margin-right: 50px;
                                    margin-top: -5px; overflow: auto; width: 910px;" Height="59px">
                                </asp:Accordion>
                            </td>
                        </tr>
                    </table>
                    <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <br />
                </asp:Panel>
                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" runat="server" TargetControlID="Panel7"
                    CollapseControlID="Panel6" ExpandControlID="Panel6" Collapsed="true" TextLabelID="LabelGeneral"
                    CollapsedSize="0" ImageControlID="ImageGeneral" CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
            </div>
            <%--------------------------------------------------- added by prabhaKaran D 16/10/2017 -------------------------------------------------------------%>
            <%-- <div>               
                <asp:Panel ID="Panel13" Visible="false" runat="server" CssClass="cpHeader" Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Revaluation Application</span>
                </asp:Panel>
                <asp:Panel ID="Panel14" Visible="true" runat="server">
                    <asp:Panel ID="Panel15" runat="server" CssClass="cpBody">
                        <asp:Button ID="btnXeroxApplication" runat="server" Visible="true" Text="Xerox Application"
                            OnClick="btnXeroxApplication_OnClick" CssClass="myButton" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnRevaluationApplication" runat="server" Visible="true" Text="Revaluation Application"
                            OnClick="btnRevaluationApplication_OnClick" CssClass="myButton" />
                    </asp:Panel>
                    <asp:Panel runat="server">
                        <div id="divgridXeroxApplication" visible="false" runat="server">
                            <asp:GridView ID="gvdReval" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
                                OnDataBound="gvdReval_OnDataBinding">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>' Visible="true"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgridsubjectcode" runat="server" Text='<%# Bind("subject_code") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgridsubjectname" runat="server" Text='<%# Bind("subject_name") %>'></asp:Label>
                                            <asp:Label ID="lblgridSubjectNo" runat="server" Visible="false" Text='<%# Bind("subject_no") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Semester">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgridsem" runat="server" Text='<%# Bind("semester") %>'></asp:Label>
                                            <asp:Label ID="lblgridExamCode" Visible="false" runat="server" Text='<%# Bind("exam_code") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="gridcb" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#CCCC99" />
                                <HeaderStyle BackColor="#00008B" Font-Bold="True" ForeColor="White" />
                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                <SortedAscendingHeaderStyle BackColor="#848384" />
                                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                <SortedDescendingHeaderStyle BackColor="#575357" />
                            </asp:GridView>
                            <asp:Button ID="btnXeroxSubmit" runat="server" Visible="false" Enabled="false" Text="Submit"
                                OnClick="btnXeroxSubmit_OnClick" />
                           
                            <asp:Label ID="lblAlertReval" runat="server" Visible="false"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server">
                        <div id="divRevaluationApplication" runat="server" visible="false">
                            <asp:GridView ID="grdRevalApply" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
                                OnDataBound="grdRevalApply_OnDataBinding">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSnoreval" runat="server" Text='<%#Container.DataItemIndex+1 %>'
                                                Visible="true"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgridsubjectcodereval" runat="server" Text='<%# Bind("subject_code") %>'></asp:Label>
                                            <asp:Label ID="lblgridsubjectnoreval" runat="server" Text='<%# Bind("subjectNo") %>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgridsubjectnamereval" runat="server" Text='<%# Bind("subject_name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                                    </asp:TemplateField>
                                  
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="gridcbreval" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#CCCC99" />
                                <HeaderStyle BackColor="#00008B" Font-Bold="True" ForeColor="White" />
                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                <SortedAscendingHeaderStyle BackColor="#848384" />
                                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                <SortedDescendingHeaderStyle BackColor="#575357" />
                            </asp:GridView>
                            <asp:Button ID="btnRevalSubmit" runat="server" Text="Apply" Visible="false" OnClick="btnRevalSubmit_OnClick" />
                           
                            <asp:Label ID="lblRevalSubmitApplication" runat="server" Visible="false"></asp:Label>
                        </div>
                    </asp:Panel>
                    <br />
                </asp:Panel>
            </div>
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender10" runat="server" TargetControlID="Panel14"
                CollapseControlID="Panel13" ExpandControlID="Panel13" Collapsed="true" TextLabelID="LabelGeneral"
                CollapsedSize="0" ImageControlID="ImageGeneral" CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
            </asp:CollapsiblePanelExtender>--%>
            <%--^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ added by prabhaKaran D 16/10/2017  ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^--%>
            <div>
                <asp:Panel ID="Panel9" runat="server" CssClass="cpHeader" Visible="false" Height="11px"
                    Width="1000px" BackImageUrl="~/image/student/RequestBand1.jpg">
                    <br />
                </asp:Panel>
                <asp:Panel ID="Panel10" Visible="false" runat="server">
                    <asp:Panel ID="Panel11" runat="server" CssClass="cpBody">
                        <asp:ImageButton ID="ImageButton4" runat="server" Visible="true" ImageUrl="~/image/Student/Request.jpg"
                            CssClass="cpimage" OnClick="imgbuttonreq_Click" />
                        <asp:ImageButton ID="imagebutton5" runat="server" Visible="true" ImageUrl="~/image/Student/Request.jpg"
                            CssClass="cpimage" OnClick="imgbuttonreqapp_click" />
                        <asp:ImageButton ID="imagebutton6" runat="server" Visible="true" ImageUrl="~/image/Student/hostelRequest.jpg"
                            CssClass="cpimage" alt="Hostel Request" OnClick="imgbuttonreqhostel_click" />
                    </asp:Panel>
                    <asp:Label ID="lbl_show_err" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    <br />
                    <center>
                        <div id="divreq" style="float: left;" runat="server" visible="false">
                            <div class="subdivstyle" style="background-color: White; height: 500px; width: 525px;">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div id="indiv">
                                            <br />
                                            <center>
                                                <table>
                                                    <tr>
                                                        <tr>
                                                            <td>
                                                                Apply Date
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_appldatereq" runat="server" CssClass="textbox textbox1" Width="100px"
                                                                    Enabled="false"></asp:TextBox>
                                                                <asp:CalendarExtender ID="calapplreqdt" runat="server" TargetControlID="txt_appldatereq"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Request Reason
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlgatepass" runat="server" CssClass="ddlheight5
        textbox1" Width="200px" onchange="reason(this)" onfocus="return myFunction(this)">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_ddlgatepassreson" runat="server" Style="display: none;" onfocus="return myFunction(this)"
                                                                    CssClass="textbox textbox1
        "></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Expected Date From
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtfromdatereq" runat="server" OnTextChanged="txtfromdatereq_TextChanged"
                                                                    CssClass="textbox textbox1" Width="100px" AutoPostBack="true"> </asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                                                                    ValidChars="/" runat="server" TargetControlID="txtfromdatereq">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:CalendarExtender ID="CalendarExtender4" CssClass="cal_Theme1 ajax__calendar_active"
                                                                    TargetControlID="txtfromdatereq" Format="dd/MM/yyyy" runat="server" Enabled="True">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Expected Time Exit
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlendhourreq" runat="server" Width="50px" CssClass="ddlheight2 textbox1">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlendminreq" runat="server" Width="50px" CssClass="ddlheight2 textbox1">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlenssessionreq" runat="server" Width="50px" CssClass="ddlheight2 textbox1">
                                                                    <asp:ListItem>AM</asp:ListItem>
                                                                    <asp:ListItem>PM</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Expected Date To
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txttodatereq" runat="server" OnTextChanged="txttodatereq_TextChanged"
                                                                    AutoPostBack="true" CssClass="textbox textbox1" Width="100px"> </asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender51" FilterType="Custom,Numbers"
                                                                    ValidChars="/" runat="server" TargetControlID="txttodatereq">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:CalendarExtender ID="CalendarExtender41" CssClass="cal_Theme1 ajax__calendar_active"
                                                                    TargetControlID="txttodatereq" Format="dd/MM/yyyy" runat="server" Enabled="True">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Expected Time Entry
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlhourreq" Width="50px" runat="server" CssClass="ddlheight textbox1">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlminreq" Width="50px" runat="server" CssClass="ddlheight
        textbox1">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlsessionreq" runat="server" Width="50px" CssClass="ddlheight textbox1">
                                                                    <asp:ListItem>AM</asp:ListItem>
                                                                    <asp:ListItem>PM</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Requested By
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddl_requestedbyk" runat="server" Width="160px" CssClass="ddlheight2 textbox1">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Additional Information
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_studdetail" runat="server" Height="30px" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom,Numbers,UppercaseLetters,LowercaseLetters"
                                                                    ValidChars=" ," runat="server" TargetControlID="txt_studdetail">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblerrreq" Text="" Style="color: Red;" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                </table>
                                                <br />
                                                <asp:Button ID="btnreq" runat="server" OnClick="btnreq_click" Text="Request" CssClass="textbox textbox1 btn2" />
                                            </center>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div id="divright" runat="server" visible="false" style="float: right;">
                            <div class="subdivstyle" style="background-color: White; height: 500px; width: 425px;">
                                <br />
                                <br />
                                <br />
                                <div style="height: 400px; overflow: auto;">
                                    <asp:GridView ID="grdshow" runat="server" Visible="false" AutoGenerateColumns="false"
                                        GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Month" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_month" runat="server" Text='<%#Eval("month") %>'></asp:Label>
                                                    <%-- <asp:Label ID="lbl_monno" runat="server" Text='<%#Eval("monthno") %>'></asp:Label>
                                                    --%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Allowed Permission" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lblleave" runat="server" Text='<%#Eval("allleave") %>'></asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Granted Permission" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="grantleave" runat="server" Text='<%#Eval("grantleave") %>'></asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remaining Permission" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="remainlev" runat="server" Text='<%#Eval("balleave") %>'></asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div id="gate_div" runat="server" visible="false">
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        From Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_fromdt" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdt" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        To Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_todt" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                        <asp:CalendarExtender ID="caltodt" runat="server" TargetControlID="txt_todt" Format="dd/MM/yyyy"
                                            CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_goreqapp" runat="server" Text="Go" CssClass="textbox textbox1"
                                            OnClick="btn_goreqapp_click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div>
                                <center>
                                    <asp:Panel ID="pheaderfilter4" Visible="false" runat="server" CssClass="maintablestyle"
                                        Height="22px" Width="850px" Style="margin-top: -0.1%;">
                                        <asp:Label ID="lbl_mag" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                            Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                        <asp:Image ID="Image5" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                    </asp:Panel>
                                </center>
                                <br />
                            </div>
                            <asp:Panel ID="pcolumnorder4" Visible="false" runat="server" CssClass="maintablestyle"
                                Width="850px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox_column4" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column4_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton6" runat="server" Font-Size="X-Small" Height="16px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                Visible="false" Width="111px" OnClick="LinkButtonsremove4_Click">Remove  All</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                            <asp:TextBox ID="tborder4" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                                AutoPostBack="true" runat="server" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cblcolumnorder4" runat="server" Height="43px" AutoPostBack="true"
                                                Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder4_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="RequestCode">Requisition No</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="RequestDate">Requisition Date</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="ReqStaffAppNo">Requested Staff</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="Stud_Name">Student Name</asp:ListItem>
                                                <asp:ListItem Value="Staff_Name">Staff Name</asp:ListItem>
                                                <asp:ListItem Value="MemType">Mem Type</asp:ListItem>
                                                <asp:ListItem Value="RequestBy">Request By</asp:ListItem>
                                                <asp:ListItem Value="RequestMode">Request Mode</asp:ListItem>
                                                <asp:ListItem Value="GateReqExitDate">Exit Date</asp:ListItem>
                                                <asp:ListItem Value="GateReqExitTime">Exit Time</asp:ListItem>
                                                <asp:ListItem Value="GateReqEntryDate">Entry Date</asp:ListItem>
                                                <asp:ListItem Value="GateReqEntryTime">Entry Time</asp:ListItem>
                                                <asp:ListItem Value="ReqAppStatus">Approval Status</asp:ListItem>
                                                <asp:ListItem Value="ReqApproveStage"> Approval Stage</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </asp:Panel>
                            <asp:CollapsiblePanelExtender ID="cpecolumnorder4" runat="server" TargetControlID="pcolumnorder4"
                                CollapseControlID="pheaderfilter4" ExpandControlID="pheaderfilter4" Collapsed="true"
                                TextLabelID="lbl_mag" CollapsedSize="0" ImageControlID="Image5" CollapsedImage="right.jpeg"
                                ExpandedImage="down.jpeg">
                            </asp:CollapsiblePanelExtender>
                            <br />
                            <asp:Label ID="lbl_err_mag" runat="server" ForeColor="Red"></asp:Label>
                            <center>
                                <br />
                                <FarPoint:FpSpread ID="Fpspread6" runat="server" Visible="false" BorderWidth="5px"
                                    BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread6_ButtonCommand"
                                    ShowHeaderSelection="false">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                                <%--</div>--%>
                                <br />
                            </center>
                            <asp:Label ID="lbl_errview" runat="server" Visible="false" ForeColor="Red" Font-Bold="true"></asp:Label>
                            <center>
                                <asp:Button ID="btn_reqdelete" runat="server" Text="Delete" Visible="false" Width="100px"
                                    OnClick="btn_reqdelete_Click" CssClass="btn1 textbox textbox1" /></center>
                        </div>
                        <%--barath 10.05.17--%>
                        <div id="hostelreq" runat="server" visible="false">
                            <center>
                                <span>Hostel accommodation required?</span>
                                <asp:RadioButton ID="rdbhostelreq" runat="server" GroupName="a" Text="YES" />
                                <asp:RadioButton ID="rdbhostelnotreq" runat="server" GroupName="a" Text="NO" Checked="true" />
                                <asp:Button ID="btn_hostelreq" runat="server" Text="Save" OnClick="btn_hostelreq_Click"
                                    CssClass="btn1 textbox textbox1" />
                                <asp:Label ID="lbl_hostel" runat="server"></asp:Label>
                            </center>
                        </div>
                        <br />
                    </center>
                    <table>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel12" runat="server">
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Accordion ID="Accordion1" CssClass="accordion" HeaderCssClass="accordionHeader"
                                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                                    runat="server" SelectedIndex="0" FadeTransitions="true" SuppressHeaderPostbacks="true"
                                    TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false" AutoSize="None"
                                    Style="border-color: #FFFFFF; height: auto; margin-left: 1px; margin-right: 50px;
                                    margin-top: -5px; overflow: auto; width: 985px;" Height="59px">
                                </asp:Accordion>
                            </td>
                        </tr>
                    </table>
                    <br />
                </asp:Panel>
            </div>
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender6" runat="server" TargetControlID="Panel10"
                CollapseControlID="Panel9" ExpandControlID="Panel9" Collapsed="true" TextLabelID="LabelGeneral"
                CollapsedSize="0" ImageControlID="ImageGeneral" CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
            </asp:CollapsiblePanelExtender>
            <%--------------Counselling Details Added By Saranya on 10/10/2018-----------%>
            <div>
                <asp:Panel ID="Panelcounselling" Visible="true" runat="server" CssClass="cpHeader"
                    Style="width: 910px;">
                    <span style="font-size: medium; font-weight: bold; font-family: Book Antiqua; float: left;">
                        Counselling Details</span>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pBodyCounselling" runat="server" Width="900px" Visible="true" CssClass="cpBody">
                    <center>
                        <div id="divCounsellingReport" runat="server" visible="false" style="overflow: auto;"
                            width="900px">
                            <asp:GridView ID="grdcounselling" Height="63px" Width="900px" Font-Size="Larger"
                                runat="server" ShowFooter="false" AutoGenerateColumns="true" Font-Names="Book Antiqua"
                                toGenerateColumns="true">
                                <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" Font-Size="Larger" ForeColor="Black" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </center>
                    <br />
                </asp:Panel>
                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender7" runat="server" TargetControlID="pBodyCounselling"
                    CollapseControlID="Panelcounselling" ExpandControlID="Panelcounselling" Collapsed="true"
                    TextLabelID="LabelCounselling" CollapsedSize="0" ImageControlID="ImageCounselling"
                    CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
            </div>
            <%-- ------------------------------------------------------------------------%>
        </div>
    </center>
    <br />
    <asp:Panel ID="panelnotification" runat="server" BorderColor="Black" BackColor="White"
        Visible="false" BorderWidth="2px" Style="left: 150px; top: 450px; position: absolute;"
        Height="550px" Width="690px">
        <div class="PopupHeaderrstud2" id="Div3" style="text-align: left; font-family: MS Sans Serif;
            font-size: Small; font-weight: bold">
            <br />
            <asp:Label ID="lblnotification" runat="server" Text="Notification" Font-Bold="true"
                Font-Names="Book Antiqua" Font-Size="XX-Large" Style="top: 5px; left: 250px;
                position: absolute;"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lblnvdate" runat="server" Text="Date :" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 20px; left: 5px; position: absolute;"></asp:Label>
            <asp:Label ID="lblndate" runat="server" Font-Bold="false" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 20px; left: 50px; position: absolute;"></asp:Label>
            <asp:Label ID="lblsubject" ForeColor="Red" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 50px; left: 5px; position: absolute;"></asp:Label>
            <asp:TextBox ID="txtnotification" runat="server" Enabled="false" ReadOnly="true"
                TextMode="MultiLine" Style="top: 130px; left: 280px; position: absolute; width: 400px;
                height: 350px;"></asp:TextBox>
            <asp:Image ID="Image3" runat="server" Style="top: 130px; left: 25px; position: absolute;
                width: 250px; height: 350px;" />
            <asp:Button ID="btnnok" runat="server" Text="Ok" Font-Names="Book Antiqua" Font-Size="Medium"
                Font-Bold="true" OnClick="btnnok_Click" Style="top: 515px; left: 600px; width: 80px;
                position: absolute;" />
        </div>
    </asp:Panel>
    <br />
    </form>
</body>
</html>
