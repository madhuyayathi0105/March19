<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWizardMod/MasterWizard.master"
    AutoEventWireup="true" CodeFile="Semester Information.aspx.cs" Inherits="Semester_Information" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function QuantityChange1() {
            var cun = 0;
            var daycun = 0;
            for (var i = 0; i < 7; i++) {
                var ddl_select = document.getElementById('MainContent_cblholiday_' + i.toString());
                if (ddl_select.checked == true) {
                    cun++;
                }

            }
            daycun = 7 - cun;
            var lblAmt4 = document.getElementById("<%=txtholiday.ClientID %>");
            lblAmt4.value = "Holiday(" + cun + ")";
            var lblAmt3 = document.getElementById("<%=txtdays.ClientID %>");
            lblAmt3.value = daycun;
            var cun5 = 0;
            var daycun = 0;
            var id = document.getElementById("<%=txt_startdate.ClientID %>");
            var from = id.value;
            var end = document.getElementById("<%=txt_enddate.ClientID %>");
            var to = end.value;
            var sp = from.split("/");
            from = sp[1] + '/' + sp[0] + '/' + sp[2];
            var sp1 = to.split("/");
            to = sp1[1] + '/' + sp1[0] + '/' + sp1[2];
            var varDate = new Date(from);
            var ToDate = new Date(to);
            var d = new Date(from);
            var month = (d.getMonth() + 1);
            var day = d.getDate();
            var year = d.getFullYear();
            var m = month.length;
            if (month < 10) month = '0' + month;
            if (day < 10) day = '0' + day;
            var dto = new Date(to);
            var monthto = (dto.getMonth() + 1);
            var dayto = dto.getDate();
            var yearto = dto.getFullYear();
            if (monthto < 10) monthto = '0' + monthto;
            if (dayto < 10) dayto = '0' + dayto;
            var fndate = month + '/' + day + '/' + year;
            var tndate = monthto + '/' + dayto + '/' + yearto;
            if (new Date(fndate).getTime() > new Date(tndate).getTime()) {
                alert("The Start Date must be Less to End date");
                return false;
            }
            else {
                var diffc = new Date(fndate).getTime() - new Date(tndate).getTime();
                var days = Math.round(Math.abs(diffc / (1000 * 60 * 60 * 24)));
                days += 1;


                var loopDate = new Date(fndate);
                while (loopDate < new Date(tndate)) {
                    var dayss = loopDate.getDay();
                    for (var is = dayss; is < dayss + 1; is++) {
                        var ddl_select = document.getElementById('MainContent_cblholiday_' + is.toString());
                        if (ddl_select.checked == true) {
                            cun5++;
                        }
                    }
                    var newDate = loopDate.setDate(loopDate.getDate() + 1);

                }
                var lblAmt5 = document.getElementById("<%=txt_work.ClientID %>");
                lblAmt5.value = days - cun5;
                document.getElementById("<%=hidwork.ClientID %>").value = days - cun5;

                var ddl_selects = document.getElementById('MainContent_gdayset_txt_gviewfrom_0');
                var val = ddl_selects.value;
                var lblAmt7 = document.getElementById("<%=txt_work.ClientID %>");
                var work = lblAmt7.value;
                document.getElementById("<%=hidwork.ClientID %>").value = work;
                var lblhor = document.getElementById("<%=txt_hour.ClientID %>");
                lblhor.value = val * work;
                document.getElementById("<%=hid.ClientID %>").value = val * work;


            }
            return true;

        }
        function QuantityChange2() {
            var id = document.getElementById("<%=ddlschedule.ClientID %>");
            var tosem = id.options[id.selectedIndex].text;
            if (tosem != "Day Order")
                document.getElementById("<%=ddlStartday.ClientID %>").disabled = true;
            else
                document.getElementById("<%=ddlStartday.ClientID %>").disabled = false;
        }
        function thour() {
            var ddl_select = document.getElementById('MainContent_gdayset_txt_gviewfrom_0');
            var val = ddl_select.value;
            var lblAmt4 = document.getElementById("<%=txt_work.ClientID %>");
            var work = lblAmt4.value;
            document.getElementById("<%=hidwork.ClientID %>").value = work;
            var lblhor = document.getElementById("<%=txt_hour.ClientID %>");
            lblhor.value = val * work;
            document.getElementById("<%=hid.ClientID %>").value = val * work;
        }
        function tdate() {
            var cun = 0;
            var daycun = 0;
            var id = document.getElementById("<%=txt_startdate.ClientID %>");
            var from = id.value;
            var end = document.getElementById("<%=txt_enddate.ClientID %>");
            var to = end.value;
            var sp = from.split("/");
            from = sp[1] + '/' + sp[0] + '/' + sp[2];
            var sp1 = to.split("/");
            to = sp1[1] + '/' + sp1[0] + '/' + sp1[2];
            var varDate = new Date(from);
            var ToDate = new Date(to);
            var d = new Date(from);
            var month = (d.getMonth() + 1);
            var day = d.getDate();
            var year = d.getFullYear();
            var m = month.length;
            if (month < 10) month = '0' + month;
            if (day < 10) day = '0' + day;
            var dto = new Date(to);
            var monthto = (dto.getMonth() + 1);
            var dayto = dto.getDate();
            var yearto = dto.getFullYear();
            if (monthto < 10) monthto = '0' + monthto;
            if (dayto < 10) dayto = '0' + dayto;
            var fndate = month + '/' + day + '/' + year;
            var tndate = monthto + '/' + dayto + '/' + yearto;
            if (new Date(fndate).getTime() > new Date(tndate).getTime()) {
                alert("The Start Date must be Less to End date");
                return false;
            }
            else {
                var diffc = new Date(fndate).getTime() - new Date(tndate).getTime();
                var days = Math.round(Math.abs(diffc / (1000 * 60 * 60 * 24)));
                days += 1;


                var loopDate = new Date(fndate);
                while (loopDate < new Date(tndate)) {
                    var dayss = loopDate.getDay();
                    for (var i = dayss; i < dayss + 1; i++) {
                        var ddl_select = document.getElementById('MainContent_cblholiday_' + i.toString());
                        if (ddl_select.checked == true) {
                            cun++;
                        }
                    }
                    var newDate = loopDate.setDate(loopDate.getDate() + 1);

                }
                var lblAmt4 = document.getElementById("<%=txt_work.ClientID %>");
                lblAmt4.value = days - cun;
                document.getElementById("<%=hidwork.ClientID %>").value = days - cun;
                var ddl_selects = document.getElementById('MainContent_gdayset_txt_gviewfrom_0');
                var val = ddl_selects.value;
                var lblAmt7 = document.getElementById("<%=txt_work.ClientID %>");
                var work = lblAmt7.value;
                document.getElementById("<%=hidwork.ClientID %>").value = work;
                var lblhor = document.getElementById("<%=txt_hour.ClientID %>");
                lblhor.value = val * work;
                document.getElementById("<%=hid.ClientID %>").value = val * work;

            }
            return true;
        }
      

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="Label2" runat="server" Text="Semester Information" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="xx-large" ForeColor="Green"></asp:Label>
    </center>
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <table class="maintablestyle" style="width: 700px; height: 40px; background-color: #0CA6CA;">
                    <tr>
                        <td>
                            <asp:Label ID="Lblclg" runat="server" Text="College" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlclg" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Width="200px" CausesValidation="True"
                                OnSelectedIndexChanged="ddlclg_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" CausesValidation="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox txtheight3 textbox1"
                                ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                            <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                                <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="cb_degree_checkedchange" />
                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_degree"
                                PopupControlID="p1" Position="Bottom">
                            </asp:PopupControlExtender>
                            <%--                      </ContentTemplate>
                    <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                        Font-Bold="True" Font-Names="Book Antiqua" Width="100px" Font-Size="Medium" CausesValidation="True"
                        OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                    </asp:DropDownList>--%>
                        </td>
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox txtheight3 textbox1"
                                ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                            <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                                <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="cb_branch_checkedchange" />
                                <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_branch"
                                PopupControlID="Panel1" Position="Bottom">
                            </asp:PopupControlExtender>
                            <%--<asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" Width="245px">
                    </asp:DropDownList>--%>
                        </td>
                        <td>
                            <asp:Label ID="lblsem" runat="server" Text="Semester " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox txtheight3 textbox1" ReadOnly="true"
                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                            <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                                <asp:CheckBox ID="ch_sem" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="ch_sem_checkedchange" />
                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_sem"
                                PopupControlID="Panel2" Position="Bottom">
                            </asp:PopupControlExtender>
                            <%-- <asp:DropDownList ID="ddlsem" runat="server" AutoPostBack="True" Height="21px"
                        Font-Bold="True" Font-Names="Book Antiqua" Width="100px" Font-Size="Medium" CausesValidation="True"
                        >
                    </asp:DropDownList>--%>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Go" OnClick="btnGo_Click" /></ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Add" OnClick="btnadd_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                </center>
                <br />
                <center>
                    <asp:Label ID="lblexer" runat="server" Font-Size="Medium" Font-Bold="true" ForeColor="Red"></asp:Label>
                </center>
                <br />
                <center>
                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="gview" runat="server" ShowHeader="false" Width="1000" OnSelectedIndexChanged="gview_onselectedindexchanged"
                        OnRowCreated="OnRowCreated" BackColor="AliceBlue">
                        <%--onchange="QuantityChange1(this)"--%>
                        <Columns>
                        </Columns>
                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                        <RowStyle ForeColor="#333333" />
                    </asp:GridView>
                </center>
                <br />

                  <center>
                    <asp:Label ID="Lblreport" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcl" runat="server" onkeypress="display()" Height="20px" Width="180px"
                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:TextBox>
                    <asp:Button ID="btnxcl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        OnClick="btnxcl_click" Font-Size="Medium" />
                    <asp:Button ID="btnprnt" runat="server" Text="Print" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true" OnClick="btnprnt_Click" />
                    <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                    <asp:Label ID="lblerr" runat="server" Font-Size="Medium" Font-Bold="true" ForeColor="Red"></asp:Label>
                </center>
                </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnGo" />
                <asp:PostBackTrigger ControlID="btnxcl" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="up3" runat="server"><ContentTemplate>
                <div id="poperrjs" runat="server" visible="false" class="popupstyle popupheight3"
                    style="height: auto;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 20px; margin-left: 1202px;"
                        OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <center>
                        <div class="subdivstyle" style="background-color: White; height: 1080px; width: 1095px;">
                            <br />
                            <div>
                                <center>
                                    <span style="color: Green; font-size: large;">Semester Details</span>
                                </center>
                            </div>
                            <br />
                            <table class="maintablestyle" style="width: 700px; height: 40px; background-color: #0CA6CA;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcoll" runat="server" Text="College" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcol" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Width="200px" CausesValidation="True"
                                            OnSelectedIndexChanged="ddlcoll_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbatc" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatc" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" CausesValidation="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldeg" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdeg" runat="server" CssClass="textbox txtheight3 textbox1" ReadOnly="true"
                                            onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                                            <asp:CheckBox ID="cbdegree" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbdegree_checkedchange" />
                                            <asp:CheckBoxList ID="cbldegree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbldegree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtdeg"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbran" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtbran" runat="server" CssClass="textbox txtheight3 textbox1" ReadOnly="true"
                                            onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel5" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                                            <asp:CheckBox ID="cbbranch" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbbranch_checkedchange" />
                                            <asp:CheckBoxList ID="cblbranch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblbranch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtbran"
                                            PopupControlID="Panel5" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblseme" runat="server" Text="Semester " Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsem" runat="server" AutoPostBack="True" Height="21px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Width="100px" Font-Size="Medium" CausesValidation="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <asp:Panel ID="pheaderfilter" runat="server" BackColor="#719DDB" Width="1043px" Style="margin-left: 17px;">
                                <asp:Label ID="Labelfilter" Text="Semester Information" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                            <asp:Panel ID="pbodyfilter" runat="server" CssClass="cpBody" Style="margin-left: 193px;">
                                <table>
                                    <tr>
                                        <td>
                                            <%--  <tr>
           <%--  <fieldset>--%>
                                            <%--       <center>--%>
                                            <table class="maintablestyle" style="margin: 0px; font-family: Book Antiqua; margin-bottom: 0px;
                                                position: relative; width: 591px; height: 230px; margin-left: -218px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_startdate" runat="server" Text="Semester Start Date"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_startdate" CssClass="textbox textbox1 txtheight1" Width="100px"
                                                            runat="server" onchange="tdate()"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_startdate" Format="dd/MM/yyyy"
                                                            runat="server">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_enddate" runat="server" Text="Semester End Date"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_enddate" CssClass="textbox textbox1 txtheight1" Width="100px"
                                                            runat="server" onchange="tdate()"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_enddate" Format="dd/MM/yyyy"
                                                            runat="server">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_Holiday" runat="server" Text="Holiday"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtholiday" runat="server" CssClass="textbox txtheight3 textbox1"
                                                            ReadOnly="true" Width="101px">--Select--</asp:TextBox>
                                                        <asp:Panel ID="pholiday" runat="server" CssClass="multxtpanel" Width="130px" Height="118px">
                                                            <asp:CheckBoxList ID="cblholiday" runat="server" Font-Size="Medium" Style="font-family: 'Book Antiqua'"
                                                                Font-Bold="True" Font-Names="Book Antiqua" Height="40px" onclick="return QuantityChange1(this)">
                                                                <asp:ListItem>Sunday </asp:ListItem>
                                                                <asp:ListItem>Monday </asp:ListItem>
                                                                <asp:ListItem>Tuesday </asp:ListItem>
                                                                <asp:ListItem>Wednesday </asp:ListItem>
                                                                <asp:ListItem>Thursday </asp:ListItem>
                                                                <asp:ListItem>Friday </asp:ListItem>
                                                                <asp:ListItem>Saturday </asp:ListItem>
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txtholiday"
                                                            PopupControlID="pholiday" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_schedule" runat="server" Text="Schedule Order"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlschedule" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" CausesValidation="True" Width="115px" onchange="return QuantityChange2(this)">
                                                            <asp:ListItem>Day Order</asp:ListItem>
                                                            <asp:ListItem>Week Days</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_days" runat="server" Text="No.of Days/Week"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtdays" BackColor="#ffffcc" Enabled="false" Text="7" CssClass="textbox textbox1 txtheight1"
                                                            Width="115px" runat="server"></asp:TextBox>
                                                        <%--  <asp:DropDownList ID="ddldays" runat="server"  Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Enabled="false" CausesValidation="True" Width="101px">
                         
                    </asp:DropDownList>--%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Lblstartday" runat="server" Text="Starting Dayorder"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlStartday" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" CausesValidation="True" Width="121px">
                                                            <asp:ListItem>Day Order1</asp:ListItem>
                                                            <asp:ListItem>Day Order2</asp:ListItem>
                                                            <asp:ListItem>Day Order3</asp:ListItem>
                                                            <asp:ListItem>Day Order4</asp:ListItem>
                                                            <asp:ListItem>Day Order5</asp:ListItem>
                                                            <asp:ListItem>Day Order6</asp:ListItem>
                                                            <asp:ListItem>Day Order7</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_work" runat="server" Text="No.of Working Days"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_work" CssClass="textbox textbox1 txtheight1" Width="115px" runat="server"
                                                            BackColor="#ffffcc" Enabled="false"></asp:TextBox>
                                                        <input type="hidden" runat="server" id="hidwork" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_hour" runat="server" Text="No.of Working Hours"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_hour" BackColor="#ffffcc" Enabled="false" CssClass="textbox textbox1 txtheight1"
                                                            Width="115px" runat="server"></asp:TextBox>
                                                        <input type="hidden" runat="server" id="hid" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--        </center>--%>
                                            <%--</fieldset>--%><%--</tr>--%>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <%--  <div style="float: left; width: 450px; height: 220px;">
                <fieldset id="fildset" runat="server">
                                <legend>Day Order Settings</legend>--%>
                                                    <asp:GridView ID="gdayset" runat="server" ShowHeader="true" AutoGenerateColumns="false"
                                                        Height="222px">
                                                        <%--onchange="QuantityChange1(this)"--%>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_sno" runat="server" Text='<%# Eval("Header") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total No.of Hours">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_gviewfrom" runat="server" Text="0" onchange="thour()" Width="63px"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txt_gviewfrom"
                                                                        FilterType="numbers,custom" ValidChars=" ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Min Hours To be Present">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_gviewto" runat="server" Text="0" Width="63px"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_gviewto"
                                                                        FilterType="numbers,custom" ValidChars=" ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                        <RowStyle ForeColor="#333333" />
                                                    </asp:GridView>
                                                    <%--   </fieldset>--%>
                                                    <%--</div><--%></tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pbodyfilter"
                                CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                                TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
                                ExpandedImage="../images/down.jpeg">
                            </asp:CollapsiblePanelExtender>
                            <br />
                            <br />
                            <asp:Panel ID="Panel3" runat="server" BackColor="#719DDB" Width="1043px" Style="margin-left: 17px;">
                                <asp:Label ID="Label1" Text="Attendance Settings" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" />
                                <asp:Image ID="Image1" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                            <asp:Panel ID="Panel7" runat="server" CssClass="cpBody" Style="margin-left: 193px;">
                                <br />
                                <br />
                                <table>
                                    <tr>
                                        <asp:LinkButton ID="myLink" Text="Total Hours Settings as per University" OnClick="LinkButton_Click"
                                            runat="server" />
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset id="fildset" runat="server" style="margin-left: -160px;">
                                                <legend>Attendance Criteria For Internal Mark Calculation</legend>
                                                <asp:Label ID="lblmark" Text="Max Mark" runat="server" Font-Size="Medium" Font-Bold="True"
                                                    Font-Names="Book Antiqua" />
                                                <asp:TextBox ID="TextBox1" runat="server" CssClass="textbox txtheight3 textbox1"
                                                    Text="" MaxLength="3"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="TextBox1"
                                                    FilterType="numbers,custom" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                                <span>%</span>
                                                <asp:Button ID="btnaddrow" runat="server" Text="Add Row" CssClass="textbox btn2"
                                                    OnClick="btnaddrow_Click" BackColor="#0cf337" />
                                                <br />
                                                <br />
                                                <asp:GridView ID="gattn" runat="server" ShowHeader="true" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="From(%)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_gviewfrom" runat="server" Text='<%# Eval("frange") %>'></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_gviewfrom"
                                                                    FilterType="numbers,custom" ValidChars=" .">
                                                                </asp:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To(%)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_gviewto" runat="server" Text='<%# Eval("trange") %>'></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_gviewto"
                                                                    FilterType="numbers,custom" ValidChars=" .">
                                                                </asp:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mark">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_gviewmark" runat="server" Text='<%# Eval("attnd_mark") %>'></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_gviewmark"
                                                                    FilterType="numbers,custom" ValidChars=" .">
                                                                </asp:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                    <RowStyle ForeColor="#333333" />
                                                </asp:GridView>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset id="Fieldset1" runat="server" style="margin-left: 83px; margin-top: -6px">
                                                <legend>Attendance Eligibility Settings For Current Exam</legend>
                                                <asp:GridView ID="genligi" runat="server" ShowHeader="true" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_sno" runat="server" Text='<%# Eval("Header") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Min.% Eligible">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_gviewMin" runat="server" Text="0"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_gviewMin"
                                                                    FilterType="numbers,custom" ValidChars=" .">
                                                                </asp:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                    <RowStyle ForeColor="#333333" />
                                                </asp:GridView>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="Panel7"
                                CollapseControlID="Panel3" ExpandControlID="Panel3" Collapsed="true" TextLabelID="Label1"
                                CollapsedSize="0" ImageControlID="Image1" CollapsedImage="../images/right.jpeg"
                                ExpandedImage="../images/down.jpeg">
                            </asp:CollapsiblePanelExtender>
                            <br />
                            <br />
                            <asp:Panel ID="Panel10" runat="server" BackColor="#719DDB" Style="margin-left: 17px;
                                width: 1058px;">
                                <asp:Label ID="Label4" Text="Grade Settings" runat="server" Font-Size="Medium" Font-Bold="True"
                                    Font-Names="Book Antiqua" />
                                <asp:Image ID="Image3" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                            <asp:Panel ID="Panel11" runat="server" CssClass="cpBody" Style="margin-left: 193px;">
                                <table>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="Gvgrade" runat="server" ShowHeader="true" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="From">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_gviewfrom" runat="server" Text='<%# Eval("from") %>'></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_gviewfrom"
                                                                FilterType="numbers,custom" ValidChars=" .">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="To">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_gviewto" runat="server" Text='<%# Eval("To") %>'></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_gviewto"
                                                                FilterType="numbers,custom" ValidChars=" .">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Mark Grade">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_gviewmark" runat="server" Text='<%# Eval("Mark") %>'></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_gviewmark"
                                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Point">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_gviewPoint" runat="server" Text='<%# Eval("Point") %>'></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_gviewPoint"
                                                                FilterType="numbers,custom" ValidChars=" .">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                                <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                <RowStyle ForeColor="#333333" />
                                            </asp:GridView>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnrow" runat="server" Text="Add Row" CssClass="textbox btn2" OnClick="btnrow_Click"
                                                BackColor="#0cf337" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="Panel11"
                                CollapseControlID="Panel10" ExpandControlID="Panel10" Collapsed="true" TextLabelID="Label4"
                                CollapsedSize="0" ImageControlID="Image3" CollapsedImage="../images/right.jpeg"
                                ExpandedImage="../images/down.jpeg">
                            </asp:CollapsiblePanelExtender>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="textbox btn2"
                                                    OnClientClick="return valid2()" OnClick="btn_update_Click" Visible="false" BackColor="#c288d8" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                                                    OnClientClick="return valid2()" OnClick="btn_delete_Click" Visible="false" BackColor="#8ae02d" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btn_save" runat="server" Text="Save" Visible="false" OnClick="btn_save_Click"
                                                    CssClass="textbox btn2" OnClientClick="return valid2()" BackColor="#0ce3f3" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit_Click"
                                            BackColor="#0cf337" />
                                    </td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
                </ContentTemplate></asp:UpdatePanel>
                <br />
              
                <center>
                <asp:UpdatePanel ID="up6" runat="server"><ContentTemplate>
                    <div id="duni" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="divuni" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 511px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblclghrs" runat="server" Text="Total Number Of Actual Working Hours"
                                                    Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtclghrs" Text="" CssClass="textbox textbox1 txtheight1" Width="115px"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbluni" runat="server" Text="Total Number Of  Hours As Per University Norms"
                                                    Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtunihrs" Text="" CssClass="textbox textbox1 txtheight1" Width="115px"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" CssClass="textbox btn2"
                                                        BackColor="#0ce3f3" />
                                                    <asp:Button ID="btnexit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btnexit_Click"
                                                        BackColor="#0cf337" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                    </ContentTemplate></asp:UpdatePanel>
                </center>
            
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <center>
                    <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
        <center>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel25">
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
        <center>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
        <center>
            <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
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
            <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
                PopupControlID="UpdateProgress3">
            </asp:ModalPopupExtender>
        </center>
        <center>
            <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
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
            <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress4"
                PopupControlID="UpdateProgress4">
            </asp:ModalPopupExtender>
        </center>
        <center>
            <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanel4">
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
            <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress5"
                PopupControlID="UpdateProgress5">
            </asp:ModalPopupExtender>
        </center>
    </center>
    <br />
</asp:Content>
