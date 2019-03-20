<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Internal.aspx.cs" Inherits="Internal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function checktest() {

            var grid = document.getElementById('<%=GridView2.ClientID%>');
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl3 = document.getElementById('MainContent_GridView2_cbcell_1');
                var check = 0;
                if (ddl3 == true || ddl3 == 1) {
                    check = check + 1;
                    if (check > 1) {
                        alert("Select Single Test Only!");
                    }
                }
            }
        }

        function validate(e) {

            var mk = -16;
            max = parseInt(document.getElementById("<%=txttestbox.ClientID %>").innerHTML);
            if (e.value < mk || e.value > max) {
               // alert(max);
               e.value = "";
               
                alert("Enter Mark Less Than Or Equal To Maximum Mark");
            }

        }

        function validate1(e) {

            var mk = -16;
            max = parseInt(document.getElementById("<%=txttestbox1.ClientID %>").innerHTML);
            if (e.value < mk || e.value > max) {
                // alert(max);
                e.value = "";

                alert("Enter Mark Less Than Or Equal To Maximum Mark");
            }

        }

        function validate2(e) {

            var mk = -16;
            max = parseInt(document.getElementById("<%=txttestbox2.ClientID %>").innerHTML);
            if (e.value < mk || e.value > max) {
                // alert(max);
                e.value = "";

                alert("Enter Mark Less Than Or Equal To Maximum Mark");
            }

        }
        function validate3(e) {

            var mk = -16;
            max = parseInt(document.getElementById("<%=txttestbox3.ClientID %>").innerHTML);
            if (e.value < mk || e.value > max) {
                // alert(max);
                e.value = "";

                alert("Enter Mark Less Than Or Equal To Maximum Mark");
            }

        }
        function validate4(e) {

            var mk = -16;
            max = parseInt(document.getElementById("<%=txttestbox4.ClientID %>").innerHTML);
            if (e.value < mk || e.value > max) {
                // alert(max);
                e.value = "";

                alert("Enter Mark Less Than Or Equal To Maximum Mark");
            }

        }

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
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
            font-family: "auto Trebuchet MS" , Verdana;
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <span id="lblStripHead" runat="server" class="fontstyleheader" style="color: Green;">
                CAM Entry </span>
        </center>
        <center>
            <asp:Panel ID="pnlHeadingCAM" runat="server" Height="52px">
                <table class="maintablestyle" style="height: 30px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="21px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBatch" runat="server" Height="20px" Width="59px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True" Style="">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style="width: 100px; height: 21px;"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Width="74px" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td class="style1">
                            <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style="height: 21px; width: 52px"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                Width="290px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblSemYr" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style="height: 20px; width: 33px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px;
                                width: 44px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td class="style5">
                            <asp:DropDownList ID="ddlSec" runat="server" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px;
                                width: 47px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 25px; width: 40px" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBatch"
                    ErrorMessage="Select Batch" Font-Bold="True" ForeColor="#FF3300" InitialValue="-1">Select Batch</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlDegree"
                    ErrorMessage="Select Degree" Font-Bold="True" ForeColor="#FF3300" InitialValue="-1">Select Degree</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlBranch"
                    ErrorMessage="Select Branch" Font-Bold="True" ForeColor="#FF3300" InitialValue="-1">Select Branch</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlSemYr"
                    ErrorMessage="Select Sem" Font-Bold="True" ForeColor="#FF3300" InitialValue="-1">Select Sem</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlSec"
                    ErrorMessage="Select Section" Font-Bold="True" ForeColor="#FF3300" InitialValue="-1">Select Section</asp:RequiredFieldValidator>
            </asp:Panel>
        </center>
    </div>
    <asp:Label ID="lblErrorMsg" runat="server" Text="There are no Records Found" ForeColor="Red"
        Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
    <asp:Label ID="lblSubNo" runat="server" Visible="false"></asp:Label>
    <center>
        <asp:Panel ID="pnlEntry" runat="server" Height="215px" Width="835px">
            <asp:CheckBox ID="chkmarkattendance" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Mark Attendance" Width="150px" AutoPostBack="true" OnCheckedChanged="chkmarkattendance_CheckedChanged" />
        </asp:Panel>
        <br />
        <center>
            <asp:Panel ID="pHeaderEntry" runat="server" CssClass="style8" Height="16px" Width="949px">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                    ID="Labelpersonal" Text="Subject Details" runat="server" Font-Size="Medium" Font-Bold="True"
                    Font-Names="Book Antiqua" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Image ID="Imagepersonal" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
        </center>
        <br />
        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
        <asp:GridView ID="GridView1" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
            width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" OnRowCreated="OnRowCreated"
            OnRowDataBound="gridview1_OnRowDataBound" OnSelectedIndexChanged="SelectedIndexChanged"
            BackColor="AliceBlue">
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Batch_Year">
                    <ItemTemplate>
                        <asp:Label ID="lblbatchyear" runat="server" Text='<%# Eval("batch_year") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Degree">
                    <ItemTemplate>
                        <asp:Label ID="lbldegreecode" runat="server" Text='<%# Eval("degree_code") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lbldegree" runat="server" Text='<%# Eval("degree") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Semester">
                    <ItemTemplate>
                        <asp:Label ID="lblsem" runat="server" Text='<%# Eval("Semester") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Section">
                    <ItemTemplate>
                        <asp:Label ID="lblsection" runat="server" Text='<%#Eval("Section") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subject">
                    <ItemTemplate>
                        <asp:Label ID="lblsubject" runat="server" Text='<%#Eval("Subject") %>' Font-Underline="true"
                            ForeColor="Blue"></asp:Label>
                        <asp:Label ID="lblsubjectno" runat="server" Text='<%#Eval("subject_no") %>' Visible="false"></asp:Label>
                        <%--<asp:LinkButton ID="lbllink_4" runat="server" Text='<%#Eval("Subject") %>' OnClick="subjectlink_OnClick"></asp:LinkButton>--%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="380px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subject_Code">
                    <ItemTemplate>
                        <asp:Label ID="lblsubcode" runat="server" Text='<%#Eval("Subject_Code") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
        </asp:GridView>
        </br>
        <center>
            <asp:Panel ID="pHeaderReport" runat="server" CssClass="style9" Width="949px">
                <asp:Label ID="lblSub" Text="Test Details" runat="server" Font-Size="Medium" Font-Bold="True"
                    Font-Names="Book Antiqua" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Image ID="Image1" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
        </center>
        <asp:GridView ID="GridView2" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
            width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" Visible="false"
            OnRowDataBound="gridview2_OnRowDataBound" BackColor="AliceBlue" Width="700px">
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
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Test">
                    <ItemTemplate>
                        <asp:Label ID="lbltest" runat="server" Text='<%# Eval("test") %>'></asp:Label>
                        <asp:Label ID="lblsubdet" runat="server" Text='<%# Eval("subdetails") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="250px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ExamCode" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblexamcode" runat="server" Text='<%# Eval("examcode")  %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Exam-Date">
                    <ItemTemplate>
                        <asp:Label ID="lblexamdate" runat="server" Text='<%#Eval("examdate") %>' Visible="false"></asp:Label>
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
                        <%-- <asp:LinkButton ID="lbllink" runat="server" Text='<%#Eval("Subject") %>' OnClick="subjectlink_OnClick"></asp:LinkButton>--%>
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
                <asp:TemplateField HeaderText="Hrs">
                    <ItemTemplate>
                        <asp:Label ID="lblhrs" runat="server" Text='<%#Eval("durationhrs") %>' Visible="false"></asp:Label>
                        <asp:DropDownList ID="ddlhrs" runat="server" OnSelectedIndexChanged="ddlhrs_OnSelectedIndexedChanged">
                        </asp:DropDownList>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mins">
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
                <asp:TemplateField HeaderText="Start Period" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblstartperiod" runat="server" Text='<%#Eval("start_period") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="End Period" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblendperiod" runat="server" Text='<%#Eval("end_period") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CriteriaNo" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblcriteriano" runat="server" Text='<%#Eval("criteria_no") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
        </asp:GridView>
        <table>
            <tr>
                <td style="width: 120px; text-align: right;">
                    <asp:Label ID="lblselectstaff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Staff Name"></asp:Label>
                </td>
                <td style="width: 165px;">
                    <asp:DropDownList ID="ddlstaffname" runat="server" Font-Bold="True" Width="160px"
                        Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td style="width: 150px;">
                    <asp:CheckBox ID="chkretest" Text="Include Re-Test" runat="server" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="138px" Font-Bold="true" OnCheckedChanged="chkretest_CheckedChanged" />
                </td>
                <td style="text-align: right; width: 130px;">
                    <asp:Label ID="lblRetestMin" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Retest Min Mark" Visible="false"></asp:Label>
                </td>
                <td style="width: 80px;">
                    <asp:TextBox ID="txt_RetestMin" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width=" 70px" MaxLength="3" onchange="checkFloatValue(this);"
                        Visible="false"></asp:TextBox>
                </td>
                <td align="right">
                    <asp:Button ID="btnok" runat="server" Text="Ok" OnClick="btnok_Click" Font-Bold="true"
                        Width="54px" Height="26px" Font-Names="Book Antiqua" Font-Size="Medium" OnClientClick="checktest()" />
                </td>
            </tr>
        </table>
    </center>
    <center>
        <asp:Panel ID="pnlReport" runat="server">
            </br>
            <asp:Label ID="lblerror" runat="server" Text="" ForeColor="Red" Visible="False" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            <asp:Label ID="lblnorec" runat="server" Text="There are no Records Found" ForeColor="Red"
                Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
        </asp:Panel>
    </center>
    <table class="style11">
        <tr>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <%--<td align="right">
                    <asp:Button ID="Exit1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Visible="false" Font-Size="Medium" Text="Exit" Width="60px" OnClick="Exit1_Click"
                        CausesValidation="False" />
                </td>--%>
            </td>
        </tr>
    </table>
    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlEntry"
        CollapseControlID="pHeaderEntry" ExpandControlID="pHeaderEntry" AutoExpand="true"
        AutoCollapse="false" TextLabelID="Labelpersonal" CollapsedSize="0" ImageControlID="Imagepersonal"
        CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
    </asp:CollapsiblePanelExtender>
    <asp:CheckBox ID="chkGrp" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="chkGrp_CheckedChanged"
        Text="Use Test Group" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
    <asp:DropDownList ID="ddlGrp" runat="server" AutoPostBack="True" Enabled="False"
        OnSelectedIndexChanged="ddlGrp_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium">
    </asp:DropDownList>
    <br />
    <br />
    <div>
        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="pnlReport"
            CollapseControlID="pHeaderReport" ExpandControlID="pHeaderReport" AutoExpand="true"
            AutoCollapse="false" TextLabelID="lblSub" CollapsedSize="0" ImageControlID="Image1"
            CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
        </asp:CollapsiblePanelExtender>
    </div>
    <br />
    <center>
        <div id="posalign" runat="server">
            <asp:Panel runat="server" ID="pnlcover" Visible="false">
                <asp:Panel ID="pHeaderSettings" runat="server" CssClass="style10" Visible="false">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                        ID="Label4" Text="Student Details" runat="server" Font-Size="Medium" Font-Bold="True"
                        Font-Names="Book Antiqua" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Image ID="Image2" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
                </asp:Panel>
            </asp:Panel>
        </div>
        <asp:Panel ID="pnlSettings" runat="server" BorderColor="Black">
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblrecord" runat="server" Visible="False" Font-Bold="True" Text="Records Per Page :"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                                Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="24px" Width="58px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                                AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        </td>
                        <td>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="TextBoxother"
                                FilterType="Numbers" />
                        </td>
                        <td>
                            <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search :" Visible="False"
                                Width="97px" Font-Names="Book Antiqua" Font-Size="Medium" Height="21px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                                OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="16px" Width="34px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="TextBoxpage"
                                FilterType="Numbers" />
                        </td>
                        <td>
                            <asp:Label ID="lblPageSearch" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                </table>
                <%--<div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <center>
                                <center>
                                </center>
                            </center>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>--%>
            </div>
            <br />
            <asp:Label ID="lblexcelerror" runat="server" ForeColor="Red" Visible="false" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" Text="Kindly Enter report name"></asp:Label>
            <br />
            <br />
        </asp:Panel>
    </center>
    <br />
    <%--
    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="pnlSettings"
        CollapseControlID="pHeaderSettings" ExpandControlID="pHeaderSettings" AutoCollapse="false"
        AutoExpand="true" TextLabelID="Label4" CollapsedSize="0" ImageControlID="Image2"
        CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
    </asp:CollapsiblePanelExtender>--%>
    <asp:HiddenField runat="server" ID="hfcon" />
    <asp:ModalPopupExtender ID="ModalPopupExtender2" Drag="True" CancelControlID="Btnpanelexit"
        TargetControlID="Delete" PopupControlID="pnldeleterecord" runat="server" BackgroundCssClass="ModalPopupBG"
        DynamicServicePath="" Enabled="True">
    </asp:ModalPopupExtender>
    <asp:HiddenField ID="hfphoto" runat="server" />
    <asp:Panel ID="pnldeleterecord" runat="server" CssClass="modalPopup" Style="display: none;
        height: 500; width: 500;" DefaultButton="btnOk">
        <table width="500">
            <tr class="topHandle">
                <td colspan="2" align="left" runat="server" id="td1">
                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                        Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 60px" valign="middle" align="center">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Info-48x48.png" />
                </td>
                <td valign="middle" align="left">
                    <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>--%>
                    <asp:Label ID="Label7" Text="Do You Want To Delete The Record" runat="server" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="Btnpaneldelete" runat="server" Text="Yes" OnClick="btnpaneldelete_Click"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    <asp:Button ID="Btnpanelexit" runat="server" Text="No" OnClick="btnpanelexit_Click"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table>
        <tr>
            <td>
                <asp:PlaceHolder ID="placeholder1" runat="server"></asp:PlaceHolder>
            </td>
        </tr>
    </table>
    <center>
        <div id="divPopSpread" runat="server" visible="false" style="height: 220em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
          <%--  <asp:ImageButton ID="btnClose" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                Style="height: 30px; width: 30px; margin-top: 11%; margin-left: 502px; position: absolute;"
                OnClick="btnclosespread_OnClick" />--%>
            <center>
                <div id="divPopSpreadContent" runat="server" class="table" style="background-color: White;
                    height: auto; width: 66%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 20%; right: 39%; top: 5%; padding: 5px; position: absolute; border-radius: 10px;">
                    <center>
                        <center style="height: 30px; color: Navy">
                            MARK ENTRY</center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Text="Note:Please Enter If AAA:-1|EOD:-3|P:Mark>=0"></asp:Label>
                                <asp:Label ID="lblnote2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Text="ML:-4|SOD:-5|NSS:-6|NJ:-7|S:-8|L:-9|OD:-16"></asp:Label>
                                <asp:Label ID="lblNote3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Text="NCC:-10|HS:-11|PP:-12|SYOD:-13|COD:-14|OOD:-15|EL:-2|RAA:-18"></asp:Label></tr>
                            <tr>
                             
                                <td align="right">
                                    <asp:Label ID="lbltab" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Small" ForeColor="Green" Text="Navigation For Box-Tab Key" Visible="false"></asp:Label>
                                </td>
                            </tr>
