<%@ Page Title="Send Currcular Master" Language="C#" MasterPageFile="~/smsmod/SMSSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CollegeWiseDegreeWiseSendCircularMaster.aspx.cs"
    Inherits="CollegeWiseDegreeWiseSendCircularMaster" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
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
    <style type="text/css">
        .style1
        {
            width: 80px;
        }
        .style2
        {
            width: 120px;
        }
        .dropdown
        {
            font: 12px/0.8 Arial;
            border: solid 1px #6FA602;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            cursor: pointer;
            width: auto;
        }
        .modal_popup_background_color
        {
            background-color: #666699;
            filter: alpha(opacity=100);
            opacity: 0.7;
        }
    </style>
    <script type="text/javascript">
        function get(txt1, lbl1batch, lbl1degree, lbl1semester, lbl1section, lblentry) {
            var batch = lbl1batch;
            var degree = lbl1degree;
            var sem = lbl1semester;
            var sect = lbl1section;
            var sst = document.getElementById(txt1).value;
            var enrty = lblentry;
            $.ajax({
                type: "POST",
                url: "AllStudentAttendance1.aspx/CheckUserName",
                data: '{rollno: "' + sst + '",batch:"' + batch + '",degree:"' + degree + '",sem:"' + sem + '",sec:"' + sect + '",entryby:"' + enrty + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccess(response) {
            var mesg = $("#msg1")[0];
            switch (response.d) {
                case "0":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Admission No Not Exist";
                    break;
                case "1":
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "2":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Please Enter Admission No";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }

        function restrictspecial(e, key, obj) {
            var keynum;
            if (window.event) // IE
            {
                keynum = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                keynum = e.which;
            }
            switch (key) {

                case "STRING":
                    var keyboardchars = /[\x00\x08]/;
                    var validchars = new RegExp("[ A-Za-z]");
                    break;

                case "NUMERIC1TO9":
                    var keyboardchars = /[\x00\x08]/;
                    var validchars = new RegExp("[1-9]");
                    break;

                case "EMAIL":
                    var keyboardchars = /[\x00\x08]/;
                    var validchars = new RegExp("[A-Za-z0-9,]");
                    break;
            }
            var keychar = String.fromCharCode(keynum);
            if (!validchars.test(keychar) && !keyboardchars.test(keychar)) {
                return false
            } else {
                return keychar.toUpperCase();
            }
        }

        function setHeight(txtdesc) {
            txtdesc.style.height = txtdesc.scrollHeight + "px";
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
                                <span class="Header">Circular Master</span>
                            </center>
                        </td>
                    </tr>
                </thead>
            </table>
            <center>
                <div class="maindivstyle" style="width: 100%; height: auto; width: -moz-max-content;">
                    <table class="maintablestyle" style="margin: 10px; width: auto; height: auto;">
                        <tr>
                            <td>
                                <asp:Label ID="lblCollege" CssClass="fontCommon" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <%--<div style="position: relative;">--%>
                                <asp:UpdatePanel ID="upnlBatch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtBatch" runat="server" Height="20px" CssClass="textbox textbox1"
                                            ReadOnly="true" Width="120px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pnlBatch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="chkBatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkBatch_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblBatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBatch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popUpExtBatch" runat="server" TargetControlID="txtBatch"
                                            PopupControlID="pnlBatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%-- </div>--%>
                            </td>
                            <td>
                                <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <%--<div style="position: relative;">--%>
                                <asp:UpdatePanel ID="upnlDegree" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtDegree" runat="server" Height="20px" ReadOnly="true" CssClass="textbox textbox1"
                                            Width="120px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pnlDegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            Height="250px" Width="200px" BorderWidth="2px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="chkDegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblDegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popUpExtDegree" runat="server" TargetControlID="txtDegree"
                                            PopupControlID="pnlDegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%-- </div>--%>
                            </td>
                            <td>
                                <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <%-- <div style="position: relative;">--%>
                                <asp:UpdatePanel ID="unlBranch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtBranch" runat="server" Height="20px" CssClass="textbox textbox1"
                                            ReadOnly="true" Width="120px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pnlBranch" runat="server" Height="250px" BackColor="White" BorderColor="Black"
                                            BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="chkBranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblBranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popUpExtBranch" runat="server" TargetControlID="txtBranch"
                                            PopupControlID="pnlBranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%-- </div>--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" ForeColor="Black"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnlSec" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtSec" runat="server" Height="20px" CssClass="textbox textbox1"
                                                        ReadOnly="true" Width="120px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                    <asp:Panel ID="pnlSec" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                                        <asp:CheckBox ID="chkSec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSec_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblSec" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                            Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Height="58px" OnSelectedIndexChanged="cblSec_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popUpExtSec" runat="server" TargetControlID="txtSec"
                                                        PopupControlID="pnlSec" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblCollegeOrDegreeWise" runat="server" RepeatDirection="Horizontal"
                                                RepeatLayout="Table" AutoPostBack="true" CssClass="fontCommon" OnSelectedIndexChanged="rblCollegeOrDegreeWise_SelectedIndexChanged_">
                                                <asp:ListItem Selected="True" Text="College Wise" Value="0"></asp:ListItem>
                                                <asp:ListItem Selected="False" Text="Degree Wise" Value="1"></asp:ListItem>
                                                <%--<asp:ListItem Selected="False" Text="Degree Wise" Value="0"></asp:ListItem>--%>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnGo" runat="server" Height="30px" CssClass="textbox1 btn2" Text="Go"
                                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnGo_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <div id="divMainGrid" runat="server" visible="false" style="margin: 10px;">
                        <center>
                            <div id="divPrint" runat="server" visible="false" style="margin: 20px;">
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
                                <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                                <%--<asp:Button ID="btnSave" runat="server" Visible="true" Width="60px" Height="35px"
                                    CssClass="textbox textbox1" Text="Save" Font-Bold="true" OnClick="btnSave_Click" />--%>
                            </div>
                            <table id="Panel3" runat="server" style="margin-left: 0px; width: 976px;">
                                <tr>
                                    <td>
                                        <div id="divDegreeDetails" visible="false" runat="server" style="border-style: none;
                                            border-color: inherit; border-width: 2px; height: 425px; width: 973px; overflow: scroll;">
                                            <asp:GridView Visible="False" ID="gvDegreeDetails" runat="server" AutoGenerateColumns="False"
                                                CellPadding="1" ForeColor="#333333" GridLines="None" Width="559px">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" ShowHeader="true" HeaderStyle-Width="70px" ItemStyle-Width="70px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Bold="true"
                                                        ControlStyle-Font-Size="Medium" HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSno" Width="40px" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Left" Font-Size="Medium" Width="70px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" Font-Bold="True" Width="70px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Batch" ShowHeader="true" HeaderStyle-Width="80px"
                                                        ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left"
                                                        ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDWiseBatch" Width="40px" runat="server" Text='<%#Eval("batch_Year")%>' />
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="True" Font-Size="Medium" Width="80px">
                                                        </HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Degree" ShowHeader="true" HeaderStyle-Width="110px"
                                                        ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDWiseCourse_Name" Width="80px" runat="server" Text='<%#Eval("Course_Name")%>' />
                                                            <asp:Label ID="lblDWiseCourse_id" Visible="false" runat="server" Text='<%#Eval("degree_code")%>' />
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Font-Size="Medium" Width="100px">
                                                        </HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" Width="110px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Department" ShowHeader="true" HeaderStyle-Width="150px"
                                                        ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                        ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDWiseDept_Name" Width="180px" runat="server" Text='<%#Eval("Dept_Name")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="True" Font-Size="Medium" Width="150px">
                                                        </HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Width="150px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sem" ShowHeader="true" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true"
                                                        HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDWiseCurrent_Semester" Width="38px" runat="server" Text='<%#Eval("current_semester")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Font-Size="Medium" Width="70px">
                                                        </HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sec" ShowHeader="true" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true"
                                                        HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDWiseSections" Width="38px" runat="server" Text='<%#Eval("sections")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Font-Size="Medium" Width="70px">
                                                        </HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Circular Message" ShowHeader="true" HeaderStyle-Width="400px"
                                                        ItemStyle-Width="400px" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true"
                                                        HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDWiseCircularMessage" runat="server" Width="385px" AutoPostBack="true"
                                                                TextMode="MultiLine" Height="33px" onkeydown="setHeight(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle Font-Bold="True" Font-Size="Medium" Width="400px"></HeaderStyle>
                                                        <ItemStyle Width="400px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Select" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center"
                                                        ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" Font-Overline="false"
                                                                CommandArgument='<%# Container.DataItemIndex %>' Text="Select"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle Font-Bold="True" Font-Size="Medium" Width="80px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Select" ShowHeader="true" HeaderStyle-Width="400px"
                                                        ItemStyle-Width="400px" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true"
                                                        HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkDWiseSelect" runat="server" Width="100px" />
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle Font-Bold="True" Font-Size="Medium" Width="400px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EditRowStyle BackColor="#7C6F57" />
                                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#E3EAEB" />
                                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                                            </asp:GridView>
                                        </div>
                                        <div id="divCollegeWise" visible="false" runat="server" style="border-style: none;
                                            border-color: inherit; border-width: 2px; height: 425px; width: 973px; overflow: scroll;">
                                            <asp:GridView Visible="False" ID="gvCollegeWise" runat="server" AutoGenerateColumns="False"
                                                CellPadding="1" ForeColor="#333333" GridLines="None" Style="width: 100%;">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-Width="70px" ItemStyle-Width="70px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                                        ItemStyle-Font-Bold="true" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSno" Width="40px" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Left" Font-Size="Medium" Width="70px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="True" Width="70px">
                                                        </ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="College Name" HeaderStyle-Width="480px" ShowHeader="true"
                                                        ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-VerticalAlign="Middle" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true"
                                                        HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCWiseCollegeName" Style="width: auto;" runat="server" Text='<%#Eval("collname")%>' />
                                                            <asp:Label ID="lblCWiseCollegeCode" Visible="false" Width="40px" runat="server" Text='<%#Eval("college_code")%>' />
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="True" Font-Size="Medium" Width="80px">
                                                        </HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Font-Bold="True" VerticalAlign="Middle" Width="480px">
                                                        </ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Circular Message" HeaderStyle-Width="400px" ItemStyle-Width="400px"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ControlStyle-Font-Size="Medium"
                                                        HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtCWiseCircularMessage" runat="server" Width="385px" AutoPostBack="true"
                                                                TextMode="MultiLine" Height="33px" onkeydown="setHeight(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle Font-Bold="True" Font-Size="Medium" Width="400px"></HeaderStyle>
                                                        <ItemStyle Width="400px" VerticalAlign="Middle" HorizontalAlign="Left"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="400px" ItemStyle-Width="400px"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" ControlStyle-Font-Size="Medium"
                                                        HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkCWiseSelect" runat="server" Width="100px" />
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Size="Medium"></ControlStyle>
                                                        <HeaderStyle Font-Bold="True" Font-Size="Medium" Width="400px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EditRowStyle BackColor="#7C6F57" />
                                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#E3EAEB" />
                                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:CheckBox ID="chkselectall" runat="server" Font-Bold="True" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="chkselectall_Change" Style="padding-top: 5px;"
                                            Font-Names="Book Antiqua" Visible="false" Font-Size="Medium" />
                                        <asp:CheckBox ID="chkSms" runat="server" Font-Bold="True" Text="SMS" Style="padding-top: 5px;"
                                            Font-Names="Book Antiqua" Visible="false" Font-Size="Medium" />
                                        <%--<asp:CheckBox ID="chkvoice" runat="server" Font-Bold="True" Text="Voice Call" Style="padding-top: 5px;"
                                            Font-Names="Book Antiqua" Visible="false" Font-Size="Medium" />--%>
                                        <asp:Button ID="btnSendSMS" runat="server" Text="Send SMS" Font-Bold="True" Font-Names="Book Antiqua"
                                            Visible="false" Font-Size="Medium" OnClick="btnSendSMS_Click" />
                                        <asp:Button ID="btnprintmaster" runat="server" Text="Cancel" Width="85px" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="true" Visible="false" />
                                    </td>
                                </tr>
                            </table>
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
                </div>
            </center>
        </div>
    </center>
</asp:Content>
