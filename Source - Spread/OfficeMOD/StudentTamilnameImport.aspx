<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentTamilnameImport.aspx.cs" Inherits="OfficeMOD_StudentTamilnameImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function checkLength(startyear) {
            var startyear = document.getElementById('<%=txt_startyear.ClientID%>');
            if (startyear.value.length != 4) {
                startyear.style.borderColor = 'Red';
                alert("Enter Valid Year")
            }
        }
        function checkLength1(endyear) {
            var startyear = document.getElementById('<%=txt_startyear.ClientID%>');
            var endyear = document.getElementById('<%=txt_endyear.ClientID%>');
            if (endyear.value.length != 4) {
                endyear.style.borderColor = 'Red';
                alert("Enter Valid Year")
            }
        }
        function myFunction(x) {
            x.style.borderColor = "#c4c4c4";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <center>
                <div style="width: 970px;" class="maindivstyle">
                    <br />
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <span>Institution Name</span>
                                    <asp:DropDownList ID="ddl_collegename" runat="server" Width="205px" CssClass="textbox1 ddlstyle ddlheight6">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblstart" runat="server" Text="Start Year "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_startyear" placeholder="YYYY" runat="server" Width="57px" CssClass="textbox textbox1 txtheight1"
                                        MaxLength="4" onblur="checkLength(this)" onfocus="return myFunction(this)">
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_startyear"
                                        FilterType="Numbers" ValidChars="">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_endyear" runat="server" Text="End Year "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_endyear" runat="server" placeholder="YYYY" Width="57px" CssClass="textbox textbox1 txtheight1"
                                        MaxLength="4" onblur="checkLength1(this)" onfocus="return myFunction(this)">
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_endyear"
                                        FilterType="Numbers" ValidChars="">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" Text="Go" runat="server" OnClick="btn_go_Click" CssClass="textbox textbox1 btn1" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <asp:Label ID="lbl_error" Visible="false" ForeColor="red" runat="server"></asp:Label>
                        </center>
                        <center>
                            <asp:Label ID="lbl_degree" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lbl_branch" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lbl_sem" runat="server" Visible="false"></asp:Label>
                            <asp:GridView ID="importgrid" runat="server" AutoGenerateColumns="false" Width="800px"
                                HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" OnDataBound="importgrid_DataBound"
                                OnRowCommand="importgrid_RowCommand" OnRowDataBound="importgrid_RowDataBound"
                                BackColor="white" CssClass="spreadborder ">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="85px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Batch Year">
                                        <ItemTemplate>
                                            <asp:Label ID="lblbtch" runat="server" Text='<%#Eval("BatchYear") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="85px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edu Level">
                                        <ItemTemplate>
                                            <asp:Label ID="lbedulv" runat="server" Text='<%#Eval("EduLevel") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="85px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Degree">
                                        <ItemTemplate>
                                            <asp:Label ID="lbfdeg" runat="server" Text='<%#Eval("Degree") %>'></asp:Label>
                                            <asp:Label ID="lbl_degreecode" runat="server" Visible="false" Text='<%#Eval("degreecode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="85px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department">
                                        <ItemTemplate>
                                            <asp:Label ID="lbdpt" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="85px" />
                                    </asp:TemplateField>
                                    <%--  <asp:TemplateField HeaderText="Semester">
                                        <ItemTemplate>
                                            <asp:Label ID="lbsem" runat="server" Text='<%#Eval("Semester") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="85px" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Download">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_download" runat="server" Text=""></asp:Label>
                                            <asp:Image ID="d1" runat="server" ImageUrl="~/image/d3.png" Width="35px" Height="35px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="85px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Upload">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_upload" runat="server" Text="" ForeColor="Blue" Font-Underline="True"
                                                Font-Size="Small"></asp:Label>
                                            <asp:Image ID="d2" runat="server" ImageUrl="~/image/d4.png" Width="35px" Height="35px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="85px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Help">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_help" runat="server" Text="" ForeColor="Blue" Font-Underline="True"
                                                Font-Size="Small"></asp:Label>
                                            <asp:Image ID="help" runat="server" ImageUrl="~/image/h.png" Height="25px" Width="25px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="85px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                        </center>
                    </center>
                </div>
            </center>
            <br />
            <center>
                <div id="Browsefile_div" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; height: 100em; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0; left: 0px;">
                    <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 187px; margin-left: 210px;"
                        OnClick="imagebtnpopclose_Click" />
                    <center>
                        <div id="div1" runat="server" class="table" style="background-color: White; height: 160px;
                            width: 450px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <center>
                                    <asp:Label ID="lbl_header" runat="server" Text="Student Tamil Name Import" ForeColor="Green"
                                        Visible="true" Font-Size="Larger"></asp:Label>
                                </center>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblfile_upload" runat="server" Text="Browse File"></asp:Label>
                                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="textbox textbox1" ForeColor="Green"
                                                Width="185px" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btn_errorclose1" Style="height: 36px; width: 36px;" OnClick="btn_upload_click"
                                                ImageUrl="~/image/okimg.jpg" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btn_download" Visible="false" CssClass="textbox btn2" runat="server"
                                                Text="Download" OnClick="btn_download_click" />
                                            <asp:CheckBox ID="cb_importcomplete" ForeColor="Green" Visible="false" runat="server"
                                                Text="Is Import Completed" />
                                            <asp:Label ID="lbl_alert" runat="server" Text="Please Browse Upload File" ForeColor="red"
                                                Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="cannot_insert_div" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px; height: 100em;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 61px; margin-left: 405px;"
                        OnClick="btn_Exit_Click1" />
                    <center>
                        <div id="cannot_insert" runat="server" style="background-color: White; height: 400px;
                            width: 840px; border: 5px solid #0CA6CA; border-top: 5px solid #0CA6CA; margin-top: 72px;
                            border-radius: 10px;">
                            <asp:Label ID="lbl_upload_suc" runat="server" Visible="false" ForeColor="Blue"></asp:Label>
                            <br />
                            <asp:Label ID="lbl_cannotsave" Visible="false" runat="server" Style="color: Red;"
                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                            <div style="height: 345px; width: 700px; overflow: auto;">
                                <asp:TextBox ID="lbl_cannotinsert" TextMode="MultiLine" runat="server" Style="height: 334px;
                                    overflow: auto;" Visible="false" Width="650px" ForeColor="Blue" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="alertmessage" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px; height: 100em;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alerterror" Visible="true" runat="server" Text="" Style="color: Red;"
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
        </div>
    </body>
</asp:Content>
