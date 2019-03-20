<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="IndividualStudentCamReport.aspx.cs" EnableEventValidation="false"
    Inherits="IndividualStudentCamReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblerr').innerHTML = "";
        }
        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=Showgrid.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 1; i < (tbl.rows.length - 1); i++) {
                var chkSelectid = document.getElementById('MainContent_Showgrid_selectchk_' + i.toString());

                if (chkSelAll.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }

        }

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
    <asp:ScriptManager ID="scriptvel" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <center>
                    <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 15px;
                        margin-top: 10px;">CAM R16-Individual Student CAM Report</span>
                    <table class="maintablestyle" style="margin: 0px; margin-bottom: 15px; margin-top: 5px;">
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 18px; width: 44px"></asp:Label><asp:DropDownList
                                        ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Width="90px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Height="25px" Width="69px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldegree" runat="server" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Height="25px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                                        AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        Height="25px" Width="41px">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="style8">
                                            <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style25">
                                            <asp:DropDownList ID="ddlsection" runat="server" AutoPostBack="True" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="52px" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsubject" runat="server" Text="Subject" Width="61px" Font-Bold="True"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtsubject" runat="server" Height="20px" ReadOnly="true" Width="150px"
                                                Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="psubject" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" ScrollBars="Auto" Width="350px" Style="font-family: 'Book Antiqua';
                                                margin-left: 196px">
                                                <asp:CheckBox ID="chksubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="chksubject_CheckedChanged" Text="Select All"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklstsubject" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="chklstsubject_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:DropDownExtender ID="ddesubject" runat="server" DropDownControlID="psubject"
                                                DynamicServicePath="" Enabled="true" TargetControlID="txtsubject">
                                            </asp:DropDownExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbltest" runat="server" Text="Test Name" Font-Bold="True" ForeColor="Black"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txttest" runat="server" Height="20px" ReadOnly="true" Width="180px"
                                                Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="paneltest" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" ScrollBars="Vertical" Width="232px" Style="margin-left: 48px;">
                                                <asp:CheckBox ID="chktest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="chktest_CheckedChanged" Text="Select All"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="cbltest" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="cbltest_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:DropDownExtender ID="DropDownExtender1" runat="server" DropDownControlID="paneltest"
                                                DynamicServicePath="" Enabled="true" TargetControlID="txttest">
                                            </asp:DropDownExtender>
                                        </td>
                                        <td class="style15">
                                            <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td style="width: 100%; height: 100%; margin-left: 20px; margin-top: 3px;">
                                            <center>
                                                <asp:Button ID="btnprint" runat="server" Text="Student Report" Font-Bold="True" Visible="false"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" Height="30px" OnClick="btnprint_Click" /></center>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Style="margin: 0px; margin-bottom: 15px; margin-top: 10px;"></asp:Label>
                </center>
                <center>
                    <div>
                        <center>
                            <table>
                                <tr>
                                    <td style="width: 100%; height: 100%">
                                        <center>
                                            <span style="padding-right: 100px; margin-left: 442px; margin-top: 3px;">
                                                <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                                                    onchange="return SelLedgers();" />
                                            </span>
                                        </center>
                                        <asp:GridView ID="Showgrid" Height="500" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                            HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="false" OnRowDataBound="Showgrid_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_sno" runat="server" Style="width: auto;" Text='<%#Eval("Sno") %>'></asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Roll No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_rollno" runat="server" Style="width: auto; text-align: right;"
                                                            Text='<%#Eval("RollNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reg No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_regno" runat="server" Style="width: auto;" Text='<%#Eval("RegNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Student Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_stdname" runat="server" Style="width: auto;" Text='<%#Eval("stdname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Student Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_stdtype" runat="server" Style="width: auto;" Text='<%#Eval("stdtype") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px" Visible="false">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_StartDate" runat="server" Style="width: auto;" Text='<%#Eval("StartDate") %>'
                                                                Visible="false"></asp:Label>
                                                            <asp:Label ID="lbl_admDate" runat="server" Style="width: auto;" Text='<%#Eval("AdmDate") %>'
                                                                Visible="false"></asp:Label>
                                                            <asp:Label ID="lbl_mode" runat="server" Style="width: auto;" Text='<%#Eval("Mode") %>'
                                                                Visible="false"></asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="selectchk" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                    <asp:Panel ID="panelv" runat="server" BackColor="White" Visible="false" Style="left: 0px;
                        top: 0px; width: 100%; position: absolute; z-index: 1000;">
                        <%--Width="1100px"--%>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnclose" runat="server" Text="Back" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="100px" Height="30px" OnClick="btnclose_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 100%">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 100%">
                                    <center>
                                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Report Name"></asp:Label>
                                        <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
                                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnClick="btnxl_Click" />
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblerr" runat="server" Text="" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Label ID="norecordlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Width="676px" Text=""></asp:Label>
                    <asp:CheckBoxList ID="cvl" runat="server">
                    </asp:CheckBoxList>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div style="height: 1px; width: 1px; overflow: auto;">
                <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--progressBar for Go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel_go">
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
    <%--progressBar for Sem--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel_sem">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
