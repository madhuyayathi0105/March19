<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="internalassessment.aspx.cs" Inherits="internalassessment" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <style type="text/css">
        .font
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            margin-left: 0px;
        }
        
        .style1
        {
            top: 270px;
            left: -23px;
            width: 1063px;
            position: absolute;
        }
        
        .style2
        {
            width: 145px;
        }
        .style3
        {
            width: 101px;
        }
        
        .style4
        {
            width: 119px;
        }
        .style5
        {
            width: 101px;
        }
        .head
        {
            background-color: Teal;
            font-family: Book Antiqua;
            font-size: medium;
            color: black;
            top: 165px;
            position: absolute;
            font-weight: bold;
            width: 980px;
            height: 25px;
            left: 15px;
        }
        .mainbatch
        {
            background-color: #3AAB97;
            width: 980px;
            position: absolute;
            height: 80px;
            top: 190px;
            left: 15px;
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: black;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblerr').innerHTML = "";

        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
    <br />
    <center>
        <span class="fontstyleheader" style="color: Green;">CAM R7-Internal Assessment</span>
        <br />
        <br />
        <asp:Panel ID="pnlhome" runat="server" Style="background-color: #0CA6CA; position: static;
            border: 1px solid  #000; height: 80PX; width: 957px;">
            <table style="float: left;">
                <tr>
                    <td>
                        <asp:Label ID="lblbatch" runat="server" Text="Batch" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="font" Width="70px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="style3">
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" CssClass="font" Width="81px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Height="23px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Text="Branch" CssClass="font"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:DropDownList ID="ddlbranch" runat="server" CssClass="font" Width="200px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsem" runat="server" Text="Sem" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsem" runat="server" CssClass="font" Width="50px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsec" runat="server" Text="Sec" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsec" runat="server" CssClass="font" Width="50px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblmrkoutof" runat="server" Text="Mark OutOf" CssClass="font" Width="103px"
                            Height="20px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtmrkoutof" runat="server" Width="40px" MaxLength="3" CssClass="font"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtmrkoutoffilter" runat="server" TargetControlID="txtmrkoutof"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
            </table>
            <table style="float: left;">
                <tr>
                    <%--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ SUBJECT TYPE @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@--%>
                    <td class="style5">
                        <asp:Label ID="lblsubjtype" runat="server" Text="Subject Type" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtsubjtype" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" CssClass="Dropdown_Txt_Box" ReadOnly="true" Height="20px" Width="136px"></asp:TextBox>
                        <asp:Panel ID="pveh1" runat="server" CssClass="multxtpanel" Height="128px" Width="141px">
                            <asp:CheckBox ID="chksubjtype" runat="server" CssClass="font" Text="Select All" OnCheckedChanged="chksubjtype_CheckedChanged"
                                AutoPostBack="true" />
                            <asp:CheckBoxList ID="chkbxlistsubjtype" runat="server" CssClass="font" AutoPostBack="true"
                                OnSelectedIndexChanged="chkbxlistsubjtype_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender123" runat="server" TargetControlID="txtsubjtype"
                            PopupControlID="pveh1" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <%--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ SUBJECT NAME @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@--%>
                    <td>
                        <asp:Label ID="lblsubj" runat="server" Text="Subject" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtsubj" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" CssClass="Dropdown_Txt_Box" ReadOnly="true" Height="20px" Width="111px"></asp:TextBox>
                        <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="200" Width="340px">
                            <asp:CheckBox ID="chksubj" runat="server" CssClass="font" Text="Select All" OnCheckedChanged="chksubj_CheckedChanged"
                                AutoPostBack="true" />
                            <asp:CheckBoxList ID="chkbxlistsubj" runat="server" CssClass="font" AutoPostBack="true"
                                OnSelectedIndexChanged="chkbxlistsubj_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtsubj"
                            PopupControlID="Panel3" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                        <asp:Button ID="btngenerate" runat="server" Text="Go" CssClass="font" OnClick="btngenerate_Click" />
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                    <asp:CheckBox ID="chkincludepastout" runat="server" Text="Include PassedOut" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="includepastout_CheckedChanged"   
                                            AutoPostBack="True" /></td>
                                            <td>  <asp:CheckBox ID="chkRoundoff1" runat="server" Text="With Round off" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" /></td>

                    <%--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ SUBJECT NAME OR CODE @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@--%>
                    <td class="style4">
                        <asp:Label ID="lblsub_name_code" runat="server" Text="Display Option" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtsub_name_code" runat="server" Font-Names="Book Antiqua" placeholder="--Select--"
                            Font-Size="Medium" Font-Bold="true" CssClass="Dropdown_Txt_Box" Height="20px"
                            Width="136px" ReadOnly="true"></asp:TextBox>
                        <asp:CheckBox ID="chckstaff" runat="server" Text="Staff" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                        <asp:CheckBox ID="chcksec" runat="server" Text="Section" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                        <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Height="200" Width="250px">
                            <asp:CheckBoxList ID="chkbxlisisub_name_code" runat="server" CssClass="font" AutoPostBack="true"
                                OnSelectedIndexChanged="chkbxlisisub_name_code_SelectedIndexChanged">
                                <asp:ListItem>Subject Name</asp:ListItem>
                                <asp:ListItem>Subject Code</asp:ListItem>
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsub_name_code"
                            PopupControlID="Panel4" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <%--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@--%>
                    <td colspan="13">
                        <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Text="Print Master Setting"
                            Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnPrint_Click"
                            Width="151px" />
                        &nbsp;
                        <asp:Label ID="lblpages" runat="server" Text="Page" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="false"></asp:Label>
                        &nbsp;<asp:DropDownList ID="ddlpage" runat="server" AutoPostBack="True" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                            Height="16px" Width="58px" Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" BackColor="#719DDB"
            Width="959px">
            <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                Font-Bold="True" Font-Names="Book Antiqua" />
            <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                ImageAlign="Right" />
        </asp:Panel>
        <asp:Panel ID="pbodyfilter" runat="server" CssClass="cpBody" Width="952px">
            <asp:CheckBoxList ID="chklscolumn" runat="server" Font-Size="Medium" AutoPostBack="True"
                OnSelectedIndexChanged="chklscolumn_SelectedIndexChanged" Font-Bold="True" RepeatColumns="5"
                RepeatDirection="Horizontal" Font-Names="Book Antiqua">
                <asp:ListItem Text="S.No"></asp:ListItem>
                <asp:ListItem Text="ROLL No"></asp:ListItem>
                <asp:ListItem Text="REGISTRATION No"></asp:ListItem>
                <asp:ListItem Text="STUDENT NAME"></asp:ListItem>
                <asp:ListItem Text="STUDENT TYPE"></asp:ListItem>
                <asp:ListItem Text="SUBJECTS"></asp:ListItem>
                <asp:ListItem Text="TOTAL"></asp:ListItem>
                <asp:ListItem Text="PERCENTAGE"></asp:ListItem>
                <asp:ListItem Text="RANK"></asp:ListItem>
                <asp:ListItem Text="NO. OF SUBJECTS FAILED"></asp:ListItem>
                <asp:ListItem Text="NO. OF STUDENT PRESENT"></asp:ListItem>
                <asp:ListItem Text="NO. OF STUDENTS ABSENT"></asp:ListItem>
                <asp:ListItem Text="NO. OF STUDENTS ON OD"></asp:ListItem>
                <asp:ListItem Text="NO. OF STUDENTS PASSED"></asp:ListItem>
                <asp:ListItem Text="NO. OF STUDENTS FAILED"></asp:ListItem>
                <asp:ListItem Text="PASS %"></asp:ListItem>
                <asp:ListItem Text="CLASS AVERAGE"></asp:ListItem>
                <asp:ListItem Text="SUBJECT AVERAGE"></asp:ListItem>
                <asp:ListItem Text="STAFF NAME"></asp:ListItem>
                <asp:ListItem Text="STAFF CODE"></asp:ListItem>
                <asp:ListItem Text="OVERALL PASS PERCENTAGE"></asp:ListItem>
                <asp:ListItem Text="NO OF STUDENTS ALL CLEARED"></asp:ListItem>
                <asp:ListItem Text="SCRIPT WISE PASS PERCENTAGE"></asp:ListItem>
            </asp:CheckBoxList>
        </asp:Panel>
        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pbodyfilter"
            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
            ExpandedImage="../images/down.jpeg">
        </asp:CollapsiblePanelExtender>
    </center>
    <asp:Panel ID="pageset_pnl" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Label ID="Buttontotal" runat="server" Visible="false" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua">
                    </asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblrecord" runat="server" Font-Bold="True" Visible="false" Text="Records Per Page"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" Visible="false"
                        OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Height="24px" Width="58px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxother" runat="server" Visible="false" Width="34px" AutoPostBack="True"
                        OnTextChanged="TextBoxother_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                        FilterType="Numbers" />
                </td>
                <td>
                    <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Width="96px"
                        Font-Names="Book Antiqua" Visible="false" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxpage" runat="server" AutoPostBack="True" Visible="false"
                        OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="27px" Width="60px"></asp:TextBox>
                </td>
                <td>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                        FilterType="Numbers" />
                </td>
                <td>
                    <asp:Label ID="LabelE" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <center>
        <asp:Label ID="lblnorec" runat="server" Text="No Records Found" CssClass="font" Visible="false"
            ForeColor="Red"></asp:Label>
    </center>
    <br />
   <center>
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
                          
                            <asp:GridView ID="Showgrid" runat="server"  Visible="false" HeaderStyle-ForeColor="Black"
                        HeaderStyle-BackColor="#0CA6CA" >
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
   
        
                            

                    
       
        <asp:Chart ID="attedancechart" runat="server" Width="800px" Visible="true">
            <Series>
                <asp:Series Name="Series1" IsValueShownAsLabel="true" ChartArea="ChartArea1" ChartType="Column">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                    <AxisY LineColor="White">
                        <LabelStyle Font="Trebuchet MS, 8.25pt" />
                        <MajorGrid LineColor="#e6e6e6" />
                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                    </AxisY>
                    <AxisX LineColor="White">
                        <LabelStyle Font="Trebuchet MS, 8.25pt" />
                        <MajorGrid LineColor="#e6e6e6" />
                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        <br />
        <asp:Label ID="lblerr" runat="server" Text="" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium"></asp:Label>
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" ()[]{},.">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
            OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
        <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnmasterprint_Click" />
        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
        <button id="btnDirectPrint" runat="server" Visible="False"  height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
    </center>

     </ContentTemplate>
        <Triggers>
            
            <asp:PostBackTrigger ControlID="btnExcel" />
            
            
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

</asp:Content>
