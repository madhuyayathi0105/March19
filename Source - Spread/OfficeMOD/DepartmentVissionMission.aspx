<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DepartmentVissionMission.aspx.cs" Inherits="DepartmentVissionMission" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function validate() {
                var empty = true;
                var viss = document.getElementById('<%=txtvission.ClientID%>').value;
                var miss = document.getElementById('<%=txtmission.ClientID%>').value;
                if (viss == "") {
                    document.getElementById('<%=txtvission.ClientID%>').style.borderColor = 'Red';
                    empty = false;
                }
                if (miss == "") {
                    document.getElementById('<%=txtmission.ClientID%>').style.borderColor = 'Red';
                    empty = false;
                }
                if (!empty) {
                    return false;
                }
            }
            function display(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function displays() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Department Vision Mission</span></div>
            </center>
        </div>
        <div>
            <center>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblclg" Text="College" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Width="250px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <%--OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true"--%>
                        </td>
                        <td>
                            <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UP_degree" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="panel_degree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Up_dept" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                        PopupControlID="panel_dept" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Font-Bold="true" Text="Go"
                                OnClick="btngo_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnadd" runat="server" CssClass="textbox btn2" Font-Bold="true" Text="Add"
                                OnClick="btnadd_Click" />
                        </td>
                    </tr>
                </table>
            </center>
            <center>
                <%-- <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">--%>
                <table>
                    <tr>
                        <td>
                            <br />
                            <div style="height: 410px; overflow: auto;">
                                <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                                    BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                    background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder"
                                    OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <div id="print" runat="server" visible="false">
                                    <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        ForeColor="Red" Text="" Visible="false"></asp:Label>
                                    <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="displays(this)"
                                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                                    <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                        CssClass="textbox textbox1" Width="60px" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                                </div>
                            </center>
                        </td>
                    </tr>
                </table>
                <%--  </div>--%>
            </center>
            <div id="divadd" runat="server" visible="false" style="height: 55em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <asp:ImageButton ID="imgclose" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 7%; margin-left: 911px;"
                    OnClick="imgclose_Click" />
                <center>
                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 500px;
                        width: 850px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 8%;
                        border-radius: 10px;">
                        <br />
                        <center>
                            <span style="font-size: larger; font-weight: bold; font-family: Book Antiqua; color: Green;">
                                Add Details</span>
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Text="College" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcollegeadd" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="250px" OnSelectedIndexChanged="ddlcollegeadd_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <%--OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true"--%>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Degree"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegreeadd" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddldegreeadd_SelectedIndexChanged" AutoPostBack="true"
                                            Style="height: auto;">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Department"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldeptadd" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Style="height: auto;">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                     <span style="color: Green; font-size: 14px; font-family: Book Antiqua; font-weight: bold;">
                                            Vision:</span>
                                        <span style="color: Red; font-size: 14px; font-family: Book Antiqua; font-weight: bold;">
                                            Separate New Line Please Add This Symbol($) End Of The Line</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtvission" runat="server" placeholder="Enter The Vision" TextMode="MultiLine"
                                            Style="height: 150px; width: 650px;" onkeypress="display(this)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txtvission"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ,.$-">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Green; font-size: 14px; font-family: Book Antiqua; font-weight: bold;">
                                            Mission:</span> <span style="color: Red;
                                                font-size: 14px; font-family: Book Antiqua; font-weight: bold;">Separate New Line
                                                Please Add This Symbol($) End Of The Line</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtmission" runat="server" placeholder="Enter The Mission" TextMode="MultiLine"
                                            Style="height: 150px; width: 650px;" onkeypress="display(this)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtmission"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ,.$-">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnsave" runat="server" CssClass="textbox btn2" Font-Bold="true"
                                            Text="Save" OnClientClick="return validate();" OnClick="btnsave_Click" />
                                        <asp:Button ID="btnclear" runat="server" CssClass="textbox btn2" Font-Bold="true"
                                            Text="Clear" OnClick="btnclear_Click" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </div>
        <center>
            <div id="imgdiv2" runat="server" visible="false" style="height: 55em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 30%;
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
        </center>
    </body>
</asp:Content>
