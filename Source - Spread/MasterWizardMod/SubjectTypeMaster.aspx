<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWizardMod/MasterWizard.master"
    AutoEventWireup="true" CodeFile="SubjectTypeMaster.aspx.cs" Inherits="MasterWizardMod_SubjectTypeMaster" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            font-family: Book Antiqua;
            height: auto;
            background-color: #ffffff;
            color: Black;
        }
        .Chartdiv
        {
            background-color: #ffffff;
            margin: 0px;
            color: #000000;
            position: relative;
            font-family: Book Antiqua;
            height: auto;
            width: 100%;
        }
        
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
            #divMainContents
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
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%#lbl_norec1.ClientID %>').innerHTML = "";
        }
        function PrintPanel() {

            var panel = document.getElementById("<%=divMainContents.ClientID %>");
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Subject Type Master</span></div>
        </center>
    </div>
    <center>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <div style="width: 1000px; font-family: Book Antiqua; font-weight: bold; height: auto">
                                        <table class="maintablestyle" style="height: auto; margin-top: 10px; margin-bottom: 10px;
                                            padding: 6px;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_pop2collgname" Text="College" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_pop2collgname" runat="server" CssClass="textbox ddlheight5 textbox1"
                                                        AutoPostBack="true"  Width="185px" onfocus="return myFunction1(this)" OnSelectedIndexChanged="ddl_pop2collgname_selectedindexchange">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_pop2batchyr" Text="Batch" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_pop2batchyear" runat="server" CssClass="textbox ddlheight2 textbox1"
                                                        AutoPostBack="true"  Width="67px" onfocus="return myFunction1(this)" OnSelectedIndexChanged="ddl_pop2batchyear_OnSelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_pop2degre" Text="Degree" runat="server" Width="60px"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_pop2degre" runat="server"  Width="80px" CssClass="textbox ddlheight2 textbox1"
                                                        OnSelectedIndexChanged="ddl_pop2degre_SelectedIndexChanged" AutoPostBack="true"
                                                        onfocus="return myFunction1(this)">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_pop2branch" Text="Branch" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_pop2branch" runat="server"  Width="185px" CssClass="textbox ddlheight5 textbox1"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged" onfocus="return myFunction1(this)">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_pop2sem" Text="Semester"  runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsemester" runat="server" Width="50px" CssClass="textbox ddlheight5 textbox1"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                                        onfocus="return myFunction1(this)">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_go" Text="Go" OnClick="btn_go_Click" CssClass="textbox btn1"
                                                        runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_Add" Text="Add" Width="63px" OnClick="btn_Add_Click" CssClass="textbox btn1"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="popupsubject" runat="server" visible="false" class="popupstyle popupheight1">
                                        <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                            Style="height: 30px; width: 30px; position: absolute; margin-top: 26px; margin-left: 377px;"
                                            OnClick="imagebtnpopclose1_Click" />
                                        <br />
                                        <br />
                                        <div style="background-color: White; height: 495px; width: 795px; border: 5px solid #0CA6CA;
                                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                            <br />
                                            <center>
                                                <asp:Label ID="lbl_popup" runat="server" Text="Subject Type Information" class="fontstyleheader"
                                                    Style="color: Green;"></asp:Label>
                                            </center>
                                            <br />
                                            <br />
                                            <table class="maintablestyle" style="width: 717px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_college" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_college" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_college_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                                            AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Width="240px" AutoPostBack="true" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged1">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="LblSem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                            <fieldset style="height: -38px; width: 700px;">
                                                <legend style="height: 10">Paper Type</legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Text="Enter the Paper Type" Font-Bold="True"
                                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPaperType" runat="server" CssClass="  textbox txtheight1" Height="17px"
                                                                Width="271px"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="PaperType" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtPaperType"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan">
                                                            </asp:AutoCompleteExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" Text="No.Of Papers" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Textpaperno" runat="server" Font-Bold="True" Placeholder="0" CssClass="  textbox txtheight1"
                                                                Height="17px" Width="50px" MaxLength="2"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="Textpaperno"
                                                                FilterType="Numbers">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label3" runat="server" Text="EX:Allied,Therory,Practicals,Etc..."
                                                                Font-Bold="True" ForeColor="BlueViolet" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="8">
                                                            <asp:CheckBox ID="chkCompulsary" runat="server" Text="Compulsary" />
                                                            <asp:CheckBox ID="chkLAB" runat="server" Text="LAB Oriented" />
                                                            <asp:CheckBox ID="chkProject" runat="server" Text="Project/Thesis" />
                                                            <asp:CheckBox ID="chkSeminar" runat="server" Text="Seminar" />
                                                            <asp:CheckBox ID="chkElective" runat="server" Text="Elective" />
                                                            <asp:CheckBox ID="chkOnElective" runat="server" Text="Online Elective" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <fieldset style="height: -38px; width: 700px;">
                                                <legend style="height: 10">Fees/Paper In RS.</legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label4" runat="server" Text="Current" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextCurrent" runat="server" Font-Bold="True" Placeholder="0.00"
                                                                CssClass="  textbox txtheight1" Height="17px" Width="75px" MaxLength="6"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="TextCurrent"
                                                                FilterType="Numbers,Custom" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label5" runat="server" Text="Arrear" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextArrear" runat="server" Font-Bold="True" Placeholder="0.00" CssClass="  textbox txtheight1"
                                                                Height="17px" Width="75px" MaxLength="6"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextArrear"
                                                                FilterType="Numbers,Custom" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label6" runat="server" Text="Improvement" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextImprovement" runat="server" Font-Bold="True" Placeholder="0.00"
                                                                CssClass="  textbox txtheight1" Height="17px" Width="75px" MaxLength="6"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="TextImprovement"
                                                                FilterType="Numbers,Custom" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label7" runat="server" Text="Re-Total" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextRetot" runat="server" Font-Bold="True" Placeholder="0.00" CssClass="  textbox txtheight1"
                                                                Height="17px" Width="75px" MaxLength="6"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextRetot"
                                                                FilterType="Numbers,Custom" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label8" runat="server" Text="Re-Valuation" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextReval" runat="server" Font-Bold="True" Placeholder="0.00" CssClass="  textbox txtheight1"
                                                                Height="17px" Width="75px" MaxLength="6"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="TextReval"
                                                                FilterType="Numbers,Custom" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <br />
                                            <br />
                                            <center>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="textbox" BackColor="#1B9D17"
                                                                ForeColor="White" Font-Bold="true" OnClick="btn_save_Click" Style="width: 80px;
                                                                height: 30px;" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btn_Delete" runat="server" Text="Delete" Visible="false" CssClass="textbox"
                                                                BackColor="Chocolate" ForeColor="White" Font-Bold="true" OnClick="btn_Delete_Click"
                                                                Style="width: 80px; height: 30px;" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox" BackColor="Red"
                                                                ForeColor="White" Font-Bold="true" Style="width: 80px; height: 30px;" OnClick="btn_exit_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <div id="divMainContents" runat="server" style="display: table; margin: 0px; height: auto;
                    margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
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
                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="grdsubject" runat="server" ShowFooter="false" ShowHeader="false"
                        AutoGenerateColumns="true" Font-Names="book antiqua" togeneratecolumns="true"
                        Width="1238px" OnRowDataBound="Showgrid_OnRowDataBound" OnRowCreated="OnRowCreated"
                        OnSelectedIndexChanged="SelectedIndexChanged">
                        <%-- OnSelectedIndexChanged="grdsubject_onselectedindexchanged"--%>
                        <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                    </asp:GridView>
                </div>
                <br />
                <br />
                <center>
                    <div id="rptprint" class="noprint" runat="server" visible="false" style="margin: 0px;
                        margin-bottom: 20px; margin-top: 15px; position: relative;">
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
                        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                            Height="35px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                            CssClass="textbox textbox1" />
                        <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </center>
                <center>
                    <div id="alertimg" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 270px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btn_errorclose1" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_errorclose_Click1" Text="ok" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <center>
                    <div id="divPopAlertNEW" runat="server" visible="false" style="height: 550em; z-index: 2000;
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
                                                <asp:Label ID="lblAlertMsgNEW" runat="server" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btn_yes" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                        CssClass="textbox textbox1" Style="height: auto; width: 65px; margin-left: -80px;"
                                                        OnClick="btn_yes_Click" Text="Yes" runat="server" />
                                                </center>
                                            </td>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btn_No" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                        Visible="true" CssClass="textbox textbox1" Style="height: auto; width: 65px;
                                                        margin-left: -175px;" OnClick="btn_No_Click" Text="No" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
                <asp:PostBackTrigger ControlID="btnPrint" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
</asp:Content>
