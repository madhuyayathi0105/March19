<%@ Page Language="C#" AutoEventWireup="true" CodeFile="weightagesettings.aspx.cs"
    Inherits="MarkMod_weightagesettings" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">



        function validate1(e) {

                    var row = e.parentNode.parentNode;
                    var rowIndex = row.rowIndex - 1;
                    // alert(rowIndex);
                    var ddlco = document.getElementById('MainContent_GridView1_ddlconame_' + rowIndex);
                    var ddlsub = document.getElementById('MainContent_GridView1_ddlsubject_' + rowIndex);
                    var tst = document.getElementById('MainContent_GridView1_cbltest1_' + rowIndex);
                  var comtxt
                    var txt = "";
                 
                    for (var i = 0; i < tst.childNodes.length ; i++) {
                        var tstx = document.getElementById('MainContent_GridView1_cbltest1_' + rowIndex + '_' + i + '_' + rowIndex);
                       
                        if (tstx.checked) {
                           
                             var chk = document.getElementById('MainContent_GridView1_cbltest1_' + rowIndex + '_' + i + '_' + rowIndex).value.toString();
                             if (txt == "")
                                 txt = chk;
                             else
                                 txt = txt + ',' + chk;
                            
                        }
                                    
                    }
                    
                    var strUser = ddlco.options[ddlco.selectedIndex].text;
                    var strUser2 = ddlsub.options[ddlsub.selectedIndex].text;
                  
                    var grid = document.getElementById('<%=GridView1.ClientID%>');
                    for (var i = 0; i < grid.rows.length - 1; i++) {
                        if (i != rowIndex) {
                            var ddlco1 = document.getElementById('MainContent_GridView1_ddlconame_' + i);
                            var ddlsub1 = document.getElementById('MainContent_GridView1_ddlsubject_' + i);
                            var tst1 = document.getElementById('MainContent_GridView1_cbltest1_' + i);
                            var strUser1 = ddlco1.options[ddlco1.selectedIndex].text;
                            var strUser21 = ddlsub1.options[ddlsub1.selectedIndex].text;
                            var txt1 = "";
                            for (var m = 0; m < tst1.childNodes.length; m++) {
                                var tstx = document.getElementById('MainContent_GridView1_cbltest1_' + i + '_' + m + '_' + i);
                                if (tstx.checked) {
                                    var chk1 = document.getElementById('MainContent_GridView1_cbltest1_' + i + '_' + m + '_' + i).value.toString();
                                    if (txt1 == "")
                                        txt1 = chk1;
                                    else
                                        txt1 = txt1 + ',' + chk1;
                                }
                            }
                            var txet = document.getElementById('MainContent_GridView1_txttest1_' + i);
                            var ash = txet.value;
                            if (strUser1 != "--Select--" || strUser21 != "--Select--" || ash != "--Select--") {
                                if (strUser1 == strUser) {
                                    if (strUser2 == strUser21) {
                                        var df = txt.split(",");
                                        var df1 = txt1.split(",");
                                        for (var ms = 0; ms < df1.length; ms++) {
                                            for (var j = 0; j < df.length; j++) {
                                                var dt = df[j];
                                                var dt1 = df1[ms];
                                                if (dt == dt1) {
                                                    alert("Select Different Values");
                                                }
                                            }
                                        }


                                    }

                                }
                            }



                                }


                    }
        }

        function validate(e) {
            var value1 = e.value;
            var max = parseInt(value1);
            var row = e.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var ddlco = document.getElementById('MainContent_GridView1_ddlconame_' + rowIndex);
            var ddlsub = document.getElementById('MainContent_GridView1_ddlsubject_' + rowIndex);
            var struser = ddlco.options[ddlco.selectedIndex].text;
            var struser2 = ddlsub.options[ddlsub.selectedIndex].text;
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            var cd = 0;
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddlco1 = document.getElementById('MainContent_GridView1_ddlconame_' + i);
                var ddlsub1 = document.getElementById('MainContent_GridView1_ddlsubject_' + i);
                var txtval = document.getElementById('MainContent_GridView1_txtweightper_' + i);
                var valt = txtval.value;
                var max1 = parseInt(valt);
                var strUser1 = ddlco1.options[ddlco1.selectedIndex].text;
                var strUser21 = ddlsub1.options[ddlsub1.selectedIndex].text;
                if (struser == strUser1 && struser2 == strUser21) {
                    var val2 = max1;
                    cd = parseInt(cd) +  parseInt(val2);
                    if (cd > 100) {
                        e.value = "";
                        cd = 0;
                        alert("Weightage Percentage Should Be Lesser Than Or Equal To 100");
                        
                    }
                }

            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green;">Weightage Settings</span>
    </center>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <center>
                    <table class="maintablestyle" style="margin-left: 0px; height: 73px; width: 1017px;
                        margin-bottom: 0px;">
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 18px; width: 44px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="250px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Height="25px" Width="69px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldegree" runat="server" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Height="25px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Width="250px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                                        AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        Height="25px" Width="41px">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <div>
        <%-- <asp:UpdatePanel ID="up" runat="server"><ContentTemplate>--%>
        <center>
            <table>
                <tr>
                    <td align="left">
                        <asp:UpdatePanel ID="upadd" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnaddnew" runat="server" Text="Add New Row" OnClick="btnaddnewrow_OnClick"
                                    Visible="false" Font-Bold="true" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:UpdatePanel ID="upgd" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                <asp:GridView ID="GridView1" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                    width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false" OnRowDataBound="gridview1_OnRowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="CO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblconame" runat="server" Text='<%# Eval("CO") %>' Visible="false"></asp:Label>
                                                <asp:DropDownList ID="ddlconame" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="55px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Subject">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsubject" runat="server" Text='<%# Eval("Subject") %>' Visible="false"></asp:Label>
                                                <asp:DropDownList ID="ddlsubject" runat="server" OnSelectedIndexChanged="ddlsubject_OnSelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Test">
                                            <ItemTemplate>
                                                <div style="position: relative;">
                                                    <div id="div5" style="position: relative;" runat="server">
                                                        <asp:UpdatePanel ID="upnlPeriod1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txttest1" Visible="true" Width="150px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                                    ReadOnly="true">-- Select --</asp:TextBox>
                                                                <asp:Panel ID="pnlPeriod1" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                                    Width="305px">
                                                                    <asp:CheckBox ID="chktest1" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                        AutoPostBack="True" OnCheckedChanged="chktest_CheckedChanged" />
                                                                    <asp:CheckBoxList ID="cbltest1" CssClass="commonHeaderFont" runat="server" OnSelectedIndexChanged="cbltest1_OnSelectedIndexChanged"
                                                                        AutoPostBack="true">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popExtPreiod1" runat="server" TargetControlID="txttest1"
                                                                    PopupControlID="pnlPeriod1" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <asp:Label ID="lbltest" runat="server" Text='<%# Eval("Test") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Criteria Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcriterianame" runat="server" Text='<%# Eval("CriteriaName") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:TextBox ID="txtcriterianame" runat="server" Text='<%# Eval("CriteriaName") %>'
                                                    Style="text-align: center" onkeyup="return validate1(this)"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Weightage Percentage">
                                            <ItemTemplate>
                                                <asp:Label ID="lblweightper" runat="server" Text='<%# Eval("WeightagePercentage") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:TextBox ID="txtweightper" runat="server" Text='<%# Eval("WeightagePercentage") %>'
                                                    onchange="return validate(this)" Style="text-align: center"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtweightper"
                                                    FilterType="Numbers" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:UpdatePanel ID="upsave" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnsave" runat="server" OnClick="btnsave_OnClick" Text="Save" Visible="false"
                                    Font-Bold="true" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </center>
        <%--</ContentTemplate></asp:UpdatePanel>--%>
    </div>
    <center>
        <asp:UpdatePanel ID="upok" runat="server">
            <ContentTemplate>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel_go">
            <ProgressTemplate>
                <div style="height: 40px; width: 150px;">
                    <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                    <br />
                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                        Processing Please Wait...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upadd">
            <ProgressTemplate>
                <div style="height: 40px; width: 150px;">
                    <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                    <br />
                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                        Processing Please Wait...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upsave">
            <ProgressTemplate>
                <div style="height: 40px; width: 150px;">
                    <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                    <br />
                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                        Processing Please Wait...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>
    <%--<center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upsave">
            <ProgressTemplate>
                <div style="height: 40px; width: 150px;">
                    <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                    <br />
                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                        Processing Please Wait...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
    </center>--%>
</asp:Content>
