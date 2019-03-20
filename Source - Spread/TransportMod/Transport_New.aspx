<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Transport_New.aspx.cs" Inherits="Default6" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="Styles/css/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function vehicltype() {
            document.getElementById('<%=vehicleadd.ClientID%>').style.display = 'block';
            document.getElementById('<%=vehicleremove.ClientID%>').style.display = 'block';
        }
        function vehiclepur() {
            document.getElementById('<%=vehiclepuradd.ClientID%>').style.display = 'block';
            document.getElementById('<%=vehiclepurremove.ClientID%>').style.display = 'block';
        }
        function dealer() {
            document.getElementById('<%=btnadddealer.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnremovedealer.ClientID%>').style.display = 'block';
        }
        function state() {
            document.getElementById('<%=statertoadd.ClientID%>').style.display = 'block';
            document.getElementById('<%=statertoremove.ClientID%>').style.display = 'block';
        }
        function change() {
            var tbvehiid;
            tbvehiid = document.getElementById('<%=tbvehiid.ClientID%>');
            if (tbvehiid.value == "") {
                tbvehiid.style.backgroundColor = "LightYellow";
            }
            else {
                tbvehiid.style.backgroundColor = "LightYellow";
            }

        }

        function changepur() {
            var tbpuron;
            tbpuron = document.getElementById('<%=tbpuron.ClientID%>');
            if (tbpuron.value == "") {
                tbpuron.style.backgroundColor = "LightYellow";
            }
            else {
                tbpuron.style.backgroundColor = "LightYellow";
            }
        }

        function changeamount() {
            var tbtotalpuramount;
            tbtotalpuramount = document.getElementById('<%=tbtotalpuramount.ClientID%>');
            if (tbtotalpuramount.value == "") {
                tbtotalpuramount.style.borderColor = "LightYellow";
            }
            else {
                tbtotalpuramount.style.backgroundColor = "LightYellow";
            }

        }

        function changeinsu() {
            var tbinsurance;
            tbinsurance = document.getElementById('<%=tbinsurance.ClientID%>')

            if (tbinsurance.value == "") {
                tbinsurance.style.backgroundColor = "LightYellow";
            }
            else {
                tbinsurance.style.backgroundColor = "LightYellow";
            }




        }
        function changetax() {
            var tbtax;
            tbtax = document.getElementById('<%=tbtax.ClientID%>');
            if (tbtax.value == "") {
                tbtax.style.backgroundColor = "LightYellow";
            }
            else {
                tbtax.style.backgroundColor = "LightYellow";
            }
        }

        function changecast() {
            var tbvehiclecast;
            tbvehiclecast = document.getElementById('<%=tbvehiclecast.ClientID%>');
            if (tbvehiclecast.value == "") {
                tbvehiclecast.style.backgroundColor = "LightYellow";
            }
            else {
                tbvehiclecast.style.backgroundColor = "LightYellow";
            }


        }
        function changepur1() {
            var tbnoowner;
            tbnoowner = document.getElementById('<%=tbnoowner.ClientID%>');
            if (tbnoowner.value == "") {
                tbnoowner.style.backgroundColor = "LightYellow";
            }
            else {
                tbnoowner.style.backgroundColor = "LightYellow";
            }

        }
        function changepu2r() {
            var tbrcno;
            tbrcno = document.getElementById('<%=tbrcno.ClientID%>');
            if (tbrcno.value == "") {
                tbrcno.style.backgroundColor = "LightYellow";
            }
            else {
                tbrcno.style.backgroundColor = "LightYellow";
            }
        }
        function changepur3() {
            var tbregdate;
            tbregdate = document.getElementById('<%=tbregdate.ClientID%>');
            if (tbregdate.value == "") {
                tbregdate.style.backgroundColor = "LightYellow";
            }
            else {
                tbregdate.style.backgroundColor = "LightYellow";
            }
        }
        function changepur4() {
            var tbregno;
            tbregno = document.getElementById('<%=tbregno.ClientID%>');
            if (tbregno.value == "") {
                tbregno.style.backgroundColor = "LightYellow";
            }
            else {
                tbregno.style.backgroundColor = "LightYellow";
            }
        }
        //    function changepur5() {
        //        document.getElementById('<%=ddlvehicletype.ClientID%>')
        //    }
        function changepur6() {
            var tbplacereg;
            tbplacereg = document.getElementById('<%=tbplacereg.ClientID%>');
            if (tbplacereg.value == "") {
                tbplacereg.style.backgroundColor = "LightYellow";
            }
            else {
                tbplacereg.style.backgroundColor = "LightYellow";
            }
        }


        function changeduration() {
            var tbduration;
            tbduration = document.getElementById('<%=tbduration.ClientID%>');
            if (tbduration.value == "") {
                tbduration.style.backgroundColor = "LightYellow";
            }
            else {
                tbduration.style.backgroundColor = "LightYellow";
            }
        }
        function changecapacity() {
            var tbseatcapacity;
            tbseatcapacity = document.getElementById('<%=tbseatcapacity.ClientID%>');
            if (tbseatcapacity.value == "") {
                tbseatcapacity.style.backgroundColor = "LightYellow";
            }
            else {
                tbseatcapacity.style.backgroundColor = "LightYellow";
            }
        }
        function changemaxallowed() {
            var tbmaxallowed;
            tbmaxallowed = document.getElementById('<%=tbmaxallowed.ClientID%>');
            if (tbmaxallowed.value == "") {
                tbmaxallowed.style.backgroundColor = "LightYellow";
            }
            else {
                tbmaxallowed.style.backgroundColor = "LightYellow";
            }
        }
        function changeinit() {
            var tbintial;
            tbintial = document.getElementById('<%=tbintial.ClientID%>');
            if (tbintial.value == "") {
                tbintial.style.backgroundColor = "LightYellow";
            }
            else {
                tbintial.style.backgroundColor = "LightYellow";
            }
        }
        function changerenew() {
            var tbrenewdate;
            tbrenewdate = document.getElementById('<%=tbrenewdate.ClientID%>');
            if (tbrenewdate.value == "") {
                tbrenewdate.style.backgroundColor = "LightYellow";
            }
            else {
                tbrenewdate.style.backgroundColor = "LightYellow";
            }
        }
        function changetotal() {
            var tbtotaltravel;
            tbtotaltravel = document.getElementById('<%=tbtotaltravel.ClientID%>');
            if (tbtotaltravel.value == "") {
                tbtotaltravel.style.borderColor = "LightYellow";
            }
            else {
                tbtotaltravel.style.borderColor = "LightYellow";
            }
        }
        function changestudent() {
            var tbstudent;
            tbstudent = document.getElementById('<%=tbstudent.ClientID%>');
            if (tbstudent.value == "") {
                tbstudent.style.backgroundColor = "LightYellow";
            }
            else {
                tbstudent.style.backgroundColor = "LightYellow";
            }
        }
        function changestaff() {
            var tbstaff;
            tbstaff = document.getElementById('<%=tbstaff.ClientID%>');
            if (tbstaff.value == "") {
                tbstaff.style.backgroundColor = "LightYellow";
            }
            else {
                tbstaff.style.backgroundColor = "LightYellow";
            }
        }
        function changeenginno() {
            var tbenginno;
            tbenginno = document.getElementById('<%=tbenginno.ClientID%>');
            if (tbenginno.value == "") {
                tbenginno.style.borderColor = "LightYellow";
            }
            else {
                tbenginno.style.borderColor = "LightYellow";
            }
        }
        function changemanudate() {
            var tbmanudate;
            tbmanudate = document.getElementById('<%=tbmanudate.ClientID%>');
            if (tbmanudate.value == "") {
                tbmanudate.style.backgroundColor = "LightYellow";
            }
            else {
                tbmanudate.style.backgroundColor = "LightYellow";
            }
        }
        //    function changedealer() {
        //        document.getElementById('<%=ddldealerdetails.ClientID%>');
        //    }
        function changeaddress1() {
            var tbaddress1;
            tbaddress1 = document.getElementById('<%=tbaddress1.ClientID%>');
            if (tbaddress1.value == "") {
                tbaddress1.style.backgroundColor = "LightYellow";
            }
            else {
                tbaddress1.style.backgroundColor = "LightYellow";
            }
        }
        function changeadd2() {
            var tbaddress2;
            tbaddress2 = document.getElementById('<%=tbaddress2.ClientID%>');
            if (tbaddress2.value == "") {
                tbaddress2.style.backgroundColor = "LightYellow";
            }
            else {
                tbaddress2.style.backgroundColor = "LightYellow";
            }
        }
        function changecity() {
            var tbcityrto;
            tbcityrto = document.getElementById('<%=tbcityrto.ClientID%>');
            if (tbcityrto.value == "") {
                tbcityrto.style.backgroundColor = "LightYellow";
            }
            else {
                tbcityrto.style.backgroundColor = "LightYellow";
            }
        }
        //    function changepstate() {
        //        document.getElementById('<%=ddlstaterto.ClientID%>')
        //    }
        function changepincode() {
            var tbpincoderto;
            tbpincoderto = document.getElementById('<%=tbpincoderto.ClientID%>');
            if (tbpincoderto.value == "") {
                tbpincoderto.style.backgroundColor = "LightYellow";
            }
            else {
                tbpincoderto.style.backgroundColor = "LightYellow";
            }
        }

        function changconrtact() {
            var tbrtocontact;
            tbrtocontact = document.getElementById('<%=tbrtocontact.ClientID%>');
            if (tbrtocontact.value == "") {
                tbrtocontact.style.backgroundColor = "LightYellow";
            }
            else {
                tbrtocontact.style.backgroundColor = "LightYellow";
            }
        }

        function changenumber() {
            var tbcontactnumber;
            tbcontactnumber = document.getElementById('<%=tbcontactnumber.ClientID%>');
            if (tbcontactnumber.value == "") {
                tbcontactnumber.style.backgroundColor = "LightYellow";
            }
            else {
                tbcontactnumber.style.backgroundColor = "LightYellow";
            }
        }


    </script>


    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table style="width: 946px">
        <tr>
            <td align="left">
                <asp:Panel ID="pnl4" runat="server" BackImageUrl="~/image/Top Band-2.jpg" Height="20px"
                    Style="margin-left: 0px; top: 75px; left: -23px; width: 1027px; position: absolute;">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label31" runat="server" Text="Vehicle Master" Font-Bold="true" Font-Names="MS Sans Serif"
                        Font-Size="Medium" ForeColor="White"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <%-- <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="MS Sans Serif" Font-Size="Small"
                        ForeColor="White" Font-Bold="true" PostBackUrl="~/Default_login.aspx">Home</asp:LinkButton>
                    &nbsp; &nbsp;
                    <asp:LinkButton ID="LinkButton3" runat="server" Font-Names="MS Sans Serif" Font-Size="Small"
                        ForeColor="White" Font-Bold="true" PostBackUrl="~/Default_login.aspx">Back</asp:LinkButton>
                    &nbsp; &nbsp;
                    <asp:LinkButton ID="lb2" runat="server" OnClick="lb2_Click" Font-Bold="true" Font-Names="MS Sans Serif"
                        Font-Size="Small" ForeColor="White">Logout</asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:Accordion ID="Accordion1" CssClass="accordion" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        runat="server" Width="985px" Height="500px" BorderColor="White" Style="margin-right: 0px;
        margin-left: 15px; margin-top: -9px; height: 500px;">
        <Panes>
            <asp:AccordionPane ID="AccordionPane1" runat="server">
                <Header>
                    View</Header>
                <Content>
                
                    <%--<asp:UpdatePanel ID="updateview" runat="server">
         <ContentTemplate>--%>
                    <asp:Label ID="lblerrordate" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                        Font-Size="3pt" Visible="false"></asp:Label>
                    <asp:Panel ID="Panel2" runat="server" Style="border-style: solid; border-width: thin;
                        border-color: Black; background: White;">
                        <br />
                        <table class="tabl" style="width: 486px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblvehicletype" runat="server" Font-Bold="true" CssClass="font" Text="Vehicle Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlvehicletypeview" runat="server" Font-Bold="true" CssClass="font"
                                        Width="122px" AutoPostBack="True" OnSelectedIndexChanged="ddlvehicletypeview_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbltypeview" runat="server" Font-Bold="true" CssClass="font" Text="Vehicle ID"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddltypeview" runat="server" Font-Bold="true" CssClass="font"
                                        Width="122px" AutoPostBack="True" OnSelectedIndexChanged="ddltypeview_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblregno" runat="server" Font-Bold="true" CssClass="font" Text="Registration Number"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlregno" runat="server" Font-Bold="true" CssClass="font" Width="122px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                               
                                    <asp:Button ID="btnMainGo" runat="server" Text="Go" Font-Bold="True" OnClick="btnMainGo_Click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" />

                                        
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%-- </ContentTemplate>
         </asp:UpdatePanel>--%>
                    <asp:Label ID="lblerrmainapp" runat="server" Text="" Visible="false" ForeColor="Red"
                        CssClass="font"></asp:Label>
                    <FarPoint:FpSpread ID="sprdMainEnquiry" runat="server" Height="250px" Width="900px"
                        OnCellClick="sprdMainEnquiry_CellClick" OnPreRender="sprdMainEnquiry_SelectedIndexChanged"
                        ActiveSheetViewIndex="0" currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                        EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
                        <CommandBar BackColor="Control" ButtonType="PushButton">
                            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                        </CommandBar>
                        <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" />
                        <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" />
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                                GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                                SelectionForeColor="White">
                            </FarPoint:SheetView>
                        </Sheets>
                        <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                            VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False">
                        </TitleInfo>
                    </FarPoint:FpSpread>
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                    <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                        
                </Content>
            </asp:AccordionPane>
            <asp:AccordionPane ID="AccordionPane2" runat="server">
                <Header>
                    <asp:Label ID="lbladdview" runat="server" Text="Add"></asp:Label></Header>
                <Content>
                    <asp:UpdatePanel ID="UpdatedAdd" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" Style="left: -147px; border-color: Gray; border-style: solid;
                                width: 978px; height: 445px; margin-bottom: 0px; margin-right: 212px; margin-left: -6px;
                                margin-top: 50px;">
                                <asp:Label ID="lblerrtrans" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                                    Font-Size="3pt" Visible="false"></asp:Label>
                                <table class="tabl" style="top: 231px; left: 34px; position: absolute; width: 301px;
                                    border-color: Gray; border-width: thin; height: 411px;">
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="Label20" runat="server" Text="College" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_college" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="Dropdown_Txt_Box" Height="18px" Width="100px" Style="top: 0px;
                                                        left: 117px; position: absolute;"></asp:TextBox>
                                                    <asp:Panel ID="Panel8" runat="server" CssClass="MultipleSelectionDDL" Height="500"
                                                        Width="600">
                                                        <asp:CheckBox ID="chk_college" runat="server" AutoPostBack="True" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Small" OnCheckedChanged="chk_college_ChekedChanged"
                                                            Text="Select All" />
                                                        <asp:CheckBoxList ID="chklst_college" runat="server" OnSelectedIndexChanged="chklst_college_SelectedIndexChanged"
                                                            AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <br />
                                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_college"
                                                        PopupControlID="Panel8" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lbltypenew" runat="server" Text="Vehicle Type" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="vehicleadd" runat="server" Text="+" Style="display: none; position: absolute;
                                                left: 83px; top: 25px;" Font-Names="MS Sans Serif" Font-Size="Small" Height="21px"
                                                OnClick="vehicleadd_Click" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlvehicletype" runat="server" AutoPostBack="true" Font-Names="MS Sans Serif"
                                                Font-Size="Small" Height="20px" Width="125px" Style="margin-right: 25px; margin-left: 2;"
                                                OnSelectedIndexChanged="ddlvehicletype_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblerrvehicletype" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                                                Font-Size="3pt" Visible="false" Style="top: 26px; position: absolute; left: 118px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" ForeColor="Red" Text="*" Font-Size="Small"
                                                Font-Bold="false" Style="left: 243px; position: absolute;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="vehicleremove" runat="server" Text="-" Style="display: none; position: absolute;
                                                left: 243px; top: 25px;" OnClick="vehicleremove_Click" Font-Names="MS Sans Serif"
                                                Font-Size="Small" Height="21px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblvehicleid" runat="server" Text="Vehicle ID" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbvehiid" runat="server" AutoPostBack="true" OnTextChanged="tbvehiid_TextChanged"
                                                placeholder="Enter Vechicle ID" Font-Names="MS Sans Serif" Font-Size="Small"
                                                Height="15px" Width="120px"></asp:TextBox>
                                            <asp:Label ID="lblerrvehiid" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                                                Font-Size="3pt" Visible="false" Style="top: 58px; position: absolute; left: 115px;"></asp:Label>
                                            <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender31" TargetControlID="tbvehiid" FilterType="Numbers" runat="server">
        </asp:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label86" runat="server" ForeColor="Red" Text="*" Font-Size="Small"
                                                Font-Bold="false" Style="left: 243px; position: absolute;"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblregnumber" runat="server" Text="Registration No" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbregno" runat="server" AutoPostBack="true" Font-Names="MS Sans Serif"
                                                Font-Size="Small" Height="15px" Width="120px"></asp:TextBox>
                                            <%--   <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="tbregno" FilterType="Numbers" runat="server">
        </asp:FilteredTextBoxExtender>--%>
                                        </td>
                                        <td class="style55">
                                            <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*" Font-Size="Small"
                                                Font-Bold="false" Style="left: 243px; position: absolute;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblregdate" runat="server" Text="Reg.Date" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbregdate" runat="server" AutoPostBack="true" OnTextChanged="tbregdate_TextChanged"
                                                Font-Names="MS Sans Serif" Font-Size="Small" Height="15px" Width="75px"></asp:TextBox>
                                            <asp:Label ID="Labelvalidationdate" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                                                Font-Size="3pt" Visible="false" Style="position: absolute; top: 121px; left: 114px;"></asp:Label>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="tbregdate" Format="dd-MM-yyyy"
                                                runat="server" Enabled="True">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td class="style55">
                                            <asp:Label ID="Label10" runat="server" ForeColor="Red" Text="*" Font-Size="Small"
                                                Font-Bold="false" Style="left: 198px; position: absolute;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblrcno" runat="server" Text="RC No" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbrcno" runat="server" OnTextChanged="tbrcno_TextChanged" Font-Names="MS Sans Serif"
                                                Font-Size="Small" Height="15px" Width="120px"></asp:TextBox>
                                            <asp:Label ID="lblerrrcno" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                                                Font-Size="3pt" Visible="false" Style="top: 156px; position: absolute; left: 142px;"></asp:Label>
                                        </td>
                                        <td class="style55">
                                            <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*" Font-Size="Small"
                                                Font-Bold="false" Style="left: 243px; position: absolute;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="Label18" runat="server" Text="Chase No" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="txt_cheese" runat="server" OnTextChanged="tbrcno_TextChanged" Font-Names="MS Sans Serif"
                                                Font-Size="Small" Height="15px" Width="120px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblenginno" runat="server" Text="EngineNo" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbenginno" runat="server" Font-Names="MS Sans Serif" Width="61px"
                                                MaxLength="10" Font-Size="Small" Height="15px"></asp:TextBox>
                                            <asp:Label ID="enqerr" runat="server" Font-Names="MS Sans Serif" Font-Size="3pt"
                                                ForeColor="Red" Text="Errorlabel" Visible="false" Style="position: absolute;
                                                top: 369px; left: 127px;"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lbltype" runat="server" Text="Type" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            &nbsp;
                                            <asp:RadioButton ID="rbnew" Text="New" runat="server" GroupName="vehicle" AutoPostBack="true"
                                                OnCheckedChanged="rbnew_CheckedChanged" Font-Names="MS Sans Serif" Font-Size="Small" />
                                            <asp:RadioButton ID="rbold" Text="Old" runat="server" AutoPostBack="true" OnCheckedChanged="rbold_CheckedChanged"
                                                GroupName="vehicle" Font-Names="MS Sans Serif" Font-Size="Small" />
                                        </td>
                                        <td class="style55">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblnoofowner" runat="server" Text="No Of Owners" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbnoowner" runat="server" Font-Names="MS Sans Serif" Font-Size="Small"
                                                Height="15px" Width="34px" MaxLength="2"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="tbnoowner"
                                                FilterType="Numbers" runat="server">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td class="style55">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblvehicast" runat="server" Text="Vehicle Cost" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbvehiclecast" runat="server" Font-Names="MS Sans Serif" Font-Size="Small"
                                                Height="15px" Width="96px" MaxLength="8" AutoPostBack="true" OnTextChanged="tbvehiclecast_TextChanged"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="tbvehiclecast"
                                                FilterType="Numbers" runat="server">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td class="style55">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lbltax" runat="server" Text="Tax" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbtax" runat="server" Font-Names="MS Sans Serif" Font-Size="Small"
                                                Height="15px" Width="96px" MaxLength="8" AutoPostBack="true" OnTextChanged="tbtax_TextChanged"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="tbtax"
                                                FilterType="Custom" ValidChars="123456789." runat="server">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td class="style55">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblinsurance" runat="server" AutoPostBack="true" Text="Vehicle Insurance"
                                                Style="left: 2px; top: 330px; position: absolute;" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbinsurance" runat="server" Font-Names="MS Sans Serif" Font-Size="Small"
                                                Height="15px" Width="96px" MaxLength="8" AutoPostBack="true" OnTextChanged="tbinsurance_TextChanged"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="tbinsurance"
                                                FilterType="Numbers" runat="server">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td class="style55">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lbltotalAmount" runat="server" Text="Total Pur.Amount" Style="left: 1px;
                                                position: absolute; top: 361px;" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbtotalpuramount" runat="server" Font-Names="MS Sans Serif" Width="120px"
                                                MaxLength="8" Font-Size="Small" Height="15px"></asp:TextBox>
                                            <asp:Label ID="totalamount" runat="server" Font-Names="MS Sans Serif" Font-Size="3pt"
                                                ForeColor="Red" Text="Errorlabel" Visible="false"></asp:Label>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" TargetControlID="tbtotalpuramount"
                                                FilterType="Numbers" runat="server">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td class="style55">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblpurpose" runat="server" Text="Purpose Vehicle" Style="left: 4px;
                                                position: absolute; top: 390px;" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55" align="right">
                                            <asp:Button ID="vehiclepuradd" runat="server" Text="+" Style="display: none; position: absolute;
                                                left: 84px; top: 352px;" Font-Names="MS Sans Serif" Font-Size="Small" Height="21px"
                                                OnClick="vehiclepuradd_Click" />
                                        </td>
                                        <td class="style25" align="left">
                                            &nbsp;
                                            <asp:DropDownList ID="ddlvehiclepur" runat="server" AutoPostBack="true" Font-Names="MS Sans Serif"
                                                Font-Size="Small" Height="20px" Width="127px" Style="margin-right: 25px; margin-left: -5px;"
                                                OnSelectedIndexChanged="ddlvehiclepur_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblvehicleerrpur" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                                                Font-Size="3pt" Visible="false" Style="position: absolute; top: 371px; left: 115px;"></asp:Label>
                                        </td>
                                        <td class="style55" align="right">
                                            <asp:Button ID="vehiclepurremove" runat="server" Text="-" Style="display: none; position: absolute;
                                                left: 244px; top: 352px;" OnClick="vehiclepurremove_Click" Font-Names="MS Sans Serif"
                                                Font-Size="Small" Height="21px" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="Placereg" runat="server">
                        <ContentTemplate>
                            <table class="tabl" style="top: 231px; left: 338px; position: absolute; width: 301px;
                                border-color: Gray; border-width: thin; height: 411px; bottom: 414px;">
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblpuron" runat="server" Text="Purchased On:" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        <asp:TextBox ID="tbpuron" runat="server" Font-Names="MS Sans Serif" Width="67px"
                                            MaxLength="15" Font-Size="Small" Height="15px" AutoPostBack="true" OnTextChanged="tbpuron_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="tbpuron" Format="dd-MM-yyyy"
                                            runat="server" Enabled="True">
                                        </asp:CalendarExtender>
                                        <asp:Label ID="lblerrorpuron" runat="server" Font-Names="MS Sans Serif" Font-Size="3pt"
                                            ForeColor="Red" Text="Errorlabel" Visible="false" Style="top: 410px; position: absolute;
                                            left: 116px;"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblplacereg" runat="server" Text="Place Of Reg:" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        <asp:TextBox ID="tbplacereg" runat="server" Font-Names="MS Sans Serif" Width="120px"
                                            MaxLength="15" Font-Size="Small" Height="15px" OnTextChanged="tbplacereg_TextChanged"></asp:TextBox>
                                        <asp:Label ID="lblerrorplacereg" runat="server" Font-Names="MS Sans Serif" Font-Size="3pt"
                                            ForeColor="Red" Text="Errorlabel" Visible="false" Style="position: absolute;
                                            top: 28px; left: 136px;"></asp:Label>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage="only characters allowed"
                                            ControlToValidate="tbplacereg" SetFocusOnError="True" Font-Size="1pt" Display="Dynamic"
                                            EnableClientScript="True" Style="top: 27px; position: absolute; left: 136px;"
                                            ForeColor="Red" Font-Names="MS Sans Serif" ValidationExpression="^[a-zA-Z]*$"></asp:RegularExpressionValidator>
                                    </td>
                                    <td class="style55">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblduration" runat="server" Text="Duration" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        <asp:TextBox ID="tbduration" runat="server" Font-Names="MS Sans Serif" Width="33px"
                                            MaxLength="2" Font-Size="Small" Height="15px"></asp:TextBox>
                                        <asp:Label ID="Label8" runat="server" Font-Names="MS Sans Serif" Font-Size="small"
                                            ForeColor="Red" Text="Errorlabel" Visible="false"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" TargetControlID="tbduration"
                                            FilterType="Numbers" runat="server">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td class="style55">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblseatcapacity" runat="server" Text="Total No Of Seat Capacity" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        <asp:TextBox ID="tbseatcapacity" runat="server" Font-Names="MS Sans Serif" Width="33px"
                                            MaxLength="3" Font-Size="Small" Height="15px"></asp:TextBox>
                                        <asp:Label ID="Label13" runat="server" Font-Names="MS Sans Serif" Font-Size="small"
                                            ForeColor="Red" Text="Errorlabel" Visible="false"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" TargetControlID="tbseatcapacity"
                                            FilterType="Numbers" runat="server">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td class="style55">
                                        <asp:Label ID="Label16" runat="server" ForeColor="Red" Text="*" Font-Size="Small"
                                            Font-Bold="false" Style="left: 188px; position: absolute;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblmaxallowed" runat="server" Text="If Max Allowed,Extra No" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        <asp:TextBox ID="tbmaxallowed" runat="server" Font-Names="MS Sans Serif" Width="43px"
                                            MaxLength="2" Font-Size="Small" Height="15px"></asp:TextBox>
                                        <asp:Label ID="Label15" runat="server" Font-Names="MS Sans Serif" Font-Size="small"
                                            ForeColor="Red" Text="Errorlabel" Visible="false"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" TargetControlID="tbmaxallowed"
                                            FilterType="Numbers" runat="server">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td class="style55">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblintial" runat="server" Text="Initial Starting Km" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        <asp:TextBox ID="tbintial" runat="server" Font-Names="MS Sans Serif" Width="41px"
                                            Font-Size="Small" Height="15px"></asp:TextBox>
                                        <asp:Label ID="Label17" runat="server" Font-Names="MS Sans Serif" Font-Size="small"
                                            ForeColor="Red" Text="Errorlabel" Visible="false"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="tbintial"
                                            FilterType="Numbers" runat="server">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td class="style55">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblpl" runat="server" Text="Mileage" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        <asp:TextBox ID="txtkm" runat="server" Font-Names="MS Sans Serif" Width="41px" MaxLength="7"
                                            Font-Size="Small" Height="15px"></asp:TextBox>
                                        <asp:Label ID="Label14" runat="server" Font-Names="MS Sans Serif" Font-Size="small"
                                            ForeColor="Red" Text="Errorlabel" Visible="false"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtkm"
                                            FilterType="Custom,Numbers" ValidChars="." runat="server">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td class="style55">
                                        <asp:Label ID="Label11" runat="server" ForeColor="Red" Text="*" Font-Size="Small"
                                            Font-Bold="false" Style="left: 188px; position: absolute;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblrenewdate" runat="server" Text="Renew Date" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        <asp:TextBox ID="tbrenewdate" runat="server" Font-Names="MS Sans Serif" Width="72px"
                                            Font-Size="Small" Height="15px"></asp:TextBox>
                                        <asp:Label ID="Label4" runat="server" Font-Names="MS Sans Serif" Font-Size="small"
                                            ForeColor="Red" Text="Errorlabel" Visible="false"></asp:Label>
                                        <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="tbrenewdate" Format="dd-MM-yyyy"
                                            runat="server" Enabled="True">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td class="style55">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblgendare" runat="server" Text="Gender" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        <asp:DropDownList ID="ddlgender" runat="server" Font-Names="MS Sans Serif" Font-Size="Small">
                                            <asp:ListItem Text="Both" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Female" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style55">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lbltotaltra" runat="server" Text="Total No.Of Travellers" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        <asp:TextBox ID="tbtotaltravel" runat="server" Font-Names="MS Sans Serif" Width="43px"
                                            MaxLength="3" Font-Size="Small" Height="15px"></asp:TextBox>
                                        <asp:Label ID="lblerrortravel" runat="server" Font-Names="MS Sans Serif" Font-Size="small"
                                            ForeColor="Red" Text="Errorlabel" Visible="false"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" TargetControlID="tbtotaltravel"
                                            FilterType="Numbers" runat="server">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td class="style55">
                                        <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*" Font-Size="Small"
                                            Font-Bold="false" Style="left: 188px; position: absolute;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblstudent" runat="server" Text="Student" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td class="style25">
                                        <asp:TextBox ID="tbstudent" runat="server" Font-Names="MS Sans Serif" Width="28px"
                                            MaxLength="3" Font-Size="Small" Height="15px"></asp:TextBox>
                                        <asp:Label ID="lblerrorstudent" runat="server" Font-Names="MS Sans Serif" Font-Size="small"
                                            ForeColor="Red" Text="Errorlabel" Visible="false"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredT" TargetControlID="tbstudent" FilterType="Numbers"
                                            runat="server">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*" Font-Size="Small"
                                            Font-Bold="false"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblstaff" runat="server" Text="Staff" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        <asp:TextBox ID="tbstaff" runat="server" Font-Names="MS Sans Serif" Width="28px"
                                            MaxLength="3" Font-Size="Small" Height="15px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" TargetControlID="tbstaff"
                                            FilterType="Numbers" runat="server">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Label ID="lblerrorstaff" runat="server" Font-Names="MS Sans Serif" Font-Size="small"
                                            ForeColor="Red" Text="Errorlabel" Visible="false"></asp:Label>
                                        <asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*" Font-Size="Small"
                                            Font-Bold="false"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblmanudate" runat="server" Text="Manufacture Date" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        <asp:TextBox ID="tbmanudate" runat="server" Font-Names="MS Sans Serif" Width="70px"
                                            Font-Size="Small" Height="15px"></asp:TextBox>
                                        <asp:Label ID="Label6" runat="server" Font-Names="MS Sans Serif" Font-Size="small"
                                            ForeColor="Red" Text="Errorlabel" Visible="false"></asp:Label>
                                        <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="tbmanudate" Format="dd-MM-yyyy"
                                            runat="server" Enabled="True">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td class="style55">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="purch" runat="server">
                        <ContentTemplate>
                            <table class="tabl" style="top: 231px; left: 643px; position: absolute; width: 316px;
                                border-color: Gray; border-width: thin; height: 124px;">
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lblpurfrom" runat="server" Text="Purchased From" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25">
                                        &nbsp;
                                        <asp:RadioButton ID="rbpruindu" Text="Individual" runat="server" GroupName="vehicleind"
                                            AutoPostBack="true" OnCheckedChanged="rbpruindu_CheckedChanged" Font-Names="MS Sans Serif"
                                            Font-Size="Small" />
                                        <asp:RadioButton ID="rbdealer" Text="Dealers" runat="server" AutoPostBack="true"
                                            OnCheckedChanged="rbdealer_CheckedChanged" Style="top: 26px; position: absolute;"
                                            GroupName="vehicleind" Font-Names="MS Sans Serif" Font-Size="Small" />
                                    </td>
                                    <td class="style55">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="lbldealerdetails" runat="server" Text="Dealer Details" Style="top: 65px;
                                            left: 30px; position: absolute;" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55" align="right">
                                        <asp:Button ID="btnadddealer" runat="server" Text="+" Style="display: none; top: 67px;
                                            position: absolute; left: 118px;" Font-Names="MS Sans Serif" Font-Size="Small"
                                            Height="21px" OnClick="btnadddealer_Click" />
                                    </td>
                                    <td class="style25" align="left">
                                        &nbsp;
                                        <asp:DropDownList ID="ddldealerdetails" runat="server" AutoPostBack="true" Font-Names="MS Sans Serif"
                                            Font-Size="Small" Height="20px" Width="127px" Style="margin-right: 25px; margin-left: -9px;
                                            top: 68px; position: absolute;" OnSelectedIndexChanged="ddldealerdetails_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblerredealer" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                                            Font-Size="3pt" Visible="false"></asp:Label>
                                    </td>
                                    <td class="style55" align="right">
                                        <asp:Button ID="btnremovedealer" runat="server" Text="-" Style="display: none; left: 281px;
                                            position: absolute; top: 67px;" OnClick="btnremovedealer_Click" Font-Names="MS Sans Serif"
                                            Font-Size="Small" Height="21px" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="Rtpupdated" runat="server">
                        <ContentTemplate>
                            <table class="tabl" style="top: 356px; left: 643px; position: absolute; width: 316px;
                                border-color: Gray; border-width: thin; height: 266px;">
                                <caption style="height: 20px; font-size: Small">
                                    RTO Office Address
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lbladdress1" runat="server" Text="Address1" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbaddress1" runat="server" Font-Names="MS Sans Serif" Width="141px"
                                                Font-Size="Small" Height="15px" OnTextChanged="tbaddress1_TextChanged"></asp:TextBox>
                                            <asp:Label ID="lbladdress1error" runat="server" Font-Names="MS Sans Serif" Font-Size="3pt"
                                                ForeColor="Red" Text="Errorlabel" Visible="false" Style="top: 53px; position: absolute;
                                                left: 129px;"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lbladdress2" runat="server" Text="Address2" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbaddress2" runat="server" OnTextChanged="tbaddress2_TextChanged"
                                                Font-Names="MS Sans Serif" Font-Size="Small" Height="15px" Width="141px"></asp:TextBox>
                                            <asp:Label ID="lbladdress2error" runat="server" Font-Names="MS Sans Serif" Font-Size="3pt"
                                                ForeColor="Red" Text="Errorlabel" Visible="false" Style="top: 93px; position: absolute;
                                                left: 128px;"></asp:Label>
                                        </td>
                                        <td class="style4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblcity" runat="server" Text="City" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbcityrto" runat="server" OnTextChanged="tbcityrto_TextChanged"
                                                Font-Names="MS Sans Serif" Font-Size="Small" Height="15px" Width="120px"></asp:TextBox>
                                            <asp:Label ID="lblcityrto" runat="server" Font-Names="MS Sans Serif" Font-Size="3pt"
                                                ForeColor="Red" Text="Errorlabel" Visible="false" Style="top: 130px; left: 127px;
                                                position: absolute;"></asp:Label>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="only characters allowed"
                                                ControlToValidate="tbcityrto" SetFocusOnError="True" Font-Size="1pt" Display="Dynamic"
                                                EnableClientScript="True" Style="top: 128px; position: absolute; left: 136px;"
                                                ForeColor="Red" Font-Names="MS Sans Serif" ValidationExpression="^[a-zA-Z]*$"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblstate" runat="server" Text="State" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55" align="right">
                                            <asp:Button ID="statertoadd" runat="server" Text="+" Style="display: none; top: 145px;
                                                position: absolute; left: 95px;" Font-Names="MS Sans Serif" Font-Size="Small"
                                                Height="21px" OnClick="statertoadd_Click" />
                                        </td>
                                        <td class="style25" align="left">
                                            &nbsp;
                                            <asp:DropDownList ID="ddlstaterto" runat="server" AutoPostBack="true" Font-Names="MS Sans Serif"
                                                Font-Size="Small" Height="20px" Width="127px" Style="left: 152px; position: absolute;
                                                margin-right: 25px; margin-left: -24px;" OnSelectedIndexChanged="ddlstaterto_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblerrorstate" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                                                Font-Size="3pt" Visible="false" Style="top: 164px; position: absolute; left: 130px;"></asp:Label>
                                        </td>
                                        <td class="style55" align="right">
                                            <asp:Button ID="statertoremove" runat="server" Text="-" Style="display: none; left: 256px;
                                                position: absolute; top: 145px;" OnClick="statertoremove_Click" Font-Names="MS Sans Serif"
                                                Font-Size="Small" Height="21px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblrtopin" runat="server" Text="Pin Code" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbpincoderto" runat="server" OnTextChanged="tbpincoderto_TextChanged"
                                                Font-Names="MS Sans Serif" Font-Size="Small" Height="15px" Width="46px" MaxLength="6"></asp:TextBox>
                                            <asp:Label ID="lblpincoderto" runat="server" Text="Enter Valid Pincode" ForeColor="Red"
                                                Font-Names="MS Sans Serif" Font-Size="Small" Visible="False"></asp:Label>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" TargetControlID="tbpincoderto"
                                                FilterType="Numbers" runat="server" Enabled="True">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td class="style4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblrtocontact" runat="server" Text="Contact Person" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbrtocontact" runat="server" MaxLength="13" Font-Names="MS Sans Serif"
                                                Width="89px" Font-Size="Small" Height="15px"></asp:TextBox>
                                            <asp:Label ID="lblrtocontacterror" runat="server" Font-Names="MS Sans Serif" Font-Size="3pt"
                                                ForeColor="Red" Text="Errorlabel" Visible="false" Style="top: 238px; position: absolute;
                                                left: 128px;"></asp:Label>
                                        </td>
                                        <td class="style4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style5" align="right">
                                            <asp:Label ID="lblcontactnumber" runat="server" Text="Contact Numbers" Font-Names="MS Sans Serif"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                        <td class="style55">
                                        </td>
                                        <td class="style25">
                                            <asp:TextBox ID="tbcontactnumber" runat="server" Font-Names="MS Sans Serif" Width="89px"
                                                MaxLength="10" Font-Size="Small" Height="15px"></asp:TextBox>
                                            <asp:Label ID="lblcontactnumbererror" runat="server" Text="Enter Valid Mobile No"
                                                Font-Names="MS Sans Serif" Font-Size="3pt" ForeColor="Red" Visible="False"></asp:Label>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="tbcontactnumber"
                                                FilterType="Numbers" Enabled="True" />
                                        </td>
                                        <td class="style4">
                                        </td>
                                    </tr>
                                </caption>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="updatedadd3" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="Paneladd" runat="server" Visible="False" Style="width: 200px; height: 100px;
                                top: 290px; left: 185px; position: absolute;" BorderStyle="Solid" BorderWidth="1px"
                                BackColor="#CCCCCC" Font-Names="MS Sans Serif" Font-Size="Small">
                                <center>
                                    <caption runat="server" id="newcaption" style="height: 10px; top: 10px; font-variant: Small-caps">
                                    </caption>
                                    <br />
                                    <asp:TextBox ID="tbaddnew" Width="146px" Height="14px" runat="server"></asp:TextBox>
                                    <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" TargetControlID="tbaddnew" InvalidChars="1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,^,&,*,(,),<,>,?,|,_,,/,\,:,;,',[,],{,},`,~"
         runat="server" FilterMode="InvalidChars">
        </asp:FilteredTextBoxExtender>--%>
                                    <br />
                                    <asp:Button ID="addnew" Width="50px" runat="server" Text="Add" OnClick="addnew_Click"
                                        Font-Names="MS Sans Serif" Font-Size="Small" Height="25px" />
                                    &nbsp;
                                    <asp:Button ID="exitnew" Width="50px" runat="server" Text="Exit" OnClick="exitnew_Click"
                                        Font-Names="MS Sans Serif" Font-Size="Small" Height="25px" />
                                </center>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </asp:AccordionPane>
            <asp:AccordionPane ID="AccordionPane3" runat="server">
                <Header>
                    Vehicle Permit Details</Header>
                <Content>
                    <asp:UpdatePanel ID="updatepermit" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="Panel6" runat="server" Style="left: -147px; border-color: Gray; border-style: solid;
                                width: 978px; height: 187px; margin-bottom: 0px; margin-right: 212px; margin-left: -6px;
                                margin-top: 50px;">
                                <asp:Button ID="btnremovepermit" runat="server" OnClick="addremovePermit" Text="Remove"
                                    Style="top: 264px; left: 851px; position: absolute;" />
                                <table class="tabl" style="top: 267px; left: 212px; position: absolute; width: 561px;
                                    border-color: Gray; border-width: thin; height: 148px;">
                                    <asp:Button ID="btnaddpermit" runat="server" OnClick="addrowPermit" Text="Add" Style="top: 264px;
                                        left: 808px; position: absolute;" />
                                    <caption style="height: 20px; font-size: Small">
                                        <b>Vehicle Permit Details</b>
                                        <tr>
                                            <td>
                                                <center>
                                                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                        OnButtonCommand="FpSpread1_ButtonCommand" OnPreRender="FpSpread1prerender" OnCellClick="FpSpread1_click"
                                                        BorderWidth="1px" Height="108" Width="598">
                                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                            ButtonShadowColor="ControlDark">
                                                        </CommandBar>
                                                        <Sheets>
                                                            <FarPoint:SheetView SheetName="Sheet1">
                                                            </FarPoint:SheetView>
                                                        </Sheets>
                                                        <%-- <ClientEvents --%>
                                                    </FarPoint:FpSpread>
                                                </center>
                                            </td>
                                        </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </asp:AccordionPane>
            
      
            <asp:AccordionPane ID="AccordionPane4" runat="server">
                <Header>
                    InsuranceDetails</Header>
                <Content>
                    <asp:Button ID="btnremoverowfee" runat="server" OnClick="addremoveinsurance" Text="Remove"
                        Style="top: 296px; left: 902px; position: absolute;" />
                    <asp:Panel ID="Panel3" runat="server" Style="left: -147px; border-color: Gray; border-style: solid;
                        width: 978px; height: 244px; margin-bottom: 0px; margin-right: 212px; margin-left: -6px;
                        margin-top: 50px;">
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                            Font-Size="3pt" Visible="false"></asp:Label>
                        <asp:Button ID="btnaddrowfee" runat="server" OnClick="addrowfee" Text="Add" Style="top: 296px;
                            left: 859px; position: absolute;" />
                        <table class="tabl" style="top: 331px; left: 54px; position: absolute; width: 930px;
                            border-color: Gray; border-width: thin; height: 174px;">
                            <caption style="height: 20px; font-size: Small">
                                <b>Insurance Details</b>
                                <tr>
                                    <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                        <FarPoint:FpSpread ID="sprdMaininsurance" runat="server" BorderColor="Black" OnButtonCommand="sprdMaininsurance_ButtonCommand"
                                            BorderStyle="Solid" OnCellClick="sprdMaininsurance_CellClick" OnPreRender="sprdMaininsuranceprerender"
                                            BorderWidth="1px" Width="900" Height="118" Style="margin-top: 18px; margin-left: 11PX;"
                                            ButtonType="PushButton" ShowPDFButton="True">
                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                ButtonShadowColor="ControlDark">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                        </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="sprdMaininsurance" />
                                </Triggers>
                             </asp:UpdatePanel>
                                    </td>
                                </tr>
                        </table>
                        <asp:Panel ID="pnldirectInsurance" runat="server" Width="350px" Height="180px" Font-Names="MS Sans Serif"
                            Font-Size="Small" AutoPostBack="true" ScrollBars="Auto" BackColor="White" BorderColor="Black"
                            BorderStyle="Double" Style="display: none; height: 400; width: 700; font-weight: bold;">
                            <center>
                                <caption id="Caption5" runat="server" style="height: 10px; top: 10px; font-family: MS Sans Serif;
                                    font-size: large; font-weight: bold;">
                                    Insurance Certificate</caption>
                            </center>
                            <div class="PopupHeaderrstud" id="Div10" style="text-align: center; font-family: MS Sans Serif;
                                font-size: Small; font-weight: bold">
                                <table class="tabl" style="top: 50px; left: 30px; position: absolute; width: 290px;
                                    height: 29px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label133" runat="server" Text="Add Certificate Copy"></asp:Label>
                                        </td>
                                        <td>
                                        
                                            <asp:FileUpload ID="FileUpload3" runat="server" />
                                            
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                            <asp:Button ID="btnsave" runat="server" Text="Save" Style="top: 120px; left: 100px;
                                position: absolute; width: 48px;" OnClick="btncertifiInsurance1_Click" />
                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnsave" />
                                </Triggers>
                             </asp:UpdatePanel>
                            <asp:Button ID="Buttonclosedirect" runat="server" Text="Close" Style="top: 120px;
                                left: 150px; position: absolute;" />
                        </asp:Panel>
                        <asp:ModalPopupExtender ID="mpedirect" Drag="True" CancelControlID="Buttonclosedirect"
                            TargetControlID="hfdirect" PopupControlID="pnldirectInsurance" runat="server"
                            BackgroundCssClass="ModalPopupBG" DynamicServicePath="" Enabled="True">
                        </asp:ModalPopupExtender>
                        <asp:HiddenField ID="hfdirect" runat="server" />
                    </asp:Panel>
                </Content>
            </asp:AccordionPane>
            <asp:AccordionPane ID="AccordionPane5" runat="server">
                <Header>
                    FCDetails</Header>
                <Content>
                    <asp:Button ID="btnremoveFC" runat="server" OnClick="addremoveFC" Text="Remove" Style="top: 329px;
                        left: 913px; position: absolute;" />
                    <asp:Panel ID="Panel4" runat="server" Style="left: -147px; border-color: Gray; border-style: solid;
                        width: 978px; height: 244px; margin-bottom: 0px; margin-right: 212px; margin-left: -6px;
                        margin-top: 50px;">
                        <asp:Label ID="lblinserrorcer" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                            Font-Size="3pt" Visible="false"></asp:Label>
                        <asp:Button ID="btnaddrowFC" runat="server" OnClick="addrowFC" Text="Add" Style="top: 329px;
                            left: 870px; position: absolute;" />
                        <table class="tabl" style="top: 374px; left: 125px; position: absolute; width: 773px;
                            margin-top: 192; border-color: Gray; border-width: thin; height: 163px;">
                            <caption style="height: 20px; font-size: Small">
                                <b>FC Details</b>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                <FarPoint:FpSpread ID="sprdmainFC" runat="server" BorderColor="Black" OnButtonCommand="sprdmainFC_ButtonCommand"
                                    BorderStyle="Solid" OnCellClick="sprdmainFC_CellClick" OnPreRender="sprdmainFCprerender"
                                    BorderWidth="1px" Width="739" Height="118" Style="margin-top: 31px; margin-left: 16px;"
                                    ButtonType="PushButton" ShowPDFButton="True">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="sprdmainFC" />
                                </Triggers>
                             </asp:UpdatePanel>
                            </caption>
                        </table>
                        <asp:Panel ID="pnldirectFC" runat="server" Width="350px" Height="180px" Font-Names="MS Sans Serif"
                            Font-Size="Small" AutoPostBack="true" ScrollBars="Auto" BackColor="White" BorderColor="Black"
                            BorderStyle="Double" Style="display: none; height: 400; width: 700; font-weight: bold;">
                            <center>
                                <caption id="Caption1" runat="server" style="height: 10px; top: 10px; font-family: MS Sans Serif;
                                    font-size: large; font-weight: bold;">
                                    FC Certificate</caption>
                            </center>
                            <div class="PopupHeaderrstud" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                                font-size: Small; font-weight: bold">
                                <table class="tabl" style="top: 50px; left: 30px; position: absolute; width: 290px;
                                    height: 29px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="Add Certificate Copy"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="FileUpload4" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                            <asp:Button ID="btnsave1" runat="server" Text="Save" Style="top: 120px; left: 100px;
                                position: absolute;" OnClick="btncertifiFC1_Click" />
                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnsave1" />
                                </Triggers>
                             </asp:UpdatePanel>
                            <asp:Button ID="Buttonclosedirect1" runat="server" Text="Close" Style="top: 120px;
                                left: 150px; position: absolute;" />
                        </asp:Panel>
                        <asp:ModalPopupExtender ID="mpedirect1" Drag="True" CancelControlID="Buttonclosedirect1"
                            TargetControlID="hfdirect1" PopupControlID="pnldirectFC" runat="server" BackgroundCssClass="ModalPopupBG"
                            DynamicServicePath="" Enabled="True">
                        </asp:ModalPopupExtender>
                        <asp:HiddenField ID="hfdirect1" runat="server" />
                    </asp:Panel>
                </Content>
            </asp:AccordionPane>
            <asp:AccordionPane ID="AccordionPane6" runat="server">
                <Header>
                    Photo Copy</Header>
                <Content>
                    <asp:Panel ID="Panel5" runat="server" Style="left: -147px; border-color: Gray; border-style: solid;
                        width: 978px; height: 414px; margin-bottom: 0px; margin-right: 212px; margin-left: -6px;
                        margin-top: 50px;">
                        <table class="tabl" style="top: 357px; left: 27px; position: absolute; width: 192px;
                            border-color: Gray; border-width: thin; height: 183px;">
                            <caption style="height: 20px; font-size: Small">
                                <b>Reg.Certificate</b>
                                <tr style="height: 25px">
                                    <td>
                                        <%--    <asp:FileUpload id="imgUpload"  runat="server"   />--%>
                                        <asp:FileUpload ID="FileUploadnew" runat="server" />
                                        <center>
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                            <asp:Button ID="btncertifiphoto" runat="server" Text="Attach" Height="21px" OnClick="UploadCertificate_Click"
                                                CssClass="font" />
                                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btncertifiphoto" />
                                </Triggers>
                             </asp:UpdatePanel>
                                        </center>
                                    </td>
                                </tr>
                                <tr style="height: 100px">
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="ImageRegCer" runat="server" Height="99px" Width="115px" />
                                    </td>
                                </tr>
                        </table>
                        <table class="tabl" style="top: 357px; left: 222px; position: absolute; width: 193px;
                            border-color: Gray; border-width: thin; height: 183px;">
                            <caption style="height: 20px; font-size: Small">
                                <b>Vehicle Back Photo</b>
                                <tr style="height: 25px">
                                    <td>
                                        <asp:FileUpload ID="imgUpload2" runat="server" />
                                        <center>
                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                            <ContentTemplate>
                                            <asp:Button ID="btnbackphoto" runat="server" Text="Attach" Height="21px" OnClick="UploadBackPhoto_Click"
                                                CssClass="font" />
                                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnbackphoto" />
                                </Triggers>
                             </asp:UpdatePanel>
                                        </center>
                                    </td>
                                </tr>
                                <tr style="height: 100px">
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="ImgBackPhoto" runat="server" Height="99px" Width="115px" />
                                    </td>
                                </tr>
                        </table>
                        <table class="tabl" style="top: 357px; left: 417px; position: absolute; width: 194px;
                            border-color: Gray; border-width: thin; height: 183px;">
                            <caption style="height: 20px; font-size: Small">
                                <b>Vehicle Front Photo</b>
                                <tr style="height: 25px">
                                    <td>
                                        <asp:FileUpload ID="imgUpload3" runat="server" />
                                        <center>
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <ContentTemplate>
                                            <asp:Button ID="btnFrontPhoto" runat="server" Text="Attach" Height="21px" OnClick="UploadFrontPhoto_Click"
                                                CssClass="font" />
                                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnFrontPhoto" />
                                </Triggers>
                             </asp:UpdatePanel>
                                        </center>
                                    </td>
                                </tr>
                                <tr style="height: 100px">
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="imgFrontPhoto" runat="server" Height="99px" Width="115px" />
                                    </td>
                                </tr>
                        </table>
                        </table>
                        <table class="tabl" style="top: 357px; left: 612px; position: absolute; width: 191px;
                            border-color: Gray; border-width: thin; height: 183px;">
                            <caption style="height: 20px; font-size: Small">
                                <b>Vehicle left Side Photo</b>
                                <tr style="height: 25px">
                                    <td>
                                        <asp:FileUpload ID="imgUpload4" runat="server" />
                                        <center>
                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                            <ContentTemplate>
                                            <asp:Button ID="btnleftphoto" runat="server" Text="Attach" Height="21px" OnClick="Uploadleftphoto_Click"
                                                CssClass="font" />
                                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnleftphoto" />
                                </Triggers>
                             </asp:UpdatePanel>
                                        </center>
                                    </td>
                                </tr>
                                <tr style="height: 100px">
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="imgleftphoto" runat="server" Height="99px" Width="115px" />
                                    </td>
                                </tr>
                        </table>
                        </table>
                        <table class="tabl" style="top: 357px; left: 807px; position: absolute; width: 187px;
                            border-color: Gray; border-width: thin; height: 183px;">
                            <caption style="height: 20px; font-size: Small">
                                <b>Vehicle Right Side Photo</b>
                                <tr style="height: 25px">
                                    <td>
                                        <asp:FileUpload ID="imgUpload5" runat="server" />
                                        <center>
                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                            <ContentTemplate>
                                            <asp:Button ID="btnrightphoto" runat="server" Text="Attach" Height="21px" OnClick="UploadrightPhoto_Click"
                                                CssClass="font" />
                                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnrightphoto" />
                                </Triggers>
                             </asp:UpdatePanel>
                                        </center>
                                    </td>
                                </tr>
                                <tr style="height: 100px">
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="imgrightphoto" runat="server" Height="99px" Width="115px" />
                                    </td>
                                </tr>
                        </table>
                        <table class="tabl" style="top: 562px; left: 25px; position: absolute; width: 192px;
                            border-color: Gray; border-width: thin; height: 183px;">
                            <caption style="height: 20px; font-size: Small">
                                <b>Vehicle Other Photo1</b>
                                <tr style="height: 25px">
                                    <td>
                                        <asp:FileUpload ID="imgUpload6" runat="server" />
                                        <center>
                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                            <ContentTemplate>
                                            <asp:Button ID="btnother1" runat="server" Text="Attach" Height="21px" OnClick="Uploadother1Photo_Click"
                                                CssClass="font" />
                                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnother1" />
                                </Triggers>
                             </asp:UpdatePanel>
                                        </center>
                                    </td>
                                </tr>
                                <tr style="height: 100px">
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="imgother1photo" runat="server" Height="99px" Width="115px" />
                                    </td>
                                </tr>
                        </table>
                        <table class="tabl" style="top: 562px; left: 220px; position: absolute; width: 193px;
                            border-color: Gray; border-width: thin; height: 183px;">
                            <caption style="height: 20px; font-size: Small">
                                <b>Vehicle Other Photo2</b>
                                <tr style="height: 25px">
                                    <td>
                                        <asp:FileUpload ID="imgUpload7" runat="server" />
                                        <center>
                                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                            <ContentTemplate>
                                            <asp:Button ID="btnother2" runat="server" Text="Attach" Height="21px" OnClick="Uploadother2Photo_Click"
                                                CssClass="font" />
                                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnother2" />
                                </Triggers>
                             </asp:UpdatePanel>
                                        </center>
                                    </td>
                                </tr>
                                <tr style="height: 100px">
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="imgother2photo" runat="server" Height="99px" Width="115px" />
                                    </td>
                                </tr>
                        </table>
                        <table class="tabl" style="top: 562px; left: 415px; position: absolute; width: 189px;
                            border-color: Gray; border-width: thin; height: 183px;">
                            <caption style="height: 20px; font-size: Small">
                                <b>Vehicle Other Photo3</b>
                                <tr style="height: 25px">
                                    <td>
                                        <asp:FileUpload ID="imgUpload8" runat="server" />
                                        <center>
                                        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                            <ContentTemplate>
                                            <asp:Button ID="btnother3" runat="server" Text="Attach" Height="21px" OnClick="Uploadother3Photo_Click"
                                                CssClass="font" />
                                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnother3" />
                                </Triggers>
                             </asp:UpdatePanel>
                                        </center>
                                    </td>
                                </tr>
                                <tr style="height: 100px">
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="imgother3photo" runat="server" Height="99px" Width="115px" />
                                    </td>
                                </tr>
                        </table>
                        <table class="tabl" style="top: 562px; left: 610px; position: absolute; width: 177px;
                            border-color: Gray; border-width: thin; height: 183px;">
                            <caption style="height: 20px; font-size: Small">
                                <b>Vehicle Other Photo4</b>
                                <tr style="height: 25px">
                                    <td>
                                        <asp:FileUpload ID="imgUpload9" runat="server" />
                                        <center>
                                        <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                            <ContentTemplate>
                                            <asp:Button ID="btnother4" runat="server" Text="Attach" Height="21px" OnClick="Uploadother4Photo_Click"
                                                CssClass="font" />
                                                </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btnother4" />
                                </Triggers>
                             </asp:UpdatePanel>
                                        </center>
                                    </td>
                                </tr>
                                <tr style="height: 100px">
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="imgother4photo" runat="server" Height="99px" Width="115px" />
                                    </td>
                                </tr>
                        </table>
                    </asp:Panel>
                </Content>
            </asp:AccordionPane>

            
        </Panes>
    </asp:Accordion>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
    <asp:Panel ID="pnlmsgboxdelete" runat="server" CssClass="modalPopup" Style="display: none;
        height: 100; width: 300;" DefaultButton="btnOk">
        <table width="100%">
            <tr class="topHandle">
                <td colspan="2" align="left" runat="server" id="tdCaption">
                    <asp:Label ID="lblCaption" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                        Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 60px" valign="middle" align="center">
                    <asp:Image ID="imgInfo" runat="server" ImageUrl="~/Info-48x48.png" />
                </td>
                <td valign="middle" align="left">
                    <asp:Label ID="lblMessage" Text="Do You want to Delete the Record?" runat="server"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnOk" runat="server" Text="Yes" OnClick="btnOk_Click" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" />
                    <asp:Button ID="btnCancel" runat="server" Text="No" OnClick="btnCancel_Click" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:HiddenField runat="server" ID="hfdelete" />
    <asp:ModalPopupExtender ID="mpemsgboxdelete" runat="server" TargetControlID="hfdelete"
        PopupControlID="pnlmsgboxdelete">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel7" runat="server" Style="left: -147px; border-color: Gray; border-style: solid;
        width: 978px; height: 28px; margin-bottom: 0px; margin-right: 212px; margin-left: 0px;
        margin-top: 2px;">
        <asp:Label ID="lblerrordisplay" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
            Font-Size="8pt" Font-Bold="true" Visible="false" Style="position: absolute;"></asp:Label>
        <%--      <table class="tablfont"
            style="top:1188px; left:785px; position: absolute; width:158px; height:auto; border-color:Gray;">--%>
        <tr>
            <asp:Button ID="btnnew" runat="server" Text="New" OnClick="btnNew_Click" Font-Bold="true"
                Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False" ForeColor="Black"
                Width="60px" Height="25px" Style="color: Black; font-family: MS Sans Serif; font-size: small;
                font-weight: bold; text-decoration: none; height: 25px; width: 60px; margin-left: 740px;" />
            <asp:Button ID="Buttonsave" runat="server" Text="Save" OnClick="Buttonsave_Click"
                Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False"
                ForeColor="Black" Width="60px" Height="25px" />
            <asp:Button ID="Buttondelete" runat="server" Text="Delete" OnClick="Buttondelete_Click"
                Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False"
                ForeColor="Black" Width="70px" Height="25px" Enabled="False" />
    </asp:Panel>

        
        </ContentTemplate>
    </asp:UpdatePanel>
     
        </ContentTemplate>
    </asp:UpdatePanel>
     
    <style type="text/css">
        ody, input
        {
            font-family: Tahoma;
            font-size: 10px;
        }
        
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
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
        
        .topHandle
        {
            background-color: #97bae6;
        }
        .floatr
        {
            border-style: solid;
            border-color: Black;
            border-width: thin;
            float: right;
        }
        
        
        .tabl
        {
            empty-cells: show;
            border-style: solid;
            border-color: Gray;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: normal;
        }
        .tablfont
        {
            empty-cells: show;
            border-style: solid;
            border-color: Gray;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
        }
        .tabl3
        {
            empty-cells: hide;
            border-style: solid;
            border-color: Black;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
            text-align: left;
        }
        .tabl5
        {
            border-style: solid;
            border-color: Black;
            border-width: thin;
            text-align: left;
        }
        .tabl1
        {
            empty-cells: show;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
        }
        .linkbtn
        {
            border-color: White;
            border-style: none;
            background-color: transparent;
            cursor: pointer;
        }
        .HeaderSelectedCSS
        {
            color: white;
            background-color: #719DDB;
            font-weight: bold;
            font-size: medium; /* font-style:italic;  */
        }
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .style2
        {
            width: 118px;
        }
        .style4
        {
            width: 43px;
        }
        .stylefp
        {
            cursor: pointer;
        }
        .style5
        {
            width: 185px;
        }
        .style55
        {
            width: 25px;
        }
        .style27
        {
            width: 25px;
        }
        .style25
        {
            width: 210px;
        }
        .style251
        {
            width: 125px;
        }
        .style6
        {
            width: 528px;
        }
        .style12
        {
            width: 200px;
        }
        .style22
        {
            width: 122px;
        }
        .style24
        {
            width: 30px;
        }
        
        .font
        {
            font-size: Small;
            font-family: MS Sans Serif;
        }
        .HeaderCSS
        {
            color: white;
            background-color: #719DDB;
            font-size: small; /* border:solid 1px salmon; */
            font-weight: bold;
            height: 10px;
        }
        .cpBody
        {
            background-color: #DCE4F9; /*font: normal 11px auto Verdana, Arial;
            border: 1px gray;               
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
            width:720;*/
        }
        .accordion
        {
            width: 300px;
        }
        
        .accordionHeader
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #2E4d7B;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionHeaderSelected
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #5078B3;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionContent
        {
            background-color: White;
            border: 1px dashed #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 10px;
        }
    </style>
</asp:Content>
