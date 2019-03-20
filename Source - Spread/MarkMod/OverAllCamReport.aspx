<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master" AutoEventWireup="true" CodeFile="OverAllCamReport.aspx.cs" Inherits="OverAllCamReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <body>
        <script type="text/javascript">

            function display() {
                document.getElementById('MainContent_lblmsg').innerHTML = "";
            }
            function fnc(value, min, max) {
                if (parseInt(value) < 0 || isNaN(value))
                    return 0;
                else if (parseInt(value) > 100)
                    return 0;
                else return value;
            }

        </script>
     
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="20px"
                Style="width: 1110px">
                <center>
                <asp:Label ID="Label1" runat="server" Text="CR18-Over All Cam Report" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White"></asp:Label>
              </center>
            </asp:Panel>
        </div>
        <asp:Panel ID="Panel1" runat="server" Height="121px" BackColor="Lightblue" BorderColor="Black"
            ClientIDMode="Static" Width="1110px" Style="margin-bottom: 0px; border: 1px solid #000;
             ">
            <table>
            <tr>
            <td>
            <asp:Label ID="Label4" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Style=" height: 18px;
                width: 44px"></asp:Label>
                </td>
                <td>
            <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" AutoPostBack="True"
                Style="">
            </asp:DropDownList>
             </td>
                <td>
            <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Style=" height: 18px;
                width: 44px"></asp:Label>
                 </td>
                <td>
            <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" AutoPostBack="true" Width="80px" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                Style="">
            </asp:DropDownList>
            <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Style=" height: 18px;
                width: 44px"></asp:Label>
                 </td>
                <td>
            <asp:TextBox ID="txtdegree" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                Width="100px" Style=" font-family: 'Book Antiqua';"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
            <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Height="250px">
                <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" OnCheckedChanged="chkdegree_ChekedChange"
                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                <asp:CheckBoxList ID="chklsdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsdegree_SelectedIndexChanged">
                </asp:CheckBoxList>
            </asp:Panel>
            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                PopupControlID="pdegree" Position="Bottom">
            </asp:PopupControlExtender>
             </td>
                <td>
            <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Style=" height: 18px;
                width: 44px"></asp:Label>
                 </td>
                <td>
            <asp:TextBox ID="txtbranch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                Width="100px" Style=" font-family: 'Book Antiqua';"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
            <asp:Panel ID="pbranch" runat="server" CssClass="multxtpanel" Height="250px">
                <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" OnCheckedChanged="chkbranch_ChekedChange"
                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                <asp:CheckBoxList ID="chklsbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsbranch_SelectedIndexChanged">
                </asp:CheckBoxList>
            </asp:Panel>
            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                PopupControlID="pbranch" Position="Bottom">
            </asp:PopupControlExtender>
             </td>
                <td>
            <asp:Label ID="lblsemster" runat="server" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Style="height: 18px;
                width: 44px"></asp:Label>
                 </td>
                <td>
            <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="51px" AutoPostBack="True"
                Style="">
            </asp:DropDownList>
             </td>
                <td>
            <asp:Label ID="lblsec" runat="server" Text="Section" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Style=" height: 18px;
                width: 44px"></asp:Label> </td>
                <td>
            <asp:DropDownList ID="ddlsec" runat="server" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="51px" AutoPostBack="True"
                Style="">
            </asp:DropDownList>
            </td>
            </tr>
            <tr>
            
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Criteria" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style=" height: 15px;
                        width: 46px">
                    </asp:Label> </td>
                <td>
                    <asp:TextBox ID="txtcriteria" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" CssClass="Dropdown_Txt_Box" Style=" height: 17px; width: 148px;"></asp:TextBox>
                    <asp:Panel ID="pcriteria" runat="server" CssClass="multxtpanel" Height="250px">
                        <asp:CheckBox ID="chkcriteria" runat="server" AutoPostBack="True" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkcriteria_CheckedChanged"
                            Text="Select All" />
                        <asp:CheckBoxList ID="chklscriteria" runat="server" OnSelectedIndexChanged="chklscriteria_SelectedIndexChanged"
                            AutoPostBack="true" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium">
                            <asp:ListItem Value="0">Rank</asp:ListItem>
                            <%--    <asp:ListItem Value="1">Medium</asp:ListItem>
                            <asp:ListItem Value="2">12th Info</asp:ListItem>
                            <asp:ListItem Value="3">CGPA</asp:ListItem>--%>
                            <%--  <asp:ListItem Value="4">Class Strength</asp:ListItem>--%>
                            <%-- <asp:ListItem Value="5">Students Appeared</asp:ListItem>--%>
                            <asp:ListItem Value="1">Students Absent</asp:ListItem>
                            <asp:ListItem Value="2">Students Passed</asp:ListItem>
                            <asp:ListItem Value="3">Students Failed</asp:ListItem>
                            <asp:ListItem Value="4">Average(&lt;50)</asp:ListItem>
                            <asp:ListItem Value="5">Average(50to65)</asp:ListItem>
                            <asp:ListItem Value="6">Average(&gt;65)</asp:ListItem>
                            <%--  <asp:ListItem Value="12">Class Average</asp:ListItem>
                            <asp:ListItem Value="13">Class Max-Min Mark</asp:ListItem>--%>
                            <asp:ListItem Value="7">Pass Percentage</asp:ListItem>
                            <%--    <asp:ListItem Value="15">Staff Name</asp:ListItem>--%>
                            <asp:ListItem Value="8">DayScholarPass</asp:ListItem>
                            <asp:ListItem Value="9">HostlerPass</asp:ListItem>
                            <asp:ListItem Value="10">TamMediumPass</asp:ListItem>
                            <asp:ListItem Value="11">EngMediumPass</asp:ListItem>
                            <asp:ListItem Value="12">Exam Date</asp:ListItem>
                            <asp:ListItem Value="13">GirlsPass</asp:ListItem>
                            <asp:ListItem Value="14">BoysPass</asp:ListItem>
                            <%--   <asp:ListItem Value="23">Quota</asp:ListItem>
                            <asp:ListItem Value="24">NFPS</asp:ListItem>
                            <asp:ListItem Value="25">NoOfHrAttended</asp:ListItem>--%>
                            <asp:ListItem Value="15">Attendance %</asp:ListItem>
                            <asp:ListItem Value="16">Average(&gt;=75) %</asp:ListItem>
                            <asp:ListItem Value="17">Average(60to74)</asp:ListItem>
                            <asp:ListItem Value="18">Average(50to59)</asp:ListItem>
                            <asp:ListItem Value="19">Average(30to49)</asp:ListItem>
                            <asp:ListItem Value="20">Average(20to29)</asp:ListItem>
                            <asp:ListItem Value="21">Average(&lt;=19)</asp:ListItem>
                            <%-- <asp:ListItem Value="33">Maxmark rollno</asp:ListItem>--%>
                            <%--   <asp:ListItem Value="35">Conducted Hours</asp:ListItem>--%>
                            <asp:ListItem Value="22">Subjects Failed</asp:ListItem>
                            <asp:ListItem Value="23">Average(&gt;60)</asp:ListItem>
                            <asp:ListItem Value="24">Average(&gt;80)</asp:ListItem>
                            <asp:ListItem Value="25">No of all Cleared</asp:ListItem>
                            <asp:ListItem Value="26">% of all Cleared</asp:ListItem>
                            <%--  <asp:ListItem Value="27">Grade</asp:ListItem>--%>
                            <asp:ListItem Value="27">No of Failures</asp:ListItem>
                            <asp:ListItem Value="28">No of Subject Absent</asp:ListItem>
                            <asp:ListItem Value="29">Subject Average</asp:ListItem>
                            <asp:ListItem Value="30">Staff Name</asp:ListItem>
                            <asp:ListItem Value="31">Student Present</asp:ListItem>
                            <asp:ListItem Value="32">Student OD</asp:ListItem>
                            <asp:ListItem Value="33">Total Students</asp:ListItem>
                            <%-- <asp:ListItem Value="29">Exam Date</asp:ListItem>--%>
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtcriteria"
                        PopupControlID="pcriteria" Position="Bottom">
                    </asp:PopupControlExtender>
                     </td>
                <td>
            <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text=" Test" Style="
                width: 31px">
            </asp:Label> </td>
                <td>
            <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged"
                Height="24px" Style=" width: 150px;" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium">
            </asp:DropDownList> </td>
                <td>
            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text=" Fail List" Style="
                width: 81px">
            </asp:Label> </td>
                <td>
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlfail_SelectedIndexChanged"
                Height="24px" Style=" width: 106px;" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium">
                <asp:ListItem>Both</asp:ListItem>
                <asp:ListItem>Dayscholar</asp:ListItem>
                <asp:ListItem>Hostler</asp:ListItem>
            </asp:DropDownList> </td>
                <td>
            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="List" Style="">
            </asp:Label> </td>
                <td>
            <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged"
                Height="24px" Style="" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                <asp:ListItem>--Select--</asp:ListItem>
                <asp:ListItem>1Sub</asp:ListItem>
                <asp:ListItem>2Sub</asp:ListItem>
                <asp:ListItem>3Sub</asp:ListItem>
                <asp:ListItem>Above 3Sub</asp:ListItem>
            </asp:DropDownList> </td>
              
                <td colspan="3">
            <asp:RadioButtonList ID="rbt" runat="server" Font-Bold="True" AutoPostBack="true"
                Font-Names="Book Antiqua" OnSelectedIndexChanged="rbtselected" RepeatDirection="Horizontal"
                Font-Size="Medium" Style="">
                <asp:ListItem>Pass</asp:ListItem>
                <asp:ListItem>Fail</asp:ListItem>
                <asp:ListItem>All</asp:ListItem>
            </asp:RadioButtonList>
        </td>
                </tr>
                <tr>
               
                
                <td colspan="2">
            <asp:Label ID="lbladminpass" runat="server" Text="Optional Min Pass Mark" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" Style="
                height: 15px;"></asp:Label> </td>
                <td>
            <asp:TextBox ID="txtoptiminpassmark" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" MaxLength="3" Style="
                height: 15px; width: 71px" onkeyup="this.value = fnc(this.value, 0, 100)"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtoptiminpassmark"
                FilterType="Numbers" /> </td>
                <td colspan="2">
            <asp:RadioButtonList ID="rbtsubject" runat="server" Font-Bold="True" AutoPostBack="true"
                Font-Names="Book Antiqua" OnSelectedIndexChanged="rbtselected" RepeatDirection="Horizontal"
                Font-Size="Medium" Style="">
                <asp:ListItem Value="0">Subject Code</asp:ListItem>
                <asp:ListItem Value="1">Subject Name</asp:ListItem>
            </asp:RadioButtonList>
            </td>
            <td>
            <asp:Button ID="btnGo" runat="server" Style="text-align: center; " Text="Go"
                Width="40px" Height="28px" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnGo_Click" />
                 </td>
                  <td>
            <asp:CheckBox ID="chtopper" runat="server" Style="text-align: center;" Text="Toppers"
                Font-Bold="True" Visible="true" Font-Names="Book Antiqua" Font-Size="Medium"
                AutoPostBack="true" OnCheckedChanged="chtopperchecked"></asp:CheckBox>
                 </td>
                <td>
            <asp:CheckBox ID="Chgrade" runat="server" Style="text-align: center; " Text="Grade"
                Font-Bold="True" Visible="true" Font-Names="Book Antiqua" Font-Size="Medium"
                AutoPostBack="true" OnCheckedChanged="Chgradechecked"></asp:CheckBox> </td>
                <td colspan="3">
                 <asp:CheckBox ID="chkIncludeAbsent" Checked="false" runat="server" Text="Include Absent in Pass Pecentage"
                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
            </tr>
            </table>
        </asp:Panel>

        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel3" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
                    Style=" width: 1110px;">
                </asp:Panel>
                <asp:Label ID="errmsg" runat="server" Text="svfas" ForeColor="Red" Font-Bold="true"
                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                <br />
                <asp:Panel ID="newpnl" runat="server" Style="margin-left: 1px;">
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px">
                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                            ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </center>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
            
        </asp:UpdatePanel>

        <center>

        <div id="rptprint" runat="server" visible="false">
            <table>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Visible="False" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Black" Text="Report Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()">
                        </asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnExcel" runat="server" Text="Export Excel" CssClass="dropdown"
                            Height="32px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnxl_Click" />
                    </td>
                    <td>
                        <asp:Button ID="BtnPrint" runat="server" Text="Print" CssClass="dropdown" Height="32px"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" OnClick="btnprintmaster_Click" />
                        <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblmessage1" runat="server" Text="" ForeColor="Red" Visible="False"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </div>
                <%--</ContentTemplate>
           
        </asp:UpdatePanel>--%>
            <br />
            <br />
        </center>
    </body>
    </html>
</asp:Content>

