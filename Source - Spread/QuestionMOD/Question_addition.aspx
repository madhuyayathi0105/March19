<%@ Page Title="Question Addition" Language="C#" MasterPageFile="~/QuestionMOD/QuestionBankSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Question_addition.aspx.cs" Inherits="Question_addition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .Header
        {
            font-weight: bold;
            text-align: center;
            font-size: 22px;
            color: Green;
            margin-top: 20px;
            margin-bottom: 20px;
            line-height: 3em;
        }
        .nv
        {
            text-transform: uppercase;
        }
        .noresize
        {
            resize: none;
        }
        .fontCommon
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: #000000;
        }
        .defaultHeight
        {
            width: auto;
            height: auto;
        }
    </style>
    <script type="text/javascript">
        function check() {
            var id;
            var empty = "";

            if (document.getElementById("<%=txt_mark.ClientID %>").value.trim() == "") {
                id = document.getElementById("<%=txt_mark.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
        function display(x) {
            x.style.borderColor = "#c4c4c4";
        }
                 
    </script>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript">
        $(document).ready(function () {
            //            var items = $('#ddlQMarks option').length;
            //            alert(items);#<%=ddlQMarks.ClientID %>
            $('#<%=btnSaveQuestions.ClientID %>').click(function () {
                var length = $('#<%=ddlQMarks.ClientID %>').children('option').length;
                var questionName = $.trim($('#txtQuestionName').val());
                $('#txtQuestionName').val($.trim(quesname));
                if (quesname == "") {
                    alert('Please Enter the Questions');
                    return false;
                }
                else {
                    return true;
                }
            });
            $('#txtQuestionName').focusout(function () {
                var quesname = $.trim($('#txtQuestionName').val());
                $('#txtQuestionName').val($.trim(quesname));
                if (quesname == "") {
                    $('#txtQuestionName').focus();
                    alert('Please Enter the Questions');
                    return false;
                }
                else {
                    return true;
                }
            });
            $('#txtNoofOptionsCount').focusout(function () {
                var option = $.trim($('#txtNoofOptionsCount').val());
                $('#txtNoofOptionsCount').val($.trim(option));
                if (option == "") {
                    $('#txtNoofOptionsCount').val("");
                    $('#txtNoofOptionsCount').focus();
                    $('#divOptions').css("visibility", 'hidden');
                    alert('Please Enter the No. of Options');
                    return false;
                }
                else if (option == 0) {
                    $('#txtNoofOptionsCount').val("");
                    $('#txtNoofOptionsCount').focus();
                    $('#divOptions').css("visibility", 'hidden');
                    alert('Please Enter the No. of Options Must Be Greater Than Zero');
                    return false;
                }
                else {
                    return true;
                }
            });

            $('#txtNoofQuestionCount').focusout(function () {
                var questions = $.trim($('#txtNoofQuestionCount').val());
                $('#txtNoofQuestionCount').val($.trim(questions));
                if (questions == "") {
                    $('#txtNoofQuestionCount').focus();
                    $('#divMatches').css("visibility", 'hidden');
                    alert('Please Enter the No. of Questions');
                    return false;
                }
                else if (questions == 0) {
                    $('#txtNoofQuestionCount').val("");
                    $('#txtNoofQuestionCount').focus();
                    $('#divMatches').css("visibility", 'hidden');
                    alert('Please Enter the No. of Questions Greater Must Be Than Zero');
                    return false;
                }
                else {
                    return true;
                }
            });
        });

        function ValidateQuestionMaster() {
            var questionName = $.trim($('#txtQuestionName').val());
            $('#txtQuestionName').val($.trim(quesname));
            if (quesname == "") {
                alert('Please Enter the Questions');
                return false;
            }
            else {
                return true;
            }

        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div style="width: 100%; height: auto;">
            <table>
                <thead>
                    <tr>
                        <td colspan="3">
                            <center>
                                <span class="Header">Question Addition</span>
                            </center>
                        </td>
                    </tr>
                </thead>
            </table>
            <center>
                <div class="maindivstyle" style="width: 100%; height: auto; width: -moz-max-content;">
                    <div>
                        <center>
                            <table class="maintablestyle" width="633px" style="margin: 10px; height: auto;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_clg" runat="server" Text="College" CssClass="fontCommon"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged"
                                            AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbatch" runat="server" Text="Batch" CssClass="fontCommon"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                            AutoPostBack="true" Width="58px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldegree" runat="server" Text="Degree" CssClass="fontCommon"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegree" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                            AutoPostBack="true" Width="65px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbranch" runat="server" Text="Branch" CssClass="fontCommon" AutoPostBack="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbranch" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                            AutoPostBack="true" Width="130px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Text="Sem" CssClass="fontCommon"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsem" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                            AutoPostBack="true" Width="50px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblsec" runat="server" Text="Sec" CssClass="fontCommon"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsec" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged"
                                                        AutoPostBack="true" Width="50px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblsubject" runat="server" Text="Subject" CssClass="fontCommon"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsubject" runat="server" CssClass="fontCommon" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged" Width="90px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chklst_general" runat="server" AutoPostBack="True" Text="General"
                                                        OnCheckedChanged="chklst_general_SelectedIndexChanged" Font-Bold="True" ForeColor="Black"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Width="98px"></asp:CheckBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_go" runat="server" Visible="true" Width="44px" Height="26px"
                                                        CssClass="textbox textbox1" Text="Go" Font-Bold="true" OnClick="btn_go_Click" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="download" runat="server" ImageUrl="~/images/downimg.png" Width="35px"
                                                        Height="35px" OnClick="download_click" />
                                                </td>
                                                <td colspan="2">
                                                    <asp:ImageButton ID="help" runat="server" ImageUrl="~/images/h.jpg" Height="25px"
                                                        Width="25px" OnClick="help_click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </div>
                    <div style="margin: 10px;">
                        <asp:GridView ID="questiongrid" Visible="false" runat="server" AutoGenerateColumns="false"
                            GridLines="Both" OnRowDataBound="questiongrid_OnRowDataBound" Width="100px">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl_rs" runat="server" Width="60px" Text='<%#Eval("Sno") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit" HeaderStyle-BackColor="#0CA6CA">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_unit" runat="server" Width="160px" Text='<%#Eval("Unit") %>'></asp:Label>
                                        <asp:Label ID="lbl_no" runat="server" Visible="false" Width="160px" Text='<%#Eval("Topic_No") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="85px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Button ID="btn_addquestion" Width="90px" runat="server" BackColor="Teal" Text="Add Question"
                                                CssClass="textbox textbox3 btn1" OnClick="btn_addquestion_Click" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Button ID="btn_import" Width="90px" runat="server" BackColor="Aquamarine" BorderColor="Green"
                                                Text="Import" CssClass="textbox textbox3 btn1" OnClick="btn_import_Click" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </center>
            <%--added by Deepali 16-11-17 --%>
            <center>
                <div id="Browsefile_div" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 187px; margin-left: 186px;"
                        OnClick="imagebtnpopclose_Click" />
                    <center>
                        <div id="div1" runat="server" class="table" style="background-color: White; height: 150px;
                            width: 400px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <center>
                                    <asp:Label ID="lbl_header" runat="server" ForeColor="Green" Visible="true" Font-Size="Larger"></asp:Label>
                                </center>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblfile_upload" runat="server" Text="Browse File"></asp:Label>
                                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="textbox" ForeColor="Green" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ImageButton1" Style="height: 36px; width: 36px;" ImageUrl="~/images/okimg.jpg"
                                                runat="server" OnClick="btn_upload_click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_alert1" runat="server" Text="Please Browse Upload File" ForeColor="red"
                                                Visible="false"></asp:Label>
                                            <asp:CheckBox ID="cb_iscomplet" runat="server" ForeColor="Green" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Is Completed" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <center>
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
                                                <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_errorclose_Click" Text="OK" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
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
                                            <asp:Label ID="lblErrMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
            </center>
            <div id="Add_questiontype" runat="server" visible="false" style="height: 400em; z-index: 2000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="792px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -12px; margin-left: 439px;"
                        OnClick="imagebtnpopclose1_Click" />
                    <div id="panel_add" runat="server" visible="true" class="table" style="background-color: White;
                        height: auto; width: 920px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 27px; border-radius: 10px;">
                        <br />
                        <div>
                            <span class="fontstyleheader" style="color: Green">Question Master </span>
                        </div>
                        <br />
                        <center>
                            <asp:RadioButton ID="rb_object" Width="88px" runat="server" GroupName="same1" Text="Objective"
                                OnCheckedChanged="rb_object_CheckedChanged" AutoPostBack="true" Checked="true">
                            </asp:RadioButton>
                            <asp:RadioButton ID="rb_discript" runat="server" Width="100px" GroupName="same1"
                                Text="Descriptive" OnCheckedChanged="rb_discript_CheckedChanged" AutoPostBack="true">
                            </asp:RadioButton>
                            <br />
                            <br />
                            <table width="470px" style="height: auto; margin-left: -262px;">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_imagqstn" runat="server" Text="If You Want to Add Image" Style="height: auto;"
                                            Width="199px" AutoPostBack="True" OnCheckedChanged="cb_imagqstn_CheckedChanged" />
                                        <asp:FileUpload ID="img_uplod" Visible="false" runat="server" />
                                        <%-- margin-left: 106px;--%>
                                    </td>
                                </tr>
                            </table>
                            <center>
                                <asp:UpdatePanel ID="UpdatePanel20" runat="server" Width="100px" style="height: auto;
                                    margin-left: -490px;">
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel_questions" runat="server" Width="100px" Style="height: auto;
                                            margin-left: 10px;">
                                            <table width="500px" style="height: auto; margin-left: -100px;">
                                                <tr>
                                                    <%-- <asp:Image ID="imgleftlogo" runat="server" ImageUrl='<%# "data:image/jpg;base64," + Convert.ToBase64String((byte[])Eval("logo1")) %>'
                                        Width="100px" Height="95px" />--%>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_question" Width="103px" runat="server" Text="Question Name  : "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_questionname" Width="674px" Rows="10" autocomplete="off" Height="40px"
                                                            runat="server" TextMode="MultiLine" CssClass="textbox  txtheight2 noresize"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblmarks" Width=" 103px" runat="server" Text="Marks :"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_marks" Width="40px" Visible="false" runat="server" autocomplete="off"
                                                            CssClass="textbox  txtheight2"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_marks"
                                                            FilterType="numbers,Custom" ValidChars=" ">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:Button ID="btn_add" runat="server" Text="+" CssClass="textbox textbox1 btn"
                                                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_add_Click" />
                                                        <asp:DropDownList ID="ddlmark" Width="56px" CssClass="textbox textbox1 ddlheight2"
                                                            runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btn_min" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                            Font-Names="Book Antiqua" CssClass="textbox textbox1 btn" OnClick="btn_min_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <div id="objectiv" runat="server" visible="true">
                                                <%--<div id="match_option" runat="server" visible="true" style="height: auto; margin-left: 30px;">
                                        <div style="width: 650px; overflow: auto; ackground-color: White; border-radius: 0px;
                                            height: auto;">
                                            <br />
                                            <center>
                                               
                                            </center>
                                        </div>
                                    </div>--%>
                                                <br />
                                                <table width="500px" style="height: auto; margin-left: 2px;">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="cb_matchthefollowing" runat="server" Text="Match The Following"
                                                                Style="height: auto;" Width="170" AutoPostBack="True" OnCheckedChanged="cb_matchthefollowing_CheckedChanged" />
                                                            <asp:Label ID="lbl_noof_question" Width=" 130px" Visible="false" runat="server" Text="No of Questions  :"></asp:Label>
                                                            <asp:TextBox ID="txt_qstcount" Visible="false" Width=" 40px" autocomplete="off" runat="server"
                                                                MaxLength="1" OnTextChanged="txt_qstcount_OnTextChanged" AutoPostBack="true"
                                                                CssClass="textbox  txtheight2"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_qstcount"
                                                                FilterType="numbers,Custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gridView2" runat="server" Style="height: auto; margin-left: 140px;"
                                                                AutoGenerateColumns="false" GridLines="Both" Width="500px">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                                        HeaderStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:Label ID="lbl_rs" runat="server" Width="40px" Text='<%#Eval("Sno") %>'></asp:Label>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Question" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:TextBox ID="txtqstn" runat="server" autocomplete="off" CssClass="textbox" Height="17px"
                                                                                    Width="190px"></asp:TextBox>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Answer" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:Label ID="lbl_order" Width="14px" runat="server" Text='<%#Eval("orderkey") %>'></asp:Label>
                                                                                <asp:TextBox ID="txt_answer" runat="server" autocomplete="off" CssClass="textbox"
                                                                                    Height="17px" Width="180px"></asp:TextBox>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="lbl_no_option" Width=" 100px" runat="server" Text="No of Optional   :"></asp:Label>
                                                            <asp:TextBox ID="txt_nooption" Width=" 40px" autocomplete="off" MaxLength="1" runat="server"
                                                                OnTextChanged="Txt_nooption_OnTextChanged" AutoPostBack="true" CssClass="textbox  txtheight2"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_nooption"
                                                                FilterType="numbers,Custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div id="optionqstn" runat="server" visible="true" style="height: auto; margin-left: 145px;">
                                                    <div style="width: 450px; overflow: auto; ackground-color: White; border-radius: 0px;
                                                        height: auto;">
                                                        <br />
                                                        <center>
                                                            <asp:GridView ID="gridView1" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                                Width="450px">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                                        HeaderStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:Label ID="lbl_rs" runat="server" Width="40px" Text='<%#Eval("Sno") %>'></asp:Label>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Option" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:TextBox ID="txtOption" runat="server" autocomplete="off" CssClass="textbox"
                                                                                    Height="17px" Width="280px"></asp:TextBox>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Select Answer" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:CheckBox ID="cb_answer" runat="server" Width="30px" AutoPostBack="True" OnCheckedChanged="cb_answer_CheckedChanged" />
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </center>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="descript" runat="server" visible="true">
                                                <table width="500px" style="height: auto; margin-left: -121px;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_answer" Width="100px" runat="server" Text="Answer    : "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_answer" Width="437px" runat="server" autocomplete="off" TextMode="MultiLine"
                                                                CssClass="textbox  txtheight2" Style="height: auto;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <center>
                                                <br />
                                                <table width="400px" style="height: auto; margin-left: 2px;">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rb_Easy" runat="server" Width="90px" GroupName="typequestion"
                                                                Text="Easy" Checked="true" AutoPostBack="true"></asp:RadioButton>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rb_medium" runat="server" Width="90px" GroupName="typequestion"
                                                                Text="Medium" AutoPostBack="true"></asp:RadioButton>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rb_difficult" runat="server" Width="90px" GroupName="typequestion"
                                                                Text="Difficult" AutoPostBack="true"></asp:RadioButton>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rb_hard" runat="server" Width="90px" GroupName="typequestion"
                                                                Text="Hard" AutoPostBack="true"></asp:RadioButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </center>
                                            <asp:Button ID="btn_Savequestion" Width="90px" runat="server" Text="Save" CssClass="textbox textbox1 btn1"
                                                OnClick="btn_Savequestion_Click" />
                                            <div id="imgdiv5" runat="server" visible="false" style="height: 100%; z-index: 10000;
                                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                                left: 0px;">
                                                <center>
                                                    <div id="panel_mark" runat="server" class="table" style="background-color: White;
                                                        height: auto; width: 250px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                                        margin-top: 200px; border-radius: 10px;">
                                                        <table>
                                                            <tr>
                                                                <td align="center">
                                                                    <span class="fontstyleheader" style="color: Green">Mark</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txt_mark" runat="server" Height="25px" onfocus=" return display(this)"
                                                                        MaxLength="3" Style="text-transform: capitalize;" CssClass="textbox textbox1"
                                                                        Width="50px"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="txtFilterExterder" runat="server" FilterType="Numbers,custom"
                                                                        ValidChars="." TargetControlID="txt_mark">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <caption>
                                                                <br />
                                                                <tr>
                                                                    <td align="center">
                                                                        <br />
                                                                        <asp:Button ID="btn_addmark" runat="server" CssClass="textbox textbox1" Height="32px"
                                                                            OnClick="btn_addmark_Click" OnClientClick="return checkadd()" Text="Add" Visible="true"
                                                                            Width="58px" />
                                                                        <asp:Button ID="btn_exitmark" runat="server" CssClass="textbox textbox1" Height="32px"
                                                                            OnClick="btn_exitmark_Click" Text="Exit" Visible="true" Width="68px" />
                                                                        <br />
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                            </caption>
                                                        </table>
                                                    </div>
                                                </center>
                                            </div>
                                            <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                                left: 0px;">
                                                <center>
                                                    <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                                                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                                        border-radius: 10px;">
                                                        <center>
                                                            <table style="height: 100px; width: 100%">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lbl_alert" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                                        <asp:Label ID="Label1" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <center>
                                                                            <asp:Button ID="btn_errorclose1" CssClass="textbox textbox1" Style="height: 28px;
                                                                                width: 65px;" OnClick="btn_errorclose1_Click" Text="Ok" runat="server" />
                                                                        </center>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </center>
                                                    </div>
                                                </center>
                                            </div>
                                            <div id="imgdiv4" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                                left: 0px;">
                                                <center>
                                                    <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
                                                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                                        border-radius: 10px;">
                                                        <center>
                                                            <table style="height: 100px; width: 100%">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lbl_warningmsghed" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <center>
                                                                            <asp:Button ID="btn_warningmsgmrk" CssClass="textbox textbox1" Style="height: 28px;
                                                                                width: 65px;" OnClick="btn_warningmsmark_Click" Text="Yes" runat="server" />
                                                                            <asp:Button ID="btn_warningmsghed_exit" CssClass="textbox textbox1" Style="height: 28px;
                                                                                width: 65px;" OnClick="btn_warningmsgmark_exit_Click" Text="No" runat="server" />
                                                                        </center>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </center>
                                                    </div>
                                                </center>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                        </center>
                        <br />
                        <br />
                    </div>
                </center>
                <br />
                <br />
            </div>
            <div id="divMainQuestionMaster" runat="server" visible="false" style="height: 100%;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 0; left: 0px; font-family: Book Antiqua; font-size: medium; font-weight: bold;">
                <center>
                    <asp:ImageButton ID="imgbtnQuestionMaster" runat="server" Width="792px" Height="40px"
                        ImageUrl="~/images/close.png" Style="height: 30px; width: 30px; position: absolute;
                        margin-top: 10px; margin-left: 435px;" OnClick="imgbtnQuestionMaster_Click" />
                    <div id="divQuesPanel" class="fontCommon" runat="server" visible="true" style="background-color: White;
                        height: auto; width: 980px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin: 10px; padding: 2px; margin-top: 27px; border-radius: 10px; display: table;">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: Green">Question Master </span>
                        </center>
                        <br />
                        <div style="border: 2px solid #0CA6CA; border-top: 2px solid #0CA6CA; border-radius: 10px;
                            margin-bottom: 10px;">
                            <asp:RadioButtonList ID="rblObjectiveDescriptive" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rblObjectiveDescriptive_SelectedIndexChanged"
                                CssClass="fontCommon">
                                <asp:ListItem Text="Objective" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Descriptive" Value="1" Selected="false"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div>
                            <div id="divQuestionImage" runat="server" style="display: table-row; width: auto;
                                text-align: left; height: auto; margin-left: 0px; margin-top: 10px;">
                                <asp:CheckBox ID="chkAddQuesImage" CssClass="fontCommon" runat="server" Text="If You Want to Add Image To Question"
                                    AutoPostBack="true" OnCheckedChanged="chkAddQuesImage_CheckedChanged" Style="display: table-cell;" />
                                <asp:UpdatePanel ID="upnlQuestionImage" runat="server" Width="100px" style="height: auto;">
                                    <ContentTemplate>
                                        <asp:FileUpload ID="fuQuestionImage" Visible="false" runat="server" ViewStateMode="Enabled" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <%--  <asp:UpdatePanel ID="upnlQuestions" runat="server" style="height: auto; width: auto;
                            position: relative;">
                            <ContentTemplate>--%>
                        <asp:Panel ID="pnlQuestionContent" runat="server" Style="height: auto; width: auto;
                            margin-top: 10px;">
                            <table class="fontCommon" style="height: auto;">
                                <tr>
                                    <td>
                                        <span>Question Name : </span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQuestionName" Width="680px" Rows="5" autocomplete="off" runat="server"
                                            TextMode="MultiLine" CssClass="textbox  txtheight2 noresize" Style="height: auto;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Image ID="imgQuestionImage" AlternateText="" runat="server" Style="width: 90px;
                                            height: 90px;" />
                                    </td>
                                </tr>
                                <tr id="tdDescript" runat="server">
                                    <td>
                                        <span>Answer</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQuestionAnswer" Width="680px" Rows="5" autocomplete="off" runat="server"
                                            TextMode="MultiLine" CssClass="textbox  txtheight2 noresize" Style="height: auto;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblQMarks" Width=" 103px" runat="server" Text="Marks :"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtQMarks" Width="40px" Visible="false" runat="server" autocomplete="off"
                                            CssClass="textbox  txtheight2"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filtertxtQmarks" runat="server" TargetControlID="txtQMarks"
                                            FilterType="numbers,Custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnAddQMarks" runat="server" Text="+" CssClass="textbox textbox1 defaultHeight"
                                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnAddQMarks_Click" />
                                        <asp:DropDownList ID="ddlQMarks" CssClass="fontCommon" Width="56px" runat="server">
                                        </asp:DropDownList>
                                        <asp:Button ID="btnDeleteQMarks" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox textbox1 defaultHeight" OnClick="btnDeleteQMarks_Click" />
                                    </td>
                                </tr>
                            </table>
                            <div id="divObjective" runat="server" style="display: table-row; margin: 0px; margin-top: 20px;
                                position: relative; height: auto; padding: 5px; width: auto; font-family: Book Antiqua;
                                font-size: medium; font-weight: bold; text-align: left;">
                                <div id="divQuestionType" runat="server" style="display: -moz-box; width: auto; height: auto;
                                    text-align: left; border: 1px solid black; border-radius: 5px; margin-top: 10px;">
                                    <div>
                                        <asp:RadioButtonList ID="rblQuestionType" CssClass="fontCommon" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="rblQuestionType_SelectedIndexChanged"
                                            Style="text-align: left;">
                                            <asp:ListItem Text="Choose The Best Answer" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Fill in The Blanks" Value="2" Selected="False"></asp:ListItem>
                                            <asp:ListItem Text="Match The Following" Value="3" Selected="False"></asp:ListItem>
                                            <asp:ListItem Text="True Or False" Value="4" Selected="False"></asp:ListItem>
                                            <asp:ListItem Text="Rearranging" Value="5" Selected="False"></asp:ListItem>
                                            <asp:ListItem Text="Paragraph With Questions and Options" Value="6" Selected="False"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div id="divSubType" runat="server" style="display: -moz-box; width: auto; height: auto;
                                    text-align: left; border: 1px solid black; border-radius: 5px; margin-top: 10px;">
                                    <div>
                                        <asp:RadioButtonList ID="rblSingleorMutiChoice" runat="server" RepeatDirection="Horizontal"
                                            Style="font-family: Book Antiqua; font-size: medium; font-weight: bold; display: table-cell;
                                            text-align: left;">
                                            <asp:ListItem Text="Single Answer" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Multiple Answer" Value="2" Selected="False"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <asp:RadioButtonList ID="rblMatchSubType" RepeatDirection="Horizontal" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblMatchSubType_SelectedIndexChanged"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold; display: table-row;
                                        margin: 5px;">
                                        <asp:ListItem Text="Statement Vs Statement" Value="3" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Statement Vs Image" Value="4" Selected="False"></asp:ListItem>
                                        <asp:ListItem Text="Image Vs Statement" Value="5" Selected="False"></asp:ListItem>
                                        <asp:ListItem Text="Image Vs Image" Value="6" Selected="False"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <div id="divMatchSubType" class="fontCommon" runat="server" style="font-family: Book Antiqua;
                                        font-size: medium; font-weight: bold; display: table-row; text-align: left;"
                                        visible="false">
                                        <div style="font-family: Book Antiqua; font-size: medium; font-weight: bold; display: table-row;
                                            text-align: left; margin: 0px; padding: 15px;">
                                            <asp:Label ID="lblMatchType" runat="server" Visible="true" Text="Match Type" Style="display: table-cell;
                                                text-align: left; margin: 5px;"></asp:Label>
                                            <asp:DropDownList ID="ddlMatchSubType" RepeatDirection="Horizontal" runat="server"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlMatchSubType_SelectedIndexChanged"
                                                Style="display: table-cell; margin: 5px;">
                                                <asp:ListItem Text="Statement Vs Statement" Value="3" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Statement Vs Image" Value="4" Selected="False"></asp:ListItem>
                                                <asp:ListItem Text="Image Vs Statement" Value="5" Selected="False"></asp:ListItem>
                                                <asp:ListItem Text="Image Vs Image" Value="6" Selected="False"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="divQuestionOption" runat="server" style="display: table-row; width: auto;
                                    height: auto; text-align: left; border: 1px solid black; border-radius: 5px;
                                    margin-top: 10px;">
                                    <asp:Label ID="lblMQuestionCount" runat="server" Visible="false" Text="No. of Questions"
                                        Style="text-align: left; margin: 5px;"></asp:Label>
                                    <asp:TextBox ID="txtNoofQuestionCount" Visible="false" Width="40px" autocomplete="off"
                                        runat="server" MaxLength="1" OnTextChanged="txtNoofQuestionCount_OnTextChanged"
                                        AutoPostBack="true" CssClass="textbox  txtheight2" Style="text-align: left; margin: 5px;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filtertxtNoofQuestionCount" runat="server" TargetControlID="txtNoofQuestionCount"
                                        FilterType="numbers">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                                <center>
                                    <div id="divMatches" runat="server" style="margin: 5px; margin-top: 10px;">
                                        <div id="divMatchContent" runat="server" style="display: table-row; width: auto;
                                            height: auto; margin: 5px; padding: 15px; text-align: left;">
                                            <asp:GridView ID="gvMatchQuestion" runat="server" Style="height: auto; width: auto;
                                                margin: 0px;" AutoGenerateColumns="false" GridLines="Both" OnRowDataBound="gvMatchQuestion_RowDataBound"
                                                OnRowCommand="gvMatchQuestion_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="40px">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lblQMatchSno" Visible="true" runat="server" Width="40px" Text='<%#Eval("Sno") %>'></asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Question" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <center style="margin: 0px;">
                                                                <asp:Label ID="lblMatchQuestions" runat="server" Text="" Visible="false"></asp:Label>
                                                                <asp:TextBox ID="txtMatchQuestions" runat="server" autocomplete="off" CssClass="textbox"
                                                                    Text="" Height="17px" Style="margin: 0px; display: table-cell; width: 80%;"></asp:TextBox>
                                                                <%--<div id="divLhs" runat="server" style="margin:0px; display:table-cell; width:70%;">--%>
                                                                <asp:FileUpload ID="fuLhsQMatch" runat="server" EnableViewState="true" Text="Upload"
                                                                    Style="margin: 0px; display: table-cell; width: 45%;" Visible="false" />
                                                                <%--OnClick="btnLhsImage_Click"--%>
                                                                <asp:Button ID="btnLhsImage" runat="server" Text="Upload" Style="margin: 0px; display: table-cell;
                                                                    width: 20%;" CommandName="Lupload" />
                                                                <asp:Image ID="imgLhsQMatch" runat="server" ImageUrl="" ImageAlign="Right" Width="190px"
                                                                    AlternateText="" />
                                                                <%--     </div>--%>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Answer" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <center style="margin: 0px;">
                                                                <asp:Label ID="lblMatchAnsSno" runat="server" Text='<%#Eval("AnswerSno") %>' Style="width: 14px;
                                                                    margin: 5px;"></asp:Label>
                                                                <asp:Label ID="lblRhsFile" runat="server" Text="" Visible="false"></asp:Label>
                                                                <asp:TextBox ID="txtMatchAnswer" runat="server" autocomplete="off" CssClass="textbox"
                                                                    Text="" Height="17px" Style="margin: 0px; display: table-cell; width: 80%;"></asp:TextBox>
                                                                <%-- <div id="divRhs" runat="server" style="margin: 0px; display: table-cell; width: 50%;">--%>
                                                                <asp:FileUpload ID="fuRhsAMatch" runat="server" EnableViewState="true" Style="margin: 0px;
                                                                    display: table-cell; width: 45%;" Visible="false" />
                                                                <%--  OnClick="btnRhsImage_Click"--%>
                                                                <asp:Button ID="btnRhsImage" runat="server" Text="Upload" Style="margin: 0px; display: table-cell;
                                                                    width: 20%;" CommandName="Rupload" />
                                                                <asp:Image ID="imgRhsAMatch" AlternateText="" Width="180px" runat="server" ImageUrl=""
                                                                    ImageAlign="Right" />
                                                                <%-- </div>--%>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </center>
                                <center>
                                    <div id="divParagraph" runat="server" style="display: -moz-box; width: auto; height: auto;
                                        margin: 10px; text-align: left; margin-top: 10px;">
                                        <asp:GridView ID="gvParagraph" runat="server" AutoGenerateColumns="true" GridLines="Both"
                                            Width="300px" OnRowDataBound="gvParagraph_RowDataBound" Style="height: auto;">
                                        </asp:GridView>
                                    </div>
                                </center>
                                <div id="divTypeChoice" runat="server" style="display: -moz-box; width: auto; height: auto;
                                    text-align: left; margin-top: 10px;">
                                    <asp:Label ID="lblNoofOptions" runat="server" Visible="false" Text="No. of Options"
                                        Style="text-align: left; margin: 5px;"></asp:Label>
                                    <asp:TextBox ID="txtNoofOptionsCount" Visible="false" Width="40px" autocomplete="off"
                                        runat="server" MaxLength="1" OnTextChanged="txtNoofOptionsCount_TextChanged"
                                        AutoPostBack="true" CssClass="textbox  txtheight2" Style="margin: 5px;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filter" runat="server" TargetControlID="txtNoofOptionsCount"
                                        FilterType="numbers">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:CheckBox ID="chkNeedOptions" runat="server" Text="Need Options" OnCheckedChanged="chkNeedOptions_CheckedChanged"
                                        Checked="false" AutoPostBack="true" Visible="true" Style="text-align: left; margin: 5px;" />
                                </div>
                                <center>
                                    <div id="divOptions" runat="server" style="display: table-row; height: auto; margin: 5px;
                                        padding: 15px; text-align: center; margin-top: 10px;">
                                        <center>
                                            <asp:GridView ID="gvQOptions" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                OnRowDataBound="gvQOptions_RowDataBound" Style="width: auto; height: auto;">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="40px">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:Label ID="lblOptionSno" runat="server" Width="40px" Text='<%#Eval("Sno") %>'></asp:Label>
                                                            </center>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Option" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:TextBox ID="txtQOption" runat="server" autocomplete="off" CssClass="textbox"
                                                                    Height="17px" Width="280px"></asp:TextBox>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select Answer" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:CheckBox ID="chkQOptionAnswer" runat="server" Width="30px" AutoPostBack="True"
                                                                    OnCheckedChanged="chkQOptionAnswer_CheckedChanged" />
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </center>
                                    </div>
                                </center>
                            </div>
                            <div id="divDescriptive" runat="server" style="display: table-row; margin: 0px; margin-top: 30px;
                                position: relative; height: auto; padding: 15px; width: auto;">
                            </div>
                            <div id="divQuestionGrading" runat="server" style="display: -moz-box; width: auto;
                                height: auto; margin: 10px; text-align: left; margin-top: 10px;">
                                <span>Question Grading</span>
                                <asp:RadioButtonList ID="rblQuestionGrading" CssClass="fontCommon" runat="server"
                                    RepeatDirection="Horizontal" Style="width: auto; height: auto; margin: 0px; padding: 15px;
                                    margin-left: 98px;">
                                    <asp:ListItem Text="Easy" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Medium" Value="1" Selected="False"></asp:ListItem>
                                    <asp:ListItem Text="Difficult" Value="2" Selected="False"></asp:ListItem>
                                    <asp:ListItem Text="Hard" Value="3" Selected="False"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <center>
                                <asp:Button ID="btnSaveQuestions" Width="90px" runat="server" Text="Save" CssClass="textbox textbox1 defaultHeight"
                                    OnClick="btnSaveQuestions_Click" />
                            </center>
                            <div id="divAddQmarks" runat="server" visible="false" style="height: 400em; z-index: 20000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="divAddMarkContent" runat="server" class="table" style="background-color: White;
                                        height: auto; width: 250px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                        margin-top: 200px; border-radius: 10px;">
                                        <table>
                                            <tr>
                                                <td align="center">
                                                    <span class="fontstyleheader" style="color: Green">Mark</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox ID="txtAddQmark" runat="server" Height="25px" onfocus=" return display(this)"
                                                        MaxLength="3" Style="text-transform: capitalize;" CssClass="textbox textbox1"
                                                        Width="50px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="filtertxtAddQmark" runat="server" FilterType="Numbers,custom"
                                                        ValidChars="." TargetControlID="txtAddQmark">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <br />
                                            <tr>
                                                <td align="center">
                                                    <br />
                                                    <asp:Button ID="btnSaveAddQMarks" runat="server" Visible="true" Width="58px" Height="32px"
                                                        CssClass="textbox textbox1" Text="Add" OnClientClick="return checkadd()" OnClick="btnSaveAddQMarks_Click" />
                                                    <asp:Button ID="btnExitAddQMark" runat="server" Visible="true" Width="68px" Height="32px"
                                                        CssClass="textbox textbox1" Text="Exit" OnClick="btnExitAddQMark_Click" />
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </div>
                            <div id="divPopAlert" runat="server" visible="false" style="height: 400em; z-index: 2000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                        margin-top: 200px; border-radius: 10px;">
                                        <center>
                                            <table style="height: 100px; width: 100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:Button ID="btnPopAlertClose" CssClass="textbox textbox1" Style="height: 28px;
                                                                width: 65px;" OnClick="btnPopAlertClose_Click" Text="Ok" runat="server" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </center>
                            </div>
                            <div id="divWarning" runat="server" visible="false" style="height: 400em; z-index: 2000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="divWarningContent" runat="server" class="table" style="background-color: White;
                                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                        margin-top: 200px; border-radius: 10px;">
                                        <center>
                                            <table style="height: 100px; width: 100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblWarningMsgs" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:Button ID="btnWarningMsgYes" CssClass="textbox textbox1" Style="height: 28px;
                                                                width: 65px;" OnClick="btnWarningMsgYes_Click" Text="Yes" runat="server" />
                                                            <asp:Button ID="btnExitWarningNo" CssClass="textbox textbox1" Style="height: 28px;
                                                                width: 65px;" OnClick="btnExitWarningNo_Click" Text="No" runat="server" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </center>
                            </div>
                        </asp:Panel>
                        <br />
                        <br />
                    </div>
                </center>
                <br />
                <br />
            </div>
        </div>
    </center>
    <center>
        <FarPoint:FpSpread ID="fQuesBankImport" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
            CssClass="stylefp">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
    </center>
    <center>
        <div id="cannot_insert_div" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px; height: 100em;">
            <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 61px; margin-left: 405px;"
                OnClick="btn_Exit_Click1" />
            <center>
                <div id="cannot_insert" runat="server" style="background-color: White; height: 400px;
                    width: 840px; border: 5px solid #0CA6CA; border-top: 5px solid #0CA6CA; margin-top: 72px;
                    border-radius: 10px;">
                    <asp:Label ID="lbl_upload_suc" runat="server" Visible="false" ForeColor="Blue"></asp:Label>
                    <br />
                    <asp:Label ID="lbl_cannotsave" Visible="true" runat="server" Style="color: Red;"
                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="height: 345px; width: 700px; overflow: auto;">
                        <asp:TextBox ID="txt_cannotinsert" TextMode="MultiLine" runat="server" Style="height: 334px;
                            overflow: auto;" Visible="true" Width="650px" ForeColor="Blue" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
