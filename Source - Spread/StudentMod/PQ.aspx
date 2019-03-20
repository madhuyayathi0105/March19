<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="PQ.aspx.cs" Inherits="StudentMod_PQ" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        function hideclick() {
            document.getElementById('<%=panel4.ClientID%>').style.display = 'none';
            return false;
        }
        function verifystatus() {
            var agevalue = document.getElementById("<%=txt_keyword.ClientID %>").value;
            if (agevalue == "20jcpnm16") {
                document.getElementById('<%=paneltemp.ClientID%>').style.display = 'none';
                return true;
            }
            else {
                document.getElementById('<%=paneltemp.ClientID%>').style.display = 'none';
                alert('Enter correct Key Word');
                return false;
            }
        }
        function showpopup() {
            document.getElementById("<%=txt_keyword.ClientID %>").value = "";
            document.getElementById('<%=paneltemp.ClientID%>').style.display = 'block';
            return false;
        }   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <center>
                <span style="font-weight: bold; font-size: large; color: Green;">PQ Selection</span>
            </center>
            <br />
            <div style="height: auto; width: 1000px;">
                <div>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:RadioButton ID="rdbug" runat="server" GroupName="same" Text="UG" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdbpg" runat="server" GroupName="same" Text="PG" />
                            </td>
                            <td>
                                <asp:TextBox ID="txt_appno" runat="server" TextMode="MultiLine" Width="300px" Height="40px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClick="Btn_click" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbsports" runat="server" Text="Sports Quota / NRI / Foreign" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <center>
                    <div>
                        <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    </div>
                </center>
                <br />
                <div>
                    <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" AutoPostBack="true" OnButtonCommand="FpSpread3_command" Visible="false"
                        HierBar-ShowParentRow="False">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <br />
                <div>
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbselect" runat="server" Visible="false" Text="Select All" AutoPostBack="true"
                                    OnCheckedChanged="cbselect_Change" />
                            </td>
                            <td>
                                <asp:Button ID="button" runat="server" Visible="false" Text="Generate Admit Card"
                                    OnClientClick="return showpopup()" />
                            </td>
                            <td>
                                <asp:Button ID="print" runat="server" Visible="false" Text="Print" OnClick="btnadmitprint_click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <asp:Panel ID="paneltemp" runat="server" Style="display: none; height: 100em; z-index: 1000;
                    width: 100%; position: absolute; top: 0; left: 0;">
                    <center>
                        <div style="background-color: #FFFFFF; height: 200px; margin-top: 180px; width: 300px;">
                            <br />
                            <br />
                            <span id="Span1" runat="server">Enter your key word</span>&nbsp;
                            <asp:TextBox ID="txt_keyword" runat="server" TextMode="Password"></asp:TextBox>
                            <br />
                            <br />
                            <br />
                            <br />
                            <asp:Button ID="Button4" runat="server" Style="background-color: rgb(0, 128, 128);
                                border: 0px; color: White;" Text="Okay" OnClientClick="return verifystatus()"
                                OnClick="btn_confirm_clcik" />
                            <%-- <button style="background-color: rgb(0, 128, 128); border: 0px; color: White;">
                    Okay</button>--%>
                        </div>
                    </center>
                </asp:Panel>
                <asp:Panel ID="panel4" runat="server" Style="background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83);
                    border-color: inherit; border-style: none; border-width: 1px; height: 101%; left: -8px;
                    position: absolute; top: -9px; width: 101%; display: none; z-index: 10000;" BorderColor="Blue">
                    <asp:Panel ID="panel6" runat="server" Visible="true" BackColor="mediumaquamarine"
                        Style="border-style: none; border-color: inherit; border-width: 1px; height: 590px;
                        width: 985px; left: 7px; top: 67px; position: absolute;" BorderColor="Blue">
                        <br />
                        <div class="panel6" id="Div1" style="text-align: center; font-family: Book Antiqua;
                            font-size: medium; font-weight: bold">
                            <caption style="top: 20px; border-style: solid; border-color: Black; position: absolute;
                                left: 200px">
                                <asp:Label ID="Label17" runat="server" Text="Student Details" Font-Bold="true" Font-Size="Large"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </caption>
                            <asp:Button ID="Button5" runat="server" Text="X" ForeColor="Black" OnClientClick="return hideclick()"
                                Style="top: 0px; left: 942px; position: absolute; height: 26px; border-width: 0;
                                background-color: mediumaquamarine; width: 25px;" Font-Bold="True" Font-Names="Microsoft Sans Serif"
                                Font-Size="Medium" />
                            <br />
                            <br />
                            <asp:Panel ID="panel3" runat="server" Visible="true" BackColor="Lavender" Height="500px"
                                ScrollBars="Vertical" Style="border-style: none; border-color: inherit; border-width: 1px;
                                width: 949px; left: 21px; position: absolute;" BorderColor="Blue">
                                <table align="right">
                                    <tr align="right">
                                        <td align="right">
                                            <asp:Button ID="Button6" runat="server" Font-Bold="True" Visible="false" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Print" Width="50px" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <center>
                                    <div id="coursedetails">
                                        <table>
                                            <tr>
                                                <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                    <span style="font-weight: bold; font-size: large;">Course Information</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Stream</span>
                                                </td>
                                                <td>
                                                    <span id="college_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Graduation</span>
                                                </td>
                                                <td>
                                                    <span id="degree_Span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Degree</span>
                                                </td>
                                                <td>
                                                    <span id="graduation_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Subject / Course</span>
                                                </td>
                                                <td>
                                                    <span id="course_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: right;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                    <span style="font-weight: bold; font-size: large;">Personal Information</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Applicant Name</span>
                                                </td>
                                                <td>
                                                    <span id="applicantname_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>
                                                    <span>Last Name</span>
                                                </td>
                                                <td>
                                                    <span id="lastname_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Date of birth</span>
                                                </td>
                                                <td>
                                                    <span id="dob_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Gender</span>
                                                </td>
                                                <td>
                                                    <span id="gender_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Parent Name</span>
                                                </td>
                                                <td>
                                                    <span id="parent_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>
                                                    <span>RelationShip</span>
                                                </td>
                                                <td>
                                                    <span id="relationship_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Occupation</span>
                                                </td>
                                                <td>
                                                    <span id="occupation_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Mother Tongue</span>
                                                </td>
                                                <td>
                                                    <span id="mothertongue_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Religion</span>
                                                </td>
                                                <td>
                                                    <span id="religion_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Nationality</span>
                                                </td>
                                                <td>
                                                    <span id="nationality_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Community</span>
                                                </td>
                                                <td>
                                                    <span id="commuity_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Caste</span>
                                                </td>
                                                <td>
                                                    <span id="Caste_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Are You of Tamil Origin From Andaman and Nicobar Islands ?</span>
                                                </td>
                                                <td>
                                                    <span id="tamilorigin_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Are You a Child of an ex-serviceman of Tamil Nadu origin ?</span>
                                                </td>
                                                <td>
                                                    <span id="Ex_service_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Are You Differently abled</span>
                                                </td>
                                                <td>
                                                    <span id="Differentlyable_Span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Are you first genaration learner ?</span>
                                                </td>
                                                <td>
                                                    <span id="first_generation_Span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Is Residence on Campus Required ? </span>
                                                </td>
                                                <td>
                                                    <span id="residancerequired_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Distinction in Sports </span>
                                                </td>
                                                <td>
                                                    <span id="sport_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Extra Curricular Activites/Co-Curricular Activites </span>
                                                </td>
                                                <td>
                                                    <span id="Co_Curricular_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Are you NCC cadet?</span>
                                                </td>
                                                <td>
                                                    <span id="ncccadetspan" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                    <span style="font-weight: bold; font-size: large;">Communication Address</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Address Line1</span>
                                                </td>
                                                <td>
                                                    <span id="caddressline1_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Address Line2</span>
                                                </td>
                                                <td>
                                                    <span id="Addressline2_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Address Line3</span>
                                                </td>
                                                <td>
                                                    <span id="Addressline3_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>City</span>
                                                </td>
                                                <td>
                                                    <span id="city_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>State</span>
                                                </td>
                                                <td>
                                                    <span id="state_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Country</span>
                                                </td>
                                                <td>
                                                    <span id="Country_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>PIN Code</span>
                                                </td>
                                                <td>
                                                    <span id="Postelcode_Span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Mobile Number</span>
                                                </td>
                                                <td>
                                                    <span id="Mobilenumber_Span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Alternate Mobile No</span>
                                                </td>
                                                <td>
                                                    <span id="Alternatephone_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Email ID</span>
                                                </td>
                                                <td>
                                                    <span id="emailid_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Phone Number with (Landline) STD/ISD code</span>
                                                </td>
                                                <td>
                                                    <span id="std_ist_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                    <span style="font-weight: bold; font-size: large;">Permanent Address</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Address Line1</span>
                                                </td>
                                                <td>
                                                    <span id="paddressline1_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Address Line2</span>
                                                </td>
                                                <td>
                                                    <span id="paddressline2_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Address Line3</span>
                                                </td>
                                                <td>
                                                    <span id="paddressline3_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>City</span>
                                                </td>
                                                <td>
                                                    <span id="pcity_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>State</span>
                                                </td>
                                                <td>
                                                    <span id="pstate_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Country</span>
                                                </td>
                                                <td>
                                                    <span id="pcountry_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>PIN Code</span>
                                                </td>
                                                <td>
                                                    <span id="ppostelcode_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>
                                                    <span>Mobile Number</span>
                                                </td>
                                                <td>
                                                    <span id="pmobilenumber_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>
                                                    <span>Alternate Mobile No</span>
                                                </td>
                                                <td>
                                                    <span id="palternatephone_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>
                                                    <span>Email ID</span>
                                                </td>
                                                <td>
                                                    <span id="peamilid_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Phone Number with (Landline) STD/ISD code</span>
                                                </td>
                                                <td>
                                                    <span id="pstdisd_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: right;">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="Academicinfo">
                                        <div id="ugdiv_verification" runat="server" visible="false">
                                            <table>
                                                <tr>
                                                    <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                        <span style="font-weight: bold; font-size: large;">Academic Information</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Qualifying Examination Pass</span>
                                                    </td>
                                                    <td>
                                                        <span id="qualifyingexam_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Name of School</span>
                                                    </td>
                                                    <td>
                                                        <span id="Nameofschool_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Location of School</span>
                                                    </td>
                                                    <td>
                                                        <span id="locationofschool_Span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Medium of Study of Qualifying Examination</span>
                                                    </td>
                                                    <td>
                                                        <span id="mediumofstudy_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Qualifying Board & State</span>
                                                    </td>
                                                    <td>
                                                        <span id="qualifyingboard_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Are you Vocational stream</span>
                                                    </td>
                                                    <td>
                                                        <span id="Vocationalspan" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Marks/Grade</span>
                                                    </td>
                                                    <td>
                                                        <span id="marksgrade_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <asp:GridView ID="VerificationGridug" runat="server">
                                            </asp:GridView>
                                            <br />
                                        </div>
                                        <div id="pgdiv_verification" runat="server" visible="false">
                                            <table style="width: 600px;">
                                                <tr>
                                                    <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                        <span style="font-weight: bold; font-size: large;">Academic Information</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Qualifying Examination Passed</span>
                                                    </td>
                                                    <td>
                                                        <span id="ugqualifyingexam_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Name of the College</span>
                                                    </td>
                                                    <td>
                                                        <span id="nameofcollege_Sapn" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Location of the College</span>
                                                    </td>
                                                    <td>
                                                        <span id="locationofcollege_sapn" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Mention Major</span>
                                                    </td>
                                                    <td>
                                                        <span id="major_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Type of Major</span>
                                                    </td>
                                                    <td>
                                                        <span id="typeofmajor_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Type of Semester</span>
                                                    </td>
                                                    <td>
                                                        <span id="typeofsemester_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Medium of Study at UG level</span>
                                                    </td>
                                                    <td>
                                                        <span id="mediumofstudy_spanug" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Marks/Grade</span>
                                                    </td>
                                                    <td>
                                                        <span id="marksorgradeug_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Registration No as Mentioned on your Mark Sheet </span>
                                                    </td>
                                                    <td>
                                                        <span id="reg_no_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <asp:GridView ID="Verificationgridpg" runat="server">
                                            </asp:GridView>
                                            <br />
                                        </div>
                                        <div id="ugtotaldiv" runat="server" visible="false">
                                            <table style="width: 700px;">
                                                <tr>
                                                    <td>
                                                        <span>Total Marks Obtained</span>
                                                    </td>
                                                    <td>
                                                        <span id="total_marks_secured" runat="server"></span>
                                                    </td>
                                                    <td>
                                                        <span>Maximum Marks</span>
                                                    </td>
                                                    <td>
                                                        <span id="maximum_marks" runat="server"></span>
                                                    </td>
                                                    <td>
                                                        <span>Percentage</span>
                                                    </td>
                                                    <td>
                                                        <span id="percentage_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="pgtotaldiv" runat="server" visible="false">
                                            <table style="width: 890px;">
                                                <tr>
                                                    <td>
                                                        <span>Total percentage of marks in all subjects (Language/major/Allied/Ancillary/Elective
                                                            inclusive ofTheory and Practical</span>
                                                    </td>
                                                    <td>
                                                        <span id="percentagemajorspan" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Total % of Marks in Major subjects alone (Including theory & Practicals)</span>
                                                    </td>
                                                    <td>
                                                        <span id="majorsubjectspan" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory
                                                            and Practicals</span>
                                                    </td>
                                                    <td>
                                                        <span id="alliedmajorspan" runat="server"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                        <br />
                                    </div>
                                </center>
                                <br />
                                <br />
                                <br />
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </asp:Panel>
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </div>
        </center>
        </form>
    </body>
</asp:Content>
