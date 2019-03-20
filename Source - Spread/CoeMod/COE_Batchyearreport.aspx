<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="COE_Batchyearreport.aspx.cs" Inherits="COE_Batchyearreport" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div>
        <script type="text/javascript">
            function PrintPanel() {

                var panel = document.getElementById("<%=pnlContents.ClientID %>");
                var printWindow = window.open('', '', 'height=842,width=1191');
                printWindow.document.write('<html');
                printWindow.document.write('<head><title>DIV Contents</title>');
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
       
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       <br />
            <center>
                <asp:Label ID="Label4" runat="server" Text="Batchwise Year Report" Font-Bold="True"
                   ForeColor="Green"
                    Font-Names="Book Antiqua" Font-Size="Large"></asp:Label>
            </center>
            <br />
           
                             
        <center>
            <table style="width:700px; height:70px; background-color:#0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="Lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                             Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                             Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                             Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="150Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsem" runat="server" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                             Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsubtype" runat="server" Text="Subject Type" Font-Bold="True" Font-Names="Book Antiqua"
                             Font-Size="Medium" Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtsubtype" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="150px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="psubtype" runat="server" CssClass="MultipleSelectionDDL">
                                    <asp:CheckBox ID="chksubtype" runat="server" Font-Bold="True" OnCheckedChanged="chksubtype_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True"
                                        ForeColor="Black" />
                                    <asp:CheckBoxList ID="chklssubtype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklssubtype_SelectedIndexChanged"
                                        ForeColor="Black">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsubtype"
                                    PopupControlID="psubtype" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        
                        <asp:Button ID="Buttongo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="Buttongo_Click" Text="Go" />
                    </td>
                </tr>
            </table>
            </center>
        
        
        
        <asp:Label ID="lblerror" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
            Width="800px" Font-Bold="true" ForeColor="Red"></asp:Label>
        
        <asp:Button ID="btnreset" runat="server" OnClick="btnresetclick" Text="Reset" Visible="false"
            Style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;" />
        
        <asp:Panel ID="pnlContents" runat="server">
            <style type="text/css" media="print">
                @page
                {
                    size: A3 portrait;
                    margin: 0.5cm;
                }
                
                @media print
                {
                
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
                       
                    }
                    #footer
                    {
                        position: fixed;
                        bottom: 0px;
                        
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
                    .hb
                    {
                        display: block;
                    }
                    tfoot
                    {
                        display: block;
                    }
                }
            </style>
            
            <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                ShowHeaderSelection="false" BorderWidth="1px" Width="2000" Visible="false" OnUpdateCommand="FpSpread1_UpdateCommand"
                HorizontalScrollBarPolicy="Never">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark" ShowPDFButton="false">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </asp:Panel>
        
        <asp:Button ID="btnsave" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnsave_Click" Text="Save" />
        <div id="rptprint" runat="server" visible="false" style="font-family: Book Antiqua;
            font-size: medium; font-weight: bold;">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblrptname" runat="server" Style="font-family: Book Antiqua; font-size: medium;
                            font-weight: bold;" Text="Report Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" Style="font-family: Book Antiqua;
                            font-size: medium; font-weight: bold;" runat="server" Height="20px" Width="180px"
                            onkeypress="display()"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Style="font-family: Book Antiqua;
                            font-size: medium; font-weight: bold;" CssClass="textbox btn1" Text="Export To Excel"
                            Width="127px" />
                    </td>
                    <td>
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" Style="font-family: Book Antiqua;
                            font-size: medium; font-weight: bold;" OnClick="btnprintmaster_Click" CssClass="textbox btn2" />
                        <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
                    </td>
                    <td>
                        <asp:Button ID="btnPrint" runat="server" Style="font-family: Book Antiqua; font-size: medium;
                            font-weight: bold;" Text="Direct Print" OnClientClick="return PrintPanel();" />
                    </td>
                    <td>
                        <asp:Button ID="btn_commonprint" runat="server" Style="font-family: Book Antiqua;
                            font-size: medium; font-weight: bold;" Text="Common Print" OnClick="btn_commonprint_OnClick" />
                    </td>
                </tr>
            </table>
        </div>
        
        <%--</ContentTemplate>--%>
        <%-- </asp:UpdatePanel>--%>
    </div>
</asp:Content>

