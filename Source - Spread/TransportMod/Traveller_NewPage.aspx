<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Traveller_NewPage.aspx.cs" Inherits="Default6" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <link href="~/Styles/css/Style.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function changerollno() {
            var tbenqno; 
            tbenqno = document.getElementById('<%=tbenqno.ClientID%>');
            if (tbenqno.value == "") {
                tbenqno.style.backgroundColor = "LightYellow";
            }
            else {
                tbenqno.style.backgroundColor = "LightYellow";
            }

        }
        function changeseatno() {
            var tbseatno;
            tbseatno = document.getElementById('<%=tbseatno.ClientID%>');
            if (tbseatno.value == "") {
                tbseatno.style.backgroundColor = "LightYellow";
            }
            else {
                tbseatno.style.backgroundColor = "LightYellow";
            }

        }
        function keyvalue() {
            var txt = document.getElementById('<%=lbprint.ClientID %>');
            txt.innerHTML = "";
            txt.style.display = "none";
        }

    </script>
    <script type = "text/javascript">
        window.onload = function () {
            var scrollY = parseInt('<%=Request.Form["scrollY"] %>');
            if (!isNaN(scrollY)) {
                window.scrollTo(0, scrollY);
            }
        };
        window.onscroll = function () {
            var scrollY = document.body.scrollTop;
            if (scrollY == 0) {
                if (window.pageYOffset) {
                    scrollY = window.pageYOffset;
                }
                else {
                    scrollY = (document.body.parentElement) ? document.body.parentElement.scrollTop : 0;
                }
            }
            if (scrollY > 0) {
                var input = document.getElementById("scrollY");
                if (input == null) {
                    input = document.createElement("input");
                    input.setAttribute("type", "hidden");
                    input.setAttribute("id", "scrollY");
                    input.setAttribute("name", "scrollY");
                    document.forms[0].appendChild(input);
                }
                input.value = scrollY;
            }
        };
    </script>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
   



    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
    <br />
    




     
    
 <Content>
                    <asp:Label ID="lblMainError" runat="server" CssClass="font" ForeColor="Red"></asp:Label>
                    <%--        <asp:UpdatePanel ID="updateview" runat="server">
         <ContentTemplate>--%>
                    <asp:Label ID="lblerrordate" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                        Font-Size="5pt" Visible="false"></asp:Label>
                    <asp:Panel ID="Panel2" runat="server" Style="border-style: solid; border-width: thin;
                        border-color: Black; background: White;">
                        <br />
                       
                         


         <center>
        <span class="fontstyleheader" style="color: Green; font-weight: bold; margin: 0px;
            margin-bottom: 15px; margin-top: 10px;">Traveller Allotment </span>
        <table class="maintablestyle" style="width: 980px; height: auto; background-color: #0CA6CA;
            padding: 8px; margin: 0px; margin-bottom: 15px; margin-top: 10px;">
            <tr>
                 <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                    background-color: lightblue ; border-width: 1px;">
                                    <asp:Label ID="Label16" runat="server" Font-Bold="true" CssClass="font" Text="College"></asp:Label>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                    border-width: 1px; border-right-style: solid;">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtclg" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="160px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="Panel4" runat="server" Width="280px" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="cbclg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="cbclg_CheckedChanged" Text="Select All"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="cblclg" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="cblclg_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtclg"
                                                PopupControlID="Panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                    background-color: lightblue; border-width: 1px;">
                                    <asp:Label ID="lblroute" runat="server" Font-Bold="true" CssClass="font" Text="Route ID"
                                        Width="90px"></asp:Label>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                    border-width: 1px; border-right-style: solid;">
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_routeview"  runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                            ReadOnly="true" Style="height: 20px; width: 100px; font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel" Style="width: 123px;"
                                            Height="250px" >
                                                <asp:CheckBox ID="cb_routeview" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_routeview_checkedchanged" />
                                                <asp:CheckBoxList ID="cbl_routeview" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_routeview_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_routeview"
                                                PopupControlID="Panel6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%-- <asp:DropDownList ID="ddlrouteview" runat="server" Font-Bold="true" CssClass="font"
                                        Width="122px" AutoPostBack="True" OnSelectedIndexChanged="ddlrouteview_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                </td>



                                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                    background-color: lightblue; border-width: 1px;">
                                    <asp:Label ID="lbltypeview" runat="server" Font-Bold="true" CssClass="font" Text="Vehicle ID"
                                        Width="100px"></asp:Label>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                    border-width: 1px; border-right-style: solid;">
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_vehicletype" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                            ReadOnly="true" Style="height: 20px; width: 100px; font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 123px;"
                                            Height="250px">
                                                <asp:CheckBox ID="cb_vehicletype" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_vehicletype_checkedchanged" />
                                                <asp:CheckBoxList ID="cbl_vehicletype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_vehicletype_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_vehicletype"
                                                PopupControlID="Panel5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%--<asp:DropDownList ID="ddlvehicletype" runat="server" Font-Bold="true" CssClass="font"
                                        Width="122px" AutoPostBack="True" OnSelectedIndexChanged="ddlvehicletype_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                    background-color: lightblue; border-width: 1px;">
                                    <asp:Label ID="lblstage" runat="server" Font-Bold="true" CssClass="font" Text="Stage"></asp:Label>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                    border-width: 1px; border-right-style: solid;">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_stage" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                            ReadOnly="true" Style="height: 20px; width: 100px; font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel7" runat="server" CssClass="multxtpanel" Style="width: 123px;"
                                            Height="250px">
                                                <asp:CheckBox ID="cb_stage" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_stage_checkedchanged" />
                                                <asp:CheckBoxList ID="cbl_stage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stage_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_stage"
                                                PopupControlID="Panel7" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%-- <asp:DropDownList ID="ddlstage" runat="server" Font-Bold="true" CssClass="font" Width="122px"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlstage_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                </td>
                                <%-- <table class="tabl" style="top: 226px; left: 666px; position: absolute; width: 184px;
                                    border-color: Gray; border-width: thin; height: 0px;">
                                    <tr>
                                        <td style="border-bottom-style: solid; border-top-style: solid; border-right-style: solid;
                                            border-width: 1px; background-color: #E6E6FA;">
                                            <asp:RadioButton ID="rbregular" Text="Student" runat="server" AutoPostBack="True"
                                                OnCheckedChanged="rbregular_CheckedChanged" GroupName="StudentType" Font-Names="MS Sans Serif"
                                                Font-Size="Small" />
                                            <asp:RadioButton ID="rblateral" Text="Staff" runat="server" AutoPostBack="True" OnCheckedChanged="rblateral_CheckedChanged"
                                                GroupName="StudentType" Font-Names="MS Sans Serif" Font-Size="Small" />
                                            <asp:RadioButton ID="rbtransfer" Text="Both" runat="server" AutoPostBack="True" OnCheckedChanged="rbtransfer_CheckedChanged"
                                                GroupName="StudentType" Font-Names="MS Sans Serif" Font-Size="Small" />
                                        </td>
                                    </tr>
                                    </caption>
                                </table>--%>
                                
                            </tr>
                             <td colspan="10">

                            <table class="tabl" style="text-align: center; top: 30px; font-family: MS Sans Serif;
                            font-size: Small; font-weight: bold">
                            <tr>
                             <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                    background-color: lightblue; border-width: 1px;">
                                    <asp:Label ID="Label7" runat="server" Text="Batch" Width="100px" Font-Bold="True"
                                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                    border-width: 1px; border-right-style: solid;">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="100px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pbatch" runat="server" Width="110px" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="chkbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="chkbatch_CheckedChanged" Text="Select All"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklstbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbatch"
                                                PopupControlID="pbatch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>


                                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                    background-color: lightblue; border-width: 1px;">
                                    <asp:Label ID="Label8" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                    border-width: 1px; border-right-style: solid;">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtdegree" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="100px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pdegree1" runat="server" Width="300px" Style="text-align: left;" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="chkdegree_CheckedChanged" Text="Select All"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklstdegree" runat="server" Style="font-family: Book Antiqua;
                                                    font-size: medium; font-weight: bold; text-align: left;" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged" Height="58px" Font-Bold="True"
                                                    Font-Names="Book Antiqua">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                                PopupControlID="pdegree1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                    background-color: lightblue; border-width: 1px;">
                                    <asp:Label ID="Label9" runat="server" Text="Branch" Width="90px" Font-Bold="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                    border-width: 1px; border-right-style: solid;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="110px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pbranch" runat="server" Width="350px" Style="text-align: left;" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="chkbranch_CheckedChanged" Text="Select All"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged" Style="font-family: 'Book Antiqua';
                                                    text-align: left;" Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                                PopupControlID="pbranch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>


                                </table>

                                <table class="tabl" style="top: 210px;margin-left: 652px; position: absolute; width: 184px;
                                    border-color: Gray; border-width: thin; height: 0px;">
                                <tr>
                              <td style="border-bottom-style: solid; border-top-style: solid; border-right-style: solid;
                                            border-width: 1px; background-color: #E6E6FA;">
                                            <asp:RadioButton ID="rbregular" Text="Student" runat="server" AutoPostBack="True"
                                                OnCheckedChanged="rbregular_CheckedChanged" GroupName="StudentType" Font-Names="MS Sans Serif"
                                                Font-Size="Small" />
                                            <asp:RadioButton ID="rblateral" Text="Staff" runat="server" AutoPostBack="True" OnCheckedChanged="rblateral_CheckedChanged"
                                                GroupName="StudentType" Font-Names="MS Sans Serif" Font-Size="Small" />
                                            <asp:RadioButton ID="rbtransfer" Text="Both" runat="server" AutoPostBack="True" OnCheckedChanged="rbtransfer_CheckedChanged"
                                                GroupName="StudentType" Font-Names="MS Sans Serif" Font-Size="Small" />
                                        </td>
                                        
                                </tr>
                                
                                </table>
                                 <table style="top: 240px;margin-left: 652px; position: absolute; ">
                                <tr>
                                <td>
                             <asp:LinkButton ID="lb_header" runat="server" Font-Size="Large" Text="Header Settings"
                                            OnClick="lb_header_Click" Visible="false"></asp:LinkButton>
                                        &nbsp; &nbsp;

                                <asp:LinkButton ID="lb_footer" runat="server" Font-Size="Large" Text="Footer Settings"
                                            OnClick="lb_footer_Click" Visible="false"></asp:LinkButton>
                                            
                                 </td>
                                 </tr>
                                 
                                </table>
                                 

                               


                                </tr>
                                <td colspan="10">
                               
                                <table class="tabl" style="text-align: center; top: 30px; font-family: MS Sans Serif;
                            font-size: Small; font-weight: bold">
                            <tr>
                                <%-- <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                    background-color: lightblue; border-width: 1px;">
                                    <asp:Label ID="Label13" runat="server" Text="College" Font-Names="MS Sans Serif"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                    border-width: 1px; border-right-style: solid;">
                                    <asp:DropDownList ID="ddlcollegestaff" runat="server" OnSelectedIndexChanged="ddlcollegestaff_SelectedIndexChanged"
                                        Font-Names="MS Sans Serif" Font-Size="Small" Height="20px" Width="101px" Font-Bold="true"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>--%>
                                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                    background-color: lightblue; border-width: 1px;">
                                    <asp:Label ID="lblstaff" runat="server" Text="Designation" Font-Bold="True" ForeColor="Black"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                    border-width: 1px; border-right-style: solid;">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtstaff" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="110px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pstaff" runat="server" Height="400px" Width="241px" Style="text-align: left;"
                                                CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="chksatff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="chksatff_CheckedChanged" Text="Select All"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklststaff" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="chklststaff_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtstaff"
                                                PopupControlID="pstaff" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                                    background-color: lightblue; border-width: 1px;">
                                    <asp:Label ID="lblstaffDept" runat="server" Text="Department" Font-Bold="True" ForeColor="Black"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 269px; left: 480px;"></asp:Label>
                                </td>
                                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                    border-width: 1px; border-right-style: solid;">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtstaffDept" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="110px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pstaffDept" runat="server" Height="400px" Width="335px" Style="text-align: left;"
                                                CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="chksatffDept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="chksatffDept_CheckedChanged" Text="Select All"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklststaffDept" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="chklststaffDept_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtstaffDept"
                                                PopupControlID="pstaffDept" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbcancel" runat="server" Text="Include Cancel" />
                                    
                                </td>
                               
                            </tr>
                            
                            
                            
                           
                        </table>
                        <table>
                        <tr>
                                                <td>
                                                    <asp:Label ID="Label17" runat="server" Text="NoOfRecords" Style="margin-left: 1px;">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    
                                                            <asp:TextBox ID="Txt_PageNo" runat="server" Style="width: 60px; margin-left: 0px;"
                                                                CssClass="textbox txtheight2" AutoPostBack="true" OnTextChanged="Txt_PageNo_OnTextChanged"></asp:TextBox>
                                                        
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblpge" runat="server" Text="Page No">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    
                                                            <asp:Button ID="btn_Previous" Style="width: 32px; margin-left: -4px;"  runat="server" CssClass="textbox btn2"
                                                                Text="<<" OnClick="btn_Previous_Click" />
                                                        
                                                </td>
                                                <td>
                                                    
                                                            <asp:DropDownList ID="ddl_Txt_PageNo" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Style="width: 142px; margin-left: -2px;" AutoPostBack="True" OnSelectedIndexChanged="ddl_Txt_PageNo_OnSelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        
                                                </td>
                                                <td>
                                                    
                                                            <asp:Button ID="btn_Next" Style="width: 32px; margin-left: -1px;" runat="server"
                                                                CssClass="textbox btn2" Text=">>" OnClick="btn_Next_Click" />
                                                        
                                                </td>
                                                </tr>
                                 </table>
                                
                        
                                </table>
                               
                                <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                    <ContentTemplate>
                        <asp:Button ID="btnMainGo" runat="server" Style="margin-left: 360px; top: 210px; width: 47px;
                            position: absolute;" Text="Go" Font-Bold="True" OnClick="btnMainGo_Click" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                             <asp:Button ID="btnAdd" runat="server" Style="margin-left: 410px; top: 210px; width: 47px;
                            position: absolute;" Text="Add" Font-Bold="True" OnClick="btnAdd_Click" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                        <asp:Panel ID="Panel3" runat="server" BackImageUrl="~/image/Top Band-2.jpg" Height="12px">
                        </asp:Panel>
                         
                        <asp:Label ID="lblerrmainapp" runat="server" Text="" Visible="false" ForeColor="Red"
                            CssClass="font"></asp:Label>
                            


                                             
                 <div id="popheader" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 33px; margin-left: 307px;"
                        OnClick="ImageButton2_Click" />
                    <br />
                    <br />
                    <div class="subdivstyle" style="background-color: White; overflow: auto; width: 650px;
                        height: 315px;" align="center">
                        <br />
                        <br />
                        <br />
                        <center>
                            <table style="width: 540px;" runat="server" id="collinfo">
                                <tr>
                                    <td>
                                        <fieldset>
                                            <asp:CheckBox ID="chkselall" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="chkselall_CheckedChanged" />
                                            <asp:CheckBoxList ID="chkcollege" runat="server" Height="43px" AutoPostBack="true"
                                                RepeatColumns="3" RepeatDirection="Vertical">
                                                <asp:ListItem Value="0" Text="College Name"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="University"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Affliated By"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Address"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="City"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="District & State & Pincode"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="Phone No & Fax"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="Email & Web Site"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="Right Logo"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="Left Logo"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btnsavehead" runat="server" CssClass="textbox textbox1 btn2" Text="Save"
                                OnClick="btnsavehead_Click" />
                            <asp:Button ID="btnexithead" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                                OnClick="btnexithead_Click" />
                        </center>
                    </div>
                </div>

                                            <div id="popfooter" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 33px; margin-left: 243px;"
                        OnClick="ImageButton1_Click" />
                    <br />
                    <br />
                    <div class="subdivstyle" style="background-color: White; overflow: auto; width: 525px;
                        height: 315px;" align="center">
                        <br />
                        <br />
                        <br />
                        <center>
                            <table style="width: 330px;" runat="server" id="Table1">
                                <tr>
                                    <td>
                                        Footer1
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfoot0" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Footer2
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfoot1" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Footer3
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfoot2" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Footer4
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfoot3" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Footer5
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfoot4" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btnsavefoot" runat="server" CssClass="textbox textbox1 btn2" Text="Save"
                                OnClick="btnsavefoot_Click" />
                            <asp:Button ID="btnexitfoot" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                                OnClick="btnexitfoot_Click" />
                        </center>
                    </div>
                </div>
                        <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alert1" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" CssClass="textbox textbox1 btn1" OnClick="btn_errorclose_Click"
                                                    Text="OK" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>                    
                           
                           
                        <%--      <FarPoint:FpSpread ID="sprdMainapplication" runat="server" Height="250px" Width="900px"
                            ActiveSheetViewIndex="0" currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                            EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
                            OnCellClick="sprdMainapplication_CellClick" OnPreRender="sprdMainapplication_SelectedIndexChanged"
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
                        </FarPoint:FpSpread>--%>
                        <asp:Button ID="btnDel" runat="server" Visible="false" Style="left: 760px; top: 200px;
                            width: 47px; position: absolute;" Text="Delete" Font-Bold="true" Width="60px"
                            Font-Names="Book Antiqua" OnClick="btnDel_Click" />
                        <asp:Button ID="btnCan" runat="server" Visible="false" Style="left: 807px; top: 200px;
                            width: 47px; position: absolute;" Text="Cancel" Font-Bold="true" Width="60px"
                            Font-Names="Book Antiqua" OnClick="btnCan_Click" />
                            <br />
                            <br />
                         
                                   <div>
                        <center>
                            <asp:Panel ID="pheaderfilter0" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="850px" Style="margin-top: -0.1%;">
                                <asp:Label ID="lbl_st" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Image7" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <br />
                    </div>
                    <asp:Panel ID="pcolumnorder0" runat="server" CssClass="maintablestyle" Width="850px">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="CheckBox_column0" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column0_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton8" runat="server" Font-Size="X-Small" Height="16px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                        Visible="false" Width="111px" OnClick="LinkButtonsremove0_Click">Remove  All</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                    <asp:TextBox ID="tborder0" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                        AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblcolumnorder0" runat="server" Height="43px" AutoPostBack="true"
                                        Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                        RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder0_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="Route">Route
