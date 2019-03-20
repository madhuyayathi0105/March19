<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CAMLetter.aspx.cs" Inherits="CAMLetter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
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
        .style55
        {
            top: 240px;
            left: -4px;
            position: absolute;
            width: 1169px;
        }
        .style56
        {
            height: 31px;
        }
    </style>
    <body>
         
        <script type="text/javascript">
            function display() {

                document.getElementById('MainContent_norecordlbl').innerHTML = "";
            }
        </script>
        <asp:Panel ID="panelmove" runat="server" Style="width: 405px; height: 20px;">
        </asp:Panel>
       <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>--%>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:CalendarExtender ID="TextBox1_CalendarExtender6" runat="server" Format="dd/MM/yyyy"
                Enabled="True" TargetControlID="TextBox1">
            </asp:CalendarExtender>
            <asp:CalendarExtender ID="CalendarExtender11" runat="server" Format="dd/MM/yyyy"
                Enabled="True" TargetControlID="TextBox2">
            </asp:CalendarExtender>
            <br />
            <center>
                <span class="fontstyleheader" style="color: Green;">CR6 - CAM-Letter Format</span>
            </center>
            <br />
        </div>
        <div>
            <center>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBatch" runat="server" Height="21px" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                                Width="71px" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
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
                                OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Width="93px" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="190px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
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
                            <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text=" Test">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="udpnlTest" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_Test" runat="server" ReadOnly="true" CssClass="textbox textbox1"
                                        Height="20px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="150px">---Select---</asp:TextBox>
                                    <asp:Panel ID="ptest" runat="server" CssClass="multxtpanel" Style="width: 140px;">
                                        <asp:CheckBox ID="chktest" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chktest_ChekedChange"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="true" />
                                        <asp:CheckBoxList ID="chklstest" runat="server" Font-Size="Medium" Width="130px"
                                            Height="58px" Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="true"
                                            OnSelectedIndexChanged="chklsttest_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_Test"
                                        PopupControlID="ptest" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <%-- <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>--%>
                            <asp:Button ID="btngoto" runat="server" Text="Go" OnClick="btngoto_Click" CssClass="textbox btn1"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                             <%--   </ContentTemplate>
                        </asp:UpdatePanel>--%>
                        </td>
                        <td colspan="8">
                            <asp:DropDownList ID="ddlletter" runat="server" AutoPostBack="true" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlletter_SelectedIndexChanged">
                                <asp:ListItem> - - -  Select - - - </asp:ListItem>
                                <asp:ListItem>Letter Format1</asp:ListItem>
                                <asp:ListItem>Letter Format7</asp:ListItem>
                                <asp:ListItem>Tamil Report4</asp:ListItem>
                                <asp:ListItem Text="Multiple Test Marks" Value="Multiple Test Marks"></asp:ListItem>
                                <asp:ListItem>Tamil Report Mass</asp:ListItem>
                                <asp:ListItem>Letter Format8</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Labelnew" runat="server" Text="Mark Out of " Font-Bold="True" Visible="false"
                                Font-Size="Medium" Font-Names="Book Antiqua">
                            </asp:Label>
                            <asp:TextBox ID="TextBoxnew" runat="server" AutoPostBack="true" Visible="false" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Width="44px" OnTextChanged="TextBoxnew_TextChanged"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="TextBoxnew"
                                FilterType="Numbers" />
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="From"></asp:Label>
                            <asp:TextBox ID="TextBox2" runat="server" OnTextChanged="TextBox2_TextChanged" Font-Bold="True"
                                Height="18px" Width="100px" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="To"></asp:Label>
                            <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="18px" Width="100px"></asp:TextBox>
                            <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Text="Print Master Setting"
                                Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnPrint_Click"
                                Width="151px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="incVisnMisn" runat="server" Text="Include Vision Mission" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10" align="center">
                            <asp:Label ID="lblerr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="#FF3300" Text="Please Select Atleast one test."
                                Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="#FF3300" Text="There is no record found" Visible="False"></asp:Label>
                            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                                Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="24px" Width="58px">
                            </asp:DropDownList>
                            <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                                AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                                FilterType="Numbers" />
                            <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                                Width="96px" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                                OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="17px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                                FilterType="Numbers" />
                            <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" Style="text-align: center; margin-left: 0px;
                                margin-bottom: 10px; height: 19px;" Text="Print" Width="57px" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Visible="False" />
                            <asp:Button ID="format2btn" runat="server" Text="Print" Width="57px" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Visible="False" OnClick="format2btn_Click" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblnote" runat="server" Text="Note" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtnote" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="700px" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </center>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblesr" runat="server" Text=" " Font-Bold="True" ForeColor="Red" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <center>
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Small" ForeColor="#FF3300" Text="Select any One Student" Visible="False"
                                Style="height: 18px; width: 750px;"></asp:Label>
                        </center>
                    </td>
                </tr>
                <tr>
                    <td>
                        <center>
                            <asp:Panel ID="Pnltamilformat" runat="server" Height="200px" BackColor="Lightblue"
                                BorderColor="black" BorderWidth="1px" BorderStyle="Solid" Visible="false" ClientIDMode="Static"
                                Width="769px" Style="margin-bottom: 66px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbltamilnote" runat="server" Font-Bold="true"
                                                Font-Names="Book Antiqua" Text="Enter Note" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txttamilnote" TextMode="MultiLine" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Height="150px" Width="400"></asp:TextBox>

                                         </td>

                                        <td>
                                            <asp:Button ID="btnnotesave" runat="server" Text="Save" OnClick="btnnotesave_Click"                                                               CssClass="textbox btn1" Font-Names="Book Antiqua" Font-Size="Medium" 
                                                         Font-Bold="true" Width="100"/>
                                        </td>
                                        <td>
                                            <asp:Button ID="btntamilprint" runat="server" Text="Print" OnClick="btntamilprint_Click"                                                               CssClass="textbox btn1" Font-Names="Book Antiqua" Font-Size="Medium" 
                                                        Font-Bold="true" Width="100" />
                                        </td>
                                    </tr>
                                    <tr>
                                    <td colspan="4" style="text-align: center;">
                                            <asp:Label ID="lblsave" runat="server" Visible="false" Font-Bold="true"
                                                Font-Names="Book Antiqua" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlMultiFormat" runat="server" Height="60px" BackColor="Lightblue"
                                BorderColor="black" BorderWidth="1px" BorderStyle="Solid" Visible="false" ClientIDMode="Static"
                                Width="769px" Style="margin-bottom: 66px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMultiFrmt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Multiple Formats">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbEnglish" runat="server" Text="English" Font-Size="Medium"
                                                Font-Bold="true" GroupName="MultiLetter" />
                                            <asp:RadioButton ID="rbTamil" runat="server" Text="Tamil" Font-Size="Medium" Font-Bold="true"
                                                GroupName="MultiLetter" />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblConvertTo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Marks Converted To : ">
                                            </asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txt_ConvertTo" runat="server" Width="70px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnConversion" runat="server" Text="Use Mark Conversion" CssClass="textbox btn1"
                                                Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua" Width="200px" OnClick="btnConversion_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblMultierr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Small" ForeColor="#FF3300" Text="" Visible="False" Style="height: 18px;
                                                width: 140px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </center>
                    </td>
                </tr>
                <tr>
                    <td>
                        

                     <center>
           
                            
                     <center>
                            <asp:GridView ID="grdover" runat="server" Width="500px" BorderStyle="Double" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" GridLines="Both" CellPadding="4"  
                            ShowFooter="false" ShowHeader="true">
                            <Columns>
                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <center>
                                     <asp:CheckBox ID="chkselectall" runat="server" Width="30px" AutoPostBack="true" OnCheckedChanged="chkselectall_CheckedChanged"></asp:CheckBox>
                                    <asp:CheckBox ID="lbl_cb" runat="server" Width="30px" ></asp:CheckBox>
                                </center>
                                    <asp:Label ID="lblresult" runat="server" Visible="false" Width="30px"></asp:Label>
                                    <asp:Label ID="lblappno" runat="server" Visible="false" Width="30px"></asp:Label>
                                     
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Control" />
                            <PagerStyle BackColor="#336666"  HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True"  />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
                        </asp:GridView>

                        </center>   
                    
                </center>
                        <center>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </center>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Label ID="lblnotes" runat="server" Font-Bold="true"
                            Font-Names="Book Antiqua" Text="Enter Note" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtnotes" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="25px" Width="400"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Text="Enter Report Name" Font-Size="Medium" Style="margin-left: 28px; position: absolute;">
                        </asp:Label>
                        <asp:TextBox ID="txtexcelname" onkeypress="display()" runat="server" Style="font-family: 'Book Antiqua';
                            height: 21px; margin-left: 180px; position: absolute; width: 192px;" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                        </Ajax:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Style="margin-left: 387px;
                            position: absolute; width: 127px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="norecordlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Width="676px" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <%--//////////////////////////////////format 1 start///////////////////////////////////////////--%>
            <asp:HiddenField ID="hfgo_print" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="hfgo_print"
                CancelControlID="Button1" PopupControlID="Panel4" PopupDragHandleControlID="PopupHeader"
                Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panel4" runat="server" Width="1100px" Height="550px" ScrollBars="Auto"
                BorderColor="Black" BorderStyle="Double" Style="display: none; height: 400; width: 700;">
                <div class="HellowWorldPopup">
                    <div class="PopupHeader" id="Div2" style="text-align: center; color: Blue; font-family: Book Antiqua;
                        font-size: xx-large; font-weight: bold">
                    </div>
                    <div class="PopupBody">
                    </div>
                    <div class="Controls">
                        <center>
                            <asp:UpdatePanel ID="id_upnl" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpoppage" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Page" Visible="true">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlpoppage" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    CausesValidation="true" AutoPostBack="true" Font-Size="Medium" Visible="true"
                                                    Width="60" OnSelectedIndexChanged="ddlpoppage_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                    </center>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button1" runat="server" Text="Close" />
                            <br />
                        </center>
                    </div>
                </div>
            </asp:Panel>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
        </div>
        <div>
        </div>
        <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
        </div>
    </div>

    <%-- </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
            
            
            
        </Triggers>
    </asp:UpdatePanel>--%>

     <%--<center>
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>--%>
    </body>
    </html>
</asp:Content>
