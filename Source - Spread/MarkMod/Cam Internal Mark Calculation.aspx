<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Cam Internal Mark Calculation.aspx.cs" Inherits="Cam_Internal_Mark_Calculation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function validate(e) {

            var mk = 1;
            var max = 0;
            max = parseInt(document.getElementById("<%=txtMaxAttndValue.ClientID %>").value);
            if (e.value < mk || e.value > max) {
                // alert(max);
                e.value = "";

                alert("Please enter the mark less than or equal to " + max + "");
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <center>
                    <asp:Label ID="Label5" runat="server" class="fontstyleheader" Font-Names="Book Antiqua"
                        ForeColor="Green" Text="Cam Internal Mark Calculation"></asp:Label>
                    <br />
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:CheckBox ID="ckhdegreewise" Text="Degree Wise" runat="server" Font-Bold="True"
                                    Font-Size="Medium" Font-Names="Book Antiqua" AutoPostBack="true" OnCheckedChanged="chkdegreewise_Checkedchange"
                                    Width="133px" />
                            </td>
                            <td>
                                <asp:Label ID="lbledulevel" runat="server" Text="Education Level" Font-Bold="True"
                                    ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddledulevel" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddledulevel_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <div id="setwidth" runat="server">
                                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                    Width="80" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                    Width="100" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsec" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblSuType" runat="server" Text="Type"></asp:Label>
                                <asp:TextBox ID="txtSubtype" runat="server" CssClass="textbox txtheight2" ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="300px" Width="300px">
                                    <asp:CheckBox ID="chkSubtype" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="CheckBox1_checkedchange" />
                                    <asp:CheckBoxList ID="cblSubtype" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                        AutoPostBack="true" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtSubtype"
                                    PopupControlID="Panel3" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblSubject" runat="server" Text="Subject"></asp:Label>
                                <asp:TextBox ID="txtSubject" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Width="550px" Height="350px">
                                    <asp:CheckBox ID="cbSubjet" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="cbSubjet_checkedchange" />
                                    <asp:CheckBoxList ID="cblSubject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblSubject_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtSubject"
                                    PopupControlID="Panel5" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblexamyear" runat="server" Text="Exam Year" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlexamyear" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblexammonth" runat="server" Text="Exam Month" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlexammonth" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua">
                                    <asp:ListItem Text="Jan" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                    <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upgo" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btngo" runat="server" Text="Go" Width="50px" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnClick="btngo_Click" /></ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btncolor1" BackColor="White" Text="" runat="server" Enabled="false" />
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Criteria Not Entered" Visible="true"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="Button1" BackColor="Aquamarine" Text="" runat="server" Enabled="false" />
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Criteria Entered" Visible="true"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="Button2" BackColor="AntiqueWhite" Text="" runat="server" Enabled="false" />
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Calculation Entered" Visible="true"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="chkRound100" runat="server" Text="Round Off" Font-Names="Book Antiqua"
                                            OnCheckedChanged="chkRound100_checkedchange" AutoPostBack="true" Font-Size="Small" /></ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="Txtround" runat="server" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true" Visible="false">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="150px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_round" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_round_checkedchange" />
                                            <asp:CheckBoxList ID="chkroundoff" runat="server" Font-Names="Book Antiqua" Font-Size="Small">
                                                <asp:ListItem Value="0">Internal Marks Min</asp:ListItem>
                                                <asp:ListItem Value="1">Internal Marks Max</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="Txtround"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="upgrd" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridView1" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" OnRowDataBound="gridview1_OnRowDataBound"
                                BackColor="AliceBlue">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Degree">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldegree" runat="server" Text='<%# Eval("degree") %>'></asp:Label>
                                            <asp:Label ID="lblsection" runat="server" Text='<%# Eval("Sections") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsubject" runat="server" Text='<%# Eval("subject_name") %>'></asp:Label>
                                            <asp:Label ID="lblsubno" runat="server" Text='<%# Eval("subject_no") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblsyllcode" runat="server" Text='<%# Eval("syll_code") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_OnCheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbselect" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Update">
                                        <ItemTemplate>
                                            <asp:Button ID="btnupdate" runat="server" Text="Update" OnClick="btnupdate_OnClick" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Calculate">
                                        <ItemTemplate>
                                            <asp:Button ID="btncalculate" runat="server" Text="Calculate" OnClick="btncalculate_OnClick" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View">
                                        <ItemTemplate>
                                            <asp:Button ID="btnview" runat="server" Text="View" OnClick="btnview_OnClick" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClick="btndelete_OnClick" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblcriteria" runat="server" Text="No of Cam Criteria" Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtcriteria" runat="server" Width="50px" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Small" MaxLength="2"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtcriteria"
                                    FilterType="Numbers" />
                            </td>
                            <td>
                                <asp:Label ID="lblcalulate" runat="server" Text="No of Calculate Criteria " Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtcalculate" runat="server" Width="50px" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Small" MaxLength="2"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtcalculate"
                                    FilterType="Numbers" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkattendance" runat="server" Text="Attendance" Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Small" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkSubSub" runat="server" Text="Based On SubSubject" Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Small" />
                            </td>
                            <td>
                                <asp:Button ID="btncriteria" runat="server" Text="Ok" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btncriteria_Click" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkbasedSettings" runat="server" Visible="false" Text="Attendance Based Setting"
                                    Font-Names="Book Antiqua" Font-Size="Small" AutoPostBack="true" OnCheckedChanged="chkbasedSettings_CheckedChanged" />
                            </td>
                            <td>
                                <asp:Button ID="btnAttenSetting" runat="server" Text="Setting" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnAttenSetting_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Panel ID="panel4" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                        BorderWidth="2px" Style="left: 170px; top: 820px; position: absolute;" Height="500px"
                        Width="700px">
                        <table style="text-align: left">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkattsem" runat="server" Text="Sem Date" Font-Names="Book Antiqua"
                                        Font-Size="Small" AutoPostBack="true" OnCheckedChanged="chkattsem_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:Label ID="lblfromdate" runat="server" Text="From Date" Font-Names="Book Antiqua"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfromdate" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                        Width="85px" AutoPostBack="true" OnTextChanged="txtfromdate_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtfromdate" runat="server"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Names="Book Antiqua"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txttodate" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                        Width="85px" AutoPostBack="true" OnTextChanged="txttodate_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txttodate" runat="server"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbvoverall" runat="server" GroupName="Attendance" Text="Over All"
                                        Font-Names="Book Antiqua" Font-Size="Small" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbvsubjectwise" runat="server" Text="Subject Wise" Font-Names="Book Antiqua"
                                        Font-Size="Small" GroupName="Attendance" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbvattmaxmark" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                        GroupName="Percent" Text="MaxMark" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbvattpercentage" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                        GroupName="Percent" Text="Percentage" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <%-- <tr>   <td>
                            <asp:CheckBox ID="chkbasedSettings" runat="server" Text="Setting Based" Font-Names="Book Antiqua"
                                Font-Size="Small" AutoPostBack="true" OnCheckedChanged="chkbasedSettings_CheckedChanged" />
                        </td></tr>--%>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                    <asp:Button ID="btnclose" runat="server" Text="Close" OnClick="btnclose_Click" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Label ID="errror" runat="server" Text="" Visible="false" Font-Names="Book Antiqua"
                        ForeColor="Red" Font-Size="Medium"></asp:Label>
                    <table>
                        <tr>
                            <td>
                                <asp:GridView ID="GridView2" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                    width: auto;" HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua"
                                    ShowHeaderWhenEmpty="true" OnRowDataBound="gridview2_OnRowDataBound" BackColor="AliceBlue">
                                    <%--<Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                </asp:TemplateField>
            </Columns>--%>
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <div id="divPrint" runat="server" visible="false">
                        <asp:Label ID="lblreportname" runat="server" Text="Report Name" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                        <asp:TextBox ID="txtreport" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtreport"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnxl_Click" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </center>
                <center>
                    <div id="popaddnewF2" runat="server" class="popupstyle popupheight1" visible="false">
                        <asp:ImageButton ID="btn_popupclose2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; left: 87%; top: 0%; right: 0px;"
                            OnClick="btn_popupclose2_Click" />
                        <div style="background-color: White; width: 80%; height: 600px; overflow: auto; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <center>
                                <span class="fontstyleheader" style="color: #008000;">Attendance Settings</span>
                            </center>
                            <table class="maintablestyle" style="height: auto; width: auto;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Style="height: 18px; width: 10px"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="182px" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"
                                            AutoPostBack="True" Style="">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBatch1" runat="server" Text="Batch" CssClass="commonHeaderFont"
                                            AssociatedControlID="txtBatch"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="upnlBatch" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtBatch" Visible="true" Width="67px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
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
                                        <asp:Label ID="lblDegree1" runat="server" CssClass="commonHeaderFont" Text="Degree"
                                            AssociatedControlID="txtDegree"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="upnlDegree" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtDegree" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                        ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="pnlDegree" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                        Width="140px">
                                                        <asp:CheckBox ID="chkDegree" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                            AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblDegree" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                                        PopupControlID="pnlDegree" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBranch1" runat="server" CssClass="commonHeaderFont" Text="Branch"
                                            AssociatedControlID="txtBranch"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="upnlBranch" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtBranch" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                        ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="pnlBranch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                        Width="280px">
                                                        <asp:CheckBox ID="chkBranch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                            AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblBranch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                                        PopupControlID="pnlBranch" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSem1" runat="server" CssClass="commonHeaderFont" Text="Sem" AssociatedControlID="txtSem"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtSem" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                        ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="Panel1" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                        Width="140px">
                                                        <asp:CheckBox ID="chksem" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                            AutoPostBack="True" OnCheckedChanged="chksem_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblSem" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="cblSem_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtSem"
                                                        PopupControlID="Panel1" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFromDate1" runat="server" Text="From Date" Font-Names="Book Antiqua"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate1" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Width="85px" AutoPostBack="true" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate1" runat="server"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblToDate1" runat="server" Text="To Date" Font-Names="Book Antiqua"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate1" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Width="85px" AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate1" runat="server"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNoRows" runat="server" Text="No.of.Rows" Font-Names="Book Antiqua"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNoRows" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Width="85px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMaxAttndValues" runat="server" Text="Max. Attnd. Values" Font-Names="Book Antiqua"
                                            Font-Size="Small"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMaxAttndValue" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Width="85px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSettingGo" runat="server" Text="Go" OnClick="btnSettingGo_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnview1" runat="server" Text="View" OnClick="btnSettingView1_Click"
                                            Font- Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Label ID="lblError" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                ForeColor="Red" Visible="false"></asp:Label>
                            <br />
                            <center>
                                <asp:GridView ID="GridView3" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                    width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" BackColor="AliceBlue">
                                    <Columns>
                                        <asp:TemplateField HeaderText="From Range">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtfrmrange" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Range">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txttorange" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Attendance Mark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtatndrpt" runat="server" onkeyup="return validate(this)"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                </asp:GridView>
                            </center>
                            <center>
                                <asp:GridView ID="GridView4" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                    width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" BackColor="AliceBlue">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text='<%#Eval("Sno4") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="From Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfrmdat" runat="server" Text='<%#Eval("fromdate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltodate" runat="server" Text='<%#Eval("todate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Frange">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfrang" runat="server" Text='<%#Eval("Frange") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Trange">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltrang" runat="server" Text='<%#Eval("Trange") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="atndmrk">
                                            <ItemTemplate>
                                                <asp:Label ID="lblatndmrk" runat="server" Text='<%#Eval("atndmark") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                </asp:GridView>
                            </center>
                            <asp:Button ID="btnsaveSettings" runat="server" Text="Save" OnClick="btnsaveSettings_Click"
                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                            <br />
                            <asp:Label ID="lblSave" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                ForeColor="Red" Visible="false"></asp:Label>
                            <br />
                        </div>
                    </div>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
            </Triggers>
        </asp:UpdatePanel>
        <center>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upgo">
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
        <center>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upgrd">
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
    </body>
</asp:Content>
