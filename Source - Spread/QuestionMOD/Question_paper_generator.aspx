<%@ Page Title="Question Paper Genarator" Language="C#" MasterPageFile="~/QuestionMOD/QuestionBankSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Question_paper_generator.aspx.cs" Inherits="Question_paper_generator" %>

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
    <script language="javascript">
        function myPostBack() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
            }
        }
    </script>
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
                                <span class="Header">Question Paper Genarator</span>
                            </center>
                        </td>
                    </tr>
                </thead>
            </table>
            <center>
                <div class="maindivstyle" style="width: 100%; height: auto; width: -moz-max-content;">
                    <div>
                        <center>
                            <table class="maintablestyle fontCommon" width="933px" style="margin: 10px; height: auto;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_clg" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_collegename" CssClass="fontCommon" runat="server" OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged"
                                            AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbatch" runat="server" Text="Batch" CssClass="fontCommon"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                            AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldegree" runat="server" Text="Degree" CssClass="fontCommon"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegree" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                            AutoPostBack="true" Width="130px">
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
                                        <asp:Label ID="lblsem" runat="server" Text="Sem"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsem" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                            AutoPostBack="true" Width="50px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsec" runat="server" Text="Sec" CssClass="fontCommon"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsec" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged"
                                            AutoPostBack="true" Width="50px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="12">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" ForeColor="Black"
                                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsubject" runat="server" CssClass="fontCommon" Width="130px"
                                                        OnSelectedIndexChanged="ddlsubject_Selectchanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTopic" runat="server" Text="Topic Name" CssClass="fontCommon"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" style="width: 80px; margin: 0px;">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtTopic" ReadOnly="true" Width="70px" runat="server" CssClass="textbox  txtheight2 fontCommon">---Select---</asp:TextBox>
                                                            <asp:Panel ID="pnlTopic" runat="server" CssClass="multxtpanel">
                                                                <asp:CheckBox ID="chkTopic" runat="server" CssClass="fontCommon" OnCheckedChanged="chkTopic_CheckedChanged"
                                                                    Text="Select All" AutoPostBack="True" />
                                                                <asp:TreeView ID="treeTopic" runat="server" ShowCheckBoxes="All" OnTreeNodeCheckChanged="treeTopic_TreeNodeCheckChanged"
                                                                    CssClass="fontCommon" ExpandDepth="0" ShowLines="true" ShowExpandCollapse="true"
                                                                    OnSelectedNodeChanged="treeTopic_SelectedNodeChanged" OnClick="myPostBack();"
                                                                    AutoPostBack="true">
                                                                </asp:TreeView>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popExtTopic" runat="server" TargetControlID="txtTopic"
                                                                PopupControlID="pnlTopic" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_month" runat="server" Text="Month" CssClass="fontCommon"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_month" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddl_month_SelectedIndexChanged"
                                                        AutoPostBack="true" Width="82px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_year" runat="server" Text="Year" CssClass="fontCommon"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_year" runat="server" CssClass="fontCommon" OnSelectedIndexChanged="ddl_year_SelectedIndexChanged"
                                                        AutoPostBack="true" Width="56px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_testname" runat="server" Text="Test Name" CssClass="fontCommon"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_testname" runat="server" CssClass="fontCommon" AutoPostBack="true"
                                                        Width="130px" OnSelectedIndexChanged="ddl_testname_Selectchanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <fieldset style="width: 238px; padding: 0px; background-color: #ffccff; border-radius: 10px;
                                                        border-color: #6699ee; overflow: auto;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rb_internel" Width="82px" Visible="true" runat="server" CssClass="fontCommon"
                                                                        GroupName="same1" Text="Internal" OnCheckedChanged="rb_internel_CheckedChanged"
                                                                        AutoPostBack="true" Checked="true"></asp:RadioButton>
                                                                    <asp:RadioButton ID="rb_external" runat="server" Visible="true" CssClass="fontCommon"
                                                                        Width="85px" GroupName="same1" Text="External" OnCheckedChanged="rb_external_CheckedChanged"
                                                                        AutoPostBack="true"></asp:RadioButton>
                                                                    <asp:Button ID="btn_go" runat="server" Visible="true" Width="50px" Height="26px"
                                                                        CssClass="textbox textbox1" Text="Go" BackColor="LightGreen" Font-Bold="true"
                                                                        OnClick="btn_go_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" style="width:150px; height:auto; margin: 10px;"></asp:Label>
                    </div>
                    <center>
                        <div id="objective_check" runat="server" visible="false" style="margin: 10px; margin-top:40px;">
                            <center>
                                <div style="width: auto">
                                    <asp:RadioButton ID="cb_existqstn" Font-Bold="true" GroupName="includ" Checked="true"
                                        runat="server" Text="Include Already Existed questions" AutoPostBack="true" OnCheckedChanged="cb_existqstn_CheckedChanged" />
                                    <asp:RadioButton ID="cb_notexist" runat="server" Font-Bold="true" GroupName="includ"
                                        AutoPostBack="true" OnCheckedChanged="cb_notexist_CheckedChanged" Text=" Not Include Already Existed questions" />
                                </div>
                            </center>
                            <asp:Label ID="lblerrors" runat="server" Visible="false" Style="color: Red;" Font-Bold="true"
                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            <asp:GridView ID="grd_dynamic" Visible="true" runat="server" AutoGenerateColumns="false"
                                GridLines="Both" Style="width: auto; height: auto;">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                        HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_rs" runat="server" Width="60px" Text=' <%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Section" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_sec" runat="server" Width="60px" Text='<%#Eval("Section") %>'></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mark" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_mark" runat="server" MaxLength="3" AutoPostBack="true" autocomplete="off"
                                                    OnTextChanged="txt_mark_OnTextChanged" CssClass="  textbox txtheight1" Height="17px"
                                                    Width="30px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxt_mark" runat="server"
                                                    TargetControlID="txt_mark" FilterType="Numbers,Custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Objective" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <center>
                                                <%--<asp:CheckBox ID="rb_objct"  runat="server" AutoPostBack="True"
                                            OnCheckedChanged="rb_objct_CheckedChanged" />--%>
                                                <asp:RadioButton ID="rb_objct" Width="80px" GroupName="isdesc" Checked="true" runat="server"
                                                    AutoPostBack="true" OnCheckedChanged="rb_objct_CheckedChanged" />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descriptive" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:RadioButton ID="rb_descrip" Width="80px" GroupName="isdesc" runat="server" AutoPostBack="true"
                                                    OnCheckedChanged="rb_descrip_CheckedChanged" />
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Question Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:DropDownList ID="ddlQuestionType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlQuestionType_SelectedIndexChanged"
                                                    Width="100">
                                                    <asp:ListItem Text="Choose The Best Answer" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Fill in The Blanks" Value="2" Selected="False"></asp:ListItem>
                                                    <asp:ListItem Text="Match The Following" Value="3" Selected="False"></asp:ListItem>
                                                    <asp:ListItem Text="True Or False" Value="4" Selected="False"></asp:ListItem>
                                                    <asp:ListItem Text="Rearranging" Value="5" Selected="False"></asp:ListItem>
                                                    <asp:ListItem Text="Paragraph With Questions and Options" Value="6" Selected="False"></asp:ListItem>
                                                </asp:DropDownList>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Question SubType" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:DropDownList ID="ddlQuestionSubType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlQuestionSubType_SelectedIndexChanged"
                                                    Width="100">
                                                    <asp:ListItem Text="Single Answer" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Multiple Answer" Value="2" Selected="False"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlQuestionMatchType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlQuestionMatchType_SelectedIndexChanged"
                                                    Visible="false" Width="100">
                                                    <asp:ListItem Text="Statement Vs Statement" Value="3" Selected="False"></asp:ListItem>
                                                    <asp:ListItem Text="Statement Vs Image" Value="4" Selected="False"></asp:ListItem>
                                                    <asp:ListItem Text="Image Vs Statement" Value="5" Selected="False"></asp:ListItem>
                                                    <asp:ListItem Text="Image Vs Image" Value="6" Selected="False"></asp:ListItem>
                                                </asp:DropDownList>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Available Questions" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_availabl" runat="server" Width="60px" Text=""></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No of Questions" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_noqustion" runat="server" MaxLength="3" AutoPostBack="true"
                                                    autocomplete="off" OnTextChanged="Txt_noqution_OnTextChanged" CssClass="  textbox txtheight1"
                                                    Height="17px" Width="30px" Style=""></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxt_qustion" runat="server"
                                                    TargetControlID="txt_noqustion" FilterType="Numbers,Custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Must Attend Questions" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_mustattnd" runat="server" MaxLength="3" AutoPostBack="true"
                                                    autocomplete="off" OnTextChanged="txt_mustattnd_OnTextChanged" CssClass="  textbox txtheight1"
                                                    Height="17px" Width="30px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxt_mustattnd" runat="server"
                                                    TargetControlID="txt_mustattnd" FilterType="Numbers,Custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Easy" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="90px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_Easy" runat="server" MaxLength="3" autocomplete="off" AutoPostBack="true"
                                                    OnTextChanged="txt_Easy_OnTextChanged" CssClass="  textbox txtheight1" Height="17px"
                                                    Width="30px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxt_Easy" runat="server"
                                                    TargetControlID="txt_Easy" FilterType="Numbers,Custom" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:Label ID="lbl_easy" runat="server" Style="color: Green;" Text=""></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Medium" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="90px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_Medium" runat="server" MaxLength="3" autocomplete="off" AutoPostBack="true"
                                                    OnTextChanged="txt_Medium_OnTextChanged" CssClass="  textbox txtheight1" Height="17px"
                                                    Width="30px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxt_Medium" runat="server"
                                                    TargetControlID="txt_Medium" FilterType="Numbers,Custom" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:Label ID="lbl_mediam" runat="server" Style="color: Green;" Text=""></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Difficult" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="90px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_Difficult" runat="server" MaxLength="3" autocomplete="off" AutoPostBack="true"
                                                    OnTextChanged="txt_Difficult_OnTextChanged" CssClass="  textbox txtheight1" Height="17px"
                                                    Width="30px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxt_Difficult" runat="server"
                                                    TargetControlID="txt_Difficult" FilterType="Numbers,Custom" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:Label ID="lbl_diffc" runat="server" Style="color: Green;" Text=""></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hard" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="90px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_Hard" runat="server" MaxLength="3" autocomplete="off" AutoPostBack="true"
                                                    OnTextChanged="txt_Hard_OnTextChanged" CssClass="  textbox txtheight1" Height="17px"
                                                    Width="30px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxt_Hard" runat="server"
                                                    TargetControlID="txt_Hard" FilterType="Numbers,Custom" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:Label ID="lbl_hard" runat="server" Style="color: Green;" Text=""></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Section Description" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_sec_name" runat="server" autocomplete="off" CssClass="  textbox txtheight1"
                                                    Height="17px"></asp:TextBox>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <asp:Button ID="btn_gendrate" Width="90px" runat="server" Text="Generate " CssClass="textbox textbox1 defaultHeight"
                                OnClick="btn_gendrate_Click" />
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="divPopQuesprepar" runat="server" width="792px;" visible="false" style="height: auto;
                    z-index: 10000; background-color: rgba(54, 25, 25, .2);" class="popupstyle popupheight1 ">
                    <center>
                        <asp:ImageButton ID="ImageButton2" runat="server" Width="792px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; top: 13px; left: 962px;"
                            OnClick="imagebtnpopclose1_Click" />
                    </center>
                    <br />
                    <center>
                        <div id="Div1" runat="server" class="table" style="background-color: White; height: 100%;
                            width: auto; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; border-radius: 10px;
                            overflow: scroll;">
                            <center>
                                <span class="fontstyleheader" style="color: Green">Question And Answer</span>
                                <br />
                                <br />
                                <asp:CheckBox ID="chk_answer" runat="server" Text="Answer" AutoPostBack="True" OnCheckedChanged="chk_answer_CheckedChanged" />
                                <asp:CheckBox ID="chk_option" runat="server" Text="Option" AutoPostBack="True" OnCheckedChanged="chk_option_CheckedChanged" />
                                <br />
                                <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="true" BorderStyle="Solid"
                                    BorderWidth="0px" Style="margin-left: 15px; margin-right: 15px; overflow: scroll;
                                    border: 0px solid #999999; border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                    CssClass="spreadborder" ShowHeaderSelection="false">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </center>
                            <br />
                            <br />
                            <div id="rptprint1" runat="server" visible="true">
                                <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                    Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                                    Height="35px" CssClass="textbox textbox1" />
                                <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                                    CssClass="textbox textbox1" />
                                <asp:Button ID="btn_save" Width="60px" runat="server" BackColor="Green" Text="Save"
                                    CssClass="textbox textbox1 btn1" OnClick="btn_save_Click" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                            </div>
                            <br />
                        </div>
                    </center>
                </div>
            </center>
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
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alert1" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
        </div>
    </center>
</asp:Content>
