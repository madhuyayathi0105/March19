<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ExamValuationSettings.aspx.cs" Inherits="ExamValuationSettion"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .fontbold
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        
        .fontnormal
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <center>
        <div class="maindivstyle">
            <span style="color: Green; font-size: large;" class=" fontstyleheader">Exam Valuation
                Settings</span>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="Label20" runat="server" Text="Year and Month"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="textbox ddlheight" Width="90px"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="textbox ddlheight" Width="90px"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Staff"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlstaream" runat="server" CssClass="textbox ddlheight" Width="101px"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlstaream_SelectedIndexChanged">
                            <asp:ListItem Text="Internal" Value="0"></asp:ListItem>
                            <asp:ListItem Text="External" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox ddlheight" Width="90px"
                            AutoPostBack="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Department"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldepat" runat="server" CssClass="textbox ddlheight" Width="200px"
                            AutoPostBack="True" OnSelectedIndexChanged="ddldepat_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Text="GO" CssClass="textbox btn" OnClick="btngo_Click" />
                        <asp:Button ID="btnaddsubject" runat="server" CssClass="textbox btn1" Text="Add"
                            OnClick="btnaddsubject_click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="9">
                        <div id="NewDiv" runat="server" visible="false">
                            <asp:RadioButton ID="rdbValuation" runat="server" Text="Valuation" GroupName="same"
                                AutoPostBack="true" OnCheckedChanged="rdbValuation_Change" />
                            <asp:RadioButton ID="rdbQsetter" runat="server" Text="Question Paper Setter" GroupName="same"
                                AutoPostBack="true" OnCheckedChanged="rdbQsetter_Change" />
                            <asp:CheckBox ID="chkSendEMail" runat="server" Text="Send EMail" Checked="false" />
                            <asp:CheckBox ID="cbsendSMS" runat="server" Text="Send SMS" Checked="false" 
                                oncheckedchanged="cbsendSMS_CheckedChanged" />
                             <asp:Button ID="btnsendsms" runat="server" Text="Send SMS" CssClass=" textbox btn"
                        Width="120px" OnClick="btnsendsms_click" />
                        </div>
                    </td>
                </tr>
            </table>
            <div>
                <br />
                <asp:Label ID="lblmsg" runat="server" CssClass="fontbold" ForeColor="Red"></asp:Label>
                <br />
                <FarPoint:FpSpread ID="FpValuation" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Visible="false" OnButtonCommand="FpValuation_ButtonCommand"
                    CssClass="stylefp">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <div id="ValidationDiv" runat="server" visible="false">
                    <asp:Label ID="lblvaldate" runat="server" CssClass="fontbold" Text="Valuation Last Date "></asp:Label>
                    <asp:TextBox ID="txtvaldate" runat="server" Width="80px" CssClass="fontbold"></asp:TextBox>
                    <asp:CalendarExtender ID="cetxtExamFinishDate" runat="server" TargetControlID="txtvaldate"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:Label ID="lblvaltodate" runat="server" CssClass="fontbold" Text="Valuation To Date "></asp:Label>
                    <asp:TextBox ID="txtvaltodate" runat="server" Width="80px" CssClass="fontbold"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtvaltodate"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:CheckBox ID="chkvalsummry" runat="server" CssClass="fontbold" Text="Valuation Summary" />
                    <asp:Button ID="btnvaluationletter" runat="server" Visible="false" Text="Valuation Letter Generate"
                        CssClass="textbox btn" Width="220px" OnClick="btnvaluationletter_click" />
                    <asp:Button ID="btnsubdelete" runat="server" Visible="false" Text="Delete Subject"
                        CssClass="textbox btn" Width="120px" OnClick="btnsubdelete_click" />
                </div>
                <div id="QuestionDiv" runat="server" visible="false">
                    <asp:Label ID="lblQPBefore" runat="server" CssClass="fontbold" Text="Question Paper Send Before "></asp:Label>
                    <asp:TextBox ID="txtQpBefore" runat="server" CssClass=" textbox" Width="80px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtQpBefore"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:CheckBox ID="cbQValSummary" runat="server" CssClass="fontbold" Text="Valuation Summary" />
                    <asp:Button ID="btnGeneratePdf" runat="server" Text="Question Paper Letter Generate"
                        CssClass=" textbox btn" Width="320px" OnClick="btnGeneratePdf_click" />
                    <asp:Button ID="btnDeleteSubjectValue" runat="server" Text="Delete Subject" CssClass=" textbox btn"
                        Width="120px" OnClick="btnDeleteSubjectValue_click" />
                       
                </div>
                <br />
                <asp:Label ID="lblexcsea" runat="server" Visible="false" Text="Report Name" CssClass="fontbold"></asp:Label>
                <asp:TextBox ID="txtexseat" runat="server" Height="20px" Style="font-family: 'Book Antiqua'"
                    CssClass=" textbox" Width="180px" Visible="false" onkeypress="display()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexseat"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="Excel_seating" runat="server" Visible="false" Text="Export Excel"
                    CssClass=" textbox btn" Width="120px" OnClick="Excelseating_click" />
                <asp:Button ID="Print_seating" runat="server" CssClass=" textbox btn" Width="80px"
                    Visible="false" Text="Print" OnClick="printseating_click" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                <div id="Panelsubject" runat="server" style="height: 100%; z-index: 1000; width: 100%;
                    background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <div class="PopupHeaderrstud2" id="Div3" class="table" style="background-color: White;
                        height: 250px; width: 800px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 150px; border-radius: 10px;">
                        <table style="text-align: left">
                            <tr>
                                <td colspan="2">
                                    <center>
                                        <span style="font-size: large; color: Green;" class="fontstyleheader">Valuation Paper
                                            Add</span></center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblssem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Subject Category"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsubcatrory" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="true" Width="600px" OnSelectedIndexChanged="ddlsubcatrory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Subject Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="600px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Staff"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlvalstaff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="600px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="textbox btn" Width="80px"
                            OnClick="btnsave_Click" />
                        <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="textbox btn" Width="80px"
                            OnClick="btndelete_Click" />
                        <asp:Button ID="btnexit" runat="server" Text="Exit" CssClass="textbox btn" Width="80px"
                            OnClick="btnexit_Click" />
                        <br />
                        <asp:Label ID="lblserror" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </center>
</asp:Content>
