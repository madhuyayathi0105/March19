<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Transport_Master.aspx.cs" Inherits="Transport_Master" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%--<%@ Register TagPrefix="Ajaxified" Assembly="Ajaxified" Namespace="Ajaxified" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

     <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
    <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
    <%--    <meta http-equiv="refresh" content="20"> 
    --%>
    <div style="top: 70px; position: absolute;">
        <div>
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="position: absolute;
                width: 1015px; height: 21px; margin-bottom: 0px; top: 8px; left: 10px;">
                <%-- style="top: 71px; left: 0px; position: absolute; width: 960px"--%>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="Transport Monitor" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="White"></asp:Label>&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <%--  
                &nbsp;&nbsp;<asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Back</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="lb1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Home</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="lb2" runat="server" OnClick="lb2_Click" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Small" ForeColor="White" CausesValidation="False">Logout</asp:LinkButton>
                <%--<asp:Label ID="lbltitle" runat="server" Font-Names="Book Antiqua" 
                    Font-Size="Large" ForeColor="White"></asp:Label>--%>
            </asp:Panel>
        </div>
        <div style="position: absolute; width: 1000px; left: 10px; background-color: lightcyan;
            top: 29px; height: 97px;">
            <table>
                <tr>
                    <td style="top: 13px; position: absolute; left: 165px; width: 77px;">
                        <asp:Label ID="lbl_vech" runat="server" Text="Vehicle Id" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td style="width: 122px; top: 13px; position: absolute; left: 244px;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_vech" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="120px" Height="15px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    OnTextChanged="txt_vech_TextChanged">---Select---</asp:TextBox>
                                <asp:Panel ID="pnl_vech" runat="server" CssClass="multxtpanel" Style="width: 123px;"
                                            Height="250px">
                                    <asp:CheckBox ID="chk_vech" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="chk_vech_ChekedChange" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chkls_vech" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chkls_vech_SelectedIndexChanged" Width="100px" Height="200px"
                                        Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_vech"
                                    PopupControlID="pnl_vech" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="top: 13px; position: absolute; width: 46px; left: 409px;">
                        <asp:Label ID="lbl_route" runat="server" Text="Route" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td style="top: 13px; position: absolute; width: 121px; left: 458px;">
                        <asp:UpdatePanel ID="Updatepanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_route" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="120px" Font-Bold="True" Height="15px" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pnl_route" runat="server" CssClass="multxtpanel" Style="width: 123px;"
                                            Height="250px">
                                    <asp:CheckBox ID="Chk_route" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="Chk_route_ChekedChange" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="Chkls_route" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="Chkls_route_SelectedIndexChanged" Width="100px" Font-Bold="True"
                                        Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_route"
                                    PopupControlID="pnl_route" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="top: 13px; position: absolute; left: 619px;">
                        <asp:Label ID="lbl_stage" runat="server" Text="Stage" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td style="width: 120px; top: 13px; position: absolute; left: 662px;">
                        <asp:UpdatePanel ID="Updatepanel6" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_stage" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="120px" Height="15px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pnl_stage" runat="server" CssClass="multxtpanel" Style="width: 160px;"
                                            Height="250px">
                                    <asp:CheckBox ID="Chk_stage" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="Chk_stage_ChekedChange" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="Chkls_stage" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="Chkls_stage_SelectedIndexChanged" Width="190px" Height="200px"
                                        Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_stage"
                                    PopupControlID="pnl_stage" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 57px; top: 13px; position: absolute;">
                        <asp:Label ID="Label1" runat="server" Text="Session" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td style="position: absolute; top: 13px; width: 85px; left: 61px;">
                        <asp:DropDownList ID="ddl_session" runat="server" OnSelectedIndexChanged="ddl_session_SelectedIndexChanged">
                            <asp:ListItem>Morning</asp:ListItem>
                            <asp:ListItem>Afternoon</asp:ListItem>
                            <asp:ListItem>Both</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table>
                <tr>
                    <td style="width: 35px; position: absolute; top: 81px; left:10px;">
                        <asp:Label ID="lbl_fromdate" runat="server" Text="Date" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td style="position: absolute; top: 81px; left: 47px;">
                        <asp:TextBox ID="txt_date" runat="server" Width="105px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                            width: 105px; height: 15px;">
                        </asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" Format="d/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="ft1" runat="server" TargetControlID="txt_date" FilterType="Custom,Numbers"
                            ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="position: absolute; top: 81px; width: 41px; left: 180px;">
                        <asp:Label ID="lbl_view" runat="server" Text="View" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td style="top: 81px; position: absolute; left: 244px; width: 90px;">
                        <asp:DropDownList ID="ddl_view" runat="server" AutoPostBack="true" Width="120px">
                            <asp:ListItem>Monitor Screen</asp:ListItem>
                            <%-- <asp:ListItem>Chart</asp:ListItem>
                            <asp:ListItem>Graph</asp:ListItem>
                            <asp:ListItem>Alter Report</asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
                    <td style="position: absolute; top: 81px; width: 59px; left: 397px;">
                        <asp:Label ID="lbl_events" runat="server" Text="Display" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td style="top: 81px; position: absolute; width: 122px; left: 458px;">
                        <asp:TextBox ID="txt_events" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                            Width="120px" Height="15px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                        <asp:Panel ID="pnl_events" runat="server" CssClass="multxtpanel" Style="visibility: visible;
                            position: absolute; left: 1px; top: 20px; z-index: 1000; height: 165px; width: 173px;">
                            <asp:CheckBox ID="chk_display" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                OnCheckedChanged="Chk_events_ChekedChange" Font-Size="Medium" Text="Select All"
                                AutoPostBack="True" />
                            <asp:CheckBoxList ID="chkls_display" runat="server" Font-Size="Medium" AutoPostBack="True"
                                OnSelectedIndexChanged="Chkls_events_SelectedIndexChanged" Width="156px" Height="22px"
                                Font-Bold="True" Font-Names="Book Antiqua">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_events"
                            PopupControlID="pnl_events" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td style="position: absolute; top: 81px; left: 592px; width: 94px;">
                        <asp:Label ID="lbl_view_type" runat="server" Text="Visible Type" Font-Size="Medium"
                            Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td style="top: 81px; position: absolute; left: 692px; width: 90px;">
                        <asp:DropDownList ID="ddl_view_type" runat="server" Width="90px" OnSelectedIndexChanged="ddl_view_type_SelectedIndexChanged"
                            OnTextChanged="ddl_view_type_TextChanged">
                            <asp:ListItem>Normal</asp:ListItem>
                            <asp:ListItem>3D Effect</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 100px; position: absolute; left: 800px; top: 80px;">
                     <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                        <ContentTemplate>
                        <asp:Button ID="btngo" runat="server" Text="GO" Height="22px" Width="100px" OnClick="btngo_Click1" />

                        </ContentTemplate>
                     </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <%--<div style="width:500px; position:absolute; height:30px; background-color:Green;">        --%>
        <asp:Panel ID="Panel_Hide" runat="server" Visible="false">
            <asp:UpdatePanel ID="Updatepanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pabsenties" runat="server" Style="position: absolute; left: 193px;
                        top: 113px; width: 800px; height: 30px;">
                        <asp:Label ID="lblfrom" Style="position: absolute; left: 2px; top: 6px;" runat="server"
                            Text="From" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                        <asp:TextBox ID="txtfrom" runat="server" Width="75px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="position: absolute; left: 52px; top: 5px; font-family: Book Antiqua;
                            font-size: medium; font-weight: bold; height: 15px;">
                        </asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtfrom" Format="dd/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtfrom"
                            FilterType="Custom,Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                        <asp:Label ID="lblto" Style="position: absolute; left: 152px; top: 6px;" runat="server"
                            Text="To" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                        <asp:TextBox ID="txtto" runat="server" Width="75px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="position: absolute; left: 182px; top: 5px; font-family: Book Antiqua;
                            font-size: medium; font-weight: bold; height: 15px;">
                        </asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtto" Format="dd/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtto"
                            FilterType="Custom,Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                        <asp:RadioButtonList ID="Rbtnlstinout" ForeColor="White" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                            Style="position: absolute; top: 2px; left: 275px;" OnSelectedIndexChanged="Rbtnlstinout_SelectedIndexChanged">
                            <asp:ListItem>Innnn&Out</asp:ListItem>
                            <asp:ListItem>InnnnnOnly</asp:ListItem>
                            <asp:ListItem>OutttttttOnly</asp:ListItem>
                            <asp:ListItem>UnRegisterStaff</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:Image ID="Image9" runat="server" Height="20px" ImageUrl="~/bioimage/In out.gif"
                            Width="81px" Style="position: absolute; top: 5px; left: 304px" />
                        <asp:Image ID="Image15" runat="server" ImageUrl="~/bioimage/In Only.jpg" Height="20px"
                            Style="position: absolute; top: 5px; left: 414px;" />
                        <asp:Image ID="Image16" runat="server" ImageUrl="~/bioimage/Out Only.jpg" Style="position: absolute;
                            margin-left: 8px; top: 5px; left: 526px;" Height="20px" Width="83px" />
                        <asp:Image ID="Image20" runat="server" ImageUrl="~/bioimage/Un Register Staff.jpg"
                            Height="20px" Width="86px" Style="position: absolute; top: 5px; left: 654px" />
                    </asp:Panel>
                    <asp:Panel ID="Ptime" runat="server" Style="position: absolute; left: 0px; top: 150px;
                        width: 800px; height: 30px;">
                        <asp:Label ID="lblintime" runat="server" Text="In Time" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua" Style="position: absolute; top: 6px; left: 2px"></asp:Label>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/bioimage/In Time.jpg" Height="20px"
                            Width="81px" Style="position: absolute; top: 6px; left: -11px" />
                        <asp:Label ID="lbltimefrm" runat="server" Text="From" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua" Style="position: absolute; top: 6px; left: 83px"></asp:Label>
                        <asp:DropDownList ID="ddl_fromhr" runat="server" Style="position: absolute; top: 6px;
                            left: 127px; height: 21px">
                            <asp:ListItem>HH</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_frommin" runat="server" Style="position: absolute; top: 6px;
                            left: 184px; height: 21px">
                            <asp:ListItem>MM</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                            <asp:ListItem>24</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>26</asp:ListItem>
                            <asp:ListItem>27</asp:ListItem>
                            <asp:ListItem>28</asp:ListItem>
                            <asp:ListItem>29</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>31</asp:ListItem>
                            <asp:ListItem>32</asp:ListItem>
                            <asp:ListItem>33</asp:ListItem>
                            <asp:ListItem>34</asp:ListItem>
                            <asp:ListItem>35</asp:ListItem>
                            <asp:ListItem>36</asp:ListItem>
                            <asp:ListItem>37</asp:ListItem>
                            <asp:ListItem>38</asp:ListItem>
                            <asp:ListItem>39</asp:ListItem>
                            <asp:ListItem>40</asp:ListItem>
                            <asp:ListItem>41</asp:ListItem>
                            <asp:ListItem>42</asp:ListItem>
                            <asp:ListItem>43</asp:ListItem>
                            <asp:ListItem>44</asp:ListItem>
                            <asp:ListItem>45</asp:ListItem>
                            <asp:ListItem>46</asp:ListItem>
                            <asp:ListItem>47</asp:ListItem>
                            <asp:ListItem>48</asp:ListItem>
                            <asp:ListItem>49</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>51</asp:ListItem>
                            <asp:ListItem>52</asp:ListItem>
                            <asp:ListItem>53</asp:ListItem>
                            <asp:ListItem>54</asp:ListItem>
                            <asp:ListItem>55</asp:ListItem>
                            <asp:ListItem>56</asp:ListItem>
                            <asp:ListItem>57</asp:ListItem>
                            <asp:ListItem>58</asp:ListItem>
                            <asp:ListItem>59</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_frommerdian" runat="server" Style="position: absolute;
                            top: 6px; left: 247px; height: 21px">
                            <asp:ListItem>AM</asp:ListItem>
                            <asp:ListItem>PM</asp:ListItem>
                        </asp:DropDownList>
                        <%--<asp:TextBox ID="txttimefrm" runat="server" Width="75" Style="position: absolute;
                top: 6px; left: 407px; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                height: 15px"></asp:TextBox>--%>
                        <%--<Ajaxified:TimePicker ID="tpfrom" runat="server" TargetControlID="txttimefrm">
            </Ajaxified:TimePicker>--%>
                        <asp:Label ID="lbltimeto" runat="server" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                            Text="To" Style="position: absolute; top: 7px; left: 316px;"></asp:Label>
                        <asp:DropDownList ID="ddl_tohr" runat="server" Style="position: absolute; top: 6px;
                            left: 340px; height: 21px">
                            <asp:ListItem>HH</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_tomin" runat="server" Style="position: absolute; top: 6px;
                            left: 397px; height: 21px">
                            <asp:ListItem>MM</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                            <asp:ListItem>24</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>26</asp:ListItem>
                            <asp:ListItem>27</asp:ListItem>
                            <asp:ListItem>28</asp:ListItem>
                            <asp:ListItem>29</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>31</asp:ListItem>
                            <asp:ListItem>32</asp:ListItem>
                            <asp:ListItem>33</asp:ListItem>
                            <asp:ListItem>34</asp:ListItem>
                            <asp:ListItem>35</asp:ListItem>
                            <asp:ListItem>36</asp:ListItem>
                            <asp:ListItem>37</asp:ListItem>
                            <asp:ListItem>38</asp:ListItem>
                            <asp:ListItem>39</asp:ListItem>
                            <asp:ListItem>40</asp:ListItem>
                            <asp:ListItem>41</asp:ListItem>
                            <asp:ListItem>42</asp:ListItem>
                            <asp:ListItem>43</asp:ListItem>
                            <asp:ListItem>44</asp:ListItem>
                            <asp:ListItem>45</asp:ListItem>
                            <asp:ListItem>46</asp:ListItem>
                            <asp:ListItem>47</asp:ListItem>
                            <asp:ListItem>48</asp:ListItem>
                            <asp:ListItem>49</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>51</asp:ListItem>
                            <asp:ListItem>52</asp:ListItem>
                            <asp:ListItem>53</asp:ListItem>
                            <asp:ListItem>54</asp:ListItem>
                            <asp:ListItem>55</asp:ListItem>
                            <asp:ListItem>56</asp:ListItem>
                            <asp:ListItem>57</asp:ListItem>
                            <asp:ListItem>58</asp:ListItem>
                            <asp:ListItem>59</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_tomeridian" runat="server" Style="position: absolute; top: 6px;
                            left: 460px; height: 21px">
                            <asp:ListItem>AM</asp:ListItem>
                            <asp:ListItem>PM</asp:ListItem>
                        </asp:DropDownList>
                        <%--<asp:TextBox ID="txttimeto" runat="server" Width="75" Style="position: absolute;
                top: 6px; left: 580px; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                height: 15px"></asp:TextBox>--%>
                        <%--<Ajaxified:TimePicker ID="tpto" runat="server" TargetControlID="txttimeto">
            </Ajaxified:TimePicker>--%>
                        <asp:Label ID="lblouttime" runat="server" Text="Out Time" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua" Style="position: absolute; top: 6px; left: 535px"></asp:Label>
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/bioimage/Out Time.jpg" Height="20px"
                            Width="81px" Style="position: absolute; top: 6px; left: 528px;" />
                        <asp:Label ID="lbloutfrom" runat="server" Text="From" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua" Style="position: absolute; top: 6px; left: 620px"></asp:Label>
                        <asp:DropDownList ID="ddl_fromouthr" runat="server" Style="position: absolute; top: 6px;
                            left: 665px; height: 21px">
                            <asp:ListItem>HH</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_fromoutmin" runat="server" Style="position: absolute; top: 6px;
                            left: 720px; height: 21px">
                            <asp:ListItem>MM</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                            <asp:ListItem>24</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>26</asp:ListItem>
                            <asp:ListItem>27</asp:ListItem>
                            <asp:ListItem>28</asp:ListItem>
                            <asp:ListItem>29</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>31</asp:ListItem>
                            <asp:ListItem>32</asp:ListItem>
                            <asp:ListItem>33</asp:ListItem>
                            <asp:ListItem>34</asp:ListItem>
                            <asp:ListItem>35</asp:ListItem>
                            <asp:ListItem>36</asp:ListItem>
                            <asp:ListItem>37</asp:ListItem>
                            <asp:ListItem>38</asp:ListItem>
                            <asp:ListItem>39</asp:ListItem>
                            <asp:ListItem>40</asp:ListItem>
                            <asp:ListItem>41</asp:ListItem>
                            <asp:ListItem>42</asp:ListItem>
                            <asp:ListItem>43</asp:ListItem>
                            <asp:ListItem>44</asp:ListItem>
                            <asp:ListItem>45</asp:ListItem>
                            <asp:ListItem>46</asp:ListItem>
                            <asp:ListItem>47</asp:ListItem>
                            <asp:ListItem>48</asp:ListItem>
                            <asp:ListItem>49</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>51</asp:ListItem>
                            <asp:ListItem>52</asp:ListItem>
                            <asp:ListItem>53</asp:ListItem>
                            <asp:ListItem>54</asp:ListItem>
                            <asp:ListItem>55</asp:ListItem>
                            <asp:ListItem>56</asp:ListItem>
                            <asp:ListItem>57</asp:ListItem>
                            <asp:ListItem>58</asp:ListItem>
                            <asp:ListItem>59</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_fromoutmeri" runat="server" Style="position: absolute;
                            top: 6px; left: 782px; height: 21px">
                            <asp:ListItem>AM</asp:ListItem>
                            <asp:ListItem>PM</asp:ListItem>
                        </asp:DropDownList>
                        <%--<asp:TextBox ID="txttimefrm" runat="server" Width="75" Style="position: absolute;
                top: 6px; left: 407px; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                height: 15px"></asp:TextBox>--%>
                        <%--<Ajaxified:TimePicker ID="tpfrom" runat="server" TargetControlID="txttimefrm">
            </Ajaxified:TimePicker>--%>
                        <asp:Label ID="lbltoout" runat="server" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                            Text="To" Style="position: absolute; top: 7px; left: 846px;"></asp:Label>
                        <asp:DropDownList ID="ddl_toouthr" runat="server" Style="position: absolute; top: 6px;
                            left: 870px; height: 21px">
                            <asp:ListItem>HH</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_tooutmin" runat="server" Style="position: absolute; top: 6px;
                            left: 926px; height: 21px">
                            <asp:ListItem>MM</asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                            <asp:ListItem>24</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>26</asp:ListItem>
                            <asp:ListItem>27</asp:ListItem>
                            <asp:ListItem>28</asp:ListItem>
                            <asp:ListItem>29</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>31</asp:ListItem>
                            <asp:ListItem>32</asp:ListItem>
                            <asp:ListItem>33</asp:ListItem>
                            <asp:ListItem>34</asp:ListItem>
                            <asp:ListItem>35</asp:ListItem>
                            <asp:ListItem>36</asp:ListItem>
                            <asp:ListItem>37</asp:ListItem>
                            <asp:ListItem>38</asp:ListItem>
                            <asp:ListItem>39</asp:ListItem>
                            <asp:ListItem>40</asp:ListItem>
                            <asp:ListItem>41</asp:ListItem>
                            <asp:ListItem>42</asp:ListItem>
                            <asp:ListItem>43</asp:ListItem>
                            <asp:ListItem>44</asp:ListItem>
                            <asp:ListItem>45</asp:ListItem>
                            <asp:ListItem>46</asp:ListItem>
                            <asp:ListItem>47</asp:ListItem>
                            <asp:ListItem>48</asp:ListItem>
                            <asp:ListItem>49</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>51</asp:ListItem>
                            <asp:ListItem>52</asp:ListItem>
                            <asp:ListItem>53</asp:ListItem>
                            <asp:ListItem>54</asp:ListItem>
                            <asp:ListItem>55</asp:ListItem>
                            <asp:ListItem>56</asp:ListItem>
                            <asp:ListItem>57</asp:ListItem>
                            <asp:ListItem>58</asp:ListItem>
                            <asp:ListItem>59</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_tooutmeri" runat="server" Style="position: absolute; top: 6px;
                            left: 988px; height: 21px">
                            <asp:ListItem>AM</asp:ListItem>
                            <asp:ListItem>PM</asp:ListItem>
                        </asp:DropDownList>
                    </asp:Panel>
                    <asp:Panel ID="Panel3" runat="server" Style="position: absolute; left: -5px; top: 183px;
                        width: 400px; height: 30px;">
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                            Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua">
                            <asp:ListItem>Morning</asp:ListItem>
                            <asp:ListItem>Evening</asp:ListItem>
                            <asp:ListItem>Both</asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="Updatepanel4" runat="server">
                <ContentTemplate>
                    <%--<div style="position:absolute; top:102px; left:687px; Width:100px; height:47px;">--%>
                    <asp:Panel ID="Pchklstabs" runat="server" Style="position: absolute; top: 112px;
                        left: 0px; width: 180px; height: 30px;">
                        <asp:CheckBox ID="chkabs" runat="server" Text="Absenties" AutoPostBack="true" Style="position: absolute;
                            left: 0px; top: 6px; font-family: Book Antiqua; font-size: medium; font-weight: bold;"
                            OnCheckedChanged="chkabs_CheckedChanged" />
                        <asp:CheckBox ID="chktimeing" runat="server" Text="Timings" AutoPostBack="true" Style="position: absolute;
                            left: 92px; top: 6px; font-family: Book Antiqua; font-size: medium; font-weight: bold;"
                            OnCheckedChanged="chktimeing_CheckedChanged" />
                        <%--<asp:CheckBoxList ID="chklstabs" runat="server" Font-Bold="true" 
                   onselectedindexchanged="chklstabs_SelectedIndexChanged1" AutoPostBack="true">
               <asp:ListItem>Absenties</asp:ListItem>
               <asp:ListItem>Timings</asp:ListItem>
               </asp:CheckBoxList>--%>
                        <%--<asp:CheckBox ID="CheckBox1" runat="server" Text="Absenties" Font-Bold="true" Font-Size="Large" />
               <asp:CheckBox ID="CheckBox2" runat="server" Text="Timings" Font-Bold="true" Font-Size="Large" />--%>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <%--</div>--%>
        <%--</div>--%>
        <div style="width: 960px; top: 130px; position: absolute;">
            <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="position: absolute;
                width: 1015px; height: 21px; margin-bottom: 0px; left: 10px;">
            </asp:Panel>
        </div>
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:Label ID="Error_Msg" runat="server" Text=""></asp:Label>
    <FarPoint:FpSpread ID="FpTransport" runat="server">
        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
            ButtonShadowColor="ControlDark" ButtonType="PushButton">
            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>
        </CommandBar>
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
            </FarPoint:SheetView>
        </Sheets>
        <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
            Font-Size="X-Large">
        </TitleInfo>
    </FarPoint:FpSpread>
    <asp:UpdatePanel ID="Updatepanel5" runat="server">
        <ContentTemplate>
            <FarPoint:FpSpread ID="Fp_Absenties" runat="server">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark" ButtonType="PushButton">
                    <%--<Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>--%>
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
                <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                    Font-Size="X-Large">
                </TitleInfo>
            </FarPoint:FpSpread>
        </ContentTemplate>
    </asp:UpdatePanel>
    <FarPoint:FpSpread ID="Fp_InOut" runat="server">
        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
            ButtonShadowColor="ControlDark" ButtonType="PushButton">
            <%--<Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>--%>
        </CommandBar>
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1">
            </FarPoint:SheetView>
        </Sheets>
        <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
            Font-Size="X-Large">
        </TitleInfo>
    </FarPoint:FpSpread>
    <%--<asp:Chart ID="Chart1" runat="server">
        
    </asp:Chart>

    <asp:Chart ID="Graph_Chart" runat="server" Width="1000px" Height="500px"></asp:Chart>--%>
    <asp:Chart ID="Chart1" runat="server">
    </asp:Chart>
    <asp:Chart ID="Graph_Chart" runat="server">
    </asp:Chart>

    </ContentTemplate>
     </asp:UpdatePanel>

      <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
