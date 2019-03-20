<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="PresentnAbsentCountDetails.aspx.cs" Inherits="PresentnAbsentCountDetails" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Present
            & absent Count Details</span>
        <div style="width: 100%; margin: 0px; margin-bottom: 10px; margin-top: 10px;" visible="true">
            <table class="maintablestyle" style="height: auto; width: auto;">
                <tr>
                    <td>
                        <asp:RadioButton ID="Radioformat1" runat="server" Style="" CssClass="font" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" GroupName="format" Width="100px"
                            OnCheckedChanged="Radioformat1_CheckedChanged" Text="Student" AutoPostBack="True" />
                    </td>
                    <td>
                        <asp:RadioButton ID="Radioformat2" runat="server" Style="" AutoPostBack="true" CssClass="font"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px" GroupName="format"
                            OnCheckedChanged="Radioformat2_CheckedChanged" Text="Staff" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbguest" runat="server" Style="" AutoPostBack="true" CssClass="font"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px" GroupName="format"
                            OnCheckedChanged="rdbguest_CheckedChanged" Text="Guest" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="height: 18px; width: 10px"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="182px" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"
                            AutoPostBack="True" Style="">
                        </asp:DropDownList>
                    </td>
                    <%-- <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AssociatedControlID="txtBatch"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlBatch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBatch" Visible="true" Width="67px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlBatch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="140px">
                                        <asp:CheckBox ID="chkBatch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkBatch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblBatch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblBatch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtBatch" runat="server" TargetControlID="txtBatch"
                                        PopupControlID="pnlBatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>--%>
                    <td>
                        <asp:Label ID="lbldep" runat="server" Text="Department : " Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <contenttemplate>
                                            <asp:TextBox ID="txt_dept" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 250px;">
                                                <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_dept"
                                                PopupControlID="p1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </contenttemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Degree" AssociatedControlID="txtDegree"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlDegree" runat="server">
                                <contenttemplate>
                                    <asp:TextBox ID="txtDegree" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlDegree" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="140px">
                                        <asp:CheckBox ID="chkDegree" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblDegree" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                        PopupControlID="pnlDegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </contenttemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Branch" AssociatedControlID="txtBranch"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlBranch" runat="server">
                                <contenttemplate>
                                    <asp:TextBox ID="txtBranch" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlBranch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="280px">
                                        <asp:CheckBox ID="chkBranch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblBranch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                        PopupControlID="pnlBranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </contenttemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblDate" runat="server" CssClass="font" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" AutoPostBack="true" CssClass="font" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" Height="20px" OnTextChanged="txtDate_OnTextChanged"
                            Width="83px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDate">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                            OnClick="btnGo_Click" Text="Go" Style="width: 60px; height: auto;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_colord" runat="server" Text="Report Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_colord" runat="server" CssClass="ddlheight3 textbox textbox1">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <br />
    <center>
        <div style="border-radius: 7px; width: 400px; margin-left: 722px;">
            <asp:ImageButton ID="imgbtn_columsetting" Visible="true" runat="server" Width="30px"
                Height="30px" Text="All" ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="imgbtn_all_Click" />
        </div>
        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="5px"
            BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px"
            OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged">
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <asp:UpdatePanel ID="up_spd1" runat="server">
            <contenttemplate>
                                <center>
                                    <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderWidth="5px"
                                        BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px"
                                        OnButtonCommand="fpspread2_ButtonCommand">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </center>
                            </contenttemplate>
        </asp:UpdatePanel>
    </center>
    <div id="div_report" runat="server" visible="false">
        <center>
            <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                CssClass="textbox textbox1 txtheight5" onkeypress="return ClearPrint1()"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                AutoPostBack="true" Font-Names="Book Antiqua" OnClick="btnExcel_Click" Font-Bold="true" />
            <asp:Button ID="btn_printmaster" Font-Names="Book Antiqua" runat="server" Text="Print"
                CssClass="textbox textbox1 btn2" AutoPostBack="true" OnClick="btn_printmaster_Click"
                Font-Bold="true" />
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        </center>
    </div>
    <center>
        <div id="poppernew" runat="server" visible="false" style="height: 355em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
            left: 0;">
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                width: 30px; position: absolute; margin-top: -4px; margin-left: 474px;" OnClick="imagebtnpopclose1_Click" />
            <br />
            <center>
                <div class="popsty" style="background-color: White; height: 700px; width: 974px;
                    border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                    margin-top: -8px">
                    <br />
                    <br />
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_columnordertype" Text="Report Type" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btn_addtype" runat="server" Text="+" CssClass="textbox textbox1 btn1"
                                        OnClick="btn_addtype_OnClick" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_coltypeadd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_coltypeadd_SelectedIndexChanged"
                                        CssClass="textbox textbox1 ddlheight4">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_deltype" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                        OnClick="btn_deltype_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <fieldset style="border-radius: 10px; width: 900px; height: 500px">
                        <legend style="font-size: larger; font-weight: bold">ColumnOrder Header Settings</legend>
                        <table class="table">
                            <tr>
                                <td>
                                    <asp:ListBox ID="lb_selectcolumn" runat="server" SelectionMode="Multiple" Height="490px"
                                        Width="300px"></asp:ListBox>
                                </td>
                                <td>
                                    <table class="table1">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnMvOneRt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                    Text=">" CssClass="textbox textbox1 btn1" OnClick="btnMvOneRt_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnMvTwoRt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                    Text=">>" CssClass="textbox textbox1 btn1" OnClick="btnMvTwoRt_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnMvOneLt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                    Text="<" CssClass="textbox textbox1 btn1" OnClick="btnMvOneLt_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnMvTwoLt" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                                    Text="<<" CssClass="textbox textbox1 btn1" OnClick="btnMvTwoLt_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:ListBox ID="lb_column1" runat="server" SelectionMode="Multiple" Height="490px"
                                        Width="300px"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lblalerterr" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        <br />
                        <center>
                            <asp:Button ID="btnok" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                Text="OK" CssClass="textbox textbox1 btn2" OnClick="btnok_click" />
                            <asp:Button ID="btnclose" Font-Names="Book Antiqua" Font-Bold="true" runat="server"
                                Text="Close" CssClass="textbox textbox1 btn2" OnClick="btnclose_click" />
                        </center>
                    </fieldset>
                </div>
            </center>
        </div>
    </center>
    <div id="imgdiv33" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="panel_description11" runat="server" visible="false" class="table" style="background-color: White;
                height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                margin-top: 200px; border-radius: 10px;">
                <table>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_description111" runat="server" Text="Description" Font-Bold="true"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:TextBox ID="txt_description11" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopadd_Click" />
                            <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopexit_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </center>
    </div>
    <center>
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
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
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
