<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DegreewiseResultAnalysis.aspx.cs" Inherits="DegreewiseResultAnalysis" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript">
    function printTTOutput() {
        var panel = document.getElementById("<%=printdiv.ClientID %>");
        var printWindow = window.open('', '', 'height=816,width=980');
        printWindow.document.write('<html><head>');
        printWindow.document.write('</head><body >');
        printWindow.document.write(panel.innerHTML);
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        setTimeout(function () {
            printWindow.print();
        }, 500);
        return false;
    }
    </script>
    <style tyle="text/css">
        .printclass
        {
            display: none;
        }
        .marginSet
        {
            margin: 0px;
            padding: 0px;
        }
        .headerDisp
        {
            font-size: 25px;
            font-weight: bold;
        }
        .headerDisp1
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        @media print
        {
            #printdiv
            {
                display: block;
            }
            .printclass
            {
                display: block;
                font-family: Book Antiqua;
            }
            .noprint
            {
                display: none;
            }
        }
        @media screen,print
        {
        
        }
        @page
        {
            size: A4;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title>Degree wise Result Analysis</title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <script>
            function display() {
                document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
            }
        </script>
    </head>
    <body>
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <asp:Label ID="Label1" runat="server" Text="Degree wise Result Analysis" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
        <br />
        <center>
            <table class="maintablestyle" style="width: 900px; height: 70px;">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox  ddlheight3"
                            AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexChanged"
                            Font-Bold="True" Font-Names="Book Antiqua">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UP_batch" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                    Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_batch_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua" />
                                    <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                    PopupControlID="panel_batch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UP_degree" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                    Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_degree_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua" />
                                    <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                    PopupControlID="panel_degree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="Up_dept" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                    Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel multxtpanleheight">
                                    <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_dept_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua" />
                                    <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua">
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
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UP_sem" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                    onfocus="return myFunction1(this)" Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cb_sem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_sem_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua" />
                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pce_sem" runat="server" TargetControlID="txt_sem" PopupControlID="panel_sem"
                                    Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="Section" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UP_sec" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_sec" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                    onfocus="return myFunction1(this)" Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                <asp:Panel ID="panel_sec" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cb_sec" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_sec_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua" />
                                    <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec_OnSelectedIndexChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pce_sec" runat="server" TargetControlID="txt_sec" PopupControlID="panel_sec"
                                    Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text="Test Name" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UP_test" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_test" runat="server" CssClass="textbox  ddlheight3 multxtpanleheight"
                                    Font-Bold="True" Font-Names="Book Antiqua">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                        <asp:Button ID="btn_go" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Text="Go" OnClick="btn_go_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            <br />
            <div id="divspread" runat="server" style="width: 950px; height: 390px; overflow: auto;"
                class="table">

            <center>
        <div id="printdiv" runat="server">
            <table class="printclass" style="width: 98%; height: auto; margin: 0px; padding: 0px;">
                <tr>
                    <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                        <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                            Width="100px" Height="100px" />
                    </td>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spCollegeName" class="headerDisp" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spAddr" class="headerDisp1" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spReportName" class="headerDisp1" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <td class="marginSet" colspan="3" align="center">
                        <span id="spDegreeName" class="headerDisp1" runat="server"></span>
                    </td>
                    <td class="marginSet" colspan="3" align="right">
                        <span id="spSem" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td class="marginSet" colspan="3" align="left">
                        <span id="spProgremme" class="headerDisp1" runat="server"></span>
                    </td>
                    <td class="marginSet" colspan="3" align="right">
                        <span id="spSection" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                        HeaderStyle-BackColor="#0CA6CA" BorderColor="Black"  Width="950px" >
                                    </asp:GridView>

            <table class="printclass" style="width: 98%; height: auto; margin-top: 100px; padding: 0px;">
                <tr>
                    <td>
                        
                    </td>
                    <td style="text-align: right">
                        
                    </td>
                </tr>
            </table>
        </div>
    </center>
                
                
                                    
             
            </div>
            <br />

             
            <div id="rptprint" runat="server" visible="false">
                <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false" onkeypress="display()"></asp:Label>
                <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                    CssClass="textbox textbox1"></asp:TextBox>
                <asp:Button ID="btn_excel" runat="server" Text="Export To Excel" Width="127px" CssClass="textbox btn2"
                    OnClick="btn_excel_Click" />
                <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                    OnClick="btn_printmaster_Click" Width="60px" />
                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />

                <button id="btnPrint" runat="server" visible="true" height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
            </div>
            <br />
        </center>
        </form>
         </ContentTemplate>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="btn_excel" />
                                </Triggers>
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
    </body>
    </html>
</asp:Content>
