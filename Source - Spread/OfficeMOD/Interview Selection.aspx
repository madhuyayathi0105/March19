<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Interview Selection.aspx.cs" Inherits="Interview_Selection" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function checkmain() {
            var grid = document.getElementById('<%=gview.ClientID%>');
            for (var i = 0; i < grid.rows.length; i++) {
                document.getElementById('<%=gview.ClientID%>').rows[i].cells[0].style.display = "none";


            }
        }
        //        function QuantityChange1(objRef, colIndex) {
        //            var row = document.getElementById('<%=gview.ClientID%>').parentNode.parentNode;
        //            alert(row);
        //            var rowIndex = row.rowIndex - 1;
        //            var cnt = document.getElementById('<%=gview.ClientID%>').rows[0].cells.length;
        //            var rows = 0;
        //            var actrow = 10;
        //            var Userid = row.cells[0].childNodes[1].value;
        //            alert(Userid);
        //            var a=0;
        //            for (var m = 10; m < cnt + 1; m++) {
        //                alert(m);
        //                var val = "";
        //                alert(rows);
        //              
        //               
        //                   
        //                    a = m - actrow;
        //                     
        //                
        //                alert(a);
        //                if (a < 10) {


        //                    val = "0" + a + "";
        //                }
        //                else {
        //                    val = Convert.ToString(m);
        //                }
        //                var chkname = "ctl" + val + "";

        //                var stud_rollno = document.getElementById('MainContent_gview_'+ chkname+'_2');
        //                if (stud_rollno.checked == true) {
        //                    for (var i = 3; i < document.getElementById('<%=gview.ClientID%>').rows.length; i++) {
        //                        var ddl_select = document.getElementById('MainContent_gview_' + chkname + '_' + i.toString());
        //                        ddl_select.checked = true;

        //                    }
        //                }
        //                else if (stud_rollno.checked == false) {
        //                    for (var i = 3; i < document.getElementById('<%=gview.ClientID%>').rows.length; i++) {
        //                        var ddl_select = document.getElementById('MainContent_gview_' + chkname + '_' + i.toString());
        //                        ddl_select.checked = false;

        //                    }
        //                }
        //               

        //                }
        //            



        //        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <form id="form1">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <div>
            <span style="color: Green;" class="fontstyleheader">Interview Selection</span>
            <br />
            <br />
        </div>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <div class="maindivstyle" style="height: auto; width: 1094px;">
                    <br />
                    <div>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_companyName" runat="server" Text="Company Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpcompany" AutoPostBack="true" runat="server" CssClass="textbox textbox1"
                                        Width="145px" Height="31px" OnSelectedIndexChanged="drpcompany_SelectedIndexChanged">
                                    </asp:DropDownList>
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
                           
                            <td>
                                <asp:Label ID="lblindate" Text="Interview Date" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldate" runat="server" Height="28px" Width="109px" Font-Bold="True" AutoPostBack="true"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddldate_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>

                             <td>
                                <asp:Label ID="lblround" runat="server" Text="No Of Round"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtround" runat="server" CssClass="textbox textbox1 txtheight1"
                                    ReadOnly="true" Width="127px" Height="18px">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                    height: 200px;">
                                    <asp:CheckBox ID="Cbround" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_round_CheckedChanged" />
                                    <asp:CheckBoxList ID="Cblround" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_round_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtround"
                                    PopupControlID="Panel1" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td colspan="2"> <asp:CheckBox ID="Chksms" runat="server" Text=" Send SMS" AutoPostBack="True" /></td>
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
                            <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                <asp:GridView ID="gview" runat="server" ShowHeader="false" Width="1000" OnSelectedIndexChanged="gview_SelectedIndexChanged" OnRowCreated="OnRowCreated">
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
                        <center>
                            <td>
                              <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                <asp:Button ID="btnsave" runat="server" CssClass="textbox btn1" Text="Save" Width="83px"
                                    Height="33px" BackColor="#00FFD2" OnClick="btnsave_Click" Visible="false" /></ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
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

                          <center>
                            <div id="imgdiv3" runat="server" visible="false" style="height: 0px; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                        border-radius: 10px;">
                                        <center>
                                            <table style="height: 100px; width: 100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="Label1" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:Button ID="Button1" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                width: 65px;" OnClick="Button1_Click" Text="ok" runat="server" />
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
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
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

         <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
