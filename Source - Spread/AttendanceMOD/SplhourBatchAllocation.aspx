<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SplhourBatchAllocation.aspx.cs" Inherits="SplhourBatchAllocation" %>

<%@ Register Src="~/Usercontrols/Input_Events.ascx" TagName="collegedeatils" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .sty
        {
            font-size: medium;
            font-family: Book Antiqua;
        }
    </style>
    <script type="text/javascript">

        function check(cbControl) {

            if (cbControl.checked) {
                var e = cbControl.id;
                var array = e.split("_");
                var position = array[4].toString(); alert(position);
                var pos = array[3].toString(); alert(pos);
                var idpos = pos.replace('lab', '');
                alert(idpos);
                var strchkclst = document.getElementById('MainContent_gview1_Chklst_lab' + idpos + '_' + position + ''); 'MainContent_gview1_Chklst_lab1_0_0_0'
                'MainContent_gview1_Chklst_lab1_0_1_0'
                var chkclst = strchkclst.id;
                var iid = document.getElementById(chkclst.toString());
                for (var i = 0; i < iid.length; i++) {
                    var col = iid[i].toString();
                    iid[i].checked = true;
                    alert('end');
                }
            }
            else {
                //alert('unchacked');
            }
            var e = cbControl.id;
            //alert(e);

            //            var txtBox = document.getElementById('MainContent_divgview1_gview1_txt_lab1');
            //                var chkBoxList = document.getElementById('MainContent_gview1_Chklst_lab1');
            //            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            //            var cnt = 0;
            //            alert(txtBox);
            //            if (cbControl.checked == true) {
            //                for (var i = 0; i < chkBoxCount.length; i++) {
            //                    cnt++;
            //                    chkBoxCount[i].checked = true;
            //                    var cb = chkBoxCount[i].toString();
            //                    alert(cb);
            //                }
            //                //txtBox.value = "Hour(" + cnt + ")";
            //            }
            //            else {
            //                for (var i = 0; i < chkBoxCount.length; i++) {
            //                    chkBoxCount[i].checked = false;
            //                }
            //                //txtBox.value = "--Select--";
            //            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="Label2" runat="server" Text="Special Hour Batch Allocation" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
    </center>
    <br />
    <center>
        <asp:UpdatePanel ID="UPGo1" runat="server">
            <%--ID="Upanel1"--%>
            <ContentTemplate>
                <table class="maintablestyle" style="width: 700px; height: 40px; background-color: #0CA6CA;">
                    <tr>
                        <td>
                            <asp:Label ID="Label31" runat="server" Text="College" Font-Bold="true" Font-Names="MS Sans Serif"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                Font-Size="medium" Width="150px" OnSelectedIndexChanged="ddlcollege_change" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbatch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                Font-Size="medium" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_Change">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Degree" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldegree" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                Font-Size="medium" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_Change">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Department" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldepartment" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                Font-Size="medium" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddldepartment_Change">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Sem" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsem" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                Font-Size="medium" Width="50px" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_Change">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Sec" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsection" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                Font-Size="medium" Width="50px" AutoPostBack="true" OnSelectedIndexChanged="ddlsection_Change">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <%--<asp:UpdatePanel ID="UPGo1" runat="server">
                                <ContentTemplate>--%>
                            <asp:Button ID="btngo" runat="server" Text="GO" Font-Bold="true" OnClick="btngo_click"
                                Width="70px" Height="30px" />
                            <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="Upanl2" runat="server">
            <ContentTemplate>
                <table id="subtable" runat="server" visible="false">
                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Date" Font-Bold="true" Font-Names="MS Sans Serif"
                                Font-Size="Medium" ForeColor="Green"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlspecialdate" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="medium" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlspecialdate_Change">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Subject" Font-Bold="true" Font-Names="MS Sans Serif"
                                Font-Size="Medium" ForeColor="Green"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="medium" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="No of Batches" Font-Bold="true" Font-Names="MS Sans Serif"
                                Font-Size="Medium" ForeColor="Green"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_noofbatchs" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                MaxLength="2" Font-Size="medium" Width="50px">
                            </asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="fld" runat="server" FilterType="Numbers" TargetControlID="txt_noofbatchs">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UPGo" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnallocate" runat="server" Text="Go" Font-Bold="true" OnClick="btnallocate_click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="Upanel5" runat="server">
            <ContentTemplate>
                <br />
                <asp:Label ID="errorlable" runat="server" ForeColor="Red" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="medium" Visible="false"></asp:Label>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="Upanel4" runat="server">
            <ContentTemplate>
                <div id="mainvlaue" runat="server" visible="false">
                    <div style="float: left; width: 515px; height: 300px; overflow: auto;">
                        <asp:GridView ID="gview" runat="server" Style="height: 100px; width: 500px; overflow: auto;"
                            AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label runat="server" ID="lblsno" Text='<%#Container.DisplayIndex+1 %>' /></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:CheckBox runat="server" ID="chck" /></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Roll No">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblroll" Text='<%#Eval("Roll No") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reg No">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblreg" Text='<%#Eval("Reg No") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Student Name">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblstunme" Text='<%#Eval("Student Name") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Batch">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlbatch" Width="50" runat="server">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="lblbatch" Visible="false" Text='<%#Eval("Batch") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                Font-Names="Book Antiqua" />
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                            <RowStyle ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                        </asp:GridView>
                    </div>
                    <br />
                    <br />
                    <div style="float: left; margin-top: 300px; margin-left: -450px;">
                        <fieldset id="Fieldset2" runat="server" visible="false" style="width: 305px;">
                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged" />
                            <asp:Label ID="lblselect" runat="server" Text="Select"></asp:Label>
                            <asp:Label ID="lblfrom" runat="server" Text="From"></asp:Label>
                            <asp:TextBox ID="fromno" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="fromno"
                                FilterType="Numbers" />
                            <asp:Label ID="lblto" runat="server" Text="To"></asp:Label>
                            <asp:TextBox ID="tono" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tono"
                                FilterType="Numbers" />
                            <asp:Button ID="Button2" runat="server" Text="Go" Font-Bold="true" OnClick="selectgo_Click" />
                            <br />
                            <br />
                            <center>
                                <span style="font-family: Book Antiqua; font-size: medium;">LabBatch</span>
                                <asp:DropDownList ID="ddllabbatch" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                    Width="100px">
                                </asp:DropDownList>
                                <asp:Button ID="Btnsave" runat="server" Text="Save" Enabled="false" Visible="false"
                                    OnClick="Btnsave_Click" />
                                <asp:Button ID="Btndelete" runat="server" Text="Delete" Enabled="false" OnClick="Btndelete_Click" />
                            </center>
                        </fieldset>
                    </div>
                    <div id="divgview1" style="float: left; margin-left: 20px; width: 515px; height: 300px;
                        overflow: auto;">
                        <asp:GridView ID="gview1" runat="server" AutoGenerateColumns="false" Width="450"
                            OnRowDataBound="gview1_OnRowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblsno" runat="server" Text='<%#Container.DisplayIndex+1 %>' /></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbldate" runat="server" Text='<%#Eval("Day") %>' /></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Hour">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblhour" runat="server" Text='<%#Eval("Hour") %>' /></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lab1" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <div id="div1" style="position: relative;" runat="server">
                                            <asp:UpdatePanel ID="upnlPeriod1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_lab1" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                        Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="plab1" runat="server" CssClass="multxtpanel" Width="100px" Height="80px"
                                                        Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                        <asp:CheckBox ID="chk_lab1" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                            AutoPostBack="true" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" /><%--onclick="check(this);" --%>
                                                        <asp:CheckBoxList ID="Chklst_lab1" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popuplab1" runat="server" TargetControlID="txt_lab1"
                                                        PopupControlID="plab1" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lab2" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <div id="div2" style="position: relative;" runat="server">
                                            <asp:UpdatePanel ID="upnlPeriod2" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_lab2" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                        Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="plab2" runat="server" CssClass="multxtpanel" Width="90px" Height="80px"
                                                        Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                        <asp:CheckBox ID="chk_lab2" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="Chklst_lab2" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popuplab2" runat="server" TargetControlID="txt_lab2"
                                                        PopupControlID="plab2" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lab3" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <div id="div3" style="position: relative;" runat="server">
                                            <asp:UpdatePanel ID="upnlPeriod3" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_lab3" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                        Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="plab3" runat="server" CssClass="multxtpanel" Width="90px" Height="80px"
                                                        Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                        <asp:CheckBox ID="chk_lab3" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="Chklst_lab3" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popuplab3" runat="server" TargetControlID="txt_lab3"
                                                        PopupControlID="plab3" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lab4" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <div id="div4" style="position: relative;" runat="server">
                                            <asp:UpdatePanel ID="upnlPeriod4" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_lab4" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                        Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="plab4" runat="server" CssClass="multxtpanel" Width="90px" Height="80px"
                                                        Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                        <asp:CheckBox ID="chk_lab4" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="Chklst_lab4" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popuplab4" runat="server" TargetControlID="txt_lab4"
                                                        PopupControlID="plab4" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lab5" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <div id="div5" style="position: relative;" runat="server">
                                            <asp:UpdatePanel ID="upnlPeriod5" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_lab5" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                        Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="plab5" runat="server" CssClass="multxtpanel" Width="90px" Height="80px"
                                                        Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                        <asp:CheckBox ID="chk_lab5" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="Chklst_lab5" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popuplab5" runat="server" TargetControlID="txt_lab5"
                                                        PopupControlID="plab5" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lab6" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <div id="div6" style="position: relative;" runat="server">
                                            <asp:UpdatePanel ID="upnlPeriod6" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_lab6" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                        Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="plab6" runat="server" CssClass="multxtpanel" Width="90px" Height="80px"
                                                        Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                        <asp:CheckBox ID="chk_lab6" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="Chklst_lab6" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popuplab6" runat="server" TargetControlID="txt_lab6"
                                                        PopupControlID="plab6" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lab7" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <div id="div7" style="position: relative;" runat="server">
                                            <asp:UpdatePanel ID="upnlPeriod7" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_lab7" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                        Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="plab7" runat="server" CssClass="multxtpanel" Width="90px" Height="80px"
                                                        Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                        <asp:CheckBox ID="chk_lab7" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="Chklst_lab7" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popuplab7" runat="server" TargetControlID="txt_lab7"
                                                        PopupControlID="plab7" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lab8" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <div id="div8" style="position: relative;" runat="server">
                                            <asp:UpdatePanel ID="upnlPeriod8" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_lab8" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                        Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="plab8" runat="server" CssClass="multxtpanel" Width="90px" Height="80px"
                                                        Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                        <asp:CheckBox ID="chk_lab8" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="Chklst_lab8" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popuplab8" runat="server" TargetControlID="txt_lab8"
                                                        PopupControlID="plab8" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lab9" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <div id="div9" style="position: relative;" runat="server">
                                            <asp:UpdatePanel ID="upnlPeriod9" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_lab9" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                        Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="plab9" runat="server" CssClass="multxtpanel" Width="90px" Height="80px"
                                                        Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                        <asp:CheckBox ID="chk_lab9" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="Chklst_lab9" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popuplab9" runat="server" TargetControlID="txt_lab9"
                                                        PopupControlID="plab9" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lab10" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <div id="div10" style="position: relative;" runat="server">
                                            <asp:UpdatePanel ID="upnlPeriod10" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_lab10" CssClass="textbox  txtheight2 commonHeaderFont" Font-Names="Book Antiqua"
                                                        Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="plab10" runat="server" CssClass="multxtpanel" Width="90px" Height="80px"
                                                        Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                        <asp:CheckBox ID="chk_lab10" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="Chklst_lab10" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popuplab10" runat="server" TargetControlID="txt_lab10"
                                                        PopupControlID="plab10" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                Font-Names="Book Antiqua" />
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                            <RowStyle ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                        </asp:GridView>
                        <br />
                        <br />
                        <asp:LinkButton ID="lnkmultiple" runat="server" Visible="false" CausesValidation="False"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small" ForeColor="blue"
                            OnClick="LinkButton1_Click">To Add Multiple Batch</asp:LinkButton>
                        <div id="subdiv" runat="server" visible="false" style="border: 1px solid Red; width: 100px;
                            height: auto;">
                            <asp:CheckBoxList ID="cbbatchlist" runat="server">
                            </asp:CheckBoxList>
                            <br />
                            <center>
                                <asp:Button ID="btnsub" runat="server" Text="Ok" Font-Bold="true" OnClick="btnsub_Clcik" />
                            </center>
                        </div>
                        <center>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnbatchsave" runat="server" Visible="false" Text="Save" Font-Bold="true"
                                        OnClick="Batchallotsave_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UPGo">
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
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UPGo1">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
