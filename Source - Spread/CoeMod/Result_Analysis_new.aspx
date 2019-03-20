<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Result_Analysis_new.aspx.cs" Inherits="Result_Analysis_new" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>Result Analysis</title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function semesterTest() {
            var empty = "";
            var dep;
            var dep1;
            var semfrom = document.getElementById("<%=txt_semfrom.ClientID %>").value;
            var semto = document.getElementById("<%=txt_semto.ClientID %>").value;

            if (semfrom.trim() == "") {
                dep = document.getElementById("<%=txt_semfrom.ClientID %>");
                dep.style.borderColor = 'Red';
                empty = "E";

            }
            if (semto.trim() == "") {
                dep1 = document.getElementById("<%=txt_semto.ClientID %>");
                dep1.style.borderColor = "red";
                empty = "E";
            }

            if (semfrom.trim() != "" && semto.trim() != "") {

                if (parseInt(semfrom) > parseInt(semto)) {
                    alert("From semester shoud be less than To semester");
                    empty = "E";
                }
                if (parseInt(semfrom) > 10) {
                    alert("From semester shoud not exceed 10");
                    empty = "E";
                }
                if (parseInt(semto) > 10) {
                    alert("To semester shoud not exceed 10");
                    empty = "E";
                }
            }


            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
        function returnColor(y) {
            y.style.borderColor = "#c4c4c4";
        }
        function display() {
            document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
        }
    </script>
</head>
<body>
    <div>
        <form id="form2">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <br />
         <center>
         <asp:Label ID="Label1" runat="server" Text="Semester wise Result Analysis" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
 </center>
         <br />
            <center>
                <table style="width:700px; height:70px; background-color:#0CA6CA;">
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
                            <asp:DropDownList ID="ddl_batch" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_batch_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:DropDownList>
                      </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        </td>
                            <td>
                            <asp:DropDownList ID="ddl_degree" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_degree_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:DropDownList>
                        </td>
                        <td>
                             <asp:Label ID="Label5" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        </td>
                             <td>
                            <asp:DropDownList ID="ddl_dept" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_dept_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Subject Type" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px"></asp:Label>
                        </td>
                             <td>
                            <asp:UpdatePanel ID="UP_subtype" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_subtype" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                        onfocus="return myFunction1(this)" Font-Bold="True" Font-Names="Book Antiqua">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_subtype" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_subtype" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb__subtype_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua" />
                                        <asp:CheckBoxList ID="cbl_subtype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subtype_OnSelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_subtype" runat="server" TargetControlID="txt_subtype"
                                        PopupControlID="panel_subtype" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                             <asp:Label ID="Label7" runat="server" Text="Top Semester From" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="160px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_semfrom" runat="server" CssClass="textbox txtheight" MaxLength="2"
                                onfocus="return returnColor(this)" Font-Bold="True" Font-Names="Book Antiqua"></asp:TextBox>
                            <%--                        <span style="color: Red;">*</span>--%>
                            <asp:FilteredTextBoxExtender ID="filt_extenderfrom" runat="server" TargetControlID="txt_semfrom"
                                FilterType="custom,Numbers">
                            </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                             <asp:Label ID="Label8" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                            <asp:TextBox ID="txt_semto" runat="server" CssClass="textbox txtheight" MaxLength="2"
                                onfocus="return returnColor(this)" Font-Bold="True" Font-Names="Book Antiqua"></asp:TextBox>
                            <%--                        <span style="color: Red;">*</span>--%>
                            <asp:FilteredTextBoxExtender ID="filt_extenderto" runat="server" TargetControlID="txt_semto"
                                FilterType="custom,Numbers">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Button ID="btn_go" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Text="Go" OnClientClick="return semesterTest()"
                                OnClick="btn_go_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </div>
                <div id="divspread" runat="server" style="height: 390px; overflow: auto;" class="table">
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                        BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                        background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder">
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
        </form>
    </div>
</body>
</html>
</asp:Content>

