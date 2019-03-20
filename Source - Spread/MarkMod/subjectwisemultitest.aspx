<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="subjectwisemultitest.aspx.cs" Inherits="subjectwisemultitest" %>

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

  <%--  <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <style type="text/css">
        .gvRow
        {
            margin-right: 0px;
            margin-top: 325px;
        }
        
        .gvRow td
        {
            background-color: #F0FFFF;
            font-family: Book Antiqua;
            font-size: medium;
            padding: 3px;
            border: 1px solid black;
        }
        
        .gvAltRow td
        {
            font-family: Book Antiqua;
            font-size: medium;
            padding: 3px;
            border: 1px solid black;
            background-color: #CFECEC;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblnorec').innerHTML = "";
        }
    </script>
    <br />
    <center>
        <asp:Label ID="Label1" runat="server" Text="Subjectwise Multiple Test Result Report"
            Font-Bold="true" Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
    </center>
    <br />
    <center>
        <table border="0" class="maintablestyle" style="width: 1020px; height: 100px; background-color: #0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="lblyear" runat="server" Height="20px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Batch Year"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="dropyear" runat="server" Width="110px" Height="25px" Font-Bold="True"
                        OnSelectedIndexChanged="dropyear_SelectedIndexChanged" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblschooltype" runat="server" Height="20px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Degree"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddschooltype" runat="server" Width="110px" Height="25px" AutoPostBack="true"
                        OnSelectedIndexChanged="dropschooltype_SelectedIndexChanged" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblstandard" runat="server" Height="20px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Branch"></asp:Label>
                </td>
                <td colspan="2" style="padding-left:20px;">
                    <asp:DropDownList ID="ddstandard" runat="server" Height="25px" Width="250px" AutoPostBack="true"
                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddstandard_SelectedIndexChanged"
                        Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSemYr" runat="server" Text="Sem" Font-Bold="True" Visible="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" style="padding-right:10px;"></asp:Label>
                
                    <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Visible="true"
                        OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="height: 21px; width: 44px;">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px;
                        width: 110px;">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbltest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Test"></asp:Label>
                </td>
                <td>
                   <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>--%>
                            <asp:TextBox ID="txttest" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="True"
                                ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                                Style="font-family: Book Antiqua; font-size: medium; width: 105px;" Width="105px"></asp:TextBox>
                            <asp:Panel ID="pnltest" runat="server" CssClass="multxtpanel" Height="155px" Style="width: auto;">
                                <asp:CheckBox ID="cbtest" runat="server" Text="SelectAll" AutoPostBack="True" Font-Bold="True"
                                    ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbtest_OnCheckedChanged" />
                                <asp:CheckBoxList ID="cbltest" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Style="width: 157px;" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua"
                                    OnSelectedIndexChanged="cbltest_OnSelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txttest"
                                PopupControlID="pnltest" Position="Bottom">
                            </asp:PopupControlExtender>
                      <%--  </ContentTemplate>--%>
                       <%-- <Triggers>
                            <asp:PostBackTrigger ControlID="cbtest" />
                            <asp:PostBackTrigger ControlID="cbltest" />
                        </Triggers>--%>
                  <%--  </asp:UpdatePanel>--%>
                </td>
                <td>
                    <asp:Label ID="lblsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Subject"></asp:Label>
                </td>
                <td style="padding-left:20px;">
                  <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                            <asp:TextBox ID="txtsub" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="True"
                                ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                                Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Width="105px"></asp:TextBox>
                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Height="155px" Style="width: auto;">
                                <asp:CheckBox ID="cbsub" runat="server" Text="SelectAll" AutoPostBack="True" Font-Bold="True"
                                    ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbsub_OnCheckedChanged" />
                                <asp:CheckBoxList ID="cblsub" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Style="width: 157px;" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua"
                                    OnSelectedIndexChanged="cblsub_OnSelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtsub"
                                PopupControlID="Panel1" Position="Bottom">
                            </asp:PopupControlExtender>
                <%--        </ContentTemplate>--%>
                        <%--<Triggers>
                            <asp:PostBackTrigger ControlID="cbsub" />
                            <asp:PostBackTrigger ControlID="cblsub" />
                        </Triggers>--%>
                    <%--</asp:UpdatePanel>--%>
                </td>
                <td style="padding-left:20px;">
                    <asp:Label ID="lblgender" runat="server" Height="20px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Gender"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlgender" runat="server" Width="110px" Height="25px" Font-Bold="True"
                        OnSelectedIndexChanged="ddlgender_SelectedIndexChanged" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true">
                        <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem>Male</asp:ListItem>
                        <asp:ListItem>Female</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblhost" runat="server" Height="20px" Width="100px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Student Type"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlhost" runat="server" Width="110px" Height="25px" Font-Bold="True"
                        OnSelectedIndexChanged="ddlhost_SelectedIndexChanged" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true">
                        <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem>Day Scholar</asp:ListItem>
                        <asp:ListItem>Hostler</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="2">
                    <asp:RadioButton ID="rbmark" runat="server" Text="Mark Range" GroupName="vvas" Font-Size="Medium"
                        Font-Bold="True" AutoPostBack="true" OnCheckedChanged="rbmark_CheckedChanged"
                        Style="font-family: 'Book Antiqua';" />
               
                    <asp:RadioButton ID="rbarrear" runat="server" Text="Fail" GroupName="vvas" Font-Size="Medium"
                        Font-Bold="True" AutoPostBack="true" OnCheckedChanged="rbarrear_CheckedChanged"
                        Style="font-family: 'Book Antiqua';" />
                </td>
                <td  colspan="2">
                    <asp:Label ID="lblrngfrom" runat="server" Height="20px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Mark Range" style="padding-right:7px;" />
                
                    <asp:TextBox ID="txtrngfrom" runat="server" Height="20px" Width="57px" MaxLength="3"
                        placeholder="From" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtrngfrom"
                        FilterType="Numbers,Custom" ValidChars="-">
                    </asp:FilteredTextBoxExtender>
                    <asp:TextBox ID="txtrngto" runat="server" Height="20px" Width="57px" placeholder="To"
                        MaxLength="3" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtrngto"
                        FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                </td>
                <td colspan="2">
                    <asp:RadioButtonList ID="rbl_testOrSUbWise" runat="server" Font-Bold="true" RepeatDirection="Horizontal" Font-Names="Book Antiqua">
                    <asp:ListItem id="rb_testWise" runat="server" Text="Test Wise" Selected="True"></asp:ListItem>
                    <asp:ListItem id="rb_subWise" runat="server" Text="Subject Wise"></asp:ListItem>
                    
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:CheckBox ID="Chktotinternal" runat="server" Text="Include Internal" Font-Bold=true />
                </td>
                <td>
                 
                    <asp:Button ID="Button1" runat="server" Text="Go" OnClick="btngo_Click" />

                    
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblerrormsg" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="true" ForeColor="#FF3300"></asp:Label>
        <br />
    </center>
   

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
            <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                        HeaderStyle-BackColor="#0CA6CA" BorderColor="Black"  Width="950px" >
                                    </asp:GridView>

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
    <br />
    <center>
        <div id="final" runat="server" visible="false">
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Visible="False" CssClass="style50"></asp:Label>
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtexcelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                InvalidChars="/\">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
            <button id="btnPrint" runat="server" visible="true" height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
        </div>
    </center>

   <%-- </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnExcel" />
                                <asp:PostBackTrigger ControlID="Button1" />
                                
                                </Triggers>
                             </asp:UpdatePanel>
--%>
                             
    


</asp:Content>
