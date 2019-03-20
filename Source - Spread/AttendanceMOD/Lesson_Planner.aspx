<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Lesson_Planner.aspx.cs" Inherits="Lesson_Planner" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .MultipleSelectionDDL
        {
        }
        .style1
        {
            width: 87px;
        }
    </style>
    <script type="text/javascript">

        function TextBoxClick() {
            //document.getElementById('panel3').style.visibility = "true";
            //            var theButton = document.getElementById('<%=panel3.ClientID%>');
            //            theButton.style.display = 'block';
            var btn = document.getElementById('panelid');
            btn.style.display = "block";
            alert('hai');

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblerror').innerHTML = "";
        }
    </script>
    <br />
    <center>
        <asp:Label ID="Label4" runat="server" Text="Lesson Planner" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green"></asp:Label>
    </center>
    <br />
    <center>
        <asp:UpdatePanel ID="upanel1" runat="server">
            <ContentTemplate>
                <table class="maintablestyle" style="width: 900px; height: 60px; background-color: #0CA6CA;">
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" Height="22px"
                                Width="100px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="true" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                                Height="22px" Width="70px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="22px"
                                Width="70px" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                Height="22px" Width="150px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td colspan="4">
                            <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                            <%--</td>
                <td>--%>
                            <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged"
                                Height="22px" Width="50px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                            <%--</td>
                <td>--%>
                            <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                            <%--</td>
                <td>--%>
                            <asp:DropDownList ID="ddlSec" runat="server" Height="22px" Width="50px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlSec_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Subject" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtsubject" runat="server" Width="100px" CssClass="Dropdown_Txt_Box"
                                            ReadOnly="true" Font-Bold="true" Style="font-family: 'Book Antiqua';">---Select---</asp:TextBox>
                                        <asp:Panel ID="psubject" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                            BorderColor="Black" Width="200" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                            Height="150px">
                                            <asp:CheckBox ID="chksubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chksubject_CheckedChanged" />
                                            <asp:CheckBoxList ID="chklstsubject" runat="server" Font-Size="Medium" Style="font-family: 'Book Antiqua'"
                                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstsubject_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtsubject"
                                            PopupControlID="psubject" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="From" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtfrom" runat="server" Font-Bold="true" AutoPostBack="true" Width="75px"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua" OnTextChanged="txtfrom_TextChanged">
                            </asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfrom" runat="server"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="To" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtto" runat="server" Font-Bold="true" AutoPostBack="true" Width="75px"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua" OnTextChanged="txtto_TextChanged"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtto" runat="server"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblexclude" runat="server" Text="Type" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtexclude" runat="server" ReadOnly="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="true" CssClass="Dropdown_Txt_Box" Style="font-family: 'Book Antiqua';"></asp:TextBox>
                                        <asp:Panel ID="pexclude" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                            Height="150px">
                                            <asp:CheckBoxList ID="chklstexcl" runat="server" Font-Size="Medium" Style="font-family: 'Book Antiqua'"
                                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstexcl_SelectedIndexChanged"
                                                AutoPostBack="true">
                                                <asp:ListItem>Exclude Plan</asp:ListItem>
                                                <asp:ListItem>Exclude Daily Entry</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtexclude"
                                            PopupControlID="pexclude" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkwithoutalter" runat="server" Text=" Without Alternate Schedule"
                                Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua" Width="230px" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UPGo" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="GO" runat="server" Text="GO" Font-Bold="true" Enabled="true" OnClick="GO_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <br />
    <asp:UpdatePanel ID="UPanel2" runat="server">
        <ContentTemplate>
            <center>
                <asp:Label ID="lblerror" runat="server" Text="lblerrormsg" ForeColor="Red" Font-Size="Medium"
                    Font-Names="Book Antiqua" Font-Bold="true"></asp:Label>
            </center>
            <br />
            <center>
                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                <asp:GridView runat="server" ID="gview" OnSelectedIndexChanged="gview_selectedindexchange"
                    AutoGenerateColumns="true" OnRowCreated="gview_onRowCreated" OnDataBound="onDataBound"
                    ShowHeader="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Note" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblsnonote" runat="server" Text='<%#Eval("snonote") %>' />
                                <asp:Label ID="lbldatenote" runat="server" Text='<%#Eval("datenote") %>' />
                                <asp:Label ID="lblsubnote" runat="server" Text='<%#Eval("subnote") %>' />
                                <asp:Label ID="lblhournote" runat="server" Text='<%#Eval("hournote") %>' />
                                <asp:Label ID="lbltopicnote" runat="server" Text='<%#Eval("topicnote") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="S.No" HeaderStyle-Height="50">
                            <ItemTemplate>
                                <center>
                                    <asp:Label runat="server" ID="lblsno" Text='<%#Eval("sno") %>' /></center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date" ControlStyle-Font-Bold="true">
                            <ItemTemplate>
                                <center>
                                    <asp:Label runat="server" ID="lbldat" Text='<%#Eval("date") %>' /></center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subject" ControlStyle-Width="300">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblsub" Text='<%#Eval("sub") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hour" ControlStyle-Width="100">
                            <ItemTemplate>
                                <center>
                                    <asp:Label runat="server" ID="lblhour" Text='<%#Eval("hour") %>' /></center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Topic" ControlStyle-Width="280">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txt_top" onclick="TextBoxClick()" AutoPostBack="true"
                                    Text='<%#Eval("topic") %>' Style="height: auto;"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Large" />
                    <FooterStyle BackColor="White" ForeColor="#333333" />
                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                    <RowStyle ForeColor="#333333" />
                </asp:GridView>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" onkeypress="display()"
                                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnxl_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                            <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                            <asp:CheckBox ID="cbhourwise" runat="server" Text="Hourwise" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Visible="False" />
                        </td>
                    </tr>
                </table>
                <div id="panelid">
                    <asp:Panel ID="panel3" Visible="false" runat="server" BorderColor="Black" BackColor="AliceBlue"
                        BorderWidth="2px" Style="left: 150px; position: absolute; top: 200px; width: 800px">
                        <div>
                            <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged"
                                AutoPostBack="true" Font-Size="Large" Font-Names="TimesNewRoman" ForeColor="Blue" />
                            <asp:Label ID="lblsubtree" runat="server" Text="" Style="position: absolute; float: left;"
                                ForeColor="Blue" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                            font-size: Small; font-weight: bold">
                            <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="Leaf" OnTreeNodeCheckChanged="TreeView1_TreeNodeCheckChanged"
                                ExpandDepth="0" ShowLines="true" ShowExpandCollapse="true" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
                                AutoPostBack="true">
                            </asp:TreeView>
                        </div>
                        <div style="width: 655px; height: 80">
                            <asp:Button ID="BtnSaveTree" runat="server" Text="Save" Font-Bold="true" OnClick="BtnSaveTree_Click" />
                            <asp:Button ID="BtnExit" float="left" runat="server" Text="Exit" Font-Bold="true"
                                OnClick="BtnExitTree_Click" />
                        </div>
                    </asp:Panel>
                </div>
            </center>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnxl" />
        </Triggers>
    </asp:UpdatePanel>
    <div>
    </div>
    <%--</div>--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UPGo">
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
    </center>
</asp:Content>
