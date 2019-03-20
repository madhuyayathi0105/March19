<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="InternalMarkEntryNew.aspx.cs" Inherits="MarkMod_InternalMarkEntryNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
    <style type="text/css">
        .GridDock
        {
            overflow-x: auto;
            overflow-y: auto;
            width: 480px;
            height: 400px;
            padding: 0 0 0 0;
        }
    </style>
    <script type="text/javascript">


        function validatetxt(e) {
            var mk = -16;
            max = parseInt(document.getElementById("<%=lblMaxMark.ClientID %>").innerHTML.trim());
            if (e.value < mk || e.value > max) {
                e.value = "";
                alert("Enter Mark Less Than Or Equal To Maximum Mark");
            }

        }
        function JvfunonBlurLab() {
            var result = 0;
            var grid = document.getElementById('<%=gridLab.ClientID %>');
            for (i = 0; i < grid.rows.length - 1; i++) {
                var ddl4 = document.getElementById('MainContent_gridLab_txttest_' + i.toString());
                var max = parseFloat(ddl4.value);
                if (isNaN(max))
                    max = 0;
                result = result + max;
            }
            document.getElementById("<%=lblGrandTotal.ClientID %>").innerHTML = result;
        }

        function JvfunonBlur() {
            var result = 0;
            var grid = document.getElementById('<%=GridView3.ClientID %>');
            for (i = 0; i < grid.rows.length - 1; i++) {
                var ddl4 = document.getElementById('MainContent_GridView3_txttest_' + i.toString());
                var max = parseFloat(ddl4.value);
                if (isNaN(max))
                    max = 0;
                result = result + max;
            }
            document.getElementById("<%=lblGrandTotal.ClientID %>").innerHTML = result;
        }
       
    </script>
    <script type="text/javascript">
        function calDescTotal() {
            var mark1 = document.getElementById("<%=txtQ1.ClientID %>");
            var mark2 = document.getElementById("<%=txtQ2.ClientID %>");
            var mark3 = document.getElementById("<%=txtQ3.ClientID %>");
            var mark4 = document.getElementById("<%=txtQ4.ClientID %>");
            if (mark1.value > 5)
                alert("Mark should be below or equal to 5");
            if (mark2.value > 5)
                alert("Mark should be below or equal to 5");
            if (mark3.value > 5)
                alert("Mark should be below or equal to 5");
            if (mark4.value > 5)
                alert("Mark should be below or equal to 5");

            var m1 = document.getElementById("<%=txtQ1.ClientID %>").value;
            if (m1 < -20) {
                alert("Q1 Not Valid Marks");
            }
            var m2 = document.getElementById("<%=txtQ2.ClientID %>").value;
            if (m2 < -20) {
                alert("Q2 Not Valid Marks");
            }
            var m3 = document.getElementById("<%=txtQ3.ClientID %>").value;
            if (m3 < -20) {
                alert("Q3 Not Valid Marks");
            }
            var m4 = document.getElementById("<%=txtQ4.ClientID %>").value;
            if (m4 < -20) {
                alert("Q4 Not Valid Marks");
            }
            if (mark1.value == -20 && mark2.value == -20 && mark3.value == -20 && mark4.value == -20) {
                var descTotal = document.getElementById("<%=lblDescTotal.ClientID %>");

                descTotal.innerHTML = -20;
            }
            if (mark1.value == -16 && mark2.value == -16 && mark3.value == -16 && mark4.value == -16) {
                var descTotal = document.getElementById("<%=lblDescTotal.ClientID %>");
                descTotal.innerHTML = -16;
            }

            if (mark1.value == -1 && mark2.value == -1 && mark3.value == -1 && mark4.value == -1) {
                var descTotal = document.getElementById("<%=lblDescTotal.ClientID %>");
                descTotal.innerHTML = -1;
            }
            else {
                //                if (mark1.value <= 5 && mark2.value <= 5 && mark3.value <= 5 && mark4.value <= 5)
                if (mark1.value <= 5 || mark2.value <= 5 || mark3.value <= 5 || mark4.value <= 5) {
                    var result = 0;
                    var arr = [];
                    if (mark1.value == "") {
                        arr[0] = 0;
                    }
                    else {
                        arr[0] = mark1.value;
                    }
                    if (mark2.value == "") {
                        arr[1] = 0;
                    }
                    else {
                        arr[1] = mark2.value;
                    }
                    if (mark3.value == "") {
                        arr[2] = 0;
                    }
                    else {
                        arr[2] = mark3.value;
                    }
                    if (mark4.value == "") {
                        arr[3] = 0;
                    }
                    else {
                        arr[3] = mark4.value;
                    }

                    //                    arr[0] = mark1.value;
                    //                    arr[1] = mark2.value;
                    //                    arr[2] = mark3.value;
                    //                    arr[3] = mark4.value;

                    arr.sort(function (a, b) {
                        return a - b;
                    });
                    var a = 0;
                    for (i = arr.length; i <= arr.length; i--) {
                        a++;
                        if (a <= 2) {
                            result = parseFloat(result) + parseFloat(arr[i - 1]);
                            // alert(result);
                        } else
                            break;
                    }

                    var descTotal = document.getElementById("<%=lblDescTotal.ClientID %>");
                    //document.getElementById("<%=lblDescTotal.ClientID %>").value = Math.round(result);
                    descTotal.innerHTML = result;
                }
            }
        }

        function calDescTotal1() {
            var rec = document.getElementById("<%=txtRec.ClientID %>").value;
            if (rec < -1) {
                alert("Record Not Valid Marks");
                rec.value = "";
            }
            if (rec > 10) {
                alert("Record should be below or equal to 10");
            }
            var ob = document.getElementById("<%=txtObser.ClientID %>").value;
            if (ob < -1) {
                alert("Observation Not Valid Marks");
                ob.value = "";
            }
            if (ob > 5) {
                alert("Observation should be below or equal to 5");
            }
            var internal = document.getElementById("<%=txtinternal.ClientID %>").value;
            if (internal < -1) {
                alert("Internal Not Valid Marks");
                internal.value = "";
            }
            if (internal > 10) {
                alert("Internal should be below or equal to 10");
                internal.value = "";
            }
        }
        function agecheck1() {
            var rec = document.getElementById("<%=txtRec.ClientID %>").value;
            if (rec < -1) {
                alert("Record Not Valid Marks");
                rec.value = "";
            }
            else if (rec > 10) {
                alert("Record should be below or equal to 10");
                rec.value = "";
            }

            var ob = document.getElementById("<%=txtObser.ClientID %>").value;
            if (ob < -1) {
                alert("Observation Not Valid Marks");
                ob.value = "";
            }
            else if (ob > 5) {
                alert("Observation should be below or equal to 5");
                ob.value = "";
            }

            var internal = document.getElementById("<%=txtinternal.ClientID %>").value;
            if (internal < -1) {
                alert("Internal Not Valid Marks");
                internal.value = "";
            }
            else if (internal > 10) {
                alert("Internal should be below or equal to 10");
                internal.value = "";
            }
        }

        function agecheck() {

            var m11 = document.getElementById("<%=txtQ1.ClientID %>").value;
            if (m11 < -1) {
                alert("Q1 Not Valid Marks");
            }
            else if (m11 > 5) {
                alert("Mark1 should be below or equal to 5");
            }

            var m21 = document.getElementById("<%=txtQ2.ClientID %>").value;
            if (m21 < -1) {
                alert("Q2 Not Valid Marks");
            }
            else if (m21 > 5) {
                alert("Mark2 should be below or equal to 5");
            }

            var m31 = document.getElementById("<%=txtQ3.ClientID %>").value;
            if (m31 < -1) {
                alert("Q3 Not Valid Marks");
            }
            else if (m31 > 5) {
                alert("Mark3 should be below or equal to 5");
            }

            var m41 = document.getElementById("<%=txtQ4.ClientID %>").value;
            if (m41 < -1) {
                alert("Q4 Not Valid Marks");
            }
            else if (m41 > 5) {
                alert("Mark4 should be below or equal to 5");
            }


            var m311 = document.getElementById("<%=txtQuizMark.ClientID %>").value;
            if (m311 < -1) {
                alert("QuizMark Not Valid Marks");
                m311.value = "";
            }
            else if (m311 > 10) {
                alert("QuizMark should be below or equal to 10");
            }

            var m411 = document.getElementById("<%=txtAssignmntMark.ClientID %>").value;
            if (m411 < -1) {
                alert("AssignmntMark Not Valid Marks");
                m411.value = "";
            }
            else if (m411 > 5) {
                alert("AssignmntMark should be below or equal to 5");
            }

            var rec = document.getElementById("<%=txtObser.ClientID %>").value;
            if (rec < -1) {
                alert("Record Not Valid Marks");
                rec.value = "";
            }
            else if (rec > 10) {
                alert("Record should be below or equal to 10");
            }





        }

        function check() {

            var quizMark = document.getElementById("<%=txtQuizMark.ClientID %>");
            var assignmentMark = document.getElementById("<%=txtAssignmntMark.ClientID %>");

            if (quizMark.value > 10)
                alert("Mark should be below or equal to 10");
            if (assignmentMark.value > 5)
                alert("Mark should be below or equal to 5");

            var m311 = document.getElementById("<%=txtQuizMark.ClientID %>").value;
            if (m311 < -1) {
                alert("QuizMark Not Valid Marks");
                m311.value = "";
            }
            var m411 = document.getElementById("<%=txtAssignmntMark.ClientID %>").value;
            if (m411 < -1) {
                alert("AssignmntMark Not Valid Marks");
                m411.value = "";
            }
        }

        function getDescTotal() {

            var mark1 = document.getElementById("<%=txtQ1.ClientID %>");
            var mark2 = document.getElementById("<%=txtQ2.ClientID %>");
            var mark3 = document.getElementById("<%=txtQ3.ClientID %>");
            var mark4 = document.getElementById("<%=txtQ4.ClientID %>");

            if (mark1.value == -1 && mark2.value == -1 && mark3.value == -1 && mark4.value == -1) {
                document.getElementById("<%=hdnDescTotal.ClientID %>").value = -1;
            }
            else {
                //                if (mark1.value <= 5 && mark2.value <= 5 && mark3.value <= 5 && mark4.value <= 5)
                if (mark1.value <= 5 || mark2.value <= 5 || mark3.value <= 5 || mark4.value <= 5) {
                    var result = 0;
                    var arr = [];
                    arr[0] = mark1.value;
                    arr[1] = mark2.value;
                    arr[2] = mark3.value;
                    arr[3] = mark4.value;

                    arr.sort(function (a, b) {
                        return a - b;
                    });
                    var a = 0;
                    for (i = arr.length; i <= arr.length; i--) {
                        a++;
                        if (a <= 2) {
                            result = parseFloat(result) + parseFloat(arr[i - 1]);

                        } else
                            break;
                    }

                    document.getElementById("<%=hdnDescTotal.ClientID %>").value = result;
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <style type="text/css">
        .style1
        {
            width: 65px;
        }
        .style2
        {
            width: 58px;
        }
        .style4
        {
            width: 237px;
        }
        .style5
        {
            width: 70px;
        }
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
            width: auto;
        }
        .HellowWorldPopup
        {
            min-width: 100px;
            min-height: 50px;
            background: white;
        }
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
        .cursorptr
        {
            cursor: pointer;
        }
        .style6
        {
            width: 340px;
        }
        .style7
        {
            top: 221px;
            left: 40px;
            position: absolute;
            height: 21px;
            width: 633px;
        }
        .style8
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
            top: 190px;
            left: 0px;
        }
        .style9
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
            margin-top: 0px;
            margin-left: 0px;
            height: 14px;
        }
        .style10
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "Arial";
            width: 960px;
            height: 17px;
        }
        .style11
        {
            height: 29px;
        }
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 500px;
            min-height: 100px;
            max-height: 150px;
            top: 100px;
            left: 150px;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            border-style: solid;
            border-color: Gray;
            border-width: 1px;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
    </style>
    <script type="text/javascript">
        window.onload = function () {
            var scrollY = parseInt('<%=Request.Form["scrollY"] %>');
            if (!isNaN(scrollY)) {
                window.scrollTo(0, scrollY);
            }
        };
        window.onscroll = function () {
            var scrollY = document.body.scrollTop;
            if (scrollY == 0) {
                if (window.pageYOffset) {
                    scrollY = window.pageYOffset;
                }
                else {
                    scrollY = (document.body.parentElement) ? document.body.parentElement.scrollTop : 0;
                }
            }
            if (scrollY > 0) {
                var input = document.getElementById("scrollY");
                if (input == null) {
                    input = document.createElement("input");
                    input.setAttribute("type", "hidden");
                    input.setAttribute("id", "scrollY");
                    input.setAttribute("name", "scrollY");
                    document.forms[0].appendChild(input);
                }
                input.value = scrollY;
            }
        };
    </script>
    <script type="text/javascript">
        function validate(e) {
            var mk = -16;
            var rowIndex1 = $(e).closest('tr').index();
            var rowIndex = rowIndex1 - 1;
            var ddl4 = document.getElementById('MainContent_GridView3_lblmamrk_' + rowIndex.toString());
            //var ddl5 = document.getElementById('MainContent_GridView3_txttest_' + rowIndex.toString());
            var max = parseFloat(ddl4.innerHTML.trim());
            //var subMark = ddl5.innerHTML.trim();
            //var mark = ddl5.value;
            //alert(max);
            var test = e.value.trim();
            //alert(test);
            if (test < mk || test > max) {
                e.value = "";
                alert("Enter Mark Less Than Or Equal To Maximum Mark");
            }
        }
        function validateLab(e) {
            var mk = -16;
            var rowIndex1 = $(e).closest('tr').index();
            var rowIndex = rowIndex1 - 1;
            var ddl4 = document.getElementById('MainContent_gridLab_lblregno_' + rowIndex.toString());
            //var ddl5 = document.getElementById('MainContent_GridView3_txttest_' + rowIndex.toString());
            var max = parseFloat(ddl4.innerHTML.trim());
            //var subMark = ddl5.innerHTML.trim();
            //var mark = ddl5.value;
            //alert(max);
            var test = e.value.trim();
            //alert(test);
            if (test < mk || test > max) {
                e.value = "";
                alert("Enter Mark Less Than Or Equal To Maximum Mark");
            }
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID='UpdGridStudent' runat="server">
            <ContentTemplate>
                <input runat="server" type="hidden" id="hdnDescTotal" value="0" />
                <center>
                    <span id="lblStripHead" runat="server" class="fontstyleheader" style="color: Green;
                        padding-bottom: 28px;">Internal Mark Entry </span>
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;
                <center>
                    <div style="background-color: #719DDB; width: 977px; height: 25px; color: Black;
                        font-family: Times New Roman; font-size: large; padding-top: 5px">
                        <span runat="server" id="lblSubDetails">Subject Details</span>
                    </div>
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <center>
                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="GridView1" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                        width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" BackColor="AliceBlue"
                        OnSelectedIndexChanged="SelectedIndexChanged" OnRowCreated="OnRowCreated" OnRowDataBound="gridview1_OnRowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <asp:Label ID="lblSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Batch Year">
                                <ItemTemplate>
                                    <asp:Label ID="lblbatch" runat="server" Text='<%# Eval("batch_year") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Degree">
                                <ItemTemplate>
                                    <asp:Label ID="lbldegreecode" runat="server" Text='<%#Eval("degree_code") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lbldegree" runat="server" Text='<%# Eval("degree") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Semester">
                                <ItemTemplate>
                                    <asp:Label ID="lblsem" runat="server" Text='<%# Eval("sem") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Section">
                                <ItemTemplate>
                                    <asp:Label ID="lblsection" runat="server" Text='<%# Eval("Section") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject">
                                <ItemTemplate>
                                    <asp:Label ID="lblsubject" runat="server" Text='<%# Eval("subject_name") %>' Font-Underline="true"
                                        ForeColor="Blue"></asp:Label>
                                    <asp:Label ID="lblsubno" runat="server" Text='<%# Eval("subject_no") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblsubcode" runat="server" Text='<%# Eval("subject_code") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                    </asp:GridView>
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;
                <center>
                    <div id="testDetailsLblDiv" runat="server" style="background-color: #719DDB; width: 977px;
                        height: 25px; color: Black; font-family: Times New Roman; font-size: large; padding-top: 5px">
                        <span runat="server" id="lblTestName" />
                    </div>
                </center>
                <center>
                    <center>
                        <asp:GridView ID="GridView2" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                            width: 800px;" Font-Names="Times New Roman" AutoGenerateColumns="false" Visible="false"
                            OnRowDataBound="gridview2_OnRowDataBound" BackColor="AliceBlue">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno1" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbcell_1" runat="server" />
                                        <%--OnCheckedChanged="cbcell_OnCheckedChanged"   --%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Test">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltest" runat="server" Text='<%# Eval("test") %>'></asp:Label>
                                        <asp:Label ID="lblcriteriano" runat="server" Text='<%#Eval("criteria_no") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblexamcode" runat="server" Text='<%# Eval("examcode")  %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblbind" runat="server" Text='<%# Eval("bind")  %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblbindnote" runat="server" Text='<%# Eval("bindNote")  %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exam-Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblexamdate" runat="server" Text='<%#Eval("examdate") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblexmdt" runat="server" Text='<%#Eval("exmdt") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlexamdate" runat="server" OnSelectedIndexChanged="ddlexamdate_OnSelectedIndexedChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exam-Month">
                                    <ItemTemplate>
                                        <asp:Label ID="lblexammonth" runat="server" Text='<%#Eval("exammonth") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlexammonth" runat="server" OnSelectedIndexChanged="ddlexammonth_OnSelectedIndexedChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exam-Year">
                                    <ItemTemplate>
                                        <asp:Label ID="lblexamyear" runat="server" Text='<%#Eval("examyear") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlexamyear" runat="server" OnSelectedIndexChanged="ddlexamyear_OnSelectedIndexedChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Entry-Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblentdt" runat="server" Text='<%#Eval("entdt") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblentrydate" runat="server" Text='<%#Eval("entrydate") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlentrydate" runat="server" OnSelectedIndexChanged="ddlentrydate_OnSelectedIndexedChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Entry-Month">
                                    <ItemTemplate>
                                        <asp:Label ID="lblentrymonth" runat="server" Text='<%#Eval("entrymonth") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlentrymonth" runat="server" OnSelectedIndexChanged="ddlentrymonth_OnSelectedIndexedChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Entry-Year">
                                    <ItemTemplate>
                                        <asp:Label ID="lblentryyear" runat="server" Text='<%#Eval("entryyear") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlentryyear" runat="server" OnSelectedIndexChanged="ddlentryyear_OnSelectedIndexedChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Duration-Hrs">
                                    <ItemTemplate>
                                        <asp:Label ID="lblhrs" runat="server" Text='<%#Eval("durationhrs") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lbldurhrs" runat="server" Text='<%#Eval("durationhours") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlhrs" runat="server" OnSelectedIndexChanged="ddlhrs_OnSelectedIndexedChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Duration-Mins">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmins" runat="server" Text='<%#Eval("durationmins") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlmins" runat="server" OnSelectedIndexChanged="ddlmins_OnSelectedIndexedChanged">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MaxMark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmaxmarks" runat="server" Text='<%#Eval("max_mark") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MinMark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblminmarks" runat="server" Text='<%#Eval("min_mark") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </center>
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp; <span style="width: 333px; float: right;">
                    <asp:CheckBox ID="chkretest" runat="server" Text="ReTest Marks" Font-Names="Book Antiqua"
                        Font-Size="Medium" />
                    <asp:Button ID="btnok" runat="server" Text="Entry" Font-Bold="true" Width="70px"
                        Height="26px" OnClick="btnok_Click" Font-Names="Book Antiqua" Font-Size="Medium" />
                </span><span style="width: 90px; float: right;">
                    <asp:Button ID="btnReport" runat="server" Text="Report" Font-Bold="true" Width="70px"
                        Height="26px" OnClick="btnReport_Click" Font-Names="Book Antiqua" Font-Size="Medium" />
                </span></div> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;
                <center>
                    <div id="Div1" runat="server" style="background-color: #719DDB; width: 977px; height: 25px;
                        color: Black; font-family: Times New Roman; font-size: large; padding-top: 5px">
                        <span runat="server" id="Span1">Report</span>
                    </div>
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;
                <%--<span runat="server" id="lblNote" style="width: 404px; font-family:Book Antiqua; font-weight:bold; float: right; color: Red">
                    Note:If Absent Please enter -1</span> --%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;
                <center>
                    <div id="divReport" runat="server">
                        <asp:GridView ID="GridReport" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                            width: auto;" Font-Names="Times New Roman" Visible="false" BackColor="AliceBlue">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno1" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </div>
                    <div id="divReportNew" runat="server" visible="false">
                        <asp:GridView ID="GridView4" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                            width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" Visible="false"
                            OnRowDataBound="gridview4_onrowdatabound" BackColor="AliceBlue">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno2" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Roll No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrollno" runat="server" Text='<%# Eval("rollno") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reg No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblregno" runat="server" Text='<%#Eval("regno") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Student Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstudtype" runat="server" Text='<%#Eval("student_type") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Application No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblappno" runat="server" Text='<%#Eval("applicationno") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Admission No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbladdmno" runat="server" Text='<%#Eval("admn_no") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Student Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstudname" runat="server" Text='<%#Eval("studname") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q1 Mark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblq1mrk" runat="server" Text='<%#Eval("q1_mrk") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q2 Mark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblq2mrk" runat="server" Text='<%#Eval("q2_mrk") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q3 Mark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblq3mrk" runat="server" Text='<%#Eval("q3_mrk") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Q4 Mark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblq4mrk" runat="server" Text='<%#Eval("q4_mrk") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descriptive Mark">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldescriptmrk" runat="server" Text='<%#Eval("descript_mrk") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quiz Mark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblquizmrk" runat="server" Text='<%#Eval("quiz_mrk") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Assignment Mark">
                                    <ItemTemplate>
                                        <asp:Label ID="lblassignmrk" runat="server" Text='<%#Eval("assign_mrk") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                        <asp:GridView ID="GridView5" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                            width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" Visible="false"
                            BackColor="AliceBlue">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno2" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Roll No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrollno" runat="server" Text='<%# Eval("rollno") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reg No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblregno" runat="server" Text='<%#Eval("regno") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Student Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstudtype" runat="server" Text='<%#Eval("student_type") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Application No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblappno" runat="server" Text='<%#Eval("applicationno") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Admission No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbladdmno" runat="server" Text='<%#Eval("admn_no") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Student Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstudname" runat="server" Text='<%#Eval("studname") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Record Mark(10)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblq1mrk" runat="server" Text='<%#Eval("q1_mrk") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Observation Mark(5)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblq2mrk" runat="server" Text='<%#Eval("q2_mrk") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Internal Mark(10)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblq3mrk" runat="server" Text='<%#Eval("q3_mrk") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </div>
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp; <span style="width: 472px; float: right;"></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;
                <center>
                    <asp:Label ID="lblErrorMsg" runat="server" Text="No Record(s) Found" ForeColor="Red"
                        Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp; <span runat="server" id="lblNote2" style="width: 404px; font-family: Book Antiqua;
                    font-weight: bold; float: right; color: Red">Note:Absent - AAA</span>
                <center>
                    <div id="divPopSpread" runat="server" visible="false" style="height: 220em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <asp:ImageButton ID="btnClose" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                            Style="height: 30px; width: 30px; margin-top: 11%; margin-left: 502px; position: absolute;"
                            OnClick="btnclosespread_OnClick" />
                        <center>
                            <div id="divPopSpreadContent" runat="server" class="table" style="background-color: White;
                                height: auto; width: 72%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                left: 15%; right: 39%; top: 5%; padding: 5px; position: absolute; border-radius: 10px;">
                                <center>
                                    <center style="height: 30px; font-family: Book Antiqua; font-weight: bold; color: Navy">
                                        MARK ENTRY</center>
                                    <table style="height: 100px; width: 100%; padding: 5px;">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lbltab" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Small" ForeColor="Green" Text="Navigation For Box-Tab Key" Visible="false"></asp:Label>
                                                <asp:Label ID="lblSuNo" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCriteriaNO" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblExamCode" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="1" style="height: 30px; width: 470px; border-color: Black; background-color: #DCE4F9;">
                                                    <tr style="background-color: #0CA6CA;">
                                                        <td style="width: 138px; padding-left: 10px;">
                                                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium">Exam Name</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 10px;">
                                                            <asp:Label ID="lblTestTitle" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="background-color: #0CA6CA;">
                                                        <td style="width: 138px; padding-left: 10px;">
                                                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium">Subject Title</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 10px;">
                                                            <asp:Label ID="lblSubName" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="background-color: #0CA6CA;">
                                                        <td style="width: 138px; padding-left: 10px;">
                                                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium">Max Mark</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 10px;">
                                                            <asp:Label ID="lblMaxMark" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="background-color: #0CA6CA;">
                                                        <td style="width: 138px; padding-left: 10px;">
                                                            <asp:Label ID="lblDeg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium">Degree</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 10px;">
                                                            <asp:Label ID="lblDegName" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <fieldset>
                                                    <table>
                                                        <tr>
                                                            <td style="padding-left: 10px;">
                                                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Book Antiqua">Roll No</asp:Label>
                                                            </td>
                                                            <td style="padding-left: 10px;">
                                                                <asp:TextBox ID="txtRollOrReg" runat="server" Enabled="false" CssClass="textbox txtheight4 textbox1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 138px; padding-left: 10px;">
                                                                <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua">Name</asp:Label>
                                                            </td>
                                                            <td style="padding-left: 10px;">
                                                                <asp:Label ID="lblStuName" runat="server" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-left: 10px;">
                                                                <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Book Antiqua">Status</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButtonList ID="rblStatus" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    RepeatDirection="Horizontal" OnSelectedIndexChanged="rblStatus_OnSelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                    <asp:ListItem Text="Present" Value="0" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Absent" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="OD" Value="2"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <center>
                                                                    <asp:Button ID="BtnPerv" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" OnClick="BtnPerv_Click" Text="<<<" Width="70px" /></center>
                                                            </td>
                                                            <td style="padding-left: 10px;">
                                                                <center>
                                                                    <asp:Button ID="btnNext" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" OnClick="btnNext_Click" Text=">>>" Width="70px" /></center>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <%--  <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Panel111" runat="server">
                                                    <div class="GridDock" id="dvGridWidth">
                                                        <asp:GridView ID="GridStudent" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                                            width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" Visible="false"
                                                            BackColor="AliceBlue">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"
                                                                            OnClick="lnkAttMark11" ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                                                                        <%--<asp:Label ID="lblSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Roll No">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lblRollNO" runat="server" Text='<%# Eval("roll_no") %>' OnClick="lnkAttMark11"
                                                                            ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                                                                        <%--  <asp:Label ID="lblRollNO" runat="server" Text='<%# Eval("roll_no") %>'></asp:Label>--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Reg No">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lblregno" runat="server" Text='<%# Eval("Reg_No") %>' OnClick="lnkAttMark11"
                                                                            ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                                                                        <%--<asp:Label ID="lblregno" runat="server" Text='<%# Eval("Reg_No") %>'></asp:Label>--%>
                                                                        <asp:Label ID="lblSec" runat="server" Text='<%# Eval("sections") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lblName" runat="server" Text='<%# Eval("Stud_Name") %>' OnClick="lnkAttMark11"
                                                                            ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                                                                        <%-- <asp:Label ID="lblName" runat="server" Text='<%# Eval("Stud_Name") %>'></asp:Label>--%>
                                                                        <asp:Label ID="lblappno" runat="server" Text='<%# Eval("App_No") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtTotMark" runat="server" Text='<%# Eval("Marks") %>' MaxLength="3"
                                                                            Style="text-align: center" Width="80px" onkeyup="return validatetxt(this)" Enabled="false"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtTotMark"
                                                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                                                        </asp:FilteredTextBoxExtender>
                                                                        <asp:LinkButton ID="lblMark" runat="server" Text='<%# Eval("Marks") %>' OnClick="lnkAttMark11"
                                                                            ForeColor="Black" Font-Underline="false" Visible="false"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                        </asp:GridView>
                                                    </div>
                                                    <br />
                                                    <asp:Button ID="btndeleteAll" runat="server" Text="Delete All" Font-Bold="true" Width="100px"
                                                        Height="26px" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btndeleteAll_Click" />
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <center>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <center>
                                                                    <div id="Format2" runat="server" visible="false">
                                                                        <table border="1" style="height: 200px; width: 208px; border-color: Black; background-color: #DCE4F9;">
                                                                            <tr>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:Label ID="Label7" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium">Q1</asp:Label>
                                                                                    <asp:Label ID="lblQ1Code" runat="server" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:TextBox ID="txtQ1" runat="server" Width="62px" Height="28px" onchange="return calDescTotal()"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtQ1"
                                                                                        FilterType="numbers,custom" ValidChars=" ,.,-">
                                                                                    </asp:FilteredTextBoxExtender>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:Label ID="Label8" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium">Q2</asp:Label>
                                                                                    <asp:Label ID="lblQ2Code" runat="server" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:TextBox ID="txtQ2" runat="server" Width="62px" Height="28px" onchange="return calDescTotal()"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtQ2"
                                                                                        FilterType="numbers,custom" ValidChars=" ,.,-" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:Label ID="Label9" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium">Q3</asp:Label>
                                                                                    <asp:Label ID="lblQ3Code" runat="server" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:TextBox ID="txtQ3" runat="server" Width="62px" Height="28px" onchange="return calDescTotal()"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtQ3"
                                                                                        FilterType="numbers,custom" ValidChars=" ,.,-,-1,-20" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:Label ID="Label10" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium">Q4</asp:Label>
                                                                                    <asp:Label ID="lblQ4Code" runat="server" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:TextBox ID="txtQ4" runat="server" onchange="return calDescTotal()" Width="62px"
                                                                                        Height="28px"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtQ4"
                                                                                        FilterType="numbers,custom" ValidChars=" ,.,-" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:Label ID="Label11" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium">Descriptive Total</asp:Label>
                                                                                    <asp:Label ID="lblQDesc" runat="server" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:Label ID="lblDescTotal" runat="server" Visible="true" Width="62px" Height="28px"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:Label ID="Label12" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium">Quiz Mark</asp:Label>
                                                                                    <asp:Label ID="lblQuiz" runat="server" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:TextBox ID="txtQuizMark" runat="server" Width="62px" Height="28px" onchange="return check()"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtQuizMark"
                                                                                        FilterType="numbers,custom" ValidChars=" ,.,-" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:Label ID="Label13" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium">Assignment Mark</asp:Label>
                                                                                    <asp:Label ID="lblAss" runat="server" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:TextBox ID="txtAssignmntMark" runat="server" Width="62px" Height="28px" onchange="return check()"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtAssignmntMark"
                                                                                        FilterType="numbers,custom" ValidChars=" ,.,-" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <center>
                                                                                        <span style="width: 272px; float: right;">
                                                                                            <asp:Button ID="btnSave" runat="server" Text="Save" Font-Bold="true" Width="54px"
                                                                                                onmouseover="return agecheck()" OnClientClick="getDescTotal()" Height="26px"
                                                                                                OnClick="Save_Click" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                                                            <asp:Button ID="btnDelete" runat="server" Text="Delete" Font-Bold="true" Width="68px"
                                                                                                Height="26px" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="Delete_Click" />
                                                                                        </span>
                                                                                    </center>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div id="Lab" runat="server">
                                                                        <table border="1" style="height: 120px; width: 208px; border-color: Black; background-color: #DCE4F9;">
                                                                            <tr>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:Label ID="lblRecord" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium">Record</asp:Label>
                                                                                    <asp:Label ID="lblRec" runat="server" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:TextBox ID="txtRec" runat="server" Width="62px" Height="28px" onchange="return calDescTotal1()"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtRec"
                                                                                        FilterType="numbers,custom" ValidChars=" ,.,-">
                                                                                    </asp:FilteredTextBoxExtender>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:Label ID="lblObv" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium">Observation</asp:Label>
                                                                                    <asp:Label ID="lblOb" runat="server" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:TextBox ID="txtObser" runat="server" Width="62px" Height="28px" onchange="return calDescTotal1()"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtObser"
                                                                                        FilterType="numbers,custom" ValidChars=" ,.,-" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:Label ID="lblInter" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                                        Font-Size="Medium">Internal</asp:Label>
                                                                                    <asp:Label ID="lblint" runat="server" Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <asp:TextBox ID="txtinternal" runat="server" Width="62px" Height="28px" onchange="return calDescTotal1()"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtinternal"
                                                                                        FilterType="numbers,custom" ValidChars=" ,.,-,-1,-20" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <center>
                                                                                        <span style="width: 272px; float: right;">
                                                                                            <asp:Button ID="Button1" runat="server" Text="Save" Font-Bold="true" Width="54px"
                                                                                                onmouseover="return agecheck1()" OnClientClick="getDescTotal()" Height="26px"
                                                                                                OnClick="Save_Click" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                                                            <asp:Button ID="Button2" runat="server" Text="Delete" Font-Bold="true" Width="68px"
                                                                                                Height="26px" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="Delete_Click" />
                                                                                        </span>
                                                                                    </center>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div id="LabSub" runat="server">
                                                                        <asp:GridView ID="gridLab" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                                                            width: auto;" Font-Names="Times New Roman" OnDataBound="OnDataBound" AutoGenerateColumns="false"
                                                                            Visible="false" BackColor="AliceBlue">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Part">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSuname" runat="server" Text='<%# Eval("subSubjectName") %>'></asp:Label>
                                                                                        <asp:Label ID="lblSubId" runat="server" Text='<%# Eval("subjectId") %>' Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lblExamCode" runat="server" Text='<%# Eval("examCode") %>' Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lblAppNo" runat="server" Text='<%# Eval("appNo") %>' Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("RollNo") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="CO">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblUnitNo" runat="server" Text='<%# Eval("cono") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Alloted">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblregno" runat="server" Text='<%# Eval("maxMark") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Marks">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lbltest" runat="server" Text='<%# Eval("StuMark") %>' Visible="false"></asp:Label>
                                                                                        <asp:TextBox ID="txttest" runat="server" Text='<%# Eval("StuMark") %>' Style="text-align: center"
                                                                                            Width="80px" onkeyup="return validateLab(this)" onBlur="JvfunonBlurLab();" BackColor="AliceBlue"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender57" runat="server" TargetControlID="txttest"
                                                                                            FilterType="Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                                        </asp:GridView>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <center>
                                                                                        <span style="width: 272px; float: right;">
                                                                                            <asp:Button ID="Button3" runat="server" Text="Save" Font-Bold="true" Width="54px"
                                                                                                Height="26px" OnClick="Save_Click" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                                                            <asp:Button ID="Button4" runat="server" Text="Delete" Font-Bold="true" Width="68px"
                                                                                                Height="26px" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="Delete_Click" />
                                                                                        </span>
                                                                                    </center>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <asp:GridView ID="GridView3" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                                                        width: auto;" Font-Names="Times New Roman" OnDataBound="OnDataBound" AutoGenerateColumns="false"
                                                                        Visible="false" BackColor="AliceBlue">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Part No">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPartName" runat="server" Text='<%# Eval("PartName") %>'></asp:Label>
                                                                                    <asp:Label ID="lblPartNo" runat="server" Text='<%# Eval("PartNo") %>' Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="CO">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUnitNo" runat="server" Text='<%# Eval("CourseOutComeNo") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Q.No">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblregno" runat="server" Text='<%# Eval("QNo") %>'></asp:Label>
                                                                                    <asp:Label ID="lblCri" runat="server" Text='<%# Eval("criteria") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblMaterId" runat="server" Text='<%# Eval("MasterID") %>' Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Sub I">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSub1" runat="server" Text='<%# Eval("sub1") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Sub II">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSub2" runat="server" Text='<%# Eval("sub2") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Alloted">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblappno" runat="server" Text='<%# Eval("appno") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblsubid" runat="server" Text='<%# Eval("SubNo") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblmamrk" runat="server" Text='<%# Eval("maxmrk") %>'></asp:Label>
                                                                                    <asp:Label ID="lblExamCode" runat="server" Text='<%# Eval("examCode") %>' Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Marks">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbltest" runat="server" Text='<%# Eval("StuMark") %>' Visible="false"></asp:Label>
                                                                                    <asp:TextBox ID="txttest" runat="server" Text='<%# Eval("StuMark") %>' Style="text-align: center"
                                                                                        Width="80px" onkeyup="return validate(this)" onBlur="JvfunonBlur();" BackColor="AliceBlue"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender57" runat="server" TargetControlID="txttest"
                                                                                        FilterType="Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                                                                    </asp:FilteredTextBoxExtender>
                                                                                    <asp:Label ID="lblmaxval" runat="server" Text="Invalid Mark" ForeColor="Red" Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                                    </asp:GridView>
                                                                </center>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblTot" runat="server" Text="Grand Total : " Style="color: Green;"
                                                                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblGrandTotal" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                                Font-Size="Medium"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="Save" runat="server" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    OnClick="Save_Click" Text="Save" Width="70px" />
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="Delete" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnClick="Delete_Click" Text="Delete" Width="80px" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <center>
                    <div id="divpopalter" runat="server" visible="false" style="height: 550em; z-index: 2000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                        left: 0%;">
                        <center>
                            <div id="divpopaltercontent" runat="server" class="table" style="background-color: White;
                                height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%; padding: 5px;">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblaltermsgs" runat="server" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnokcl" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnokclk_Click"
                                                    Text="OK" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdGridStudent">
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
