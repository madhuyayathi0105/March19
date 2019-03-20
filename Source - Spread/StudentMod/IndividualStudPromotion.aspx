<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master" AutoEventWireup="true" CodeFile="IndividualStudPromotion.aspx.cs" Inherits="IndividualStudPromotion" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <title>Individual Student Promotion</title>
    <%--<link rel="Shortcut Icon" href="~/college/Left_Logo.jpeg" />--%>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <script type="text/javascript" language="javascript">
     function display() {
         document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
     }
    </script>
    <asp:ScriptManager ID="scrptMgr" runat="server">
    </asp:ScriptManager>
        <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">Individual Student Promotion</span>
            </div>
     
        </center>
    </div>
    <center>
        <div class="maindivstyle" style="width: 970px; height: 500px;">
            <center>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_college" Width="200px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_stream" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_stream" runat="server" CssClass="textbox txtheight3 textbox1"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Style="width: 100px; height: 130px;">
                                        <asp:CheckBox ID="cb_stream" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_stream_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_stream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stream_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_stream"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_batch" runat="server" Width="70px" CssClass="textbox txtheight1 textbox1"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Style="width: 120px; height: 150px;">
                                        <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_batch_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_batch"
                                        PopupControlID="Panel2" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_degree" runat="server" Text="Degree"></asp:Label>
                        </td>
                        <td>
                            <%-- <asp:DropDownList ID="ddl_degree" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_degree_OnIndexChange">
                            </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight">Degree</asp:TextBox>
                                    <asp:Panel ID="pdegree" runat="server" Width="150px" Height="170px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_degree" runat="server" OnCheckedChanged="cb_degree_ChekedChange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="pdegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_branch" runat="server" Text="Branch"></asp:Label>
                        </td>
                        <td>
                            <%--  <asp:DropDownList ID="ddl_branch" Width="200px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_branch_OnIndexChange">
                            </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_branch" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1 txtheight">Branch</asp:TextBox>
                                    <asp:Panel ID="pbranch" runat="server" Width="250px" Height="200px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_branch" runat="server" OnCheckedChanged="cb_branch_ChekedChange"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_branch"
                                        PopupControlID="pbranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Sem" runat="server" Text="Semester"></asp:Label>
                        </td>
                        <td>
                            <%--  <asp:DropDownList ID="ddl_sem" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_sem_OnIndexChange">
                            </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UPpanel_sem" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox txtheight2" Width="100px"
                                        ReadOnly="true" placeholder="Semester" onfocus="return myFunction1(this)"></asp:TextBox>
                                    <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_sem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_sem_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupce_sem" runat="server" TargetControlID="txt_sem"
                                        PopupControlID="panel_sem" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_sec" runat="server" Text="Section"></asp:Label>
                        </td>
                        <td>
                            <%--  <asp:DropDownList ID="ddl_sec" Width="80px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_sec_OnIndexChange">
                            </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UpdatePanel8sec" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sec" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1 txtheight">Section</asp:TextBox>
                                    <asp:Panel ID="pnlsec" runat="server" Width="120px" Height="80px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_sec" runat="server" OnCheckedChanged="cb_sec_ChekedChange" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_sec"
                                        PopupControlID="pnlsec" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btn_go" Text="Go" OnClick="btn_go_Click" CssClass="textbox btn1 textbox1"
                                runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btn_Add" Text="Add" OnClick="btn_Add_Click" CssClass="textbox btn2 textbox1"
                                runat="server" />
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <asp:Label ID="lbl_errormsg" Visible="false" runat="server" Font-Bold="true" Text=""
                        ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_Total" Visible="false" Font-Bold="true" runat="server" Text=""
                        ForeColor="Red"></asp:Label>
                </div>
                <div>
                    <FarPoint:FpSpread ID="spreadStudList" runat="server" Visible="false" ShowHeaderSelection="false"
                        BorderWidth="0px" Width="900px" Style="overflow: auto; height: 300px; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionPolicy="Single">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label><br />
                    <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1 txtheight2" MaxLength="70"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="fteExcel" runat="server" TargetControlID="txt_excelname"
                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" _-">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" Text="Export To Excel"
                        Width="127px" CssClass="textbox btn2 textbox1" />
                    <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                        CssClass="textbox btn2 textbox1" Width="60px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
                <br />
            </center>
        </div>
    </center>
    <%--  ******popup window******--%>
    <center>
        <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight1 ">
            <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 460px;"
                OnClick="imagebtnpopclose_Click" />
            <br />
            <br />
            <div style="background-color: White; height: 560px; width: 950px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Individual Student Promotion</span></div>
                </center>
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege1" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_college1" Width="200px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_college1_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_stream1" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_strm1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_strm1_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_batch1" runat="server" Text="Batch"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_batch1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_batch1_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_degree1" runat="server" Text="Degree"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_degree1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_degree1_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_branch1" runat="server" Text="Branch"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_branch1" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_branch1" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_branch1_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_branch1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch1"
                                        PopupControlID="p4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_sem1" runat="server" Text="From Semester"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_sem1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_sem1_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_sec1" runat="server" Text="Section"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_sec1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_sec1_OnIndexChange">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RadioButton ID="rdo_sem" runat="server" Text="Semester" GroupName="sem" Checked="true" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdo_nonsem" runat="server" Text="Non-Semester" GroupName="sem" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_semdestination" runat="server" Text="To Semester"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_semdest" Enabled="false" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btn_go1" Text="Go" OnClick="btn_go1_Click" CssClass="textbox btn1 textbox1"
                                runat="server" />
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <asp:Label ID="lbl_errormsg1" Visible="false" Font-Bold="true" runat="server" Text=""
                        ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_Total1" Visible="false" Font-Bold="true" runat="server" Text=""
                        ForeColor="Red"></asp:Label>
                </div>
                <div>
                    <FarPoint:FpSpread ID="spreadStudAdd" runat="server" Visible="false" ShowHeaderSelection="false"
                        OnUpdateCommand="spreadStudAdd_Command" BorderWidth="0px" Width="850px" Style="overflow: auto;
                        height: 300px; border: 0px solid #999999; border-radius: 10px; background-color: White;
                        box-shadow: 0px 0px 8px #999999;">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <center>
                    <div>
                        <asp:Button ID="btn_Save" runat="server" CssClass="textbox btn2 textbox1" Text="Save"
                            OnClick="btn_Save_Click" />
                        <asp:Button ID="btn_exit" runat="server" CssClass="textbox btn2 textbox1" Text="Exit"
                            OnClick="imagebtnpopclose_Click" />
                    </div>
                </center>
            </div>
        </div>
    </center>
    <%-- Pop Alert--%>
    <center>
        <div id="imgAlert" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnlAlert" runat="server" class="table" style="background-color: White; height: 120px;
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
</asp:Content>

