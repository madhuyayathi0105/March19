<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AdmissionNoGeneration.aspx.cs" Inherits="StudentMod_AdmissionNoGeneration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .txtbgColor
        {
            background-color: White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function functionx(evt) {
                if (evt.charCode > 31 && (evt.charCode < 48 || evt.charCode > 57)) {
                    alert("Allow Only Numbers");
                    return false;
                }
            }
            function validate() {
                var empty = true;
                var size = document.getElementById("<%=txtsize.ClientID %>").value;
                if (size == "") {
                    size = document.getElementById("<%=txtsize.ClientID %>");
                    size.style.borderColor = 'Red';
                    empty = false;
                }
                if (empty == false) {
                    return false;
                }
            }
            
  
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <span style="color: Green; top: 80px; left: 39%; position: absolute; font-weight: bold;
                    font-size: large;">Admission Number Generation Settings </span>
            </center>
        </div>
        <div class="center" style="position: relative; height: 130em; border-left: 1px solid;
            border-right: 1px solid; border-style: solid; top: 40px; width: 95%; border-color: Gray;">
            <br />
            <div id="step1" runat="server">
                <center>
                    <table>
                        <tr align="center">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:RadioButton ID="rdb_admissionno" runat="server" GroupName="rr" Text="Admission No Generation"
                                    AutoPostBack="true" OnCheckedChanged="rdb_admissionnoCheckedChanged" Checked="true" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_applicationno" runat="server" GroupName="rr" Text="Application No Generation"
                                    AutoPostBack="true" OnCheckedChanged="rdb_applicationno_CheckedChanged" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </center>
            </div>
            <center>
                <%--<asp:UpdatePanel ID="partupdate" runat="server">
                            <ContentTemplate>--%>
                <div style="width: 100%;">
                    <center>
                        <table>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblsize" runat="server" Text="Admission Number"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsize" CssClass="textbox textbox1" runat="server" Width="60px"
                                        onkeypress="return functionx(event)" onfocus="myFunction(this)"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblclgcode" runat="server" Text="Institution Name"></asp:Label>
                                </td>
                                <%--<td align="left">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtclgacr" runat="server" Height="20px" Visible="true" ReadOnly="true"
                                                CssClass="dropdown" Style="width: 120px; ">---Select---</asp:TextBox>
                                            <asp:Panel ID="pclgacr" runat="server" BackColor="White" Visible="true" BorderColor="Black"
                                                BorderStyle="Solid" BorderWidth="2px" Height="150px" Width="300px" Style="font-family: 'Book Antiqua';
                                                overflow-y: scroll;">
                                                <asp:CheckBox ID="chkclgacr" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="chkclgacr_CheckedChanged" />
                                                <asp:CheckBoxList ID="chklstclgacr" runat="server" AutoPostBack="True" Width="300px"
                                                    Height="58px" OnSelectedIndexChanged="chklstclgacr_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtclgacr"
                                                PopupControlID="pclgacr" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                --%>
                                <td>
                                    <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox1 ddlstyle ddlheight3"
                                        OnSelectedIndexChanged="ddlclg_SelectedIndexChanged" Width="217px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btngo" CssClass="textbox textbox1 type" runat="server" Style="width: 60px;
                                        height: 30px;" Text="Go" OnClientClick="return validate()" OnClick="Generate_Click" />
                                </td>
                        </table>
                    </center>
                </div>
                <br />
                <center>
                    <div id="popup_alert" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl" runat="server" class="table" style="background-color: White; height: 150px;
                                width: 250px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 100px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <asp:Label ID="lblerror" runat="server" ForeColor="Red" Visible="false" Font-Size="medium"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Button ID="btn_close" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                        OnClick="btnclose_Click" Text="Ok" runat="server" />
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <center>
                    <%--OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged"   OnButtonCommand="btnType_Click"--%>
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                        OnButtonCommand="FpSpread2_UpdateCommand" OnPreRender="FpSpread2_SelectedIndexChanged">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                <br />
                <asp:Button ID="btnSave" CssClass="textbox textbox1 type" runat="server" Style="width: 100px;
                    height: 35px;" Text="Save" Visible="false" OnClientClick="return validate()"
                    OnClick="Generate_Go_Click" />
            </center>
        </div>
        <center>
            <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 500px;
                        width: 350px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 100px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: -48px; margin-left: 201px;"
                                OnClick="imagebtnpopclose3_Click" />
                            <asp:Label ID="lblheader" runat="server" Text="Seat Type" Style="color: Green;" Font-Bold="true"
                                Font-Size="Medium" Visible="true"></asp:Label>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="overflow: auto; height: 400px;">
                                            <asp:GridView ID="gridquota" runat="server" AutoGenerateColumns="false" Width="300px"
                                                CssClass="spreadborder" Style="overflow: auto; font-size: small; height: 400px;"
                                                HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" Visible="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Seat Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltext" runat="server" Text='<%# Eval("textval") %>'></asp:Label>
                                                            <asp:Label ID="lblcode" runat="server" Visible="false" Text='<%# Eval("textcode") %>'></asp:Label>
                                                            <asp:Label ID="lblclgcode" runat="server" Visible="false" Text='<%# Eval("collegecode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Seat Code">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_code" runat="server" Text='<%# Eval("priority2") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnerrclose_Click" Text="Save" runat="server" />
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
