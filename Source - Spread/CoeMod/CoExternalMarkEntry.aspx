<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CoExternalMarkEntry.aspx.cs" Inherits="CoExternalMarkEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
    <style type="text/css">
        .GridDock
        {
            overflow-x: auto;
            overflow-y: auto;
            width: 600px;
            height: 600px;
            padding: 0 0 0 0;
        }
    </style>
    <script type="text/javascript">

        function validatetxt(e) {
            var mk = -4;
            max = parseInt(document.getElementById("<%=lblmaxMark.ClientID %>").innerHTML.trim());
            if (e.value < mk || e.value > max) {
                e.value = "";
                alert("Enter Mark Less Than Or Equal To Maximum Mark");
            }

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID='UpdGridStudent' runat="server">
        <ContentTemplate>
            <center>
                <div class="maindivstyle">
                    <span style="color: Green; font-size: large;" class=" fontstyleheader">CO Based Exam
                        Mark Entry</span>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblmonthYear1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Year And Month"></asp:Label>
                                <asp:DropDownList ID="ddlYear1" runat="server" CssClass="textbox ddlheight" OnSelectedIndexChanged="ddlYear1_SelectedIndexChanged"
                                    Width="70px" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlMonth1" runat="server" CssClass="textbox ddlheight" OnSelectedIndexChanged="ddlMonth1_SelectedIndexChanged"
                                    Width="60px" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:Label ID="lbltype" Text="Stream" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="ddltype" runat="server" Width="128px" CssClass="textbox ddlheight"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Label ID="Label1" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="ddldegree1" runat="server" CssClass="textbox ddlheight" Width="100px"
                                    OnSelectedIndexChanged="ddldegree1_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:Label ID="Label2" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Branch"></asp:Label>
                                <asp:DropDownList ID="ddlbranch1" runat="server" CssClass="textbox ddlheight" Width="160px"
                                    OnSelectedIndexChanged="ddlbranch1_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Sem"></asp:Label>
                                <asp:DropDownList ID="ddlsem1" runat="server" CssClass="textbox ddlheight" Width="90px"
                                    OnSelectedIndexChanged="ddlsem1_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:Label ID="lblsubtype" runat="server" Text="Subject" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                                <asp:DropDownList ID="ddlsubtype" runat="server" AutoPostBack="true" CssClass="textbox ddlheight"
                                    Width="200px" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                                <asp:DropDownList ID="ddlSubject" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged"
                                    CssClass="textbox ddlheight" Width="380px">
                                </asp:DropDownList>
                                <asp:Button ID="btnviewre" runat="server" Text="Go" OnClick="btnviewre_Click" CssClass="textbox btn"
                                    Width="120px" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lblerr1" runat="server" Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false"></asp:Label>
                    <br />
                    <asp:Label ID="lblaane" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Maroon" Text="Note:Please Enter If  AB: Absent, NR: Not Registered, NE:Not Entered, M: Mal Practice, LT: Discontinue"></asp:Label>
                    <br />
                    <asp:Label ID="lblmax" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Green" Text="Max.EXT.Mark" Visible="false"></asp:Label>
                    <asp:Label ID="lblmaxMark" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false"></asp:Label>
                    <br />
                </div>
            </center>
            <center>
                <div>
                    <table cellpadding="10">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <div class="GridDock" id="dvGridWidth">
                                                <asp:GridView ID="GridStudent" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                                    width: auto;" Font-Names="Times New Roman" OnDataBound="GridStudent_OnDataBound"
                                                    AutoGenerateColumns="false" Visible="false" BackColor="AliceBlue">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"
                                                                    OnClick="lnkAttMark11" ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Roll No">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lblRollNO" runat="server" Text='<%# Eval("roll_no") %>' OnClick="lnkAttMark11"
                                                                    ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reg No">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lblregno" runat="server" Text='<%# Eval("Reg_No") %>' OnClick="lnkAttMark11"
                                                                    ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                                                                <asp:Label ID="lblExamCode" runat="server" Text='<%# Eval("Exam_code") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblSubNo" runat="server" Text='<%# Eval("subject_no") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lblName" runat="server" Text='<%# Eval("Stud_Name") %>' OnClick="lnkAttMark11"
                                                                    ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                                                                <asp:Label ID="lblappno" runat="server" Text='<%# Eval("App_No") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="C.I.A">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblminExt" runat="server" Text='<%# Eval("min_ext_marks") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblminInt" runat="server" Text='<%# Eval("min_int_marks") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblmintTot" runat="server" Text='<%# Eval("mintotal") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblAttempts" runat="server" Visible="false"></asp:Label>
                                                                <asp:Label ID="lblEv1" runat="server" Visible="false"></asp:Label>
                                                                <asp:Label ID="lblCredit" runat="server" Text='<%# Eval("credit_points") %>' Visible="false"></asp:Label>
                                                                <asp:LinkButton ID="lblInt" runat="server" ForeColor="Black" Font-Underline="false"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="E.S.E">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtTotMark" runat="server" MaxLength="3" Style="text-align: center"
                                                                    Width="80px" onkeyup="return validatetxt(this)" Enabled="false"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtTotMark"
                                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                                                </asp:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="Button1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnClick="Button1_Click" Text="Save" Width="70px" />
                                            <asp:Button ID="Button2" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnClick="Button2_Click" Text="Delete" Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table style="width: 450px;">
                                    <tr>
                                        <td>
                                            <fieldset id="flstuRoll" runat="server" visible="false">
                                                <table>
                                                    <tr>
                                                        <td style="padding-left: 10px;">
                                                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Book Antiqua">Roll No</asp:Label>
                                                        </td>
                                                        <td style="padding-left: 10px;">
                                                            <asp:TextBox ID="txtRollOrReg" runat="server" Enabled="false" CssClass="textbox txtheight4 textbox1"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 138px; padding-left: 10px;">
                                                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua">Name</asp:Label>
                                                            <asp:Label ID="lblInternal" runat="server" Visible="false"></asp:Label>
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
                                            <asp:GridView ID="GridView3" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                                width: auto;" Font-Names="Times New Roman" OnDataBound="OnDataBound" AutoGenerateColumns="false"
                                                Visible="false" BackColor="AliceBlue">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Part No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPartName" runat="server" Text='<%# Eval("PartName") %>'></asp:Label>
                                                            <asp:Label ID="lblPartNo" runat="server" Text='<%# Eval("PartNo") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
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
                                                            <asp:Label ID="lblExamCode" runat="server" Text='<%# Eval("ExamCode") %>' Visible="false"></asp:Label>
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
                                            <table>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTot" runat="server" Text="Grand Total : " Style="color: Green;"
                                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblGrandTotal" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
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
                </td> </tr> </table>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
