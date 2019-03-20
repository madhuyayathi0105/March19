<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterWizardMod/MasterWizard.master"
    CodeFile="attndmastersettings.aspx.cs" Inherits="attndmastersettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green;">Attendance Master Settings</span>
    </center>
    <br>
    <asp:UpdatePanel ID='UpdGridStudent' runat="server">
        <ContentTemplate>
            <center>
                <div class="maindivstylesize">
                
                    <center>
                    <table>
                    <tr>
                    <td>
                  
                        <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold;
                            margin-left: 169px; margin-top: 10px; margin-bottom: 10px; padding: 6px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlclg" runat="server" OnSelectedIndexChanged="ddlclg_OnSelectedIndexChanged"
                                        CssClass="textbox ddlstyle ddlheight3" Width="232px" Style="margin-left: 10px;"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td></tr>
                                </table>
                                  </td>
                  
                               <%-- </center></div>
                                 <div class="maindivstylesize">--%>
                                 <td>
                               <%--  <center>--%>
                                <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold;
                            margin-left: 107px; margin-top: 10px; margin-bottom: 10px; padding: 6px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblbatchyr" runat="server" Text="Batch" Style="margin-left: 1px;"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="upnlBatch" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtBatch" Visible="true" Width="72px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                    ReadOnly="true">-- Select --</asp:TextBox>
                                                <asp:Panel ID="pnlBatch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                    Width="140px">
                                                    <asp:CheckBox ID="chkBatch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                        AutoPostBack="True" OnCheckedChanged="chkBatch_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cblBatch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cblBatch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popExtBatch" runat="server" TargetControlID="txtBatch"
                                                    PopupControlID="pnlBatch" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Style="margin-left: 3px;"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="Upp3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_degree" runat="server" Width="67px" CssClass="textbox  txtheight2 commonHeaderFont"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="p2" runat="server" CssClass="multxtpanel" Width="150px" Height="180px">
                                                    <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_degree_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_degree"
                                                    PopupControlID="p2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lbldept" runat="server" Text="Department" style="margin-left: -9px;"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_branch" runat="server" Width="67px" style="margin-left: 0px;" CssClass="textbox  txtheight2 commonHeaderFont"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Width="457px" Height="180px">
                                                    <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_branch_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_branch"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sem" runat="server" Width="87px" Style="margin-left:0px;"
                                                CssClass="textbox  txtheight2 commonHeaderFont" ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_sem"
                                                PopupControlID="Panel11" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblsection" runat="server" Text="Section" Style="margin-left: 10px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtsection" runat="server" Width="87px" CssClass="textbox  txtheight2 commonHeaderFont"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cbsection" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbsection_checkedchange" />
                                                <asp:CheckBoxList ID="cblsection" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblsection_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtsection"
                                                PopupControlID="Panel2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                 <asp:UpdatePanel ID="upsave" runat="server">
                                                <ContentTemplate>
                                    <asp:Button ID="btngo" runat="server" OnClick="btngo_OnClick" CssClass="textbox textbox1" Width="50px" Height="25px" Font-Bold="true" Font-Names="Book Antiqua" Text="Go" />
                                    </ContentTemplate></asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        </td>
                        </tr>
                        </table>
                        </center>
                  <%--  </center>--%>
                </div>
            </center>
            <center>
            <table>
                <tr>
                    <td>
                      
                            <asp:GridView ID="gridview1" runat="server" Style="margin-bottom: 15px; margin-top: 15px; margin-left:-389px;
                                width: 420px;" Font-Names="Times New Roman" AutoGenerateColumns="false" BackColor="AliceBlue"
                                OnRowDataBound="gridview1_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Leave Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblleavetype" runat="server" Text='<%# Eval("leavetype")  %>' Style="text-align: left"></asp:Label>
                                            <asp:Label ID="lblleaveval" runat="server" Visible="false" Text='<%# Eval("lblleaveval")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("status")  %>' Visible="false"></asp:Label>
                                            <asp:DropDownList ID="ddlstatus" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Display in Report as">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldisplay" runat="server" Text='<%# Eval("displayinreport")  %>'
                                                Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtdisplay" runat="server" Text='<%# Eval("displayinreport")  %>'
                                                Style="text-align: center"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                         </td></tr></table>
                         
                         <table style="margin-left: 380px;margin-top: -487px;">
                         
                         <tr><td> <asp:Button ID="btnaddnewrow" runat="server" Font-Bold="true" Font-Names="Book Antiqua" Text="Add New Row"  style="width: 107px;height: 27px;" OnClick="btnaddnewrow_OnClick" CssClass="textbox textbox1" /></td></tr><tr><td>
                            <asp:GridView ID="gridview3" runat="server" Style="margin-bottom: 15px; 
                                margin-left: 2px; width: 303px;" Font-Names="Times New Roman" AutoGenerateColumns="false"
                                BackColor="AliceBlue" OnRowDataBound="gridview3_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Lock Hour">
                                        <ItemTemplate>
                                        <asp:Label ID="lbllockhour" runat="server" Visible="false" Text='<%# Eval("lockHour")  %>'></asp:Label>
                                            <asp:DropDownList ID="ddllockhours" runat="server" Width="70px">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From Hour">
                                        <ItemTemplate>
                                          <asp:Label ID="lblfromhour" runat="server" Visible="false" Text='<%# Eval("fromhour")  %>'></asp:Label>
                                            <asp:DropDownList ID="ddlfromhour" runat="server" Width="70px">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Hour">
                                        <ItemTemplate>
                                        <asp:Label ID="lbltohour" runat="server" Visible="false" Text='<%# Eval("tohour")  %>'></asp:Label>
                                            <asp:DropDownList ID="ddltohour" runat="server" Width="70px">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                      <%--  </fieldset>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbldegcd" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblbatchyr1" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblsemester1" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblsections1" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2px" align="center">
                    <asp:UpdatePanel ID="upsave1" runat="server"><ContentTemplate>
                        <asp:Button ID="btnsave" runat="server" Text="Save" style="margin-left: -216px; margin-top: -269px;" Font-Bold="true" Font-Names="Book Antiqua" Width="75px" Height="28px" OnClick="btnsave_onClick" CssClass="textbox textbox1" />
                        </ContentTemplate></asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            </center>
            <center>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <center>
        <asp:UpdatePanel ID="updatepanel9" runat="server">
            <ContentTemplate>
                <div id="divPopAlertbackvolume" runat="server" visible="false" style="height: 550em;
                    z-index: 100; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0%; left: 0%; font-family: Book Antiqua; font-weight: bold">
                    <center>
                        <div id="divPopAlertback" runat="server" class="table" style="background-color: White;
                            height: 504px; width: 60%; border: 5px solid #0CA6CA; margin-right: auto; margin-left: auto;
                            border-top: 25px solid #0CA6CA; left: 19%; right: 39%; top: 20%; padding: 5px;
                            z-index: 1000; position: fixed; border-radius: 10px; font-family: Book Antiqua;
                            font-weight: bold">
                            <center>
                                <asp:Label ID="lblnonbook" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                                    position: relative;" Text="Select Department" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
                            </center>
                            <div id="divgrd" runat="server" style="height: 400px; width: 807px; overflow: auto;
                                border: solid 1px black;">
                                <asp:GridView ID="gridview2" runat="server" Style="margin-bottom: 15px; margin-top: -1px;
                                    width: 788px; height: 400px" Font-Names="Times New Roman" AutoGenerateColumns="false"
                                    BackColor="AliceBlue">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsno" runat="server" Text='<%# Eval("SNo")  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Batch Year">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbatchyr" runat="server" Text='<%# Eval("Batch Year")  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Course">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcourse" runat="server" Text='<%# Eval("Course")  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldepartm" runat="server" Text='<%# Eval("Department")  %>'></asp:Label>
                                                   <asp:Label ID="lbldegc" runat="server" Text='<%# Eval("deptcode")  %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="250px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Semester">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsems" runat="server" Text='<%# Eval("Semester")  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Section">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsect" runat="server" Text='<%# Eval("Section")  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbselect" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                </asp:GridView>
                            </div>
                            <table>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnok" runat="server" OnClick="btnok_onClick" Text="Ok" />
                                        <asp:Button ID="btnexit" runat="server" OnClick="btnexit_onClick" Text="Exit" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="div4" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div5" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                            right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label7" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnaltok" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnaltok_Click"
                                                Text="Ok" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
     <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upsave">
            <ProgressTemplate>
                <div style="height: 40px; width: 150px;">
                    <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                    <br />
                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                        Processing Please Wait...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
     <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upsave1">
            <ProgressTemplate>
                <div style="height: 40px; width: 150px;">
                    <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                    <br />
                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                        Processing Please Wait...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