</asp:ListItem>
                                         <asp:ListItem Selected="True" Value="Vehicle_ID">Vehicle ID
</asp:ListItem>
                                         <asp:ListItem Selected="True" Value="Seat_No">Seat No
</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Start_Place
">Start Place

</asp:ListItem>
                                         <asp:ListItem Selected="True" Value="Branch
">Branch

</asp:ListItem>
                                         <asp:ListItem Selected="True" Value="Photo
">Photo

</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="year_of_study">Year Of Study
</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="fee_status">Fee Status</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Student_Contact
">Student Contact
</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="parents_contact">Parents Contact</asp:ListItem>
                                        
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder0" runat="server" TargetControlID="pcolumnorder0"
                        CollapseControlID="pheaderfilter0" ExpandControlID="pheaderfilter0" Collapsed="true"
                        TextLabelID="lbl_st" CollapsedSize="0" ImageControlID="Image7" CollapsedImage="right.jpeg"
                        ExpandedImage="down.jpeg">
                    </asp:CollapsiblePanelExtender>
                  

                     
                        
                        
                        <br />
                        <br />
                        <center>
                            <FarPoint:FpSpread ID="Fpload" runat="server" BorderStyle="Solid" OnButtonCommand="Fpload_OnButtonCommand"
                                BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                class="spreadborder">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </center>

                        <br />
                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                        <ContentTemplate>
                                            <label id="lbl_pagecnt" runat="server" visible="false" style="background-color: Green">
                                            </label>
                                            <label id="lbl_totrecord" runat="server" visible="false" style="background-color: Green">
                                            </label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <br />
                                    <br />
                            </asp:Panel>
                         </ContentTemplate>
                 </asp:UpdatePanel>
                        <div style="height: 20px;">
                        </div>
                        <div style="text-align: center;">
                            <asp:Label ID="lbprint" runat="server" Visible="false" Style="color: Red;"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" onkeypress="return keyvalue(this)"
                                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                            <asp:Button ID="btn_excel" runat="server" Text="Export Excel" Font-Bold="true" Font-Names="Book Antiqua"
                                OnClick="btn_excel_Click" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                <asp:Button ID="btndirectprint" runat="server" Text="Direct Print" OnClick="btndirectprint_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" /><%-- rajasekar --%>
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                  <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                    
                    <%--</ContentTemplate>
         </asp:UpdatePanel>

                    --%>
                    <%--  <asp:UpdatePanel ID="updisp" runat="server">
                        <ContentTemplate>--%>
                    <center>
                        <div id="Div1" runat="server" visible="false" style="height: 100em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                    border-radius: 10px;">
                                    <center>
                                        <table style="height: 100px; width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lbldisp" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:Button ID="btndelclose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                            width: 65px;" OnClick="btndelclose_Click" Text="ok" runat="server" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </center>
                        </div>
                    </center>
                    <%--/ContentTemplate>
                    </asp:UpdatePanel>--%><%--delete and cancel--%><center>
                        <div id="divCan" runat="server" visible="false" style="height: 100em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="Div4" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                    border-radius: 10px;">
                                    <center>
                                        <table style="height: 100px; width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblCan" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:Button ID="btnOkCan" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                            width: 65px;" OnClick="btnOkCan_Click" Text="ok" runat="server" />
                                                        <asp:Button ID="buttCanCEl" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                            width: 65px;" OnClick="buttCanCEl_Click" Text="Cancel" runat="server" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </center>
                        </div>
                    </center>
                    <center>
                        <div id="divDel" runat="server" visible="false" style="height: 100em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                    border-radius: 10px;">
                                    <center>
                                        <table style="height: 100px; width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblDel" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:Button ID="btnOkDel" Visible="false" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                            width: 65px;" OnClick="btnOkDel_Click" Text="ok" runat="server" />
                                                        <asp:Button ID="buttDelCEl" Visible="false" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                            width: 65px;" OnClick="buttDelCEl_Click" Text="Cancel" runat="server" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </center>
                        </div>
                    
                                
                                <%-- <td>
                                    <asp:Button ID="btnDel" runat="server" Text="Delete" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btnDel_Click" />
                                </td>--%>
                                
