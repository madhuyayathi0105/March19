<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master" AutoEventWireup="true" CodeFile="CAMrptFormat2.aspx.cs" Inherits="CAMrptFormat2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
  <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblexcelerr').innerHTML = "";
        }
    </script>
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
            top: 212px;
            left: 4px;
            position: absolute;
            height: 21px;
            width: 174px;
        }
        .style38
        {
            top: 211px;
            left: 176px;
            position: absolute;
            height: 21px;
            width: 171px;
        }
        .style39
        {
            top: 250px;
            left: 208px;
            position: absolute;
            height: 21px;
            width: 35px;
        }
        .style40
        {
            top: 250px;
            left: 252px;
            position: absolute;
            height: 27px;
            width: 44px;
        }
        .style41
        {
            top: 161px;
            left: 10px;
            position: absolute;
            height: 33px;
            width: 172px;
        }
        .style42
        {
            top: 200px;
            left: 732px;
            position: absolute;
            width: 34px;
            height: 25px;
        }
        .style43
        {
            top: 250px;
            left: 20px;
            position: absolute;
            height: 19px;
            width: 168px;
        }
        .style44
        {
            top: 251px;
            left: 310px;
            position: absolute;
            height: 21px;
            width: 126px;
        }
        .style45
        {
            top: 250px;
            left: 449px;
            position: absolute;
            height: 22px;
            width: 55px;
        }
        .style46
        {
            top: 250px;
            left: 516px;
            position: absolute;
        }
        .style47
        {
            top: 250px;
            left: 570px;
            position: absolute;
            height: 21px;
        }
        .style48
        {
            top: 250px;
            left: 672px;
            position: absolute;
            width: 34px;
        }
        .style49
        {
            top: 228px;
            left: 553px;
            position: absolute;
            height: 21px;
            width: 303px;
        }
        .style50
        {
            top: 283px;
            left: 20px;
            position: absolute;
            height: 21px;
            width: 329px;
        }
        .style51
        {
            top: 230px;
            left: -4px;
            position: absolute;
            width: 1169px;
        }
        .style52
        {
            height: 73px;
            width: 1017px;
        }
        .style53
        {
            width: 10px;
        }
        .style54
        {
            width: 179px;
            height: 21px;
            position: absolute;
            left: 790px;
            top: 204px;
        }
        .style55
        {
            top: 200px;
            left: 203px;
            position: absolute;
            height: 21px;
            width: 85px;
            right: 716px;
        }
    </style>
    <body>
       
        
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <div>

            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="20px"
                Style=" width: 1169px">
               <center>
                <asp:Label ID="Label1" runat="server" Text="CAM R20-CAM REPORT FORMAT II" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White"></asp:Label>
              </center>
            </asp:Panel>
        </div>
        <div>
            <asp:Panel ID="Panel1" runat="server" BackColor="LightBlue" BorderColor="Black" BorderStyle="Solid"
                ClientIDMode="Static" BorderWidth="1px" Style="height: 120px; 
                width: 996px;">
                <table style="margin-left: 0px; margin-bottom: 0px;" class="style52">
                    <tr>
                        <td class="style35">
                            <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style=" height: 18px;
                                width: 44px"></asp:Label>
                           
                        </td>
                        <td class="style34">
                         
                            <asp:DropDownList ID="ddlBatch" runat="server" Height="21px" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                                Style=""
                                Width="71px" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                           
                        </td>
                        <td class="style33">
                            <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="
                                height: 21px; width: 56px">
                            </asp:Label>
                        </td>
                        <td class="style34">
                            
                            <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Style="" Width="93px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                            
                        </td>
                        <td class="style33">
                            <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="
                                height: 21px; width: 56px"></asp:Label>
                        </td>
                        <td class="style34">
                          
                            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Style=" width: 288px;"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                           
                        </td>
                        <td class="style34">
                            
                            <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="
                                height: 21px; width: 32px"></asp:Label>
                          
                        </td>
                        <td class="style34">
                           
                            <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Style=" width: 48px;"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td class="style34">
                            <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="
                                height: 21px; width: 26px"></asp:Label>
                        </td>
                        <td class="style36">
                            
                            <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                                Style="
                                width: 42px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                          
                        </td>
                        <td class="style11">
                      
                        </td>
                       
                        <td>
                  
                        </td>
                        <td>
                         
                        </td>
                        <td class="style53">
                            <asp:RadioButton ID="RadioHeader" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in All Pages"
                                CssClass="style37" />
                            <asp:RadioButton ID="Radiowithoutheader" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in 1st Page"
                                CssClass="style38" />
                        </td>
                        <td>
                           
                        </td>
                        <td>
                            <asp:Label ID="lbltesterr" runat="server" Font-Bold="True" Font-Size="Medium" Visible="false"
                                Font-Names="Book Antiqua" ForeColor="Red" CssClass="style54">Please Select The Test</asp:Label>
                        </td>
                        <td></td>
                    </tr>
              
                    <tr>
                         <td colspan="4">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label2" runat="server" Text="Criteria" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="height: 15px;
                    width: 46px">
                </asp:Label>
                <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnTextChanged="TextBox1_TextChanged" CssClass="Dropdown_Txt_Box"
                    Style=" height: 17px; width: 125px;"></asp:TextBox>
                <asp:Panel ID="pnlCustomers" runat="server" CssClass="multxtpanel" Height="400">
                    <asp:CheckBox ID="SelectAll" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Small" OnCheckedChanged="SelectAll_CheckedChanged"
                        Text="Select All" />
                    <asp:CheckBoxList ID="ddlreport" runat="server" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged"
                        AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                        <asp:ListItem Value="0">Rank</asp:ListItem>
                        <asp:ListItem Value="1">Medium</asp:ListItem>
                        <asp:ListItem Value="2">12th Info</asp:ListItem>
                        <asp:ListItem Value="3">CGPA</asp:ListItem>
                        <asp:ListItem Value="4">Class Strength</asp:ListItem>
                        <asp:ListItem Value="5">Students Appeared</asp:ListItem>
                        <asp:ListItem Value="6">Students Absent</asp:ListItem>
                        <asp:ListItem Value="7">Students Passed</asp:ListItem>
                        <asp:ListItem Value="8">Students Failed</asp:ListItem>
                        <asp:ListItem Value="9">Average(&lt;50)</asp:ListItem>
                        <asp:ListItem Value="10">Average(50to65)</asp:ListItem>
                        <asp:ListItem Value="11">Average(&gt;65)</asp:ListItem>
                        <asp:ListItem Value="12">Class Average</asp:ListItem>
                        <asp:ListItem Value="13">Class Max-Min Mark</asp:ListItem>
                        <asp:ListItem Value="14">Pass Percentage</asp:ListItem>
                        <asp:ListItem Value="15">Staff Name</asp:ListItem>
                        <asp:ListItem Value="16">DayScholarPass</asp:ListItem>
                        <asp:ListItem Value="17">HostlerPass</asp:ListItem>
                        <asp:ListItem Value="18">TamMediumPass</asp:ListItem>
                        <asp:ListItem Value="19">EngMediumPass</asp:ListItem>
                        <asp:ListItem Value="20">Gender</asp:ListItem>
                        <asp:ListItem Value="21">GirlsPass</asp:ListItem>
                        <asp:ListItem Value="22">BoysPass</asp:ListItem>
                        <asp:ListItem Value="23">Quota</asp:ListItem>
                        <asp:ListItem Value="24">NFPS</asp:ListItem>
                        <asp:ListItem Value="25">NoOfHrAttended</asp:ListItem>
                        <asp:ListItem Value="26">Attendance %</asp:ListItem>
                        <asp:ListItem Value="27">Average(&gt;=75) %</asp:ListItem>
                        <asp:ListItem Value="28">Average(60to74)</asp:ListItem>
                        <asp:ListItem Value="29">Average(50to59)</asp:ListItem>
                        <asp:ListItem Value="30">Average(30to49)</asp:ListItem>
                        <asp:ListItem Value="31">Average(20to29)</asp:ListItem>
                        <asp:ListItem Value="32">Average(&lt;=19)</asp:ListItem>
                        <asp:ListItem Value="33">Maxmark rollno</asp:ListItem>
                        <asp:ListItem Value="34">Exam Date</asp:ListItem>
                        <asp:ListItem Value="35">Conducted Hours</asp:ListItem>
                        <asp:ListItem Value="36">Subjects Failed</asp:ListItem>
                        <asp:ListItem Value="37">Average(&gt;60)</asp:ListItem>
                        <asp:ListItem Value="38">Average(&gt;80)</asp:ListItem>
                        <asp:ListItem Value="39">No of all Cleared</asp:ListItem>
                        <asp:ListItem Value="40">% of all Cleared</asp:ListItem>
                        <asp:ListItem Value="41">Cut Off Marks</asp:ListItem>
                        <asp:ListItem Value="42">Subject Wise Attendance Percentage</asp:ListItem>
                        <asp:ListItem Value="43">Consolidate Studnet Result</asp:ListItem>
                    </asp:CheckBoxList>
                </asp:Panel>
                <br />
                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="TextBox1"
                    PopupControlID="pnlCustomers" Position="Bottom">
                </asp:PopupControlExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
        </td>
                     <td class="style34">
                            <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text=" Test" Style="
                                width: 31px">
                            </asp:Label>
                          
                        </td>
                        <td class="style14">
                         
                            <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged1"
                                Style="
                                width: 168px; height: 23px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                  
                        </td>
                        <td>
                          <asp:Label ID="lblFromDate" runat="server" Text="From Date" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" >
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="75px" Style="
                                height: 17px;"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style=" height: 21px;
                                width: 85px">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="80px" Style="
                                height: 17px; right: 464px;"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        </tr>
                        <tr>
               
                        <td style="margin-top: 0px;" colspan="4">
                           
                               <asp:RadioButtonList ID="RadioButtonList3" runat="server" CellSpacing="0" OnSelectedIndexChanged="RadioButtonList3_SelectedIndexChanged"
                                AutoPostBack="true"  RepeatDirection="Horizontal" Style="
                                height: 12px; " Font-Bold="True">
                                <asp:ListItem Value="1">Pass</asp:ListItem>
                                <asp:ListItem Value="2">Fail</asp:ListItem>
                                <asp:ListItem Value="3">Absent</asp:ListItem>
                                <asp:ListItem Value="4">All</asp:ListItem>
                            </asp:RadioButtonList>
                            
                           
                        </td>
                        <td>
                        <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Style="text-align: center;
           " Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="true"  />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
          
            </center>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblerroe" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False"></asp:Label>
                        <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="No Record(s) Found" Visible="False"
                            CssClass="style50"></asp:Label>
                      
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                            CssClass="style43">
                        </asp:Label>
                        <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style44"></asp:Label>
                        <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                            Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="style45">
                        </asp:DropDownList>
                        <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                            AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style46"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                            FilterType="Numbers" />
                        <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                            Width="96px" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style47"></asp:Label>
                        <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                            OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="17px" CssClass="style48"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                            FilterType="Numbers" />
                        <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style49"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <FarPoint:FpSpread ID="FpEntry" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Width="900px" Visible="False">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ButtonType="PushButton">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" AllowSort="true" GridLineColor="Black">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblexcelerr" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Please Enter Your Report Name"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <center>
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server"  onkeypress="display()" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </center>
                    </td>
                </tr>
            </table>
        </div>
        <%-- </form>--%>
    </body>
    </html>
</asp:Content>

