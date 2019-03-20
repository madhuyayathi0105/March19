<%@ Page Title="Exam Application" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Exam Application.aspx.cs" Inherits="Exam_Application" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
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
    <style>
        .fontblack
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: Black;
        }
        .fontcolorb
        {
            color: Green;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <asp:Label ID="Label4" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                position: relative;" Text="Exam Application" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
        </div>
        <div style="color: White; font-family: Book Antiqua; height: auto; width: 1070px;
            margin: 0px; margin-top: 15px; margin-bottom: 15px; position: relative; text-align: left;"
            class="maintablestyle">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="70Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSec" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="65Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                     
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkdiscontine" runat="server" Text="Include Discontinue" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkpassout" runat="server" Text="Include Passed Out" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkExternalMark" runat="server" Text="Check External Pass" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td style="border: 1px solid rgb(0, 124, 124); padding: 3px;">
                        <asp:CheckBox ID="chkIsSupplementaryExam" runat="server" Checked="false" Text="Supplymentary Exam"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                     <td>
                        <asp:CheckBox ID="chksemwise" runat="server" Text="FastTrack System" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="chksrmwise_OnCheckedChanged"  />
                    </td>
                    <td>
                        <asp:Button ID="Buttongo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="Buttongo_Click" Text="Go" />
                    </td>
                     <td >
                        <asp:Label ID="lblSem1" runat="server" CssClass="commonHeaderFont" Text="Semester"
                            AssociatedControlID="txtSem"  Visible="false"></asp:Label>
                            </td>
                            <td >
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlsem" runat="server" Visible="false">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtSem" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlsem" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="100px">
                                        <asp:CheckBox ID="chksem"  runat="server" Text="Select All" ForeColor="Black"
                                            AutoPostBack="True" OnCheckedChanged="chkchksem_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblsem" ForeColor="Black" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblsem_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtsem" runat="server" TargetControlID="txtSem"
                                        PopupControlID="pnlsem" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblexamyear" runat="server" Text="Exam Year" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblexammonth" runat="server" Text="Exam Month" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="true" Width="90px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblappldate" runat="server" Text="Applied Date" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtappldate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="80px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" Format="d/MM/yyyy" TargetControlID="txtappldate"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblappllatstdate" runat="server" Text="Last Date For Application"
                            Font-Bold="true" Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtappllastdate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="80px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" Format="d/MM/yyyy" TargetControlID="txtappllastdate"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblapplfee" runat="server" Text="Cost of Application" Font-Bold="true"
                            Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtapplfee" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="80px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtapplfee"
                            FilterType="Numbers,Custom" ValidChars=".">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblstsmtfee" runat="server" Text="Cost of StMt Marks" Font-Bold="true"
                            Font-Names="Book Antiqua" ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtstmtfee" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="80px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtstmtfee"
                            FilterType="Numbers,Custom" ValidChars=".">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblfine" runat="server" Text="Fine Amount" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfine" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="80px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtfine"
                            FilterType="Numbers,Custom" ValidChars=".">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblextrfee" runat="server" Text="Extra Fee" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txtextrafeename" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="180px" placeholder="Fee Name"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtextrafee" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="80px" placeholder="Fee Amount"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtextrafee"
                            FilterType="Numbers,Custom" ValidChars=".">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblextrafee2" runat="server" Text="Extra Fee 2" Font-Bold="true" Font-Names="Book Antiqua"
                            ForeColor="White" Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txtextrafeename2" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="180px" placeholder="Fee Name"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtextrafee2" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="80px" placeholder="Fee Amount"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtextrafee2"
                            FilterType="Numbers,Custom" ValidChars=".">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnapply" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnapply_Click" Text="Apply" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkheadimage" runat="server" Text="Header Image" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rbFormat1" runat="server" Text="Format1" Checked="true" GroupName="Application"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rbFormat2" runat="server" Text="Format2" GroupName="Application"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rbFormat3" runat="server" Text="Format3" GroupName="Application"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rbFormat4" runat="server" Text="Format4" GroupName="Application"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rbFormat5" runat="server" Text="Format5" GroupName="Application"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnapplpaper" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnapplpaper_Click" Text="Applicable Paper" />
                    </td>
                    <td>
                        <asp:Button ID="btnprint" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnprint_Click" Text="Print Exam Application" />
                        <asp:Button ID="printbtn" runat="server" Text="LIST OF EXAMINATION FEES(TO BE PAID)"
                            Style="margin-left: 10px" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="printbtn_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btndelete" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btndelete_Click" Text="Delete Exam Application" />
                    </td>
                   
                </tr>
                <tr>
                    <td colspan="3">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSetFees" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnSetFees_Click" Text="Fee Setting" />
                                </td>
                                <td>
                                    <asp:Button ID="btnViewSubject" runat="server" Text="View Subject" Style="margin-left: 10px"
                                        Font-Bold="True" Visible="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnClick="btnViewSubject_Click" ToolTip="To View Subjects Select Only One Student" />
                                </td>
                                 <td>
                                    <asp:Button ID="btnfasttrackSub" runat="server" Text="View Future Subject" Style="margin-left: 10px"
                                        Font-Bold="True" Visible="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnClick="btnfasttrackSub_Click" ToolTip="To View Subjects for All Student" />
                                </td>
                                <td>
                                    <asp:Button ID="btnExamSessionSettings" runat="server" Text="Exam Session Settings"
                                        Style="margin-left: 10px" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnExamSessionSettings_Click" ToolTip="Exam Session Settings" />
                                </td>
                               <%-- rajkumar on 28/5/2016--%>
                                 <td>
                                    <asp:Button ID="btndeleteAll" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btndeleteAll_Click" Text="Delete All" />
                                </td>
                                <td>
                                <asp:Button ID="btnformatsettings" runat="server" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnformatsetting_Click" Text="Format Setting" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <asp:Label ID="lblerror" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
        Font-Bold="true" ForeColor="Red" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
        position: relative;"></asp:Label>
    <center>
        <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="250px" Width="400px" ActiveSheetViewIndex="0"
            currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
            EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5"
            OnUpdateCommand="FpSpread1_UpdateCommand" Style="margin: 0px; margin-top: 15px;
            margin-bottom: 15px; position: relative;">
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
    </center>
    <center>
        <div id="showappicable" runat="server" visible="false" style="height: 500%; z-index: 1000;
            width: 100%; background-color: rgba(0, 0, 0, 0.72); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: white; border-image: none;
                    border-radius: 10px; border-width: 25px 5px 5px; height: auto; margin-top: 52px;
                    width: 865px;">
                    <asp:LinkButton ID="linkclose" runat="server" Text="X" CssClass="fontblack" OnClick="linkcloseclick"
                        Style="bottom: 2px; color: Red; font-family: Comic Sans MS; font-size: x-large;
                        left: 365px; position: relative; text-decoration: none;"></asp:LinkButton>
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <span class="fontblack">Roll No </span>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtrollpop" CssClass="fontblack"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" TargetControlID="txtrollpop"
                                        FilterType="LowercaseLetters,UppercaseLetters,numbers " runat="server">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <span class="fontblack">Reg No</span>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtregnopop" CssClass="fontblack"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtregnopop"
                                        FilterType="LowercaseLetters,UppercaseLetters,numbers " runat="server">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnsearch" runat="server" Text="Go" CssClass="fontblack" OnClick="btnsearch_click" />
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblerrpop" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"
                            Width="800px" Font-Bold="true" ForeColor="Red"></asp:Label>
                        <br />
                        <div id="showdetails" runat="server" visible="false">
                            <table>
                                <tr>
                                    <td style="width: 150px;">
                                        <span class="fontblack">Student Name </span>
                                    </td>
                                    <td style="width: 20px;">
                                        <span class="fontblack">: </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblstudnamepop" runat="server" Text="" CssClass="fontblack fontcolorb"></asp:Label>
                                    </td>
                                    <td style="width: 100px;">
                                        <span class="fontblack">Batch Year </span>
                                    </td>
                                    <td style="width: 20px;">
                                        <span class="fontblack">: </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbatchyearpop" runat="server" Text="" CssClass="fontblack fontcolorb"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="fontblack">Degree </span>
                                    </td>
                                    <td>
                                        <span class="fontblack">: </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldegreepop" runat="server" Text="" CssClass="fontblack fontcolorb"></asp:Label>
                                    </td>
                                    <td>
                                        <span class="fontblack">Branch </span>
                                    </td>
                                    <td>
                                        <span class="fontblack">: </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbranchpop" runat="server" Text="" CssClass="fontblack fontcolorb"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="fontblack">Exam Moth / Year </span>
                                    </td>
                                    <td>
                                        <span class="fontblack">: </span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblexam_m_ypop" runat="server" Text="" CssClass="fontblack fontcolorb"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <center>
                                            <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                ShowHeaderSelection="false" BorderWidth="1px" Height="350" Width="750" HorizontalScrollBarPolicy="Never">
                                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                    ButtonShadowColor="ControlDark" ShowPDFButton="false">
                                                </CommandBar>
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                            <div id="rptprint" runat="server" visible="true">
                                                <asp:Button ID="btnsave" runat="server" Text="Save Exam Application" OnClick="btnsave_Click"
                                                    CssClass="fontblack" />
                                                <br />
                                                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="" CssClass="fontblack"
                                                    Visible="true"></asp:Label>
                                                <br />
                                                <asp:Label ID="lblrptname" runat="server" CssClass="fontblack" Text="Report Name"></asp:Label>
                                                <asp:TextBox ID="txtexcelname" runat="server" CssClass="fontblack" onkeypress="display12()"></asp:TextBox>
                                                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="fontblack"
                                                    OnClientClick="return checktxt()" Text="Export To Excel" Width="130px" />
                                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                                    CssClass="fontblack" />
                                                <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
                                            </div>
                                            <br />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Pop Set Mandatory Fees--%>
    <center>
        <div id="divMandFee" runat="server" visible="false" style="height: 100em; z-index: 100000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div1" runat="server" class="table" style="background-color: White; height: 481px;
                    width: 500px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 75px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td>
                                    <center>
                                        <b style="font-size: 20px; color: Red;">Mandatory Fees </b>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkInclFinMand" runat="Server" Text="Include Finance" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gridMandFee" runat="server" Height="200px" Width="400px" AutoGenerateColumns="false"
                                        OnDataBound="gridMandFee_OnDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label></center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fee" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFeeName" runat="server" Text='<%#Eval("FeeName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cb_SelFee" runat="server"></asp:CheckBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_SaveMandfee" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btn_SaveMandfee_Click" Text="Save" runat="server" />
                                        <asp:Button ID="btn_ResetMandFee" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btn_ResetMandFee_Click" Text="Reset" runat="server" />
                                        <asp:Button ID="btn_CloseMandFee" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btn_CloseMandFee_Click" Text="Close" runat="server" />
                                        <asp:CheckBox ID="chkSelAllMand" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="chkSelAllMand_OnCheckChange" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- FN/AN Settings --%>
    <center>
        <div id="divSessionSettings" runat="server" visible="false" style="height: 100em;
            z-index: 100000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
            top: 0; left: 0px;">
            <center>
                <div id="divTiming" runat="server" class="table" style="background-color: White;
                    height: 200px; width: 500px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 75px; border-radius: 10px;">
                    <center>
                        <center>
                            <span><b style="font-size: 20px; color: Green;">Exam Session Settings</b></span>
                        </center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <%-- <td>
                                        <asp:Label ID="lblSession" runat="server" Text="Exam Session"></asp:Label>
                                    </td>--%>
                                <td colspan="8" align="center">
                                    <asp:RadioButtonList ID="rblSessions" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblSessions_SelectedIndexChanged">
                                        <asp:ListItem Text="FN" Selected="True" Value="0">
                                        </asp:ListItem>
                                        <asp:ListItem Text="AN" Selected="False" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFromSession" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server" Text="From Time"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFHr" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFMin" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFSess" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server">
                                        <asp:ListItem Text="AM" Selected="True" Value="0">
                                        </asp:ListItem>
                                        <asp:ListItem Text="PM" Selected="False" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblToSession" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server" Text="To Time"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTHr" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTMin" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTSession" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server">
                                        <asp:ListItem Text="AM" Selected="True" Value="0">
                                        </asp:ListItem>
                                        <asp:ListItem Text="PM" Selected="False" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="8">
                                    <asp:Button ID="btnSesstionSave" CssClass=" textbox btn1 textbox1" Height="30px"
                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" runat="server"
                                        Text="Save Session" OnClick="btnSesstionSave_Click" />
                                    <asp:Button ID="btnSessionExit" CssClass=" textbox btn1 textbox1" Font-Bold="true"
                                        Font-Names="Book Antiqua" Height="30px" Font-Size="Medium" runat="server" Text="Exit"
                                        OnClick="btnSessionExit_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- View Subjects --%>
    <center>
        <div id="divViewSubjects" runat="server" visible="false" style="height: 400em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divpnlViewSubjects" runat="server" class="table" style="background-color: White;
                    height: auto; width: 900px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <span style="font-family: Book Antiqua; font-size: 20px; font-weight: bold; color: Green;
                            margin: 0px; margin-bottom: 20px; margin-top: 15px; position: relative;">Apply Subject</span>
                    </center>
                    <center>
                        <div style="width: 900px; height: auto; overflow: auto; margin: 0px; margin-bottom: 10px;
                            margin-top: 20px;">
                            <center>
                                <FarPoint:FpSpread ID="FpSpreadViewSubjects" runat="server" Height="250px" Width="400px"
                                    ActiveSheetViewIndex="0" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </center>
                        </div>
                        <asp:Label ID="lblViewSubjectError" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="false" ForeColor="Red" Style="margin: 0px; margin-bottom: 20px;
                            margin-top: 10px; position: relative;"></asp:Label>
                        <div style="margin: 0px; margin-bottom: 20px; margin-top: 10px; position: relative;">
                            <center>
                                <asp:Button ID="btnViewSaveApplication" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 28px; width: 250px;" OnClick="btnViewSaveApplication_Click"
                                    Text="Save Exam Application" runat="server" />
                                <asp:Button ID="btnViewSubjects_exit" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 28px; width: 65px;" OnClick="btnViewSubjects_exit_Clcik"
                                    Text="Exit" runat="server" />
                                <asp:CheckBox ID="chkSelAllSub" runat="server" AutoPostBack="true" Text="Select All"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkSelAllSub_CheckedChanged" />
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </div>
    </center>
     <center>
        <div id="div3" runat="server" visible="false" style="height: 400em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="div4" runat="server" class="table" style="background-color: White;
                    height: auto; width: 900px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <span style="font-family: Book Antiqua; font-size: 20px; font-weight: bold; color: Green;
                            margin: 0px; margin-bottom: 20px; margin-top: 15px; position: relative;">Apply Subject</span>
                    </center>
                    <center>
                        <div style="width: 900px; height: auto; overflow: auto; margin: 0px; margin-bottom: 10px;
                            margin-top: 20px;">
                            <center>
                                <FarPoint:FpSpread ID="FpSpread3" runat="server" Height="250px" Width="420px"
                                    ActiveSheetViewIndex="0" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </center>
                        </div>
                        <asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="false" ForeColor="Red" Style="margin: 0px; margin-bottom: 20px;
                            margin-top: 10px; position: relative;"></asp:Label>
                        <div style="margin: 0px; margin-bottom: 20px; margin-top: 10px; position: relative;">
                            <center>
                            <asp:Button ID="btnFutSave" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 28px; width: 65px;" OnClick="btnFutSave_Clcik"
                                    Text="Save" runat="server" />
                                <asp:Button ID="btnExit" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 28px; width: 65px;" OnClick="btnExit_exit_Clcik"
                                    Text="Exit" runat="server" />
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Pop Alert--%>
    <center>
        <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 100000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
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
                                        <asp:Button ID="btn_alertclose" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btn_alertclose_Click" Text="Ok" runat="server" />
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

    <center>
     <div id="divformatsettings" runat="server" visible="false" style="height: 400em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divformatset" runat="server" class="table" style="background-color: White;
                    height: auto; width: 900px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <span style="font-family: Book Antiqua; font-size: 20px; font-weight: bold; color: Green;
                            margin: 0px; margin-bottom: 20px; margin-top: 15px; position: relative;">Settings</span>
                    </center>
                     
                   <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                        position: relative; width: 900px; height: auto;">
                        <tr>
                        <td>
                        <asp:Label ID="lblsignature1" runat="server" Text="Signature1" style="margin-left:10px"></asp:Label>                     
                        <asp:TextBox ID="txtsignature1" runat="server" Style="margin-left: 11px;
                                    width: 290px"></asp:TextBox>
                       
                        <asp:Label ID="lblsignature2" runat="server" Text="Signature2" style="margin-left:51px"></asp:Label>
                      
                        <asp:TextBox ID="txtsignature2" runat="server" Style="margin-left: 10px;
                                    width: 290px" ></asp:TextBox>
                        </td>

                        </tr>
                        <tr>
                        <td>
                        <asp:Label ID="lblsignature3" runat="server" Text="Signature3" style="margin-left:10px"></asp:Label>
                       
                        <asp:TextBox ID="txtsignature3" runat="server" style="margin-left:11px; width:290px" ></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                        <td>
                         <asp:Label ID="lblnote" runat="server" Text="Note" style="margin-left:10px"></asp:Label>
                          <asp:TextBox ID="txtnote" runat="server" Width="514px" Height="160px" style="margin-left:53px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        </tr>
                        <tr align="center">
                        <td>
                      <asp:Button ID="btnsaveformatsetting" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnsaveformsetting_Click" Text="Save" />
                              <asp:Button ID="btnexit1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnexit1_Click" Text="Exit" />
                        </td>
                        </tr>
                     
                       </table>
                        </div>
                        </center>
    
    </div>
    </center>
</asp:Content>