</center>
                        
  </Content>



  <asp:UpdatePanel ID="upnlODStudDetails" runat="server">
        <ContentTemplate>
            <center>
                <asp:UpdateProgress ID="upgODDetails" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upnlODStudDetails">
                    <ProgressTemplate>
                        <center>
                            <div class="CenterPB" style="height: 40px; width: 40px; text-align: center;">
                                <img alt="" src="../images/progress2.gif" height="180px" width="180px" />
                            </div>
                        </center>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </center>
  <asp:ModalPopupExtender ID="mPopExtODDetails" runat="server" TargetControlID="upgODDetails"
                PopupControlID="upgODDetails">
            </asp:ModalPopupExtender>
            <center>
                <div id="divTarvellerEntryDetails" runat="server" visible="false" style="height: 70em; z-index: 100;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0px;">
                    <center>
                        <%--left: 15%; right: 15%; position: absolute;--%>
                        <div id="divTarvellerEntry" runat="server" class="table" style="background-color: White;
                            border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-left: auto;
                            margin-right: auto; width: 750px; height: 580px; z-index: 1000; border-radius: 5px;">
                            <asp:Label ID="lbl_clgT" runat="server" Visible="false"></asp:Label>
                            <center>
                                <span style="top: 20px; bottom: 20px; text-align: center; color: Green; font-size: large;
                                    position: relative; font-weight: bold;">Traveller Allotment Entry </span>
                            </center>
                           
                    <%--<asp:UpdatePanel ID="UpdatedAdd" runat="server">
                    
                 <ContentTemplate>  --%>
                 <center>
                    <asp:Panel ID="Panel1" runat="server" Style=" border-color: Gray; border-style: solid;
                        width: 670px; height: 386px; margin-top:100px; margin-bottom: 0px; margin-right: 58px; margin-left: 0px;">
                        <center>
                        <center>
                            <table class="tabl" style="top: 267px;  position: absolute; width: 368px;
                                border-color: Gray; border-width: thin;margin-left: 100px; margin-top: -80px; height: 220px;">
                                <tr>
                                    <td class="style5">
                                        <asp:Label ID="lblenqno" runat="server" Text="Roll No" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25" align="right">
                                        <asp:TextBox ID="tbenqno" runat="server" AutoPostBack="true" MaxLength="25" OnTextChanged="tbenqno_TextChanged"
                                            Font-Names="MS Sans Serif" Font-Size="Small" Height="15px" Width="120px" Style="margin-right: 29px;"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="tbenqno"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                            <%--    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground"--%>
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="enqbtn" runat="server" Text="?" Height="20px" OnClick="enqbtn_Click"
                                            Width="20px" />
                                        <table class="tabl" style="top: -38px; left: 0px; position: absolute; width: 157px;
                                            border-color: Gray; border-width: thin; height: 0px;">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlclgstud" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlclgstud_Selected"
                                                        CssClass="textbox ddlstyle ddlheight3" Width="202px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <table class="tabl" style="top: -28px; left: 212px; position: absolute; width: 157px;
                                            border-color: Gray; border-width: thin; height: 0px;">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rbdirectapply" Text="Student" runat="server" AutoPostBack="True"
                                                        OnCheckedChanged="rbdirectapply_CheckedChanged" GroupName="ApplyType" Font-Names="MS Sans Serif"
                                                        Font-Size="Small" Checked="true" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbenquiry" Text="Staff" runat="server" GroupName="ApplyType"
                                                        Font-Names="MS Sans Serif" Font-Size="Small" AutoPostBack="True" OnCheckedChanged="rbenquiry_CheckedChanged"  />
                                                </td>
                                            </tr>
                                        </table>
                                        <table class="tabl" style="top: -28px; left: 400px; position: absolute; width: 110px;
                                            border-color: Gray; border-width: thin; height: 80px;">
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="photo" runat="server" Visible="false" Width="100px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <asp:Label ID="lblStageCost" runat="server" Visible="false" Font-Size="Large" ForeColor="Green"></asp:Label>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                        <asp:Label ID="Label1" runat="server" Text="Student Name" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55" align="right">
                                    </td>
                                    <td class="style25" align="right">
                                        &nbsp;
                                        <asp:TextBox ID="tbpname" runat="server" AutoPostBack="true" OnTextChanged="tbpname_OnTextChanged"
                                            Font-Names="MS Sans Serif" Font-Size="Small" Height="15px" Width="150px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="tbpname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                        <asp:Label ID="Label2" runat="server" Text="Degree" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55" align="right">
                                    </td>
                                    <td class="style25" align="right">
                                        &nbsp;
                                        <asp:TextBox ID="tbdept" runat="server" AutoPostBack="true" Font-Names="MS Sans Serif"
                                            Font-Size="Small" Height="15px" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                        <asp:Label ID="Label4" runat="server" Text="Boarding Place" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55" align="right">
                                    </td>
                                    <td class="style25" align="right">
                                        &nbsp;
                                        <asp:TextBox ID="tbborplace" runat="server" AutoPostBack="true" Enabled="true" Font-Names="MS Sans Serif"
                                            Font-Size="Small" Height="15px" Width="150px" OnTextChanged="tbborplace_clicked"></asp:TextBox>
                                        <%-- <asp:AutoCompleteExtender ID="tbborplace_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetListofCountries" MinimumPrefixLength="1" EnableCaching="true"
                                            ServicePath="" TargetControlID="tbborplace">
                                        </asp:AutoCompleteExtender>--%>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetListofCountries" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="tbborplace"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <asp:Label ID="Lblplace_Value" runat="server" Text="" Font-Names="MS Sans Serif"
                                        Font-Size="Small" Visible="false"></asp:Label>
                                    <td>
                                        <asp:Button ID="btnroute" runat="server" Text="?" Height="20px" OnClick="routebtn_Click"
                                            Visible="true" Width="20px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                        <asp:Label ID="Label5" runat="server" Text="Vehicle ID" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55" align="right">
                                    </td>
                                    <td class="style25" align="right">
                                        &nbsp;
                                        <asp:TextBox ID="tbvehno" runat="server" AutoPostBack="true" Enabled="false" Font-Names="MS Sans Serif"
                                            Font-Size="Small" Height="15px" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                        <asp:Label ID="Label3" runat="server" Text="Route" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55" align="right">
                                    </td>
                                    <td class="style25" align="right">
                                        <asp:TextBox ID="tbroute" runat="server" AutoPostBack="true" MaxLength="25" Enabled="false"
                                            Font-Names="MS Sans Serif" Font-Size="Small" Height="15px" Width="120px" Style="margin-right: 29px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <td class="style5">
                                    <asp:Label ID="Label6" runat="server" Text="SeatNo" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                </td>
                                <td class="style55">
                                </td>
                                <td class="style25" align="right">
                                    &nbsp;
                                    <asp:TextBox ID="tbseatno" runat="server" AutoPostBack="true" OnTextChanged="tbseatno_TextChanged"
                                        MaxLength="3" Font-Names="MS Sans Serif" Font-Size="Small" Height="15px" Width="35px"
                                        Style="margin-right: 114px;"></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Text="Cost"></asp:Label>
                                    </td>
                                    <td class="style55">
                                    </td>
                                    <td class="style25" style="padding-left: 35px">
                                        &nbsp;
                                        <asp:Label ID="lbl_cost" runat="server" Text="NA" Style="background-color: Green;
                                            color: White; border: 2px;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                        <asp:Label ID="lbldate" runat="server" Text="Date" Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
                                    </td>
                                    <td class="style55" align="right">
                                    </td>
                                    <td class="style25" align="right">
                                        &nbsp;
                                        <asp:TextBox ID="tbdate" runat="server" AutoPostBack="true" Enabled="true" OnTextChanged="tbdate_TextChanged"
                                            Font-Names="MS Sans Serif" Font-Size="Small" Height="15px" Width="70" Style="margin-right: 79px;"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="tbdate" Format="dd-MM-yyyy"
                                            runat="server" Enabled="True">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <center>
                            <table class="tabl" style="top: 500px; position: absolute; width: 280px;
                                border-color: Gray; border-width: thin;margin-left: 100px; margin-top: -80px; height: 0px;">
                                <tr>
                                    <%-- <td>
                                        <asp:Label ID="lblTypestu" runat="server" Text="Type" Font-Names="MS Sans Serif"
                                            Font-Size="Small"></asp:Label>
                                    </td>--%>
                                    <td>
                                        <asp:RadioButton ID="rbsemtype" Text="Sem" runat="server" OnCheckedChanged="rbsemtype_Changed"  GroupName="typestu" Font-Names="MS Sans Serif" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbstutype" Text="Yearly" runat="server" OnCheckedChanged="rbstutype_Changed"
                                            AutoPostBack="true" GroupName="typestu" Font-Names="MS Sans Serif" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbtermtype" Text="Term" runat="server" OnCheckedChanged="rbtermtype_Changed"
                                            AutoPostBack="true" GroupName="typestu" Font-Names="MS Sans Serif" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbtranfer" Text="Monthly" runat="server" OnCheckedChanged="rbtranfer_Changed"
                                            AutoPostBack="true" GroupName="typestu" Font-Names="MS Sans Serif" Font-Size="Small" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <center>
                        <table class="tablfont" style="top: 548px;margin-left: 100px; margin-top: -80px; position: absolute;
                            width: 384px; height: 30px; border-color: Gray;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" ForeColor="Blue" Text="Total Seats     :"
                                        Font-Names="MS Sans Serif" Font-Size="8pt" Visible="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbltotalseat" runat="server" ForeColor="Blue" Text="0" Font-Names="MS Sans Serif"
                                        Font-Size="8pt" Visible="true" Style="top: 9px; position: absolute; left: 80px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" ForeColor="Green" Text="Allocated Seats:"
                                        Font-Names="MS Sans Serif" Font-Size="8pt" Visible="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblallotedSeat" runat="server" ForeColor="Green" Text="0" Font-Names="MS Sans Serif"
                                        Font-Size="8pt" Visible="true" Style="top: 9px; position: absolute; left: 208px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label14" runat="server" ForeColor="Red" Text="Remaining Seats:" Font-Names="MS Sans Serif"
                                        Font-Size="8pt" Visible="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblremaingSeat" runat="server" ForeColor="red" Text="0" Font-Names="MS Sans Serif"
                                        Font-Size="8pt" Visible="true" Style="top: 9px; position: absolute; left: 344px;"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        </center>
                        <center>
                        <table class="tablfont" style="top: 578px;margin-left: 100px; margin-top: -80px; position: absolute;
                            width: 384px; height: 35px; border-color: Gray;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label15" runat="server" Text="Fee Category" Font-Names="MS Sans Serif"
                                        Font-Size="8pt" Visible="true" Style="top: 10px; position: absolute; left: 10px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPD5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtfeeset" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                Width="140px" Style="top: 6px; left: 95px; height: 20px; position: absolute;
                                                font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pfeeset" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="chkfeeset" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkfeeset_CheckedChange" />
                                                <asp:CheckBoxList ID="chklsfeeset" runat="server" Font-Size="Medium" Font-Bold="True"
                                                    Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="chkfeeset_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PUCE1" runat="server" TargetControlID="txtfeeset" PopupControlID="pfeeset"
                                                Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btnfeeset" runat="server" Text="Update Fess" OnClick="btnfeeset_Click"
                                        Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Style="top: 5px;
                                        position: absolute; left: 245px; height: 25px;" />
                                </td>
                            </tr>
                        </table>
                        </center>
                        <center>
                        <table style="top: 350px; position: absolute;margin-left: 470px; margin-top: -80px; width: 200px;
                            height: 60px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblfeecat" runat="server" Text="Fee Category" CssClass="font" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="fee_cate" runat="server" Enabled="false" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblconcession" runat="server" Text="Concession" CssClass="font" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtconcession" runat="server" CssClass="font" Width="70px" MaxLength="5"
                                        Visible="false"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtconcession"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                        </center>
                        <center>

                    <table class="tablfont" style="top: 500px;margin-left: 390px; margin-top: -80px; position: absolute;
                        width: 179px; height: 28px; border-color: Gray;">
                        <tr>
                            <td>
                                <asp:Button ID="Buttondelete" runat="server" Text="New" OnClick="Buttondelete_Click"
                                    Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False"
                                    ForeColor="Black" Width="70px" Height="25px" Enabled="true" />
                            </td>
                            <td>
                                <asp:Button ID="Buttonsave" runat="server" Text="Save" OnClick="Buttonsave_Click"
                                    Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False"
                                    ForeColor="Black" Width="60px" Height="25px" />
                            </td>
                            <td>
                                <asp:Button ID="Btn_Delete" runat="server" Visible="false" Text="Delete" OnClick="Btn_Delete_Click"
                                    Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False"
                                    ForeColor="Black" Width="60px" Height="25px" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancel" runat="server" Visible="false" Text="Cancel" OnClick="btnCancel_Click"
                                    Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False"
                                    ForeColor="Black" Width="60px" Height="25px" />
                            </td>
                        </tr>
                    </table>
                    </center>
                        <asp:Label ID="lblerrdate" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                            Font-Size="8pt" Visible="false" Style="position: absolute; top: 493px; left: 139px;"></asp:Label>
                    </asp:Panel>
                    </center>
                    <asp:Label ID="Labelvalidationdate" runat="server" ForeColor="Red" Text="" Font-Names="MS Sans Serif"
                        Font-Size="3pt" Visible="false" Style="position: absolute; top: 121px; left: 114px;"></asp:Label>
                    
                     <asp:Button ID="btnExit" runat="server" Text="Exit" OnClick="btnExit_Click"  
                                    Font-Bold="true" Font-Names="MS Sans Serif" Font-Size="Small" Font-Underline="False"
                                    ForeColor="Black" Width="60px" Height="25px" />
                    <table id="tablmnth" runat="server" visible="false" class="tablfont" style="top: 541px;
                        margin-top: -80px; left: 139px; position: absolute; width: 425px; height: 37px;
                        border-color: Gray;">
                        <tr>
                            <td>
                                Month
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlmonth" CssClass="textbox3 textbox1" Enabled="false" runat="server"
                                    Style="width: 80px; height: 28px;" onfocus="myFunction(this)">
                                    <%--  <asp:ListItem Value="1">JAN</asp:ListItem>
                                    <asp:ListItem Value="2">FEB</asp:ListItem>
                                    <asp:ListItem Value="3">MAR</asp:ListItem>
                                    <asp:ListItem Value="4">APR</asp:ListItem>
                                    <asp:ListItem Value="5">MAY</asp:ListItem>
                                    <asp:ListItem Value="6">JUN</asp:ListItem>
                                    <asp:ListItem Value="7">JUL</asp:ListItem>
                                    <asp:ListItem Value="8">AUG</asp:ListItem>
                                    <asp:ListItem Value="9">SEP</asp:ListItem>
                                    <asp:ListItem Value="10">OCT</asp:ListItem>
                                    <asp:ListItem Value="11">NOV</asp:ListItem>
                                    <asp:ListItem Value="12">DEC</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Year
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlyear" CssClass="textbox3 textbox1" runat="server" Enabled="false"
                                    onfocus="myFunction(this)" Style="width: 100px; height: 28px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkMultipleMonth" runat="server" Visible="false" Text="Allot Multiple Month"
                                    OnClick="lnkMultipleMonth_Clik"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hfdegree" runat="server" />
                    <asp:HiddenField ID="hfapplydegree" runat="server" />
                    <asp:Panel ID="pnlupdate" runat="server" Visible="false" Style="top: 376px; border-color: Black;
                        background-color: lightyellow; border-style: solid; border-width: 0.5px; left: 446px;
                        position: absolute; width: 375px; height: 475px;">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            <asp:Label ID="lblmonthwise" runat="server" Visible="true" Text="Monthwise Allotment"
                                Font-Bold="true" Font-Size="Large" Font-Names="Book Antiqua" Style="margin-left: 6px;"></asp:Label>
                            <asp:Label ID="lblTotalAmount" runat="server" Visible="true" Text="Monthwise Allotment"
                                Font-Bold="true" Font-Size="Large" Font-Names="Book Antiqua" Style="margin-left: 20px;"></asp:Label>
                        </caption>
                        <asp:Panel ID="Panel20" runat="server" Style="top: 48px; border-color: Black; background-color: lightyellow;
                            border-style: solid; border-width: 0.5px; left: 4px; position: absolute; width: 330px;
                            height: 334px;">
                            <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="0.5" autopostback="true" ClientAutoCalculation="true" ShowHeaderSelection="false"
                                OnUpdateCommand="FpSpread3_Command">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true" GridLineColor="Black">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>

                        </asp:Panel>
                       
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Text="" Font-Bold="true"
                            Font-Size="Large" Visible="false" Font-Names="Book Antiqua" Style="top: 443px;
                            left: 10px; position: absolute;"></asp:Label>
                        <asp:Button ID="btnok" runat="server" Text="Ok" OnClick="btnok_Click" Style="top: 411px;
                            left: 58px; position: absolute; height: 27px; width: 88px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                        <asp:Button ID="btnexi" runat="server" Text="Exit" OnClick="btnexi_Click" Style="top: 411px;
                            left: 155px; position: absolute; height: 27px; width: 88px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="Panellookup1" Visible="false" BackColor="AliceBlue"
                        Style="border: thin solid Black; left: 23px; top: 185px; width: 978px; height: 562px;
                        position: absolute;">
                        <asp:Button ID="btncloselook1" OnClick="btncloselook1_Click" runat="server" Text="X"
                            Height="21px" BackColor="Transparent" BorderColor="Transparent" CssClass="floatr" />
                        <center>
                            <asp:Label ID="Label25" runat="server" Text="Student LookUp" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </center>
                        <table style="width: 385px; height: 85px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblcollege1" runat="server" Text="College_Name" Font-Bold="true" Font-Names="MS Sans Serif"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollegenew" runat="server" OnSelectedIndexChanged="ddlcollegenew_SelectedIndexChanged"
                                        Font-Names="MS Sans Serif" Font-Size="Small" Height="20px" Width="251px" Font-Bold="true"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="MS Sans Serif"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                        Font-Names="MS Sans Serif" Font-Size="Small" Height="20px" Width="70px" Font-Bold="true"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="true" Font-Names="MS Sans Serif"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDegree" runat="server" Font-Bold="true" Font-Names="MS Sans Serif"
                                        Font-Size="Small" Height="20px" Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="true" Font-Names="MS Sans Serif"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBranch1" runat="server" Font-Bold="true" Font-Names="MS Sans Serif"
                                        Font-Size="Small" Height="20px" Width="185px" OnSelectedIndexChanged="ddlBranch1_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnlookupgo1" runat="server" Text="Go" Height="21px" Style="top: 53px;
                                        position: absolute; left: 870px;" CssClass="font" OnClick="btnlookupgo1_Click" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 462px; height: 25px; top: 81px; position: absolute;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label87" runat="server" Text="Search By" Font-Names="MS Sans Serif"
                                        Font-Size="Small" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlheader" runat="server" AutoPostBack="true" Width="100px"
                                        OnSelectedIndexChanged="ddlheader_SelectedIndexChanged" Font-Names="MS Sans Serif"
                                        Font-Size="Small">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddloperator" runat="server" AutoPostBack="true" Width="100px"
                                        OnSelectedIndexChanged="ddloperator_SelectedIndexChanged" Font-Names="MS Sans Serif"
                                        Font-Size="Small" Enabled="False">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbvalue" runat="server" AutoPostBack="true" OnTextChanged="tbvalue_TextChanged"
                                        Font-Names="MS Sans Serif" Font-Size="Small" Height="15px" Width="153px" Enabled="False"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblerrefp1" runat="server" Text="" Visible="false" ForeColor="Red"
                            CssClass="font" Style="top: 26px; position: absolute;"></asp:Label>
                        <table style="width: 395px; height: 182px;">
                            <tr>
                                <td>
                                    <center>
                                        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                            OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged"
                                            BorderWidth="1px" Width="592" Height="117" Style="top: 110px; position: absolute;
                                            left: 73px;">
                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                ButtonShadowColor="ControlDark">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnllookstaff" Visible="false" BackColor="AliceBlue"
                        Style="border: thin solid Black; left: 23px; top: 185px; width: 809px; height: 500px;
                        position: absolute;">
                        <asp:Button ID="Button2" OnClick="btncloselook2_Click" runat="server" Text="X" Height="21px"
                            BackColor="Transparent" BorderColor="Transparent" CssClass="floatr" />
                        <center>
                            <asp:Label ID="Label10" runat="server" Text="Staff LookUp" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </center>
                        <table style="width: 385px; height: 85px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblcolleges" runat="server" Text="College Name" Font-Names="MS Sans Serif"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcolleges" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcolleges_SelectedIndexChanged"
                                        Style="top: 29px; position: absolute; left: 104px;" Font-Names="MS Sans Serif"
                                        Font-Size="Small" Height="20px" Width="251px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label99" runat="server" Text="Department" Font-Names="MS Sans Serif"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldepartment" runat="server" Font-Names="MS Sans Serif" Font-Size="Small"
                                        Height="20px" Width="263px" AutoPostBack="True" OnSelectedIndexChanged="ddldepartment_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <%--     <td>
    <asp:Label ID="Label100" runat="server" Text="Staff Category"  
              Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
    </td>
        
    <td>
    <asp:DropDownList ID="ddlstaffcategory" runat="server" style="left:378px; position:absolute; top:72px;"  
            Font-Names="MS Sans Serif" Font-Size="Small" Height="20px" Width="124px" 
            AutoPostBack="True" 
            onselectedindexchanged="ddlstaffcategory_SelectedIndexChanged">
        </asp:DropDownList>
  </td>--%>
                                <%--<td>
    <asp:Label ID="Label101" runat="server" Text="Name"  
                Font-Names="MS Sans Serif" Font-Size="Small"></asp:Label>
    </td>
        
    <td>
    <asp:DropDownList ID="ddlstaffname" runat="server"  AutoPostBack="true" 
            Font-Names="MS Sans Serif" Font-Size="Small" Height="20px" Width="124px" >
        </asp:DropDownList>

    </td>  
   <td>
  
   </td> --%>
                            </tr>
                        </table>
                        <asp:Label ID="lblerrstaff" runat="server" Text="" Visible="false" ForeColor="Red"
                            CssClass="font"></asp:Label>
                        <table style="width: 395px; height: 182px;">
                            <tr>
                                <td>
                                    <center>
                                        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                            OnCellClick="FpSpread2_CellClick" OnPreRender="FpSpread2_SelectedIndexChanged"
                                            BorderWidth="1px" Width="494" Height="117" Style="top: 133px; position: absolute;
                                            left: 145px;">
                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                ButtonShadowColor="ControlDark">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="Panellookup" Visible="false" BackColor="AliceBlue"
                        Style="border: thin solid Black; left: 23px; top: 185px; width: 871px; height: 5500px;
                        position: absolute;">
                        <asp:Button ID="btncloselook" OnClick="btncloselook_Click" runat="server" Text="X"
                            Height="21px" BackColor="Transparent" BorderColor="Transparent" CssClass="floatr" />
                        <asp:Label ID="lbllerrorlook" runat="server" Text="" Visible="false" ForeColor="Red"
                            CssClass="font"></asp:Label>
                        <%--<table class="tablfont" style="left:45px; position: absolute; width:587px; height: 30px; right:189px;">--%>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblvehicletype" runat="server" Font-Bold="true" CssClass="font" Text="Search By Place"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlserachby" runat="server" Font-Bold="true" CssClass="font"
                                        Width="122px" AutoPostBack="True" OnSelectedIndexChanged="ddlserachby_SelectedIndexChanged"
                                        Visible="false">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblrouteid" runat="server" Font-Bold="true" Visible="false" CssClass="font"
                                        Text="Search By Route_ID"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlrouteID" runat="server" Font-Bold="true" CssClass="font"
                                        Width="122px" AutoPostBack="True" OnSelectedIndexChanged="ddlrouteID_SelectedIndexChanged"
                                        Visible="false">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="Button3" runat="server" Text="Go" Font-Bold="True" OnClick="btnMainGo1_Click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" />
                                </td>
                            </tr>
                        </table>
                        <center>
                            <caption id="Caption4" runat="server" style="height: 10px; top: 15px; font-family: MS Sans Serif;
                                font-size: large; font-weight: bold;">
                                BOARDING DETAILS</caption>
                        </center>
                        <table>
                            <asp:Label ID="lblerrmainapp1" runat="server" Text="" Visible="false" ForeColor="Red"
                                CssClass="font" Style="top: 42px; position: absolute;"></asp:Label>
                            <tr>
                                <td>
                                    <center>
                                        <FarPoint:FpSpread ID="fpapplied" runat="server" BorderColor="Black" BorderStyle="Solid"
                                            OnCellClick="fpapplied_CellClick" OnPreRender="fpapplied_SelectedIndexChanged"
                                            BorderWidth="1px" Width="740" Style="left: 85px; top: 62px; position: absolute;">
                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                ButtonShadowColor="ControlDark">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <%-- Pop Alert--%>
                    <center>
                        <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
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
                                                        <asp:Button ID="btn_alertclose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                            width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
                                                    </center>
                                                </td>





                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </center>
                        </div>
                    </center>
                </Content>
                        </div>

                    </center>

                </div>
            </center>
            </ContentTemplate>
            </asp:UpdatePanel>


            </ContentTemplate>
            
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







    <style type="text/css">
        ody, input
        {
            font-family: Tahoma;
            font-size: 11px;
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
            width: 200px;
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
        
        .style3
        {
            width: 383px;
        }
        .style8
        {
            width: 86px;
        }
        .style9
        {
            width: 417px;
        }
        .style10
        {
            width: 20px;
        }
        .style11
        {
            width: 138px;
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
