<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="TransportAndHostelAllotmentReport.aspx.cs" Inherits="TransportAndHostelAllotmentReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.12.2.min.js"></script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            $(document).ready(function () {
                $("#<%=btnExcel.ClientID%>").click(function () {
                    var rbtrns = $("#<%=rbtransport.ClientID %>").attr("checked");
                    var rbhost = $("#<%=rb_hostel.ClientID %>").attr("checked");
                    var textval = "";
                    if (rbtrns)
                        textval = "Please Enter Your Transport Report Name";
                    else if (rbhost)
                        textval = "Please Enter Your Hostel Report Name";

                    var text = $("#<%=txtexcelname.ClientID%>").val();
                    if (text == "") {
                        $("#<%=lblvalidation1.ClientID %>").text(textval).show();
                        return false;
                    }
                });
            });
        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Transport And Hostel Allotment Report</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <table>
                        <tr>
                            <td>
                                <table class="maintablestyle">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true"
                                                Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_str1" runat="server" Text="Type"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlstream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged"
                                                CssClass="textbox  ddlheight" Style="width: 108px;">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Batch
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
                                            <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UP_degree" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
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
                                            <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Up_dept" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
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
                                        <td>
                                            <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Updp_sem" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_sem" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 124px;
                                                        height: 172px;">
                                                        <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="cb_sem_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_sem"
                                                        PopupControlID="panel_sem" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td colspan="4">
                                            <asp:RadioButton ID="rbtransport" runat="server" GroupName="tr" AutoPostBack="true"
                                                Checked="true" Text="Transport" OnCheckedChanged="rbtransport_Change" />
                                            <asp:RadioButton ID="rb_hostel" runat="server" GroupName="tr" Text="Hostel" AutoPostBack="true"
                                                OnCheckedChanged="rb_hostel_Change" />
                                            <asp:RadioButton ID="rbhostel" runat="server" Checked="true" GroupName="trs" Text="RoomWise"
                                                AutoPostBack="true" OnCheckedChanged="rbhostel_Change" />
                                            <asp:RadioButton ID="rbhstlname" runat="server" GroupName="trs" Text="HostelWise"
                                                AutoPostBack="true" OnCheckedChanged="rbhstlname_Change" />
                                        </td>
                                        <td colspan="2">
                                            <fieldset style="height: 18px;">
                                                <table>
                                                    <tr>
                                                        <td id="tdstname" runat="server" visible="false">
                                                            <span style="font-family: Book Antiqua;">Stage</span>
                                                        </td>
                                                        <td id="tdstvalue" runat="server" visible="false">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txtstage" runat="server" Style="height: 20px; width: 130px;" ReadOnly="true">--Select--</asp:TextBox>
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
                                                        <td id="tdrmname" runat="server" visible="false">
                                                            <asp:Label ID="lblhtlname" runat="server" Text="Room Type"></asp:Label>
                                                        </td>
                                                        <td id="tdrmvalue" runat="server" visible="false">
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txtroom" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="panel1" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                                        height: 400px;">
                                                                        <asp:CheckBox ID="cbroom" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                            OnCheckedChanged="cbroom_OnCheckedChanged" />
                                                                        <asp:CheckBoxList ID="cblroom" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblroom_OnSelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtroom"
                                                                        PopupControlID="panel1" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                        <td id="tdfmt" runat="server" visible="false" colspan="2">
                                            <asp:RadioButton ID="rbformat1" runat="server" Checked="true" GroupName="frm" AutoPostBack="true"
                                                Text="Formate1" OnCheckedChanged="rbformat1_Change" />
                                            <asp:RadioButton ID="rbformat2" runat="server" GroupName="frm" Text="Formate2" AutoPostBack="true"
                                                OnCheckedChanged="rbformat2_Change" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdrbs" runat="server" visible="false" colspan="2">
                                            <asp:RadioButton ID="rbheader" runat="server" Checked="true" GroupName="trg" AutoPostBack="true"
                                                Text="Header" OnCheckedChanged="rbheader_Change" />
                                            <asp:RadioButton ID="rbledger" runat="server" GroupName="trg" Text="Ledger" AutoPostBack="true"
                                                OnCheckedChanged="rbledger_Change" />
                                        </td>
                                        <td id="tdhtlhdr" runat="server" visible="false">
                                            <asp:Label ID="lblhdr" runat="server" Text="Header"></asp:Label>
                                        </td>
                                        <td id="tdhtlhdrval" runat="server" visible="false">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txthtlhdr" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel2" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                        height: 400px;">
                                                        <asp:CheckBox ID="cbhtlhdr" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cbhtlhdr_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cblhtlhdr" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblhtlhdr_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txthtlhdr"
                                                        PopupControlID="panel2" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td id="tdhtlldr" runat="server" visible="false">
                                            <asp:Label ID="Label1" runat="server" Text="Ledger"></asp:Label>
                                        </td>
                                        <td id="tdhtlldrval" runat="server" visible="false">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txthtlldr" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="panel3" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                        height: 400px;">
                                                        <asp:CheckBox ID="cbhtlldr" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cbhtlldr_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cblhtlldr" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblhtlldr_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txthtlldr"
                                                        PopupControlID="panel3" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td id="tdpdsdt" runat="server" visible="false" colspan="2">
                                            <asp:RadioButton ID="rbpaid" runat="server" Checked="true" GroupName="trgs" AutoPostBack="true"
                                                Text="Paid" OnCheckedChanged="rbpaid_Change" />
                                            <asp:RadioButton ID="rbunpaid" runat="server" GroupName="trgs" Text="Unpaid" AutoPostBack="true"
                                                OnCheckedChanged="rbunpaid_Change" />
                                            <asp:RadioButton ID="rbboth" runat="server" GroupName="trgs" Text="Both" AutoPostBack="true"
                                                OnCheckedChanged="rbboth_Change" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnGo" runat="server" CssClass="textbox btn2" Width="56px" Text="Go"
                                                OnClick="btnGo_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divcol" runat="server" visible="false">
                                    <center>
                                        <div>
                                            <center>
                                                <asp:Panel ID="pnlheading" runat="server" CssClass="cpHeader" Visible="true" Height="22px"
                                                    Width="146px" BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -853px;">
                                                    <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                </asp:Panel>
                                            </center>
                                        </div>
                                        <br />
                                        <div>
                                            <asp:Panel ID="pnlcolorder" runat="server" CssClass="maintablestyle" Width="930px">
                                                <div id="divcolorder" runat="server" style="height: 87px; width: 930px;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="cbcolorder" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbcolorder_Changed" />
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                                                    Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                                                    Visible="false" Width="111px">Remove  All</asp:LinkButton>
                                                                <%--OnClick="lb_Click"--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtcolorder" Visible="false" Width="867px" TextMode="MultiLine"
                                                                    CssClass="style1" AutoPostBack="true" runat="server" Enabled="false">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBoxList ID="cblcolorder" runat="server" Height="43px" Width="850px" Style="font-family: 'Book Antiqua';
                                                                    font-weight: 700; font-size: medium;" RepeatColumns="5" RepeatDirection="Horizontal">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </center>
                                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pnlcolorder"
                                        CollapseControlID="pnlheading" ExpandControlID="pnlheading" Collapsed="true"
                                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                                        ExpandedImage="~/images/down.jpeg">
                                    </asp:CollapsiblePanelExtender>
                                </div>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--farpoint--%>
                                <center>
                                    <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="true" BorderStyle="Solid"
                                        BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                        class="spreadborder">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </center>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--print--%>
                                <center>
                                    <div id="print" runat="server" visible="true">
                                        <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                        <%--Visible="false"--%>
                                        <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Text="Report Name"></asp:Label>
                                        <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
                                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                            InvalidChars="/\">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                                        <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                            CssClass="textbox textbox1" Width="60px" />
                                        <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
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
    </body>
    </html>
</asp:Content>
