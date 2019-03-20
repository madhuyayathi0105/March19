<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Subjectschedularpage.aspx.cs" Inherits="Subjectschedularpage" %>

<%@ Register Src="~/Usercontrols/Input_Events.ascx" TagName="subject" TagPrefix="UC" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Print" TagPrefix="UC" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_norecordlbl').innerHTML = "";
        }
          .HeaderFreez { position:relative ; top:expression(this.offsetParent.scrollTop);
    z-index: 10 }
    </script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
    <script src="<%= ResolveUrl("~/GridviewScroll/gridviewScroll.min.js") %>" type="text/javascript"></script>
    <script src="~/GridviewScroll/jquery.min.js" type="text/javascript"></script>
    <link href="~/GridviewScroll/gridviewScroll.css" rel="stylesheet" type="text/css" />
    <script src="~/GridviewScroll/gridviewScroll.js" type="text/javascript"></script>
    <script src="~/GridviewScroll/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            gridviewScroll();

        });
        function gridviewScroll() {


            $('#<%=grdview2.ClientID%>').gridviewScroll({
                width: 900,
                height: 580,
                freezesize: 4,
                headerrowcount: 2
            });


        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var table = $('#grdview2').DataTable({
                scrollY: true,
                scrollX: true,
                scrollCollapse: true,
                paging: false,
                fixedColumns: {
                    leftColumns: 1,
                    rightColumns: 1
                }
            });
        });
    </script>
    <script type="text/javascript">
    .panellayout { z-index: 1000; border: solid; border-width: 1px; border-color: gray;
    position: absolute; left: 1px; } .GridViewContainer { position: relative; overflow:
    auto; ) /* to freeze column cells and its respecitve header*/ .FrozenCell { background-color:
    #F0F8FF; position: relative; cursor: default; left: expression(document.getElementById("GridViewContainer").scrollLeft-2);
    z-index: 30; } /* for freezing column header*/ .FrozenHeader { position: relative;
    cursor: default; left: expression(document.getElementById("GridViewContainer").scrollLeft-2);
    z-index: 20; }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .font
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        .topHandle
        {
            background-color: #97bae6;
        }
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 500px;
            min-height: 100px;
            max-height: 150px;
            top: 100px;
            left: 150px;
        }
    </style>
    <style type="text/css">
        .GridDock
        {
            overflow-x: auto;
            overflow-y: auto;
            height: 600px;
            width: 1000px padding: 0 0 0 0;
        }
    </style>
    <script type="text/javascript">

        function AutoCompleteExtender1_OnClientPopulating(sender, args) {

            var details = document.getElementById('<%=lblValue.ClientID %>');
            sender.set_contextKey(details.innerHTML);
        }

    </script>
    <script type="text/javascript" language="javascript">
        function SelectAll(id) {
            var dropdownListId = document.getElementById(id);

            var e = document.getElementById(id);
            var strUser = e.options[e.selectedIndex].text;
            var array = id.split("_");

            var position = array[5].toString();
            var position12 = array[6].toString();
            var grid = document.getElementById('<%=grdview2.ClientID%>');

            for (var jk = 4; jk < grid.rows.length; jk++) {
                var e1 = document.getElementById('MainContent_TabContainer1_TabPanel3_grdview2_ddlrows_' + position + '_' + jk.toString());

                if (e1.disabled == false) {
                    for (value = 0; value <= e1.options.length - 1; value++) {
                        if (e1.options[value].text.trim() == strUser.trim()) {
                            var grd = grid.rows[jk - 1].cells[3].innerHTML;
                            if (grd != "&nbsp;")
                                if (strUser != "") {
                                    e1.options[value].selected = strUser;
                                }
                                else
                                    e1.selectedIndex = e.selectedIndex;
                             

                        }
                    }
                }
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function subjectcheck(id) {
            var dropdownListId = document.getElementById(id);
            var e = document.getElementById(id);
            //              alert(e);
            var strUser = e.options[e.selectedIndex].text;

            var strarray = strUser.split("-");

            var struser1 = strarray[0].toString();

            var struser2 = strarray[1].toString();

            var array = id.split("_");

            var position = array[5].toString();

            var position12 = array[6].toString();

            var grid = document.getElementById('<%=grdview2.ClientID%>');

            for (var jk = 5; jk < grid.rows.length; jk++) {
                if (jk.toString() != position) {

                    var e1 = document.getElementById('MainContent_TabContainer1_TabPanel3_grdview2_ddlrows_' + jk.toString() + '_' + position12.toString());

                    if (e1 != null) {
                        var nxtstr = e1.options[e1.selectedIndex].text;
                        var nxtarray = nxtstr.split("-");
                        var nxtarray1 = nxtarray[0].toString();
                        var nxtarray2 = nxtarray[1].toString();

                        if (nxtarray1.trim() == struser1.trim()) {

                            e.options[0].selected = true;
                            alert("Student Cannot Select The Same Subjects More Than Once");
                        }
                    }
                }
                if (jk.toString() == position) {
                    var e2 = document.getElementById('MainContent_TabContainer1_TabPanel3_grdview2_ddlrows_' + position + '_' + jk.toString());

                    if (e2 != null) {
                        var ntstr = e2.options[e2.selectedIndex].text;
                        var nxtarra = ntstr.split("-");
                        var nxtarra1 = nxtarra[0].toString();
                        var nxtarra2 = nxtarra[1].toString();
                        if (nxtarra2.trim() == struser2.trim()) {
                            e.options[0].selected = true;
                            alert("Student Cannot Select The Same Subjects and Staff More Than Once");
                        }
                    }

                }
            }

        }
    </script>
    <center>
        <asp:Label ID="Label2" CssClass="fontstyleheader" runat="server" Text="Staff Selector"
            ForeColor="Green" Visible="true"></asp:Label>
    </center>
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:UpdatePanel ID="UDP1" runat="server">
            <ContentTemplate>
                <asp:TabContainer ID="TabContainer1" runat="server" Height="725" Width="1000" ActiveTabIndex="0"
                    OnActiveTabChanged="TabContainer1_ActiveTabChanged" AutoPostBack="true">
                    <asp:TabPanel ID="tabpanel1" Visible="false" runat="server" HeaderText="Subject Chooser"
                        TabIndex="1">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server">
                                <div>
                                    <center>
                                        <table style="width: 700px; height: 70px; background-color: #0CA6CA;">
                                            <tr>
                                                <td>
                                                    <UC:subject ID="usercontrol" runat="server"></UC:subject>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                    <center>
                                        <%-- <asp:UpdatePanel ID="udp1" runat="server"><ContentTemplate>--%>
                                        <table style="width: 700px; height: 70px; background-color: #0CA6CA;">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rbsubname" Text="Subject Name" AutoPostBack="true" OnCheckedChanged="rbradio_CheckedChanged"
                                                        runat="server" GroupName="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbsubacr" Text="Subject Acronym" AutoPostBack="true" OnCheckedChanged="rbradio_CheckedChanged"
                                                        runat="server" GroupName="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkeleective" runat="server" Text="Show Only Elective" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="rbradio_CheckedChanged" />
                                                    <asp:Button ID="btnGo" Text="Go" runat="server" OnClick="btnGo_Click" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                                </td>
                                            </tr>
                                            <td>
                                                <asp:Label ID="Labelerror" runat="server" Text="Label" ForeColor="Red" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <tr>
                                            </tr>
                                        </table>
                                        <%--                                         </ContentTemplate></asp:UpdatePanel>--%>
                                    </center>
                                </div>
                                <div>
                                    <center>
                                        <asp:Button ID="Button1" runat="server" Style="position: absolute; top: 965px; left: 420px;"
                                            Text="Save" OnClick="Savebtn_Click" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" />
                                        <asp:Button ID="btnprint" runat="server" Style="position: absolute; top: 965px; margin-left: -73px;"
                                            Text="Print" OnClick="btnprint_Click" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" />
                                    </center>
                                    <UC:Print ID="printcontrol" runat="server" />
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Staff Selector" TabIndex="2"
                        Width="970px" Height="700px">
                        <ContentTemplate>
                            <div>
                                <table style="background-color: #0CA6CA;">
                                    <tr>
                                        <td style="height: 33px; width: 1015px;">
                                            <UC:subject ID="usercontrol1" runat="server"></UC:subject>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                <ContentTemplate>
                                                    <asp:Button ID="Button2" Text="Go" runat="server" OnClick="btnGo1_Click" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="true" Style="font-family: Book Antiqua;
                                                        font-size: medium; font-weight: bold; height: auto; width: auto;" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblerror" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                                <table>
                                    <tr>
                                        <td style="height: 400px; margin: 0px;">
                                            <asp:TreeView runat="server" ID="subjtree" SelectedNodeStyle-ForeColor="Red" HoverNodeStyle-BackColor="LightBlue"
                                                AutoPostBack="true" OnSelectedNodeChanged="subjtree_SelectedNodeChanged" Font-Names="Book Antiqua"
                                                Font-Size="Small" ForeColor="Black" Style="color: Black; background-color: White;
                                                font-family: Book Antiqua; font-size: small; height: 358px; border-style: solid;
                                                border-width: 1px; overflow: scroll; width: 300px;">
                                            </asp:TreeView>
                                        </td>
                                        <td style="width: 700px; height: 400px; margin: 0px;">
                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Button ID="FindBtn" runat="server" Text="Select Staff" Font-Names="Book Antiqua"
                                                            Font-Bold="true" OnClick="FindBtn_Click" />
                                                        <asp:CheckBox ID="Chkalterotherdept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            OnCheckedChanged="Chkalterotherdept_CheckedChanged" Font-Size="Medium" Text="Add Staff To Other Department"
                                                            AutoPostBack="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="justify">
                                                        <div style="height: 337px; width: 631px; overflow: auto;">
                                                            <asp:GridView runat="server" ID="gview" Visible="false" AutoGenerateColumns="false"
                                                                CssClass="grid-view" GridLines="Both">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="S.No">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:Label ID="lblno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <center>
                                                                                <asp:Label ID="allchk" runat="server" Text="Select"></asp:Label></center>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="selectchk" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Staff Code">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcodee" runat="server" Text='<%#Eval("Staff_Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Staff Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblnamee" runat="server" Text='<%#Eval("Staff_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remove">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:Button ID="btn_remove" Text="Remove" OnClick="btn_remove" runat="server" /></center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                                                <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                                <RowStyle ForeColor="#333333" />
                                                                <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                                            </asp:GridView>
                                                            <br />
                                                            <center>
                                                                <asp:Button ID="Save" runat="server" Text="Save" Font-Bold="true" Font-Size="Medium"
                                                                    Font-Names="Book Antiqua" OnClick="btnsave_Click" Width="75px" />
                                                            </center>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Student's Staff Selector"
                        TabIndex="3" Width="970px" Height="700px">
                        <ContentTemplate>
                            <div>
                                <table style="background-color: #0CA6CA; border-color: Black; border-width: 1px;
                                    border-style: solid;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlclg1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged1"
                                                Style="text-align: left" Height="22px" Width="162px" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                                Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBatch1" runat="server" AutoPostBack="true" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged1"
                                                Height="22px" Width="70px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Size="Medium"
                                                Font-Names="Book Antiqua" Style="margin-left: 3px;">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDegree1" runat="server" AutoPostBack="True" Height="22px"
                                                Width="77px" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged1" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                                Font-Names="Book Antiqua" Style="margin-left: -20px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBranch1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged1"
                                                Height="22px" Width="177px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                                Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged1"
                                                Height="22px" Width="50px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                                Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec1" runat="server" Height="22px" Width="50px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSec_SelectedIndexChanged1" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSuType" runat="server" Text="Type" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Style="margin-left: -88px;">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSubType" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                Width="135px" style="margin-left: -73px;" Font-Bold="True" Visible="true">---Select---</asp:TextBox>
                                            <asp:Panel ID="panel5" runat="server" CssClass="multxtpanel" Height="250px" Width="355px"
                                                SkinID="DefaultVisiblePanel" Style="display: none;">
                                                <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_checkedchange"
                                                    Text="Select All" AutoPostBack="True" TextAlign="Right" Style="text-align: left;" />
                                                <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged"
                                                    TextAlign="Right">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtSubType"
                                                PopupControlID="panel5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </td>
                                        <td colspan="3">
                                            <asp:RadioButton ID="rbstusubcode" Text="Subject Name" runat="server" GroupName="stusubject"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Enabled="true" />
                                            <asp:RadioButton ID="rbstusubacr" Text="Subject Acronym" runat="server" GroupName="stusubject"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Enabled="true"
                                                AutoPostBack="true" OnCheckedChanged="StudentStaffchanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:UpdatePanel ID="updgo" runat="server">
                                                <ContentTemplate>
                                                    <asp:RadioButton ID="rbstcode" runat="server" Text="Staff Code" GroupName="stfv"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Enabled="true"
                                                        AutoPostBack="true" OnCheckedChanged="StudentStaffchanged" />
                                                    <asp:RadioButton ID="rbstname" runat="server" Text="Staff Name" GroupName="stfv"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Enabled="true"
                                                        AutoPostBack="true" OnCheckedChanged="StudentStaffchanged" />
                                                    <asp:Button ID="btnstustafgo" Text="Go" runat="server" OnClick="btnstustafgo_Click"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true"
                                                        AutoPostBack="true" OnCheckedChanged="StudentStaffchanged" />
                                                    <script type="text/javascript" language="javascript">
                                                        Sys.Application.add_load(gridviewScroll);
                                                    </script>
                                                    <asp:Button ID="btnSearchBy" Text="SearchBy" runat="server" OnClick="btnSearchBy_OnClick"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <center>
                                                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="updgo">
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
                                                <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress3"
                                                    PopupControlID="UpdateProgress3">
                                                </asp:ModalPopupExtender>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Label ID="lblstustaferr" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red" Style="margin: 0px; margin-bottom: 15px; margin-top: 15px;"></asp:Label>
                                <asp:Panel ID="Panel4" runat="server" CssClass="panellayout" Style="top: 100px; height: 582px;
                                    width: 1000px; left: 0px">
                                    <div id="GridViewContainer" class="GridViewContainer" style="height: 500px; width: 900px;
                                        left: 0px">
                                        <%-- <div id="divspreadpopup" runat="server" visible="false" class="GridDock" style="width: 1000px;
                                    margin-top: 15px; border: solid 1px black;">--%>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="grdview2" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                                        BorderStyle="Double" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        GridLines="Both" CellPadding="4" HeaderStyle-BackColor="#0CA6CA" ShowHeaderWhenEmpty="true"
                                                        OnRowDataBound="grdview2_OnRowDataBound" BackColor="#F0F8FF">
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnstustaffsave" runat="server" Text="Save" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnClick="btnstustaffsave_Click" Width="75px" />
                                        <asp:Button ID="btnstustaffprint" runat="server" Text="Print" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnClick="btnstustaffprint_Click" Width="75px" Visible="false" />
                                        <NEW:NEWPrintMater runat="server" ID="NEWPrintMater" Visible="false" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <%-- <asp:AsyncPostBackTrigger ControlID="btnstustaffsave" />--%>
                                        <asp:PostBackTrigger ControlID="btnstustaffsave" />
                                        <asp:PostBackTrigger ControlID="btnstustaffprint" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>
                <%--Style="left: 353px; top: 275px; position: absolute;"--%>
                <asp:Panel ID="panel3" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                    BorderWidth="2px" Style="left: 30%; top: 35%; right: 30%; position: absolute;
                    overflow: auto; z-index: 3;" Height="480px" Width="715px">
                    <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            Select Staff Incharge
                        </caption>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblcollege" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollege" runat="server" Width="150px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldepratstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="LblCategory" runat="server" Text="Staff Category"></asp:Label>
                                    <asp:TextBox ID="txt_Category" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                        Width="135px" Font-Bold="True" Visible="true">---Select---</asp:TextBox>
                                    <asp:Panel ID="panel_Category" runat="server" CssClass="multxtpanel" Height="250px"
                                        Width="355px" Style="text-align: left;">
                                        <asp:CheckBox ID="cb_Category" runat="server" OnCheckedChanged="cb_Category_CheckedChanged"
                                            Text="Select All" AutoPostBack="True" TextAlign="Right" Style="text-align: left;" />
                                        <asp:CheckBoxList ID="cbl_Category" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Category_SelectedIndexChanged"
                                            TextAlign="Right">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_Category"
                                        PopupControlID="panel_Category" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:Button ID="BtnCategory" runat="server" Text="Go" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" OnClick="BtnCategory_Click" Width="53px" />
                                </td>
                                <%--<td>
                                    <asp:DropDownList ID="ddlCategory" runat="server" Width="150px" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>--%>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsearchby" runat="server" Text="Staff By"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                        <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                        AutoPostBack="True"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:UpdatePanel ID="upael3" runat="server">
                            <ContentTemplate>
                                <div id="div7" runat="server" style="overflow: auto; border: 1px solid Gray; width: 460px;
                                    height: 280px;">
                                    <asp:GridView runat="server" ID="gviewstaff" AutoGenerateColumns="false" Style="height: 300;
                                        width: 460px; overflow: auto;">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%#Container.DisplayIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <center>
                                                        <asp:Label ID="allchk" runat="server" Text="Select"></asp:Label></center>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="selectchk1" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Staff Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstaff" runat="server" Text='<%#Eval("Staff_Code") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Staff Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblname" runat="server" Text='<%#Eval("Staff_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                        <RowStyle ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                    </asp:GridView>
                                    <fieldset style="position: absolute; left: 345px; visibility: visible; top: 426px;
                                        width: 140px; height: 2px;">
                                        <asp:Button runat="server" ID="btnstaffadd" AutoPostBack="True" Text="Ok" Font-Bold="true"
                                            OnClick="btnstaffadd_Click" Style="width: 75px; top: 2px; position: absolute;
                                            left: 2px;" />
                                        <asp:Button runat="server" ID="btnexit" AutoPostBack="True" Text="Exit" Font-Bold="true"
                                            OnClick="btnexit_Click" Style="width: 75px; top: 2px; position: absolute; left: 85px;" />
                                    </fieldset>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
                <center>
                    <asp:Panel ID="panel2" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                        BorderWidth="2px" Style="left: 30%; top: 35%; right: 30%; position: absolute;
                        z-index: 3; height: auto; width: auto;">
                        <div id="div3" runat="server" class="PopupHeaderrstud2" visible="false" style="height: 550em;
                            z-index: 2000; width: auto; background-color: rgba(54, 25, 25, .2); top: 0%;
                            left: 0%; height: auto;">
                            <center>
                                <div id="div4" runat="server" class="PopupHeaderrstud2" style="background-color: White;
                                    height: auto; width: 464px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                    left: 28%; right: 39%; top: 27%; padding: 5px; border-radius: 10px;">
                                    <center>
                                        <div>
                                            <asp:Label ID="lblstu1" runat="server" Style="color: Green;" Text="Add Staff To another Deaprtment"
                                                CssClass="fontstyleheader"></asp:Label>
                                        </div>
                                    </center>
                                    <br />
                                    <center>
                                        <asp:GridView ID="gvatte" runat="server" ShowHeader="true" AutoGenerateColumns="False"
                                            OnRowDataBound="gvatte_OnDataBinding" Width="364px" CssClass="font">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Height="30px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Course Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcourse" runat="server" Text='<%# Eval("course")  %>' Style="width: 10px;
                                                            text-align: center;" />
                                                        <%--   onclick="Check_Click1(this);"--%>
                                                        <asp:Label ID="lblsubj" Visible="false" runat="server" Text='<%# Eval("subject_no")%>'
                                                            Style="width: 100px; text-align: center;" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="SelectAll_Checked" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk11" runat="server" CssClass="mycheckbox" /><%--onclick="Check_Click(this);"--%>
                                                        <asp:Label ID="lbldeg" Visible="false" runat="server" Text='<%# Eval("degree_code")%>'
                                                            Style="width: 100px" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </center>
                                    <br />
                                    <center>
                                        <asp:Button ID="Btnok" runat="server" Text="Ok" OnClick="Btnok_Click" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                        <asp:Button ID="Btncancle" runat="server" Text="Cancel" OnClick="Btncancle_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        <asp:Label ID="subjno" runat="server" Text="" Visible="false"></asp:Label>
                                    </center>
                                </div>
                            </center>
                        </div>
                    </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <asp:UpdatePanel ID="upd2" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfsave" runat="server" />
            <asp:ModalPopupExtender ID="mpesave" runat="server" TargetControlID="hfsave" PopupControlID="psave">
            </asp:ModalPopupExtender>
            <asp:Panel ID="psave" runat="server" CssClass="modalPopup" Style="display: none;
                height: 500; width: 500;" DefaultButton="btnsaveok">
                <table width="500">
                    <tr class="topHandle">
                        <td colspan="2" align="left" runat="server" id="td1">
                            <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                                Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 60px" valign="middle" align="center">
                            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Info-48x48.png" />
                        </td>
                        <td valign="middle" align="left">
                            <asp:Label ID="Label7" Text="Already allocate the batch for this class.You want to save this changes means, you should re-allocate the batches.Do you want to continue?"
                                runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnsaveok" runat="server" Text="Yes" OnClick="btnsaveok_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                            <asp:Button ID="btnsaveCancel" runat="server" Text="No" OnClick="btnsaveCancel_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="btnsaveok" />
        </Triggers>--%>
    </asp:UpdatePanel>
    <center>
        <asp:UpdatePanel ID="upd3" runat="server">
            <ContentTemplate>
                <div id="divPopSearchstudent" runat="server" visible="false" style="height: 550em;
                    z-index: 2000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0%; left: 0%;">
                    <center>
                        <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 50%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 39%; right: 50%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td>
                                            <table id="tblSearchStudent" runat="server" visible="true">
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlSearchBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged"
                                                            Font-Bold="true">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblValue" runat="server" Style="display: none;"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <div id="divSearchStudent">
                                                            <asp:TextBox ID="txtRollNo" runat="server" Font-Names="Book Antiqua" Width="300px"
                                                                Font-Size="Medium" Visible="false"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=","
                                                                Enabled="True" ServiceMethod="GetRollNo" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtRollNo"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan" OnClientPopulating="AutoCompleteExtender1_OnClientPopulating">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txtRegNo" runat="server" Font-Names="Book Antiqua" Width="300px"
                                                                Font-Size="Medium" Visible="false"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=","
                                                                Enabled="True" ServiceMethod="GetRegNo" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtRegNo"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan" OnClientPopulating="AutoCompleteExtender1_OnClientPopulating">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txtAdmissionNo" runat="server" Font-Names="Book Antiqua" Width="300px"
                                                                Font-Size="Medium" Visible="false"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=","
                                                                Enabled="True" ServiceMethod="GetAdmitNo" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtAdmissionNo"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan" OnClientPopulating="AutoCompleteExtender1_OnClientPopulating">
                                                            </asp:AutoCompleteExtender>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSearchbyrollorreg" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnSearchbyrollorreg_Click"
                                                            Text="Search" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnsearchByClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnsearchByClose_Click"
                                                            Text="Close" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
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
        <asp:UpdatePanel ID="Upanel" runat="server">
            <ContentTemplate>
                <div id="div5" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div6" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                            right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="Label3" runat="server" Visible="false"></asp:Label>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label4" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"
                                                Visible="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="upanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="Button3" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose1_Click"
                                                            Text="Ok" runat="server" />
                                                        <asp:Button ID="Button4" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnCancel1_Click"
                                                            Text="Cancel" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </center>
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
        <asp:UpdatePanel ID="Uupanel1" runat="server">
            <ContentTemplate>
                <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                            right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <asp:Label ID="lblSubNo" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblStaffCode" runat="server" Visible="false"></asp:Label>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                                    Text="Ok" runat="server" />
                                                <asp:Button ID="btnCancel" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnCancel_Click"
                                                    Text="Cancel" runat="server" />
                                            </center>
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
        <asp:UpdatePanel ID="UpadatePanel5" runat="server">
            <ContentTemplate>
                <div id="div8" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div9" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                            right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label5" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnpopupalert" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    AutoPostBack="False" CssClass="textbox textbox1" Style="height: auto; width: auto;"
                                                    OnClick="btnpopupalert_Click" Text="Ok" runat="server" />
                                            </center>
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
