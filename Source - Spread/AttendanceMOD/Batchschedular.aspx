<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Batchschedular.aspx.cs" Inherits="Batchschedular"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Src="~/Usercontrols/Input_Events.ascx" TagName="collegedeatils" TagPrefix="UC" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {
            document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
        }
    </script>
    <script type="text/javascript">

        function check(cbControl) {

            if (cbControl.checked) {
                var e = cbControl.id;
                alert(e);
                var array = e.split("_");
                var position = array[3].toString(); alert(position);
                var pos = array[2].toString();
                var idpos = pos.replace('chkPeriod', ''); alert(idpos);

                var chklid = document.getElementById('MainContent_gridTimeTable_cblPeriod1_0');
                alert(chklid);
                //var strchkclst = document.getElementById('MainContent_gridTimeTable_cblPeriod1_' + position + '_0_' + position);
                //alert(strchkclst);
            }
            else {
                //alert('unchacked');
            }
        }

        function chkbox(chkControl) {
            var e = chkControl.id; alert(e);

        }
    </script>
    <script type="text/javascript">
        var xPos1, yPos1;
        var prm1 = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler1(sender, args) {
            if ($get('<%=pane.ClientID%>') != null) {
                xPos1 = $get('<%=pane.ClientID%>').scrollLeft;
                yPos1 = $get('<%=pane.ClientID%>').scrollTop;
            }
        }

        function EndRequestHandler1(sender, args) {
            if ($get('<%=pane.ClientID%>') != null) {
                $get('<%=pane.ClientID%>').scrollLeft = xPos1;
                $get('<%=pane.ClientID%>').scrollTop = yPos1;
            }
        }
        prm1.add_beginRequest(BeginRequestHandler1);
        prm1.add_endRequest(EndRequestHandler1);
    </script>
    <script type="text/javascript">
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler(sender, args) {
            if ($get('<%=gridTimeTable.ClientID%>') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos = $get('<%=gridTimeTable.ClientID%>').scrollLeft;
                yPos = $get('<%=gridTimeTable.ClientID%>').scrollRight;
            }
        }

        function EndRequestHandler(sender, args) {
            if ($get('<%=gridTimeTable.ClientID%>') != null) {
                $get('<%=gridTimeTable.ClientID%>').scrollLeft = xPos;
                $get('<%=gridTimeTable.ClientID%>').scrollRight = yPos;
            }
        }
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
    </script>
    <asp:UpdatePanel ID="UPD1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UPD1">
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
            <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                PopupControlID="UpdateProgress1">
            </asp:ModalPopupExtender>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div>
                <center>
                    <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 0px;
                        margin-top: 0px;">Batch Allocation</span>
                </center>
            </div>
            <center>
                <div style="width: 990px; border-color: Black; border-width: 1px; border-style: solid;">
                    <table style="width: 990px; margin: 0px; margin-bottom: 0px; margin-top: 0px; position: relative;
                        margin: 0px; margin-bottom: 0px; margin-top: 0px; background-color: #0CA6CA;">
                        <tr>
                            <td class="coll-lbl">
                                <asp:Label ID="Label4" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="22px" Width="55px"></asp:Label>
                            </td>
                            <td class="coll-ddl">
                                <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" Font-Size="Medium" Style="text-align: left"
                                    Height="22px" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td class="batch-lbl">
                                <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Height="22px" Width="40px"></asp:Label>
                            </td>
                            <td class="batch-ddl">
                                <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="true" Font-Bold="True"
                                    OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="22px" Width="70px">
                                </asp:DropDownList>
                            </td>
                            <td class="degree-lbl">
                                <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Height="22px" Width="55px">
                                </asp:Label>
                            </td>
                            <td class="degree-ddl">
                                <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="22px"
                                    OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Width="70px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td class="branch-lbl">
                                <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Height="22px" Width="55px"></asp:Label>
                            </td>
                            <td class="branch-ddl">
                                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Height="22px"
                                    OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Width="250px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td class="sem-lbl">
                                <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Height="22px" Width="34px"></asp:Label>
                            </td>
                            <td class="sem-ddl">
                                <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="22px"
                                    OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Width="50px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td class="sec-lbl">
                                <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Height="22px" Width="26px"></asp:Label>
                            </td>
                            <td class="sem-ddl">
                                <asp:DropDownList ID="ddlSec" runat="server" Height="22px" Width="50px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlSec_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <%--<td colspan="7">
                                <UC:collegedeatils ID="user_control" runat="server" Visible="false" />
                            </td>--%>
                        </tr>
                    </table>
                    <table style="width: 990px; margin: 0px; margin-bottom: 0px; margin-top: 0px; position: relative;
                        margin: 0px; margin-bottom: 0px; margin-top: 0px; background-color: #0CA6CA;">
                        <tr style="">
                            <td>
                                <asp:Label ID="lblnobatch" runat="server" Text="No of Batches" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtbatch" runat="server" OnTextChanged="txtbatch_TextChanged" Width="40"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"
                                    MaxLength="2"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="filt" runat="server" FilterType="Numbers" TargetControlID="txtbatch">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblbatches" runat="server" Text="LabBatch:" Font-Bold="true" Width="71"
                                    Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlnobatches" runat="server" Width="84px" Font-Bold="true"
                                    AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlnobatches_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblTimetable" runat="server" Text="TimeTable Name" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltimetable" runat="server" Font-Bold="true" OnSelectedIndexChanged="ddltimetable_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnGo" Text="Go" runat="server" OnClick="btnGo_Click" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;" Visible="true" />
                            </td>
                        </tr>
                    </table>
                    <%--<table style="width: 800px; margin: 0px; margin-bottom: 0px; margin-top: 0px; background-color: lightblue;
                        border-color: Black; border-width: 1px; border-style: solid;">
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>--%>
                </div>
            </center>
            <asp:Label ID="lblerror" runat="server" Text="Label" ForeColor="Red" Font-Bold="true"
                Font-Size="Medium" Font-Names="Book Antiqua" Style="margin: 0px; margin-bottom: 0px;
                margin-top: 0px; position: relative;"></asp:Label>
            <center>
                <div style="margin: 0px; margin-bottom: 0px; margin-top: 0px; width: 1000px;">
                    <div style="width: 508px; float: left; height: auto;">
                        <fieldset id="Fieldset1" runat="server" style="height: 450px; width: 100%;">
                            <legend style="font-weight: bold; font-family: book antiqua; font-size: medium;">Batch
                                Allocation </legend>
                            <div id="gviewdiv" runat="server" style="height: 300px; overflow: auto;">
                                <asp:GridView ID="gview" OnRowDataBound="gview_OnRowDataBound" runat="server" AutoGenerateColumns="false"
                                    ShowHeader="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" Visible="false">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:CheckBox ID="chck" runat="server" /><center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lblsno" runat="server" Text='<%#Eval("SNo") %>' /></center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Roll No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblroll" runat="server" Text='<%#Eval("Roll No") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reg No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblreg" runat="server" Text='<%#Eval("Reg No") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Student Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstunme" runat="server" Text='<%#Eval("Student Name") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Batch">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlbatch" Width="50" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                    <RowStyle ForeColor="#333333" />
                                </asp:GridView>
                            </div>
                            <br />
                            <fieldset id="Fieldset2" runat="server" style="height: auto; width: 365px; margin: 0px;
                                margin-top: 5px; margin-bottom: 5px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox1" runat="server" Text="Select" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged" />
                                            <asp:Label ID="lblselect" Visible="false" runat="server" Text="Select"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblfrom" runat="server" Text="From"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="fromno" runat="server" Style="width: 53px;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="fromno"
                                                FilterType="Numbers" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblto" runat="server" Text="To"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tono" runat="server" Style="width: 53px;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tono"
                                                FilterType="Numbers" />
                                        </td>
                                        <td>
                                            <asp:Button ID="Button2" runat="server" Text="Go" OnClick="selectgo_Click" />
                                        </td>
                                        <td colspan="6" align="center">
                                            <asp:Button ID="Btnsave" runat="server" Text="Save" OnClick="Btnsave_Click" Visible="false"
                                                Enabled="false" />
                                            <asp:Button ID="Btndelete" runat="server" Text="Delete" OnClick="Btndelete_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <div id="rptprint" runat="server" visible="false">
                                <table>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                                Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1" Height="20px"
                                                Width="180px" onkeypress="display()"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                                                CssClass="textbox btn2" Style="width: auto; height: auto;" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                                CssClass="textbox btn2" Style="width: auto; height: auto;" />
                                            <NEW:NEWPrintMater runat="server" ID="Printcontrol1" Visible="false" />
                                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                    <div style="float: right; width: 410px; height: auto;">
                        <fieldset id="Fieldset3" runat="server" style="height: 450px; width: 100%;">
                            <legend style="font-weight: bold; font-family: book antiqua; font-size: medium;">Semester
                                Schedule Settings</legend>
                            <%--GRIDTABLE  OnRowDataBound="gridTimeTable_OnRowDataBound"--%>
                            <%-- OnDataBound="gridTimeTable_OnDataBound"--%>
                            <center>
                                <%--<div id="gridtimetablediv" runat="server" style="width: 400px; height: 300px; overflow: auto;">--%>
                                <asp:Panel ID="pane" runat="server" Style="width: 400px; height: 350px; overflow: auto;">
                                    <asp:GridView ID="gridTimeTable" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                        BackColor="White" OnRowDataBound="gridTimeTable_OnRowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lblsno" runat="server" Text='<%#Container.DisplayIndex+1 %>' /></center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Day">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblday" runat="server" Text='<%#Eval("day") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hour">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lblhour" runat="server" Text='<%#Eval("hour") %>' /></center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period 1" Visible="false">
                                                <ItemTemplate>
                                                    <div style="position: relative;">
                                                        <div id="div5" style="position: relative;" runat="server">
                                                            <asp:TextBox ID="txtPeriod1" Visible="true" Height="12px" Width="76px" runat="server"
                                                                CssClass="textbox  txtheight2 commonHeaderFont" ReadOnly="true">-- Select --</asp:TextBox>
                                                            <asp:Panel ID="pnlPeriod1" Visible="true" runat="server" CssClass="multxtpanel" Height="80px"
                                                                Width="95px">
                                                                <asp:CheckBox ID="chkPeriod1" CssClass="commonHeaderFont" runat="server" AutoPostBack="true"
                                                                    Text="Select All" OnCheckedChanged="chkPeriod_CheckedChanged" /><%--onclick="check(this);"--%>
                                                                <asp:CheckBoxList ID="cblPeriod1" CssClass="commonHeaderFont" runat="server" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                                    <%--onclick="chkbox(this);" --%>
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popExtPreiod1" runat="server" TargetControlID="txtPeriod1"
                                                                PopupControlID="pnlPeriod1" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period 2" Visible="false">
                                                <ItemTemplate>
                                                    <div id="div6" style="position: relative;" runat="server">
                                                        <asp:UpdatePanel ID="upnlPeriod2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtPeriod2" Visible="true" Height="12px" Width="76px" runat="server"
                                                                    CssClass="textbox  txtheight2 commonHeaderFont" ReadOnly="true">-- Select --</asp:TextBox>
                                                                <asp:Panel ID="pnlPeriod2" Visible="true" runat="server" CssClass="multxtpanel" Height="80px"
                                                                    Width="95px">
                                                                    <asp:CheckBox ID="chkPeriod2" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                        AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblPeriod2" CssClass="commonHeaderFont" runat="server" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popExtPreiod2" runat="server" TargetControlID="txtPeriod2"
                                                                    PopupControlID="pnlPeriod2" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period 3" Visible="false">
                                                <ItemTemplate>
                                                    <div id="div7" style="position: relative;" runat="server">
                                                        <asp:UpdatePanel ID="upnlPeriod3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtPeriod3" Visible="true" Height="12px" Width="76px" runat="server"
                                                                    CssClass="textbox  txtheight2 commonHeaderFont" ReadOnly="true">-- Select --</asp:TextBox>
                                                                <asp:Panel ID="pnlPeriod3" Visible="true" runat="server" CssClass="multxtpanel" Height="80px"
                                                                    Width="95px">
                                                                    <asp:CheckBox ID="chkPeriod3" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                        AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblPeriod3" CssClass="commonHeaderFont" runat="server" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popExtPreiod3" runat="server" TargetControlID="txtPeriod3"
                                                                    PopupControlID="pnlPeriod3" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period 4" Visible="false">
                                                <ItemTemplate>
                                                    <div id="div8" style="position: relative;" runat="server">
                                                        <asp:UpdatePanel ID="upnlPeriod4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtPeriod4" Visible="true" Height="12px" Width="76px" runat="server"
                                                                    CssClass="textbox  txtheight2 commonHeaderFont" ReadOnly="true">-- Select --</asp:TextBox>
                                                                <asp:Panel ID="pnlPeriod4" Visible="true" runat="server" CssClass="multxtpanel" Height="80px"
                                                                    Width="95px">
                                                                    <asp:CheckBox ID="chkPeriod4" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                        AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblPeriod4" CssClass="commonHeaderFont" runat="server" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popExtPreiod4" runat="server" TargetControlID="txtPeriod4"
                                                                    PopupControlID="pnlPeriod4" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period 5" Visible="false">
                                                <ItemTemplate>
                                                    <div id="div9" style="position: relative;" runat="server">
                                                        <asp:UpdatePanel ID="upnlPeriod5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtPeriod5" Visible="true" Height="12px" Width="76px" runat="server"
                                                                    CssClass="textbox  txtheight2 commonHeaderFont" ReadOnly="true">-- Select --</asp:TextBox>
                                                                <asp:Panel ID="pnlPeriod5" Visible="true" runat="server" CssClass="multxtpanel" Height="80px"
                                                                    Width="95px">
                                                                    <asp:CheckBox ID="chkPeriod5" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                        AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblPeriod5" CssClass="commonHeaderFont" runat="server" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popExtPreiod5" runat="server" TargetControlID="txtPeriod5"
                                                                    PopupControlID="pnlPeriod5" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period 6" Visible="false">
                                                <ItemTemplate>
                                                    <div id="div10" style="position: relative;" runat="server">
                                                        <asp:UpdatePanel ID="upnlPeriod6" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtPeriod6" Visible="true" Height="12px" Width="76px" runat="server"
                                                                    CssClass="textbox  txtheight2 commonHeaderFont" ReadOnly="true">-- Select --</asp:TextBox>
                                                                <asp:Panel ID="pnlPeriod6" Visible="true" runat="server" CssClass="multxtpanel" Height="80px"
                                                                    Width="95px">
                                                                    <asp:CheckBox ID="chkPeriod6" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                        AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblPeriod6" CssClass="commonHeaderFont" runat="server" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popExtPreiod6" runat="server" TargetControlID="txtPeriod6"
                                                                    PopupControlID="pnlPeriod6" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period 7" Visible="false">
                                                <ItemTemplate>
                                                    <div id="div11" style="position: relative;" runat="server">
                                                        <asp:UpdatePanel ID="upnlPeriod7" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtPeriod7" Visible="true" Height="12px" Width="76px" runat="server"
                                                                    CssClass="textbox  txtheight2 commonHeaderFont" ReadOnly="true">-- Select --</asp:TextBox>
                                                                <asp:Panel ID="pnlPeriod7" Visible="true" runat="server" CssClass="multxtpanel" Height="80px"
                                                                    Width="95px">
                                                                    <asp:CheckBox ID="chkPeriod7" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                        AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblPeriod7" CssClass="commonHeaderFont" runat="server" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popExtPreiod7" runat="server" TargetControlID="txtPeriod7"
                                                                    PopupControlID="pnlPeriod7" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period 8" Visible="false">
                                                <ItemTemplate>
                                                    <div id="div20" style="position: relative;" runat="server">
                                                        <asp:UpdatePanel ID="upnlPeriod8" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtPeriod8" Visible="true" Height="12px" Width="76px" runat="server"
                                                                    CssClass="textbox  txtheight2 commonHeaderFont" ReadOnly="true">-- Select --</asp:TextBox>
                                                                <asp:Panel ID="pnlPeriod8" Visible="true" runat="server" CssClass="multxtpanel" Height="80px"
                                                                    Width="95px">
                                                                    <asp:CheckBox ID="chkPeriod8" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                        AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblPeriod8" CssClass="commonHeaderFont" runat="server" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popExtPreiod8" runat="server" TargetControlID="txtPeriod8"
                                                                    PopupControlID="pnlPeriod8" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period 9" Visible="false">
                                                <ItemTemplate>
                                                    <div id="div12" style="position: relative;" runat="server">
                                                        <asp:UpdatePanel ID="upnlPeriod9" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtPeriod9" Visible="true" Height="12px" Width="76px" runat="server"
                                                                    CssClass="textbox  txtheight2 commonHeaderFont" ReadOnly="true">-- Select --</asp:TextBox>
                                                                <asp:Panel ID="pnlPeriod9" Visible="true" runat="server" CssClass="multxtpanel" Height="80px"
                                                                    Width="95px">
                                                                    <asp:CheckBox ID="chkPeriod9" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                        AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblPeriod9" CssClass="commonHeaderFont" runat="server" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popExtPreiod9" runat="server" TargetControlID="txtPeriod9"
                                                                    PopupControlID="pnlPeriod9" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period 10" Visible="false">
                                                <ItemTemplate>
                                                    <div id="div13" style="position: relative;" runat="server">
                                                        <asp:UpdatePanel ID="upnlPeriod10" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtPeriod10" Visible="true" Height="12px" Width="76px" runat="server"
                                                                    CssClass="textbox  txtheight2 commonHeaderFont" ReadOnly="true">-- Select --</asp:TextBox>
                                                                <asp:Panel ID="pnlPeriod10" Visible="true" runat="server" CssClass="multxtpanel"
                                                                    Height="95px" Width="80px">
                                                                    <asp:CheckBox ID="chkPeriod10" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                        AutoPostBack="True" OnCheckedChanged="chkPeriod_CheckedChanged" />
                                                                    <asp:CheckBoxList ID="cblPeriod10" CssClass="commonHeaderFont" runat="server" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popExtPreiod" runat="server" TargetControlID="txtPeriod10"
                                                                    PopupControlID="pnlPeriod10" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle Height="20px" Font-Size="12px" />
                                        <%--<AlternatingRowStyle Height="20px" />--%>
                                    </asp:GridView>
                                </asp:Panel>
                                <%--</div>--%>
                            </center>
                            <table>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="LinkButton1" runat="server" Visible="false" CausesValidation="False"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small" ForeColor="blue"
                                            Style="width: 150px; text-align: left;" OnClick="LinkButton1_Click">To Add Multiple Batch</asp:LinkButton>
                                    </td>
                                    <td align="right">
                                        <fieldset id="Fieldset4" runat="server" style="height: auto; margin-top:23px; width: 50px; text-align: right;">
                                            <asp:Button ID="Button1" runat="server" Text="Save" OnClick="Batchallotsave_Click" />
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset id="Fieldset6" runat="server" style="height: auto; width: 95%; text-align: left; margin-top:25px;">
                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkautoswitch" runat="server" Text="Automatic Batch Switch" Font-Bold="True"
                                                            Font-Names="Book Antiqua" AutoPostBack="true" Font-Size="Small" OnCheckedChanged="chkautoswitch_CheckedChanged"
                                                            Style="height: auto; width: auto;" />
                                                    </td>
                                                    <td align="right">
                                                        <fieldset id="Fieldset7" runat="server" style="width: 150px; background-color: white;
                                                            height: 50px;">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtautoswitch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                                            Width="150px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                                        <asp:Panel ID="pautoswitch" runat="server" CssClass="multxtpanel" Height="150px"
                                                                            Width="200px">
                                                                            <asp:CheckBox ID="chkswitch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkswitch_CheckedChanged" />
                                                                            <asp:CheckBoxList ID="chklsautoswitch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsautoswitch_SelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtautoswitch"
                                                                            PopupControlID="pautoswitch" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Button ID="btnautoswitch" runat="server" Text="Ok" OnClick="btnautoswitch_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <fieldset id="Fieldset5" runat="server" style="width: 116px; background-color: white;
                                height: 84px;">
                                <asp:CheckBoxList ID="Checkboxlistbatch" runat="server" Style="width: 92px; border-style: double;"
                                    OnSelectedIndexChanged="Checkboxlistbatch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                                <asp:Button ID="Button3" runat="server" Text="Ok" OnClick="Button3_Click" />
                            </fieldset>
                        </fieldset>
                    </div>
                </div>
            </center>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
