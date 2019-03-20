<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="TermFeeReport.aspx.cs" Inherits="TermFeeReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            function columnOrderCbl() {
                var cball = document.getElementById('<%=cb_column.ClientID%>');
                var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
                var tagname = cblall.getElementsByTagName("input");
                if (cball.checked == true) {
                    for (var i = 0; i < tagname.length; i++) {
                        tagname[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < tagname.length; i++) {
                        tagname[i].checked = false;
                    }
                }
            }

            function columnOrderCb() {
                var count = 0;
                var txt = "";
                var cball = document.getElementById('<%=cb_column.ClientID%>');
                var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
                var tagname = cblall.getElementsByTagName("input");
                for (var i = 0; i < tagname.length; i++) {
                    if (tagname[i].checked == true) {
                        count += 1;
                        txt += tagname[i].value;
                    }
                }
                if (tagname.length == count) {
                    cball.checked = true;
                }
                else {
                    cball.checked = false;
                }
                alert(txt);
                return false;

            }
            var glbCheckBoxList = "";
            function btnCheckboxClicked() {
                $('#cblcolumnorder input:checkbox:checked').each(function () {
                    var checkBoxValue = $(this).parent().attr('hiddenValue');
                    glbCheckBoxList += checkBoxValue + ', ' + this.id;
                });
                $('#<%=divcolorder.ClientID %>').show();
                alert(glbCheckBoxList);
                return false;
            }
           
        </script>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#<%=btncolorderOK.ClientID %>').click(function () {
                    $('#<%=divcolorder.ClientID %>').hide();
                    return false;
                });
            });
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Term Fee Report</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" Text="College" Font-Names="Book Antiqua" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                    AutoPostBack="true" Style="height: 24px; width: 144px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbltype" Text="Type" runat="server" Font-Names="Book Antiqua" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltype" runat="server" Style="height: 24px; width: 130px;"
                                    Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbatch" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UP_batch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                            height: auto;">
                                            <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                            PopupControlID="panel_batch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UP_degree" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 125px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: auto;">
                                            <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                            PopupControlID="panel_degree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Up_dept" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 140px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                            height: auto;">
                                            <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                            PopupControlID="panel_dept" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Updp_sem" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sem" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: auto;">
                                            <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_sem_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_sem"
                                            PopupControlID="panel_sem" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label1" Text="Header" Font-Names="Book Antiqua" Style="font-family: Book Antiqua;
                                    width: 118px;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtheader" runat="server" Height="30px" CssClass="Dropdown_Txt_Box"
                                            ReadOnly="true" Width="127px" Style="height: 20px; font-family: 'Book Antiqua';">---Select---</asp:TextBox>
                                        <asp:Panel ID="paccheader" runat="server" Style="height: auto; width: 250px;" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="cbheader" runat="server" Font-Names="Book Antiqua" OnCheckedChanged="cbheader_OnCheckedChanged"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblheader" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblheader_SelectedIndexChanged"
                                                Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                            <asp:TreeView ID="treeledger" runat="server" SelectedNodeStyle-ForeColor="Red" HoverNodeStyle-BackColor="Black"
                                                Width="450px" Font-Names="Book Antiqua" ForeColor="Black" ShowCheckBoxes="All">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtheader"
                                            PopupControlID="paccheader" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtfyear" Style="height: 20px; width: 125px;" CssClass="Dropdown_Txt_Box"
                                            runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                        <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="178px">
                                            <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                                AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtfyear"
                                            PopupControlID="Pfyear" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_detre" runat="server" Text="Deduction"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtdeduct" Font-Size="Medium" Font-Names="Book Antiqua" Style="height: 20px;
                                                        width: 128px;" CssClass="Dropdown_Txt_Box" runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel multxtpanleheight" Style="height: auto;"
                                                        Width="270px">
                                                        <asp:CheckBox ID="cbdeduct" runat="server" Text="Select All" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            OnCheckedChanged="cbdeduct_OnCheckedChanged" AutoPostBack="True" />
                                                        <asp:CheckBoxList ID="cbldeduct" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            OnSelectedIndexChanged="cbldeduct_OnSelectedIndexChanged" AutoPostBack="True">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtdeduct"
                                                        PopupControlID="Panel1" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Community" runat="server" Text="Community"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_community" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panelcomm" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                        height: auto;">
                                                        <asp:CheckBox ID="cb_community" runat="server" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="cb_community_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cbl_community" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_community_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_community"
                                                        PopupControlID="panelcomm" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="checkdicon" runat="server" Text="Include Student Catagory" AutoPostBack="true"
                                                OnCheckedChanged="checkdicon_Changed" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Style="width: 200px;" />
                                        </td>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtinclude" Enabled="false" Font-Size="Medium" Font-Names="Book Antiqua"
                                                        Style="height: 20px; width: 164px;" CssClass="Dropdown_Txt_Box" runat="server"
                                                        ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                        Width="172px" Style="height: auto;">
                                                        <asp:CheckBox ID="cbinclude" runat="server" Text="Select All" Font-Size="Medium"
                                                            Font-Names="Book Antiqua" OnCheckedChanged="cbinclude_OnCheckedChanged" AutoPostBack="True" />
                                                        <asp:CheckBoxList ID="cblinclude" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged" AutoPostBack="True">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtinclude"
                                                        PopupControlID="pnlinclude" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:LinkButton ID="lnkcolorder" runat="server" Text="Column Order" OnClick="lnkcolorder_Click"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                                    OnClick="btngo_OnClick" OnClientClick=" return validation()" />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
        <%--column order div--%>
        <center>
            <div id="divcolorder" runat="server" style="height: 100%; display: none; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 230px;
                        width: 650px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 100px;
                        border-radius: 10px;">
                        <center>
                            <table>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblcolr" runat="server" Text="Column Order" Style="font-family: Book Antiqua;
                                            font-size: 20px; font-weight: bold; color: Green;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" onchange="return columnOrderCbl()" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtcolorder" runat="server" Style="height: 40px; width: 600px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="600px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                            RepeatDirection="Horizontal" onclick="return columnOrderCb()">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                        <center>
                                            <asp:Button ID="btncolorderOK" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btncolorderOK_Click" Text="OK" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </body>
</asp:Content>
