<%@ Page Title="CR8 - Result Analysis Report" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Resultanalysis.aspx.cs" Inherits="Resultanalysis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_norecordlbl').innerHTML = "";
        }
    </script>
     <script type="text/javascript">
         function PrintDiv() {
             var panel = document.getElementById("<%=contentDiv.ClientID %>");
             var printWindow = window.open('', '', 'height=auto,width=1191');
             printWindow.document.write('<html');
             printWindow.document.write('<head> <style type="text/css"> p{ font-size: x-small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 15px;
            margin-top: 10px; position: relative;">CR5 - Result Analysis Report</span>
    </center>
    <div>
        <center>
            <table class="maintablestyle" style="margin: 0px; margin-bottom: 15px; margin-top: 10px;
                position: relative;">
                <tr>
                    <td>
                        <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" Height="23px" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                            Width="76px" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"> </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="23px"
                            OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Width="93px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Style="margin-left: 35px;"
                            Height="23px" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Font-Bold="True"
                            Font-Names="Book Antiqua" Width="235px" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="23px"
                            OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="23px" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"> </asp:Label>
                        <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" AutoPostBack="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" Width="75px" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"> </asp:Label>
                        <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" AutoPostBack="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="87px" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="d/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td colspan="2">
                        <asp:RadioButton ID="rdattnd_daywise" runat="server" GroupName="Attendance" Font-Bold="True"
                            Font-Names="Book Antiqua" AutoPostBack="true" Font-Size="Medium" Text="Day Wise Attendance"
                            OnCheckedChanged="rdattnd_daywise_CheckedChanged" />
                    </td>
                    <td colspan="4">
                        <asp:RadioButton ID="rdattnd_hourwise" runat="server" GroupName="Attendance" Style="margin-left: -134px;"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Hour Wise Attendance"
                            OnCheckedChanged="rdattnd_hourwise_CheckedChanged" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text=" Test"> </asp:Label>
                    </td>
                    <td colspan="2" class="style1">
                        <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" Style="margin-left: 33px;"
                            OnSelectedIndexChanged="ddlTest_SelectedIndexChanged1" Height="23px" Width="146px"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                        <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Text="Go" Width="57px"
                            Height="27px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true" />
                            </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkincludepastout" runat="server" Text="Include PassedOut" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="includepastout_CheckedChanged"
                            AutoPostBack="True" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIncludeAbsent" Checked="false" runat="server" Text="Include Absent in Pass Pecentage"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td colspan="3">
                        <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Text="Print Master Setting"
                            Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnPrint_Click" />
                    </td>
                    <td colspan="4">
                        <asp:Label ID="lblpages" runat="server" Text="Page" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlpage" runat="server" AutoPostBack="True" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblerroe" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False"></asp:Label>
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Text="There is no record found" Visible="False"></asp:Label>
            &nbsp;
            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>&nbsp;&nbsp;
            <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                Height="24px" Width="58px">
            </asp:DropDownList>
            <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                FilterType="Numbers" />
            &nbsp;&nbsp;
            <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                Width="96px" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>&nbsp;&nbsp;
            <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Height="17px"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                FilterType="Numbers" />
            &nbsp;&nbsp;
            <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            <asp:Button ID="Button2" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Enabled="False" Visible="False" />
            
            <br />
            <div ID="divgrid" runat="server" Visible="false" style ="width:820px;border: solid 1px black;">
                <asp:GridView ID="Showgrid" runat="server"  BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" HeaderStyle-ForeColor="Black"
                                        HeaderStyle-BackColor="#0CA6CA"   Height="600" Width="820" >
                                    </asp:GridView>
            </div>
            <br />
            <div style="margin: 0px; margin-bottom: 15px; margin-top: 15px; position: relative;">
                <asp:Label ID="norecordlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="Red" Width="250px" Text=""></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:TextBox>
                <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                </Ajax:FilteredTextBoxExtender>
                <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />

                    
                <asp:Button ID="btnprint_Pdf" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    OnClick="btnprint_Pdf_Click" Font-Size="Medium" Text="Print" Width="127px" />

                   
            </div>
           
        </center>
    </div>
     <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
        </div>
    </div>

     </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnExcel" />
                                <asp:PostBackTrigger ControlID="btnprint_Pdf" />
                                
                                </Triggers>
                             </asp:UpdatePanel>

                             <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
    

    
</asp:Content>
