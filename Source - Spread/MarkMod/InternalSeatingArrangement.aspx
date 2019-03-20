<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="InternalSeatingArrangement.aspx.cs" Inherits="MarkMod_InternalSeatingArrangement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .fontStyle
        {
            font-size: medium;
            font-weight: bolder;
            font-style: oblique;
            padding: 5px;
        }
        .fontStyle1
        {
            font-size: medium;
            font-style: oblique;
            padding: 3px;
            color: Blue;
        }
        .commonHeaderFont
        {
            font-size: medium;
            color: Black;
            font-family: 'Book Antiqua';
            font-weight: bold;
        }
        #printCommonPdf
        {
        }
        .printclass
        {
            display: none;
        }
    </style>
     <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            var span = document.getElementById('<%=Span3.ClientID%>');
            span.style.display = "block";
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>Seating Arrangement</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
         
            span.style.display = "none";
            return false;
        }
    </script>
    <script type="text/javascript">
        function PrintPanel1() {
            var panel = document.getElementById("<%=pnlContent1.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            var span = document.getElementById('<%=Span2.ClientID%>');
            span.style.display = "block";
            var span4 = document.getElementById('<%=Span4.ClientID%>');
            span4.style.display = "block";
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>Seating Arrangement</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            span4.style.display = "none";
            span.style.display = "none";
            return false;
        }

        



    </script>

     <script type="text/javascript">
         function PrintPanel2() {
             var panel = document.getElementById("<%=contentDiv.ClientID %>");
            
             var printWindow = window.open('', '', 'height=auto,width=1191');
             printWindow.document.write('<html>');
             printWindow.document.write('<head> <style type="text/css"> p{ font-size: x-small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
             printWindow.document.write('</head><body>');
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



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Internal
            Seating Arrangement</span>
        <div style="width: 100%; margin: 0px; margin-bottom: 10px; margin-top: 10px;" visible="true">
            <table class="maintablestyle" style="height: auto; width: auto;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="height: 18px; width: 10px"></asp:Label>
                            </td>
                            <td colspan="2">
                        <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="182px" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"
                            AutoPostBack="True" Style="">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" CssClass="commonHeaderFont"
                            AssociatedControlID="txtBatch"></asp:Label>
                            </td>
                            <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlBatch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBatch" Visible="true" Width="67px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlBatch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="140px">
                                        <asp:CheckBox ID="chkBatch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkBatch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblBatch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblBatch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtBatch" runat="server" TargetControlID="txtBatch"
                                        PopupControlID="pnlBatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td >
                        <asp:Label ID="lblDegree" runat="server" CssClass="commonHeaderFont" Text="Degree"
                            AssociatedControlID="txtDegree"></asp:Label>
                            </td>
                            <td >
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlDegree" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtDegree" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlDegree" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="140px">
                                        <asp:CheckBox ID="chkDegree" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblDegree" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                        PopupControlID="pnlDegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td >
                        <asp:Label ID="lblBranch" runat="server" CssClass="commonHeaderFont" Text="Branch"
                            AssociatedControlID="txtBranch"></asp:Label>
                            </td>
                            <td >
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlBranch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBranch" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlBranch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="280px">
                                        <asp:CheckBox ID="chkBranch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblBranch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                        PopupControlID="pnlBranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td >
                        <asp:Label ID="lblTest" runat="server" Text="Test" CssClass="commonHeaderFont" AssociatedControlID="ddlTest"></asp:Label>
                       </td>
                            <td >
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlTest" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlTest" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged"
                                        AutoPostBack="True" Width="126px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    </tr>
                <tr>
                    <td >
                        <asp:Label ID="lblTestDate" runat="server" Text="Test Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td >
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlTestDate" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlTestDate" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlTestDate_SelectedIndexChanged"
                                        AutoPostBack="True" Width="95px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td >
                        <asp:Button ID="btnGenerate" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                            OnClick="btnGenerate_Click" Text="Generate" Style="width: auto; height: auto;" />
                    </td>
                    <td >
                        <asp:CheckBox ID="chkReport" Text="Report" Width="72px" Checked="true" Style=""
                            CssClass="commonHeaderFont" runat="server" AutoPostBack="true" OnCheckedChanged="chkReport_CheckedChanged" />
                    </td>
                    <td >
                   
                        <asp:RadioButton ID="Single" runat="server" Style="" CssClass="fontbold" GroupName="hall"
                            Width="76px" OnCheckedChanged="Single_CheckedChanged" Text="Single"
                            AutoPostBack="True" />
                    </td>
                    <td >
                        <asp:RadioButton ID="Multiple" runat="server" Style="" CssClass="fontbold" GroupName="hall"
                            Width="80px" OnCheckedChanged="Multiple_CheckedChanged" Text="Multiple"
                            AutoPostBack="True" />
                    </td>
                     <td >
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="Upnlhall" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txthall" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlhall" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="140px">
                                        <asp:CheckBox ID="chkhall" CssClass="commonHeaderFont" runat="server" Text="Select All" OnCheckedChanged="chkhall_CheckedChanged"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cblhall" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblhall_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txthall"
                                        PopupControlID="pnlhall" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        </td>
                    <td >
                        <asp:Label ID="lblTestSession" runat="server" Text="Sessions" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td >
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlSession" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged"
                                        AutoPostBack="True" Width="100px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td >
                        <asp:Label ID="lblHall" runat="server" Text="Hall No" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="60px"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlHallNo" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlHall_SelectedIndexChanged"
                                        AutoPostBack="True" Width="90px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                            OnClick="btnGo_Click" Text="Go" Style="width: auto; height: auto;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton ID="Radioformat1" runat="server" Style="" CssClass="fontbold" GroupName="format"
                            Width="100px" OnCheckedChanged="Radioformat1_CheckedChanged" Text="Format 1"
                            AutoPostBack="True" />
                    </td>
                    <td>
                        <asp:RadioButton ID="Radioformat2" runat="server" Style="" AutoPostBack="true" CssClass="fontbold"
                            Width="100px" GroupName="format" OnCheckedChanged="Radioformat2_CheckedChanged"
                            Text="Format 2" />
                    </td>
                    <td>
                        <asp:Button ID="btnMissingStudent" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                            OnClick="btnMissingStudent_Click" Text="Missing Student" Style="width: auto;
                            height: auto;" />
                    </td>
                    
                     <td colspan="4" style="width: auto; margin: 0px;"
                        <asp:CheckBox ID="chkCommonSeating" Text="Common Seating Arrangement" 
                        Width="130px" Style=""
                            CssClass="commonHeaderFont" runat="server" AutoPostBack="true" 
                        OnCheckedChanged="chkCommonSeating_CheckedChanged" />
                        </td>
                        <td>
                    <asp:Label ID="signature" runat="server" Text="Signature" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                    <asp:TextBox ID="txtsignature" Visible="true" Width="100px" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Label ID="lblErrmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Visible="False" Font-Size="Medium" ForeColor="#CC0000"></asp:Label>
    </center>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%--Raport Formate--%>
    <div id="divFormat1" runat="server" visible="false">
        <center>
            <asp:Panel ID="pnlContent1" runat="server" Visible="false">
                <style type="text/css" media="print">
                    @page
                    {
                        size: A3 portrait;
                        margin: 0.5cm;
                    }
                    @media print
                    {
                        .printclass
                        {
                            display: table;
                        }
                        thead
                        {
                            display: table-header-group;
                        }
                        tfoot
                        {
                            display: table-footer-group;
                        }
                        #header
                        {
                            position: fixed;
                            top: 0px;
                            left: 0px;
                        }
                        #footer
                        {
                            position: fixed;
                            bottom: 0px;
                            left: 0px;
                        }
                        #printable1
                        {
                            position: relative;
                            bottom: 30px;
                            height: 300;
                            width: 100%;
                        }
                    
                    }
                    @media screen
                    {
                        thead
                        {
                            display: block;
                        }
                        tfoot
                        {
                            display: block;
                        }
                    }
                </style>
                <div id="printable1">
                    <table width="100%">
                        <thead>
                            <tr>
                                <th colspan="2">
                                    <div>
                                        <table class="printclass" style="width: 100%; font-weight: bold; font-family: Book Antiqua;
                                            font-size: medium; margin-top: 20px;">
                                            <tr>
                                                <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                                                    <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                                                        Width="100px" Height="100px" />
                                                </td>
                                                <td align="center">
                                                    <span id="spF1College" runat="server" style="font-size: 18px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="spF1Aff" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="spF1Controller" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="spF1Seating" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="spExamination" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="Span4" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <span id="spF1Date" runat="server" style="font-size: 14px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <span id="spHallNo" runat="server" style="font-size: 14px;"></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    <%--    <tbody>--%>
                            <tr>
                                <td colspan="2">
                                    <center>
                                        <asp:Table ID="tblFormat1" runat="server" Style="border-color: Black; text-align: center;
                                            border-bottom: 0px solid black; font-weight: bold; font-size: medium; border-style: solid;
                                            border-width: 1px; margin: 0px;">
                                            <asp:TableHeaderRow ID="tblHeader1" runat="server" Style="width: auto; border-color: Black;
                                                text-align: center; border-bottom: 0px solid black; font-weight: bold; font-size: medium;
                                                border-style: solid; border-width: 1px; margin: 0px;">
                                            </asp:TableHeaderRow>
                                            <asp:TableHeaderRow ID="tblHeader2" runat="server" Style="width: auto; border-color: Black;
                                                text-align: center; border-bottom: 0px solid black; font-weight: bold; font-size: medium;
                                                border-style: solid; border-width: 1px; margin: 0px;">
                                            </asp:TableHeaderRow>
                                        </asp:Table>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <center>
                                    <asp:GridView ID="GridView2" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
            width: 875px;" Font-Names="Times New Roman" AutoGenerateColumns="true" BackColor="#F0F8FF"  OnRowDataBound="gridview2_DataBound" Font-Bold="true">
         
              <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
        </asp:GridView>
                                    </center>
                                </td>
                            </tr>
                            <tr></tr>
                            <tr></tr>
                            <tr></tr>
                             <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            <tr>
                                <td align="right">
                                    <span id="Span2" runat="server" style="font-size: 15px;"></span>
                                </td>
                            </tr>
                           <tr></tr>
                            <tr></tr>
                            <tr></tr>
                             <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            <tr></tr>
                             <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            <tr></tr>
                             <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            <tr></tr>
                             <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            
                            <tr>
                            <td colspan="2" align="left">
                            
                            <asp:GridView ID="GridView3" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
            width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="true" BackColor="#F0F8FF" OnRowDataBound="gridview3_DataBound" Font-Bold="true" >
            
              <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
        </asp:GridView>
                            </td>
                            </tr>
                             
                    </table>
                    <%-- <tbody>
                        <tr>
                            <td colspan="2" align="center">
                                
                            </td>
                        </tr>
                    </tbody>--%>
                </div>
                </asp:Panel>
        </center>
                <div>
                <table>

                            
                                        <%-- <FarPoint:FpSpread ID="Fspread3" Visible="false" runat="server" ShowHeaderSelection="false">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" GridLineColor="Black">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>--%>
                                    </center>
                                </td>
                            </tr>
                            
                    
                    </table>
                    <%-- <tbody>
                        <tr>
                            <td colspan="2" align="center">
                                
                            </td>
                        </tr>
                    </tbody>--%>
                </div>
            
         <center>
            <asp:Button ID="btn_directprint" runat="server" CssClass="fontbold" Width="100px"
                Text="Direct Print" OnClientClick="return PrintPanel1();"  />
        </center>
    </div>
    <div id="divFormat2" runat="server" visible="false">
        <center>
            <asp:Panel ID="pnlContents" runat="server" Visible="false" Style="margin: 0px; margin-bottom: 10px;
                margin-top: 10px; position: relative;">
                <style type="text/css" media="print">
                    @page
                    {
                        size: A3 portrait;
                        margin: 0.5cm;
                    }
                    @media print
                    {
                        .printclass
                        {
                            display: table;
                        }
                        thead
                        {
                            display: table-header-group;
                        }
                        tfoot
                        {
                            display: table-footer-group;
                        }
                        #header
                        {
                            position: fixed;
                            top: 0px;
                            left: 0px;
                        }
                        #footer
                        {
                            position: fixed;
                            bottom: 0px;
                            left: 0px;
                        }
                        #printable
                        {
                            position: relative;
                            bottom: 30px;
                            height: 300;
                        }
                    
                    }
                    @media screen
                    {
                        thead
                        {
                            display: block;
                        }
                        tfoot
                        {
                            display: block;
                        }
                    }
                </style>
                <div id="printable">
                    <table>
                        <thead>
                            <tr>
                                <th>
                                    <div>
                                        <table class="printclass" style="width: 100%; font-weight: bold; font-family: Book Antiqua;
                                            font-size: medium; margin-top: 20px;">
                                            <tr>
                                                <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                                                    <asp:Image ID="imgLeftLogo" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                                                        Width="100px" Height="100px" />
                                                </td>
                                                <td align="center">
                                                    <span id="spCollege" runat="server" style="font-size: 18px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="spAffBy" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="spController" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="Span1" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="spSeating" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="center">
                                                    <span id="Span5" runat="server" style="font-size: 15px;"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <span id="spDateSession" runat="server" style="font-size: 14px;"></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </th>
                            </tr>
                           <%-- <tr>
                                <td colspan="2">
                                    <center>
                                        <div>
                                            <asp:Table ID="tblFormat2" runat="server" Style="width: 1000px; border-color: Black;
                                                text-align: center; border-bottom: 0px solid black; font-weight: bold; font-size: medium;
                                                border-style: solid; border-width: 1px;">
                                                <asp:TableRow ID="tblRow1" runat="server">
                                                    <asp:TableCell ID="tblcellsno" runat="server" Text="S.No" Width="30px"></asp:TableCell>
                                                    <%-- <asp:TableCell ID="tblcellInvName" runat="server" Text="Invigilator Name" Width="69px"></asp:TableCell>--%>
                                                    <%-- <asp:TableCell ID="tcInvSign" runat="server" Text="Initials<br/> of the<br/> Invigilator"
                                                        Width="65px"></asp:TableCell>--%>
                                                    <%--<asp:TableCell ID="TableCell4" runat="server" Text="Department" Width="80px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell6" runat="server" Text="Subject Code" Width="100px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell7" runat="server" Text="Register Number" Width="230px"></asp:TableCell>
                                                    <asp:TableCell ID="tblcellHallNo" runat="server" Text="Hall No" Width="55px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell8" runat="server" Text="Total No of Student" Width="80px"></asp:TableCell>--%>
                                                    <%--  <asp:TableCell ID="tcBooletNo" runat="server" Text="Answer Booklet Numbers" Width="40px"></asp:TableCell>
                                                    <asp:TableCell ID="tcHallSuperend" runat="server" Text="Signature <br/>of the<br/> Hall <br/>Superintendents"
                                                        Width="40px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell11" runat="server" Text="Present" Width="55px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell12" runat="server" Text="Absent" Width="55px"></asp:TableCell>
                                                    <asp:TableCell ID="tcBundleNo" runat="server" Text="Bundle No" Width="55px"></asp:TableCell>
                                                    <asp:TableCell ID="TableCell13" runat="server" Text="Initials<br/> of the<br/> Invigilator"
                                                        Width="65px"></asp:TableCell>--%>
                                               <%-- </asp:TableRow>
                                            </asp:Table>
                                        </div>
                                    </center>
                                </td>
                            </tr>--%>
                        </thead>
                        <tbody>
                            <%--<tr>
                                <td colspan="2" align="center">
                                    <FarPoint:FpSpread ID="Fspread3" Visible="false" CommandBar-Visible="false" runat="server"
                                        ShowHeaderSelection="false">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" GridLineColor="Black">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </td>
                            </tr>--%>
                            <tr>
                            <asp:GridView ID="GridView1" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
            width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" BackColor="AliceBlue">
            <Columns>
            <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Department">
                    <ItemTemplate>
                        <asp:Label ID="lbldepart" runat="server" Text='<%# Eval("department") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subject Code">
                <ItemTemplate>
                <asp:Label ID="lblsubcode" runat="server" Text='<%# Eval("subjectcode") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Register Number">
                <ItemTemplate>
                <asp:Label ID="lblregnum" runat="server" Text='<%# Eval("regnum") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hall No">
                <ItemTemplate>
                <asp:Label ID="lblhallno" runat="server" Text='<%# Eval("hallno") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total No of Student">
                <ItemTemplate>
                <asp:Label ID="lbltotstud" runat="server" Text='<%# Eval("totalstud") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
            </Columns>
             <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
            </asp:GridView>
                            </tr>
                             <tr></tr>
                            <tr></tr>
                            <tr></tr>
                             <tr></tr>
                            <tr></tr>
                            <tr></tr>
                            <tr>
                                <td align="right">
                                    <span id="Span3" runat="server" style="font-size: 15px;"></span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </asp:Panel>
        </center>
        <center style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <%--<asp:Label ID="lblreportname2" runat="server" Visible="false" Text="Report Name"
                CssClass="fontbold"></asp:Label>
            <asp:TextBox ID="txtreportname2" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                CssClass="fontbold" Visible="false" onkeypress="display()"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtreportname2"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="Exportexcel" runat="server" Visible="false" CssClass="fontbold" Width="100px"
                Text="Excel" OnClick="btn_excel" />
            <asp:Button ID="Printfspread3" runat="server" Visible="false" CssClass="fontbold"
                Width="100px" Text="Print" OnClick="btn1_print" />--%>
            <asp:Button ID="btnDirectPrintF2" runat="server" Visible="false" CssClass="fontbold"
                Width="100px" Text="Direct Print" OnClientClick="return PrintPanel();" OnClick="btnF2_directprint_Click" />
        </center>
    </div>
    <%----Missing btnMissingStudent------%>
    <center>
        <div id="divMainContents" runat="server" visible="false" style="width: auto; height: auto;">
            <%-- style="margin: 0px; width: 100%; text-align: center;" style="margin: 0px; margin-bottom: 15px;
                margin-top: 10px; padding: 0px; border: 0px; position: relative;"--%>
            <%-- <table style="margin: 0px; padding: 0px; border: 0px;">
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="lblExcelErr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblExcelReportName" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtExcelNameMissing" runat="server" CssClass="textbox textbox1"
                                Height="20px" Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                Font-Names="Book Antiqua" onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="filterExcelNameMissing" runat="server" TargetControlID="txtExcelNameMissing"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\@#$%^&*()-=+!~`<>?|:;'">
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
                            <Insproplus:printmaster runat="server" ID="printCommonPdf" Visible="false" Style="width: auto;
                                height: auto; left: 20%; right: 20%; top: 30%; position: fixed;" />
                        </td>
                        <td>
                            <%--<asp:Button ID="btnDirectPrint" Visible="false" CssClass="textbox textbox1" runat="server"
                                Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                height: auto;" Text="Direct Print" OnClientClick="return PrintPanel();" />
                        </td>
                    </tr>
                </table>---%>
        </div>
    </center>
     <div style="height: 1px; width: 1px; overflow: auto;">
            <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
            </div>
        </div>

</asp:Content>



