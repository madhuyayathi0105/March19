<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="DegreewiseResultAnalysis.aspx.cs" Inherits="DegreewiseResultAnalysis" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
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
    <form id="form1">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager><br /><center>
    <asp:Label ID="Label1" runat="server" Text="Degree wise Result Analysis" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Large" ForeColor="Green"></asp:Label></center>
<br />
            <center>
                <table style="width:900px; height:70px; background-color:#0CA6CA;">
                    <tr>
                        <td>
                           <asp:Label ID="Label2" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        </td>
                             <td>
                            <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox  ddlheight3"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        </td>
                             <td>
                           <asp:UpdatePanel ID="UP_batch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox txtheight2" ReadOnly="true"  Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_batch_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"/>
                                        <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
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
                                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox txtheight2" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_degree_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"/>
                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
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
                                    <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight2" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel multxtpanleheight">
                                        <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_dept_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua" />
                                        <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
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
                                            OnCheckedChanged="cb_sem_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"/>
                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
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
                                            OnCheckedChanged="cb_sec_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"/>
                                        <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
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
                                    <asp:DropDownList ID="ddl_test" runat="server" CssClass="textbox  ddlheight3 multxtpanleheight" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            
                        </td>
                        <td>
                            <asp:Button ID="btn_go" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="Go" OnClick="btn_go_Click" />
                        </td>
                        
                    </tr>
                </table>
                <br />
                <div>
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </div>
                <div id="divspread" runat="server" style="width: 950px; height: 390px; overflow: auto;"
                    class="table">
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                        BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                        background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder"
                        Width="950px">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
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
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
                </br>
            </center>
        </div>
    </div>
    </form>
</body>
</html>
</asp:Content>

