<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Academic_Calendar.aspx.cs" Inherits="AttendanceMOD_Academic_Calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script type="text/javascript">
        function frelig() {

            document.getElementById('<%=btnreasonadd.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnreasondelete.ClientID%>').style.display = 'block';
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="Label2" runat="server" Text="Inspro Calendar" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
    </center>
    <br />
    <center>
       <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
        <table class="maintablestyle" style="width: 700px; height: 40px; background-color: #0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="Lblclg" runat="server" Text="College" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlclg" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="200px" CausesValidation="True"
                        OnSelectedIndexChanged="ddlclg_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td colspan=2>
                    <asp:Label ID="lblBatch" runat="server" Text="Academic Year" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" CausesValidation="True"
                        OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblsem" runat="server" Text="Semester" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsem" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" CausesValidation="True"
                        OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                        <asp:ListItem>Odd Sem</asp:ListItem>
                        <asp:ListItem>Even Sem</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="lbsem" runat="server" Text="" Font-Bold="True" Visible="false" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                           <asp:Label ID="Lbl_sem" runat="server" Text="" Font-Bold="True" Visible="false" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                    <asp:Label ID="Lbbatch" runat="server" Text="" Font-Bold="True" Visible="false" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                        <asp:Label ID="Lbl_batch" runat="server" Text="" Font-Bold="True" Visible="false" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua">
                    </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox txtheight3 textbox1" Font-Bold="True"
                        ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                    <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                        <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                            OnCheckedChanged="cb_degree_checkedchange" />
                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_degree"
                        PopupControlID="p1" Position="Bottom">
                    </asp:PopupControlExtender>
                    <%--                      </ContentTemplate>
                    <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                        Font-Bold="True" Font-Names="Book Antiqua" Width="100px" Font-Size="Medium" CausesValidation="True"
                        OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                    </asp:DropDownList>--%>
                </td>
                <td>
                    <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox txtheight3 textbox1" Font-Bold="True"
                        ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                    <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                        <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="True"
                            OnCheckedChanged="cb_branch_checkedchange" />
                        <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_branch"
                        PopupControlID="Panel1" Position="Bottom">
                    </asp:PopupControlExtender>
                    <%--<asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" Width="245px">
                    </asp:DropDownList>--%>
                </td></tr>
                <tr>
                <td colspan=2>
                    <asp:Label ID="lbl_startdate" runat="server" Text="Semester Start Date" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
               
                    <asp:TextBox ID="txt_startdate" CssClass="textbox textbox1 txtheight1" Width="100px" Font-Bold="True"
                        runat="server" onchange="tdate()" Font-Size="Medium" Font-Names="Book Antiqua"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_startdate" Format="dd/MM/yyyy"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td colspan=2>
                    <asp:Label ID="lbl_enddate" runat="server" Text="Semester End Date" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_enddate" CssClass="textbox textbox1 txtheight1" Width="100px" Font-Bold="True" Font-Size="Medium" 
                        runat="server" onchange="tdate()" Font-Names="Book Antiqua"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_enddate" Format="dd/MM/yyyy"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td>
                  <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                            <ContentTemplate>
                    <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Go" OnClick="btnGo_Click" />
                        </ContentTemplate>
                                        </asp:UpdatePanel>
                </td>
                <%-- <td>
                    <asp:Button ID="btnadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Add" OnClick="btnadd_Click" />
                </td>--%>
            </tr>
        </table>
        <br />
        <br />
            <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
        <asp:GridView ID="gview" runat="server" ShowHeader="true" AutoGenerateColumns="false"
            Height="222px" OnSelectedIndexChanged="gview_SelectedIndexChanged" OnRowCreated="OnRowCreated">
            <%--onchange="QuantityChange1(this)"--%>
            <Columns>
                <asp:TemplateField HeaderText="January" Visible="false" ItemStyle-BackColor="#db77a5">
                    <ItemTemplate>
                        <asp:Label ID="Mon_1" runat="server"  Text='<%#Eval("Date01") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="January" Visible="false" ItemStyle-BackColor="Aqua">
                    <ItemTemplate>
                        <asp:Label ID="Mon1" runat="server" Text='<%#Eval("Day01") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="January" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Lbl1" runat="server" Text='<%#Eval("Holiday01") %>'></asp:Label>
                         <asp:Label ID="Lbl_1" runat="server" Text='<%#Eval("Holi_day01") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="February" Visible="false" ItemStyle-BackColor="#db77a5">
                    <ItemTemplate>
                        <asp:Label ID="Mon_2" runat="server" Text='<%#Eval("Date02") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="February" Visible="false" ItemStyle-BackColor="Aqua">
                    <ItemTemplate>
                        <asp:Label ID="Mon2" runat="server" Text='<%#Eval("Day02") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="February" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Lbl2" runat="server" Text='<%#Eval("Holiday02") %>'></asp:Label>
                        <asp:Label ID="Lbl_2" runat="server" Text='<%#Eval("Holi_day02") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="March" Visible="false" ItemStyle-BackColor="#db77a5">
                    <ItemTemplate>
                        <asp:Label ID="Mon_3" runat="server" Text='<%#Eval("Date03") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="March" Visible="false" ItemStyle-BackColor="Aqua">
                    <ItemTemplate>
                        <asp:Label ID="Mon3" runat="server" Text='<%#Eval("Day03") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="March" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Lbl3" runat="server" Text='<%#Eval("Holiday03") %>'></asp:Label>
                          <asp:Label ID="Lbl_3" runat="server" Text='<%#Eval("Holi_day03") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="April" Visible="false" ItemStyle-BackColor="#db77a5">
                    <ItemTemplate>
                        <asp:Label ID="Mon_4" runat="server" Text='<%#Eval("Date04") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="April" Visible="false" ItemStyle-BackColor="Aqua">
                    <ItemTemplate>
                        <asp:Label ID="Mon4" runat="server" Text='<%#Eval("Day04") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="April" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Lbl4" runat="server" Text='<%#Eval("Holiday04") %>'></asp:Label>
                          <asp:Label ID="Lbl_4" runat="server" Text='<%#Eval("Holi_day04") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="May" Visible="false" ItemStyle-BackColor="#db77a5">
                    <ItemTemplate>
                        <asp:Label ID="Mon_5" runat="server" Text='<%#Eval("Date05") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="May" Visible="false" ItemStyle-BackColor="Aqua">
                    <ItemTemplate>
                        <asp:Label ID="Mon5" runat="server" Text='<%#Eval("Day05") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="May" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Lbl5" runat="server" Text='<%#Eval("Holiday05") %>'></asp:Label>
                        <asp:Label ID="Lbl_5" runat="server" Text='<%#Eval("Holi_day05") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="June" ItemStyle-BackColor="#db77a5" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Mon_6" runat="server" Text='<%#Eval("Date06") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="June" ItemStyle-BackColor="Aqua" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Mon6" runat="server" Text='<%#Eval("Day06") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="June" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Lbl6" runat="server" Text='<%#Eval("Holiday06") %>'></asp:Label>
                         <asp:Label ID="Lbl_6" runat="server" Text='<%#Eval("Holi_day06") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="July" Visible="false" ItemStyle-BackColor="#db77a5">
                    <ItemTemplate>
                        <asp:Label ID="Mon_7" runat="server" Text='<%#Eval("Date07") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="July" Visible="false" ItemStyle-BackColor="Aqua">
                    <ItemTemplate>
                        <asp:Label ID="Mon7" runat="server" Text='<%#Eval("Day07") %>'></asp:Label>
                         
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="July" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Lbl7" runat="server" Text='<%#Eval("Holiday07") %>'></asp:Label>
                        <asp:Label ID="Lbl_7" runat="server" Text='<%#Eval("Holi_day07") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="August" Visible="false" ItemStyle-BackColor="#db77a5">
                    <ItemTemplate>
                        <asp:Label ID="Mon_8" runat="server" Text='<%#Eval("Date08") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="August" Visible="false" ItemStyle-BackColor="Aqua">
                    <ItemTemplate>
                        <asp:Label ID="Mon8" runat="server" Text='<%#Eval("Day08") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="August" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Lbl8" runat="server" Text='<%#Eval("Holiday08") %>'></asp:Label>
                        <asp:Label ID="Lbl_8" runat="server" Text='<%#Eval("Holi_day08") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="September" Visible="false" ItemStyle-BackColor="#db77a5">
                    <ItemTemplate>
                        <asp:Label ID="Mon_9" runat="server" Text='<%#Eval("Date09") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="September" Visible="false" ItemStyle-BackColor="Aqua">
                    <ItemTemplate>
                        <asp:Label ID="Mon9" runat="server" Text='<%#Eval("Day09") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="September" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Lbl9" runat="server" Text='<%#Eval("Holiday09") %>'></asp:Label>
                         <asp:Label ID="Lbl_9" runat="server" Text='<%#Eval("Holi_day09") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="October" Visible="false" ItemStyle-BackColor="#db77a5">
                    <ItemTemplate>
                        <asp:Label ID="Mon_10" runat="server" Text='<%#Eval("Date10") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="October" Visible="false" ItemStyle-BackColor="Aqua">
                    <ItemTemplate>
                        <asp:Label ID="Mon10" runat="server" Text='<%#Eval("Day10") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="October" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Lbl10" runat="server" Text='<%#Eval("Holiday10") %>'></asp:Label>
                         <asp:Label ID="Lbl_10" runat="server" Text='<%#Eval("Holi_day10") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="November" Visible="false" ItemStyle-BackColor="#db77a5">
                    <ItemTemplate>
                        <asp:Label ID="Mon_11" runat="server" Text='<%#Eval("Date11") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="November" Visible="false" ItemStyle-BackColor="Aqua">
                    <ItemTemplate>
                        <asp:Label ID="Mon11" runat="server" Text='<%#Eval("Day11") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="November" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Lbl11" runat="server" Text='<%#Eval("Holiday11") %>'></asp:Label>
                           <asp:Label ID="Lbl_11" runat="server" Text='<%#Eval("Holi_day11") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="December" Visible="false" ItemStyle-BackColor="#db77a5">
                    <ItemTemplate>
                        <asp:Label ID="Mon_12" runat="server" Text='<%#Eval("Date12") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="December" Visible="false" ItemStyle-BackColor="Aqua">
                    <ItemTemplate>
                        <asp:Label ID="Mon12" runat="server" Text='<%#Eval("Day12") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="December" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="Lbl12" runat="server" Text='<%#Eval("Holiday12") %>'></asp:Label>
                          <asp:Label ID="Lbl_12" runat="server" Text='<%#Eval("Holi_day12") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
            <RowStyle ForeColor="#333333" />
        </asp:GridView>

          <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: auto;
                        width:641px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                           <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: -28px; margin-left: 299px;"
                            OnClick="imagebtnpopclose3_Click" />
                         <center>
                        <span style="color: Green; font-size: large;">Day Order Settings</span>
                    </center>
                        <center>
                            <table>
                                <tr>
                                    <td align="center">
                                       <asp:RadioButton ID="rdbdayorder" runat="server" GroupName="dayordersettings" Text="DayOrder Change" Checked="true" OnCheckedChanged="rdbonchecked" AutoPostBack="true"/>
                                       <asp:RadioButton ID="rdbdoubleday" runat="server" GroupName="dayordersettings" Text="Double DayOrder"  OnCheckedChanged="rdbonchecked" AutoPostBack="true"/>
                                        <asp:RadioButton ID="rdbholiday" runat="server" GroupName="dayordersettings" AutoPostBack="true" Text="Holiday" OnCheckedChanged="rdbonchecked" />
                                       <asp:RadioButton ID="rdbAlternate" runat="server" GroupName="dayordersettings" Text="Alternate Schedule"  OnCheckedChanged="rdbonchecked" AutoPostBack="true" Visible="false"/>
                                      
                                    </td>
                                </tr>
                               <tr>
                               
                            <td>
                                   <fieldset id="dayorder" runat="server" visible="true">
                                <legend>Dayorder Settings</legend>
                                <table>
                                <tr>
                                <td>
                                <div id="divAlternateDayOrder" runat="server" >
                                                            <table style="">
                                                                <tr>
                                                              <td>   <asp:Label ID="lbldate" runat="server" Text="" CssClass="font" Visible="false"></asp:Label> </td>
                                                                    <td >
                                                                        <span>Alternate Day Order</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlAlternateDayOrder" runat="server" Style="
                                                                            width: auto;">
                                                                            <asp:ListItem Selected="True" Text="" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Day 1" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Day 2" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Day 3" Value="3"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Day 4" Value="4"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Day 5" Value="5"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Day 6" Value="6"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>

                                                                    <td style="padding-left: 15px; padding-right: 5px;">
                                                        <asp:Label ID="lblreasonadd" runat="server" Text="Reason" CssClass="font"></asp:Label>
                                                    </td>
                                                    <td  style="padding-right: 2px; padding-left: 2px;">
                                                        <asp:Button ID="btnreasonadd" runat="server" Text="+" CssClass="font" Height="21px"
                                                            OnClick="btnreasonadd_Click" Style="display: none; height: auto; width: auto;" />
                                                    </td>
                                                    <td style="padding-right: 2px; padding-left: 2px;">
                                                        <asp:DropDownList ID="ddlreason" runat="server" Width="150px" CssClass="font">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="padding-left: 2px;">
                                                        <asp:Button ID="btnreasondelete" runat="server" Text="-" OnClick="btnreasondelete_Click"
                                                            CssClass="font" Style="display: none; height: auto; width: auto;" />
                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                            
                                                           </td> </tr>
                                                            <tr>
                                                            <td>
                                <fieldset style="width: 504px; height: 48px;">
                                                  <legend style="font-size: larger; font-weight: bold">Day Order Settings For NextDay</legend>
                                                 <asp:RadioButton ID="rdbasperday" runat="server" Text="As Perday Schedule" 
                                                    GroupName="orderchange" AutoPostBack="true" />
                                                <asp:RadioButton ID="rdbskipday" runat="server" Text="Skipday order Change" GroupName="orderchange"
                                                    AutoPostBack="true" />
                                                <asp:RadioButton ID="rdbnextorder" runat="server" Text="Next Dayorder" GroupName="orderchange" 
                                                    AutoPostBack="true" Checked="true" />
                                              
                                            </fieldset>
                                         </td> </tr>
                                           <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="Chkincludeattendance" Text="Include Period in Attendance Report"
                                                            Checked="false" runat="server" AutoPostBack="true"  />
                                                    </td>
                                                </tr>
                                          
                                                     <tr style="margin-top: 10px; margin-bottom: 10px; padding-bottom: 5px; padding-top: 5px;">
                                                    <td colspan="6" style="margin-top: 10px; margin-bottom: 10px; padding-bottom: 5px;
                                                        padding-top: 5px;">
                                                        <asp:Label ID="lblerrmsg" runat="server" CssClass="font" ForeColor="Red" Style="margin-top: 10px;
                                                            margin-bottom: 10px; position: relative; padding: 3px;"></asp:Label>
                                                    </td>
                                                </tr>
                                           </table>

                                           <center>
                                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" BackColor="#21cc2fe6" />
                                           </center>
                                </fieldset>
                                </td></tr>
<tr>
                                <td>
                                 <fieldset id="fbDouble" runat="server" visible="false">
                                <legend>Double Dayorder</legend>
                                
                            <center>
                                <table  class="maintablestyle" style="width: 490px; height: 40px; background-color: #0CA6CA;">
                                <tr>
                                <td colspan="2">
                                                        <asp:CheckBox ID="chkbell" Text="Bell Time Not consider"
                                                            Checked="false" runat="server" AutoPostBack="true"  />
                                                    </td>
                                </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbldouble" runat="server" Text="Do you want to save  double dayorder for this date " Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btndoubl" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btndoubl_Click" Text="Yes" runat="server" BackColor="#e48795" />
                                                     <asp:Button ID="btnNo" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnNo_Click" Text="Delete" runat="server" BackColor="beige"  Visible="false"/>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                       
                 
                </fieldset>
                                </td>
                               </tr>
                             <tr>
                                <td>
                                 <fieldset id="fbholiday" runat="server" visible="false">
                                <legend>Holiday Settings</legend>
                                
                            <center>
                                <table  class="maintablestyle" style="width: 490px; height: 40px; background-color: #54A8BA;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbldesc" runat="server" Text="Description" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                        <asp:TextBox ID="txtholiday" runat="server" CssClass="textbox txtheight3 textbox1" TextMode="MultiLine" Width="243px" Height="39px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                      <td colspan="2">
                                       <center>
                                                <asp:Button ID="btnsaveholiday" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnsaveholiday_Click" Text="Save" runat="server" BackColor="#95F808" />
                                                     
                                            
                                                <asp:Button ID="btnholiday" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnholiday_Click" Text="Delete" runat="server"  BackColor="#EDD96E" Visible="false"/>
                                                     
                                            </center>
                                       </td>
                                    </tr>
                                </table>
                            </center>
                       
                 
                </fieldset>
                                </td>
                               </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>

             <center>
     
                <asp:Panel ID="panelreason" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="false" BorderWidth="2px" Style="left: 20%; top: 40%; right: 20%; height: auto;
                    position: absolute; width: 55%; z-index: 1000;">
                    <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold; margin: 6px; padding: 5px; height: auto;
                        width: auto;">
                        <span style="text-align: center; margin-bottom: 10px; margin-top: 10px; font-size: large;
                            font-weight: bold; line-height: 3px; padding: 3px;">Reason Entry </span>
                        <center>
                            <table style="margin: 0px; padding: 5px; margin-bottom: 10px; margin-top: 10px; width: 100%;
                                height: auto;">
                                <tr>
                                    <td style="margin: 0px; padding: 0px; width: auto; text-align: right; font-size: medium;
                                        padding-right: 10px;">
                                        <asp:Label ID="lblreaon" runat="server" Text="Reason" Font-Bold="True" Font-Names="Book Antiqua"
                                            Style="margin: 0px; width: auto; padding: 0px;"></asp:Label>
                                    </td>
                                    <td style="margin: 0px; padding: 0px; width: auto; text-align: left; font-size: medium;">
                                        <asp:TextBox ID="textreason" runat="server" Height="28px" TextMode="MultiLine" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" MaxLength="100" Style="resize: none;
                                            width: 98%;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="btnreasonsave" runat="server" Text="Add" OnClick="btnreasonsave_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        <asp:Button ID="btnreasonexit" runat="server" Text="Exit" OnClick="btnreasonexit_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <asp:Label ID="lblreasonerr" runat="server" CssClass="font" ForeColor="Red" Style="margin-bottom: 10px;
                            margin-top: 10px;"></asp:Label>
                    </div>
                </asp:Panel>
          
    </center>
    <center>
     <div id="divPopErr" runat="server" visible="false" style="height: 400em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnlPopErrContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblPopErr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnPopErrClose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnPopErrClose_Click" Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>

                 </ContentTemplate>
                    <Triggers>
                          <asp:PostBackTrigger ControlID="btnGo" />
                    </Triggers>
                </asp:UpdatePanel>
                <center>
                 <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel25">
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
    
    </center>
    <br />
</asp:Content>
