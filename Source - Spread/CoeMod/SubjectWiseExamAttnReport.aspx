<%@ Page Title="Exam Attendance Report" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SubjectWiseExamAttnReport.aspx.cs" Inherits="CoeMod_ExamAttendanceReportNew"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .table2
        {
            border: 1px solid #0CA6CA;
            border-radius: 10px;
            background-color: #0CA6CA;
            box-shadow: 0px 0px 8px #7bc1f7;
            padding: 3px;
        }
        .cpHeader
        {
            color: white;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
            width: 1000px;
        }
        .cpimage
        {
            vertical-align: middle;
            margin: 0px;
            padding: 0px;
            border: 0px;
            text-align: center;
            background-color: transparent;
        }
    </style>
    <script type="text/javascript">
        function validation() {
            var hall = document.getElementById('<%=txthallno.ClientID %>').value;
            if (hall == "- - Select - -") {
                alert("Please Select Hall No");
                return false;
            }
            else {
                return true;
            }
            var dept = document.getElementById('<%=txtdept.ClientID %>').value;
            if (dept == "- - Select - -") {
                alert("Please Select Department");
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">Exam Attendance Report</span>
        <table class="maintablestyle" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative; padding: 5px;">
            <tr>
                <td>
                    <asp:Label ID="lblyear" runat="server" Text="Year" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                        Width="100px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblmonth" runat="server" Text="Month" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                        Width="100px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbltype" runat="server" Text="Type" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddltype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="True" Width="100px" OnSelectedIndexChanged="ddlfrmdate_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblfrmdate" runat="server" Text="Date " Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlfrmdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="True" Width="100px" OnSelectedIndexChanged="ddlfrmdate_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:CheckBox ID="chkheadimage" runat="server" Text="Header Image" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Session" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsession" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="True" Width="100px" OnSelectedIndexChanged="ddlsession_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblhallnno" runat="server" Text="Hall No" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txthallno" runat="server" Height="20px" Width="100px" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" ReadOnly="true">- - All - -</asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="checkconsolidate" runat="server" OnCheckedChanged="checkconsolidate_OnCheckedChanged"
                        AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Consolidate" />
                </td>
                 <td>
                    <asp:CheckBox ID="chksubwise" runat="server" OnCheckedChanged="chksubwise_OnCheckedChanged"
                        AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Subjectwise" />
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Font-Bold="True" Text="Go" Font-Names="Book Antiqua"
                        OnClientClick="return validation()" Font-Size="Medium" OnClick="btngo_Click" />
                </td>
                <td>
                    <asp:Label ID="lbldepart" runat="server" Font-Bold="True" Visible="false" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text=" Department">
                    </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtdept" runat="server" Height="20px" Visible="false" ReadOnly="true"
                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium">- - All - -</asp:TextBox>
                </td>
                
                  <td>
                    <asp:DropDownList ID="ddlSubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Visible="false"
                        Font-Size="Medium" AutoPostBack="True" Width="180px" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                  <td>  <asp:CheckBox ID="CheckBox1" runat="server" 
                        AutoPostBack="false" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Without Footer"  Visible="true"/></td>
            </tr>
        </table>
        <center>
            <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader table2" Style="margin: 0px;
                margin-bottom: 10px; margin-top: 10px; position: relative; width: 920px; height: auto;">
                <center>
                    <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                        Font-Bold="True" Font-Names="Book Antiqua" />
                    <asp:Image ID="Imagefilter" runat="server" AlternateText="" ImageAlign="Right" CssClass="cpimage"
                        ImageUrl="~/images/right.jpeg" />
                </center>
            </asp:Panel>
            <asp:Panel ID="pbodyfilter" runat="server" Style="height: auto; width: 920px; margin: 0px;
                margin-bottom: 10px; margin-top: 10px; position: relative;" CssClass="table2">
                <table>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="tborder" Visible="false" TextMode="MultiLine" AutoPostBack="true"
                                runat="server">
                            </asp:TextBox>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButtonsremove" Font-Size="X-Small" Visible="false" runat="server"
                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small;">Remove  All</asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <asp:CheckBoxList ID="cblsearch" runat="server" RepeatLayout="Table" CellPadding="2"
                    Style="font-family: 'Book Antiqua'; font-weight: 700; width: auto; height: auto;
                    font-size: medium;" RepeatColumns="4" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0"> Answer Booklet No </asp:ListItem>
                    <asp:ListItem Value="1"> Seat No </asp:ListItem>
                    <asp:ListItem Value="2"> Absentees</asp:ListItem>
                    <asp:ListItem Value="3"> Signature of Candidate </asp:ListItem>
                    <asp:ListItem Value="4"> Hall No</asp:ListItem>
                    <asp:ListItem Value="5"> Student Type </asp:ListItem>
                    <asp:ListItem Value="6"> Department </asp:ListItem>
                    <asp:ListItem Value="7"> Subject Code And Name </asp:ListItem>
                    <asp:ListItem Value="8"> Student Photo </asp:ListItem>
                </asp:CheckBoxList>
            </asp:Panel>
        </center>
        <asp:CollapsiblePanelExtender ID="cpefilter" runat="server" TargetControlID="pbodyfilter"
            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
            ExpandedImage="~/images/down.jpeg">
        </asp:CollapsiblePanelExtender>
    </center>
    <center>
        <asp:Label ID="lblerr1" runat="server" Text="" Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="false" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative;"></asp:Label>
        <asp:Panel ID="Panel4" runat="server">
            <table style="margin: 0px; margin-bottom: 5px; margin-top: 5px; position: relative;">
                <tr>
                    <td>
                        <asp:Panel ID="paneltxtdept" runat="server" BackColor="White" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="2px" Height="273px" ScrollBars="Vertical" Width="303px">
                            <asp:CheckBox ID="cbbatselectall" runat="server" AutoPostBack="True" Font-Bold="True"
                                OnCheckedChanged="paneltxtdept_CheckedChanged" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="21px" Width="89px" Text="Select All" />
                            <asp:CheckBoxList ID="Chkbat" runat="server" Font-Size="Small" AutoPostBack="True"
                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="paneltxtdept_SelectedIndexChanged"
                                Height="37px">
                            </asp:CheckBoxList>
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="paneltxthallno" runat="server" BackColor="White" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="2px" Height="280px" ScrollBars="Vertical" Width="170px">
                            <asp:CheckBox ID="cbdepselectall" runat="server" AutoPostBack="True" OnCheckedChanged="paneltxthallno_CheckedChanged"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="21px" Width="89px"
                                Text="Select All" />
                            <asp:CheckBoxList ID="Chkdep" runat="server" Font-Size="Small" AutoPostBack="True"
                                Font-Bold="True" ForeColor="Black" OnSelectedIndexChanged="paneltxthallno_SelectedIndexChanged"
                                Font-Names="Book Antiqua" Height="39px">
                            </asp:CheckBoxList>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownExtender ID="DropDownExtender3" runat="server" DropDownControlID="paneltxtdept"
                            DynamicServicePath="" Enabled="true" TargetControlID="txtdept">
                        </asp:DropDownExtender>
                        <asp:DropDownExtender ID="DropDownExtender5" runat="server" DropDownControlID="paneltxthallno"
                            DynamicServicePath="" Enabled="true" TargetControlID="txthallno">
                        </asp:DropDownExtender>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <table style="margin: 0px; margin-bottom: 5px; margin-top: 5px; position: relative;">
            <tr>
                <td>
                    <asp:Label ID="lblDate" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" ForeColor="#FF3300" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
        <table style="margin: 0px; margin-bottom: 5px; margin-top: 5px; position: relative;">
            <tr>
                <td align="right">
                    <FarPoint:FpSpread ID="AttSpread" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Visible="true" OnButtonCommand="AttSpread_OnUpdateCommand"
                        ShowHeaderSelection="false">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
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
        <table style="margin: 0px; margin-bottom: 5px; margin-top: 5px; position: relative;">
            <tr>
                <td>
                    <asp:Label ID="lblexportxl" runat="server" Visible="false" Width="95px" Height="20px"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Export Excel"
                        ForeColor="Black"></asp:Label>
                    <asp:TextBox ID="txtexcell" runat="server" Visible="false" Height="20px" Width="180px"
                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
                        InvalidChars="/\">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="g1btnexcel" runat="server" OnClick="g1btnexcel_OnClick" Visible="false"
                        Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Style="margin-left: 6px;" />
                </td>
                <td>
                    <asp:Button ID="g1btnprint" runat="server" OnClick="g1btnprint_OnClick" Visible="false"
                        Text="Print" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    <Insproplus:PRINTPDF runat="server" ID="PRINTPDF1" Visible="false" />
                </td>
            </tr>
        </table>
        <table style="margin: 0px; margin-bottom: 5px; margin-top: 5px; position: relative;">
            <tr>
                <td colspan="8">
                    <asp:Label ID="lblDispErr" runat="server" Text="" Font-Bold="True" ForeColor="Red"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Style="margin: 0px;
                        margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnDummyNoSheets" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false" OnClick="btnDummyNoSheets_click" Text="Dummy Number Sheet" />
                </td>
                <td align="right">
                    <asp:Button ID="btnDisplay" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false" OnClick="btndisplay_click" Text="Print" />
                </td>
              
                <td align="right">
                    <asp:Button ID="btnPhaseSheet" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false" OnClick="btnPhaseSheet_click" Text="Phasing Sheet" />
                </td>
                <td align="right">
                    <asp:Button ID="btnFoilCard" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false" OnClick="btnFoilCard_click" Text="Foil Card" />
                </td>
                <td align="right">
                    <asp:Button ID="btngenerate" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false" OnClick="btngenerate_click" Text="Display" />
                </td>
                <td align="right">
                    <asp:Label ID="lblPages" runat="server" Font-Bold="True" Visible="false" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Hall No :"></asp:Label>
                </td>
                <td align="right">
                    <asp:DropDownList ID="ddlPageNo" Visible="false" runat="server" Font-Bold="true"
                        Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlPageNo_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:Button ID="btnrest" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false" OnClick="btnrest_click" Text="Reset" />
                </td>
            </tr>
        </table>
        <div id="divDummyNoSheets" runat="server" visible="false" style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">
            <asp:GridView ID="gvDummyNoSheet" Visible="true" runat="server" AutoGenerateColumns="false"
                GridLines="Both" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
                <Columns>
                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                        HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <center>
                                <asp:Label ID="lblSno" runat="server" Width="60px" Text='<%#Eval("SNo") %>'></asp:Label>
                            </center>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Register No" HeaderStyle-BackColor="#0CA6CA">
                        <ItemTemplate>
                            <asp:Label ID="lblRegNo" runat="server" Width="200px" Text='<%#Eval("Reg_No") %>'></asp:Label>
                            <asp:Label ID="lblRollNo" runat="server" Visible="false" Width="200px" Text='<%#Eval("Roll_No") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="85px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject Code" Visible="false" HeaderStyle-BackColor="#0CA6CA">
                        <ItemTemplate>
                            <asp:Label ID="lblSubjectCode" runat="server" Width="200px" Text='<%#Eval("Subject_code") %>'></asp:Label>
                            <asp:Label ID="lblSubjectNo" runat="server" Visible="false" Width="200px" Text='<%#Eval("Subject_no") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="85px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dummy No 1" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblDummyNo1" runat="server" Width="200px" Text='<%#Eval("DummyNo") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dummy No 2" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblDummyNo2" runat="server" Width="200px" Text='<%#Eval("DummyNo") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dummy No 3" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblDummyNo3" runat="server" Width="200px" Text='<%#Eval("DummyNo") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dummy No 4" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblDummyNo4" runat="server" Width="200px" Text='<%#Eval("DummyNo") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <FarPoint:FpSpread ID="FpDummyNoSheets" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Visible="false" ShowHeaderSelection="false" Style="margin: 0px;
                margin-bottom: 10px; margin-top: 10px; position: relative;">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </div>
        <table style="margin: 0px; margin-bottom: 5px; margin-top: 5px; position: relative;
            height: auto; width: auto;">
            <tr>
                <td>
                    <asp:Label ID="lblerror1" runat="server" Text="" Font-Bold="True" ForeColor="Red"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Style="margin: 0px;
                        margin-bottom: 5px; margin-top: 5px; position: relative;"></asp:Label>
                    <center>
                        <FarPoint:FpSpread ID="Subjectspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="2px" Visible="false" ShowHeaderSelection="false" Style="margin: 0px;
                            margin-bottom: 5px; margin-top: 5px; position: relative;">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblrptname" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Visible="false" onkeypress="display()"
                        Height="20px" Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                    <asp:Button ID="btnExcel" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                        OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                    <asp:Button ID="Button2" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Print" OnClick="btnPrint_Click1" Width="127px" />
                    <asp:Label ID="printcheckvalue" runat="server" Visible="false"></asp:Label>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+|\}{][':;?><,./">
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblnorecc" runat="server" Font-Bold="True" Width="650px" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
    </center>
     <asp:PlaceHolder ID="plBarCode" runat="server" />
    <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
</asp:Content>
