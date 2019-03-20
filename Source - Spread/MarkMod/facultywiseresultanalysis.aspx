<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="facultywiseresultanalysis.aspx.cs" Inherits="facultywiseresultanalysis" %>

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
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Registration.css" rel="Stylesheet" type="text/css" />
        <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js" type="text/javascript"></script>
        <style type="text/css">
            .sty
            {
                font-size: medium;
                font-family: Book Antiqua;
                font-weight: bold;
            }
            .multicheckbox
            {
                z-index: 1;
                left: 258px;
                top: -1222px;
                position: absolute;
                overflow: auto;
                background-color: white;
                border: 1px solid gray;
                color: Black;
            }
            .maintablestyle
            {
                border: 1px solid #0CA6CA;
                border-radius: 10px;
                background-color: #0CA6CA;
                box-shadow: 0px 0px 8px #999999;
            }
            .maindivstyle
            {
                border: 1px solid #999999; /* background-color: #F0F0F0;*/
                box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
                -moz-box-shadow: 0px 0px 10px #999999;
                -webkit-box-shadow: 0px 0px 10px #999999;
                border: 3px solid #D9D9D9;
                border-radius: 15px;
            }
        </style>
        <script type="text/javascript" language="javascript">

            function display12() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function checktxt() {
                empty = "";
                id = document.getElementById("<%=txtexcelname.ClientID %>").value;
                if (id.trim() == "") {
                    document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "Please Enter Your Report Name";
                    empty = "E";
                }

                if (empty != "") {
                    return false;
                }
                else {

                    return true;
                }
            }
        </script>
    </head>
    <body>

        <form id="form1">
        <asp:ScriptManager runat="server" ID="ScriptManger1">
        </asp:ScriptManager>
        <br />
        <center>
            <asp:Label ID="Label5" runat="server" Style="font-size: large; font-weight: bold;
                color: Green;" Text="CR37 - Overall College Faculty Wise Result Analysis Report"></asp:Label></center>
        <br />
        <center>
            <table style="width: 700px; height: 60px; background-color: #0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="120px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbach" runat="server" Text="Batch" Style="font-family: 'Book Antiqua';"
                            Font-Bold="True" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Width="120px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbatch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    CssClass="multicheckbox" BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                    PopupControlID="pbatch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Style="font-family: 'Book Antiqua';"
                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" CssClass="dropdown"
                                    Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pdegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    CssClass="multicheckbox" BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua';
                                    overflow-y: scroll;">
                                    <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                    PopupControlID="pdegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Style="font-family: 'Book Antiqua';" Text="Branch"
                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    CssClass="multicheckbox" BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua';
                                    overflow-y: scroll;">
                                    <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Height="58px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                                    PopupControlID="pbranch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblcri" runat="server" Style="font-family: 'Book Antiqua';" Text="Test"
                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txttest" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="Panel3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    CssClass="multicheckbox" BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua';
                                    overflow-y: scroll;">
                                    <asp:CheckBox ID="chktest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chktest_CheckedChanged" />
                                    <asp:CheckBoxList ID="chkltest" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Height="58px" OnSelectedIndexChanged="chkltest_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txttest"
                                    PopupControlID="Panel3" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Height="30px" CssClass="dropdown" Text="Go"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btngo_Click" />
                    </td>
                </tr>
            </table>
        </center>
        <br />
        <asp:Label ID="errmsg" runat="server" ForeColor="Red" CssClass="sty" Visible="false"></asp:Label>
        <br />
        <div id="showdata" runat="server">
            
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
            <br />
            <div id="rptprint" runat="server" visible="true">
                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="" CssClass="sty"
                    Visible="true"></asp:Label>
                <br />
                <asp:Label ID="lblrptname" runat="server" CssClass="sty" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" CssClass="sty" onkeypress="display12()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="sty"
                    OnClientClick="return checktxt()" Text="Export To Excel" Width="130px" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                    CssClass="sty" />
                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />

                <button id="btnPrint" runat="server" visible="true" height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
            </div>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
