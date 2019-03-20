<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Foil_Sheet_for_Internal_External.aspx.cs" Inherits="Foil_Sheet_for_Internal_External" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <html>
    <head>
        <title></title>
        <style type="text/css">
            .head
            {
                font-family: Book Antiqua;
                font-size: medium;
                color: black;
                top: 70px;
                position: absolute;
                font-weight: bold;
                width: 950px;
                height: 25px;
                left: 15px;
            }
            .mainbatch
            {
                background-color: #0CA6CA;
                width: 950px;
                position: absolute;
                height: 145px;
                top: 120px;
                left: 15px;
                font-family: Book Antiqua;
                font-size: medium;
                font-weight: bold;
                color: black;
            }
            .fontset
            {
                font-family: Book Antiqua;
                font-size: medium;
                font-weight: bold;
            }
            
            .stylemodel
            {
                background-color: #FFFFFF;
                border-width: 3px;
                border-style: solid;
                border-color: black;
                width: 200px;
                height: 180px;
                top: 200px;
                position: absolute;
            }
            .collname
            {
                margin-left: 25px;
                margin-top: 350px;
            }
            
            .gvRow
            {
                margin-right: 0px;
                margin-top: 330px;
            }
            
            .gvHeader th
            {
                padding: 3px;
                background-color: #008080;
                color: black;
                border: 1px solid black;
                font-family: Book Antiqua;
                font-size: medium;
                margin-left: 0px;
            }
            .tet
            {
                background: white;
                border: 1px solid #DDD;
                border-radius: 5px;
                box-shadow: 0 0 5px #DDD inset;
                color: #666;
                outline: none;
                height: 25px;
                width: 250px;
            }
            .overflow
            {
                overflow: auto;
                overflow-y: hidden;
            }
            
            .mainbatch1
            {
                width: 200px;
                position: absolute;
                height: 50px;
                top: 300px;
                left: 15px;
            }
            .font14b
            {
                font-family: Book Antiqua;
                font-size: medium;
                font-weight: bold;
            }
            .font14b
            {
                font-family: Book Antiqua;
                font-size: medium;
            }
        </style>
        <script type="text/javascript">


            function clickvalidate() {

                var checkvalidation = document.getElementById('<%=DropExammonth.ClientID%>').value;
                var checkvalidation1 = document.getElementById('<%=DropExamyear.ClientID%>').value;
                var checkvalidation3 = document.getElementById('<%=dropsubject.ClientID%>').value;
                <%--  var checkvalidation2 = document.getElementById('<%=dropdate.ClientID%>').value;
                var checkvalidation4 = document.getElementById('<%=Dropsession.ClientID%>').value;--%> 

                if (checkvalidation == 0 && checkvalidation1 == 0  && checkvalidation3 == "") {
                    alert(" Please Select Exam month" + "\n" + " Please Select Year" + "\n" + " Please Select Date" + "\n" + " Please Select Session" + "\n" + " Please Select Course & Subject");
                    return false;
                }
                else if (checkvalidation == 0) {
                    alert(" Please Select Exam Month");
                    return false;
                }
                else if (checkvalidation1 == 0) {
                    alert(" Please Select Year");
                    return false;
                }
                else if (checkvalidation3 == "") {

                    alert(" Please Select Subject");
                    return false;
                }
                return true;
            }


            function IsNumeric(e) {
                var specialKeys = new Array();
                specialKeys.push(8);
                var keyCode = e.which ? e.which : e.keyCode
                var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
                document.getElementById("error").style.display = ret ? "none" : "inline";
                return ret;
            }

            function buttoncheck() {
                //alert("hi");
                var serial = document.getElementById('<%=txtSerials.ClientID%>').value;
                var section = document.getElementById('<%=txt_section.ClientID%>').value;
                if (serial == "" && section == "") {
                    alert("Please Enter Section & No of columns");
                    return false;
                }
                else if (section == "" && serial != "") {
                    alert("Please Enter No of Columns");
                    return false;
                }
                else if (serial == "" && section != "") {
                    alert("Please Enter Section");
                    return false;
                }
                else {
                    return true;
                }
            }

            function address(txt, max) {
                var empty = "";
                if (txt.value > max) {
                    return empty;
                    alert("Exceeding the Limit");
                }
                if (txt.value == "") {
                    return empty;
                    alert("Please Enter Values");
                }

                if (txt.value == 0) {
                    return empty;
                }

                else {
                    return txt.value;
                }
            }

            function check(txt) {

                var sst = document.getElementById(txt).value;
                var number = /^[a-zA-Z]+$/;
                if (number.test(sst)) {
                }
                else {
                    document.getElementById(txt).value = "";
                    document.getElementById(txt).focus();
                    alert("Please Enter Characters");
                }
            }

            function validate(tx) {
                var sst1 = document.getElementById(tx).value;
                var number = /^[0-9]+$/;
                var number1 = /^[a-zA-Z]+$/;
                if (number.test(sst1) || (number1.test(sst1))) {
                }

                else {
                    document.getElementById(tx).value = "";
                    document.getElementById(tx).focus();
                }
            }

            function submitclick() {
                var empty = "";

                if (txt.value == "") {
                    return empty;
                }
                else {
                    return txt.value;
                }
            }
            //            function display() {
            //                var seriall = document.getElementById('<%=txtexcelname.ClientID%>').value;
            //                if (seriall != "") {
            //                    
            //                }
            //            }
            function display() {
                document.getElementById('MainContent_Iblerr').innerHTML = "";
            }
        </script>
    </head>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <asp:Label ID="lbl_head" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                ForeColor="Green" Font-Size="Large" Text="Foil Sheet For Internal/External"></asp:Label></center>
        <%-- <asp:LinkButton ID="LinkButtonb1" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                Style="left: 760px; top: 0px; position: absolute;"  Font-Bold="true"
                PostBackUrl="~/coe.aspx">Back</asp:LinkButton>
            <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                Style="left: 800px; top: 0px; position: absolute;"  Font-Bold="true"
                PostBackUrl="~/Default_login.aspx">Home</asp:LinkButton>
            <asp:LinkButton ID="lb2" Font-Size="Small" Font-Names="Book Antiqua" Font-Bold="true"
                Style="left: 850px; top: 0px; position: absolute;" runat="server" 
                OnClick="Logout_btn_Click">Logout</asp:LinkButton>--%>
        <div class="mainbatch" style="height: 160px;">
            <table style="margin-left: 10px; margin-top: -200px; position: absolute;">
                <tr>
                    <td>
                        <asp:Label ID="IblYear" Text="Exam Year" Style="position: absolute; left: 0px; top: 210px;"
                            Width="100px" runat="server"> </asp:Label>
                        <asp:DropDownList ID="DropExamyear" runat="server" Style="position: absolute; left: 100px;
                            top: 210px;" Width="100px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                            AutoPostBack="true" ForeColor="Black">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="IblExam" Text="Exam Month" Style="position: absolute; left: 224px;
                            top: 210px;" Width="100px" runat="server"> </asp:Label>
                        <asp:DropDownList ID="DropExammonth" runat="server" Style="position: absolute; left: 330px;
                            top: 210px;" Width="100px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                            AutoPostBack="true" ForeColor="Black">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkmergecoll" runat="server" Text="Merge College" Style="position: absolute;
                            left: 450px; top: 200px;" Width="170" />
                        <asp:CheckBox ID="chkbatch" runat="server" Text="Include Batch" Style="position: absolute;
                            left: 450px; top: 217px;" Width="170" AutoPostBack="true" OnCheckedChanged="chkbatch_CheckedChanged" />
                        <asp:CheckBox ID="chksubwise" runat="server" Text="Inculde Department" Style="position: absolute;
                            left: 450px; top: 236px;" Width="170" AutoPostBack="true" OnCheckedChanged="chksubwise_CheckedChanged" />
                    </td>
                    <td>
                        <asp:Label ID="lblbatch" Text="Batch" Style="position: absolute; left: 630px; top: 210px;"
                            Width="100px" runat="server"> </asp:Label>
                        <asp:DropDownList ID="ddlbatch" runat="server" AutoPostBack="true" Style="position: absolute;
                            left: 695px; top: 210px;" Width="65px" ForeColor="Black" OnSelectedIndexChanged="ddlbatch_selected">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbldegree" Text="Course" Style="position: absolute; left: 765px; top: 210px;"
                            Width="100px" runat="server"> </asp:Label>
                        <asp:DropDownList ID="ddldegree" runat="server" AutoPostBack="true" Style="position: absolute;
                            left: 832px; top: 210px;" Width="100px" ForeColor="Black" OnSelectedIndexChanged="ddldegree_selected">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Iblcourse" Text="Department" Style="position: absolute; left: 0px;
                            top: 260px;" Width="100px" runat="server"> </asp:Label>
                        <asp:DropDownList ID="DropCourse" runat="server" AutoPostBack="true" Style="position: absolute;
                            left: 100px; top: 260px;" Width="150px" ForeColor="Black" OnSelectedIndexChanged="dropcourse_selected">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsem" Text="Sem" Style="position: absolute; left: 270px; top: 260px;"
                            Width="100px" runat="server"> </asp:Label>
                        <asp:DropDownList ID="ddlsem" runat="server" AutoPostBack="true" Style="position: absolute;
                            left: 330px; top: 260px;" Width="100px" ForeColor="Black" OnSelectedIndexChanged="ddlsem_selected">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsubtype" runat="server" Text="Subject Type" Style="position: absolute;
                            left: 450px; top: 260px;" Width="130px"></asp:Label>
                        <asp:DropDownList ID="ddlsubtype" Width="100px" Font-Bold="true" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlsubtype_selected" runat="server" Style="position: absolute;
                            left: 550px; top: 260px;">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="IblSubject" runat="server" Text="Subject" Style="position: absolute;
                            left: 657px; top: 260px;" Width="130px"></asp:Label>
                        <asp:DropDownList ID="dropsubject" Width="200px" Font-Bold="true" AutoPostBack="true"
                            OnSelectedIndexChanged="dropsubject_selected" runat="server" Style="position: absolute;
                            left: 721px; top: 260px;">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:LinkButton ID="link" Text="Setting" runat="server" Style="position: absolute;
                            left: 873px; top: 310px;" Height="30px" ForeColor="Black" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" OnClick="linksetting" OnClientClick=" return clickvalidate();"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Iblbundle" runat="server" CssClass="font14b" Text="Bundle" Style="position: absolute;
                            left: 0px; top: 310px;"></asp:Label>
                        <asp:DropDownList ID="Dropbundle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dropbundle_selected"
                            CssClass="font14b" Width="50px" Style="position: absolute; left: 100px; top: 308px;">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbform1" runat="server" Text="Format 1" GroupName="Report" Font-Names="Book Antiqua"
                            Style="position: absolute; left: 161px; top: 310px;" Font-Bold="true" Font-Size="Medium"
                            Width="110px" AutoPostBack="true" OnCheckedChanged="Forametchage" />
                        <asp:RadioButton ID="rbform2" runat="server" Text="Format 2" GroupName="Report" Font-Names="Book Antiqua"
                            Style="position: absolute; left: 261px; top: 310px;" Font-Bold="true" Font-Size="Medium"
                            AutoPostBack="true" Width="110px" OnCheckedChanged="Forametchage" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkconsolidate" Text="Date Wise Consolidate" runat="server" Visible="true"
                            AutoPostBack="true" Style="position: absolute; left: 354px; top: 310px;" Width="250px"
                            OnCheckedChanged="chkconsolidate_checkedchange" />
                        <asp:DropDownList ID="ddlpdateexam" runat="server" Style="margin-left: 536px; margin-top: 299px;">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIIIval" Text="III Valuation Only" runat="server" Enabled="true"
                            Visible="true" AutoPostBack="true" Style="position: absolute; left: 649px; top: 310px;"
                            Width="250px" OnCheckedChanged="chkconsolidate_checkedchange" />
                    </td>
                    <td>
                        <asp:Button ID="Btngo" runat="server" Visible="true" Style="position: absolute; left: 815px;
                            top: 308px;" Text="Go" OnClick="btngo_click" OnClientClick="return clickvalidate()" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:CheckBox ID="Cb_CheckBox" runat="server" Text="Inculude Seating Arrangement" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Label ID="Iblerror" runat="server" CssClass="font14b" Visible="false" Text=""
            Style="margin-left: -9px; margin-top: 280px; position: absolute;" ForeColor="Red">
        </asp:Label>
        <div>
            <asp:GridView ID="showreport" runat="server" AutoGenerateColumns="true" Font-Bold="true"
                Font-Names="Book Antiqua" Font-Size="Medium" HeaderStyle-CssClass="gvHeader"
                CssClass="gvRow" Style="margin-left: -9px; margin-top: 310px;" OnRowCreated="gridview_created"
                OnRowDataBound="GridView1_RowDataBound">
            </asp:GridView>
        </div>
        <div id="printdiv" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Button ID="Excel" Text="Export Excel" runat="server" Width="120px" Height="35px"
                            Style="position: absolute; right: 145px;" Font-Names="Book Antiqua" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Visible="false" OnClick="Exportexcel_click" />
                    </td>
                    <td>
                        <asp:Button ID="btnpdf" Text="Print" runat="server" Style="position: absolute; right: 82px;"
                            Font-Bold="true" Font-Names="Book Antiqua" Visible="false" ForeColor="Black"
                            Font-Size="Medium" Height="35px" OnClick="btnpdf_generate" />
                    </td>
                </tr>
            </table>
        </div>
        <center>
            <div style="margin-left: -10px; margin-top: 310px;">
                <FarPoint:FpSpread ID="AttSpreadfoil" runat="server" BorderColor="Black" BorderStyle="Solid"
                    OnButtonCommand="AttSpreadfoil_OnUpdateCommand" BorderWidth="1px" Visible="false"
                    VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never" CssClass="stylefp">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <%--  <FarPoint:FpSpread ID="Foilprint" runat="server" BorderColor="Black" BorderStyle="Solid"
                OnButtonCommand="foilprint_OnUpdateCommand" BorderWidth="1px" Visible="false"
                VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never" CssClass="stylefp">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>--%>
            </div>
            <div>
                <asp:Label ID="Iblerr" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="true"
                    Font-Size="Medium" Font-Names="Book Antique" Style="position: absolute; left: 75px;"></asp:Label>
                <asp:Label ID="IblRpt" runat="server" Text="Report Name" Visible="false" ForeColor="Black"
                    Font-Bold="true" Font-Size="Medium" Font-Names="Book Antique" Style="position: absolute;
                    left: 75px;"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Visible="false" Height="20px" Width="180px"
                    Style="position: absolute; left: 175px;" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+|\}{][':;?><,./">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="printexcel" Text="Export To Excel" runat="server" OnClick="btnprintexcel_click"
                    Visible="false" Style="position: absolute; left: 365px;" onkeypress="display()" />
                <asp:Button ID="printpdf" Text="Print" runat="server" OnClick="printpdf_click" Visible="false"
                    Style="position: absolute; left: 505px;" />
            </div>
        </center>
        <div style="height: auto; width: auto; overflow-x: hidden; overflow-y: auto;">
            <asp:ModalPopupExtender ID="modelpopsetting" PopupControlID="Panel1" TargetControlID="link"
                runat="server">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panel1" runat="server" Visible="false" Style="background-color: White;
                border-color: Black; border-style: solid; border-width: 3px; padding: 15px; top: 150px;">
                <asp:Label ID="Iblcoursefilter" runat="server" Text="Course" Style="margin-left: 5px;
                    position: absolute;" CssClass="font14b"></asp:Label>
                <asp:DropDownList ID="dropcoursefilter" CssClass="font14b" Width="93px" Style="margin-left: 116px;
                    position: absolute;" runat="server">
                </asp:DropDownList>
                <asp:Label ID="Iblnoofsec" runat="server" Text="Section Name" Font-Names="Book Antiqua"
                    Font-Size="Medium" Font-Bold="true"></asp:Label>
                <asp:TextBox ID="txtSerials" runat="server" MaxLength="20" Style="margin-left: 14px;
                    position: absolute;" Font-Names="Book Antiqua" Font-Size="Medium" Width="88px"></asp:TextBox>
                <asp:Label ID="Label1" runat="server" Text="No of Columns" Font-Names="Book Antiqua"
                    Font-Size="Medium" Font-Bold="true"></asp:Label>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom"
                    ValidChars="123456789" TargetControlID="txt_section">
                </asp:FilteredTextBoxExtender>
                <asp:TextBox ID="txt_section" runat="server" Width="88px" sFont-Names="Book Antiqua"
                    Font-Size="Medium" MaxLength="1" onkeypress="return IsNumeric(event);"></asp:TextBox>
                <asp:Button ID="buttonpopup" Style="margin-left: 20px; position: absolute;" Text="Go"
                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" runat="server"
                    OnClick="buttongoClick" OnClientClick="return buttoncheck();" />
                <asp:LinkButton ID="close" Text="X" Font-Names="Book Antiqua" Font-Size="Medium"
                    Font-Bold="true" ForeColor="Black" Style="right: 20px; position: absolute; top: 10px;"
                    runat="server" OnClick="closepanel" OnClientClick="closepanel1"></asp:LinkButton>
                <center>
                    <div style="height: 171px; width: 340px; overflow-x: hidden; overflow-y: auto;">
                        <table>
                            <tr align="center">
                                <td align="center">
                                    <asp:GridView ID="showgrid" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" HeaderStyle-CssClass="gvHeader" Style="margin-left: 20px;"
                                        Width="300px" Height="150px" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Section" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium"
                                                HeaderStyle-Font-Names="Book Antiqua">
                                                <ItemTemplate>
                                                    <asp:Label ID="Iblsection" Width="100px" runat="server" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Font-Bold="true" Text='<%#Eval("serials")%>'>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No of Columns" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-VerticalAlign="Middle" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium"
                                                HeaderStyle-Font-Names="Book Antiqua">
                                                <ItemTemplate>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom"
                                                        ValidChars="1234567890" TargetControlID="txt_column">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:TextBox ID="txt_column" Width="100px" MaxLength="2" onkeypress="this.value=address(this,20)"
                                                        CssClass="tet" runat="server" Text='<%#Eval("columns")%>'>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
                <center>
                    <asp:Button ID="btnok" Text="Ok" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true" runat="server" OnClick="btnok_Click" />
                </center>
                <center>
                    <FarPoint:FpSpread ID="FpFoilSetting" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="100" Width="649" HorizontalScrollBarPolicy="AsNeeded"
                        VerticalScrollBarPolicy="AsNeeded">
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
                    <asp:Button ID="btnsubmit" Text="Submit" Visible="false" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="true" runat="server" OnClientClick="return submitclick();"
                        OnClick="btngo_grid" />
                </center>
            </asp:Panel>
        </div>
        <%--     <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />--%>
    </body>
    </html>
</asp:Content>
