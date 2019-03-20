<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Department_performance.aspx.cs" Inherits="Department_performance" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <style type="text/css">
        .style11
        {
            width: 68px;
            height: 2px;
        }
        .style14
        {
            height: 2px;
            width: 73px;
        }
        .style33
        {
            height: 2px;
            width: 65px;
        }
        .style34
        {
            height: 2px;
        }
        .style35
        {
            height: 2px;
            width: 138px;
        }
        .style36
        {
            height: 2px;
            width: 54px;
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
        .style37
        {
            top: 226px;
            left: 10px;
            position: absolute;
            height: 21px;
            width: 249px;
        }
        @media print
        {
        
            divStudentMentor
            {
                display: block;
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
    <body>
        <script type="text/javascript">
            function display() {

                document.getElementById('MainContent_lblerr').innerHTML = "";

            }

            function PrintPanel() {
                var panel = document.getElementById("<%=divStudentMentor.ClientID %>");
                var printWindow = window.open('', '', 'height=842,width=1191');
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
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="20px"
                Style="width: 1003px">
                <center>
                    <asp:Label ID="Label1" runat="server" Text=" Department Performance" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White"></asp:Label>
                </center>
            </asp:Panel>
        </div>
        <div>
            <asp:Panel ID="Panel1" runat="server" Height="121px" BackColor="Lightblue" BorderColor="Black"
                ClientIDMode="Static" Width="1000px" Style="margin-bottom: 0px; border: 1px solid #000;
                height: 63px;">
                <table style="height: 73px; width: 1017px; margin-bottom: 0px;">
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style="height: 18px; width: 44px"></asp:Label>
                            <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" AutoPostBack="True"
                                Style="">
                            </asp:DropDownList>
                        </td>
                        <td class="style34">
                            <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text=" Test" Style="width: 31px">
                            </asp:Label>
                        </td>
                        <td class="style14">
                            <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged"
                                Height="21px" Style="width: 171px;" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnPrintMaster" runat="server" Text="Print Master Setting" Visible="False"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="width: 160px;" />
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" Style="text-align: center;" Text="Go" Width="40px"
                                Height="28px" Font-Bold="True" Visible="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                OnClick="btnGo_Click1" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblerror" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="#FF3300" Style="height: 21px; width: 329px" Text="No Record(s) Found"
                                Visible="False"></asp:Label>
                        </td>
                    </tr>
                </table>
                <center>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnsection" Visible="false" runat="server" BackColor="Aquamarine"
                                    Text="" />
                            </td>
                            <td>
                                <asp:Label ID="lblsctnn" runat="server" Font-Size="Large" Text="With Out Sections"
                                    Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="newpnl" runat="server" Style="margin-left: 1px;">
                        <div id="divStudentMentor" runat="server"  style="display: table;
                            margin: 0px; height: auto; margin-bottom: 20px; margin-top: 10px; position: relative;
                            width: auto; text-align: left;">
                            <tr>
                                <td>
                                    <center>
                                        <asp:GridView ID="gridviewdep" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                            HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true"
                                            OnRowDataBound="OnRowDataBound">
                                        </asp:GridView>
                                    </center>
                                </td>
                            </tr>
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btnxl_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                                    <asp:Button ID="BtnPrint" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btnprintmaster_Click" Font-Size="Medium" Text="Print" Width="127px" />
                                    <asp:Button ID="Button1" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblerr" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                </center>
            </asp:Panel>
            <%-- Hidden by srinath 12/3/2014
                <asp:Image ID="Image1" runat="server" 
            style="top: 373px; left: 229px; position: absolute; height: 16px; width: 14px" />--%>
            <asp:Panel ID="Panel3" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
                Style="width: 1003px;">
                <%-- style="top: 71px; left: 0px; position: absolute; width: 960px"--%>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <br />
            </asp:Panel>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </center>
            <br />
            <asp:Button ID="Button2" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="top: 197px; left: 690px; position: absolute; height: 33px;
                width: 54px" Enabled="False" Visible="False" />
            <br />
            <br />
            <br />
            <br />
            <%-- <asp:Label ID="lblnospread" runat="server" Text="No Records Found" 
            Visible="False" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua" 
            Font-Size="Medium" 
            style="top: 330px; left: 7px; position: absolute; height: 20px; width: 215px" ></asp:Label>--%>
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Style="margin-top: 37px;" Text="There is no record found"
                Visible="False" CssClass="style37"></asp:Label>
            &nbsp;
            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                Style="top: 225px; left: 4px; position: absolute; height: 21px; width: 219px"></asp:Label>
            &nbsp;&nbsp;
            <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 226px; left: 230px;
                position: absolute; height: 21px; width: 126px"></asp:Label>
            &nbsp;&nbsp;
            <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" Font-Bold="True"
                Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" Height="24px" Width="58px"
                Style="top: 223px; left: 365px; position: absolute">
            </asp:DropDownList>
            &nbsp;&nbsp;
            <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                Style="top: 228px; left: 433px; position: absolute"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                FilterType="Numbers" />
            &nbsp;&nbsp;
            <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                Width="96px" Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 224px;
                left: 476px; position: absolute; height: 21px"></asp:Label>
            &nbsp;&nbsp;
            <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="17px" Style="top: 226px;
                left: 579px; position: absolute; width: 34px;"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                FilterType="Numbers" />
            &nbsp;&nbsp;
            <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 226px; left: 628px;
                position: absolute; height: 21px; width: 303px"></asp:Label>
            <br />
            <%-- <asp:Button ID="Button4" runat="server" 
            style="top: 166px; left: 901px; position: absolute; height: 23px; width: 56px" 
            Text="Button" />--%>
            <br />
            <br />
            <br />
            <br />
            <br />
            <%-- </form>--%>
    </body>
    </html>
</asp:Content>
