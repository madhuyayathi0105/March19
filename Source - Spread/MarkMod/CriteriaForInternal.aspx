<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CriteriaForInternal.aspx.cs" Inherits="CriteriaForInternal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lbl_err').innerHTML = "";
        }
        function frelig() {

            document.getElementById('<%=btnnewcriteria.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnreovecritreia.ClientID%>').style.display = 'block';
        }
    </script>
    <style type="text/css">
        .head
        {
            background-color: Teal;
            font-family: Book Antiqua;
            font-size: medium;
            color: black;
            top: 70px;
            position: absolute;
            font-weight: bold;
            width: 950px;
            height: 25px;
            left: 15px;
        }
        .mainbatch
        {
            background-color: #0CA6CA;
            width: 970px;
            position: absolute;
            height: 52px;
            top: 125px;
            left: 15px;
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: black;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release" />
    <script type="text/javascript">
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler(sender, args) {
            if ($get('<%=p2.ClientID%>') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos = $get('<%=p2.ClientID%>').scrollLeft;
                yPos = $get('<%=p2.ClientID%>').scrollTop;
            }
        }

        function EndRequestHandler(sender, args) {
            if ($get('<%=p2.ClientID%>') != null) {
                $get('<%=p2.ClientID%>').scrollLeft = xPos;
                $get('<%=p2.ClientID%>').scrollTop = yPos;
            }
        }
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
    </script>
    <script type="text/javascript">
        var xPos1, yPos1;
        var prm1 = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler11(sender, args) {
            if ($get('<%=Panel1.ClientID%>') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos1 = $get('<%=Panel1.ClientID%>').scrollLeft;
                yPos1 = $get('<%=Panel1.ClientID%>').scrollTop;
            }
        }

        function EndRequestHandler11(sender, args) {
            if ($get('<%=Panel1.ClientID%>') != null) {
                $get('<%=Panel1.ClientID%>').scrollLeft = xPos1;
                $get('<%=Panel1.ClientID%>').scrollTop = yPos1;
            }
        }
        prm1.add_beginRequest(BeginRequestHandler11);
        prm1.add_endRequest(EndRequestHandler11);
    </script>
    <script type="text/javascript">
        var xPos3, yPos3;
        var prm3 = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler33(sender, args) {
            if ($get('<%=Panel2.ClientID%>') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos3 = $get('<%=Panel2.ClientID%>').scrollLeft;
                yPos3 = $get('<%=Panel2.ClientID%>').scrollTop;
            }
        }

        function EndRequestHandler33(sender, args) {
            if ($get('<%=Panel2.ClientID%>') != null) {
                $get('<%=Panel2.ClientID%>').scrollLeft = xPos3;
                $get('<%=Panel2.ClientID%>').scrollTop = yPos3;
            }
        }
        prm3.add_beginRequest(BeginRequestHandler33);
        prm3.add_endRequest(EndRequestHandler33);
    </script>
    <script type="text/javascript">
        var xPos2, yPos2;
        var prm2 = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler22(sender, args) {
            if ($get('<%=Panel3.ClientID%>') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos2 = $get('<%=Panel3.ClientID%>').scrollLeft;
                yPos2 = $get('<%=Panel3.ClientID%>').scrollTop;
            }
        }

        function EndRequestHandler22(sender, args) {
            if ($get('<%=Panel3.ClientID%>') != null) {
                $get('<%=Panel3.ClientID%>').scrollLeft = xPos2;
                $get('<%=Panel3.ClientID%>').scrollTop = yPos2;
            }
        }
        prm2.add_beginRequest(BeginRequestHandler22);
        prm2.add_endRequest(EndRequestHandler22);

    </script>
    </asp:ScriptManager>
    <br />
    <center>
        <span class="fontstyleheader" style="color: Green;">Criteria For Internal</span>
    </center>
    <br />
    <asp:UpdatePanel ID="Ma" runat="server">
        <ContentTemplate>
            <center>
            <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="Ma">
            <ProgressTemplate>
                <div style="height: 40px; width: 150px;">
                    <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                    <br />
                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                        Processing Please Wait...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
                <center>
                    <div style="width: 100%; margin: 0px; margin-bottom: 10px; margin-top: 10px;" visible="true">
                        <table class="maintablestyle" style="height: auto; width: auto;">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Iblbatch" runat="server" Text="Batch"></asp:Label>
                                                <asp:DropDownList ID="ddlbatch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                                    Width="60px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Ibldegree" Text="Degree" runat="server"></asp:Label>
                                                <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="p2" runat="server" CssClass="multxtpanel" Width="150px" Height="180px">
                                                    <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_degree_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_degree"
                                                    PopupControlID="p2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="Iblbranch" runat="server" Text="Branch"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Width="480px" Height="180px">
                                                    <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_branch_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_branch"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblsem" runat="server" Text="Sem"></asp:Label>
                                                <asp:DropDownList ID="ddlsem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                                    Width="40px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSuType" runat="server" Text="Type"></asp:Label>
                                                <asp:TextBox ID="txtSubType" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Width="350px" Height="180px">
                                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="CheckBox1_checkedchange" />
                                                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtSubType"
                                                    PopupControlID="Panel3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSubject" runat="server" Text="Subject"></asp:Label>
                                                <asp:TextBox ID="txtSubject" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Width="350px" Height="180px">
                                                    <asp:CheckBox ID="cbSubjet" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cbSubjet_checkedchange" />
                                                    <asp:CheckBoxList ID="cblSubject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblSubject_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtSubject"
                                                    PopupControlID="Panel2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblcriteria" runat="server" Text="Criteria"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnreovecritreia" runat="server" OnClick="btnreovecritreia_Click"
                                                    Text="-" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcriteria" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcriteria_SelectedIndexChanged"
                                                    Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnnewcriteria" runat="server" OnClick="btnnewcriteria_Click" Text="+" />
                                            </td>
                                            <td>
                                                <%--<asp:UpdatePanel ID="upgo" runat="server">--%>
                                                   <%-- <ContentTemplate>--%>
                                                        <asp:Button ID="btngo" runat="server" Font-Names="Book Antiqua" Text="Go" OnClick="btngo_Click"
                                                            Font-Size="Medium" Font-Bold="true" /><%--</ContentTemplate>--%>
                                              <%--  </asp:UpdatePanel>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
                <br />
                <asp:Label ID="lbl_err" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium"
                    Font-Names="Book Antiqua"></asp:Label>
                <br />
                <br />
                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                <asp:GridView ID="GridView2" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                    width: auto;" Font-Names="Times New Roman"  AutoGenerateColumns="true" OnRowCreated="OnRowCreated"
                    OnRowDataBound="gridview2_OnRowDataBound" OnDataBound="gridview2_DataBound" OnSelectedIndexChanged="SelectedIndexChanged">
                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                </asp:GridView>
                <br />
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                    Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ .">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnxl_Click" />
                <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnmasterprint_Click" />
                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                <asp:Panel ID="PCriteria" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="false" BorderWidth="2px" Width="700px">
                    <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            Criteria Entry
                        </caption>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblcriteriaadd" runat="server" Text="Criteria" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtcriteria" runat="server" Width="600px" Height="30px" TextMode="MultiLine"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btncriteraiadd" runat="server" Text="Add" OnClick="btncriteraiadd_Click"
                                        Style="left: 420px; position: absolute; height: 26px; width: 88px" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                </td>
                                <td>
                                    <asp:Button ID="btncriteraiexit" runat="server" Text="Exit" Style="left: 510px; position: absolute;
                                        height: 26px; width: 88px" OnClick="btncriteraiexit_Click" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <asp:Label ID="lblerrcritiria" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </div>
                </asp:Panel>
                <%-- creating new criteira --%>
                <asp:Panel ID="SubCriteria" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="false" BorderWidth="2px" Width="700px" Style="z-index: 1000; top: 30%;
                    left: 15%; right: 15%; position: absolute;">
                    <div class="panelinfraction" id="Div3" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black;">
                            SUB CRITERIA NEW ENTRY
                        </caption>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsubcriteriaadd" runat="server" Text="Subject" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsubcriteria" runat="server" Width="600px" Height="30px" TextMode="MultiLine"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnsubcriteriaadd" runat="server" Text="Add" OnClick="btnsubcriteraiadd_Click"
                                        Style="left: 420px; position: absolute; height: 26px; width: 88px" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                </td>
                                <td>
                                    <asp:Button ID="btnsubcriteriaexit" runat="server" Text="Exit" Style="left: 510px;
                                        position: absolute; height: 26px; width: 88px" OnClick="btnsubcriteraiexit_Click"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <asp:Label ID="lblsuberrcritiria" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </div>
                </asp:Panel>
                <%-- creating new Sub criteira --%>
                <asp:Panel ID="PExamdetails" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="false" BorderWidth="2px" Style="left: 150px; top: 277px; position: absolute;">
                    <div class="panelinfraction" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            Exam Details
                        </caption>
                        <br />
                        <asp:Label ID="Label1" runat="server" ForeColor="Green" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <br />
                        <table style="text-align: left">
                            <tr>
                                <td>
                                    <asp:Label ID="lblall" runat="server" Text="Common For Selected Subject's" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="chkcommonforall" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="chkcommonforall_OnCheckedChanged" />
                                    <%--</td>
                                <td>--%>
                                    <asp:Label ID="lblSameSub" runat="server" Text="Common For Same Subject's" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <asp:CheckBox ID="chkSameSub" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="chkSameSub_OnCheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblscode" runat="server" Text="Subject Code" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtscode" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Enabled="false" Width="200px"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsname" runat="server" Text="Subject Name" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtsname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Enabled="false" Width="350px"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbledate" runat="server" Text="Exam Date" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtedate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="80px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtedate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStartTime" runat="server" Text="Starting Time" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStartTime" runat="server" ToolTip="Time in 24 Hour Format.eg.HH:MM"
                                        placeHolder="14:30" Style="width: 40px;"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="starttime" Display="Dynamic" runat="server" ErrorMessage="*"
                                        ForeColor="Red" ToolTip="Hours : Minutes" ControlToValidate="txtStartTime" ValidationExpression="^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$">
                                    </asp:RegularExpressionValidator>
                                    <asp:FilteredTextBoxExtender ID="filterExtStartTime" runat="server" Enabled="True"
                                        TargetControlID="txtStartTime" FilterType="Numbers,Custom " ValidChars=":">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="lblstartTimeErr" runat="server" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblEndTime" runat="server" Text="Ending Time" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndTime" runat="server" ToolTip="Time in 24 Hour Format.eg.HH:MM "
                                        OnTextChanged="txtEndTime_TextChanged" AutoPostBack="true" placeHolder="16:30"
                                        Style="width: 40px;"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="endtime" Display="Dynamic" runat="server" ErrorMessage="*"
                                        ForeColor="Red" ControlToValidate="txtEndTime" ValidationExpression="^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$">
                                    </asp:RegularExpressionValidator>
                                    <asp:FilteredTextBoxExtender ID="filterExtEndTime" runat="server" Enabled="True"
                                        TargetControlID="txtEndTime" FilterType="Numbers,Custom " ValidChars=":">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblerr" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblExamDuration" runat="server" Text="Exam Duration" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDuration" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="80px" Placeholder="00:00:00" Text="" ReadOnly="true"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regDuration" runat="server" ValidationExpression="^([01]?\d|2[0-3]):([0-5]?\d):([0-5]?\d)$"
                                        Text="Duration Must Be in the Format hh:mm:ss/HH:mm:ss" ForeColor="Red" ControlToValidate="txtDuration"
                                        ValidationGroup="duration" ErrorMessage="Duration Must Be in the Format hh:mm:ss/HH:mm:ss"
                                        Display="Dynamic" SetFocusOnError="true"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsubcatogery" runat="server" Text="Select Sub Catogery" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="subctgrychckbox" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="subctgrychckbox_CheckedChanged" />
                                </td>
                                <td rowspan="3">
                                    <div id="divSubCatagory" runat="server" visible="false">
                                        <asp:Button ID="btnsubdelcritreia" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                            Font-Size="Medium" OnClick="btnsubdelcritreia_Click" Text="-" />
                                        <asp:DropDownList ID="ddlsubcriteriamain" runat="server">
                                            <asp:ListItem>Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Button ID="btnnewsubcriteria" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                            Font-Size="Medium" OnClick="btnnewsubcriteria_Click" Text="+" />
                                        <asp:Label ID="lblCo" runat="server" Text="Course outcome" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:DropDownList ID="ddlCo" runat="server">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:GridView ID="GridView1" runat="server" ShowFooter="True" AutoGenerateColumns="false"
                                            OnRowDataBound="GridView1_DataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Category">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlsubcriteria" runat="server">
                                                            <asp:ListItem>Select</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblCategory" Visible="false" Text='<%#Eval("CategoryCode") %>' runat="server"></asp:Label>
                                                        <asp:Label ID="lblSubjectId" Visible="false" Text='<%#Eval("subjectId") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Min Marks ">
                                                    <ItemTemplate>
                                                        <itemtemplate>
                                                                <asp:TextBox ID="txtminmarks" runat="server" Text='<%#Eval("Min Marks") %>'></asp:TextBox>
                                                                </itemtemplate>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Max Marks">
                                                    <ItemTemplate>
                                                        <itemtemplate>
                                                                <asp:TextBox ID="txtmaxmarks" runat="server" Text='<%#Eval("Max Marks") %>'></asp:TextBox>
                                                            </itemtemplate>
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Button ID="savesubentrymarks" runat="server" Text="Add New Row" OnClick="addnewrow" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <%--grid vew to get the marks --%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbllock" runat="server" Text="Is Lock" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chklock" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="chklock_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblldate" runat="server" Text="Lock Date" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtldate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="80px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txtldate">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblminmark" runat="server" Text="Min.Mark" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtminmark" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" MaxLength="3" Width="50px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtminmark"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblmaxmark" runat="server" Text="Max.Mark" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtmaxmark" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" MaxLength="3" Width="50px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtmaxmark"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Visiblity in Part Settings (For Staff Only)"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="CheckBox2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"  />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                               <%-- <asp:UpdatePanel ID="upsav" runat="server"><ContentTemplate>--%>
                                <td>
                                    <asp:Button ID="btnexamsave" ValidationGroup="duration" CausesValidation="true" runat="server"
                                        Text="Save" OnClick="btnexamsave_Click" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                    <asp:Button ID="btnDelete" CausesValidation="false" runat="server" Text="Delete"
                                        OnClick="btnexamDelete_Click" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                    <asp:Button ID="btnexamexits" CausesValidation="false" runat="server" Text="Exit"
                                        OnClick="btnexamexits_Click" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </td>
                              <%--  </ContentTemplate></asp:UpdatePanel>--%>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lblexamerror" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </div>
                </asp:Panel>
            </center>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnmasterprint" />
            <asp:PostBackTrigger ControlID="btnxl" />
           

           <%-- <asp:PostBackTrigger ControlID="btngo" />--%>
        </Triggers>
    </asp:UpdatePanel>
    
     <%--<center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upsav">
            <ProgressTemplate>
                <div style="height: 40px; width: 150px;">
                    <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                    <br />
                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                        Processing Please Wait...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
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
                                        <td align="center">
                                            <asp:Button ID="btnConfirm" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopConfirm_Click"
                                                Text="Confirm" runat="server" />
                                            <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                                Text="Ok" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="div4" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div5" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label3" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                       
                                            <asp:Button ID="btnaltok" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnaltok_Click"
                                                Text="Ok" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
      <center>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="div6" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div7" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label4" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnrm" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnrm_Click"
                                                Text="Ok" runat="server" />
                                            <asp:Button ID="btnrm1" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnrm1_Click"
                                                Text="Exit" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
</asp:Content>
