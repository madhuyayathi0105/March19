<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="University_mark.aspx.cs" Inherits="University_mark" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <script runat="server">
 </script>
   
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
    
      .style1
      {
          width: 87px;
      }
    
      .style6
      {
          width: 98px;
      }
    
      </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<body>
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager><br /><center>
         <asp:Label ID="Label1" runat="server" Text="Consolidated Grade Sheet" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
            <br />
         <center>
                <table style="width:700px; height:70px; background-color:#0CA6CA;">
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
                            <br />
                            
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
                            <%-- <br />
                        <asp:Label ID="lblEdegree" runat="server" ForeColor="Red" Text="Select Degree" 
                            Visible="False" Font-Bold="True" 
                            
                            
                           ></asp:Label>--%>
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
                        <td>
                            <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                            <br />
                        </td>
                    </tr>
               
                    <tr>
                        <td>
                            <asp:Label ID="lblExamMonth" runat="server" Text="ExamMonth" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                            </asp:DropDownList>
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="lblExamYear" runat="server" Text="ExamYear" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                            </asp:DropDownList>
                            <br />
                        </td>
                        <td class="style6">
                            <asp:Button ID="Button1" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                        <td>
                            <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Text="Print Master Setting"
                                Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnPrint_Click"
                                Width="160px" />
                        </td>
                        <td>
                            <asp:Label ID="lblpages" runat="server" Text="Page" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true"></asp:Label>
                            <asp:DropDownList ID="ddlpage" runat="server" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" AutoPostBack="True"
                                Width="47px" Height="21px">
                            </asp:DropDownList>
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
                        <td class="style6">
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
                </table></center>
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
                        <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                            Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                            Height="24px" Width="58px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                            AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                            FilterType="Numbers" />
                    </td>
                    <td>
                        <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                            Width="96px" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                            OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="16px" Width="32px"></asp:TextBox>
                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                            FilterType="Numbers" />
                    </td>
                    <td>
                        <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <table>
            <tr>
                <td>
                    <asp:Panel ID="pnlSpread" runat="server">
                        <%-- <FarPoint:FpSpread ID="FpExternal" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="1px" Width="900px" HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="AsNeeded"
                style="top: 258px; left: 103px; height:300px;"  >
            <CommandBar ShowPDFButton="true" ButtonType="PushButton" Visible="false" >
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" AllowSort="true" GridLineColor="Black" BackColor ="White">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>--%>
                        <FarPoint:FpSpread ID="FpExternal" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Width="1500px">
                            <CommandBar ShowPDFButton="true" ButtonType="PushButton">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" AllowSort="true" GridLineColor="Black" BackColor="White"
                                    AutoPostBack="true">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnxl_Click" />
                </td>
            </tr>
        </table>
    </body>
    </html>
</asp:Content>

