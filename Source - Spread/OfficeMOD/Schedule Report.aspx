<%@ Page Title="Schedule Report" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Schedule Report.aspx.cs" Inherits="Schedule_Report" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Student Strength Status Report</title>
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <form id="form1">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <div>
            <span style="color: Green;" class="fontstyleheader">Schedule Report</span>
            <br />
            <br />
        </div>
    </center>
                <br />
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                        <div class="maindivstyle maindivstylesizes" id="sdiv">
                            <br />
                            <table class="maintablestyle" width="1200px">
                            
                                <tr>
                                   <td>
                                    <asp:Label ID="lbl_companyName" runat="server" Text="Company Name"></asp:Label>
                                </td>
                                <td>

                                
                                    <asp:TextBox ID="txtcompany" runat="server" CssClass="textbox textbox1 txtheight1"
                                        ReadOnly="true" Width="127px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                        height: 200px;">
                                        <asp:CheckBox ID="chkcom" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="chkcom_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblcom" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblcom_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtcompany"
                                        PopupControlID="Panel6" Position="Bottom">
                                    </asp:PopupControlExtender>
                                
                                    <%--<asp:DropDownList ID="drpcompany" AutoPostBack="true" runat="server" CssClass="textbox textbox1"
                                        Width="145px" Height="31px" OnSelectedIndexChanged="drpcompany_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                </td>
                                <td>
                                    <asp:Label ID="lblbatch" runat="server" Text="Batch Year"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox textbox1 txtheight1"
                                        ReadOnly="true" Width="127px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_batch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_batch"
                                        PopupControlID="Panel2" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblcourse" runat="server" Text="Course"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtcourse" runat="server" CssClass="textbox textbox1 txtheight1"
                                        ReadOnly="true" Width="127px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_course" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_course_ChekedChange" />
                                        <asp:CheckBoxList ID="cblcourse" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblcourse_ChekedChange">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtcourse"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbldegree" runat="server" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdegree" runat="server" CssClass="textbox textbox1 txtheight1"
                                        ReadOnly="true" Width="127px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_degree_ChekedChange" />
                                        <asp:CheckBoxList ID="cbldegree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldegree_ChekedChange">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtdegree"
                                        PopupControlID="Panel4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbldepartment" runat="server" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdept" runat="server" CssClass="textbox textbox1 txtheight1" ReadOnly="true"
                                        Width="127px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_departemt" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cbdepartment_Change" />
                                        <asp:CheckBoxList ID="cbldepartment" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldepartment_ChekedChange">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtdept"
                                        PopupControlID="Panel5" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <tr>
                                </td>
                            </tr>
                          <tr>

                           <td>
                                    <asp:Label ID="lbldes" runat="server" Text="Description"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdes" runat="server" CssClass="textbox textbox1 txtheight1"
                                        ReadOnly="true" Width="127px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                        height: 200px;">
                                        <asp:CheckBox ID="chkdes" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="chkdes_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbldes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldes_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdes"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>


                            <td>
                             <asp:CheckBox ID="chkdate" runat="server" Text="From Date" AutoPostBack="True"
                                            OnCheckedChanged="chkdate_CheckedChanged" />
                              
                                
                            </td>
                            <td>
                                <%--<asp:DropDownList ID="ddldate" runat="server" Height="28px" Width="109px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>--%>
                                   <asp:TextBox ID="txt_fromdate" runat="server" CssClass="txtcaps txtheight" Enabled="false" 
                                                            ></asp:TextBox>
                                                       <asp:CalendarExtender ID="Cal_date" TargetControlID="txt_fromdate" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                                            Format="d/MM/yyyy">
                                                       </asp:CalendarExtender>
                            </td>

                              <td>
                                <asp:Label ID="Label1" Text="To Date" runat="server"></asp:Label>
                            </td>
                            <td>
                                <%--<asp:DropDownList ID="ddldate" runat="server" Height="28px" Width="109px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>--%>
                                   <asp:TextBox ID="txt_todate" runat="server" CssClass="txtcaps txtheight"   Enabled="false" 
                                                       ></asp:TextBox>
                                                       <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                                            Format="d/MM/yyyy">
                                                       </asp:CalendarExtender>
                            </td>
                           
                                  
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" /></ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            </tr>
                        </table>

                        <br />
                        <br />
                        <center>
                            <div class="GridDock" id="divgrid">
                                <asp:GridView ID="gview" runat="server" ShowHeader="false" Width="1000">
                                    <%--onchange="QuantityChange1(this)"--%>
                                    <Columns>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                    <RowStyle ForeColor="#333333" />                                    
                                </asp:GridView>
                            </div>
                        </center>
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
                        </div>

                          
                            <center>
                                <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                                </asp:Label></center>
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
                                    <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                                </center>
                            </div>
                        
                        </ContentTemplate>   <Triggers>
                        <asp:PostBackTrigger ControlID="btn_Excel" />
                    </Triggers></asp:UpdatePanel>
                    </form>
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
                    
                    
                    </asp:Content>