<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="External Internal Staff Neft Details.aspx.cs" Inherits="CoeMod_External_Internal_Staff_Neft_Details" %>


<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <span class="fontstyleheader" style="color: Green;">External Internal Staff Neft Details</span>
        </center>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="width: 1104px; font-family: Book Antiqua; font-weight: bold; height: auto">
                    <table class="maintablestyle" style="height: auto; margin-top: 10px; margin-bottom: 10px;
                        padding: 6px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College">
                                </asp:Label>  </td>
                            <td>
                                <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="width: 160px; " Height="" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblYear" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Exam Year"></asp:Label>  </td>
                            <td>
                                <asp:DropDownList ID="ddlYear1" runat="server" CssClass="textbox ddlheight" OnSelectedIndexChanged="ddlYear1_SelectedIndexChanged"
                                    Width="130px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblmonth" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Month"></asp:Label>  </td>
                            <td>
                                <asp:DropDownList ID="ddlMonth1" runat="server" CssClass="textbox ddlheight" OnSelectedIndexChanged="ddlMonth1_SelectedIndexChanged"
                                    Style="width: 133px;" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                         
                            <td>
                                <asp:Label ID="LblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>  </td>
                            <td>
                               <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox textbox1 txtheight1"
                                        ReadOnly="true" Width="125px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="width: 142px;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_degree_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="Panel2" Position="Bottom">
                                    </asp:PopupControlExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblDept" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Department"></asp:Label>  </td>
                            <td>
                               <asp:TextBox ID="txtdept" runat="server" CssClass="textbox textbox1 txtheight1" ReadOnly="true"
                                        Width="151px" Height="18px">--Select--</asp:TextBox>
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
                            </td>
                            
                            <td>
                                <asp:Label ID="lblsubtype" runat="server" Text="Subject Type" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>  </td>
                            <td>
                                       <asp:TextBox ID="txtsubtype" runat="server" CssClass="textbox textbox1 txtheight1" ReadOnly="true"
                                        Width="120px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Pnl1" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_subtype" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_subtype_Change" />
                                        <asp:CheckBoxList ID="cbl_subtype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subtype_ChekedChange">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsubtype"
                                        PopupControlID="Pnl1" Position="Bottom">
                                    </asp:PopupControlExtender>
                              
                            </td>
                            <td>
                                <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>  </td>
                            <td>
                                      <asp:TextBox ID="txtsubject" runat="server" CssClass="textbox textbox1 txtheight1" ReadOnly="true"
                                        Width="127px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Pnl2" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_subject" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_subject_Change" />
                                        <asp:CheckBoxList ID="cbl_subject" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subject_ChekedChange">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtsubject"
                                        PopupControlID="Pnl2" Position="Bottom">
                                    </asp:PopupControlExtender>
                               
                                <td>
                                <asp:Label ID="LblSubSubject" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Sub-Subject"></asp:Label>  </td>
                            <td>
                                      <asp:TextBox ID="txtsubsubject" runat="server" CssClass="textbox textbox1 txtheight1" ReadOnly="true"
                                        Width="127px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Pnl3" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                        height: 200px;">
                                        <asp:CheckBox ID="cb_subsubject" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_subsubject_Change" />
                                        <asp:CheckBoxList ID="cbl_subsubject" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subsubject_ChekedChange">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsubsubject"
                                        PopupControlID="Pnl3" Position="Bottom">
                                    </asp:PopupControlExtender>
                             
                            </td>
                        </tr>
                        <tr>
                        
                               <td>
                               <asp:Label ID="Label2" Text="From Date" runat="server"></asp:Label>
                            </td>
                            <td>
                                   <asp:TextBox ID="txt_fromdate" runat="server"  CssClass="txtcaps txtheight" 
                                                            ></asp:TextBox>
                                                       <asp:CalendarExtender ID="Cal_date" TargetControlID="txt_fromdate" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                                            Format="dd-MM-yyyy">
                                                       </asp:CalendarExtender>
                            </td>

                              <td>
                                <asp:Label ID="Label1" Text="To Date" runat="server"></asp:Label>
                            </td>
                            <td>
                                
                                   <asp:TextBox ID="txt_todate" runat="server"  CssClass="txtcaps txtheight" 
                                                       ></asp:TextBox>
                                                       <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                                            Format="dd-MM-yyyy">
                                                       </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpGoAdd" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" CssClass="textbox btn"
                                            Style="width: 80px;" />
                                       
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <div>
                     <div>
                        <center>
                            <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="940px" Style="margin-top: -0.1%;">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <br />
                    </div>
                    <center>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="940px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder" Visible="false" Width="930px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="928px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="7" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="Staff Code/Name">Staff Code/Name</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Subject Code/Name">Subject Code/Name</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Exam Date">Exam Date</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="College Name">College Name </asp:ListItem>
                                            <asp:ListItem Value="Canditate Registred">Canditate Registred</asp:ListItem>
                                            <asp:ListItem Value="Canditate Examined">Canditate Examined</asp:ListItem>
                                            <asp:ListItem Value="Role">Role</asp:ListItem>
                                            <asp:ListItem Value="Distance In Km">Distance In Km</asp:ListItem>
                                            <asp:ListItem Value="Rem R.s">Rem R.s</asp:ListItem>
                                            <asp:ListItem Value="T.A R.S">T.A R.S</asp:ListItem>
                                            <asp:ListItem Value="D.A R.S">D.A R.S</asp:ListItem>
                                            <asp:ListItem Value="Acquittance">Acquittance</asp:ListItem>
                                            <asp:ListItem Value="IFSC Code">IFSC Code</asp:ListItem>
                                            <asp:ListItem Value="Account Number">Account Number</asp:ListItem>
                                            <asp:ListItem Value="Bank Name">Bank Name</asp:ListItem>
                                             <asp:ListItem Value="Total Amount">Total Amount</asp:ListItem>
                                            <asp:ListItem Value="Sign">Sign</asp:ListItem>
                                          
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                        ExpandedImage="down.jpeg">
                    </asp:CollapsiblePanelExtender>

                    <br />
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

                        <br />
                        <br />

                          <div id="imgdiv2" runat="server" visible="false" style="height: 300em; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="Div1" runat="server" class="table" style="background-color: White; height: 120px;
                                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 400px;
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
                    </div>
                </div>
                  </ContentTemplate>
                   <Triggers>
                        <asp:PostBackTrigger ControlID="btn_Excel" />
                          <asp:PostBackTrigger ControlID="btnGo" />
                    </Triggers>
                </asp:UpdatePanel>
        </center>
          <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGoAdd">
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