<tr><td align="center">
            
                <asp:Label ID="lblsubtstdet" Text="Test Details" runat="server" Font-Size="Medium" Font-Bold="True" ForeColor="Navy"
                    Font-Names="Book Antiqua" />
              
                             
      </td>  </tr>


                          <tr>
                          <td align="center">
                           <asp:Label ID="lbltsthead1" runat="server" Visible="true" ForeColor="Navy" ></asp:Label>
                          </td></tr><tr>
                          <td align="center">
                          <asp:Label ID="lblmi" runat="server" Visible="true" Text="Minimum Mark:" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                           <asp:Label ID="textboxmin" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True" ></asp:Label>
                          
                             <asp:Label ID="lblmk" runat="server" Visible="true" Text="Maximum Mark:" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                              <asp:Label ID="txttestbox" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True" ></asp:Label>
                                                            
                            </td>
                           
                          </tr>
                           <tr>
                          <td align="center">
                           <asp:Label ID="lbltsthead2" runat="server" Visible="true" ForeColor="Navy" ></asp:Label>
                          </td></tr><tr>
                          <td align="center">
                          <asp:Label ID="lblmi1" runat="server" Visible="true" Text="Minimum Mark:" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                           <asp:Label ID="textboxmin1" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label> 
                            
                             <asp:Label ID="lblmk1" runat="server" Visible="true" Text="Maximum Mark:" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                           <asp:Label ID="txttestbox1" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True" ></asp:Label>
                  
                            </td>
                           
                          </tr>
                           <tr>
                          <td align="center">
                           <asp:Label ID="lbltsthead3" runat="server" Visible="true" ForeColor="Navy" ></asp:Label>
                          </td></tr><tr>
                          <td align="center">
                          <asp:Label ID="lblmi2" runat="server" Visible="true" Text="Minimum Mark:" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                           <asp:Label ID="textboxmin2" runat="server" Visible="true"  Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                          
                             <asp:Label ID="lblmk2" runat="server" Visible="true" Text="Maximum Mark:" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                           <asp:Label ID="txttestbox2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True" ></asp:Label>
                                                     
                            </td>
                           
                          </tr>
                           <tr>
                          <td align="center">
                           <asp:Label ID="lbltsthead4" runat="server" Visible="true" ForeColor="Navy" ></asp:Label>
                          </td></tr><tr>
                          <td align="center">
                          <asp:Label ID="lblmi3" runat="server" Visible="true" Text="Minimum Mark:" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                           <asp:Label ID="textboxmin3" runat="server" Visible="true"  Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                          
                             <asp:Label ID="lblmk3" runat="server" Visible="true" Text="Maximum Mark:" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                           <asp:Label ID="txttestbox3" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True" ></asp:Label>
                                                     
                            </td>
                           
                          </tr>
                          <tr>
                          <td align="center">
                           <asp:Label ID="lbltsthead5" runat="server" Visible="true" ForeColor="Navy" ></asp:Label>
                          </td></tr><tr>
                          <td align="center">
                          <asp:Label ID="lblmi4" runat="server" Visible="true" Text="Minimum Mark:" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                           <asp:Label ID="textboxmin4" runat="server" Visible="true"  Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                          
                             <asp:Label ID="lblmk4" runat="server" Visible="true" Text="Maximum Mark:" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True"></asp:Label>
                           <asp:Label ID="txttestbox4" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Maroon" Font-Bold="True" ></asp:Label>
                                    <asp:Label ID="lblsubexl" runat="server" Visible="false" ></asp:Label>
                                     <asp:Label ID="lblsubcout" runat="server" Visible="false" ></asp:Label>
                                     <asp:Label ID="lbltxtbxnam" runat="server" Visible="false" ></asp:Label>
                                     <asp:Label ID="tstnameget" runat="server" Visible="false"></asp:Label>
                                                     
                            </td>
                           
                          </tr>

                            <tr>
                                <td>
                                    <center>
                                        <asp:GridView ID="GridView3" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                            width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" Visible="false"
                                            OnRowDataBound="gridview3_OnRowDataBound" OnDataBound="gridview3_databound" BackColor="AliceBlue">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Roll No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrollno" runat="server" Text='<%# Eval("rollno") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reg No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblregno" runat="server" Text='<%# Eval("regno") %>'></asp:Label>
                                                        <asp:Label ID="lblsections1" runat="server" Text='<%# Eval("section") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lbldegreecode2" runat="server" Text='<%# Eval("degree") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblsems2" runat="server" Text='<%# Eval("semester") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblbatch2" runat="server" Text='<%# Eval("batch") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblsubno2" runat="server" Text='<%# Eval("subno") %>' Visible="false"></asp:Label>
                                                          <asp:Label ID="lblexmcd" runat="server" Text='<%# Eval("examcode") %>' Visible="false"></asp:Label>
                                                       
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maximum Mark">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblappno" runat="server" Text='<%# Eval("appno") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblsubid" runat="server" Text='<%# Eval("subId") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblmamrk" runat="server" Text='<%# Eval("maxmrk") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblmin" runat="server" Text='<%# Eval("minmrk") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Student Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstudname" runat="server" Text='<%# Eval("studname") %>' Font-Bold="false"></asp:Label>
                                                        <asp:Label ID="lblcollcode" runat="server" Text='<%# Eval("collcode") %>' Visible="false"></asp:Label>
                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Test">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltest" runat="server" Text='<%# Eval("test") %>' Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txttest" runat="server" Text='<%# Eval("test") %>' 
                                                            Style="text-align: center" Width="80px" onkeyup="return validate(this)" BackColor="AliceBlue"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txttest"
                                                            FilterType="numbers,custom,UppercaseLetters" ValidChars=".-">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:Label ID="lblmaxval" runat="server" Text="Invalid Mark" ForeColor="Red" Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                </asp:TemplateField>
                                               
                                                 <asp:TemplateField HeaderText="Test1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltest1" runat="server" Text='<%# Eval("test1") %>' Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txttest1" runat="server" Text='<%# Eval("test1") %>' 
                                                            Style="text-align: center" Width="80px" onkeyup="return validate1(this)" BackColor="AliceBlue"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txttest1"
                                                            FilterType="numbers,custom,UppercaseLetters" ValidChars=".-">
                                                        </asp:FilteredTextBoxExtender>
                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                </asp:TemplateField>
                                              
                                                 <asp:TemplateField HeaderText="Test2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltest2" runat="server" Text='<%# Eval("test2") %>' Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txttest2" runat="server" Text='<%# Eval("test2") %>' 
                                                            Style="text-align: center" Width="80px" onkeyup="return validate2(this)" BackColor="AliceBlue"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txttest2"
                                                            FilterType="numbers,custom,UppercaseLetters" ValidChars=".-">
                                                        </asp:FilteredTextBoxExtender>
                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                </asp:TemplateField>
                                              
                                                 <asp:TemplateField HeaderText="Test3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltest3" runat="server" Text='<%# Eval("test3") %>' Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txttest3" runat="server" Text='<%# Eval("test3") %>' 
                                                            Style="text-align: center" Width="80px" onkeyup="return validate3(this)" BackColor="AliceBlue"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txttest3"
                                                            FilterType="numbers,custom,UppercaseLetters" ValidChars=".-">
                                                        </asp:FilteredTextBoxExtender>
                                                       
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                </asp:TemplateField>
                                               
                                                 <asp:TemplateField HeaderText="Test4">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltest4" runat="server" Text='<%# Eval("test4") %>' Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txttest4" runat="server" Text='<%# Eval("test4") %>' 
                                                            Style="text-align: center" Width="80px" onkeyup="return validate4(this)" BackColor="AliceBlue"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txttest4"
                                                            FilterType="numbers,custom,UppercaseLetters" ValidChars=".-">
                                                        </asp:FilteredTextBoxExtender>
                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField HeaderText="Test5">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltest5" runat="server" Text='<%# Eval("test5") %>' Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txttest5" runat="server" Text='<%# Eval("test5") %>' 
                                                            Style="text-align: center" Width="80px" onkeyup="return validate(this)" BackColor="AliceBlue"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender175" runat="server" TargetControlID="txttest5"
                                                            FilterType="numbers,custom,UppercaseLetters" ValidChars=".-">
                                                        </asp:FilteredTextBoxExtender>
                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                </asp:TemplateField>
                                               
                                                 <asp:TemplateField HeaderText="Test6">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltest6" runat="server" Text='<%# Eval("test6") %>' Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txttest6" runat="server" Text='<%# Eval("test6") %>' 
                                                            Style="text-align: center" Width="80px" onkeyup="return validate(this)" BackColor="AliceBlue"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender167" runat="server" TargetControlID="txttest6"
                                                            FilterType="numbers,custom,UppercaseLetters" ValidChars=".-">
                                                        </asp:FilteredTextBoxExtender>
                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                </asp:TemplateField>
                                               
                                                 <asp:TemplateField HeaderText="Test7">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltest7" runat="server" Text='<%# Eval("test7") %>' Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txttest7" runat="server" Text='<%# Eval("test7") %>' 
                                                            Style="text-align: center" Width="80px" onkeyup="return validate(this)" BackColor="AliceBlue"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender178" runat="server" TargetControlID="txttest7"
                                                            FilterType="numbers,custom,UppercaseLetters" ValidChars=".-">
                                                        </asp:FilteredTextBoxExtender>
                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                </asp:TemplateField>
                                               
                                                 <asp:TemplateField HeaderText="Test8">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltest8" runat="server" Text='<%# Eval("test8") %>' Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txttest8" runat="server" Text='<%# Eval("test8") %>' 
                                                            Style="text-align: center" Width="80px" onkeyup="return validate(this)" BackColor="AliceBlue"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender817" runat="server" TargetControlID="txttest8"
                                                            FilterType="numbers,custom,UppercaseLetters" ValidChars=".-">
                                                        </asp:FilteredTextBoxExtender>
                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                </asp:TemplateField>
                                               
                                                 <asp:TemplateField HeaderText="Test9">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltest9" runat="server" Text='<%# Eval("test9") %>' Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txttest9" runat="server" Text='<%# Eval("test9") %>' Visible="false"
                                                            Style="text-align: center" Width="80px" onkeyup="return validate(this)" BackColor="AliceBlue"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender971" runat="server" TargetControlID="txttest9"
                                                            FilterType="numbers,custom,UppercaseLetters" ValidChars=".-">
                                                        </asp:FilteredTextBoxExtender>
                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Re-Test">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblretest" runat="server" Text='<%# Eval("retest") %>' Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txtretest" runat="server" Text='<%# Eval("retest") %>' 
                                                            Width="80px" Style="text-align: center" BackColor="AliceBlue" onkeyup="validate(this);"></asp:TextBox>
                                                       
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtretest"
                                                            FilterType="numbers,custom,UppercaseLetters" ValidChars=".-">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                                                </asp:TemplateField>
                                                
                                            </Columns>
                                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                        </asp:GridView>
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
                                        Font-Size="Medium" OnClick="Delete_Click" Text="Delete" Width="65px" />
                                </td>
                                <td align="right" style="width: 50px;">
                                    <asp:Label ID="lblrptname" runat="server" Width="120px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name" style="margin-left: -21px;"></asp:Label>
                                </td>
                                <td align="left" style="width: 10px;">
                                    <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
                                        Width="150px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="131px" />
                                </td>
                                 <td>
                                    <asp:Button ID="btnclosepopup" runat="server" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"  Text="Close"
                                        OnClick="btnclosepopup_Click"  Width="70px" Height="26px" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td style="width: auto;">
                                    <asp:FileUpload runat="server" ID="fpmarkexcel" Visible="false" Font-Names="Book Antiqua"
                                        Font-Bold="True" Font-Size="Medium" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_import" Text="Import" runat="server" Visible="false" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_importex" />
                                </td>
                               <%-- <td>
                                    <asp:Button ID="Buttonexit" runat="server" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Text="Exit"
                                        OnClick="Buttonexit_Click"  Width="70px" Height="26px" />
                                </td>--%>
                               
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
   
    <asp:Label ID="lblmin1" runat="server" Visible="false"></asp:Label>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            AutoPostBack="False" CssClass="textbox textbox1" Style="height: auto; width: auto;"
                                            OnClick="btnPopAlertClose_Click" Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
