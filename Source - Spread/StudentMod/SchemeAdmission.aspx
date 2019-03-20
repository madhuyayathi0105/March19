<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SchemeAdmission.aspx.cs" Inherits="SchemeAdmission" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        body
        {
            font-family: Book Antiqua;
            font-weight: bold;
            font-size: 17px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function getapplNo(txtapplno) {
            $.ajax({
                type: "POST",
                url: "SchemeAdmission.aspx/applicationNo",
                data: '{applno: "' + txtapplno + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: Success,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function Success(response) {
            var mesg1 = document.getElementById('<%=lblerr.ClientID %>');
            switch (response.d) {
                case "0":
                    mesg1.style.color = "green";
                    mesg1.innerHTML = "Not Exist";
                    break;
                case "1":
                    mesg1.style.color = "Red";
                    document.getElementById('<%=txtadmno.ClientID %>').value = "";
                    mesg1.innerHTML = "Exist";
                    break;
                case "2":
                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Enter Admission No";
                    break;
                case "error":
                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Error Occurred";
                    break;
            }
        }
    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Scheme Admission</span></div>
        </center>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 950px; height: 450px;">
                <br />
                <fieldset id="fldRad" style="width: 350px; border: 1px solid #999999; background-color: #F0F0F0;
                    box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999; -webkit-box-shadow: 0px 0px 10px #999999;
                    border: 3px solid #D9D9D9; border-radius: 15px;">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="radApplNo" runat="server" Checked="true" Text="Application No"
                                    OnCheckedChanged="radApplNo_Change" AutoPostBack="true" GroupName="RadAppAdmNo" />
                            </td>
                            <td>
                                <asp:RadioButton ID="radAdmNo" runat="server" Text="Admission No" OnCheckedChanged="radAdmNo_Change"
                                    AutoPostBack="true" GroupName="RadAppAdmNo" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblclg" Text="College" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" Height="25px" runat="server" CssClass="textbox3 textbox1"
                                Width="190px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblappno" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtappl" runat="server" CssClass="txtheight3 txtcaps" OnTextChanged="txtappl_Changed"
                                AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" id="tddet" runat="server" visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblstudname" runat="server" Text="Student Name:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbstudname" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="lbappno" runat="server" Visible="false" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="School Type:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbscltype" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblstand" runat="server" Text="Standard:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbstand" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="lbldegree" runat="server" Visible="false" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblyear" runat="server" Text="Year:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbyear" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="lblclgcode" runat="server" Visible="false" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_reas" runat="server" Text="Scheme Type"></asp:Label>
                                        <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                        <asp:DropDownList ID="ddl_reason" runat="server" CssClass="textbox3 textbox1" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbamt" runat="server" Text="Amount"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtamount" runat="server" CssClass="textbox textbox1" MaxLength="9"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="ftTxt" runat="server" FilterType="Numbers,Custom"
                                            ValidChars="." TargetControlID="txtamount">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr id="trAdm" runat="server">
                                    <td>
                                        <asp:CheckBox ID="cbincadmis" runat="server" Text="Admission No" AutoPostBack="true"
                                            OnCheckedChanged="cbincamdis_Changed" />
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtadmno" runat="server" Enabled="false" CssClass="textbox textbox1"
                                            onkeypress="display(this)" onblur="return getapplNo(this.value)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom"
                                            ValidChars="." TargetControlID="txtadmno">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblerr" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="btnadmit" runat="server" Text="Admit" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox btn2" OnClick="btnadmit_Click" />
                                        <%--  </td>
                                    <td>--%>
                                        <asp:Button ID="btnclear" runat="server" Text="Reset" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox btn2" OnClick="btnclear_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <center>
            <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                <center>
                    <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                        height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <table style="line-height: 30px">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_addgroup" runat="server" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox textbox1"
                                        onkeypress="display1()"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="line-height: 35px">
                                    <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Names="Book Antiqua"
                                        CssClass="textbox textbox1" Height="32px" Width="60px" OnClick="btn_addgroup_Click" />
                                    <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Names="Book Antiqua"
                                        CssClass="textbox textbox1" Height="32px" Width="60px" OnClick="btn_exitaddgroup_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
        </center>
        <div id="alertdel" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="alertdelete" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <br />
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_del" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_del" Visible="false" CssClass="textbox textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btn_del_Click" Text="Ok" runat="server" />
                                        <asp:Button ID="btn_ok" Visible="false" CssClass="textbox textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btn_ok_Click" Text="Cancel" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </div>
    <center>
        <div id="imgdiv2" runat="server" visible="false" style="height: 50em; z-index: 1000;
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
</asp:Content>
