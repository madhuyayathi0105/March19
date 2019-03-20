<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master" AutoEventWireup="true" CodeFile="Room_Allocation.aspx.cs" Inherits="Room_Allocation" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .maindivstylesize
        {
            height: 800px;
            width: 1000px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
  <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       <center>
        
                <center><br />
                   <asp:Label ID="Label2" runat="server" Text="Room Allocation" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Green"></asp:Label></center>
        
                <br />
                <center>
                    <table style="width:700px; height:70px; background-color:#0CA6CA;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_college" Text="College" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_college" runat="server" AutoPostBack="true" CssClass="textbox textbox1 ddlheight4"
                                    OnSelectedIndexChanged="ddl_college_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_batch" Text="Batch" runat="server"></asp:Label>
                            </td>
                            <%-- <td><asp:DropDownList ID="ddl_batch" runat="server" CssClass="textbox textbox1 ddlheight3" OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged"></asp:DropDownList></td>--%>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox textbox1 txtheight3"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_batch_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_batch"
                                            PopupControlID="p4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_degree_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_degree"
                                            PopupControlID="p1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_department" runat="server" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox  textbox1 txtheight3"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="p2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_dept_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_dept_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_dept"
                                            PopupControlID="p2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_sem" runat="server" Text="Sem"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox  textbox1 txtheight3"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_sem"
                                            PopupControlID="p3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_add" Text="Add New" runat="server" CssClass="textbox btn2" OnClick="btn_add_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <center>
                    <asp:Label Style="color: Red;" ID="lblerr" runat="server" Visible="false"></asp:Label></center>
                <br />
                <center>
                    <div class="spreadborder" style="height: 380px; width: 860px; overflow: auto;">
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" Style="overflow: auto;
                            background-color: White;" class="spreadborder" OnCellClick="FpSpread1_CellClick"
                            OnPreRender="FpSpread1_SelectedIndexChanged">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                </center>
                <center>
                    <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                    </asp:Label></center>
                <div id="div_report" runat="server" visible="false">
                    <center>
                        <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                            CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox btn2"
                            AutoPostBack="true" OnClick="btnExcel_Click" />
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                            AutoPostBack="true" OnClick="btn_printmaster_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </center>
                </div>
            </div>
            <center>
                <div id="popwindow1" runat="server" class="popupstyle" visible="false" style="height: 61em;
                    z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0; left: 0;">
                    <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 443px;"
                        OnClick="imagebtnpop1close_Click" />
                    <br />
                    <br />
                    <div class="subdivstyle" style="background-color: White; height: 460px; width: 915px;
                        border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span style="font-size: large; color: #008000;">Room Allocation</span></div>
                        </center>
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1college" Text="College Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_pop1college" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_pop1college_SelectedIndexChanged"
                                            CssClass="textbox textbox1 ddlheight5">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1batch" Text="Batch" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_pop1batch" AutoPostBack="true" runat="server" CssClass="textbox textbox1 ddlheight4"
                                            OnSelectedIndexChanged="ddl_pop1batch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1degree" Text="Degree" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Upp11" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_pop1degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="P11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_pop1degree" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_pop1degree_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_pop1degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop1degree_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_pop1degree"
                                                    PopupControlID="P11" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1dept" Text="Department" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Upp22" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_pop1dept" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="p22" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_pop1dept" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_pop1dept_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_pop1dept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop1dept_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_pop1dept"
                                                    PopupControlID="p22" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1sem" Text="Sem" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Upp33" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_pop1sem" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="p33" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_pop1sem" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_pop1sem_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_pop1sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop1sem_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_pop1sem"
                                                    PopupControlID="p33" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_sub" Text="Subject" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_pop1sub" runat="server" AutoPostBack="true" CssClass="textbox textbox1 ddlheight6"
                                                    OnSelectedIndexChanged="ddl_pop1sub_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_roomno" runat="server" Text="Room No"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_pop1roomno" runat="server" AutoPostBack="true" CssClass="textbox ddlheight2 textbox1"
                                            OnSelectedIndexChanged="ddl_pop1roomno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1bul" Text="Building" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1bul" runat="server" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop1floor" Text="Floor" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pop1floor" runat="server" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <br />
                        <center>
                            <asp:Button ID="btn_pop1save" Text="Save" CssClass="textbox btn2" runat="server"
                                OnClick="btn_pop1save_Click" />
                            <asp:Button ID="btn_pop1exit" Text="Exit" CssClass="textbox btn2" runat="server"
                                OnClick="btn_pop1exit_Click" />
                        </center>
                        <br />
                        <div id="divdel" runat="server">
                            <asp:Button ID="btn_pop1update" Text="Update" runat="server" Visible="false" CssClass="textbox btn2"
                                OnClick="btn_pop1update_Click" />
                            <asp:Button ID="btn_pop1delete" Text="Delete" runat="server" Visible="false" CssClass="textbox btn2"
                                OnClick="btn_pop1delete_Click" />
                            <asp:Button ID="btn_pop1exit1" Text="Exit" runat="server" Visible="false" CssClass="textbox btn2"
                                OnClick="btn_pop1exit1_Click" />
                        </div>
                    </div>
                </div>
            </center>
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 245px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
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
                                                OnClick="btnerrclose_Click" Text="Yes" runat="server" />
                                            <asp:Button ID="btnclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnclose_Click" Text="No" runat="server" Visible="false" />
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

