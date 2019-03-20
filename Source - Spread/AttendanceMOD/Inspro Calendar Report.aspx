<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master" AutoEventWireup="true" CodeFile="Inspro Calendar Report.aspx.cs" Inherits="AttendanceMOD_Inspro_Calendar_Report" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script type="text/javascript">
     function PrintPanel() {
         var panel = document.getElementById("<%=divgview.ClientID %>");
         var printWindow = window.open('', '', 'height=842,width=1191');
         printWindow.document.write('<html');
         printWindow.document.write('<head>');
         printWindow.document.write('</head><body >');
         printWindow.document.write('<form>');
         printWindow.document.write(panel.innerHTML);
         printWindow.document.write(' </form>');
         printWindow.document.write('</body></html>');
         printWindow.document.close();
         setTimeout(function () {
             printWindow.print();
         }, 500);
         return false;
     }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="Label2" runat="server" Text="Inspro Calendar Report" Font-Bold="True"
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
                    <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddl_Bat" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" CausesValidation="True"
                        OnSelectedIndexChanged="ddl_Bat_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                
               
                    <td>
                    <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua">
                    </asp:Label>
                </td>
                <td>
                   <asp:DropDownList ID="ddl_degree" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" CausesValidation="True"
                        OnSelectedIndexChanged="ddl_degree_SelectedIndexChanged">
                    </asp:DropDownList>
                  
                </td>
                <td>
                    <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                     <asp:DropDownList ID="ddlbranch" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" CausesValidation="True"
                        OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                    </asp:DropDownList>
                    <%--<asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" Width="245px">
                    </asp:DropDownList>--%>
                </td>
                 <td>
                    <asp:Label ID="lblsemster" runat="server" Text="Semester " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlduration" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" CausesValidation="True"
                        OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                        
                    </asp:DropDownList>
                    </td>
                    <td>
                    <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="true" Font-Size="Medium"
                     Font-Names="Book Antiqua" Visible="false"></asp:Label>
                    </td>
                    <td>
                    <asp:DropDownList ID="ddlsec" runat="server" AutoPostBack="true" Font-Bold="true"
                     Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" CausesValidation="true"
                      OnSelectedIndexChanged="ddlsec_SelectedIndexChanged" Visible="false">

                      </asp:DropDownList>
                    </td>
<%--
                    <td class="sec-lbl">
                                <span id="MainContent_lblSec" style="display:inline-block;font-family:Book Antiqua;font-size:Medium;font-weight:bold;height:22px;width:26px;">Sec</span>
                            </td>
                            <td class="sem-ddl">
                                <select name="ctl00$MainContent$ddlSec" onchange="javascript:setTimeout(&#39;__doPostBack(\&#39;ctl00$MainContent$ddlSec\&#39;,\&#39;\&#39;)&#39;, 0)" id="MainContent_ddlSec" style="font-family:Book Antiqua;font-size:Medium;font-weight:bold;height:22px;width:50px;">
		<option selected="selected" value="A">A</option>
		<option value="B">B</option>

	</select>
                            </td>--%>
            </tr>
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
              
            </tr>
        </table>
         <br />
        <br />
          <div id="divgview" runat="server" style="display: table; margin: 0px; height: auto;
                                margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
            <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
        <asp:GridView ID="gview" runat="server" ShowHeader="false" AutoGenerateColumns="true" toGenerateColumns="false" 
            Height="222px" >
            <%--onchange="QuantityChange1(this)"--%>
            <Columns></Columns>
          <%--  <Columns>
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
            </Columns>--%>
            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
            <RowStyle ForeColor="#333333" />
        </asp:GridView>
        </div>
        <center>
        <br />
        <br />
         <center>
         <asp:Button ID="btndirectPrint" runat="server" Text="Direct Print" Visible="false" OnClientClick="return PrintPanel();"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Visible="false" />
                        <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                  
               </center>
                                        
                                    </center>
        </ContentTemplate>
        </asp:UpdatePanel>
        </center>
</asp:Content>

