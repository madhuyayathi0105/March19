<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Incometaxcalculation_report.aspx.cs" Inherits="Incometaxcalculation_report" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>--%>
    <style type="text/css">
        .FpreadtextAlign
        {
            vertical-align: middle;
            height: auto !important;
            word-wrap: break-word;
            text-align: justify;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function PrintDiv() {
                var panel = document.getElementById("<%=contentDiv.ClientID %>");
                var printWindow = window.open('', '', 'height=auto,width=1191');
                printWindow.document.write('<html');
                printWindow.document.write('<head> <style type="text/css"> p{ font-size: 10px;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px; font-size: 10px;}</style>');
                //                
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
            //             add by poomalar line-height:15px;
            function PrtDiv() {
                var panel = document.getElementById("<%=spread2div.ClientID %>");
                var printWindow = window.open('', '', 'height=auto,width=1191');
                printWindow.document.write('<html');
                printWindow.document.write('<head> <style type="text/css"> p{ font-size: x-small;margin: 0px; padding: 2px; border: 0px;} body{ margin:0px;}</style>');
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
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <br />
                <center>
                    <div>
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Income Tax Calculation Report</span>
                            </div>
                        </center>
                    </div>
                </center>
                <div class="maindivstyle" style="width: 1000px; height: auto;">
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_college" runat="server" Text="College Name" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="120px"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="updatecollege" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_OnSelectedIndexChanged"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="285px" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_dept" runat="server" Text="Department" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                            Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                        <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                            border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                            box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                            <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_dept"
                                            PopupControlID="p1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_desig" runat="server" Text="Designation" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_desig" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                            Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                        <asp:Panel ID="P2" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                            border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                            box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                            <asp:CheckBox ID="cb_desig" runat="server" Text="Select All" OnCheckedChanged="cb_desig_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cbl_desig" runat="server" OnSelectedIndexChanged="cbl_desig_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_desig"
                                            PopupControlID="P2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_staffc" runat="server" Text="Staff Category" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_staffc" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                            Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                        <asp:Panel ID="P3" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                            border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                            box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                            <asp:CheckBox ID="cb_staffc" runat="server" Text="Select All" OnCheckedChanged="cb_staffc_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cbl_staffc" runat="server" OnSelectedIndexChanged="cbl_staffc_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_staffc"
                                            PopupControlID="P3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_sname" runat="server" Text="Staff Name" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_sname" runat="server" MaxLength="50" Style="font-weight: bold;
                                    font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sname"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td colspan="3">
                                <fieldset>
                                    <asp:RadioButtonList ID="radFormat" runat="server" RepeatDirection="Horizontal" Width="330px">
                                        <asp:ListItem Selected="True" Value="0" Text="IT Calculation"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Format II"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Form 16"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </fieldset>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" Text="Go" OnClick="btn_go_Click" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;" CssClass="textbox btn1" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:CheckBox ID="CbShowDiscription" runat="server" Text="Show Discription" Style="font-weight: bold;" />
                                <asp:CheckBox ID="cb_relived" runat="server" Text="Include Relieved Staff" Style="font-weight: bold;" />
                            </td>
                            
                            
                               
                            
                            <%-- <td>
                                    <asp:CheckBox ID="cbRabate" runat="server" Text=" Rebate Amount" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRebate" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                </td>--%>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="ermsg" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
                    <center>
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="5px"
                            BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px"
                            OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged"
                            OnButtonCommand="Fpspread1_ButtonCommand">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <br />
                    <center>
                        <div>
                            <asp:Button ID="btnExcel16" runat="server" Visible="false" Text="Export Excel" OnClick="btnPrintExcel_Click"
                                Width="100px" CssClass="textbox btn1" />
                            <asp:Button ID="btnPrint" runat="server" Visible="false" Text="Print PDF" OnClick="btnPrint_Click"
                                Width="100px" CssClass="textbox btn1" />
                        </div>
                    </center>
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow" runat="server" visible="false" style="height: 220em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <asp:ImageButton ID="btnClose" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 50px; margin-left: 474px;"
                    OnClick="btnClose_Click" />
                <center>
                    <div id="div4" runat="server" class="table" style="background-color: White; height: auto;
                        width: 975px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 60px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <table>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:Button ID="btn_cumulativetax" runat="server" Text="Individual Cumulative Tax"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium;" OnClick="btn_cumulativetax_Click"
                                            CssClass="textbox textbox1 btn2" Width="225px" />
                                    </td>
                                    <td colspan="4">
                                        <asp:Button ID="btn_individualincometaxstatus" runat="server" Text="Individual Income Tax Status"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium;" CssClass="textbox textbox1 btn2"
                                            Width="230px" OnClick="btn_individualincometaxstatus_Click" />
                                    </td>
                                </tr>
                            </table>
                            <table id="filters_tbl" runat="server">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_frommonth" runat="server" Style="font-weight: bold; font-family: book antiqua;
                                            font-size: medium;" Text="From Month"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_frommonth" Width="90px" runat="server" CssClass="textbox1 ddlstyle ddlheight">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_fromyear" runat="server" Style="font-weight: bold; font-family: book antiqua;
                                            font-size: medium;" Text="From Year"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_fromyear" Width="60px" runat="server" CssClass="textbox1 ddlstyle ddlheight">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_tomonth" runat="server" Style="font-weight: bold; font-family: book antiqua;
                                            font-size: medium;" Text="To Month"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_tomonth" Width="90px" runat="server" CssClass="textbox1 ddlstyle ddlheight">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_toyear" runat="server" Style="font-weight: bold; font-family: book antiqua;
                                            font-size: medium;" Text="To Year"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_toyear" Width="60px" runat="server" CssClass="textbox1 ddlstyle ddlheight">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span>Allowance Name</span>
                                    </td>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="Upp4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_allowancemultiple" runat="server" CssClass="textbox  textbox1 txtheight3">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Width="250px" Height="180px">
                                                    <asp:CheckBox ID="cb_allowancemultiple" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_allowancemultiple_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_allowancemultiple" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_allowancemultiple_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_allowancemultiple"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <span>Deduction Name</span>
                                    </td>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_deduction" runat="server" CssClass="textbox  textbox1 txtheight3">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Width="250px" Height="180px">
                                                    <asp:CheckBox ID="cb_deduction" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_deduction_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_deduction" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_deduction_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_deduction"
                                                    PopupControlID="Panel3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_popgo" runat="server" Text="Go" OnClick="btn_popgo_Click" Style="font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" CssClass="textbox btn1" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </center>
                        <center>
                            <asp:Label ID="lbl_error" Visible="false" runat="server" ForeColor="red"></asp:Label>
                        </center>
                        <table runat="server" id="individualcumlative_table">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_collegename" runat="server" ForeColor="Green" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_incomeheader" runat="server" ForeColor="Green" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <div id="spread2div" runat="server" visible="true" style="width: 946px; height: auto;"
                                class="spreadborder">
                                <FarPoint:FpSpread ID="Fpspread2" runat="server" Width="750px" Height="1000px" VerticalScrollBarPolicy="AsNeeded"
                                    CssClass="spreadborder">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                        </center>
                        <center>
                            <div id="individualdiv" runat="server" visible="false" style="width: 946px; height: 550px;"
                                class="spreadborder">
                                <br />
                                <center>
                                    <FarPoint:FpSpread ID="Fpspread3" runat="server">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </center>
                            </div>
                        </center>
                        <br />
                        <center>
                            <div id="rptprint" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                                    Width="180px" onkeypress="display()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                                    Text="Export To Excel" Width="127px" />
                                <asp:Button ID="btnsavetax" runat="server" Text="Save" CssClass="textbox textbox1 btn2"
                                    OnClick="btnsavetax_Click" />
                                <asp:Button ID="btnprintmaster" runat="server" Visible="false" Text="Print" OnClick="btnprintmaster_Click"
                                    CssClass="textbox btn1" />
                                <%-- add by poomalar--%>
                                <asp:Button ID="btnprintcell" Visible="false" runat="server" Text="Print" CssClass="textbox textbox1 btn2"
                                    OnClick="btnprintcell_click" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                            </div>
                        </center>
                        <br />
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="alertmessage" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alerterror" Visible="false" runat="server" Text="" Style="color: Red;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox btn2 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="OK" runat="server" />
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
        <div style="height: 1px; width: 1px; overflow: auto;">
            <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
            </div>
        </div>
        <iframe id="txtArea1" style="display: none"></iframe>
    </body>
    </html>
</asp:Content>
