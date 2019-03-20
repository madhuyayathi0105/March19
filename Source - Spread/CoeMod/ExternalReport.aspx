<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ExternalReport.aspx.cs" Inherits="ExternalReport" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html>
    <%--<head runat="server">
    <title></title>
</head>--%>
    <style type="text/css">
         .ModalPopupBG
{
    background-color: #666699;   
    filter: alpha(opacity=50);
    opacity: 0.7;
}

.HellowWorldPopup
{
    min-width:600px;
    min-height:400px;
    background:white;
     .style37
      {
          position: absolute;
          left: 711px;
          top: 144px;
      }
      .style38
      {
          left: 12px;
          top: 337px;
          width: 65px;
          height: 20px;
      }
      .style39
      {
          height: 73px;
          width: 1017px;
      }
      .style42
      {
          top: 449px;
          left: 638px;
          height: 33px;
          width: 145px;
      }
      .style43
      {
          top: 204px;
          left: 188px;
          position: absolute;
          height: 21px;
          width: 126px;
          bottom: 272px;
      }
      .style44
      {
          top: 200px;
          left: 315px;
          position: absolute;
      }
      .style45
      {
          top: 206px;
          left: 376px;
          position: absolute;
      }
      .style46
      {
          top: 204px;
          left: 406px;
          position: absolute;
          height: 21px;
      }
      .style47
      {
          top: 206px;
          left: 498px;
          position: absolute;
          width: 34px;
      }
      .style48
      {
          top: 205px;
          left: 534px;
          position: absolute;
          height: 21px;
          width: 303px;
      }
      .style49
      {
          top: 103px;
          left: 690px;
          position: absolute;
          width: 48px;
      }
      .style50
      {
          top: 106px;
          left: 17px;
          position: absolute;
          height: 21px;
          width: 46px;
      }
      .style51
      {
          top: 104px;
          left: 67px;
          position: absolute;
          height: 26px;
          width: 56px;
      }
      .style52
      {
          top: 107px;
          left: 130px;
          position: absolute;
          height: 21px;
          width: 56px;
      }
      .style53
      {
          top: 105px;
          left: 191px;
          position: absolute;
      }
      .style54
      {
          top: 133px;
          left: 114px;
          position: absolute;
          width: 59px;
          height: 21px;
      }
      .style57
      {
          top: 0px;
          left: 50px;
          width: 42px;
          height: 21px;
          position: absolute;
      }
}
    
      .style42
      {}
    
      .style48
      {
          width: 185px;
      }
    
        .style49
        {
            height: 44px;
        }
    
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblxlerr').innerHTML = "";

        }
    </script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <asp:Label ID="Label1" runat="server" Text="Individual Student Mark Sheet" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
        </center>
        <br />
        <center>
            <table style="width: 700px; height: 70px; background-color: #0CA6CA;" class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            CausesValidation="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                            OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" CausesValidation="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True"
                            Width="180px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"> </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                            OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" CausesValidation="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblExamYear" runat="server" Text="ExamYear" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblExamMonth" runat="server" Text="ExamMonth" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Page" runat="server" Text="Page" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Page" runat="server" Font-Bold="True" AutoPostBack="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddl_Page_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblsubjtype" runat="server" Font-Bold="True" AutoPostBack="true" Font-Names="Book Antiqua"
                            Text="Subject Type" Font-Size="Medium" Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtsubjtype" runat="server" Font-Bold="True" AutoPostBack="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" Height="19px" OnTextChanged="txtsubjtype_TextChanged"
                            Width="105px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblarrear_sem" runat="server" Font-Bold="True" AutoPostBack="true"
                            Font-Names="Book Antiqua" Text="Arrear Sem" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtarrear_sem" runat="server" Font-Bold="True" AutoPostBack="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" Height="16px" Width="116px"></asp:TextBox>
                        <asp:Panel ID="pnlsubjtype" runat="server" CssClass="multxtpanel">
                            <asp:CheckBoxList ID="chksubjtype" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="chksubjtype_SelectedIndexChanged"
                                Width="99px" Height="20px">
                                <asp:ListItem Value="0">Regular</asp:ListItem>
                                <asp:ListItem Value="1">Arrear</asp:ListItem>
                                <%--   <asp:ListItem Value="2">Both</asp:ListItem>--%>
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <Ajax:DropDownExtender ID="ddesubjtype" runat="server" TargetControlID="txtsubjtype"
                            DropDownControlID="pnlsubjtype">
                        </Ajax:DropDownExtender>
                        <asp:Panel ID="pnlarrear_Sem" runat="server" CssClass="multxtpanel">
                            <asp:CheckBoxList ID="chkarrear_Sem" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="chkarrear_Sem_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <Ajax:DropDownExtender ID="ddearrear_sem" runat="server" TargetControlID="txtarrear_sem"
                            DropDownControlID="pnlarrear_sem">
                        </Ajax:DropDownExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblletterformat" runat="server" Text="Letter Format" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Width="110px">

                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlletterformat" runat="server" Font-Bold="True" AutoPostBack="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlletter_SelectedIndexChanged">
                            <asp:ListItem> - - -  Select - - - </asp:ListItem>
                            <asp:ListItem Value="0">Letter Format1</asp:ListItem>
                            <asp:ListItem Value="1">Letter Format2</asp:ListItem>
                            <asp:ListItem Value="2">Tamil Report</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblvisible_setting" runat="server" Text="Visible Setting" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="True" Width="120px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtvsbl_setting" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="True" Height="17px" Width="98px"></asp:TextBox>
                        <asp:Panel ID="pnlvsbl_setting" runat="server" CssClass="multxtpanel">
                            <asp:CheckBoxList ID="chkvsbl_setting" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="True" Width="106px" AutoPostBack="true" OnSelectedIndexChanged="chkvsbl_setting_SelectedIndexChanged">
                                <asp:ListItem>MinMark</asp:ListItem>
                                <asp:ListItem>MaxMark</asp:ListItem>
                                <asp:ListItem>Result</asp:ListItem>
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <Ajax:DropDownExtender ID="ddlvsbl_setting" runat="server" TargetControlID="txtvsbl_setting"
                            DropDownControlID="pnlvsbl_setting">
                        </Ajax:DropDownExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDOP" runat="server" Text="Publication Date" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="True" Width="130px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDOP" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="True" Height="24px" Width="96px"></asp:TextBox>
                        <Ajax:CalendarExtender ID="calndr1" runat="server" TargetControlID="txtDOP" Format="d/MM/yyyy">
                        </Ajax:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblDate" runat="server" Text="Date" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="True" Height="24px" Width="80px"></asp:TextBox>
                        <Ajax:CalendarExtender ID="calndr2" runat="server" TargetControlID="txtDate" Format="d/MM/yyyy">
                        </Ajax:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_subjectwisegrade" runat="server" Text="2015 Regulation" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="True" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblRegulation" runat="server" Text="Regulation" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRegulation" runat="server" Visible="False"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblGetDegree" runat="server" Text="Degree" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtGetDegree" runat="server" Visible="False"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblGetDept" runat="server" Text="Department" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDepartment" runat="server" Visible="False"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblChkCourse" runat="server" Text="CourseCode" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="Chkbxcou" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                            Visible="False" />
                    </td>
                    <td>
                        <asp:Label ID="lblCOE" runat="server" Text="COE Enrollment No" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCOE" runat="server" Visible="False"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblOutgone" runat="server" Text="OutGone" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkOutgone" runat="server" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnGo_Click" Text="Go" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <asp:CheckBox ID="chk_IncludePassedOut" runat="server" Text="Include Passedout" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chk_IncludePassedOut_OnCheckedChanged" />
                        <asp:CheckBox ID="chkincludeRoundOff" runat="server" Text="Round Off GPA/CGPA calculation"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True" />
                    </td>
                </tr>
            </table>
            <div id="divSendSMS" runat="server" visible="false" style="">
                <fieldset id="Fieldset3" runat="server" style="height: auto;">
                    <table style="width: 900px; height: auto; background-color: #0CA6CA;">
                        <tr>
                            <td>
                                <asp:Label ID="lblMode" runat="server" Text="Mode"></asp:Label>
                            </td>
                            <td>
                                <fieldset id="Fieldset1" runat="server" style="height: 10px; width: 150px;">
                                    <asp:CheckBox ID="cbViaSms" Checked="false" runat="server" Text="SMS" />
                                    <asp:CheckBox ID="cbViaEmail" Checked="false" runat="server" Text="EMAIL" />
                                </fieldset>
                            </td>
                            <td>
                                <asp:Label ID="lblSendTo" runat="server" Text="Send To"></asp:Label>
                            </td>
                            <td>
                                <fieldset id="Fieldset2" runat="server" style="height: 10px; width: 250px;">
                                    <asp:CheckBox ID="cbSendToFather" Checked="false" runat="server" Text="Father" />
                                    <asp:CheckBox ID="cbSendToMother" Checked="false" runat="server" Text="Mother" />
                                    <asp:CheckBox ID="cbSendToStudent" Checked="false" runat="server" Text="Student" />
                                </fieldset>
                            </td>
                            <td>
                                <asp:Button ID="btnsendSmsandEmail" runat="server" OnClick="btnsendSmsandEmail_OnClick"
                                    Text="Send" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </center>
        <br />
        <asp:Panel ID="pnlrecordcount" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="There is no record found" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="9">
                        <asp:Label ID="lblstudselect" runat="server" Font-Bold="True" Visible="False" Width="449px"
                            Font-Names="Book Antiqua" ForeColor="#FF3300" Text="Please Select Atleast One Student To Print The GradeSheet"
                            Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <asp:Panel ID="pnlSpread" runat="server">
            <FarPoint:FpSpread ID="FpExternal" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Width="900px" HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="AsNeeded">
                <CommandBar ShowPDFButton="false" ButtonType="PushButton" Visible="true">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" AllowSort="false" GridLineColor="Black" BackColor="White">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </asp:Panel>
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Panel ID="Panel5" runat="server" Visible="false" Style="left: -147px; border-color: Gray;
            border-style: solid; width: 1050px; height: 71px; margin-bottom: 0px; margin-right: 212px;
            margin-left: -6px; margin-top: -10px;">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lbl_selectall" runat="server" Text="Select All" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_select_all" runat="server" AutoPostBack="true" OnCheckedChanged="chk_select_all_CheckedChanged" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_hideall" runat="server" Text="Hide Select Column" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:Label>
                        <asp:CheckBox ID="chk_hide_all" runat="server" AutoPostBack="true" OnCheckedChanged="chk_hide_all_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td class="style49">
                        <asp:RadioButton ID="rdMark" runat="server" Text="Mark " TextAlign="Right" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="margin-left: 0px; margin-bottom: 16px;"
                            OnCheckedChanged="rdMark_CheckedChanged" GroupName="MrkGradeSheet" Visible="False"
                            CssClass="style38" />
                    </td>
                    <td class="style49">
                        <asp:RadioButton ID="rdGrade" runat="server" Text="Grade" TextAlign="Right" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="margin-left: 0px; margin-bottom: 16px;
                            top: 556px; left: 450px; width: 68px; height: 19px;" GroupName="MrkGradeSheet"
                            OnCheckedChanged="rdGrade_CheckedChanged" Visible="False" />
                    </td>
                    <td class="style49">
                        <asp:Button ID="btnLoad" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnLoad_Click" Text="Load The Grade Sheet for Individual Student"
                            Style="top: 553px; left: 555px; height: 25px; width: 382px" Visible="False" />
                    </td>
                    <td class="style49">
                        &nbsp; &nbsp; &nbsp;<asp:Button ID="btnLetterFormat" runat="server" Font-Bold="true"
                            Font-Size="Medium" Text="Letter Format" Font-Names="Book Antiqua" OnClick="btnLetterFormat_Click"
                            Visible="False" CssClass="style42" Height="25px" />
                    </td>
                    <td>
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="tamilbutton" runat="server" Font-Bold="true" Font-Size="Medium" Text="Tamil Report"
                            Font-Names="Book Antiqua" Height="25px" CssClass="style42" OnClick="tamilbutton_Click"
                            Visible="False" />
                    </td>
                    <td>
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="btnletterformat1" runat="server" Font-Bold="true" Font-Size="Medium"
                            Text="Letter Format1" Font-Names="Book Antiqua" Height="25px" CssClass="style42"
                            OnClick="btnLetterformat1_Click" Visible="False" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Button ID="btnPrint" runat="server" Font-Bold="true" Font-Size="Medium" Text="Print The GradeSheet"
            Font-Names="Book Antiqua" OnClick="btnPrint_Click" Style="top: 586px; left: 619px;
            height: 28px; width: 193px" Visible="False" />
        <asp:HiddenField ID="hiddentamil_rpt" runat="server" />
        <Ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="hiddentamil_rpt"
            CancelControlID="btnClose" PopupControlID="Panel4" PopupDragHandleControlID="PopupHeader"
            Drag="true" BackgroundCssClass="ModalPopupBG">
        </Ajax:ModalPopupExtender>
        <asp:Panel ID="Panel4" runat="server" Width="880px" Height="500px" ScrollBars="Auto"
            BorderColor="Black" BorderStyle="Double" Style="display: none; height: 500; width: 880;">
            <div class="HellowWorldPopup">
                <div class="PopupHeader" id="Div2" style="text-align: center; color: Blue; font-family: Book Antiqua;
                    font-size: xx-large; font-weight: bold">
                </div>
                <div class="PopupBody">
                </div>
                <div class="Controls">
                    <div id="divMarkSheet" runat="server">
                        <center>
                            <FarPoint:FpSpread ID="FpMarkSheet" runat="server" BorderColor="White" BorderStyle="Solid"
                                BorderWidth="0" Height="800" Visible="false" HorizontalScrollBarPolicy="Never"
                                VerticalScrollBarPolicy="Never">
                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark" ButtonType="PushButton">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" GridLineColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </center>
                    </div>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnClose" runat="server" Text="Close" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="panelchech" ScrollBars="None" Width="400px" Height="100px"
            BackColor="AliceBlue" BorderColor="Black" BorderStyle="Double" Style="top: 285px;
            left: 368px; position: absolute">
            <table>
                <tr>
                    <td>
                        <asp:RadioButton runat="server" ID="radiobtn1" Text="Without Remarks" GroupName="ds"
                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" Checked="true" />
                    </td>
                    <td>
                        <asp:RadioButton runat="server" ID="radiobtn2" Text="With Remarks" GroupName="ds"
                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton runat="server" ID="rad_noheader" Text="Without Header" GroupName="ds1"
                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" Checked="true" />
                    </td>
                    <td>
                        <asp:RadioButton runat="server" ID="rad_header" Text="With Header" GroupName="ds1"
                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" Checked="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button runat="server" ID="btnpop" Text="OK" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnClick="btnpop_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <Ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="HiddenField1"
            CancelControlID="Button2" PopupControlID="Panel3" PopupDragHandleControlID="PopupHeader"
            Drag="true" BackgroundCssClass="ModalPopupBG">
        </Ajax:ModalPopupExtender>
        <asp:Panel ID="Panel3" runat="server" Width="1000px" Height="500px" ScrollBars="Auto"
            BorderColor="Black" BorderStyle="Double" Style="display: none; height: 500; width: 880;">
            <div class="HellowWorldPopup">
                <div class="PopupHeader" id="Div1" style="text-align: center; color: Blue; font-family: Book Antiqua;
                    font-size: xx-large; font-weight: bold">
                </div>
                <div class="PopupBody">
                </div>
                <div class="Controls">
                    <table>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlPage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageNumberSelected">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <FarPoint:FpSpread ID="sprdLetterFormat" runat="server" BorderColor="Black" BorderStyle="None"
                            HorizontalScrollBarPolicy="Never" VerticalScrollBarPolicy="Never" Width="1000"
                            Height="600px">
                            <CommandBar ButtonType="PushButton">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <br />
                    <asp:Button ID="Button2" runat="server" Text="Close" />
                </div>
            </div>
        </asp:Panel>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblxl" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtxlname" runat="server" onkeypress="display()" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnxl_Click" />
                    <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtxlname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </Ajax:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblxlerr" Text="Please Enter Your Report Name" runat="server" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </body>
    </html>
</asp:Content>
