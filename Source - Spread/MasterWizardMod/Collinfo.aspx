<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWizardMod/MasterWizard.master"
    AutoEventWireup="true" CodeFile="Collinfo.aspx.cs" Inherits="Collinfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .maindivstylesize
        {
            height: 3000px;
            width: 1200px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
       </asp:ScriptManager>
      <script type="text/javascript">
    function frelig1() {
            document.getElementById('<%= btn_pls_pubpl .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_pubpl.ClientID%>').style.display = 'block';

        }
         </script>
 
    <center>
        <span class="fontstyleheader" style="color: Green;">College Information</span>
    </center>
    <br>
    <center>
       <%-- <asp:UpdatePanel ID='UpdGridStudent' runat="server">
            <ContentTemplate>--%>
                <div class="maindivstylesize">
               <%-- <asp:UpdatePanel ID="up1" runat="server"><ContentTemplate>--%>
                <center>
                    <table class="maintablestyle" style="top: 130px; margin-left: 28px; width: 708px;">
                    
                        <%-- <table>--%>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblcollege1" runat="server" Text="Select College" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="true" style="margin-left: -55px;"></asp:Label>
                           <%-- </td>
                            <td>--%>
                                <asp:DropDownList ID="ddlcollege1" runat="server" Width="486px" Font-Bold="true" style="margin-left: 22px;"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlcollege1_SelectedIndexChanged" AutoPostBack="true"><asp:ListItem>" "</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblCollName" runat="server" Text="College Name" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="true" style="margin-left: -3px;"></asp:Label>
                            <%--</td>
                            <td>--%>
                                <asp:TextBox ID="txtCollName" runat="server" Width="520px" CssClass="textbox txtheight1"
                                    Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 12px;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblUniver" runat="server" Text="University" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="true"></asp:Label>
                           <%-- </td>
                            <td>--%>
                                <asp:TextBox ID="txtUniver" runat="server" Width="520px" CssClass="textbox txtheight1"
                                    Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: 36px;"></asp:TextBox>
                            </td>
                        </tr>
                    </table></center><%--</ContentTemplate></asp:UpdatePanel>--%>
                    <br />
                    <div>
                        <asp:Panel ID="panel2" runat="server" CssClass="cpHeader" BackColor="#719DDB" Width="1100px">
                            <asp:Label ID="Label28" Text="Affiliation Details" runat="server" Font-Size="Large"
                                Font-Bold="true" Font-Names="Book Antiqua" />
                            <%--<asp:Image ID="image1" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                ImageAlign="Right" />--%>
                        </asp:Panel>
                    </div>
                   <%-- <asp:UpdatePanel ID="up2" runat="server"><ContentTemplate>--%>
                    <asp:Panel ID="Panel3" runat="server" Height="400px">
                        <fieldset style="top: 16px; height: auto; width: 1070px; border-color: Black; background-color: #eefbfb;
                            border-bottom-width: 2px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Total No.of.Affiliation" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="true"></asp:Label>
                                        <asp:TextBox ID="TextBox1" runat="server" Width="90px" CssClass="textbox txtheight1"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -2px;" AutoPostBack="true" OnTextChanged="TextBox1_OnTextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblColStYr" runat="server" Text="College Started Year" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="true"></asp:Label>
                                        <asp:TextBox ID="txtColStYr" runat="server" Width="90px" CssClass="textbox txtheight1" MaxLength="4"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; margin-left: -2px;"></asp:TextBox>
                                              <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtColStYr"
                                                        FilterType="numbers" ></asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView ID="gridAffiliation" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                            width: 950px;" Font-Names="Times New Roman" AutoGenerateColumns="false" BackColor="AliceBlue">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkSno" runat="server" Text="<%# Container.DisplayIndex+1 %>" OnClick="lnkAttMark11"
                                                            ForeColor="Black" Font-Underline="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Affiliated By">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAffiliation" runat="server" Text='<%# Eval("Affiliatedby") %>'
                                                            Style="text-align: left" Width="600px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Affiliated Year">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtaffYr" runat="server" Text='<%# Eval("AffiliatedYR") %>'
                                                            Style="text-align: center" Width="80px"></asp:TextBox>
                                                      
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                 
                                            </Columns>
                                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel><%--</ContentTemplate></asp:UpdatePanel>--%>
                   <%-- <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="panel3"
                        CollapseControlID="panel2" ExpandControlID="panel2" Collapsed="true" TextLabelID="Label28"
                        CollapsedSize="0" ImageControlID="imagecontactcollaps" CollapsedImage="../images/right.jpeg"
                        ExpandedImage="../images/down.jpeg">
                    </asp:CollapsiblePanelExtender>--%>
                   
                    <div>
                        <asp:Panel ID="panelcontactcollaps" runat="server" CssClass="cpHeader" BackColor="#719DDB" 
                            Width="1100px" style="margin-top: -237px; border-color:Black">
                            <asp:Label ID="lblcontactcollaps" Text="Contact" runat="server" Font-Size="Large"
                                Font-Bold="true" Font-Names="Book Antiqua" />
                           <%-- <asp:Image ID="imagecontactcollaps" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                                ImageAlign="Right" />--%>
                        </asp:Panel>
                    </div>
                    <asp:Panel ID="Panel5" runat="server" Height="400px">
                        <table border="1" style=" border-color:Black; border-collapse:collapse">
                            <tr>
                                <td>
                               <%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>--%>
                                    <asp:Panel ID="permantpanel" runat="server" Style="border-color: Gray; border-width: thin;">
                                        <table class="tabl" style="width: 450px; height: 300px; background-color: #eefbfb;margin-top: -3px;">
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblCategory" runat="server" Text="Category" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td class="style25">
                                                            <asp:RadioButtonList ID="rbcategory" runat="server"  Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="rbcategory_OnSelectedIndexedChanged" RepeatDirection="Horizontal" AutoPostBack="true" ><asp:ListItem>Autonomous</asp:ListItem>
                                                            <asp:ListItem>Affilated</asp:ListItem></asp:RadioButtonList>
                                                              
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblpaddress" runat="server" Text="Address" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txt_paddress" runat="server" Width="250px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="fte1" runat="server" TargetControlID="txt_paddress"
                                                        FilterType="Custom,Lowercaseletters,Uppercaseletters,Numbers" ValidChars="/,.() ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblStreet" runat="server" Text="Street" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtaddress2" runat="server" Width="250px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblCity" runat="server" Text="City" Font-Names="Book Antiqua" Style="margin-left: 20px;"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtaddress3" runat="server" Width="250px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblDistrict" runat="server" Text="District" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtaddress4" runat="server" Width="250px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblState" runat="server" Text="State" Font-Names="Book Antiqua" Style="margin-left: 20px;"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                <asp:Button ID="btn_pls_pubpl" runat="server" Font-Names="Book Antiqua" Font-Size="Large"
                                                    Height="31px" Style="height: 31px; display:none;  position: absolute;background-color: coral;
                                                    width: 37px;margin-left: -18px;" ForeColor="WhiteSmoke" OnClick="btn_pls_pubpl_Click" Font-Bold="true" Text="+" />
                                                <asp:DropDownList ID="ddlstate" runat="server" Style="width: 262px; height: 32px;margin-left: 20px;font-size: medium;"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlddlstate_SelectedIndexChanged" Font-Bold="true"
                                                    CssClass="textbox ddlstyle ddlheight3" Font-Names="Book Antiqua">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_min_pubpl" runat="server" Font-Names="Book Antiqua" Font-Size="Large"
                                                    Height="32px" ForeColor="WhiteSmoke" Style="height: 32px; display:none;  position: absolute;
                                                     width: 37px; margin-top: -32px; margin-left: 282px;background-color: coral;" OnClick="btn_min_pubpl_Click" Text="-" Font-Bold="true" />
                                                     </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblPinCode" runat="server" Text="Pincode" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtpincode" runat="server" Width="250px" MaxLength="6" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtpincode"
                                                        FilterType="Numbers" >
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblPhone" runat="server" Text="Phone No" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtphone" runat="server" Width="250px" MaxLength="30" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtphone"
                                                        FilterType="Custom,numbers" ValidChars=","></asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblFax" runat="server" Text="Fax" Font-Names="Book Antiqua" Style="margin-left: 20px;"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtfaxno" runat="server" Width="250px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtfaxno"
                                                        FilterType="Custom,numbers" ValidChars=","></asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblEmail" runat="server" Text="E-Mail" Font-Names="Book Antiqua" Style="margin-left: 20px;"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtemail" runat="server" Width="250px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblWebSite" runat="server" Text="Web Site" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtwebsite" runat="server" Width="250px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel><%--</ContentTemplate></asp:UpdatePanel>--%>
                                </td>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" Style="border-color: Gray; border-width: thin;">
                                        <table class="tabl" style="width: 650px; height: 400px; margin-left: -5px; margin-top: -5px; background-color: #eefbfb;">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblAcr" runat="server" Text="Acronym" Font-Names="Book Antiqua" Style="margin-left: 20px;"
                                                        Font-Size="Medium"></asp:Label>
                                                    <asp:TextBox ID="txtacronym" runat="server" Width="70px" MaxLength="10" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;text-transform:uppercase;"></asp:TextBox>
                                                    <asp:Label ID="lblColCode" runat="server" Text="College Code" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                    <asp:TextBox ID="txtclgcode" runat="server" Width="70px" MaxLength="10" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px; text-transform:uppercase;"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtclgcode"
                                                            FilterType="numbers,custom,UppercaseLetters" ValidChars=".-/[]{},<>/?"></asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblChancellor" runat="server" Text="Chancellor" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtchancellor" runat="server" Width="200px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="btnAdd" runat="server"  Height="27px" ImageUrl="~/college/Ques Icon.png" OnClick="btnadd_OnClick" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblViceChancellor" runat="server" Text="Vice-Chancellor" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtvicechancellor" runat="server" Width="200px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                     <asp:ImageButton ID="btnvicechan" runat="server"  Height="27px" ImageUrl="~/college/Ques Icon.png" OnClick="btnvicechan_OnClick" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblChairman" runat="server" Text="Correspondent/Chairman" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtcorres" runat="server" Width="200px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="btnchairman" runat="server"  Height="27px" ImageUrl="~/college/Ques Icon.png" OnClick="btnchairman_OnClick" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblSecretary" runat="server" Text="Secretary" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtsecretary" runat="server" Width="200px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                  <asp:ImageButton ID="btnsecretary" runat="server"  Height="27px" ImageUrl="~/college/Ques Icon.png" OnClick="btnsecretary_OnClick" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblPrincipal" runat="server" Text="Principal/Registrar" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtprincipal" runat="server" Width="200px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="btnprincipal" runat="server"  Height="27px" ImageUrl="~/college/Ques Icon.png" OnClick
                                                    ="btnprincipal_OnClick" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lbladmin" runat="server" Text="Vice-principal/Cheif     Administrator Officeer"
                                                        Font-Names="Book Antiqua" Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtviceprincipal" runat="server" Width="200px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="btnviceprin" runat="server"  Height="27px" ImageUrl="~/college/Ques Icon.png" OnClick="btnviceprin_OnClick" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style5">
                                                    <asp:Label ID="lblEstNo" runat="server" Text="Establishment No" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                <asp:TextBox ID="txtest1" runat="server" Width="30px" style="margin-left: 19px;" MaxLength="2" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Font-Bold="true" Font-Size="Medium"></asp:TextBox>
                                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtest1"
                                FilterType="UppercaseLetters,Numbers,LowercaseLetters" />
                                                        <asp:Label ID="lblest1" runat="server" Text="-" Font-Bold="true"></asp:Label>
                                                         <asp:TextBox ID="txtest2" runat="server" Width="43px" MaxLength="3" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Font-Bold="true" Font-Size="Medium"></asp:TextBox>
                                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtest2"
                                FilterType="UppercaseLetters,Numbers,LowercaseLetters" />
                                                        <asp:Label ID="Label8" runat="server" Text="-" Font-Bold="true"></asp:Label>
                                                        <asp:TextBox ID="txtest3" runat="server" Width="80px" MaxLength="10" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Font-Bold="true" Font-Size="Medium"></asp:TextBox>
                                                          <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtest3"
                                FilterType="UppercaseLetters,Numbers,LowercaseLetters" />
                                                </td>
                                               <%-- <td class="style25">
                                                    <asp:TextBox ID="txtestablishmentno" runat="server" Width="200px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCOE" runat="server" Text="Name of Controller of Examimation" Font-Names="Book Antiqua"
                                                        Style="margin-left: 20px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtcoe" runat="server" Width="200px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtcoe"
                                FilterType="UppercaseLetters,LowercaseLetters" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                            <td colspan="2px">
                             <asp:Panel ID="Panel4" runat="server" Style="border-color: Gray; border-width: thin;">
                                        <table class="tblcolnam" style="width: 1100px; height: 68px; background-color: #eefbfb; margin-top: -6px">
                                        <tr>
                                            <td colspan="4px" align="center">
                                             <asp:Label ID="Label5" runat="server" Text="Mass Id For Mass Mailing" Font-Names="Book Antiqua"
                                                        Font-Bold="true" Font-Size="Large" style="margin-left: -33px;"></asp:Label>
                                            </td>
                                            </tr>
                                             <tr>

                                <td colspan="4px" >
                                <asp:Label ID="lblmassmail" runat="server" Text="E-Mail"  Font-Names="Book Antiqua" Font-Size="Medium" style="margin-left: 387px;" ></asp:Label></td><td>
                                <asp:TextBox ID="txtmassmail" runat="server" Font-Bold="true" CssClass="textbox txtheight2" style="margin-left: -589px;; width:249px"></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                <td colspan="4px" >
                                <asp:Label ID="lblmassmailpwd" runat="server" Text="Password" Font-Names="Book Antiqua" style="margin-left: 385px;" Font-Size="Medium" ></asp:Label></td><td>
                                <asp:TextBox ID="txtmassmailpwd" runat="server"  Font-Bold="true"  CssClass="textbox txtheight2" style="margin-left: -589px; width:249px" TextMode="Password"></asp:TextBox>
                                </td>
                                </tr>
                                          <tr>
                                                <td class="style5" colspan="2px">
                                                    <asp:Label ID="lblcommonclgname" runat="server" Text="Common College Name For Multicollege" Font-Names="Book Antiqua"
                                                        Style="margin-left: 166px;" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style25">
                                                    <asp:TextBox ID="txtcommonclgname" runat="server" Width="450px" MaxLength="100" Font-Names="Book Antiqua"
                                                        CssClass="textbox txtheight1" Style="font-weight: bold; font-family: book antiqua;
                                                        font-size: medium; margin-left: -139px;" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                            
                                             <tr>
                    <td colspan="4px" align="center">
                    <asp:LinkButton ID="lnkbtsignature" runat="server" OnClick="lnkbtsignature_OnClick" Text="Signature Settings" style="margin-top:10px"  ForeColor="Blue"></asp:LinkButton>
                    
                   </td></tr><tr><td colspan="4px" align="center">
                    <asp:Button ID="btnlogo" runat="server" Text="Logo" Width="72px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Style="color: Black; position: relative;" 
                                        OnClick="btnlogo_OnClick"  />
                                      
                                        <asp:Button ID="Btnsave" runat="server" Text="Save" Width="70px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Style="color: Black; position: relative; "
                                        OnClick="Btnsave_OnClick" />
                                         <asp:Button ID="btndelete" runat="server" Text="Delete" Width="85px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Style="color: Black; position: relative;"
                                        OnClick="btndelete_OnClick" />
                    </td>
                    </tr>

                                        </table>

                                        </asp:Panel>
                            </td>
                            </tr>
                        </table>
                    </asp:Panel>
                   <%-- <center>
                    <table style="margin-top:5px">
                   
                    </table>
                    </center>--%>
                    <%--<asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender14" runat="server" TargetControlID="Panel5"
                        CollapseControlID="panelcontactcollaps" ExpandControlID="panelcontactcollaps"
                        Collapsed="true" TextLabelID="lblcontactcollaps" CollapsedSize="0" ImageControlID="imagecontactcollaps"
                        CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
                    </asp:CollapsiblePanelExtender>--%>
                    <br />
                </div>
           <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>

   <%-- <asp:UpdatePanel ID="upphoto" runat="server"><ContentTemplate>--%>
      <center>
                <asp:Panel ID="panelphoto" runat="server" BorderColor="Black" BackColor="oldlace"
                    Visible="false" BorderWidth="2px" Style="left: 247px; top: 279px; position: absolute;"
                    Height="314px" Width="911px">
                    <div class="PopupHeaderrstud2" id="Div14" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            <asp:Label ID="lblcaption" runat="server" Text="Institution Logo" Font-Bold="True"
                                ForeColor="Green" style="font-size:21px" Font-Names="Book Antiqua"></asp:Label>
                        </caption>
                    </div>
                    <br />
                    <br />
                     <fieldset style="left: 25px; top: 54px; width: 232px; height: 130px; position: absolute;">
                        <asp:Label ID="Label3" runat="server" Text="Left Logo" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 96px;
                            top: 0px;"></asp:Label>
                        <asp:Image ID="imgstudp" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="fulstudp" runat="server" Style="position: absolute; left: 5px;
                            top: 130px;" />
                            <asp:Button ID="Btnsaveleftlogo" runat="server" Text="Save" Width="80px" Font-Bold="true"
                            OnClick="Btnsaveleftlogo_Click" Style="position: absolute; left: 175px; top: 100px;" />
                            

                             <asp:Button ID="btnrmv_leftlogo" runat="server" Text="Remove" Width="75px" Font-Bold="true"
                            OnClick="btnrmvleftlogo_Click" Style="position: absolute; left: 178px; top: 130px;" />
                            
                       
                    </fieldset>
                    
                   
                    <fieldset style="width: 232px; height: 130px; position: absolute; left: 310px; top: 54px;">
                        <asp:Label ID="Label4" runat="server" Text="Right Logo" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 93px;
                            top: 0px;"></asp:Label>
                        <asp:Image ID="imgmotp" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="fulmp" runat="server" onchange="callme(this)" Style="position: absolute;
                            left: 5px; top: 130px;" />

                             <asp:Button ID="btnrightlogo" runat="server" Text="Save" Width="80px" Font-Bold="true"
                            OnClick="btnrightlogo_Click" Style="position: absolute; left: 175px; top: 100px;" />
                              <asp:Button ID="btnrmv_rightlogo" runat="server" Text="Remove" Width="75px" Font-Bold="true"
                            OnClick="btnrmvrightlogo_Click" Style="position: absolute; left: 178px; top: 130px;" />
                      
                    </fieldset>
                      <fieldset style="left: 596px; top: 54px; width: 232px; height: 130px; position: absolute;">
                        <asp:Label ID="Label2" runat="server" Text="College Photo" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 83px;
                            top: 0px;"></asp:Label>
                        <asp:Image ID="imgfatp" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="fulfatp" runat="server"  Style="position: absolute;
                            left: 5px; top: 130px;" />

                            <asp:Button ID="Btnsavclgphoto" runat="server" Text="Save" Width="80px" Font-Bold="true"
                            OnClick="Btnsavclgphoto_Click" Style="position: absolute; left: 175px; top: 100px;" />
                             <asp:Button ID="btnrmv_clgphoto" runat="server" Text="Remove" Width="75px" Font-Bold="true"
                            OnClick="btnrmvclgphoto_Click" Style="position: absolute; left: 178px; top: 130px;" />
                       
                    </fieldset>


                    <asp:Label ID="lblphotoerr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Style="position: absolute; left: 5px; top: 380px;"></asp:Label>
                    <fieldset style="width: 150px; height: 12px; position: absolute; left: 369px; top: 249px;">
                        
                        <asp:Button ID="btnstuph" runat="server" Text="Ok" Width="75px" Font-Bold="true"
                            OnClick="btnstuph_Click" Style="position: absolute; left: 6px; top: 2px;" />
                        <asp:Button ID="Button2" runat="server" Text="Exit" Width="75px" Font-Bold="true"
                            OnClick="btnexit_Click" Style="position: absolute; left: 96px; top: 2px;" />
                    </fieldset>
                   

                </asp:Panel>
            </center><%--</ContentTemplate></asp:UpdatePanel>--%>


                  <%-- Stafflookup--%>
                  <%-- <asp:UpdatePanel ID="upphoto" runat="server"><ContentTemplate>--%>
                              <center>
           <asp:Panel ID="panel6" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                    BorderWidth="2px" Style="left: 30%; top: 23%; right: 30%; position: absolute;
                    " Height="530px" Width="523px">
                     <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -8px; margin-left: 237px;"
                        OnClick="imagebtnpopclose5_Click" />
                    <div class="PopupHeaderrstud2" id="Div16" style="text-align: center; font-family: Book Antiqua;
                        font-size: Small; font-weight: bold;">
                        <br />
                       <center>
                                <asp:Label ID="lblselstaf" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                                    position: relative;" Text="Select Staff" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
                            </center>
                        <br />
                        <br />
                        <table>
                           <tr>
                            <td>
                                    <asp:Label ID="lblstafftype" runat="server" Text="Staff Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstafftype" runat="server" Width="150px" OnSelectedIndexChanged="ddlstafftype_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                              
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsearchby" runat="server" Text="Staff By"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                        <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                        AutoPostBack="True"></asp:TextBox>
                                </td>
                                </tr>
                           
                        </table>
                       <br />
                        <asp:Label ID="Labelstaffalert" runat="server" Text="No Record Found!" Font-Bold="true" ForeColor="Red"></asp:Label>
                        <br />
                       
                                <div id="divstaff" runat="server" style="overflow: auto; border: 1px solid Gray; width: 480px;
                                    height: 280px; margin-left:17px">
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                    <asp:GridView runat="server" ID="gviewstaff" AutoGenerateColumns="false" Style="height: 300;
                                        width: 460px; overflow: auto;" OnRowCreated="OnRowCreated" OnSelectedIndexChanged="SelectedIndexChanged">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%#Container.DisplayIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblname" runat="server" Text='<%#Eval("Staff_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Designation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldesig" runat="server" Text='<%#Eval("designation") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltype" runat="server" Text='<%#Eval("type") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                        <RowStyle ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                    </asp:GridView>
                                    
                                  
                                </div>
                            <br />
                                    <center>
                    <div id="staffok" runat="server" visible="false">
                        <asp:Button ID="btn_staffok" runat="server"  CssClass="textbox btn2" Text="Ok" OnClick="btn_staffok_Click"/>
                           
                        <asp:Button ID="btn_staffexit" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_staffexit_Click" />
                    </div>
                </center>
                    </div>

                </asp:Panel>
                </center>
               <%-- </ContentTemplate></asp:UpdatePanel>--%>


                  <center>
       <%-- <asp:UpdatePanel ID="UpdatePanel35" runat="server">--%>
            <ContentTemplate>
                <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                    <center>
                        <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                            height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <table style="line-height: 30px">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox txtheight2"
                                            onkeypress="display1()"></asp:TextBox>
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="line-height: 35px">

                                    <asp:Button ID="btn_addgroup1" runat="server" Font-Bold="true" OnClick="btn_addgroup_Click" Text="Add" />
                                      <asp:Button ID="btn_exitgroup1" runat="server" Font-Bold="true" OnClick="btn_exitaddgroup_Click" Text="Exit" />
                                      
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
           <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>

    <center>
                <asp:Panel ID="panel7" runat="server" BorderColor="Black" BackColor="oldlace"
                    Visible="false" BorderWidth="2px" Style="left: 372px; top: 279px; position: absolute;"
                    Height="314px" Width="614px">
                    <div class="PopupHeaderrstud2" id="divsignature" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            <asp:Label ID="Label6" runat="server" Text="Signature Settings" Font-Bold="True"
                                ForeColor="Green" style="font-size:21px" Font-Names="Book Antiqua"></asp:Label>
                        </caption>
                    </div>
                    <br />
                    <br />
                     <fieldset style="left: 25px; top: 54px; width: 232px; height: 130px; position: absolute;">
                        <asp:Label ID="lblcoesignature" runat="server" Text="COE Signature " Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 96px;
                            top: 0px;"></asp:Label>
                        <asp:Image ID="imgcoesign" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="flupcoe" runat="server" Style="position: absolute; left: 5px;
                            top: 130px;" />
                            <asp:Button ID="btncoesign" runat="server" Text="Save" Width="80px" Font-Bold="true"
                            OnClick="btncoesign_Click" Style="position: absolute; left: 175px; top: 100px;" />
                            

                             <asp:Button ID="btncoeremove" runat="server" Text="Remove" Width="75px" Font-Bold="true"
                            OnClick="btncoeremove_Click" Style="position: absolute; left: 178px; top: 130px;" />
                            
                       
                    </fieldset>
                    
                   
                    <fieldset style="width: 232px; height: 130px; position: absolute; left: 310px; top: 54px;">
                        <asp:Label ID="lblprincisign" runat="server" Text="Principal Signature" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 93px;
                            top: 0px;"></asp:Label>
                        <asp:Image ID="imgprincisign" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="fupprincisign" runat="server" onchange="callme(this)" Style="position: absolute;
                            left: 5px; top: 130px;" />

                             <asp:Button ID="btnprincisign_save" runat="server" Text="Save" Width="80px" Font-Bold="true"
                            OnClick="btnprincisign_save_Click" Style="position: absolute; left: 175px; top: 100px;" />
                              <asp:Button ID="btnprinciremove" runat="server" Text="Remove" Width="75px" Font-Bold="true"
                            OnClick="btnprinciremove_Click" Style="position: absolute; left: 178px; top: 130px;" />
                      
                    </fieldset>
                     

                  
                    <fieldset style="width: 150px; height: 12px; position: absolute; left: 219px; top: 249px;">
                        
                        <asp:Button ID="btnsignsave" runat="server" Text="Ok" Width="75px" Font-Bold="true"
                            OnClick="btnsignsave_Click" Style="position: absolute; left: 6px; top: 2px;" />
                        <asp:Button ID="btnsignexit" runat="server" Text="Exit" Width="75px" Font-Bold="true"
                            OnClick="btnsignexit_Click" Style="position: absolute; left: 96px; top: 2px;" />
                    </fieldset>
                   

                </asp:Panel>
            </center>

             <center>
       <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>--%>
                <div id="div4" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div5" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label7" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                       
                                            <asp:Button ID="btnaltok" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnaltok_Click"
                                                Text="Ok" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
           <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>


</asp:Content>
