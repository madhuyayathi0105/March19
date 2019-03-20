<%@ Page Title="" Language="C#" MasterPageFile="~/HostelMod/hostelsite.master" AutoEventWireup="true"
    CodeFile="Hostel_Attendance_Manual.aspx.cs" Inherits="Hostel_Attendance_Manual" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .subdivstyle
            {
                z-index: 1000;
                width: 100%;
                position: absolute;
                top: 239px;
               
            }
        </style>
          <script type="text/javascript">
              function Check_Click(objRef, colIndex) {
                  var row = objRef.parentNode.parentNode;
                  var rowIndex = row.rowIndex - 1;
                  var customerId = row.cells[6].innerHTML;
                  var ID = row.cells[6].innerHTML;
                  var pres = row.cells[6].getElementsByTagName("input")[0].value;
                  var chkSelectid = row.cells[5].innerHTML;
                  var m = chkSelectid;
                  var g = m.length;
                  if (m.length != 156) {
                      if (pres == "Present") {
                          row.cells[6].getElementsByTagName("input")[0].value = "Absent";
                          row.cells[6].getElementsByTagName("input")[0].style.backgroundColor = "Red";
                          row.cells[5].innerHTML = "Present";
                          row.cells[6].style.backgroundColor = "Red";
                          row.cells[5].style.backgroundColor = "green";
                          //                    row.cells[6].getElementsByTagName("input")[0].value = "OD";
                          //                    row.cells[6].getElementsByTagName("input")[0].style.backgroundColor = "Aqua";


                      }
                      else {
                          row.cells[5].innerHTML = "Absent";
                          row.cells[5].style.backgroundColor = "Red";
                          row.cells[6].getElementsByTagName("input")[0].value = "Present";
                          row.cells[6].getElementsByTagName("input")[0].style.backgroundColor = "green";
                          //                    row.cells[6].getElementsByTagName("input")[0].value = "OD";
                          //                    row.cells[6].getElementsByTagName("input")[0].style.backgroundColor = "Aqua";
                          //                    row.cells[6].style.backgroundColor = "Aqua";
                          row.cells[6].style.backgroundColor = "green";
                          row.cells[5].style.backgroundColor = "Red";
                      }
                  }
      


              }
              function Check_Click1(objRef, colIndex) {
                  var grdvw = document.getElementById('<%= gvatte.ClientID %>');
                  var row = objRef.parentNode.parentNode;
                  var rowIndex = row.rowIndex - 1;
                  var customerId = row.cells[7].innerHTML;
                  var press = row.cells[7].getElementsByTagName("input")[0].value;
                  var press1 = row.cells[5].getElementsByTagName("input")[0].value;

                  var m = get;
                  var g = m.length;

                  if (m.length != 156) {
                      if (press == "OD") {
                          //                    row.cells[6].getElementsByTagName("input")[0].value = "Absent";
                          //                    row.cells[6].getElementsByTagName("input")[0].style.backgroundColor = "Red";
                          row.cells[5].innerHTML = "OD";
                          //                    row.cells[6].style.backgroundColor = "Red";
                          row.cells[5].style.backgroundColor = "Aqua";
                          row.cells[6].getElementsByTagName("input")[0].value = "Present";
                          row.cells[6].getElementsByTagName("input")[0].style.backgroundColor = "green";
                          row.cells[6].style.backgroundColor = "green";
                      }
                      else {
                          //                    row.cells[6].getElementsByTagName("input")[0].value = "OD";
                          //                    row.cells[6].getElementsByTagName("input")[0].style.backgroundColor = "Aqua";
                          row.cells[5].innerHTML = "Absent";
                          //                    row.cells[6].style.backgroundColor = "Aqua";
                          row.cells[5].style.backgroundColor = "Red";
                          row.cells[6].style.backgroundColor = "green";
                          row.cells[6].getElementsByTagName("input")[0].value = "Present";
                          row.cells[6].getElementsByTagName("input")[0].style.backgroundColor = "green";
                      }
                  }
              }
              function Check_Click2() {
                  var GridVwHeaderChckbox = document.getElementById("<%=gvatte.ClientID %>");
                  var s;
                  for (var i = 2; i < GridVwHeaderChckbox.rows.length; i++) {
                      var get = GridVwHeaderChckbox.rows[i].cells[5].innerHTML;
                      var m = get;
                      var g = m.length;
                      if (m.length == 160) {
                          GridVwHeaderChckbox.rows[i].cells[5].innerHTML = "Absent";
                          get = GridVwHeaderChckbox.rows[i].cells[5].innerHTML;
                          m = get;
                      }
                      if (m.length == 161) {
                          GridVwHeaderChckbox.rows[i].cells[5].innerHTML = "Present";
                          get = GridVwHeaderChckbox.rows[i].cells[5].innerHTML;
                          m = get;
                      }
                      if (m.length == 156) {
                          GridVwHeaderChckbox.rows[i].cells[5].innerHTML = "OD";
                          get = GridVwHeaderChckbox.rows[i].cells[5].innerHTML;
                          m = get;
                      }

                      if (i == 2)
                          s = m;
                      else
                          s = s + ',' + m;
                  }
                  document.getElementById("<%=hid.ClientID %>").value = s;
                  var ms = document.getElementById("<%=hid.ClientID %>").value;
              }
        </script>
       
    </head>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <center>
        <span class="fontstyleheader" style="color: green; margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative;">Hostel Attendance Manual</span>
    </center>
    <br />
    <div class="maindivstyle maindivstylesize">
        <br />
        <center>
            <table class="maintablestyle" style="margin: 0px; margin-bottom: 0px; margin-top: 10px;
                position: relative;" >
                <tr>
                    <td colspan="10">
                        <fieldset style="width: 700px; height: 35px;">
                      
                            <asp:RadioButton ID="rdbhostel" runat="server" Text="Hostel /Study Hours Attendance"
                                OnCheckedChanged="rdbhostel_CheckedChange" GroupName="Attendance" AutoPostBack="true" />
                       
                            <asp:RadioButton ID="rdbmess" runat="server" Text="Mess Attendance" GroupName="Attendance"
                                OnCheckedChanged="rdbmess_CheckedChange" AutoPostBack="true" />
                        <asp:UpdatePanel ID="updatepanel6" runat="server">
                            <ContentTemplate>
                            <asp:RadioButton ID="rdbstudy" runat="server" Text="Study Hours Attendance" GroupName="Attendance"
                                Visible="false" OnCheckedChanged="rdbstudy_CheckedChange" AutoPostBack="true" /> </ContentTemplate>
                        </asp:UpdatePanel>
                        </fieldset>
                    </td>
                    <td>
                        <asp:Label ID="Lblroll" runat="server" Text="" Visible="false" CssClass="commonHeaderFont"></asp:Label>
                        <asp:Label ID="Lblapp" runat="server" Text="" Visible="false" CssClass="commonHeaderFont"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblhostel" runat="server" Text="Hostel" CssClass="commonHeaderFont" Visible="false"></asp:Label>
                    </td>
                    <td>
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                        <asp:DropDownList ID="ddl_Hostel" runat="server" CssClass="textbox1  ddlheight1" Visible="false"
                            OnSelectedIndexChanged="ddl_Hostel_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                         </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblAttendance" runat="server" Text="Attendance Date" CssClass="commonHeaderFont" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_attandance" runat="server" CssClass="textbox  txtheight" AutoPostBack="true" Visible="false"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_attandance" runat="server"
                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="Lblsession" runat="server" Text="Session" CssClass="commonHeaderFont" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsession" runat="server" CssClass="textbox1  ddlheight1" Visible="false">
                            <asp:ListItem Value="0">Morning</asp:ListItem>
                            <asp:ListItem Value="1">Evening</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <%--<td>
                        <asp:Label ID="Lblstatus" runat="server" Text="Status" CssClass="commonHeaderFont"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_status" runat="server" CssClass="textbox1  ddlheight1">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>
                    </td>--%>
                     <td>
                                <asp:Label ID="Lblbuild" Text="Building Name" runat="server" Width="52px" Visible="false"></asp:Label>
                            </td>
                            <td>
                               <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="drbbuilding"  runat="server" CssClass="textbox textbox1 ddlheight1" Visible="false"
                                            AutoPostBack="true" OnSelectedIndexChanged="drbbuilding_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                    <td>
                        <asp:Label ID="lbl_floorname" Text="Floor" runat="server" Width="52px" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upp1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_floorname" runat="server" Visible="false" CssClass="textbox textbox1"
                                    Width="82px" ReadOnly="true" Height="20px" >--Select--</asp:TextBox>
                                <asp:Panel ID="pflrnm" runat="server" Visible="false" CssClass="multxtpanel" Width="155px"
                                    Height="250px">
                                    <asp:CheckBox ID="cb_floorname" runat="server" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_floorname_CheckedChange" />
                                    <asp:CheckBoxList ID="cbl_floorname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_floorname_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupExt4" runat="server" TargetControlID="txt_floorname"
                                    PopupControlID="pflrnm" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                  <%--  <td>
                    <asp:UpdatePanel ID="updatepanel3" runat="server">
                            <ContentTemplate>
                    <asp:CheckBox ID="Chroll" runat="server" Text="Roll No" AutoPostBack="true" OnCheckedChanged="Chroll_CheckedChanged" Visible="false" />
                        <asp:TextBox ID="txrollno" runat="server" Visible="false" Width="69px" Enabled="true" 
                            Text="" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txrollno"
                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="rollno" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txrollno"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender></ContentTemplate>
                        </asp:UpdatePanel>
                       
                    </td>--%>
                    <td>
                                    <asp:Label ID="lblnum" runat="server" Text="Roll No" Visible="false"></asp:Label></td>
                                    <td>
                                    <asp:UpdatePanel ID="updatepanel5" runat="server">
                            <ContentTemplate>
                                    <asp:DropDownList ID="ddlrollno" runat="server" AutoPostBack="True" CssClass="textbox1 ddlheight1" Visible="false"
                                        OnSelectedIndexChanged="ddlrollno_SelectedIndexChanged">
                                      <%--  <asp:ListItem>Roll No</asp:ListItem>
                                        <asp:ListItem>Reg No</asp:ListItem>
                                       <%-- <asp:ListItem>Adm No</asp:ListItem>
                                        <asp:ListItem>App No</asp:ListItem>--%>
                                       <%-- <asp:ListItem>Name</asp:ListItem>
                                        <asp:ListItem>Hostel Id</asp:ListItem>--%>
                                    </asp:DropDownList></ContentTemplate></asp:UpdatePanel>
                                </td>
                                     <td>
                                      <asp:UpdatePanel ID="updatepanel3" runat="server">
                            <ContentTemplate>
                    <asp:CheckBox ID="Chroll" runat="server" Text="" AutoPostBack="true" OnCheckedChanged="Chroll_CheckedChanged" Visible="false" />
                                    <asp:TextBox ID="txtno" runat="server" CssClass="textbox textbox1" Width="89px" Visible="false" Enabled="false"
                                        AutoPostBack="True"></asp:TextBox>
                                    <%--OnTextChanged="txtno_TextChanged"--%>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtno"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -,.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender></ContentTemplate></asp:UpdatePanel>
                                </td>
                    <td>
                        <center>
                            <asp:Button ID="Btnmess" runat="server" CssClass="fontbold" Width="44px" Height="28px" Visible="false"
                                Text="Go" OnClick="Go_Click" />
                        </center>
                    </td>
                </tr>
                <tr></tr>
                <tr>
                    <td >
                        <asp:Label ID="Lblroom" runat="server" Text="Room" CssClass="commonHeaderFont" Visible="false"></asp:Label>
                        
                                </td>
                                <td>
                                 <asp:UpdatePanel ID="updatepanel_room" runat="server" Visible="false">
                            <ContentTemplate>
                                    <asp:TextBox ID="txt_room" runat="server" CssClass="textbox  txtheight2" ReadOnly="true" >--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_room" runat="server" Width="128px" CssClass="multxtpanel multxtpanleheight">
                                        <asp:CheckBox ID="cb_room" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_room_CheckedChanged"  />
                                        <asp:CheckBoxList ID="cbl_room" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_room_SelectedIndexChanged" >
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_room" 
                                        PopupControlID="panel_room" Position="Bottom">
                                    </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="updatepanel2" runat="server">
                            <ContentTemplate>
                        <asp:CheckBox ID="Cbsearchroom" runat="server" Text="search" AutoPostBack="true" Visible="false"
                            OnCheckedChanged="Cbsearchroom_CheckedChanged" />  </ContentTemplate>
                        </asp:UpdatePanel></td>
                            <td>
                             <asp:UpdatePanel ID="updatepanel7" runat="server">
                            <ContentTemplate>
                        <asp:TextBox ID="txtroom" runat="server" Visible="false" Width="69px" Enabled="true" 
                            Text="Room No" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderroll" runat="server" TargetControlID="txtroom"
                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:AutoCompleteExtender ID="autocomplete_txtroom" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="roomno" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtroom"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <center>
                            <asp:Button ID="btn_go" runat="server" CssClass="fontbold" Width="44px" Height="28px" Visible="false"
                                Text="Go" OnClick="Go_Click" />
                        </center>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <p style="width: 691px;" align="left">
        <asp:Label ID="lbl_absent" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
    </p>
    <p style="width: 691px;" align="left">
        <asp:Label ID="lbl_present" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
    </p>
    <p style="width: 691px;" align="left">
        <asp:Label ID="lbl_total" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
    </p>
    <center>
        <%--<FarPoint:FpSpread ID="Fpspread" runat="server" BorderColor="white" BorderStyle="Solid"
                                BorderWidth="0px" Width="470px" Style="height: 354px; overflow: auto; background-color: white;"
                                OnPreRender="Fpspread_SelectedIndexChanged" OnCellClick="Fpspread_CellClick">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" BackColor="white">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>--%>
                              <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="white" BorderStyle="Solid"
            BorderWidth="0px" Style="overflow: auto; background-color: white;" OnPreRender="Fpspread_SelectedIndexChanged"
            OnCellClick="Fpspread_CellClick">
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" BackColor="white">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
                             <asp:UpdatePanel ID="updatepanel4" runat="server">
                            <ContentTemplate>
        <FarPoint:FpSpread ID="Fpspread" runat="server" BorderColor="white" BorderStyle="Solid"
            BorderWidth="0px" Style="overflow: auto; background-color: white;" OnPreRender="Fpspread_SelectedIndexChanged"
            OnCellClick="Fpspread_CellClick">
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" BackColor="white">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
          </ContentTemplate>
                        </asp:UpdatePanel>
        <asp:GridView ID="gvatte1" runat="server" ShowHeader="true" AutoGenerateColumns="False"
            Width="100px" Height="100px"  CssClass="font">
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>" ></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Roll No" Visible="false" >
                    <ItemTemplate>
                        <input type="button" runat="server" id="btn" name="ADD" class="button" value='<%# Eval("Roll_no") %>'
                            onclick="Check_Click1(this);" style="width: 100px" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reg No">
                    <ItemTemplate>
                        <input type="button" runat="server" id="btn1" name="ADD" class="button" value='<%# Eval("Roll_no") %>'
                            onclick="Check_Click1(this);" style="width: 100px" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Admission No">
                    <ItemTemplate>
                        <input type="button" runat="server" id="btn2" name="ADD" class="button" value='<%# Eval("Roll_no") %>'
                            onclick="Check_Click1(this);" style="width: 100px" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <input type="button" runat="server" id="btn3" name="ADD" class="button" value='<%# Eval("Roll_no") %>'
                            onclick="Check_Click1(this);" style="width: 100px" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="350px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Room No">
                    <ItemTemplate>
                        <input type="button" runat="server" id="btn4" name="ADD" class="button" value='<%# Eval("Roll_no") %>'
                            onclick="Check_Click1(this);" style="width: 100px" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Current Status">
                    <ItemTemplate>
                        <input type="button" runat="server" id="btn5" name="ADD" class="button" value='<%# Eval("Roll_no") %>'
                            onclick="Check_Click1(this);" style="width: 100px" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mark as">
                    <ItemTemplate>
                        <%-- <asp:Label ID="btn1" runat="server" Text="present" OnClientClick="Check_Click(this);"></asp:Label>
                        <%-- <input type="button" ID="btn1" Text="Select"  runat="server" CommandName="Select" OnClientClick="Check_Click(this);" />--%>
                        <input type="button" runat="server" id="btn6" name="ADD" class="button" value=""
                            onclick="Check_Click(this);" style="width: 100px" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mark as">
                    <ItemTemplate>
                        <%-- <asp:Button ID="btn" Text="OD" runat="server"  CommandName="Select"  OnClientClick="Check_Click(this);" />--%>
                        <input type="button" runat="server" id="btn7" name="ADD" class="button" value=""
                            onclick="Check_Click1(this);" style="width: 100px" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="val" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblpre" runat="server" ForeColor="Brown" Text="" Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="vals" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblroll_nos" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </center>
    <center>
        <asp:Button ID="Btnsavemess" runat="server" CssClass="fontbold" Width="96px" Height="28px"
            Text="Save" OnClick="Btnsavemess_Click" />
    </center>

    
    <center>
        <div id="popwindow1" style="background-color: white; height: auto; width: 979px; margin-left:245px"
            margin-top="19px"  runat="server" visible="false" class="subdivstyle"  >
            <br />
            <asp:ImageButton ID="imgbtn_popclose" Visible="false" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: -52px; margin-left: 388px;"
                OnClick="imagebtnpopclose_Click" />
            <br />
            <center>
                <div>
                    <asp:Label ID="lblstu1" runat="server" Style="color: black;" Text="Student Attendance"></asp:Label>
                </div>
            </center>
            <p style="width: 691px;" align="right">
                <asp:Label ID="lbl_total1" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
            </p>
            <FarPoint:FpSpread onDataChanged="show(event)" ID="Fpspread2" runat="server" BorderColor="white"
                BorderStyle="Solid" BorderWidth="0px" Style="overflow: scroll; background-color: blue;"
                OnButtonCommand="btnType_Click" onClick="reply_click(this.id)" VerticalScrollBarPolicy="Never"
                HorizontalScrollBarPolicy="Never">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" BackColor="white">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <div>
                <asp:GridView ID="gvatte" runat="server" ShowHeader="true" AutoGenerateColumns="False"
                    Width="100px" Height="100px" CssClass="font">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>" ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Student Id" >
                            <ItemTemplate>
                                <asp:Label ID="lblid_no" runat="server" Text='<%# Eval("id") %>' Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Roll No" >
                            <ItemTemplate>
                                <asp:Label ID="lblroll_no" runat="server" Text='<%# Eval("Roll_no") %>' Width="130px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>
                        <%-- <asp:TemplateField HeaderText="Reg No">
                            <ItemTemplate>
                                <asp:Label ID="lblReg_no" runat="server" Text='<%# Eval("Reg_no") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Admission No">
                            <ItemTemplate>
                                <asp:Label ID="lblAdmitNo" runat="server" Text='<%# Eval("Roll_Admit") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Student Name">
                            <ItemTemplate>
                                <asp:Label ID="lblstud_name" runat="server" Text='<%# Eval("stud_name") %>' Width="200px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="550px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Room No">
                            <ItemTemplate>
                                <asp:Label ID="lblstud_type" runat="server" ForeColor="Brown" Text='<%# Eval("Room_Name") %>' Width="70px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Current Status">
                            <ItemTemplate>
                                <asp:Label ID="lblAdmitNo" runat="server" Text="" Width="100px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mark as">
                            <ItemTemplate>
                                <%-- <asp:Label ID="btn1" runat="server" Text="present" OnClientClick="Check_Click(this);"></asp:Label>--%>
                                <%-- <input type="button" ID="btn1" Text="Select"  runat="server" CommandName="Select" OnClientClick="Check_Click(this);" />--%>
                                <input type="button" runat="server" id="btn1" name="ADD" class="button" value=""
                                    onclick="Check_Click(this);" style="width: 100px" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                        </asp:TemplateField>
                           <asp:TemplateField >
                            
                            <HeaderTemplate>
                            <th colspan="7">Mess Attendance Status</th>
                      <tr >
                           
                            <th colspan="8"></th>
                                <th colspan="1">Break Fast</th>                        
           <th colspan="1">Lunch</th>
           <th colspan="1">Dinner</th>                        
           </tr> 
                                                    
                    </HeaderTemplate>   
                            <ItemTemplate>
                            <td>
                                  <asp:Label ID="lblAdmitNo1" runat="server" Text="" width="90px"></asp:Label></td>
                                   <td>  <asp:Label ID="lblAdmitNo12" runat="server" Text="" ></asp:Label></td>
                                   <td>  <asp:Label ID="lblAdmitNo13" runat="server" Text="" ></asp:Label></td>
                               
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>
                     <%-- <asp:TemplateField HeaderText="Mark as">
                            <ItemTemplate>
                                
                                <input type="button" runat="server" id="btn" name="ADD" class="button" value="" onclick="Check_Click1(this);"
                                    style="width: 100px" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="val" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblpre" runat="server" ForeColor="Brown" Text="" Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="vals" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblroll_nos" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <center>
        <div id="Div1" runat="server" visible="false" style="height: 0%; z-index: 100px;
            width: 0%; background-color: rgba(54, 25, 25, .2); position: absolute; top: -40px;
            left: 0px;">
            <center>
                <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;     margin-left: 153px;

                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Lblerror" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="Button1" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btnerrclose_Click1" Text="ok" runat="server" />
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
            <%--<FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="white" BorderStyle="Solid" OnDataChanged="get_row()"
                        BorderWidth="0px" Width="790px" Style="height: 354px; overflow: auto; background-color: white;"
                        OnButtonCommand="btnType_Click">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" BackColor="white">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>--%>
            <br />
            <input type="hidden" runat="server" id="hid" />
            <center>
                <asp:Button ID="Btnsave" CssClass="textbox btn1 comm" OnClick="Btnsave_Click" BackColor="#c300ff66"
                    Text="save" runat="server" ForeColor="Black" OnClientClick="return Check_Click2();"
                    Style="width: 100px" />
                <asp:Button ID="Btnclose" CssClass=" textbox btn1 comm" OnClick="Btnclose_Click"
                    BackColor="#ff6c00e6" ForeColor="Black" Text="Close" runat="server"   Style="width: 81px"/>
            </center>
            <br />
             

        </div>
        <%-- </div>--%>
        
    </center>
    <%--  <center>
                    <div id="alertpopwindow" runat="server" visible="false" style="height: 140px; z-index: 1000;
                        width: 258px; background-color: blue; position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: Red; height: 120px;
                                width: 238px; border: 5px solid Red; border-top: 25px solid Red; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="height: 100px; width:0px">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" OnClick="btnerrclose_Click"
                                                        Text="Ok" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>--%>
    <center>
        <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 100px;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 50px;
            left: 0px;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btnerrclose_Click" Text="ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    </html>
</asp:Content>
