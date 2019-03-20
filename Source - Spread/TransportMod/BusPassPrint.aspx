<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="BusPassPrint.aspx.cs" Inherits="BusPassPrint" %>

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
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=btnExcel.ClientID%>').click(function () {
                var excelName = $('#<%=txtexcelname.ClientID%>').val();
                if (excelName == null || excelName == "") {
                    $('#<%=lblvalidation1.ClientID%>').show();
                    return false;
                }
                else {
                    $('#<%=lblvalidation1.ClientID%>').hide();
                }
            });
            $('#<%=txtexcelname.ClientID %>').keypress(function () {
                $('#<%=lblvalidation1.ClientID %>').hide();
            });
        });

    </script>
   <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Bus Pass Print</span></div>
        </center>
    </div>
        </ContentTemplate>
     </asp:UpdatePanel>
        
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">

               
                <table class="maintablestyle">
                    <tr>
                         
                        <td colspan="5">
                        
                            <fieldset style="width: 200px; height: 15px;">

                            
                                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                    OnSelectedIndexChanged="rblType_Selected">
                                    <asp:ListItem Text="Student" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Staff"></asp:ListItem>
                                    <asp:ListItem Text="Both"></asp:ListItem>
                                </asp:RadioButtonList>

                                
                            </fieldset>
                            
                        </td>

                    </tr>
                    <tr>
                        <td>
                         <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 350px;
                                        height: 120px;">
                                        <asp:CheckBox ID="cbclg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cbclg_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cblclg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblclg_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtclg"
                                        PopupControlID="pnlclg" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                            <span style="font-family: Book Antiqua;">Route ID</span>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UP_route" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtroute" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_route" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                        height: 300px;">
                                        <asp:CheckBox ID="cbroute" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cbroute_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cblroute" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblroute_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_route" runat="server" TargetControlID="txtroute"
                                        PopupControlID="panel_route" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                <ContentTemplate>
                            <span style="font-family: Book Antiqua;">Vechile ID</span>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtvechile" runat="server" Style="height: 20px; width: 120px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_vechile" runat="server" CssClass="multxtpanel" Style="width: 163px;
                                        height: 300px;">
                                        <asp:CheckBox ID="cbvechile" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cbvechile_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cblvechile" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblvechile_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtvechile"
                                        PopupControlID="panel_vechile" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                            <span style="font-family: Book Antiqua;">Stage</span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtstage" runat="server" Style="height: 20px; width: 160px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_stage" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                        height: 400px;">
                                        <asp:CheckBox ID="cbstage" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cbstage_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cblstage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblstage_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtstage"
                                        PopupControlID="panel_stage" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr id="trstud" runat="server" visible="false">
                        <%--<td>
                            <asp:Label ID="lblstr" runat="server" Text="Type"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged"
                                CssClass="textbox  ddlheight" Style="width: 108px;">
                            </asp:DropDownList>
                        </td>--%>
                        <td>
                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                <ContentTemplate>
                            <span style="font-family: Book Antiqua;">Batch</span>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UP_batch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                        height: 200px;">
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
                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                <ContentTemplate>
                            <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UP_degree" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 120px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: 200px;">
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
                        <td>
                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                <ContentTemplate>
                            <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                             </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Up_dept" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 160px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                        height: 300px;">
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
                        
                    </tr>
                    <tr>
                        <td colspan="4" id="tdstaf" runat="server" visible="false">
                            <table>
                                <tr>
                                    <td>
                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                        <ContentTemplate>
                                        <span style="font-family: Book Antiqua;">Designation</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtdesg" runat="server" Style="height: 20px; width: 154px;" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_desg" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                    height: 300px;">
                                                    <asp:CheckBox ID="cbdesg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cbdesg_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cbldesg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbldesg_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtdesg"
                                                    PopupControlID="panel_desg" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                        <ContentTemplate>
                                        <span style="font-family: Book Antiqua;">Department</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtstafdept" runat="server" Style="height: 20px; width: 164px;"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_stafdept" runat="server" CssClass="multxtpanel" Style="width: 254px;
                                                    height: 172px;">
                                                    <asp:CheckBox ID="cbstafdept" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cbstafdept_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cblstafdept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblstafdept_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtstafdept"
                                                    PopupControlID="panel_stafdept" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                        <ContentTemplate>
                            <asp:Label ID="lblRptType" runat="server" Text="Report Type"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                        <td>
                        <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                        <ContentTemplate>
                            <asp:DropDownList ID="ddlRptType" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Width="102px">
                                <asp:ListItem Text="ID Printed"></asp:ListItem>
                                <asp:ListItem Text="ID Not Printed"></asp:ListItem>
                            </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        </td>

                            
                        <td>
                        
                        
                            <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                              
                            
                        </td>
                        <td id="tdPrint" runat="server" visible="false">
                            <asp:Button ID="btnPrint" runat="server" CssClass="textbox btn2" Style="background-color: Green;
                                color: White; font-weight: bold;" Text="Print" OnClick="btnPrint_Click" />

                              
                        <asp:CheckBox ID="cb_oddoreven" runat="server"  Width="60px" Text="odd/Even" Checked="true"
                                             />
                       
                        </td>
                         <td colspan="2" id="tdDate" runat="server" visible="false">
                        <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                        <ContentTemplate>
                            <div id="divdatewise" runat="server">
                                <table>
                                    <tr>
                                        
                                    
                                        <td>
                                            <asp:Label ID="lbl_fromdate" runat="server" Text="Valid From"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"
                                                onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_todate" runat="server" Text="Valid To" Style="margin-left: 4px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>

                                        <td>
                                            
                                    </td>
                                    </tr>
                                </table>
                            </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                        
                        

                        <%--added by saranya--%>
                        <td colspan="2" id="td_date" runat="server">
                         <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                        <ContentTemplate>
                            <div id="divFrmDateToDate" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="frmdate" runat="server" Text="From Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtfromdate" runat="server" Style="height: 20px; width: 75px;"
                                                onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="Txtfromdate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="todate" runat="server" Text="To Date" Style="margin-left: 4px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txttodate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="Txttodate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>


                       
                    </tr>
                </table>
                
                <br />
                <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                    <ContentTemplate>
                <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="true" BorderStyle="Solid"
                    BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                    background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder"
                    OnButtonCommand="spreadDet_ButtonCommand">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                    
                <center>
                
                    <div id="print" runat="server" visible="true">
                        <asp:Label ID="lblvalidation1" runat="server" Text="Please Enter Your Report Name"
                            Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red" Style="display: none;"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Visible="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Visible="true" Width="180px" onkeypress="display()"
                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" Visible="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                            Height="32px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmasterhed" runat="server" Visible="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                            Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                        <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                        &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                        <asp:TextBox ID="txt_freepass" runat="server" Visible="true" Width="20px" hight="10" BackColor="Green" Enabled="false"></asp:TextBox>
                         <asp:Label ID="lbl_freepass" runat="server" Visible="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Free Bus Pass"></asp:Label>
                    </div>

                     
                 </center>
            </div>
        </center>
    </div>
     
    


    

    
</asp:Content>
