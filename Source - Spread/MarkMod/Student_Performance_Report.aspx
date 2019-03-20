<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Student_Performance_Report.aspx.cs" Inherits="MarkMod_Student_Performance_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_Label1').innerHTML = "";
        }
        function PrintPanel() {
            var panel = document.getElementById("<%=divMainContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
            printWindow.document.write('</head><body >');
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
                margin-top: 10px; position: relative;">Course Outcomes Based Report - 1</span>
        </div>
    </center>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <center>
                    <table class="maintablestyle" style="margin-left: 0px; height: 73px; width: 1017px;
                        margin-bottom: 0px;">
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 18px; width: 44px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="250px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                    AutoPostBack="True">
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
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Width="250px">
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
                                            <asp:Label ID="lblknow" runat="server" Text="Knowledge"  Font-Bold="True"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"></asp:Label>
                                        </td>
                                        <td id="tblknowlwdge" runat="server" visible="false">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtkonw" Height="20px" Width="100px"
                                                        runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="Panelknow" runat="server" CssClass="multxtpanel" Height="200px">
                                                        <asp:CheckBox ID="chk_know" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chk_know_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cbl_know" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_know_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtkonw"
                                                        PopupControlID="Panelknow" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblcourse" runat="server" Text="CourseOutCome" Width="61px" Font-Bold="True"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="Textcourse" Style="margin-left: 76px" Height="20px" Width="100px"
                                                        runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="Pcourse" runat="server" CssClass="multxtpanel" Height="200px">
                                                        <asp:CheckBox ID="Checkcourse" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="Checkcourse_CheckedChanged" />
                                                        <asp:CheckBoxList ID="CkLcourse" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="CkLcourse_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="Textcourse"
                                                        PopupControlID="Pcourse" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsubject" runat="server" Text="Subject" Width="61px" Font-Bold="True"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin-left: 2px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_subject" runat="server" OnSelectedIndexChanged="ddl_subject_OnSelectedIndexChanged"
                                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Style="margin-left: -6px; height: 25px; width: 190px;">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbltest" runat="server" Text="Test Name" Font-Bold="True" ForeColor="Black"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Upnltest" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txttest" Style="margin-left: -7px" Height="20px" Width="150px" runat="server"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2"
                                                        ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="paneltest" runat="server" CssClass="multxtpanel" Height="200px"  Width="150px">
                                                        <asp:CheckBox ID="chktest" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chktest_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cbltest" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbltest_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popupExttest" runat="server" TargetControlID="txttest"
                                                        PopupControlID="paneltest" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:RadioButton ID="rbtformate1" GroupName="Formate" runat="server" Font-Bold="True" AutoPostBack="true"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Text="Formate 1" OnCheckedChanged="rbtformate3_CheckedChanged" />
                                        </td>
                                        <td colspan="3">
                                            <asp:RadioButton ID="rbtformate2" runat="server" GroupName="Formate" Font-Bold="True" AutoPostBack="true"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Text="Formate 2" OnCheckedChanged="rbtformate3_CheckedChanged" />
                                        </td>
                                         <td colspan="3">
                                            <asp:RadioButton ID="rbtformate3" runat="server" GroupName="Formate" Font-Bold="True" AutoPostBack="true"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="rbtformate3_CheckedChanged" Text="Knowledge Report" />
                                        </td>
                                        <td class="style15">
                                            <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>

            <br />
            <br />
           
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="divMainContents" runat="server" style="display: table; margin: 0px; height: auto;
                    margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
                    <asp:GridView ID="Showgrid" Style="height: auto;" runat="server" Visible="false"
                        HeaderStyle-ForeColor="Black" font-name="Book Antiqua" HeaderStyle-BackColor="#0CA6CA"
                        AutoGenerateColumns="true" ShowHeaderWhenEmpty="true" ShowHeader="false" OnRowDataBound="Showgrid_OnRowDataBound">
                        <%--     OnRowDataBound="Showgrid_OnRowDataBound"--%>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <br />
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div id="printtable" runat="server" visible="false">
                <center>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblreptname" runat="server" Text="Report Name" font-name="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="true" Width="100px"></asp:Label>
                            </td>
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                ForeColor="Red" Text="No Record Were Found" Font-Size="Medium" Visible="False"></asp:Label>
                            <td>
                                <asp:TextBox ID="txtreptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" onkeypress="display()" Width="130px"></asp:TextBox>
                            </td>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtreptname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <td>
                                <asp:Button ID="Excel" runat="server" Text="Export Excel" Font-Size="Medium" Font-Bold="true"
                                    OnClick="Excel_OnClick" Font-Names="Book Antiqua" />
                            </td>
                            <td>
                                <asp:Button ID="Print" runat="server" Text="Print" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="true" OnClick="Print_OnClick" />
                               <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                            </td>
                            <td>
                                <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--progressBar for Upbook_go--%>
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
