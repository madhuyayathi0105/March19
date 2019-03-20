<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Student_FingerPrint_Reg.aspx.cs" Inherits="StudentMod_Student_FingerPrint_Reg" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Student FingerPrint Report</span></div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1135px; height: auto">
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <div style="width: 1000px; height: auto">
                                        <table class="maintablestyle" style="height: auto; margin-left: -23px; margin-top: 10px;
                                            margin-bottom: 10px; padding: 6px;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblclg" runat="server" Text="College">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="205px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <%--  <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                                    </asp:DropDownList>--%>
                                                    <asp:UpdatePanel ID="Upp4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_batchyr" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                Width="70px" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                                BorderWidth="2px" CssClass="multxtpanel" Width="84px" Height="180px" Style="position: absolute;">
                                                                <asp:CheckBox ID="cb_batchyear" runat="server" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_batchyear_checkedchange" />
                                                                <asp:CheckBoxList ID="cbl_batchyear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batchyear_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_batchyr"
                                                                PopupControlID="p3" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <%--    <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                                        AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                                    </asp:DropDownList>--%>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnldegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                                BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px" Style="position: absolute;">
                                                                <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_degree_checkedchange" />
                                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_degree"
                                                                PopupControlID="pnldegree" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <%--  <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                                                    </asp:DropDownList>--%>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtbranch" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlbranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                                BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px" Style="position: absolute;">
                                                                <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_branch_checkedchange" />
                                                                <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                                                                PopupControlID="pnlbranch" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblSem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <%--  <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                                                    </asp:DropDownList>--%>
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtsem" runat="server" Width="70px" CssClass="textbox  textbox1 txtheight3"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlsem" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                                BorderWidth="2px" CssClass="multxtpanel" Width="80px" Height="180px" Style="position: absolute;">
                                                                <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsem"
                                                                PopupControlID="pnlsem" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="Sec">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <%-- <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Font-Bold="True"
                                                        Visible="true" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black">
                                                    </asp:DropDownList>--%>
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtsec" runat="server" Width="70px" CssClass="textbox  textbox1 txtheight3"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlsec" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                                BorderWidth="2px" CssClass="multxtpanel" Width="80px" Height="180px" Style="position: absolute;">
                                                                <asp:CheckBox ID="cbsec" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbsec_checkedchange" />
                                                                <asp:CheckBoxList ID="cblsec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblsec_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtsec"
                                                                PopupControlID="pnlsec" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <fieldset id="fldfinger" runat="server" style="border-radius: 5px; width: 232px;">
                                                        <asp:RadioButton ID="rbfingerid" runat="server" Text="FingerID" GroupName="fin" Checked="true" />
                                                        <asp:RadioButton ID="rbnofingerid" runat="server" Text="No FingerID" GroupName="fin" />
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btngo" runat="server" Text="GO" CssClass="textbox1 btn2" OnClick="btngo_click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="textbox1 btn2" OnClick="btnAdd_click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </center>
        <br />
        <br />
        <center>
            <FarPoint:FpSpread ID="Fpspreadpop" runat="server" Visible="false" BorderColor="Black"
                BorderStyle="Solid" BorderWidth="1px" Width="650px" Height="350px" Style="margin-left: 2px;"
                class="spreadborder" ShowHeaderSelection="false" OnButtonCommand="Fpspreadpop_ButtonCommand">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br />
            <br />
            <center>
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                        Font-Size="Medium" CssClass="textbox txtheight2"></asp:TextBox>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                        Text="Export To Excel" Width="127px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Width="60px" CssClass="textbox textbox1 btn2" />
                    <asp:Button ID="btndelete" runat="server" Font-Bold="true" Visible="false" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Delete" OnClick="btndelete_Click" CssClass="textbox textbox1 btn2"
                        Width="100px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </center>
        </center>
    </div>
    <center>
        <div id="poperrjs" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0;">
            <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 430px;"
                OnClick="imagebtnpopcloseadd_Click" />
            <br />
            <br />
            <div class="subdivstyle" style="background-color: White; overflow: auto; width: 900px;
                height: 690px;" align="center">
                <br />
                <center>
                    <span class="fontstyleheader" style="color: Green;">Student Finger Print Registration</span>
                </center>
                <br />
                <table id="Table1" class="maintablestyle" runat="server" style="width: 875px;">
                    <tr>
                        <td>
                            College
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcoll" runat="server" CssClass="textbox1 ddlheight3" Width="165px"
                                OnSelectedIndexChanged="ddlcoll_Change" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbatchyear1" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <%--<asp:DropDownList ID="dlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                AutoPostBack="true" OnSelectedIndexChanged="dlbatch_SelectedIndexChanged">
                            </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbatchyear1" runat="server" CssClass="textbox  textbox1 txtheight3"
                                        Width="70px" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlbatchyear1" runat="server" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Width="84px" Height="180px"
                                        Style="position: absolute;">
                                        <asp:CheckBox ID="cb_batchyear1" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_batchyear1_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_batchYear1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batchYear1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtbatchyear1"
                                        PopupControlID="pnlbatchyear1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbldegree1" runat="server" Text="Degree" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                           <%-- <asp:DropDownList ID="dldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="dldegree_SelectedIndexChanged">
                            </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtdegree1" runat="server" CssClass="textbox  textbox1 txtheight3"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnldegree1" runat="server" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px"
                                        Style="position: absolute;">
                                        <asp:CheckBox ID="cb_degree1" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_degree1_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_degree1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txtdegree1"
                                        PopupControlID="pnldegree1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:Label ID="lblbranch1" runat="server" Text="Branch" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                          <%--  <asp:DropDownList ID="dlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="dlbranch_SelectedIndexChanged">
                            </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbranch1" runat="server" CssClass="textbox  textbox1 txtheight3"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlbranch1" runat="server" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px"
                                        Style="position: absolute;">
                                        <asp:CheckBox ID="cb_branch1" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_branch1_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_branch1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txtbranch1"
                                        PopupControlID="pnlbranch1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                           <asp:Label ID="lbl_sem" runat="server" Text="Sem" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                           <%-- <asp:DropDownList ID="dlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="dlsem_SelectedIndexChanged">
                            </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtsem1" runat="server" CssClass="textbox  textbox1 txtheight3"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlsem1" runat="server" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px"
                                        Style="position: absolute;">
                                        <asp:CheckBox ID="cb_sem1" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_sem1_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_sem1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txtsem1"
                                        PopupControlID="pnlsem1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                         <asp:Label ID="lblsec1" runat="server" Text="Sec" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            
                        </td>
                        <td>
                          <%--  <asp:DropDownList ID="dlsec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dlSec_SelectedIndexChanged"
                                Font-Bold="True" Visible="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                ForeColor="Black">
                            </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtsec1" runat="server" CssClass="textbox  textbox1 txtheight3"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlsec1" runat="server" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px"
                                        Style="position: absolute;">
                                        <asp:CheckBox ID="cb_sec1" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_sec1_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_sec1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sec1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtsec1"
                                        PopupControlID="pnlsec1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <asp:Label ID="lblstudeName" runat="server" Text="StuentName" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updddlstd" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlstdlst" runat="server" CssClass="textbox1 ddlheight4" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlstdlst_change" Width="162px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            MachineID
                        </td>
                        <td>
                            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>--%>
                            <asp:TextBox ID="txt_macid" runat="server" OnTextChanged="txt_macid_Change" AutoPostBack="true"
                                MaxLength="50" CssClass="textbox textbox1 txtheight3"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetMacID" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_macid"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="txtsearchpan">
                            </asp:AutoCompleteExtender>
                            <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </td>
                        <td>
                            FingerID
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlfingerid" runat="server" CssClass="textbox1 ddlheight3"
                                        Width="145px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btnmatch" runat="server" Text="Match" CssClass="textbox1 btn2" OnClick="btnmatch_click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblerr" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Larger" ForeColor="Red" Visible="false"></asp:Label>
                <br />
                <br />
                <div id="sp_div" runat="server" visible="true" style="width: 800px; height: 350px;
                    border-radius: 10px; background-color: White;">
                    <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                        BorderStyle="Solid" BorderWidth="1px" Width="800px" Height="350px" Style="margin-left: 2px;"
                        class="spreadborder" ShowHeaderSelection="false">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <br />
                <asp:Button ID="btnsave" runat="server" Text="Save" Visible="false" CssClass="textbox1 btn2"
                    OnClick="btnsave_click" />
                <asp:Button ID="btnexit" runat="server" Text="Exit" CssClass="textbox1 btn2" OnClick="btnexit_click" />
                <br />
                <br />
            </div>
        </div>
    </center>
    <center>
        <div id="alertpopwindow" runat="server" class="popupstyle popupheight1" visible="false"
            style="position: fixed; width: 100%; z-index: 1000; height: 100%; margin-left: 2px;
            margin-top: 25px;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 90px;
                    border-radius: 10px;">
                    <center>
                        <br />
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            Text="Ok" runat="server" OnClick="btnerrclose_Click" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
