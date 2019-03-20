<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="MessAttendance.aspx.cs" Inherits="MessAttendance" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="viewport" content="initial-scale=1.0;width=device-width" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .maindivstylesize
            {
                height: 500px;
                width: 1000px;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">
            function date() {
                var dateObj = new Date();
                var week = dateObj.format("dddd/MMMM/yyyy h:mm:ss tt");
                document.getElementById('<%=lbltime.ClientID %>').innerHTML = week.toString();
                setTimeout('date()', 500);
                return "";
            }
            function enterkeyvoid(e) {
                if (e.keycode == 13 || e.which == 13) {
                    // void enter key
                    return false;
                }
            }

            function autoComplete1_OnClientPopulating(sender, args) {
                var sfk = document.getElementById("<%=ddl_session.ClientID%>").value;
                var hostel = document.getElementById("<%=ddl_Messname.ClientID%>").value;
                var type = document.getElementById("<%=ddl_studtype.ClientID%>").value;
                var mess_sess = hostel + '-' + sfk + '-' + type;
                sender.set_contextKey(mess_sess);
            }
            //Enter text box
            function getsmartrollno(txt1) {
                var dne = txt1.value;
                var dw = "0";

                var ddl = document.getElementById("<%=ddl_session.ClientID%>");
                if (ddl.options.length > 0) {
                    var sname = ddl.options[ddl.selectedIndex].text;
                    var sfk = document.getElementById("<%=ddl_session.ClientID%>").value;
                }
                var hostel = document.getElementById("<%=ddl_Messname.ClientID%>").value;
                var date = document.getElementById("<%=txt_date.ClientID %>").value;
                var type = document.getElementById("<%=ddl_studtype.ClientID%>").value;
                dw = sname + '-' + sfk + '-' + hostel + '-' + date + '-' + type;
                $.ajax({
                    type: "POST",
                    url: "MessAttendance.aspx/studroll",
                    data: '{Smart_No: "' + dne + '",j: "' + dw + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (responsesm) {
                        bindstudent(responsesm.d);
                    },
                    failure: function (responsesm) {
                        alert(responsesm);
                    }
                });
            }
            //Smart card textbox
            function getsmartno(txt1) {
                var smartcardno = txt1.value;
                var smartcardno_len = smartcardno.length;
                if (smartcardno_len >= 10) {
                    var someVarName = "";
                    var someVarName1 = localStorage.getItem("someVarName");
                    if (someVarName1 == "" || someVarName1 == "Null") {
                        localStorage.setItem("someVarName", someVarName);
                        someVarName1 = "";
                    }
                    if (someVarName1 != smartcardno) {
                        localStorage.setItem("someVarName", smartcardno);
                        var dw = "0";
                        var ddl = document.getElementById("<%=ddl_session.ClientID%>");
                        if (ddl.options.length > 0) {
                            var sname = ddl.options[ddl.selectedIndex].text;
                            var sfk = document.getElementById("<%=ddl_session.ClientID%>").value;
                        }
                        var hostel = document.getElementById("<%=ddl_Messname.ClientID%>").value;
                        var date = document.getElementById("<%=txt_date.ClientID %>").value;
                        var type = document.getElementById("<%=ddl_studtype.ClientID%>").value;
                        dw = sname + '-' + sfk + '-' + hostel + '-' + date + '-' + type;
                        $.ajax({
                            type: "POST",
                            url: "MessAttendance.aspx/studsmartno",
                            data: '{Smart_No: "' + smartcardno + '",j: "' + dw + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (responsesm) {
                                bindstudent(responsesm.d);
                            },
                            failure: function (responsesm) {
                                alert(responsesm);
                            }
                        });
                    }
                    else {
                        localStorage.clear();
                        document.getElementById('<%=txt_smartno.ClientID %>').value = "";
                        var textbox = document.getElementById('<%=txt_smartno.ClientID %>');
                        textbox.focus();
                        textbox.scrollIntoView();
                        //alert('Attendance AlreadyMarked');
                    }
                }
            }
            function bindstudent(Employees) {

                var statusmsg1 = Employees[5].toString();
                var rno = Employees[0].toString();
                if (statusmsg1 == "1") {
                    document.getElementById('<%=txt_roll_no.ClientID %>').value = rno;
                    document.getElementById('<%=txt_studentname.ClientID %>').value = Employees[1].toString();
                    document.getElementById('<%=txt_studenttype.ClientID %>').value = Employees[2].toString();
                    document.getElementById('<%=txt_degree.ClientID %>').value = Employees[3].toString();
                    document.getElementById('<%=lbl_studimage.ClientID %>').src = Employees[4].toString();
                    //Mark msg
                    var mesg = document.getElementById('<%=lbl_att.ClientID %>');
                    mesg.style.color = "green";
                    mesg.innerHTML = "Attendance Marked";
                    //Lable text
                    var type = Employees[6].toString()
                    if (type == "1") {
                        var rolllbl = document.getElementById('<%=lbl_roll.ClientID %>');
                        rolllbl.innerHTML = "Staff Code";
                        rolllbl = document.getElementById('<%=lbl_type.ClientID %>');
                        rolllbl.innerHTML = "Staff Type";
                        rolllbl = document.getElementById('<%=lbl_desig.ClientID %>');
                        rolllbl.innerHTML = "Designation";
                    }
                    if (type == "0") {
                        var rolllbl = document.getElementById('<%=lbl_roll.ClientID %>');
                        rolllbl.innerHTML = "Roll No";
                        rolllbl = document.getElementById('<%=lbl_type.ClientID %>');
                        rolllbl.innerHTML = "Student Type";
                        rolllbl = document.getElementById('<%=lbl_desig.ClientID %>');
                        rolllbl.innerHTML = "Degree";
                    }
                    document.getElementById('<%=txt_rollno.ClientID %>').value = "";
                    var textbox = document.getElementById('<%=txt_rollno.ClientID %>');
                    textbox.focus();
                    textbox.scrollIntoView();
                } else {
                    document.getElementById('<%=txt_roll_no.ClientID %>').value = rno;
                    document.getElementById('<%=txt_studentname.ClientID %>').value = Employees[1].toString();
                    document.getElementById('<%=txt_studenttype.ClientID %>').value = Employees[2].toString();
                    document.getElementById('<%=txt_degree.ClientID %>').value = Employees[3].toString();
                    document.getElementById('<%=lbl_studimage.ClientID %>').src = Employees[4].toString();
                    var mesg = document.getElementById('<%=lbl_att.ClientID %>');
                    var statusmsg1 = Employees[5].toString();
                    if (statusmsg1 == "0") {
                        mesg.style.color = "red";
                        mesg.innerHTML = "Attendance UnMarked";

                        var type = document.getElementById("<%=ddl_studtype.ClientID%>").value;
                        if (type == "0") {
                            document.getElementById('<%=txt_smartno.ClientID %>').value = "";
                            var textbox = document.getElementById('<%=txt_smartno.ClientID %>');
                            textbox.focus();
                            textbox.scrollIntoView();
                        }
                        else {
                            document.getElementById('<%=txt_rollno.ClientID %>').value = "";
                            var textbox = document.getElementById('<%=txt_rollno.ClientID %>');
                            textbox.focus();
                            textbox.scrollIntoView();
                        }
                        alert('Please Check Student Details');
                    }
                    if (statusmsg1 == "2") {
                        mesg.style.color = "brown";
                        mesg.innerHTML = "Attendance AlreadyMarked";
                        var type = document.getElementById("<%=ddl_studtype.ClientID%>").value;
                        if (type == "0") {
                            document.getElementById('<%=txt_smartno.ClientID %>').value = "";
                            var textbox = document.getElementById('<%=txt_smartno.ClientID %>');
                            textbox.focus();
                            textbox.scrollIntoView();
                        }
                        else {
                            document.getElementById('<%=txt_rollno.ClientID %>').value = "";
                            var textbox = document.getElementById('<%=txt_rollno.ClientID %>');
                            textbox.focus();
                            textbox.scrollIntoView();
                        }
                        //alert('Attendance AlreadyMarked');
                    }
                }
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <span style="color: #008000" class="fontstyleheader">Mess Attendance</span>
            <div style="margin-right: 853px; margin-top: 0px;">
                <asp:Label ID="lbltime" runat="server" ForeColor="#336699" Width="280px"><script>                                                                                             document.write(date())</script></asp:Label>
            </div>
            <%--onblur="return date()"--%>
            <br />
        </center>
        <center>
            <div class="maindivstyle maindivstylesize">
                <br />
                <center>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_messname" Text="Mess Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_Messname" runat="server" Visible="true" AutoPostBack="True"
                                    CssClass="textbox1 ddlheight4" OnSelectedIndexChanged="ddl_Messname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_session" Text="Session Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_session" runat="server" CssClass="textbox1 ddlheight4">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_studtype" runat="server" CssClass="textbox1 ddlheight"
                                    Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddl_studtype_selectedindexchanged">
                                    <asp:ListItem Value="0">Smart Card</asp:ListItem>
                                    <asp:ListItem Value="1">Roll No</asp:ListItem>
                                    <asp:ListItem Value="2">Reg No</asp:ListItem>
                                    <asp:ListItem Value="3">Student Name</asp:ListItem>
                                    <asp:ListItem Value="4">Staff Code</asp:ListItem>
                                    <asp:ListItem Value="5">Staff Name</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_smartno" Visible="false" runat="server" CssClass="textbox textbox1"
                                    Width="110px" onkeyup="return getsmartno(this)" TextMode="Password" onkeypress="return enterkeyvoid(event)"
                                    placeholder="Smartcard No"></asp:TextBox><%--OnTextChanged="rollNo_Oncheckedchange" AutoPostBack="true" onkeyup--%>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_rollno"
                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" .">
                                </asp:FilteredTextBoxExtender>
                                <asp:TextBox ID="txt_rollno" runat="server" CssClass="textbox textbox1" Width="110px"
                                    onchange="return getsmartrollno(this)" onkeypress="return enterkeyvoid(event)"></asp:TextBox><%--OnTextChanged="rollNo_Oncheckedchange" AutoPostBack="true" onkeyup--%>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_rollno"
                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" .">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" Enabled="True"
                                    ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                    CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground"
                                    UseContextKey="true" OnClientPopulating="autoComplete1_OnClientPopulating" DelimiterCharacters="">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_date" Text="Date" runat="server"></asp:Label>
                                <asp:TextBox ID="txt_date" runat="server" CssClass="textbox textbox1" Width="80px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                                    Format="dd/MM/yyyy">
                                    <%-- CssClass="cal_Theme1 ajax__calendar_active"--%>
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <center>
                        <div>
                            <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        </div>
                    </center>
                </center>
                <br />
                <center>
                    <asp:Panel ID="Panel1" runat="server" BorderStyle="Solid" Style="height: auto; width: 766px;"
                        CssClass="maindivstyle">
                        <%--BorderColor="#993333"--%>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_studentname" runat="server" CssClass="textbox  textbox1 txtheight5"></asp:TextBox>
                                </td>
                                <td rowspan="4">
                                    <asp:Image ID="lbl_studimage" runat="server" Width="130px" Height="120px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_roll" runat="server">Roll No</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_roll_no" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_type" runat="server">Student Type</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_studenttype" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_desig" runat="server"> Degree</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Attendance
                                </td>
                                <td>
                                    <asp:Label ID="lbl_att" runat="server" Style="color: Red;" Font-Size="Large" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </asp:Panel>
                </center>
                <br />
                <div id="alertmessage" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alerterror" Visible="false" runat="server" Text="" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <%-- <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;" OnClick="btn_errorclose_Click" Text="OK" runat="server" />--%>
                                                <asp:ImageButton ID="btn_errorclose" Style="height: 40px; width: 40px;" OnClick="btn_errorclose_Click"
                                                    ImageUrl="~/images/okimg.jpg" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </div>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
