<%@ Page Title="" Language="C#" MasterPageFile="~/Requestmod/RequestSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="EventRequest.aspx.cs" Inherits="EventRequest" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
     <script type="text/javascript">
         function QuantityChange2() {
             alert("hiii");
             var ddl_select = document.getElementById('MainContent_rdo_single');
             alert("hio");
             if (ddl_select.checked == true) {
                 alert("hi");
                    var lblAmt4 = document.getElementById("<%=txtfd.ClientID %>");
                    var lblAmt3 = document.getElementById("<%=txttd.ClientID %>");
                    alert("ho");
                    if (lblAmt4.value == lblAmt3.value) {
                        return false;
                    }
                    else {
                        alert("h");
                        alert("The From Date Equal to To date");
                        return false;
                    }
                }

            }
            </script>
    <style type="text/css">
        .maindivstylesize
        {
            height: 3000px;
            width: 1000px;
        }
        .lbl
        {
            text-align: center;
        }
        .container
        {
            width: 100%;
        }
        .col1
        {
            float: left;
            width: 50%;
        }
        .col2
        {
            float: right;
            width: 50%;
        }
        .newtextbox
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        
        .textboxshadow:hover
        {
            outline: none;
            border: 1px solid #BAFAB8;
            box-shadow: 0px 0px 8px #BAFAB8;
            -moz-box-shadow: 0px 0px 8px #BAFAB8;
            -webkit-box-shadow: 0px 0px 8px #BAFAB8;
        }
        .textboxchng
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        .div1
        {
            width: 899px;
            height: auto;
            border: 1px solid;
            overflow: auto;
        }
        .div2
        {
            width: 900px;
            height: auto;
            border: 1px solid;
            overflow: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
 <script type="text/javascript">

     function checkForm(form) {
         var starttime;
         var endtime;
         re = /(?:[0-1]?[0-9]|[2][1-4]):[0-5]?[0-9]:AM|pm?/;
         re1 = /(?:[0-1]?[0-9]|[2][1-4]):[0-5]?[0-9]:PM|pm?/;
         var tbl = document.getElementById("<%=gridadd.ClientID %>");
         var gridViewControls = tbl.getElementsByTagName("input");
         for (var i = 0; i < (gridViewControls.length); i++) {
             starttime = document.getElementById('gridadd_txt_start_' + i.toString());
             endtime = document.getElementById('gridadd_txt_end_' + i.toString());

             if (starttime.value != '' && (!starttime.value.match(re))) {
                 if (starttime.value != '' && (!starttime.value.match(re1))) {
                     alert("Invalid time format: " + starttime.value);
                     return false;
                 }

             }
             if (endtime.value != '' && (!endtime.value.match(re))) {
                 if (endtime.value != '' && (!endtime.value.match(re1))) {
                     alert("Invalid time format: " + endtime.value);
                     return false;
                 }
             }

         }
     }

     function checkTime(form) {
         var starttime;
         var endtime;
         var tbl = document.getElementById("<%=gridadd.ClientID %>");
         var gridViewControls = tbl.getElementsByTagName("input");
         for (var i = 0; i < (gridViewControls.length); i++) {
             starttime = document.getElementById('gridadd_txt_start_' + i.toString());
             endtime = document.getElementById('gridadd_txt_end_' + i.toString());
             if (i == (gridViewControls.length) - 1) {
                 alert(starttime.value);
             }

         }
     }

     function closepartidiv() {
         var stud = document.getElementById("<%=pop_add_staff_stud_othr.ClientID %>");
         stud.style.display = "none";
         return false;

     }

     function closelocation() {
         var stud = document.getElementById("<%=pop_bul_flr_room.ClientID %>");
         stud.style.display = "none";
         return false;

     }

     function closepresentdiv() {
         var stud = document.getElementById("<%=pop_add_staff_stud_othr1.ClientID %>");
         stud.style.display = "none";
         return false;

     }
     function expnname(id) {
         var value1 = id.value;

         if (value1.trim().toUpperCase() == "OTHERS") {
             var idval = document.getElementById("<%=txt_expn_name.ClientID %>");
             idval.style.display = "block";

         }
         else {
             var idval = document.getElementById("<%=txt_expn_name.ClientID %>");
             idval.style.display = "none";
         }
     }
     function actinnam(id) {
         var value1 = id.value;

         if (value1.trim().toUpperCase() == "OTHERS") {
             var idval = document.getElementById("<%=txt_act_namenew.ClientID %>");
             idval.style.display = "block";

         }
         else {
             var idval1 = document.getElementById("<%=txt_act_namenew.ClientID %>");
             idval1.style.display = "none";
         }
     }
     function tour(id) {
         var value1 = id.value;

         if (value1.trim().toUpperCase() == "OTHERS") {
             var idval = document.getElementById("<%=txt_Tournament.ClientID %>");
             idval.style.display = "block";

         }
         else {
             var idval = document.getElementById("<%=txt_Tournament.ClientID %>");
             idval.style.display = "none";
         }
     }

     function titleevent(id) {
         var value1 = id.value;

         if (value1.trim().toUpperCase() == "OTHERS") {
             var idval = document.getElementById("<%=txt_poprd_title.ClientID %>");
             idval.style.display = "block";

         }
         else {
             var idval = document.getElementById("<%=txt_poprd_title.ClientID %>");
             idval.style.display = "none";
         }
     }

     function game(id) {
         var value1 = id.value;

         if (value1.trim().toUpperCase() == "OTHERS") {
             var idval = document.getElementById("<%=txt_game.ClientID %>");
             idval.style.display = "block";

         }
         else {
             var idval = document.getElementById("<%=txt_game.ClientID %>");
             idval.style.display = "none";
         }
     }
     function Award(id) {
         var value1 = id.value;

         if (value1.trim().toUpperCase() == "OTHERS") {
             var idval = document.getElementById("<%=txt_awdcat.ClientID %>");
             idval.style.display = "block";

         }
         else {
             var idval = document.getElementById("<%=txt_awdcat.ClientID %>");
             idval.style.display = "none";
         }
     }
     function seminar(id) {
         var value1 = id.value;

         if (value1.trim().toUpperCase() == "OTHERS") {
             var idval = document.getElementById("<%=txt_seminar.ClientID %>");
             idval.style.display = "block";

         }
         else {
             var idval = document.getElementById("<%=txt_seminar.ClientID %>");
             idval.style.display = "none";
         }
     }

     function eventname(id) {
         var value1 = id.value;

         if (value1.trim().toUpperCase() == "OTHERS") {
             var idval = document.getElementById("<%=txtothers.ClientID %>");
             idval.style.display = "block";

         }
         else {
             var idval = document.getElementById("<%=txtothers.ClientID %>");
             idval.style.display = "none";
         }
     }
     function checkEmail(id) {

         var filter = /^([a-z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
         if (!filter.test(id.value)) {
             id.style.borderColor = 'Red';
             id.value = "";
             email.focus;
         }
         else {
             id.style.borderColor = '#c4c4c4';
         }
     }
     function outinstitution(id) {
         var value1 = id.value;

         if (value1.trim().toUpperCase() == "OTHERS") {
             var idval = document.getElementById("<%=txt_outinstitution.ClientID %>");
             idval.style.display = "block";

         }
         else {
             var idval = document.getElementById("<%=txt_outinstitution.ClientID %>");
             idval.style.display = "none";
         }
     }

     function outorgnzname(id) {
         var value1 = id.value;

         if (value1.trim().toUpperCase() == "OTHERS") {
             var idval = document.getElementById("<%=txt_outorganz.ClientID %>");
             idval.style.display = "block";

         }
         else {
             var idval = document.getElementById("<%=txt_outorganz.ClientID %>");
             idval.style.display = "none";
         }
     }
  </script>
      <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="">
        <ProgressTemplate>
            <center>    
            <div style="height: 300%; z-index: 100000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
                      <div class="CenterPB" style="height: 40px; width: 40px; top: 900px; left: 450px;
                    position: absolute;">
                    <image src="images/ajax_loader_green_350.gif" height="180px" width="180px" />
                    </div>
                    </div>
                
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <br />
   
     <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: #008000; font-size: xx-large">Event Request</span></div>
                <br />
        </center>
    </div>
    <div class="maindivstyle">
       <div id="div_event" runat="server" visible="true">
            <br />
                <center>
                
                    <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                        height: 40px; margin-left: 500px; margin-top:20px">
                        <table>
                            <tr>
                                <td>
                                    Requisition No
                                </td>
                                <td>
                                    <asp:TextBox ID="rqustn_no_event" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                    </asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label29" Text="Req Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TextBox6" runat="server" ReadOnly="true" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="TextBox6" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                     <div style="margin-left:15px; margin-top: -5px; width: 900px; height: 49px" class="maindivstyle">
              
            
    <div style="margin-left: -633px; margin-top: 5px; width: 250px; height: 30px" class="maindivstyle">
    <table>
    <tr>
    <td><asp:RadioButton ID="rdo_single" Text="Single Day" runat="server" AutoPostBack="true" GroupName="hh" Checked="true" OnCheckedChanged="rdo_single_CheckedChanged" /></td>
    <td><asp:RadioButton ID="rdo_multipl" Text="Multiple Day" runat="server" AutoPostBack="true" GroupName="hh" OnCheckedChanged="rdo_multipl_CheckedChanged" /></td></tr></table>
    </div>
              
              <div style="margin-top: -37px;margin-left: 271px;" class="maindivstyle">
               <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblfd" runat="server" Width="130" Style="left: 30px;">From Date</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtfd" runat="server" AutoPostBack="true" CssClass="textbox textbox1 txtheight"
                                OnTextChanged="txtfd_TextChanged" >
                            </asp:TextBox>
                            <asp:CalendarExtender ID="calefd" TargetControlID="txtfd" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="txtext" runat="server" TargetControlID="txtfd" FilterType="Custom,Numbers"
                                ValidChars="31/12/2100" />
                        </td>
                        <td>
                            <asp:Label ID="lbltd" runat="server" Width="110">To Date</asp:Label>
                            <asp:TextBox ID="txttd" runat="server" AutoPostBack="true" CssClass="textbox textbox1 txtheight"
                                OnTextChanged="txttd_TextChanged" >
                            </asp:TextBox>
                            <asp:CalendarExtender ID="caletd" TargetControlID="txttd" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txttd"
                                FilterType="Custom,Numbers" ValidChars="31/12/2100" />
                            <span id="spanerr" runat="server" style="color: Red;" visible="false">*</span>
                        </td>
                        <td>
                            <span id="spandays" runat="server" visible="true">
                                <asp:Label ID="lbldays" runat="server" Width="100">Days</asp:Label>
                                <asp:TextBox ID="txtdays" runat="server" Width="50px" Style="top: 10px; left: 250px;"
                                    AutoPostBack="true" CssClass="textbox textbox1" ReadOnly="true" />
                                     <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txtdays"
                                            FilterType="numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                            </span>
                            <center>
                                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 20px;
                                    left: 0px;">
                                    <center>
                                        <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 0px;
                                            border-radius: 10px;">
                                            <center>
                                                <br />
                                                <table style="height: 100px; width: 100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lblalerterr" runat="server" Text="Enter Valid Date " Style="color: Red;"
                                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <center>
                                                                <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                                    Text="Ok" runat="server" OnClick="btnerrclose_Click" />
                                                            </center>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </center>
                                </div>
                            </center>
                            <%-- <asp:Label ID="lblerr"  runat="server" ForeColor="Red" Visible="false" Font-Bold="true"                                                 Font-Size="Medium" Text="Please Enter After Date to From Date" ></asp:Label>--%>
                        </td>
                        <td>
                            <asp:Button ID="btn_go_event" Text="Add Details" Visible="false" CssClass="btn2 textbox textbox1" runat="server"
                                OnClick="btn_go_event_Click" />
                        </td>
                    </tr>
                </table>
                </div> 
                <%--  <%--</div>--%>
               
            </div>
                    <br />

                     <table style="margin-left:8px; margin-top: -8px;" class="maindivstyle" width="900px">
                    <tr>
                        <td>
                            <asp:RadioButton ID="rdb_Papers" Text="Papers Published" GroupName="ii" runat="server"
                                AutoPostBack="true" OnCheckedChanged="chkselect_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdb_Paper" Text="Paper Presented" GroupName="ii" runat="server"
                                AutoPostBack="true" OnCheckedChanged="cb_pap_prsnt_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdb_Patents" runat="server" GroupName="ii" Text="Patents" AutoPostBack="true"
                                OnCheckedChanged="chkaward_CheckedChanged" />
                        </td>
                        <td >
                            <asp:RadioButton ID="rdb_Conference" Visible="true" runat="server" GroupName="ii" Text="Conference"
                                AutoPostBack="true" OnCheckedChanged="CheckBox2_CheckedChanged" />
                        </td>
                         <td>
                            <asp:RadioButton ID="rdb_seminor" runat="server" GroupName="ii" Text="Seminar"
                                AutoPostBack="true" OnCheckedChanged="rdb_seminor_CheckedChanged" />
                        </td>
                         <td>
                            <asp:RadioButton ID="rdb_workshop" runat="server" GroupName="ii" Text="WorkShop"
                                AutoPostBack="true" OnCheckedChanged="rdb_workshop_CheckedChanged" />
                        </td>
                     
                        <td>
                            <asp:RadioButton ID="rdb_Award" runat="server" GroupName="ii" Text="Award details"
                                AutoPostBack="true" OnCheckedChanged="rdb_Award_CheckedChanged" />
                        </td>
                           </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rdb_student" runat="server" GroupName="ii" Text="student intership"
                                AutoPostBack="true" OnCheckedChanged="rdb_student_CheckedChanged" />
                        </td>
                    
                        <td>
                            <asp:RadioButton ID="rdb_ReSearch" runat="server" GroupName="ii" Text="ReSearch"
                                AutoPostBack="true" OnCheckedChanged="rdb_ReSearch_CheckedChanged" />
                        </td>
                        <td id="mem" runat="server" visible="false">
                            <asp:RadioButton ID="rdb_Membership" runat="server" GroupName="ii" Text="Membership"
                                AutoPostBack="true" OnCheckedChanged="rdb_Membership_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdb_Distinguished" runat="server" GroupName="ii" Text="Visitors"
                                AutoPostBack="true" OnCheckedChanged="rdb_Distinguished_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdb_Tournamentk" runat="server" GroupName="ii" Text="Tournament"
                                AutoPostBack="true" OnCheckedChanged="rdb_Tournamentk_CheckedChanged" />
                        </td>
                       <%-- <td>
                            <asp:RadioButton ID="rdb_EVENTS" runat="server" GroupName="ii" Text="Events Organized"
                                AutoPostBack="true" OnCheckedChanged="rdb_EVENTS_CheckedChanged" />
                        </td>--%>
                        <td>
                            <asp:RadioButton ID="rdb_Symposium" runat="server" GroupName="ii" Text="Symposium "
                                AutoPostBack="true" OnCheckedChanged="rdb_Symposium_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdb_gust" runat="server" GroupName="ii" Text="Guest Lectures"
                                AutoPostBack="true" OnCheckedChanged="rdb_gustCheckedChanged" />
                        </td>
                          <td>
                            <asp:RadioButton ID="RDB_OTHERS" runat="server" GroupName="ii" Text="Others "
                                AutoPostBack="true" OnCheckedChanged="RDB_OTHERS_CheckedChanged" />
                        </td>
                    </tr>
                </table>

                <br />
                  <div id="pop_radiodiv" runat="server" visible="false" style="border: 1px solid silver; border-radius: 10px; font-size: medium; margin-left: 15px;
                margin-top: -11px; " class="maindivstyle div1">
              <br />
                    <br />
                    <div style="margin-left: -22px; margin-top: -28px; width: 200px; height: 36px">
                        <asp:Label ID="lbl_heading" runat="server" ForeColor="Green"></asp:Label>
                    </div>
                    <div id="rdb_nat_int" runat="server" style="margin-left: -451px; margin-top: 20px; width: 200px; height: 36px" class="maindivstyle">
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdb_popnational" runat="server" Text="National" GroupName="cc"
                                        Checked="true" />
                                    <asp:RadioButton ID="rdb_popinternatioanl" runat="server" Text="International" GroupName="cc" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div id="DDDD" runat="server" style="margin-left: 188px; margin-top: -61px; width: 420px; height: 36px" class="maindivstyle">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="txt_poprdo_title" Text="Title" runat="server" Width="60px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_popuptitle" runat="server" onchange="titleevent(this)"
                                        CssClass="ddlheight4 textbox textbox1">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_poprd_title" Style="display: none;" onfocus="return myFunction(this)"
                                        CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div id="div_Published" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_jour" Text="Journal" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_jour" runat="server" CssClass="textbox1 textbox txtheight4"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender65" runat="server" TargetControlID="txt_jour"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_impact" Text="Impact Factor" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_impact" runat="server" CssClass="textbox1 textbox txtheight4"></asp:TextBox>
                                       <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txt_impact"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_present" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_confn" runat="server" Text="Details Of Conference Proceedings"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_confn" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="txt_confn"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_Patents" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_patentnumb" Text="Patents Number" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_patentnumb" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txt_patentnumb"
                                                FilterType="Numbers,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_patentappno" Text="Application No" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_patentappno" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txt_patentappno"
                                                FilterType="Numbers,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_patenappdate" Text="Application Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_patenappdate" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender14" TargetControlID="txt_patenappdate"
                                        runat="server" CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_patentappstatus" Text="Application Status" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_patentappstatus" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_patentappstatus"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_Conference" runat="server" visible="false">
                       
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_seminar" runat="server" Text="Seminar Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_seminar" runat="server" CssClass="ddlheight5 textbox textbox1"
                                        onchange="seminar(this)">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_seminar" Style="display: none;" CssClass="textbox textbox1 txtheight5"
                                        runat="server" onfocus="return myFunction(this)"></asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txt_seminar"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </div>

                      <div id="div_seminar" runat="server" visible="false">
                      <table>
                      <tr>
                      <td>
                      <asp:Label ID="lbl_seminr" Text="ConferenceType" runat="server"></asp:Label></td>
                      <td>
                      <asp:TextBox ID="txt_seminartit" runat="server" CssClass="textbox txtheight4 txtheight"></asp:TextBox>
                       <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_seminartit"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                      </td>
                      </tr>
                      </table>
                      </div>
                       <div id="div_workshop" runat="server" visible="false">
                      <table>
                      <tr>
                      <td>
                      <asp:Label ID="lbl_workshop" Text="WorkShop Type" runat="server"></asp:Label></td>
                      <td>
                      <asp:TextBox ID="txt_workshop" runat="server" CssClass="textbox txtheight4 txtheight"></asp:TextBox>
                       <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_workshop"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                      </td>
                      </tr>
                      </table>
                      </div>
                    <div id="div_Award" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_awadcat" runat="server" Text="Award Category"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_awdcat" runat="server" CssClass="ddlheight5 textbox textbox1"
                                        onchange="Award(this)">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_awdcat" Style="display: none;" CssClass="textbox textbox1 txtheight4"
                                        onfocus="return myFunction(this)" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_won" runat="server" Text="Prize Won"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_won" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txt_won"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_intership" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_dur" runat="server" Text="Duration"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_dur" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txt_won"
                                                FilterType="Numbers,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_work" runat="server" Text="Nature oF Work"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_work" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                       <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" TargetControlID="txt_work"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_research" runat="server" visible="false">
                        <center>
                            <asp:RadioButton ID="rdo_enroll" Text="Enrollment" runat="server" GroupName="jj" />
                            <asp:RadioButton ID="rdo_awad" Text="Award" runat="server" GroupName="jj" /></center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_scholr" runat="server" Text="Name Of The Scholar"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_shlr" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" TargetControlID="txt_shlr"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_nameofprg" runat="server" Text="Name Of The Program"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_nameofprg" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="txt_nameofprg"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_mainsuper" runat="server" Text="Main Supervisor"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_supervisormain" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" TargetControlID="txt_supervisormain"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_co_sup" runat="server" Text="CO-Supervisor"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_co_sup" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" TargetControlID="txt_co_sup"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_Membership" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_society" runat="server" Text="Name Of The Society "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_society" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" runat="server" TargetControlID="txt_society"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_membership" runat="server" Text="Membership Detail"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_membership" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                       <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender31" runat="server" TargetControlID="txt_membership"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_dis_vist" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_namevist" runat="server" Text="Name Of The Visitor "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_namevist" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender32" runat="server" TargetControlID="txt_namevist"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_org" runat="server" Text="Organization"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_org" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender33" runat="server" TargetControlID="txt_org"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_pur_vist" runat="server" Text="Purpose Of Visitor"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_pur_vist" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender34" runat="server" TargetControlID="txt_pur_vist"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_Tournament" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Tournament" runat="server" Text="Tournament Type "></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_Tournament" runat="server" CssClass="ddlheight5 textbox textbox1"
                                        onchange="tour(this)">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Tournament" Style="display: none;" onfocus="return myFunction(this)" CssClass="textbox textbox1 txtheight4" runat="server"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender35" runat="server" TargetControlID="txt_Tournament"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_nametour" runat="server" Text="Name Of The Tournament "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_nametour" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender36" runat="server" TargetControlID="txt_nametour"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_game" runat="server" Text="Name Of The Game "></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_game" runat="server" CssClass="ddlheight5 textbox textbox1"
                                        onchange="game(this)">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_game" Style="display: none;" CssClass="textbox textbox1 txtheight4"
                                        runat="server" onfocus="return myFunction(this)"></asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender37" runat="server" TargetControlID="txt_game"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                         <div id="div_rdo_others" runat="server" visible="false">
                         <table>
                         <tr>
                         <td>
                         <asp:Label ID="lbl_rdo_othe_action" runat="server" Text="Action Name"></asp:Label>
                         </td>
                         <td><asp:Button ID="btn_actadd" CssClass="btn textbox textbox1" runat="server" Text="+" OnClick="btn_actadd_Click" />
                                <asp:DropDownList ID="ddl_actname" runat="server" CssClass="ddlheight3 textbox textbox1" AutoPostBack="true" OnSelectedIndexChanged="ddl_actname_SelectedIndexChanged"></asp:DropDownList>
                                <asp:Button ID="btn_actmin" CssClass="btn textbox textbox1" runat="server" Text="-" OnClick="btn_actmin_Click" /></td>
                         </tr>
                         </table>
                         </div>
                         <div id="div_gustt" runat="server" visible="false">
                         <table>
                         <tr>
                         <td>
                         <asp:Label ID="lbl_gdttil" runat="server" Text="Title"></asp:Label>
                         </td>
                         <td><asp:TextBox ID="txt_gsttit" runat="server" CssClass="textbox txtheight5 textbox1"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_gsttit"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender></td>
                         </tr>
                         </table>
                         </div>
                    <br />
                    <center>
                   
                   
                      <asp:Button ID="btn_rdogo" runat="server" Visible="false" Text="Go" OnClick="btn_rdogo_Click" CssClass="btn1 textbox textbox1" />
                     </center>
                </div>
              <br />
                    <div style="border: 1px solid silver; border-radius: 10px; font-size: medium; margin-left: 15px;
                        top: 20px; width: 900px; height: 60px" class="maindivstyle">
                        <br />
                        <table style="margin-top: -9px; margin-left: -213px">
                            <tr>
                                <td>
                                    <asp:Label ID="lblname" runat="server" Style="top: 15px; font-family: 'Book Antiqua'"
                                        Text="Event Name"> </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlname" Visible="false" runat="server" Style="top: 10px; width: 200px; left: 425px;"
                                        CssClass="textbox textbox1 ddlheight2" onchange="eventname(this)">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtothers" CssClass="textbox textbox1 txtheight6" onfocus="return myFunction(this)"
                                        runat="server">
                                    </asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <div style="margin-left: 533px; margin-top: -39px; width: 220px; height: 36px" class="maindivstyle">
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdb1" runat="server" Text="In Campus" AutoPostBack="true" GroupName="place"
                                                Checked="true" OnCheckedChanged="rdb1_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdb2" runat="server" Text="Out Campus" AutoPostBack="true" GroupName="place"
                                                OnCheckedChanged="rdb2_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                
            </div>
            <br />
            <br />
            <br />
            <br />
            <br />
           
               <div id="div_orgindoor" visible="false" runat="server" style="margin-left: 13px; margin-top: -76px; width: 900px; height: 90px;background-color:#CECECE" class="maindivstyle">
    <table class="maindivstyle" style="margin-left: -793px; margin-top: 4px; height:36px">
    <tr>
    <td><asp:Label ID="lbl_orgn" Text="Organized By" runat="server"></asp:Label></td>
    </tr>
     <tr><td><asp:RadioButton ID="rdo_org_staff" Text="Staff" runat="server" GroupName="eee" AutoPostBack="true" OnCheckedChanged="rdo_org_staff_Checkedchange"  /></td></tr>
    <tr><td><asp:RadioButton ID="rdo_orgstudent" Text="Student" runat="server" GroupName="eee"  AutoPostBack="true"  Checked="true" OnCheckedChanged="rdo_orgstudent_Checkedchange" /></td></tr>
    
    </table>
    <table class="maindivstyle" style="margin-left: 108px; margin-top: -78px;">
    <tr>
    <td><asp:Label ID="lbl_org_batch" Text="Batch" runat="server" ></asp:Label></td>
    <td>
    <asp:DropDownList ID="ddl_org_batch" runat="server" CssClass="ddlheight textbox textbox1" OnSelectedIndexChanged="ddl_org_batch_SelectedIndexChanged"></asp:DropDownList>
    </td>

      <td>
                                <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_degree_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_degree"
                                            PopupControlID="p3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_branch_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch"
                                            PopupControlID="p4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            </tr>
                            <tr>
                            <td>
                                <asp:Label ID="lbl_org_sem" Text="Semester" runat="server"></asp:Label>
                            </td>
                             <td>
                                <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_org_sem" runat="server" CssClass="textbox textbox1 txtheight3"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_or_sem" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_or_sem_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_or_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_or_sem_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_org_sem"
                                            PopupControlID="Panel11" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            <asp:Label ID="lb_org_staffname" runat="server" Text="Staff Name" Visible="false"></asp:Label>
                            <asp:Label ID="lbl_org_studnamee" runat="server" Text="Student Name" Visible="false"></asp:Label>
                            </td>
                            <td>
                            <asp:UpdatePanel ID="UpdatePanel25" runat="server" Visible="false">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_staffnamemul" runat="server" CssClass="textbox  txtheight3">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel12" runat="server" CssClass="multxtpanel" style="height:200px;width:261px;">
                                        <asp:CheckBox ID="cb_staff_name" runat="server" OnCheckedChanged="cb_staff_name1_CheckedChange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_staff_name" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cb_staff_name1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender21" runat="server" TargetControlID="txt_staffnamemul"
                                        PopupControlID="panel12" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="false">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_studentorgby" runat="server" CssClass="textbox  txtheight3">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel5" runat="server" CssClass="multxtpanel" style="height:200px;width:261px;">
                                        <asp:CheckBox ID="cb_studentorgby" runat="server" OnCheckedChanged="cb_studentorgby_CheckedChange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cb1_studentorgby" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cb1_studentorgby_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_studentorgby"
                                        PopupControlID="panel5" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            </td>
                             <td>
                                <asp:Label ID="lblissueperson" runat="server" Text="Search" Visible="false"></asp:Label>
                                 <asp:Label ID="lbl_orgstudentname" runat="server" Text="Search" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtissueper" runat="server" CssClass="textbox1 textbox txtheight5" Visible="false"> </asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getstaffname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtissueper"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                 <asp:TextBox ID="txt_studenorgsearch" runat="server" Width="230px" CssClass="textbox1 textbox txtheight4" Visible="false"> </asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studenorgsearch"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                </td>
    </tr></table>
    </div>
     <div id="div_orgoutdoor" visible="false" runat="server" style="margin-left: 13px; margin-top: -78px; width: 900px; height: 120px" class="maindivstyle">
     <table class="maindivstyle" style="margin-left: -787px; margin-top: 4px; height:36px">
    <tr>
    <td><asp:Label ID="lbl_outorg" Text="Organized By" runat="server"></asp:Label></td>
    </tr>
    </table>
     <table class="maindivstyle" style="margin-left: 113px; margin-top: -36px; width:680px">
    <tr>
    <td>
    <asp:Label ID="lbl_outinstitution" runat="server" Text="Institution" ></asp:Label>
    </td>
    <td><asp:DropDownList ID="ddl_outinstitution" runat="server" CssClass="ddlheight5 textbox textbox1" 
onchange="outinstitution(this)"></asp:DropDownList>
</td>
    <td><asp:TextBox ID="txt_outinstitution" runat="server" CssClass="textbox textbox1 txtheight5" style="display:none;" onfocus="return myFunction(this)"></asp:TextBox>
     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_outinstitution"
                                                FilterType="Custom,UppercaseLetters,LowercaseLetters" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender></td>
                                            </tr>
                                            <tr>
     <td>
    <asp:Label ID="lbl_outorganiser" runat="server" Text="Organiser Name " ></asp:Label>
    </td>
    <td><asp:DropDownList ID="ddl_outorganiser" runat="server" CssClass="ddlheight5 textbox textbox1" 
onchange="outorgnzname(this)"></asp:DropDownList></td>
    <td><asp:TextBox ID="txt_outorganz" runat="server" CssClass="textbox textbox1 txtheight5" style="display:none;" onfocus="return myFunction(this)"></asp:TextBox>
     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_outorganz"
                                                FilterType="Custom,UppercaseLetters,LowercaseLetters" ValidChars=" .">
                                            </asp:FilteredTextBoxExtender></td>
    </tr>
    <tr>
     <td>
    <asp:Label ID="lbl_outloc" runat="server" Text="Address" ></asp:Label>
    </td>
    <td colspan="2"><asp:TextBox ID="txt_otloc" runat="server" CssClass="textbox textbox1 txtheight6"></asp:TextBox>
     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txt_otloc"
                                                FilterType="Custom,UppercaseLetters,LowercaseLetters,numbers" ValidChars=" ,/">
                                            </asp:FilteredTextBoxExtender></td></tr>
    </table>
     </div>

              <div  id="pop_Gv1_div" runat="server" visible="false" style="border: 1px solid silver; border-radius: 10px; font-size: medium; margin-left: 15px;
                margin-top: 10px; width: 900px; height: 229px" class="maindivstyle">
                    <br />
                    <div style="width: 810px; height: 200px; overflow: auto;">
                        <asp:GridView ID="GV1" runat="server" Visible="true" AutoGenerateColumns="false"
                            OnRowDataBound="GV1_OnRowDataBound" GridLines="Both" OnRowCommand="grid_edulevel_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtdate" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>' CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Start Time" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_start" ReadOnly="true" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="End Time" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_end" ReadOnly="true" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Start Period" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_st_prd" runat="server" ReadOnly="true" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="End Period" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_end_prd" runat="server" ReadOnly="true" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                    <center>
                                        <asp:TextBox ID="txt" runat="server" ReadOnly="true" CssClass="textbox txtheight2"></asp:TextBox>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Event" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbladdd" runat="server" Text="Edit"></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
              
            </div>

          

 

         <div id="pop_minute" runat="server" visible="false" style="border: 1px solid silver; border-radius: 10px; font-size: medium; margin-left: 15px;
                margin-top: 10px;border-style:double" class="maindivstyle div1">
                    <br />
                    <span style="color:Green; font-size:x-large">Minute To Minute Program</span>
                    <br />
                    <br />
                 
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_min_startdate" Text="Start Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_min_startdate" CssClass="textbox textbox1 txtheight" runat="server" AutoPostBack="true" OnTextChanged="txt_min_startdate_changed"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender12" TargetControlID="txt_min_startdate"
                                        runat="server" CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_min_enddate" Text="Event End Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_min_enddate" CssClass="textbox  txtheight" OnTextChanged="txt_min_enddate_Changed"
                                        AutoPostBack="true" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender13" TargetControlID="txt_min_enddate" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_min_starttime" Text="Start Time" runat="server"></asp:Label>
                                </td>
                                <td>
                                   <asp:DropDownList ID="ddl_hour1" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl_minits1" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl_seconds1" Width="50px" Height="25px" Visible="false" runat="server"
                                                CssClass="textbox textbox1">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl_timeformate1" Width="50px" Height="25px" runat="server"
                                                CssClass="textbox textbox1">
                                                <asp:ListItem>AM</asp:ListItem>
                                                <asp:ListItem>PM</asp:ListItem>
                                            </asp:DropDownList>
                                </td>
                                <td></td>
                                <td>
                                    <asp:Label ID="lbl_min_endtime" Text="End Time" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_endhour1" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl_endminit1" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl_endsecnonds1" Width="50px" Height="25px" Visible="false"
                                                runat="server" CssClass="textbox textbox1">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl_endformate1" Width="50px" Height="25px" runat="server" CssClass="textbox textbox1">
                                                <asp:ListItem>AM</asp:ListItem>
                                                <asp:ListItem>PM</asp:ListItem>
                                            </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_min_startperiod" Text="Start Period" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_min_startperiod" CssClass="textbox textbox1 txtheight" MaxLength="1" runat="server"></asp:TextBox>
                                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender60" runat="server" TargetControlID="txt_min_startperiod"
                                                FilterType="Numbers" ValidChars="">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                                 <td></td>
                                <td>
                                    <asp:Label ID="lbl_min_endperiod" Text="End Period" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_min_endperiod" CssClass="textbox textbox1 txtheight" MaxLength="1" runat="server"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender61" runat="server" TargetControlID="txt_min_endperiod"
                                                FilterType="Numbers" ValidChars="">
                                            </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_min_location" Text="Location" runat="server"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txt_min_location" CssClass="textbox textbox1 txtheight5" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txt_min_location"
                                        FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ,-">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnk_min_loc" runat="server" Text="Location" OnClick="lnk_min_loc_Click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_min_action" Text="No of Action" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_min_action" CssClass="textbox textbox1" 
                                        OnTextChanged="txt_min_action_Changed" runat="server" Width="40px" ReadOnly="false"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_min_action"
                                            FilterType="numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btn_min_Action" runat="server" BackColor="#CECECE" OnClick="btn_min_Action_Click" CssClass="btn1 textbox textbox1" Text="Go" />
                                </td>
                                <td><asp:Label ID="lbl_action_name" Visible="false" runat="server" Text="Action Name"></asp:Label></td>
                                
                            </tr>
                            <tr><td colspan="2"><asp:RadioButton ID="rdo_commpati" runat="server" GroupName="c1" Checked="true" Text="Common Participate" />
                          <%--  </td>
                            <td>--%>
                             <asp:LinkButton ID="lnk_com_particate" Visible="false" runat="server" OnClick="lnk_com_particate_Click" Text="Participant"></asp:LinkButton>
                            </td>
                            <td>   
                            <asp:RadioButton ID="rdo_indivparti" runat="server" GroupName="c1" Text="Individual Action Participate" /> 
                           </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <div style="width: 810px; height: 200px; overflow: auto;">
                        <asp:GridView ID="gridadd" runat="server" Visible="true" AutoGenerateColumns="false"
                            OnRowDataBound="gridadd_OnRowDataBound" GridLines="Both" OnRowCommand="gridadd_edulevel_RowCommand" OnRowDeleting="OnRowDeleting_gridadd">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtactname" ReadOnly="true" runat="server" Text=""  CssClass="textbox txtheight4"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Description" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_descri" ReadOnly="true" runat="server" CssClass="textbox txtheight5"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                              
                                <asp:TemplateField HeaderText="Start Time" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_start" runat="server" Text='<%#Eval("Dummy3") %>' onchange="checkForm(this)" CssClass="textbox txtheight" placeholder="Ex: 12:00:AM"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="End Time" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_end" runat="server" Text='<%#Eval("Dummy4") %>' onchange="checkForm(this)" CssClass="textbox txtheight" placeholder="Ex: 12:00:PM"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>   
                                 <asp:TemplateField HeaderText="Location" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_loc" runat="server" ReadOnly="true"  CssClass="textbox txtheight3" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                <asp:TemplateField HeaderText="No Of People Participating" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_noact" ReadOnly="true" runat="server"  CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField HeaderText="No Of People Who Are Conducting The Event" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_noconper" ReadOnly="true" runat="server"  CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>                                                                   
                                <asp:TemplateField HeaderText="Event" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbladdd" runat="server" Text="Edit"></asp:Label>
                                           
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                 <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                
            
                </div>
          
                <center>
                    <div id="pop_bul_flr_room" runat="server" class="popupstyle popupheight1" visible="false" style="height:1950px">
                        <asp:ImageButton ID="ImageButton11" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 736px; margin-left: 423px;"
                           OnClick="pop_bul_flr_roomclose_Click" />
                        <br />
                        <div style="background-color: White; height: 150px; width: 870px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px; margin-top: 736px">
                            <br />
                            <span class="fontstyleheader" style="color: #008000;">Location</span>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblbuildname" Text="Building Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Upp6" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_buildingname" runat="server" CssClass="textbox textbox1 txtheight3"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="150px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_buildname" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_buildname_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_buildname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_buildname_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_buildingname"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_floorname" Text="Floor Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Upp7" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_floorname" runat="server" CssClass="textbox txtheight3 textbox1"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="p6" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="150px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_floorname" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_floorname_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_floorname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_floorname_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_floorname"
                                                    PopupControlID="p6" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_roomname" Text="Room Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Upp8" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_roomname" runat="server" CssClass="textbox txtheight3 textbox1"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="p7" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="250px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_roomname" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_roomname_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_roomname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblroomname_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_roomname"
                                                    PopupControlID="p7" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <center><asp:Button ID="btn_bul_flr_select" runat="server" Text="Go" OnClick="btn_bul_flr_select_Click" CssClass="btn1 textbox textbox1" /></center>
                        </div>
                    </div>
                </center>
              

            
        <%--  ****************************************************************     --%>
    
            <%-- *********************** end Expense **************8--%>
            <div id="poprdoview" runat="server" visible="false" style="border: 1px solid silver; border-radius: 10px; font-size: medium; margin-left: 15px;
                margin-top: 10px; width: 900px; height: 230PX" class="maindivstyle">
                   
                     
                            <br />
                            <span class="fontstyleheader" style="color:#008000;">Add Details</span>
                            <br />
            <br />
               <table><tr><td>
               <asp:Label ID="lbl_act_namenew" runat="server" Text="Action Name"></asp:Label></td>
               <td><asp:DropDownList ID="ddl_act_namenew" runat="server" CssClass="ddlheight4 textbox textbox1" onchange="actinnam(this)" onfocus="return myFunction(this)"></asp:DropDownList></td>
               <td><asp:TextBox ID="txt_act_namenew" runat="server" CssClass="textbox textbox1 txtheight4" Style="display: none;" onfocus="return myFunction(this)"  ></asp:TextBox></td>
               </tr>
               <tr>
               <td>
               <asp:Label ID="lbl_act_description" runat="server" Text="Description"></asp:Label>
               </td>
               <td>
               <asp:TextBox ID="txt_act_description" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender85" runat="server" TargetControlID="txt_act_description"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
               </td>
               </tr></table>
               <br />
               <div style="margin-right:614px; width:400px" class="maindivstyle">
                <table >
                <tr><td>
                    <asp:LinkButton ID="lnk_patici" Text="Participant Person" runat="server" OnClick="lnk_patici_Click"></asp:LinkButton>
                    </td>
                    <td style="width:10px;"></td>
                    <td>
                    <asp:LinkButton ID="lnk_prest" Text="Presented Person" runat="server" OnClick="lnk_prest_Click"></asp:LinkButton>
                    </td>
                     <td style="width:10px;"></td>
                    <td>
                     <asp:LinkButton ID="lnk_locationmul" Visible="false" Text="Location" runat="server" OnClick="lnk_locationmul_Click"></asp:LinkButton>
                     </td>
                     </tr>
                     </table>
                     </div>
                     <center><asp:Button ID="btn_go_addddnew" runat="server" Text="Go" CssClass="textbox textbox1 btn1" OnClick="btn_go_addddnew_Click" /></center>

            <%--    *****************************************--%>

       

                
                </div>
          
         
                 <div style="margin-left: -649px; margin-top: 35px; width: 230px; height: 36px" class="maindivstyle">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lb_prerequest" Text="PreRequestToEvent" runat="server"></asp:Label>
                            </td>
                             <td>
                               <asp:Button ID="btn_prerequest_addnew" runat="server" BackColor="#CECECE"  Text="Add New" CssClass="textbox textbox1 btn2" OnClick="btn_prerequest_addnew_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
              <%--  *******************************************--%>
                <div id="divprte" runat="server" visible="false" style="border: 1px solid silver;
                    border-radius: 10px; font-size: medium; margin-left: 239px; margin-top: -43px;
                    width: 635px; height: 270px" class="maindivstyle">
                    <br />
                    <table class="maindivstyle"><tr><td>
                 <asp:Label ID="lbl_pre_action" runat="server" Text="Activity"></asp:Label>
                 </td>
                 <td><asp:TextBox ID="txt_pre_action" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox></td>
                  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_pre_action"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                   <td colspan="2"><asp:Label ID="lbl_pre_actname" runat="server" Text="Description"></asp:Label></td>
                 <td><asp:TextBox ID="txt_pre_ctname" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_pre_ctname"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender></td>
                 </tr>
                 <tr><td><asp:Label ID="lbl_pre_startdate" runat="server" Text="Start Date"></asp:Label></td>
                 <td colspan="2"><asp:TextBox ID="txt_pre_Startdate" runat="server" CssClass="textbox textbox1 txtheight" AutoPostBack="true" OnTextChanged="txt_pre_Startdate_TextChanged"></asp:TextBox>
                   <asp:CalendarExtender ID="CalendarExtender15" TargetControlID="txt_pre_Startdate" runat="server"
                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                 </td>

                 <td><asp:Label ID="lbl_pre_enddate" runat="server" Text="End Date"></asp:Label></td>
                 <td><asp:TextBox ID="txt_pre_enddate" runat="server" CssClass="textbox textbox1 txtheight" AutoPostBack="true" OnTextChanged="txt_pre_enddate_TextChanged"></asp:TextBox>
                   <asp:CalendarExtender ID="CalendarExtender16" TargetControlID="txt_pre_enddate" runat="server"
                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender></td></tr>

                  <tr>
               
                 <td><asp:Label ID="lbl_pre_repby" runat="server" Text="Represented By" Width="100px"></asp:Label></td>
                 <td colspan="2"><asp:TextBox ID="txt_pre_repby" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_pre_repby"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=".- ">
                                            </asp:FilteredTextBoxExtender>
                  <asp:AutoCompleteExtender ID="AutoCompleteExtender20" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getstaffname" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pre_repby"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>

                                    <asp:TextBox ID="txt_pre_repbystud" runat="server" CssClass="textbox textbox1 txtheight5" Visible="false"></asp:TextBox>
                  <asp:AutoCompleteExtender ID="AutoCompleteExtender32" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pre_repbystud"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                 </td>
                 <td colspan="2">
                 <asp:RadioButton ID="rdo_pre_staff" Text="Staff" runat="server" GroupName="pre" Checked="true" AutoPostBack="true" OnCheckedChanged="rdo_pre_staff_CheckedChanged" />
                
                  <asp:RadioButton ID="rdo_pre_stud" Text="Student" runat="server" GroupName="pre" AutoPostBack="true" OnCheckedChanged="rdo_pre_stud_CheckedChanged" />
                 <%-- </td>
               <td>--%>
                 <asp:Button ID="btn_pre_add" runat="server" Text="Go" CssClass="btn1 textbox textbox1" OnClick="btn_pre_add_Click" /></td></tr>
                 </table>
                 <br />
                       <div id="GridView4_div" runat="server" visible="false" style="width: 600px; height: 100px; overflow: auto;">
                        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="false" OnRowCommand="grid4_Rowcommand" GridLines="Both"  OnRowDeleting="OnRowDeleting_GridView4">
                            
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno3" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Activity" HeaderStyle-BackColor="#0CA6CA"
                                    HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtact" ReadOnly="true" Text='<%#Eval("Dummy") %>' runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Start Date" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_startdate" ReadOnly="true" Text='<%#Eval("Dummy1") %>' runat="server" CssClass="textbox txtheight3"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="End Date" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_enddate" ReadOnly="true" Text='<%#Eval("Dummy2") %>' runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Activity Name" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_actname" ReadOnly="true" Text='<%#Eval("Dummy3") %>' runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Represented By" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_repsen" ReadOnly="true" Text='<%#Eval("Dummy4") %>' runat="server" CssClass="textbox txtheight3"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                <div style="margin-left: -649px; margin-top: 35px; width: 230px; height: 36px" class="maindivstyle">
                    <table>
                        <tr>
                            <td>                               
                                     <asp:Label ID="lbl_matrlrequest" Text="MaterialsRequested" runat="server"></asp:Label>
                            </td>
                             <td>
                               <asp:Button ID="btn_matrlrequest" runat="server" Text="Add New"  BackColor="#CECECE" CssClass="textbox textbox1 btn2" OnClick="btn_matrlrequest_Click" />
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="divmr" runat="server" visible="false" style="border: 1px solid silver; border-radius: 10px;
                    font-size: medium; margin-left: 239px; margin-top: -47px; width: 635px; height: 270px"
                    class="maindivstyle">
              <br />
              <table class="maindivstyle">
              <tr>
              <td><asp:Label ID="lbl_mat_itemname" runat="server" Text="Item Name"></asp:Label></td>
              <td>
              <asp:TextBox ID="txt_mat_itemname" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
              <asp:AutoCompleteExtender ID="AutoCompleteExtender21" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getitemname" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_mat_itemname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="multxtpanel1">
                                    </asp:AutoCompleteExtender>
              <asp:Button ID="btn_itemlkup" runat="server" Text="?" CssClass="btn textbox textbox1" OnClick="btn_itemlkup_Click" /></td>
              </tr>
               <tr>
              <td><asp:Label ID="lbl_mat_qunty" runat="server" Text="Quantity"></asp:Label></td><td>
              <asp:TextBox ID="txt_mat_qunty" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
               <asp:FilteredTextBoxExtender ID="flter" runat="server" TargetControlID="txt_mat_qunty"
                                            FilterType="Numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender></td></tr>
               <tr>
              <td><asp:Label ID="lbl_mat_expect" runat="server" Text="Expected Date"></asp:Label></td><td>
              <asp:TextBox ID="txt_mat_expect" runat="server" CssClass="textbox textbox1 txtheight" AutoPostBack="true" OnTextChanged="txt_mat_expect_TextChanged"></asp:TextBox>
               <asp:CalendarExtender ID="CalendarExtender17" TargetControlID="txt_mat_expect" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                           
              </td></tr>
               <tr>
              <td><asp:Label ID="lbl_pur_status" runat="server" Text="Purchase Status"></asp:Label></td>
              <td>
              <asp:RadioButton ID="rdo_tobepur" Text="To Be Purchased" Checked="true" runat="server" GroupName="yy" />
              <asp:RadioButton ID="rdo_usepur" Text="Use Existing" runat="server" GroupName="yy" />
              </td>
              <td><asp:Button ID="btn_mat_go" runat="server" Text="Go" CssClass="btn1 textbox textbox1" OnClick="btn_mat_go_Click" /></td></tr>
              </table>
              <br />
                <div id="GridView5_div" runat="server" visible="false" style="width: 600px; height: 80px; overflow: auto;">
                        <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="false" GridLines="Both" OnRowCommand="grid5_Rowcommand"  OnRowDeleting="OnRowDeleting_GridView5">
                            
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno3" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Item Name" HeaderStyle-BackColor="#0CA6CA"
                                    HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_name" ReadOnly="true" Text='<%#Eval("Dummy") %>' runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_qty" ReadOnly="true" Text='<%#Eval("Dummy1") %>' runat="server" CssClass="textbox txtheight3"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expected" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_exp" ReadOnly="true" Text='<%#Eval("Dummy2") %>' runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Purchase Status" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tx_inmax" ReadOnly="true" Text='<%#Eval("Dummy3") %>' runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                 <div style="margin-left: -649px; margin-top: 35px; width: 230px; height: 36px" class="maindivstyle">
                 <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblexp" runat="server">
                       Sponser(Income):
                            </asp:Label>
                        </td>
                        <td>
                        <asp:Button ID="btn_expense" runat="server" Text="Add New" BackColor="#CECECE"  CssClass="btn2 textbox textbox1" OnClick="btn_expense_Click" />
                        </td>
                    </tr>
                </table>
                </div>
                  <div  id="div_expence" runat="server" visible="false" style="border: 1px solid silver; border-radius: 10px; font-size: medium; margin-left: 241px;
                margin-top: -37px; width: 610px; height: 350px" class="maindivstyle">
                
                
                <table>
                    <tr>
                        <%-- <td>--%>
                        <%-- <div style="margin-left: 20px; margin-top: 47px; width: 479px; height: 36px" class="maindivstyle">
                                    <tr>--%>
                        <td>
                            <asp:RadioButton ID="rdbinst" runat="server" Text="Institution" AutoPostBack="true"
                                GroupName="expense" Width="130" OnCheckedChanged="rdbinst_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdbdept" runat="server" Text="Association" Width="130" AutoPostBack="true"
                                GroupName="expense" OnCheckedChanged="rdbdept_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdbsponser" runat="server" Text="Individual" AutoPostBack="true"
                                Width="130" GroupName="expense" OnCheckedChanged="rdbsponser_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdbcompany" runat="server" Text="Company" AutoPostBack="true"
                                Width="130" GroupName="expense" OnCheckedChanged="rdbcompany_CheckedChanged" />
                        </td>
                        
                    </tr>
                </table>
                <br />
                  <div id="gv33div" runat="server" visible="false" style="margin-left: -3px; margin-top: -14px; width: 560px; height: 296px" class="maindivstyle">
                    <br />
                    <span class="fontstyleheader" style="color: #008000;">Add Institution</span>
                    <br />
                    <table class="maindivstyle">
                 <tr>
                 <td><asp:Label id="lbl_inst_name" runat="server" Text="Institution Name"></asp:Label> </td>
                 <td><asp:TextBox ID="txt_inst_name" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender52" runat="server" TargetControlID="txt_inst_name"
                                                FilterType="UppercaseLetters,LowercaseLetters" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                             <asp:AutoCompleteExtender ID="AutoCompleteExtender30" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getindiname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_inst_name"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender></td>
                 
                 </tr>
                 <tr><td>
                 <asp:Label ID="lbl_resourse" Text="Resource" runat="server"></asp:Label>
                 </td>
                 <td><asp:TextBox ID="txt_ins_resource" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender46" runat="server" TargetControlID="txt_ins_resource"
                                                FilterType="UppercaseLetters,LowercaseLetters" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender></td></tr>
                 <tr>
                 <td>
                 <asp:Label ID="lbl_ins_amount" runat="server" Text="Amount"></asp:Label></td>
                 <td><asp:TextBox ID="txt_ins_amount" CssClass="textbox textbox1 txtheight1" runat="server"></asp:TextBox>
                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender47" runat="server" TargetControlID="txt_ins_amount"
                                                FilterType="numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                 <asp:Button ID="btn_ins_go" runat="server" Text="Add" CssClass="btn1 textbox textbox1" OnClick="btn_ins_go_Click" /></td></tr>
                    </table>
                    <br />
                    <div style="width: 500px; height: 120px; overflow: auto; margin-left: -26px; margin-top: -14px;">
                        <asp:GridView ID="GridView3" Width="400px" runat="server" AutoGenerateColumns="false" GridLines="Both" OnRowCommand="grid3_Rowcommand" OnRowDeleting="OnRowDeleting_GridView3">
                            
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno3" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Institution Name" HeaderStyle-BackColor="#0CA6CA"
                                    HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtdname" ReadOnly="true" Text='<%#Eval("Dummy") %>' runat="server" CssClass="textbox txtheight5"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Resource" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtresource" ReadOnly="true" Text='<%#Eval("Dummy1") %>' runat="server" CssClass="textbox txtheight3"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtamt" ReadOnly="true" Text='<%#Eval("Dummy2") %>' style="text-align:right" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                           <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
               
                </div>

                  <div id="POP_GV3_DIV" runat="server" visible="false" style="margin-left: -3px; margin-top: -14px; width: 560px; height: 296px" class="maindivstyle">
              
                    <br />
                    <span class="fontstyleheader" style="color: #008000;">Add Association</span>
                    <br />
                    <table class="maintablestyle">
                    <tr>
                    <td><asp:Label ID="lbl_departmentname" runat="server" Text="Association"></asp:Label>
                    </td>
                    <td><asp:TextBox ID="txt_departmentname" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender53" runat="server" TargetControlID="txt_departmentname"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                              <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getassocname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_departmentname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                            </td></tr>
                     <tr>
                    <td><asp:Label ID="lbl_dept_resourcr" runat="server" Text="Resource"></asp:Label>
                    </td>
                    <td><asp:TextBox ID="txt_dept_resource" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                       <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender54" runat="server" TargetControlID="txt_dept_resource"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender></td></tr>
                      <tr>
                    <td><asp:Label ID="lbl_dept_amt" runat="server" Text="Amount"></asp:Label>
                    </td>
                    <td><asp:TextBox ID="txt_dept_amt" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender59" runat="server" TargetControlID="txt_dept_amt"
                                                FilterType="Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btn_dept_go" runat="server" Text="Add" CssClass="textbox textbox1 btn1" OnClick="btn_dept_go_Click" /></td></tr>
                    </table>
                    <br />
                  <div style="width: 600px; height: 120px; overflow: auto; margin-left: -26px; margin-top: -14px;">
                        <asp:GridView ID="GV3" runat="server" AutoGenerateColumns="false" GridLines="Both"
                            OnRowCommand="gv3_Rowcommand" OnRowDeleting="OnRowDeleting_gv3">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno3" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Association Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtdname" ReadOnly="true" Text='<%#Eval("Dummy") %>' runat="server" CssClass="textbox txtheight4"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Resource" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtresource" ReadOnly="true" Text='<%#Eval("Dummy1") %>' runat="server" CssClass="textbox txtheight3"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtamt" ReadOnly="true"  Text='<%#Eval("Dummy2") %>' style="text-align:right" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                
              
            </div>

             <div id="POP_GV4_DIV" runat="server" visible="false" style="margin-left: -3px; margin-top: -14px; width: 560px; height: 296px" class="maindivstyle">
                <br />
             
                    <span class="fontstyleheader" style="color: #008000;">Add Individual</span>
                    <br />
                         <table class="maintablestyle">
                    <tr>
                    <td><asp:Label ID="lbl_sponscmp_name" runat="server" Text="Company Name"></asp:Label>
                    </td>
                    <td><asp:TextBox ID="txt_sponscmp_name" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender48" runat="server" TargetControlID="txt_sponscmp_name"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                               <asp:AutoCompleteExtender ID="AutoCompleteExtender31" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getothername" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sponscmp_name"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                            </td></tr>
                     <tr>
                    <td><asp:Label ID="lbl_sponc_contact" runat="server" Text="Resource"></asp:Label>
                    </td>
                    <td><asp:TextBox ID="txt_sponc_contact" runat="server" CssClass="textbox textbox1 txtheight5" MaxLength="10"></asp:TextBox>
                       <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender49" runat="server" TargetControlID="txt_sponc_contact"
                                                FilterType="Numbers" ValidChars="">
                                            </asp:FilteredTextBoxExtender></td></tr>
                      <tr>
                    <td><asp:Label ID="lbl_sponc_amount" runat="server" Text="Amount"></asp:Label>
                    </td>
                    <td><asp:TextBox ID="txt_sponc_amount" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender50" runat="server" TargetControlID="txt_sponc_amount"
                                                FilterType="Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btn_spncmp_go" runat="server" Text="Add" CssClass="textbox textbox1 btn1" OnClick="btn_spncmp_go_Click" /></td></tr>
                    </table>
                    <br />
                   <div style="width: 600px; height: 320px; overflow: auto; margin-left: -26px; margin-top: -14px;">
                        <asp:GridView ID="GV4" runat="server" AutoGenerateColumns="false" GridLines="Both" Visible="false" OnRowCommand="GV4_Rowcommand" OnRowDeleting="OnRowDeleting_GV4">
                         
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno4" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcname" ReadOnly="true" Text='<%#Eval("Dummy") %>' runat="server" CssClass="textbox txtheight4"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Resources" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcnt" ReadOnly="true" Text='<%#Eval("Dummy1") %>' runat="server" CssClass="textbox txtheight3"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtamt1" ReadOnly="true" Text='<%#Eval("Dummy2") %>' style="text-align:right" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                   
                </div>

                  <div id="POP_GV6_DIV" runat="server" visible="false" style="margin-left: -3px; margin-top: -14px; width: 560px; height: 296px" class="maindivstyle">
                <br />
             
                    <span class="fontstyleheader" style="color: #008000;">Add Company</span>
                    <br />
                         <table class="maintablestyle">
                    <tr>
                    <td><asp:Label ID="lbl_spn_cmpy" runat="server" Text="Company Name"></asp:Label>
                    </td>
                    <td><asp:TextBox ID="txt_spn_cmpy" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                      <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender62" runat="server" TargetControlID="txt_spn_cmpy"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" &">
                                            </asp:FilteredTextBoxExtender>
                                             <asp:AutoCompleteExtender ID="autocompany" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcompname1" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_spn_cmpy"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender></td></tr>
                     <tr>
                    <td><asp:Label ID="lbl_sp_cont" runat="server" Text="Resource"></asp:Label>
                    </td>
                    <td><asp:TextBox ID="txt_sp_cont" runat="server" CssClass="textbox textbox1 txtheight5" MaxLength="10"></asp:TextBox>
                       <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender63" runat="server" TargetControlID="txt_sp_cont"
                                                FilterType="Numbers" ValidChars="">
                                            </asp:FilteredTextBoxExtender></td></tr>
                      <tr>
                    <td><asp:Label ID="lbl_sp_amt" runat="server" Text="Amount"></asp:Label>
                    </td>
                    <td><asp:TextBox ID="txt_sp_amt" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender64" runat="server" TargetControlID="txt_sp_amt"
                                                FilterType="Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btn_sp_add" runat="server" Text="Add" CssClass="textbox textbox1 btn1" OnClick="btn_sp_add_Click" /></td></tr>
                    </table>
                    <br />
                   <div style="width: 600px; height: 320px; overflow: auto; margin-left: -26px; margin-top: -14px;">
                        <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="false" GridLines="Both"  Visible="false" OnRowCommand="grid6_Rowcommand" OnRowDeleting="OnRowDeleting_GridView6">
                            
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno4" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcname" ReadOnly="true" Text='<%#Eval("Dummy") %>' runat="server" CssClass="textbox txtheight4"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contact" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcnt" ReadOnly="true" Text='<%#Eval("Dummy1") %>' runat="server" CssClass="textbox txtheight3"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtamt1" ReadOnly="true" Text='<%#Eval("Dummy2") %>' style="text-align:right" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                   
                </div>
          
            </div>

             

                  <div style="margin-left: -649px; margin-top: 35px; width: 230px; height: 36px" class="maindivstyle">
                 <table>
                    <tr>
                        <td>
                            <asp:Label ID="lb_expn" runat="server">
                    Expense
                            </asp:Label>
                        </td>
                        <td>
                        <asp:Button ID="btn_ex_ad" runat="server" Text="Add New" BackColor="#CECECE"  CssClass="btn2 textbox textbox1" OnClick="btn_ex_ad_Click" />
                        </td>
                    </tr>
                </table>
                </div>

                    <div  id="ex_new_div" runat="server" visible="false" style="border: 1px solid silver; border-radius: 10px; font-size: medium; margin-left: 241px;
                margin-top: -37px; width: 570px; height: 300px" class="maindivstyle">
                <br />
                <table class="maindivstyle">
                <tr>
                <td><asp:Label ID="lbl_expn_name" runat="server" Text="Expense Name"></asp:Label>
                </td>
                <td><asp:DropDownList ID="ddl_expnc_name" runat="server" CssClass="ddlheight4 textbox1 textbox" onchange="expnname(this)"></asp:DropDownList></td>
                <td><asp:TextBox ID="txt_expn_name" runat="server" CssClass="textbox textbox1 txtheight4" onfocus="return myFunction(this)" Style="display: none;" ></asp:TextBox>
                  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender86" runat="server" TargetControlID="txt_expn_name"
                                                FilterType="LowercaseLetters,UppercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender></td>
                </tr>
                <tr>
                
                 <td><asp:Label ID="lbl_expn_descrp" runat="server" Text="Description"></asp:Label>
                </td>
                <td><asp:TextBox ID="txt_expnc_descrp" runat="server" CssClass="textbox textbox1 txtheight4" ></asp:TextBox>
                </td>
                </tr>
                <tr>
                 <td><asp:Label ID="lbl_expnc_amt" runat="server" Text="Amount"></asp:Label>
                </td>
                <td><asp:TextBox ID="txt_expnce_amt" runat="server" CssClass="textbox textbox1 txtheight" ></asp:TextBox>
                  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender111" runat="server" TargetControlID="txt_expnce_amt"
                                                FilterType="Numbers" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                </td>
                <td><asp:Button ID="bt_exp_addnew" Text="Go" runat="server" CssClass="btn1 textbox1 textbox" OnClick="bt_exp_addnew_Click" />
                </td>
                </tr>
                </table>
                <br />
                  <div style="width: 600px; height: 320px; overflow: auto; margin-left: -26px; margin-top: -14px;">
                        <asp:GridView ID="GridView7" runat="server" AutoGenerateColumns="false" GridLines="Both"  OnRowDeleting="OnRowDeleting_grid7" OnRowCommand="grid7_Rowcommand"  Visible="false">
                          
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno4" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expence Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcname" ReadOnly="true" Text='<%#Eval("Dummy") %>' runat="server" CssClass="textbox txtheight4"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcnt" ReadOnly="true" Text='<%#Eval("Dummy1") %>' runat="server" CssClass="textbox txtheight3"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtamt1" ReadOnly="true" Text='<%#Eval("Dummy2") %>' style="text-align:right" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                    </ItemTemplate>
                                   <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                              <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                
                </div>

                  <div style="margin-left: -649px; margin-top: 35px; width: 230px; height: 36px" class="maindivstyle">
                 <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_attachment" runat="server">
                    Attachment
                            </asp:Label>
                        </td>
                        <td>
                        <asp:Button ID="btn_attach" runat="server" Text="Add" BackColor="#CECECE"  CssClass="btn2 textbox textbox1" OnClick="btn_attach_Click" />
                        </td>
                    </tr>
                </table>
                </div>

             
                    <div  id="div_attch" runat="server" visible="false" style="border: 1px solid silver; border-radius: 10px; font-size: medium; margin-left: 241px;
                margin-top: -37px; width: 570px; height: 80px" class="maindivstyle">
                <br />
                <table class="maindivstyle"><tr>
                  <td>
                                    <asp:Label ID="lbl_main_atch" Text="Attachments" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileupload" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                        </td>
                                        </tr></table>
                </div>

            <center>
                <div id="div6" runat="server" visible="true" style="margin-left: -7; margin-top: 106px;">
                    <asp:Button ID="btn_event_app" runat="server" Text="Save" OnClick="btn_event_app_Click" CssClass="textbox
        textbox1 btn2" />
          <asp:Button ID="btn_event_appclear" runat="server" Text="Clear" OnClick="btn_event_appclear_Click" CssClass="textbox
        textbox1 btn2" />
                </div>
            </center>

       
          
             </center>

                  <div id="divclosess" runat="server" class="popupstyle popupheight1"
                    visible="false">
                <center>
                    <div id="divclosediv" runat="server" class="table" style="background-color: White;
                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_erroralert1" runat="server"  Style="color: Red;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_erroralert1" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btnerrclose1_Click" Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
            <center>
               <div id="pop_add_staff_stud_othr" runat="server" visible="false" style="height: 150em; z-index: 100000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 582px; margin-left: 463px;"
                     OnClick="pop_add_staff_stud_othrclose_Click" />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 750px; width: 948px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px; margin-top:536px">
                        <br />
                        <span class="fontstyleheader" style="color: #008000;">Add Participant</span>
                        <br />
                        <br />
                        <table class="maindivstyle" width="350px">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdb_addstaff" Text="Staff" runat="server" GroupName="gg" AutoPostBack="true"
                                        OnCheckedChanged="rdb_addstaff_CheckedChanged" Checked="true" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_addstudent" Text="Student" runat="server" GroupName="gg"
                                        AutoPostBack="true" OnCheckedChanged="rdb_addstudent_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_addothers" Text="Individual" runat="server" GroupName="gg" AutoPostBack="true"
                                        OnCheckedChanged="rdb_addothers_CheckedChanged" />
                                </td>
                                  <td>
                                    <asp:RadioButton ID="rdo_addcomp" Text="Company" runat="server" GroupName="gg" AutoPostBack="true"
                                        OnCheckedChanged="rdo_addcomp_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="popupselectstd" runat="server" visible="false">
                
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_batch1" runat="server" Text="Batch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_batch1" Width="100px" Height="30px" runat="server" CssClass="textbox textbox1"
                                            onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_degree2" runat="server" Text="Degree"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_degree2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1"
                                                    Width="120px">--Select--</asp:TextBox>
                                                <asp:Panel ID="pdegree" runat="server" Width="150px" Height="170px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="cb_degree2" runat="server" OnCheckedChanged="cb_degree2_ChekedChange"
                                                        Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cbl_degree2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree2_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_degree2"
                                                    PopupControlID="pdegree" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_branch2" runat="server" Text="Branch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_branch2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1"
                                                    Width="120px">--Select--</asp:TextBox>
                                                <asp:Panel ID="pbranch" runat="server" Width="250px" Height="200px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="cb_branch1" runat="server" OnCheckedChanged="cb_branch1_ChekedChange"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cbl_branch1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_branch2"
                                                    PopupControlID="pbranch" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_rollno1" runat="server" Text="Roll No"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_rollno1" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                            Height="20px" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txt_rollno1"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getroll" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno1"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go1" Text="Go" OnClick="btn_go1stud_Click" CssClass="textbox btn1"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <p style="width: 691px;" align="right">
                                <asp:Label ID="lbl_cnt" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </p>
                            <div>
                                <asp:Label ID="Label31" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </div>
                            <center>
                             <div style="height: 448px;  overflow: auto; margin-left:-6px" width="900px">
                                <asp:GridView ID="GridView1" OnDataBound="GridView1_OnDataBound" CellPadding="4" runat="server" AutoGenerateColumns="false" width="860px"   OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="True">
                              
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text='<%#Container.DataItemIndex+1
        %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle ForeColor="White" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxSelectAll_CheckedChanged" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkup3" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_dept" runat="server" Text='<%# Eval("Dept_Name") %>' Width="210px"></asp:Label>                                          
                                       </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="180px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Roll No" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrollno" runat="server" Text='<%# Eval("Roll_No") %>' Width="80px"></asp:Label>
                                            <asp:Label ID="lblgatepass" runat="server" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Student Name" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreg" runat="server" Text='<%# Eval("Stud_Name") %>' Width="210px"></asp:Label>
                                             <asp:Label ID="lblappno" Visible="false" runat="server" Text='<%# Eval("app_no") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="180px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                  
                                    </Columns>
                                    </asp:GridView>
                            </div>
                            </center>
                            <br />
                            <br />
                           
                        </div>
                        <div id="pop_others" runat="server" visible="false">
                            
                            <table>
                             
                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_pname1" Text="Person Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_pname1" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng txtheight5 textbox1" onblur="getindividual(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txt_othr_pname1"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                           <asp:AutoCompleteExtender ID="AutoCompleteExtender14" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getotherpername" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_pname1"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cmp1name" Text="Institution Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_cmpname1" runat="server" onkeyup="myFunCaps(this.id)" onfocus="return myFunction(this)" onblur="getindividual(this.value)" AutoPostBack="true" OnTextChanged="txt_cmpname1_TextChanged"   CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>
                                       <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender38" runat="server" TargetControlID="txt_cmpname1"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                           <asp:AutoCompleteExtender ID="AutoCompleteExtender15" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getothernameprst" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_cmpname1"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_addcom" Text="Address" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tx_compadd" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng txtheight5  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender39" runat="server" TargetControlID="tx_compadd"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom,numbers" ValidChars=",/ ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cmstreet" Text="Street" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tx_comstr" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender40" runat="server" TargetControlID="tx_comstr"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=", /">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cm_city" Text="City" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_cmcity" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender41" runat="server" TargetControlID="txt_cmcity"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_cmpin" Text="Pin Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_cmpin" runat="server" MaxLength="6" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" TargetControlID="txt_cmpin"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cmstate" Text="State" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_cmstste" onkeyup="myFunCaps(this.id)" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender43" runat="server" TargetControlID="txt_cmstste"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                          <asp:AutoCompleteExtender ID="AutoCompleteExtender18" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getstate" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_cmstste"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cmcountry" Text="Country" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_cmcountry" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender44" runat="server" TargetControlID="txt_cmcountry"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                         <asp:AutoCompleteExtender ID="AutoCompleteExtender19" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcountry" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_cmcountry"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                       
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cmphn" Text="Phone No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_cmpho" runat="server" CssClass="textboxchng  textbox1" MaxLength="13"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender45" runat="server" TargetControlID="txt_cmpho"
                                            FilterType="numbers,custom" ValidChars="+ ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cmmail" Text="Mail ID" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_cmmail" runat="server" onfocus="return myFunction(this)" onblur="return checkEmail(this)" CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>
                                    </td>
                                    </tr>
                                    <tr>
                  <td>
                                    <asp:Label ID="lbl_part_indi_attch" Text="Attachments" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="FileUpload1_part_indi_attch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                        </td>
                                      
                                    <td><asp:Button ID="btn_add_indiadd" runat="server" Text="Add" CssClass="btn1 textbox textbox1" OnClick="btn_add_indiadd_Click" OnClientClick="return checkEmail()"  /></td>
                                  </tr>
                                  <tr><td colspan="2"><asp:Label ID="lbl_emptyerror1" runat="server" Visible="false" ForeColor="Red"></asp:Label></td></tr>
                            </table>
                       
                         <div style="width: 810px; height: 180px; overflow: auto;">
                        <asp:GridView ID="GridView9" runat="server" Visible="true" AutoGenerateColumns="false" OnRowCommand="grid9_Rowcommand" OnRowDeleting="OnRowDeleting_GridView9">
                       
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtactname" ReadOnly="true" runat="server" Text='<%#Eval("Dummy") %>'  CssClass="textbox txtheight4"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Person Name" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_per" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>' CssClass="textbox txtheight5"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                              
                                <asp:TemplateField HeaderText="Address" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_add" ReadOnly="true" runat="server" Text='<%#Eval("Dummy2") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Street" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_st" ReadOnly="true" runat="server" Text='<%#Eval("Dummy3") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>   
                                 <asp:TemplateField HeaderText="City" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_city" ReadOnly="true" runat="server" Text='<%#Eval("Dummy4") %>'  CssClass="textbox txtheight3" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                <asp:TemplateField HeaderText="Pin Code" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_pin" ReadOnly="true" runat="server" Text='<%#Eval("Dummy5") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField HeaderText="State" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_state" ReadOnly="true" runat="server" Text='<%#Eval("Dummy6") %>'  CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>                                                                   
                                <asp:TemplateField HeaderText="Country" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        
                                       <asp:TextBox ID="txt_country" ReadOnly="true" runat="server" Text='<%#Eval("Dummy7") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Phone No" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_phn" ReadOnly="true" runat="server" Text='<%#Eval("Dummy8") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField HeaderText="Mail Id" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_mail" ReadOnly="true" runat="server"  Text='<%#Eval("Dummy9") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                <asp:TemplateField HeaderText="Attachment" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_attch" ReadOnly="true" runat="server"  Text='<%#Eval("Dummy10") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                          <asp:TextBox ID="txt_e" runat="server" visible="false" Text='<%#Eval("Dummy11") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                           <asp:TextBox ID="txt_dt" runat="server" visible="false" Text='<%#Eval("Dummy12") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                            <br />
                            <center>
                                <asp:Button ID="btn_add_others" Visible="false" runat="server" CssClass="btn1
        textbox textbox1" Text="Add" /></center>
                        </div>

                        <div id="popup_selectstaff" runat="server" visible="false">
                         <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_poupcollege" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_poupcollege" CssClass="ddlheight4 textbox textbox1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_poupdept" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                   <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_staff_dept11" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel7" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="cb_staff_dept11" runat="server" Width="100px" Text="Select All" OnCheckedChanged="cb_staff_dept11_CheckedChanged"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_staff_dept11" runat="server" OnSelectedIndexChanged="cbl_staff_dept11_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txt_staff_dept11"
                                                PopupControlID="panel7" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                           
                                <td>
                                    <asp:Label ID="lbl_popupstafftype" runat="server" Text="Staff Type">
                                    </asp:Label>
                                </td>
                                <td>
                                  <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_staff_type11" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel8" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="cb_staff_type111" runat="server" Width="100px" Text="Select All" OnCheckedChanged="cb_staff_type111_CheckedChanged"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_staff_type111" runat="server" OnSelectedIndexChanged="cb_staff_type111_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender16" runat="server" TargetControlID="txt_staff_type11"
                                                PopupControlID="panel8" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                           <%--      </tr>
                            <tr>--%>
                                <td>
                                    <asp:Label ID="lbl_popdesign" runat="server" Text="Designation"></asp:Label>
                                </td>
                                <td>
                                   <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_staff_desg111" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel10" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="cb_staff_desn11" runat="server" Width="100px" Text="Select All" OnCheckedChanged="cb_staff_desn11_CheckedChanged"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_staff_desn11" runat="server" OnSelectedIndexChanged="cbl_staff_desn11_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender17" runat="server" TargetControlID="txt_staff_desg111"
                                                PopupControlID="panel10" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                           <td><asp:Button ID="btn_staff_go11" runat="server" CssClass="btn1 textbox textbox1" Text="Go" OnClick="btn_staff_go11_Click" /></td>
                           </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_popstaffby" runat="server" Text="Staff By" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_popstaffby" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                        runat="server" OnSelectedIndexChanged="ddl_popstaffby_SelectedIndexChanged" AutoPostBack="true"
                                        Visible="false">
                                        <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                        <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_pop_search" runat="server" OnTextChanged="txt_pop_search_TextChanged"
                                        AutoPostBack="True" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        
                      
                       

                        <br />
                        <center>
                            <asp:Label ID="Label33" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
 <div style="height: 438px; width: 900px; overflow: auto; margin-left:-7px">
                                <asp:GridView ID="GridView2" CellPadding="4" OnPageIndexChanging="GridView2_PageIndexChanging" runat="server" AutoGenerateColumns="false" width="880px" AllowPaging="True">
                              
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text='<%#Container.DataItemIndex+1
        %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle ForeColor="White" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxSelectAllSTAFF_CheckedChanged" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkup3" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="5px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrollno" runat="server" Text='<%# Eval("staff_code") %>' Width="20px"></asp:Label>
                                            <asp:Label ID="lblgatepass" runat="server" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="20px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Staff Name" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreg" runat="server" Text='<%# Eval("staff_name") %>' Width="220px"></asp:Label>
                                             <asp:Label ID="lblappno" Visible="false" runat="server" Text='<%# Eval("appl_id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="130px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldept" runat="server" Text='<%# Eval("dept_name") %>' Width="220px"></asp:Label>                                           
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="130px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                    </Columns>
                                    </asp:GridView>
                            </div>
                        </center>
                        <br />
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
             

         
         
                        </div>

                         <div id="popcm" runat="server" visible="false">
                            
                            <table>
                             
                                 <tr>
                                    <td>
                                        <asp:Label ID="Label27" Text="Company Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox1" runat="server"  CssClass="textboxchng txtheight5 textbox1"  onfocus="return myFunction(this)" onblur="getcompany(this.value)" AutoPostBack="true" OnTextChanged="TextBox1_TextChanged" ></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender66" runat="server" TargetControlID="TextBox1"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" &.">
                                        </asp:FilteredTextBoxExtender>
                                           <asp:AutoCompleteExtender ID="AutoCompleteExtender22" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcompname1" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox1"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label28" Text="Person Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox2" runat="server" onkeyup="myFunCaps(this.id)" onfocus="return myFunction(this)" onblur="getothers(this.value)"  CssClass="textboxchng txtheight5 textbox1" ></asp:TextBox>
                                       <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender67" runat="server" TargetControlID="txt_cmpname1"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                           <asp:AutoCompleteExtender ID="AutoCompleteExtender23" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcompnameper" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox2"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td>
                                        <asp:Label ID="Label30" Text="Address" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox3" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng txtheight5  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender68" runat="server" TargetControlID="TextBox3"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=", ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label35" Text="Street" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox4" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender69" runat="server" TargetControlID="TextBox4"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=", /">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label36" Text="City" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox8" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender70" runat="server" TargetControlID="TextBox8"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="Label37" Text="Pin Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox9" runat="server" MaxLength="6" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender71" runat="server" TargetControlID="TextBox9"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label38" Text="State" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox10" onkeyup="myFunCaps(this.id)" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender72" runat="server" TargetControlID="txt_cmstste"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                          <asp:AutoCompleteExtender ID="AutoCompleteExtender24" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getstate" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox10"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label39" Text="Country" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox11" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender73" runat="server" TargetControlID="txt_cmcountry"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                         <asp:AutoCompleteExtender ID="AutoCompleteExtender25" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcountry" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox11"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                       
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        <asp:Label ID="Label42" Text="Phone No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox12" runat="server" CssClass="textboxchng  textbox1" MaxLength="13"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender74" runat="server" TargetControlID="TextBox12"
                                            FilterType="numbers,custom" ValidChars="+- ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label43" Text="Mail ID" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox13" runat="server" onfocus="return myFunction(this)" onblur="return checkEmail(this)" CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>
                                    </td>
                                    </tr>
                                    <tr>
                  <td>
                                    <asp:Label ID="lbl_part_comp_attch" Text="Attachments" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="FileUpload_part_comp_attch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                        </td>
                                       
                                    <td><asp:Button ID="btn_addcompanydetails" runat="server" Text="Add" CssClass="btn1 textbox textbox1" OnClick="btn_addcompanydetails_Click" /></td>
                                </tr>
                                <tr><td colspan="2"><asp:Label ID="lbl_emptyerror2" runat="server" Visible="false" ForeColor="Red"></asp:Label></td></tr>
                            </table>
                       

                           <div style="width: 810px; height: 180px; overflow: auto;">
                        <asp:GridView ID="GridView8" runat="server" Visible="true" AutoGenerateColumns="false" OnRowCommand="grid8_Rowcommand" OnRowDeleting="OnRowDeleting_GridView8">
                       
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtactname" ReadOnly="true" runat="server" Text='<%#Eval("Dummy") %>'  CssClass="textbox txtheight4"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Person Name" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_per" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>' CssClass="textbox txtheight5"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                              
                                <asp:TemplateField HeaderText="Address" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_add" ReadOnly="true" runat="server" Text='<%#Eval("Dummy2") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Street" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_st" ReadOnly="true" runat="server" Text='<%#Eval("Dummy3") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>   
                                 <asp:TemplateField HeaderText="City" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_city" ReadOnly="true" runat="server" Text='<%#Eval("Dummy4") %>'  CssClass="textbox txtheight3" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                <asp:TemplateField HeaderText="Pin Code" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_pin" ReadOnly="true" runat="server" Text='<%#Eval("Dummy5") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField HeaderText="State" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_state" ReadOnly="true" runat="server" Text='<%#Eval("Dummy6") %>'  CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>                                                                   
                                <asp:TemplateField HeaderText="Country" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        
                                       <asp:TextBox ID="txt_country" ReadOnly="true" runat="server" Text='<%#Eval("Dummy7") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Phone No" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_phn" ReadOnly="true" runat="server" Text='<%#Eval("Dummy8") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField HeaderText="Mail Id" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_mail" ReadOnly="true" runat="server"  Text='<%#Eval("Dummy9") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                  <asp:TemplateField HeaderText="Attachment" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_attch" ReadOnly="true" runat="server"  Text='<%#Eval("Dummy10") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                    <asp:TextBox ID="txt_e" runat="server" visible="false" Text='<%#Eval("Dummy11") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                           <asp:TextBox ID="txt_dt" runat="server" visible="false" Text='<%#Eval("Dummy12") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                            <br />
                            <center>
                                <asp:Button ID="Button2" Visible="false" runat="server" CssClass="btn1
        textbox textbox1" Text="Add" /></center>
                        </div>
                      
                        <center><asp:Button ID="btn_go_staff" runat="server" OnClick="btn_go_staff_Click" Text="Go" CssClass="btn1 textbox textbox1" /></center>
                        
                        </div>
      
                  </div>

        </center>

                <%--  ******************************************************************************--%>
                <center>
                    <div id="pop_add_staff_stud_othr1" runat="server" visible="false" style="height: 150em; z-index: 100000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton6" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 856px; margin-left: 440px;"
                    OnClick="pop_add_staff_stud_othrclose1_Click" />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 770px; width: 908px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px; margin-top:813px"">
                        <br />
                        <span class="fontstyleheader" style="color: #008000;">Add Presented Person</span>
                        <br />
                        <br />
                        <table class="maindivstyle" width="350px">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdp_prsnt_staff" Text="Staff" runat="server" GroupName="tt" AutoPostBack="true"
                                        OnCheckedChanged="rdp_prsnt_staff_CheckedChanged" Checked="true" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdo_prsnt_stud" Text="Student" runat="server" GroupName="tt"
                                        AutoPostBack="true" OnCheckedChanged="rdo_prsnt_stud_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdo_prsnt_othr" Text="Individual" runat="server" GroupName="tt" AutoPostBack="true"
                                        OnCheckedChanged="rdo_prsnt_othr_CheckedChanged" />
                                </td>
                                 <td>
                                    <asp:RadioButton ID="rdo_prsnt_com" Text="Company" runat="server" GroupName="tt" AutoPostBack="true"
                                        OnCheckedChanged="rdo_prsnt_com_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                        <br />


                        <div id="div_prsnt_stud" runat="server" visible="false">
                 
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_prsntbatch" runat="server" Text="Batch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_prsnt_batch" Width="100px" Height="30px" runat="server" CssClass="textbox textbox1"
                                            onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_prsnt_deg" runat="server" Text="Degree"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_prsnt_degree" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1"
                                                    Width="120px">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel2" runat="server" Width="150px" Height="170px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="cb_prsnt_degree" runat="server" OnCheckedChanged="cb_prsnt_degreeChekedChange"
                                                        Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cbl_prsnt_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cb_prsnt_degree_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_prsnt_degree"
                                                    PopupControlID="Panel2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lb_prsnt_branch" runat="server" Text="Branch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_prsnt_branch" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1"
                                                    Width="120px">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" Width="250px" Height="200px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="cb_presnt_branch" runat="server" OnCheckedChanged="cb_prsnt_branch_ChekedChange"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cbl_prsnt_branch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_prsnt_branch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_prsnt_branch"
                                                    PopupControlID="Panel3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_prsnt_roll" runat="server" Text="Roll No"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_prsnt_roll" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                            Height="20px" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txt_prsnt_roll"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getroll" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_prsnt_roll"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_prsnt_studgo" Text="Go" OnClick="btn_prsnt_studgo_Click" CssClass="textbox btn1"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <p style="width: 691px;" align="right">
                                <asp:Label ID="Label32" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </p>
                            <div>
                                <asp:Label ID="Label34" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </div>
                           <div style="height: 468px; width: 862px; overflow: auto; margin-left:-1px">
                                <asp:GridView ID="GridView13" CellPadding="4" runat="server" AutoGenerateColumns="false" OnDataBound="GridView13_OnDataBound" OnRowDataBound="GridView13_OnRowDataBound" width="830px" OnPageIndexChanging="GridView13_PageIndexChanging" AllowPaging="True">
                              
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text='<%#Container.DataItemIndex+1
        %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle ForeColor="White" />
                                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxSelectAllstud2_CheckedChanged" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkup3" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldept" runat="server" Text='<%# Eval("Dept_Name") %>' Width="180px"></asp:Label>
                                
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Roll No" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrollno" runat="server" Text='<%# Eval("Roll_No") %>' Width="80px"></asp:Label>
                                         
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Student Name" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreg" runat="server" Text='<%# Eval("Stud_Name") %>' Width="180px"></asp:Label>
                                             <asp:Label ID="lblappno" Visible="false" runat="server" Text='<%# Eval("app_no") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                     
                                       <asp:TemplateField HeaderText="Category" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                        <asp:DropDownList ID="ddl_categofstaff" runat="server" CssClass="ddlheight textbox textbox1"></asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                   
                                    </Columns>
                                    </asp:GridView>
                            </div>
                            <br />
                            <br />
                           
                        </div>

                 

                        <div id="div_prsnt_otherss" runat="server" visible="false">
                           <table>
                              
                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_pname" Text="Person Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_pname" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng txtheight5 textbox1" onblur="getothers1(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="flt_oth_pname" runat="server" TargetControlID="txt_othr_pname"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                           <asp:AutoCompleteExtender ID="AutoCompleteExtender17" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getotherpername1" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_pname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_name" Text="Institution Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_name" runat="server" onkeyup="myFunCaps(this.id)" onfocus="return myFunction(this)"  onblur="getothers(this.value)" AutoPostBack="true" OnTextChanged="txt_othr_name_TextChanged"  CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>
                                       <asp:FilteredTextBoxExtender ID="flt_oth_name" runat="server" TargetControlID="txt_othr_name"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                           <asp:AutoCompleteExtender ID="AutoCompleteExtender16" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getothername" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_name"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_add" Text="Address" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_add" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng txtheight5  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender95" runat="server" TargetControlID="txt_othr_add"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=", ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_str" Text="Street" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_str" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender96" runat="server" TargetControlID="txt_othr_str"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=", /">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_city" Text="City" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_city" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender97" runat="server" TargetControlID="txt_othr_city"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lbl_ot_pin" Text="Pin Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_pin" runat="server" MaxLength="6" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender81" runat="server" TargetControlID="txt_othr_pin"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_state" Text="State" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_state" onkeyup="myFunCaps(this.id)" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender98" runat="server" TargetControlID="txt_othr_state"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                          <asp:AutoCompleteExtender ID="AutoCompleteExtender12" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getstate" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_state"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_county" Text="Country" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_county" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender99" runat="server" TargetControlID="txt_othr_county"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                         <asp:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcountry" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr_county"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                       
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_ph" Text="Phone No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_ph" runat="server" CssClass="textboxchng  textbox1" MaxLength="13"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="flt_phn" runat="server" TargetControlID="txt_othr_ph"
                                            FilterType="numbers,custom" ValidChars="+ ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_othr_mail" Text="Mail ID" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_othr_mail" runat="server" onfocus="return myFunction(this)" onblur="return checkEmail(this)" CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>
                                    </td>
                                    </tr>
                                    <tr>
                  <td>
                                    <asp:Label ID="lbl_prsnt_ind_atch" Text="Attachments" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="FileUpload_prsnt_ind_atch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                        </td>
                                       
                                    <td><asp:Button ID="btn_add_prsn_indi" runat="server" CssClass="btn1 textbox textbox1" Text="Add" OnClick="btn_add_prsn_indi_Click" /></td>
                                </tr>
                                <tr><td colspan="2"><asp:Label ID="lbl_emptyerror3" runat="server" Visible="false" ForeColor="Red"></asp:Label></td></tr>
                            </table>
                              <div style="width: 810px; height: 180px; overflow: auto;">
                        <asp:GridView ID="GridView10" runat="server" Visible="true" AutoGenerateColumns="false" OnRowCommand="grid10_Rowcommand" OnRowDeleting="OnRowDeleting_GridView10">
                       
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtactname" ReadOnly="true" runat="server" Text='<%#Eval("Dummy") %>'  CssClass="textbox txtheight4"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Person Name" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_per" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>' CssClass="textbox txtheight5"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                              
                                <asp:TemplateField HeaderText="Address" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_add" ReadOnly="true" runat="server" Text='<%#Eval("Dummy2") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Street" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_st" ReadOnly="true" runat="server" Text='<%#Eval("Dummy3") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>   
                                 <asp:TemplateField HeaderText="City" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_city" ReadOnly="true" runat="server" Text='<%#Eval("Dummy4") %>'  CssClass="textbox txtheight3" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                <asp:TemplateField HeaderText="Pin Code" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_pin" ReadOnly="true" runat="server" Text='<%#Eval("Dummy5") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField HeaderText="State" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_state" ReadOnly="true" runat="server" Text='<%#Eval("Dummy6") %>'  CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>                                                                   
                                <asp:TemplateField HeaderText="Country" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        
                                       <asp:TextBox ID="txt_country" ReadOnly="true" runat="server" Text='<%#Eval("Dummy7") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Phone No" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_phn" ReadOnly="true" runat="server" Text='<%#Eval("Dummy8") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField HeaderText="Mail Id" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_mail" ReadOnly="true" runat="server"  Text='<%#Eval("Dummy9") %>' CssClass="textbox txtheight5" ></asp:TextBox>

                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField HeaderText="Attchment" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtattch" ReadOnly="true" runat="server"  Text='<%#Eval("Dummy10") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                         <asp:TextBox ID="txt_e" runat="server" visible="false" Text='<%#Eval("Dummy11") %>' CssClass="textbox txtheight5" ></asp:TextBox>

                                           <asp:TextBox ID="txt_dt" runat="server" visible="false" Text='<%#Eval("Dummy12") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                
 <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" /> 
                            </Columns>
                        </asp:GridView>
                    </div>
                            <br />
                            <center>
                                <asp:Button ID="btn_prsnt_others" Visible="false" runat="server" CssClass="btn1
        textbox textbox1" Text="Add" /></center>
                        </div>

                        <div id="div_prsnt_staff" runat="server" visible="false">
                      
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_prsnt_clg" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_prnst_staff_clg" CssClass="ddlheight4 textbox textbox1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_prst_dpt" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPdp_deprt" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_staffdeprt" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_deprt" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="cb_staffdeprt" runat="server" Width="100px" Text="Select All" OnCheckedChanged="cb_staffdeprt_CheckedChanged"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_staffdeprt" runat="server" OnSelectedIndexChanged="cbl_staffdeprt_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popdesgtype" runat="server" TargetControlID="txt_staffdeprt"
                                                PopupControlID="panel_deprt" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                           
                                <td>
                                    <asp:Label ID="lbl_prsnt_stafftype" runat="server" Text="Staff Type">
                                    </asp:Label>
                                </td>
                                <td>
                                     <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_staff_type" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel4" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="cb_stafftype" runat="server" Width="100px" Text="Select All" OnCheckedChanged="cb_stafftype_CheckedChanged"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_stafftype" runat="server" OnSelectedIndexChanged="cbl_stafftype_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_staff_type"
                                                PopupControlID="panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                 </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_prsnt_stsff_design" runat="server" Text="Designation"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_staff_desgn" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel6" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="cb_staff_desng" runat="server" Width="100px" Text="Select All" OnCheckedChanged="cb_staff_desng_CheckedChanged"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_staff_desng" runat="server" OnSelectedIndexChanged="cbl_staff_desng_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_staff_desgn"
                                                PopupControlID="panel6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            <td><asp:Label ID="lblcat" runat="server" Text="Category"></asp:Label></td>
                            <td colspan="3">
                            <asp:Button ID="btn_addddd" runat="server" Text="+" CssClass="btn1 textbox textbox1" OnClick="btn_addddd_Click" />
                                <asp:DropDownList ID="ddl_viewdetails1" runat="server" CssClass="textbox textbox1 ddlheight4"></asp:DropDownList>
                                  <asp:Button ID="btn_min" runat="server" Text="-" CssClass="btn textbox textbox1" OnClick="btn_min_Click" /></td>
                                  <td><asp:Button ID="btn_staffgo" runat="server" CssClass="textbox textbox1 btn1" Text="Go" OnClick="btn_staffgo_Click" /></td></tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label40" runat="server" Text="Staff By" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownList7" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                        runat="server"  AutoPostBack="true"
                                        Visible="false">
                                        <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                        <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox7" runat="server"
                                        AutoPostBack="True" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                       
                        <center>
                            <asp:Label ID="Label41" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                 <div style="height: 468px; width: 890px; overflow: auto; margin-left:-19px">
                                <asp:GridView ID="GridView12" CellPadding="4" OnPageIndexChanging="GridView12_PageIndexChanging" OnRowDataBound="GridView12_OnRowDataBound" runat="server" AutoGenerateColumns="false" width="840px" AllowPaging="True">
                              
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text='<%#Container.DataItemIndex+1
        %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle ForeColor="White" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxSelectAllstaff2_CheckedChanged" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkup3" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrollno" runat="server" Text='<%# Eval("staff_code") %>' Width="50px"></asp:Label>
                                            <asp:Label ID="lblgatepass" runat="server" Visible="false" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="40px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Staff Name" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblreg" runat="server" Text='<%# Eval("staff_name") %>' Width="200px"></asp:Label>
                                             <asp:Label ID="lblappno" Visible="false" runat="server" Text='<%# Eval("appl_id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldept" runat="server" Text='<%# Eval("dept_name") %>' Width="200px"></asp:Label>
                                        
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Category" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                        <asp:DropDownList ID="ddl_categofstaff" runat="server" CssClass="ddlheight1 textbox textbox1"></asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="20px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>
                                    </Columns>
                                    </asp:GridView>
                            </div>
                           
                        </center>
                        <br />
                         <div id="imgdiv44" runat="server" visible="false" class="popupstyle popupheight" style="z-index: 3000000;">             
                <br />
                <br />

                <div id="panel_description22" runat="server" style="background-color: White; height: 120px; width: 500px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_description222" runat="server" Text="Description" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txt_description22" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                      
                        <tr>
                            <td align="center">
                                <asp:Button ID="btn_adddesc2" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btndesc2popadd_Click" />
                                <asp:Button ID="btn_exitdesc2" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btndesc2popexit_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                </div>
                 
                <br />
               </div>
              
                      
                         <div id="div_pesnt_com" runat="server" visible="false">
                           <table>
                              
                                
                                <tr>
                                    <td>
                                        <asp:Label ID="Label45" Text="Company Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox15" runat="server" onkeyup="myFunCaps(this.id)" onfocus="return myFunction(this)" onblur="getothers(this.value)" AutoPostBack="true" OnTextChanged="TextBox15_TextChanged"   CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>
                                       <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender76" runat="server" TargetControlID="txt_othr_name"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                           <asp:AutoCompleteExtender ID="AutoCompleteExtender27" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcompnameprst" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox15"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label44" Text="Person Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox14" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng txtheight5 textbox1" onblur="getothers1(this.value)" onfocus="return myFunction(this)" ></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender75" runat="server" TargetControlID="txt_othr_pname"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                           <asp:AutoCompleteExtender ID="AutoCompleteExtender26" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getotherpername" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox14"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label46" Text="Address" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox16" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng txtheight5  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender77" runat="server" TargetControlID="TextBox16"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=", ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label47" Text="Street" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox17" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender78" runat="server" TargetControlID="TextBox17"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=", /">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label48" Text="City" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox18" runat="server" onkeyup="myFunCaps(this.id)" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender79" runat="server" TargetControlID="TextBox18"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="Label49" Text="Pin Code" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox19" runat="server" MaxLength="6" CssClass="textboxchng  textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender80" runat="server" TargetControlID="TextBox19"
                                            FilterType="numbers" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label50" Text="State" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox20" onkeyup="myFunCaps(this.id)" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender82" runat="server" TargetControlID="txt_othr_state"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                          <asp:AutoCompleteExtender ID="AutoCompleteExtender28" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getstate" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox20"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label51" Text="Country" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox21" runat="server" CssClass="textboxchng  textbox1"></asp:TextBox>
                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender83" runat="server" TargetControlID="txt_othr_county"
                                            FilterType="UppercaseLetters,LowercaseLetters" ValidChars="">
                                        </asp:FilteredTextBoxExtender>
                                         <asp:AutoCompleteExtender ID="AutoCompleteExtender29" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getcountry" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox21"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                       
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        <asp:Label ID="Label52" Text="Phone No" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox22" runat="server" CssClass="textboxchng  textbox1" MaxLength="13"></asp:TextBox>
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender84" runat="server" TargetControlID="TextBox22"
                                            FilterType="numbers,custom" ValidChars="+ ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label53" Text="Mail ID" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox23" runat="server" onfocus="return myFunction(this)" onblur="return checkEmail(this)" CssClass="textboxchng txtheight5 textbox1"></asp:TextBox>
                                    </td>
                                   </tr>
                                   <tr>
                  <td>
                                    <asp:Label ID="Label54" Text="Attachments" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="FileUpload1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                        </td>
                                       
                                    <td><asp:Button ID="btn_add_prstcomp" runat="server" Text="Add" OnClick="btn_add_prstcomp_Click" CssClass="btn1 textbox textbox1" /></td>
                                 </tr>
                                 <tr><td colspan="2"><asp:Label ID="lbl_emptyerror4" runat="server" Visible="false" ForeColor="Red"></asp:Label></td></tr>
                            </table>
                              <div style="width: 810px; height: 180px; overflow: auto;">
                        <asp:GridView ID="GridView11" runat="server" Visible="true" AutoGenerateColumns="false" OnRowCommand="grid11_Rowcommand" OnRowDeleting="OnRowDeleting_GridView11">
                       
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtactname" ReadOnly="true" runat="server" Text='<%#Eval("Dummy") %>'  CssClass="textbox txtheight4"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Person Name" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_per" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>' CssClass="textbox txtheight5"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                              
                                <asp:TemplateField HeaderText="Address" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_add" ReadOnly="true" runat="server" Text='<%#Eval("Dummy2") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Street" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_st" ReadOnly="true" runat="server" Text='<%#Eval("Dummy3") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>   
                                 <asp:TemplateField HeaderText="City" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_city" ReadOnly="true" runat="server" Text='<%#Eval("Dummy4") %>'  CssClass="textbox txtheight3" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                <asp:TemplateField HeaderText="Pin Code" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_pin" ReadOnly="true" runat="server" Text='<%#Eval("Dummy5") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField HeaderText="State" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_state" ReadOnly="true" runat="server" Text='<%#Eval("Dummy6") %>'  CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>                                                                   
                                <asp:TemplateField HeaderText="Country" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        
                                       <asp:TextBox ID="txt_country" ReadOnly="true" runat="server" Text='<%#Eval("Dummy7") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Phone No" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_phn" ReadOnly="true" runat="server" Text='<%#Eval("Dummy8") %>' CssClass="textbox txtheight" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField HeaderText="Mail Id" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_mail" ReadOnly="true" runat="server"  Text='<%#Eval("Dummy9") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                 <asp:TemplateField HeaderText="Attachment" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_attch" ReadOnly="true" runat="server"  Text='<%#Eval("Dummy10") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                    <asp:TextBox ID="txt_e" runat="server" visible="false" Text='<%#Eval("Dummy11") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                           <asp:TextBox ID="txt_dt" runat="server" visible="false" Text='<%#Eval("Dummy12") %>' CssClass="textbox txtheight5" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                 <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                            </Columns>

                        </asp:GridView>
                    </div>
                            <br />
                         
                        </div>
                             <center>
               <div id="divactionadddetails" runat="server" visible="false" style="height: 100%; z-index: 6000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; 
            left: 0px;">
            <center>
                <div id="actadd" runat="server" visible="false" class="table" style="background-color: White;
                    height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 1080px; border-radius: 10px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_actalt" runat="server" Text="Description" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txt_actiondescription" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                      
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnaddactiondescrip" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btnaddactiondescrip_Click" />
                                <asp:Button ID="btnexiteactiondescrip" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btnexiteactiondescrip_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            </div>
              <div id="divactdeletedetais" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divactdel" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 1105px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_actdeletedetail" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btactdeletexit" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btactdeletexit_Click" Text="ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
            </center> 
                      

                        <center><asp:Button ID="btn_go_prsntclik" runat="server" Visible="false" OnClick="btn_go_prsntclik_Click" Text="Ok" CssClass="btn1 textbox textbox1"  /></center>
                      
                        </div>



<%--************************************8--%>
                       
                  </div>
                  </center>
         <center>
               <div id="itemnamediv" runat="server" visible="false" class="popupstyle popupheight" style="height:2800px;">
            <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 920px; margin-left: 434px;"
                OnClick="ImageButton5_Click" />
            <br />
            <br />
            <div style="background-color: White; height: 550px; width: 900px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px; margin-top:900px">
                <br />
                <center>
                    <asp:Label ID="lbl_pophead" runat="server" Text="Select Item" Style="font-size: large;
                        color: #790D03;"></asp:Label>
                </center>
                <br />
                <table class="maintablestyle" style="width: 640px;">
                    <tr>
                    <td>
                            <asp:Label ID="lbl_itm_hdr" runat="server" Text="Item Header Name"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_itn_hdr" runat="server" CssClass="textbox  txtheight3">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel13" runat="server" CssClass="multxtpanel" style="height:200px;width:141px;">
                                        <asp:CheckBox ID="cbb_itm_hdrname" runat="server" OnCheckedChanged="cb_itm_hdrname_CheckedChange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_itm_hdrname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cb_itm_hdrname_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender22" runat="server" TargetControlID="txt_itn_hdr"
                                        PopupControlID="panel13" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_subhdrname" Width="130px" runat="server" Text="Sub Header Name"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_subhdrname" runat="server" CssClass="textbox  txtheight3">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel16" runat="server" CssClass="multxtpanel" style="height:200px;width:141px;">
                                        <asp:CheckBox ID="cb_item_subhdr" runat="server" OnCheckedChanged="cb_item_subhdr_CheckedChange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_item_subhdr" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_item_subhdr_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender23" runat="server" TargetControlID="txt_subhdrname"
                                        PopupControlID="panel16" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_itemname" runat="server" Text="Item Name"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="upp_itemname" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_itemname" runat="server" CssClass="textbox  txtheight3">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_itemname" runat="server" CssClass="multxtpanel" style="height:200px;width:141px;">
                                        <asp:CheckBox ID="cb_itemname" runat="server" OnCheckedChanged="cb_itemname_CheckedChange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbl_itemname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_itemname_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popext_itemname" runat="server" TargetControlID="txt_itemname"
                                        PopupControlID="panel_itemname" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        </tr>
                        <tr>

                        <td>
                            <asp:Label ID="lblsearch" Width="160px" runat="server" Text="Search by Item Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_itemsearch" placeholder="Search Item Name" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                CssClass="textbox  txtheight3"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="ftext_itemsearch" runat="server" TargetControlID="txt_itemsearch"
                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="acext_itemsearch" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getnamemm" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_itemsearch"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Button ID="btngo" Text="Go" OnClick="btngo_Click" CssClass="textbox btn1"
                                runat="server" />
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <asp:Label ID="Label12" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
                <div id="div4" runat="server" visible="false" style="width: 550px; height: 250px;
                    overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;">
                    <br />
                    <asp:DataList ID="DataList1" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                        RepeatColumns="5" Width="400px" ForeColor="#333333">
                        <AlternatingItemStyle BackColor="White" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <ItemStyle BackColor="#E3EAEB" BorderWidth="1px" Height="0px" />
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_itemname" ForeColor="Green" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                        <asp:Label ID="lbl_itemcode" ForeColor="Green" Visible="false" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblitemheadername" ForeColor="Green" Visible="false" runat="server"
                                            ></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:DataList>
                </div>
                <br />
                <center>
                    <div>
                        <asp:Button ID="btn_ok1" runat="server" Visible="false" CssClass="textbox btn2" Text="Add"
                            OnClick="btn_ok1_Click" />
                        <asp:Button ID="btn_exit3" runat="server" Visible="false" CssClass="textbox btn2"
                            Text="Exit" OnClick="btn_exit3_Click" />
                    </div>
                </center>
            </div>
        </div>   
                 </center>  
  </div>

  <%--</ContentTemplate>
  
  </asp:UpdatePanel>--%>

  
               <div id="imgdiv33" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="panel_description11" runat="server" visible="false" class="table" style="background-color: White;
                    height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_description111" runat="server" Text="Description" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txt_description11" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                    margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                      
                        <tr>
                            <td align="center">
                                <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopadd_Click" />
                                <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopexit_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            </div>
            </center>

 

    <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px; height:2700px">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 748px;
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

          <div id="divdown" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px; height:2700px">
            <center>
                <div id="divdown1" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 1300px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_divdown1" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_divdown" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btn_divdown_Click" Text="ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
   </div>
 
</asp:Content>
