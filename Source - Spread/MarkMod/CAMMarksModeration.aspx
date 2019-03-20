<%@ Page Title="CAM Mark Moderation" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CAMMarksModeration.aspx.cs" Inherits="MarkMod_CAMMarksBoostUp"
    EnableEventValidation="false" %>

    <%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        body
        {
            margin: 0px;
            padding: 0px;
            border: 0px;
            width: 100%;
            height: auto;
        }
    </style>
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%=lblExcelError.ClientID %>').innerHTML = "";
        }
    </script>
     <script type="text/javascript">
         function printTTOutput() {
             var panel = document.getElementById("<%=printdiv.ClientID %>");
             var printWindow = window.open('', '', 'height=816,width=980');
             printWindow.document.write('<html><head>');
             printWindow.document.write('</head><body >');
             printWindow.document.write(panel.innerHTML);
             printWindow.document.write('</body></html>');
             printWindow.document.close();
             setTimeout(function () {
                 printWindow.print();
             }, 500);
             return false;
         }
    </script>
    <style tyle="text/css">
        .printclass
        {
            display: none;
        }
        .marginSet
        {
            margin: 0px;
            padding: 0px;
        }
        .headerDisp
        {
            font-size: 25px;
            font-weight: bold;
        }
        .headerDisp1
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        @media print
        {
            #printdiv
            {
                display: block;
            }
            .printclass
            {
                display: block;
                font-family: Book Antiqua;
            }
            .noprint
            {
                display: none;
            }
        }
        @media screen,print
        {
        
        }
        @page
        {
            size: A4;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="margin: 0px; padding: 0px; border: 0px; width: 100%; height: auto;">
        <center>
            <span class="fontstyleheader" style="color: Green; font-weight: bold; margin: 0px;
                margin-bottom: 15px; margin-top: 10px; position: relative; padding: 3px;">CAM Mark
                Moderation</span>
            <table class="maintablestyle" style="width: 950px; height: auto; background-color: #0CA6CA;
                padding: 5px; margin: 0px; margin-bottom: 15px; margin-top: 10px; position: relative;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" Visible="true" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged" Width="255px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" Width="70px" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True"
                            Width="81px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True"
                            Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSem" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"> </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSem" Width="48px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"> </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSec" runat="server" Width="55px" AutoPostBack="True" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="12">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTest" runat="server" Text="Test" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"> </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTest" runat="server" Width="85px" AutoPostBack="True" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSubjects" runat="server" Text="Subject" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="upnlSubject" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtSubject" Width="95px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnlSubject" runat="server" CssClass="multxtpanel" Style="width: 280px;
                                                    height: 300px; overflow: auto; margin: 0px; padding: 0px;">
                                                    <asp:CheckBox ID="chkSubject" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                                        margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkSubject_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cblSubject" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                        runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                                        padding: 0px; border: 0px;" OnSelectedIndexChanged="cblSubject_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popExtSubject" runat="server" TargetControlID="txtSubject"
                                                    PopupControlID="pnlSubject" Position="Bottom">
                                                </asp:PopupControlExtender>
                                                <asp:DropDownList ID="ddlSubejct" Visible="false" Width="130px" runat="server" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlSubejct_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblModerationFromRanges" runat="server" Text="From Mark" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromRange" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="40px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filterFromRAnge" runat="server" FilterType="Numbers"
                                        TargetControlID="txtFromRange" />
                                </td>
                                <td>
                                    <asp:Label ID="lblModerationToRange" runat="server" Text="To Mark" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToRange" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="40px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filterToRange" runat="server" FilterType="Numbers"
                                        TargetControlID="txtToRange" />
                                </td>
                                <td>
                                <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnGo" CssClass="textbox textbox1" runat="server" Font-Bold="True"
                                        Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto; height: auto;"
                                        Text="Go" OnClick="btnGo_Click" />

                                        </ContentTemplate>
                                </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
        <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
            margin-bottom: 15px; margin-top: 10px;"></asp:Label>
        <center>
            <div id="divMainContents" runat="server" visible="false" style="margin: 0px; margin-bottom: 5px;
                margin-top: 10px;">
                <center>
                    <div id="divCAMTestMarkBoost" style="margin: 0px; margin-bottom: 5px; margin-top: 10px;">
                        <div id="divPrintContent" runat="server" style="margin: 0px; margin-top: 20px;">
                            <table>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblExcelError" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblReportName" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Report Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtExcelFileName" runat="server" CssClass="textbox textbox1" Height="20px"
                                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtExcelFileName"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                            InvalidChars="/\">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnExportExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            OnClick="btnExportExcel_Click" Font-Size="Medium" Style="width: auto; height: auto;"
                                            Text="Export To Excel" CssClass="textbox textbox1" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnPrintPDF" runat="server" Text="Print" OnClick="btnPrintPDF_Click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Style="width: auto;
                                            height: auto;" CssClass="textbox textbox1" />
                                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                    </td>
                                    <td>
                                         <button id="btnPrint" runat="server"  height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
                                    </td>
                                     <td>
                                     <asp:UpdatePanel ID="btnsaveupdatepanel" runat="server">
                                        <ContentTemplate>
                                        <asp:Button ID="btnSaveModeration" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            OnClick="btnSaveModeration_Click" Font-Size="Medium" Text="Save Moderation" CssClass="textbox textbox1"
                                            Style="width: auto; height: auto;" />

                                             </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                       


                       
                         <center>
                         <div id="printdiv" runat="server">
            <table class="printclass" style="width: 98%; height: auto; margin: 0px; padding: 0px;">
                <tr>
                    <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                        <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                            Width="100px" Height="100px" />
                    </td>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spCollegeName" class="headerDisp" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spAddr" class="headerDisp1" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spReportName" class="headerDisp1" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <td class="marginSet" colspan="3" align="center">
                        <span id="spDegreeName" class="headerDisp1" runat="server"></span>
                    </td>
                    <td class="marginSet" colspan="3" align="right">
                        <span id="spSem" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td class="marginSet" colspan="3" align="left">
                        <span id="spProgremme" class="headerDisp1" runat="server"></span>
                    </td>
                    <td class="marginSet" colspan="3" align="right">
                        <span id="spSection" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
            </table>
      <center>
                          
                            <asp:GridView ID="grdover" runat="server" Width="500px" BorderStyle="Double" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" GridLines="Both" CellPadding="4"  
                             ShowFooter="false" ShowHeader="true">
                           
                        </asp:GridView>
                        </center>
            <table class="printclass" style="width: 98%; height: auto; margin-top: 100px; padding: 0px;">
                <tr>
                    <td>
                        
                    </td>
                    <td style="text-align: right">
                        
                    </td>
                </tr>
            </table>
        </div>
                           
                    
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
    </div>

    </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnExportExcel" />
                                </Triggers>
                             </asp:UpdatePanel>

                             <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
    
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="btnsaveupdatepanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
