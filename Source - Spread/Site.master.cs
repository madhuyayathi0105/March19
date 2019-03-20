using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;

public partial class site : System.Web.UI.MasterPage
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataView dv = new DataView();
    string collegecode = string.Empty;
    Control findControlall;
    HtmlAnchor lnkcontrol = new HtmlAnchor();
    Panel pnlcontol = new Panel();
    ArrayList allheaderids = new ArrayList();
    string sql = string.Empty;
    string value = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //string strPreviousPage =string.Empty;
        //if (Request.UrlReferrer != null)
        //{
        //    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //}
        //if (strPreviousPage == "")
        //{
        //    Session["IsLogin"] = "0";
        //    Response.Redirect("~/Default.aspx");
        //}
        if (Session["collegecode"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        if (!IsPostBack)
        {
            //closemenu.Attributes.Add("style", "background-color: transparent; height: 100%; left: 0; display: block; position: fixed; top: 0; width: 100%;");
            string collegecode = Session["Collegecode"].ToString();

            string collegeName = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");

            if (da.GetFunction("select LinkValue from New_InsSettings where LinkName='UseCommonCollegeCode' and user_code ='" + Session["UserCode"].ToString() + "'") == "1")
            {
                string comCOde = da.GetFunction("select com_name from collinfo where  college_code='" + collegecode + "'").Trim();
                collegeName = (comCOde.Length > 1) ? comCOde : collegeName;
            }
            lblcolname.Text = collegeName;

            //lblcolname.Text = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");
            string color = da.GetFunction("select Farvour_color from user_color where user_code='" + Session["UserCode"].ToString() + "' and college_code='" + collegecode + "'");
            string colornew = string.Empty;
            if (color.Trim() == "0")
            {
                colornew = "#06d995";
            }
            else
            {
                colornew = color;
                //prewcolor.Attributes.Add("style", "background-color:" + colornew + ";");
            }
            MainDivIdValue.Attributes.Add("style", "background-color:" + colornew + ";border-bottom: 6px solid lightyellow; box-shadow: 0 0 11px -4px; height: 58px; left: 0; position: fixed; z-index: 2; top: 0; width: 100%;");
            if (Convert.ToString(Session["Staff_Code"]) != "")
            {
                img_stfphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                imgstdphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                string stfdescode = string.Empty;
                sql = "select desig_code from stafftrans where staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' and latestrec=1";
                stfdescode = da.GetFunction(sql);
                if (stfdescode != "" && stfdescode != null)
                {
                    string stfdesigname = string.Empty;
                    sql = "select dm.desig_name from desig_master dm where dm.desig_code='" + stfdescode.ToString() + "' and collegecode=" + Session["collegecode"].ToString();
                    stfdesigname = da.GetFunction(sql);
                    string staffname = string.Empty;
                    sql = "select staff_name from staffmaster where staff_code='" + Session["staff_code"] + "'";
                    staffname = da.GetFunction(sql);
                    string deptname = string.Empty;
                    sql = "select dt.dept_acronym from Department dt,stafftrans st where dt.Dept_code=st.dept_code and staff_code='" + Session["staff_code"] + "' and latestrec=1";
                    deptname = da.GetFunction(sql);
                    lbslstaffname.Text = Convert.ToString(staffname);
                    lbldesignation.Text = Convert.ToString(stfdesigname);
                    lbldept.Text = Convert.ToString(deptname);

                    //Notification Image Add
                    ImageButton2.ImageUrl = "~/dashbd/notif.png";
                    int notification = 0;
                    if (Session["Staff_Code"] != null)
                    {
                        notification = int.Parse(da.GetFunction("select count(*) from tbl_notification where viewrs='" + Convert.ToString(Session["Staff_Code"]) + "' and status=0"));
                    }
                    if (notification > 0)
                    {
                        CountSpan.InnerHtml = Convert.ToString(notification);
                        CountSpan.Visible = true;
                        ImageButton2.ImageUrl = "~/dashbd/notif.png";
                    }

                }
            }
            else
            {
                string staffname = string.Empty;
                sql = "select full_name from usermaster where user_code='" + Session["UserCode"] + "'";
                staffname = da.GetFunction(sql);
                lbslstaffname.Text = Convert.ToString(staffname);
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                value = da.GetFunction("select value from Master_Settings where settings='Report Display' and group_code=" + Session["group_code"] + "");
            }
            else
            {
                value = da.GetFunction("select value from Master_Settings where settings='Report Display' and usercode=" + Session["UserCode"] + "");
            }
            Session["value"] = "0";
            if (value == "0")
            {
                Session["value"] = "1";
            }
            string un = "", pw = string.Empty;
            if (Session["UserName"] != "" && Session["UserName"] != null)
                un = Session["UserName"].ToString();
            if (Session["password"] != "" && Session["password"] != null)
                pw = Session["password"].ToString();
            if (un == "Palpap Admin" && pw.Trim() != "")
            {
                allmenu.Visible = false;
                Div1.Visible = true;
                Div2.Visible = true;
            }
            else
            {
                ArrayList rights = new ArrayList();
                if (value.Trim() != "0")
                {
                    rights.Clear();
                    Boolean camlock = false;
                    string Selectquery = string.Empty;
                    if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                    {
                        //college_code=" + Session["collegecode"] + " and
                        Selectquery = "select rights_code from security_user_right where  group_code=" + Session["group_code"];
                        string checkvalue = da.GetFunction("select value  from Master_Settings where settings='CAM Calculation Lock' and Usercode ='" + Session["UserCode"] + "'");
                        if (checkvalue.Trim() != "1")
                        {
                            camlock = true;
                        }
                    }
                    else
                    {
                        Selectquery = "select rights_code from security_user_right where  user_code=" + Session["UserCode"];
                        //college_code=" + Session["collegecode"] + " and
                        camlock = true;
                    }
                    DataSet dnew = da.select_method_wo_parameter(Selectquery, "Text");
                    if (dnew.Tables.Count > 0 && dnew.Tables[0].Rows.Count > 0)
                    {
                        for (int row = 0; row < dnew.Tables[0].Rows.Count; row++)
                        {
                            rights.Add(dnew.Tables[0].Rows[row]["rights_code"].ToString());
                        }
                        // HR -s
                        if (rights.Contains("201600") || rights.Contains("201601") || rights.Contains("201602") || rights.Contains("201603") || rights.Contains("201604") || rights.Contains("201605") || rights.Contains("201606") || rights.Contains("201607") || rights.Contains("201608") || rights.Contains("201609") || rights.Contains("2016010") || rights.Contains("2016011") || rights.Contains("201701") || rights.Contains("201702") || rights.Contains("201703") || rights.Contains("201612") || rights.Contains("201613") || rights.Contains("201614") || rights.Contains("201615") || rights.Contains("201616") || rights.Contains("201617") || rights.Contains("201704") || rights.Contains("201705") || rights.Contains("201706") || rights.Contains("201707") || rights.Contains("201708") || rights.Contains("201709") || rights.Contains("201710") || rights.Contains("201711") || rights.Contains("201712") || rights.Contains("201713") || rights.Contains("201714") || rights.Contains("201715") || rights.Contains("201716") || rights.Contains("201717") || rights.Contains("201718") || rights.Contains("201719") || rights.Contains("201720") || rights.Contains("201721") || rights.Contains("201722") || rights.Contains("201723") || rights.Contains("201724") || rights.Contains("201725") || rights.Contains("201726") || rights.Contains("201727") || rights.Contains("201728") || rights.Contains("201729") || rights.Contains("201730") || rights.Contains("201731") || rights.Contains("201732") || rights.Contains("201733") || rights.Contains("201734") || rights.Contains("201735") || rights.Contains("201736") || rights.Contains("201737") || rights.Contains("201738") || rights.Contains("201739") || rights.Contains("201740") || rights.Contains("201741") || rights.Contains("201742") || rights.Contains("201743") || rights.Contains("201744") || rights.Contains("201745") || rights.Contains("201746") || rights.Contains("201747") || rights.Contains("201748"))
                        {
                            HRdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            HRdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }
                        //finance  -s                  
                        if (rights.Contains("9988776") || rights.Contains("9988777") || rights.Contains("102") || rights.Contains("103") || rights.Contains("104") || rights.Contains("105") || rights.Contains("106") || rights.Contains("107") || rights.Contains("108") || rights.Contains("109") || rights.Contains("110") || rights.Contains("9110") || rights.Contains("111") || rights.Contains("112") || rights.Contains("113") || rights.Contains("114") || rights.Contains("115") || rights.Contains("116") || rights.Contains("117") || rights.Contains("118") || rights.Contains("119") || rights.Contains("120") || rights.Contains("121") || rights.Contains("122") || rights.Contains("123") || rights.Contains("124") || rights.Contains("125") || rights.Contains("126") || rights.Contains("127") || rights.Contains("128") || rights.Contains("129") || rights.Contains("130") || rights.Contains("131") || rights.Contains("132") || rights.Contains("133") || rights.Contains("134") || rights.Contains("135") || rights.Contains("136") || rights.Contains("137") || rights.Contains("138") || rights.Contains("139") || rights.Contains("140") || rights.Contains("141") || rights.Contains("142") || rights.Contains("143") || rights.Contains("144") || rights.Contains("145") || rights.Contains("146") || rights.Contains("147") || rights.Contains("148") || rights.Contains("149") || rights.Contains("150") || rights.Contains("151") || rights.Contains("152") || rights.Contains("153") || rights.Contains("154") || rights.Contains("155") || rights.Contains("156") || rights.Contains("157") || rights.Contains("158") || rights.Contains("159") || rights.Contains("160") || rights.Contains("161") || rights.Contains("162") || rights.Contains("163") || rights.Contains("164") || rights.Contains("165") || rights.Contains("166") || rights.Contains("167") || rights.Contains("168") || rights.Contains("170") || rights.Contains("171") || rights.Contains("232017108") || rights.Contains("172") || rights.Contains("173") || rights.Contains("174") || rights.Contains("175") || rights.Contains("176") || rights.Contains("177") || rights.Contains("178") || rights.Contains("179") || rights.Contains("180") || rights.Contains("181") || rights.Contains("182") || rights.Contains("183") || rights.Contains("184") || rights.Contains("185") || rights.Contains("186") || rights.Contains("187") || rights.Contains("188") || rights.Contains("189") || rights.Contains("190") || rights.Contains("191") || rights.Contains("192") || rights.Contains("193") || rights.Contains("194") || rights.Contains("195") || rights.Contains("196") || rights.Contains("197") || rights.Contains("198") || rights.Contains("199") || rights.Contains("202") || rights.Contains("203") || rights.Contains("204") || rights.Contains("205")) //|| rights.Contains("169")
                        //|| rights.Contains("168")                  
                        {
                            Financediv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Financediv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }
                        //hostel -s
                        if (rights.Contains("1122334") || rights.Contains("1122335") || rights.Contains("110001") || rights.Contains("110002") || rights.Contains("110003") || rights.Contains("110004") || rights.Contains("110005") || rights.Contains("110006") || rights.Contains("110007") || rights.Contains("110008") || rights.Contains("110009") || rights.Contains("110010") || rights.Contains("110011") || rights.Contains("110012") || rights.Contains("110013") || rights.Contains("110014") || rights.Contains("110015") || rights.Contains("110016") || rights.Contains("110017") || rights.Contains("110018") || rights.Contains("110019") || rights.Contains("110020") || rights.Contains("110021") || rights.Contains("110022") || rights.Contains("110023") || rights.Contains("110024") || rights.Contains("110025") || rights.Contains("110026") || rights.Contains("110027") || rights.Contains("110028") || rights.Contains("110029") || rights.Contains("110030") || rights.Contains("110031") || rights.Contains("110032") || rights.Contains("110033") || rights.Contains("110034") || rights.Contains("110035") || rights.Contains("110036") || rights.Contains("110037") || rights.Contains("110038") || rights.Contains("110039") || rights.Contains("110040") || rights.Contains("110041") || rights.Contains("110042") || rights.Contains("110043") || rights.Contains("110044") || rights.Contains("110099") || rights.Contains("110045") || rights.Contains("110046") || rights.Contains("110047") || rights.Contains("110048") || rights.Contains("999026") || rights.Contains("999027") || rights.Contains("999028") || rights.Contains("999029") || rights.Contains("999030") || rights.Contains("999031") || rights.Contains("110049") || rights.Contains("110050"))
                        //|| rights.Contains("110049") || rights.Contains("110050") || rights.Contains("110051") || rights.Contains("110052") || rights.Contains("110053") || rights.Contains("110054") || rights.Contains("110055") 
                        //saranyadevi 13.3.2018
                        {
                            Hosteldiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Hosteldiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }
                        //inventory -s
                        if (rights.Contains("900") || rights.Contains("901") || rights.Contains("902") || rights.Contains("903") || rights.Contains("904") || rights.Contains("905") || rights.Contains("909") || rights.Contains("910") || rights.Contains("911") || rights.Contains("912") || rights.Contains("913") || rights.Contains("914") || rights.Contains("915") || rights.Contains("916") || rights.Contains("917") || rights.Contains("918") || rights.Contains("919") || rights.Contains("920") || rights.Contains("921") || rights.Contains("922") || rights.Contains("923") || rights.Contains("924") || rights.Contains("925") || rights.Contains("906") || rights.Contains("907") || rights.Contains("908"))

                        //-------------cmd by saranyadevi24.3.2018
                        //|| rights.Contains("906") || rights.Contains("907") || rights.Contains("908") 
                        {
                            Inventorydiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Inventorydiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }
                        //school 
                        //if (rights.Contains("1300") || rights.Contains("13001") || rights.Contains("13002") || rights.Contains("13003") || rights.Contains("13004") || rights.Contains("13005") || rights.Contains("13006") || rights.Contains("13007") || rights.Contains("13008") || rights.Contains("13009") || rights.Contains("13010") || rights.Contains("13011") || rights.Contains("13012") || rights.Contains("13013") || rights.Contains("13014") || rights.Contains("13015") || rights.Contains("13016") || rights.Contains("13017") || rights.Contains("13018") || rights.Contains("13019") || rights.Contains("13020") || rights.Contains("13021") || rights.Contains("13022") || rights.Contains("13023") || rights.Contains("13024"))
                        //{
                        //    Schooldiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;");
                        //}
                        //else
                        //{
                        //    Schooldiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        //}
                        //feedback                    
                        if (rights.Contains("5555999") || rights.Contains("2500") || rights.Contains("2501") || rights.Contains("2502") || rights.Contains("2503") || rights.Contains("2504") || rights.Contains("2506") || rights.Contains("2507") || rights.Contains("2508") || rights.Contains("2514") || rights.Contains("2511") || rights.Contains("2512"))
                        {
                            Feedbackdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Feedbackdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }
                        //request                    
                        if (rights.Contains("1500") || rights.Contains("1501") || rights.Contains("1502") || rights.Contains("1503") || rights.Contains("1504") || rights.Contains("1505") || rights.Contains("1506") || rights.Contains("1507") || rights.Contains("1508") || rights.Contains("1509") || rights.Contains("1510") || rights.Contains("1511") || rights.Contains("1512") || rights.Contains("1513"))
                        {
                            Requestdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Requestdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }
                        //office
                        if (rights.Contains("31250") || rights.Contains("31251") || rights.Contains("31252") || rights.Contains("31253") || rights.Contains("31254") || rights.Contains("31255") || rights.Contains("31256") || rights.Contains("725") || rights.Contains("726") || rights.Contains("72799") || rights.Contains("72800") || rights.Contains("72801") || rights.Contains("72802") || rights.Contains("72803") || rights.Contains("72804") || rights.Contains("72805") || rights.Contains("6001") || rights.Contains("72806") || rights.Contains("72807") || rights.Contains("72808"))
                        {
                            Officediv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Officediv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }
                        //student -s
                        if (rights.Contains("41520") || rights.Contains("41521") || rights.Contains("41522") || rights.Contains("41523") || rights.Contains("41524") || rights.Contains("41525") || rights.Contains("41526") || rights.Contains("41527") || rights.Contains("41528") || rights.Contains("41529") || rights.Contains("41530") || rights.Contains("41531") || rights.Contains("41532") || rights.Contains("41533") || rights.Contains("41534") || rights.Contains("41535") || rights.Contains("41536") || rights.Contains("41538") || rights.Contains("41539") || rights.Contains("41540") || rights.Contains("41541") || rights.Contains("41542") || rights.Contains("41543") || rights.Contains("41544") || rights.Contains("41545") || rights.Contains("41546") || rights.Contains("41547") || rights.Contains("41548") || rights.Contains("41549") || rights.Contains("41550") || rights.Contains("41551") || rights.Contains("41552") || rights.Contains("41553") || rights.Contains("41554") || rights.Contains("41555") || rights.Contains("41556") || rights.Contains("41557") || rights.Contains("41558") || rights.Contains("41559") || rights.Contains("41560") || rights.Contains("41561") || rights.Contains("41564") || rights.Contains("41565") || rights.Contains("41566") || rights.Contains("41567") || rights.Contains("41568") || rights.Contains("41571") || rights.Contains("41572") || rights.Contains("41573") || rights.Contains("41574") || rights.Contains("41575") || rights.Contains("41576") || rights.Contains("41577") || rights.Contains("41578") || rights.Contains("41579") || rights.Contains("41580") || rights.Contains("41581") || rights.Contains("41582") || rights.Contains("41583") || rights.Contains("41584") || rights.Contains("41585") || rights.Contains("41586") || rights.Contains("41587") || rights.Contains("41588") || rights.Contains("41589") || rights.Contains("41590") || rights.Contains("41591") || rights.Contains("41592") || rights.Contains("41593") || rights.Contains("41594") || rights.Contains("41595") || rights.Contains("41596") || rights.Contains("41597") || rights.Contains("41598") || rights.Contains("41599") || rights.Contains("41601"))
                        {
                            Studentdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Studentdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }

                        // added by sudhagar 24/10/2016
                        //attendance
                        if (rights.Contains("1001") || rights.Contains("100001") || rights.Contains("101") || rights.Contains("100002") || rights.Contains("100003") || rights.Contains("100004") || rights.Contains("100005") || rights.Contains("100006") || rights.Contains("100007") || rights.Contains("100008") || rights.Contains("100009") || rights.Contains("100010") || rights.Contains("100011") || rights.Contains("100012") || rights.Contains("1002") || rights.Contains("1003") || rights.Contains("1004") || rights.Contains("1005") || rights.Contains("1006") || rights.Contains("1007") || rights.Contains("1008") || rights.Contains("1009") || rights.Contains("1010") || rights.Contains("1011") || rights.Contains("1012") || rights.Contains("1013") || rights.Contains("1014") || rights.Contains("1015") || rights.Contains("1016") || rights.Contains("1017") || rights.Contains("1018") || rights.Contains("1019") || rights.Contains("1020") || rights.Contains("1021") || rights.Contains("1022") || rights.Contains("1023") || rights.Contains("1024") || rights.Contains("1025") || rights.Contains("1025") || rights.Contains("1026") || rights.Contains("1027") || rights.Contains("1028") || rights.Contains("1029") || rights.Contains("1030") || rights.Contains("1031") || rights.Contains("1032") || rights.Contains("1033") || rights.Contains("1034") || rights.Contains("1035") || rights.Contains("1025") || rights.Contains("713") || rights.Contains("717") || rights.Contains("714") || rights.Contains("719") || rights.Contains("715") || rights.Contains("722") || rights.Contains("723") || rights.Contains("724") || rights.Contains("721") || rights.Contains("728") || rights.Contains("727") || rights.Contains("720") || rights.Contains("729") || rights.Contains("1036") || rights.Contains("1037") || rights.Contains("1038") || rights.Contains("1039") || rights.Contains("1040") || rights.Contains("1041") || rights.Contains("1042") || rights.Contains("100013") || rights.Contains("100014") || rights.Contains("100015") || rights.Contains("100016") || rights.Contains("100017") || rights.Contains("100018") || rights.Contains("100019") || rights.Contains("100020") || rights.Contains("100021") || rights.Contains("100022") || rights.Contains("100023"))
                        {
                            AttednanceDiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            AttednanceDiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }

                        //cam
                        if (rights.Contains("200") || rights.Contains("2001") || rights.Contains("201") || rights.Contains("2006") || rights.Contains("2007") || rights.Contains("2008") || rights.Contains("2009") || rights.Contains("2010") || rights.Contains("2011") || rights.Contains("2012") || rights.Contains("2013") || rights.Contains("2014") || rights.Contains("2015") || rights.Contains("2016") || rights.Contains("2017") || rights.Contains("2018") || rights.Contains("2020") || rights.Contains("2021") || rights.Contains("2025") || rights.Contains("2026") || rights.Contains("2027") || rights.Contains("2030") || rights.Contains("2031") || rights.Contains("2032") || rights.Contains("2033") || rights.Contains("2034") || rights.Contains("2035") || rights.Contains("2039") || rights.Contains("2041") || rights.Contains("2043") || rights.Contains("2044") || rights.Contains("2045") || rights.Contains("2046") || rights.Contains("2047") || rights.Contains("716") || rights.Contains("2036") || rights.Contains("2037") || rights.Contains("1300") || rights.Contains("2048") || rights.Contains("2049") || rights.Contains("2050") || rights.Contains("2051") || rights.Contains("2052") || rights.Contains("2053") || rights.Contains("2054") || rights.Contains("2055") || rights.Contains("2056") || rights.Contains("2057") || rights.Contains("2058") || rights.Contains("2059") | rights.Contains("999008") || rights.Contains("999009"))//rights.Contains("2024") || || rights.Contains("2002")  || rights.Contains("2003") || rights.Contains("2019") || rights.Contains("2005")  || rights.Contains("2022") || rights.Contains("2038") || rights.Contains("2029")  || rights.Contains("2028") || rights.Contains("2023") || rights.Contains("13001") || rights.Contains("13002") || rights.Contains("13003") || rights.Contains("13004") || rights.Contains("13005") || rights.Contains("13006") || rights.Contains("13007") || rights.Contains("13008") || rights.Contains("13009") || rights.Contains("13010") || rights.Contains("13011") || rights.Contains("13012") || rights.Contains("13013") || rights.Contains("13014") || rights.Contains("13015") || rights.Contains("13016") || rights.Contains("13017") || rights.Contains("13018") || rights.Contains("13019") || rights.Contains("13020") || rights.Contains("13021") || rights.Contains("13022") || rights.Contains("13023") || rights.Contains("13024") || rights.Contains("13025") || rights.Contains("13026") || rights.Contains("13027") || rights.Contains("13028") || rights.Contains("13029")
                        {
                            Camdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Camdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }

                        // Schedule 
                        if (rights.Contains("300") || rights.Contains("3001") || rights.Contains("301") || rights.Contains("3012") || rights.Contains("3002") || rights.Contains("3003") || rights.Contains("3004") || rights.Contains("3005") || rights.Contains("3006") || rights.Contains("3007") || rights.Contains("3008") || rights.Contains("3009") || rights.Contains("3010") || rights.Contains("3011") || rights.Contains("3014"))
                        {
                            Schedulediv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Schedulediv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }

                        //general reports
                        if (rights.Contains("600") || rights.Contains("601") || rights.Contains("6002") || rights.Contains("6003") || rights.Contains("6004") || rights.Contains("6005") || rights.Contains("6006") || rights.Contains("6007") || rights.Contains("6008") || rights.Contains("6009") || rights.Contains("6010") || rights.Contains("6011") || rights.Contains("6012") || rights.Contains("6013") || rights.Contains("6014") || rights.Contains("6015") || rights.Contains("6016") || rights.Contains("6017") || rights.Contains("6018") || rights.Contains("6019") || rights.Contains("6020") || rights.Contains("6021") || rights.Contains("6022") || rights.Contains("6023") || rights.Contains("6026") || rights.Contains("6027") || rights.Contains("6028") || rights.Contains("6029") || rights.Contains("6030") || rights.Contains("6031") || rights.Contains("6032") || rights.Contains("6033") || rights.Contains("6034") || rights.Contains("6035") || rights.Contains("6036") || rights.Contains("6037") || rights.Contains("6038") || rights.Contains("6043") || rights.Contains("6044") || rights.Contains("602") || rights.Contains("6024") || rights.Contains("6025") || rights.Contains("6041") || rights.Contains("6042") || rights.Contains("603") || rights.Contains("604") || rights.Contains("605") || rights.Contains("606") || rights.Contains("607") || rights.Contains("608") || rights.Contains("609") || rights.Contains("610") || rights.Contains("613") || rights.Contains("614") || rights.Contains("615") || rights.Contains("616") || rights.Contains("617") || rights.Contains("6039") || rights.Contains("6040") || rights.Contains("6045") || rights.Contains("6046") || rights.Contains("6047") || rights.Contains("6048") || rights.Contains("6049") || rights.Contains("6050") || rights.Contains("6051") || rights.Contains("6052") || rights.Contains("6053") || rights.Contains("6054") || rights.Contains("6055") || rights.Contains("6056") || rights.Contains("6057") || rights.Contains("6058"))
                        {
                            Reportsdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Reportsdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }

                        // COE Reports 
                        if (rights.Contains("8001") || rights.Contains("80001") || rights.Contains("80002") || rights.Contains("80003") || rights.Contains("80004") || rights.Contains("80005") || rights.Contains("80006") || rights.Contains("80007") || rights.Contains("80008") || rights.Contains("80009") || rights.Contains("80010") || rights.Contains("80011") || rights.Contains("80012") || rights.Contains("80013") || rights.Contains("80014") || rights.Contains("80015") || rights.Contains("80016") || rights.Contains("80017") || rights.Contains("80018") || rights.Contains("80019") || rights.Contains("80020") || rights.Contains("80021") || rights.Contains("80022") || rights.Contains("80023") || rights.Contains("80024") || rights.Contains("80025") || rights.Contains("80026") || rights.Contains("80027") || rights.Contains("80028") || rights.Contains("80029") || rights.Contains("80030") || rights.Contains("80031") || rights.Contains("80032") || rights.Contains("80033") || rights.Contains("80034") || rights.Contains("80035") || rights.Contains("80036") || rights.Contains("80037") || rights.Contains("80038") || rights.Contains("80039") || rights.Contains("80040") || rights.Contains("80041") || rights.Contains("80042") || rights.Contains("80043") || rights.Contains("80044") || rights.Contains("80045") || rights.Contains("80046") || rights.Contains("80047") || rights.Contains("80048") || rights.Contains("80049") || rights.Contains("80050") || rights.Contains("80051") || rights.Contains("80052") || rights.Contains("80053") || rights.Contains("80054") || rights.Contains("80055") || rights.Contains("80056") || rights.Contains("80057") || rights.Contains("80058") || rights.Contains("80059") || rights.Contains("80060") || rights.Contains("80061") || rights.Contains("80062") || rights.Contains("80063") || rights.Contains("80064") || rights.Contains("80065") || rights.Contains("80066") || rights.Contains("80067") || rights.Contains("80068") || rights.Contains("80069") || rights.Contains("80070") || rights.Contains("80071") || rights.Contains("80072") || rights.Contains("80073") || rights.Contains("80074") || rights.Contains("80075") || rights.Contains("80076") || rights.Contains("80079") || rights.Contains("80080") || rights.Contains("80081") || rights.Contains("80082") || rights.Contains("80081") || rights.Contains("80082") || rights.Contains("80083") || rights.Contains("80084") || rights.Contains("80085") || rights.Contains("80086") || rights.Contains("80087") || rights.Contains("80088") || rights.Contains("80089") || rights.Contains("80090") || rights.Contains("80091") || rights.Contains("80092") || rights.Contains("80093") || rights.Contains("80094") || rights.Contains("80095") || rights.Contains("80096") || rights.Contains("80097") || rights.Contains("80098") || rights.Contains("80099") || rights.Contains("80100") || rights.Contains("80101") || rights.Contains("80102") || rights.Contains("80103") || rights.Contains("80104") || rights.Contains("80105") || rights.Contains("80106") || rights.Contains("80107") || rights.Contains("80108") || rights.Contains("80109") || rights.Contains("80110") || rights.Contains("80111") || rights.Contains("80112") || rights.Contains("80113") || rights.Contains("80114") || rights.Contains("80115") || rights.Contains("80116") || rights.Contains("80117") || rights.Contains("80118") || rights.Contains("80119") || rights.Contains("80120") || rights.Contains("80121") || rights.Contains("80122") || rights.Contains("80123") || rights.Contains("80124") || rights.Contains("80125") || rights.Contains("80126") || rights.Contains("80127") || rights.Contains("80128") || rights.Contains("80129") || rights.Contains("80130") || rights.Contains("80134") || rights.Contains("80136"))
                        {
                            COEdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            COEdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }

                        // BlockBox
                        if (rights.Contains("9001") || rights.Contains("140001") || rights.Contains("140002") || rights.Contains("140003") || rights.Contains("140004"))
                        {
                            BlackBoxdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            BlackBoxdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }

                        // SMS
                        if (rights.Contains("9002") || rights.Contains("90001") || rights.Contains("90002") || rights.Contains("90003") || rights.Contains("90004"))
                        {
                            SMSdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            SMSdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }

                        //transport                         
                        if (rights.Contains("703") || rights.Contains("704") || rights.Contains("800") || rights.Contains("705") || rights.Contains("706") || rights.Contains("707") || rights.Contains("708") || rights.Contains("709") || rights.Contains("710") || rights.Contains("7120") || rights.Contains("711") || rights.Contains("7121") || rights.Contains("7122") || rights.Contains("7123") || rights.Contains("7124") || rights.Contains("7125") || rights.Contains("7126") || rights.Contains("7127") || rights.Contains("7128") || rights.Contains("7129") || rights.Contains("7130") || rights.Contains("7131") || rights.Contains("7132") || rights.Contains("7133") || rights.Contains("7134") || rights.Contains("7135") || rights.Contains("7136") || rights.Contains("7137") || rights.Contains("7138"))
                        {
                            Transportdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Transportdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }

                        // Chart                       
                        if (rights.Contains("1200") || rights.Contains("1201") || rights.Contains("1202") || rights.Contains("1203") || rights.Contains("1204") || rights.Contains("1205") || rights.Contains("1206"))
                        {
                            Chartdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Chartdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }

                        // schedule                       
                        //if (rights.Contains("713") || rights.Contains("717") || rights.Contains("714") || rights.Contains("719") || rights.Contains("715") || rights.Contains("722") || rights.Contains("723") || rights.Contains("724") || rights.Contains("721") || rights.Contains("728") || rights.Contains("727") || rights.Contains("720"))
                        //{
                        //    Allotmentdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        //}
                        //else
                        //{
                        //    Allotmentdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        //}

                        //Question
                        if (rights.Contains("800") || rights.Contains("801") || rights.Contains("802") || rights.Contains("803") || rights.Contains("804") || rights.Contains("805") || rights.Contains("806") || rights.Contains("807") || rights.Contains("808") || rights.Contains("809") || rights.Contains("810") || rights.Contains("811") || rights.Contains("812"))
                        {
                            Questiondiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            Questiondiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        }

                        // Admission Settings
                        //if (rights.Contains("88900") || rights.Contains("88901") || rights.Contains("88902") || rights.Contains("88903") || rights.Contains("88904") || rights.Contains("88905") || rights.Contains("88906") || rights.Contains("88907") || rights.Contains("88908") || rights.Contains("88909") || rights.Contains("88910") || rights.Contains("88911") || rights.Contains("88912") || rights.Contains("88913") || rights.Contains("88914") || rights.Contains("88915") || rights.Contains("88916") || rights.Contains("88917") || rights.Contains("88918") || rights.Contains("88919") || rights.Contains("88920") || rights.Contains("88921") || rights.Contains("88922") || rights.Contains("88923") || rights.Contains("88924") || rights.Contains("88925") || rights.Contains("88926") || rights.Contains("88927") || rights.Contains("88928"))
                        //{
                        //    AdmissionDiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        //}
                        //else
                        //{
                        //    AdmissionDiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;");
                        //}

                        //i patch
                        if (rights.Contains("31256"))
                        {
                            ipatchdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            ipatchdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }


                        //sarayadevi 9feb2018 librarydiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        if (rights.Contains("1901"))//Poomalar
                        {
                            backupdiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            backupdiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        //library
                        //sarayadevi 9feb2018=========  
                        if (rights.Contains("999010") || rights.Contains("999011") || rights.Contains("999012") || rights.Contains("999013") || rights.Contains("999014") || rights.Contains("999015") || rights.Contains("999016") || rights.Contains("999017") || rights.Contains("999018") || rights.Contains("999019") || rights.Contains("999020") || rights.Contains("999021") || rights.Contains("999022") || rights.Contains("999023") || rights.Contains("999024") || rights.Contains("999025") || rights.Contains("999901") || rights.Contains("999035") || rights.Contains("999036") || rights.Contains("999037") || rights.Contains("999038") || rights.Contains("999039") || rights.Contains("999040") || rights.Contains("999041") || rights.Contains("999042") || rights.Contains("999043") || rights.Contains("999044") || rights.Contains("999045") || rights.Contains("999046") || rights.Contains("999047") || rights.Contains("999048") || rights.Contains("999049") || rights.Contains("999050") || rights.Contains("999051") || rights.Contains("999052") || rights.Contains("999053") || rights.Contains("999054") || rights.Contains("999055") || rights.Contains("999056") || rights.Contains("999057") || rights.Contains("999058") || rights.Contains("999059") || rights.Contains("999060") || rights.Contains("999061") || rights.Contains("999062") || rights.Contains("999063") || rights.Contains("999064") || rights.Contains("999065") || rights.Contains("999066") || rights.Contains("999067") || rights.Contains("999068") || rights.Contains("999069") || rights.Contains("999070") || rights.Contains("999071") || rights.Contains("999072") || rights.Contains("999902") || rights.Contains("999903"))
                        {
                            librarydiv.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            librarydiv.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        //============================
                        //========Irun Rights Added By Saranya on 8/10/2018======//
                        if (rights.Contains("910001") || rights.Contains("910002") || rights.Contains("910003") || rights.Contains("910004") || rights.Contains("910005") || rights.Contains("910006") || rights.Contains("910007") || rights.Contains("910008") || rights.Contains("910009") || rights.Contains("910010") || rights.Contains("910011") || rights.Contains("910012") || rights.Contains("910013") || rights.Contains("910014") || rights.Contains("910015") || rights.Contains("910016") || rights.Contains("910017") || rights.Contains("910018") || rights.Contains("910019") || rights.Contains("910020") || rights.Contains("910021") || rights.Contains("910022") || rights.Contains("910023") || rights.Contains("910024") || rights.Contains("910025") || rights.Contains("910025") || rights.Contains("910027") || rights.Contains("910028") || rights.Contains("910029") || rights.Contains("910030") || rights.Contains("910031") || rights.Contains("910032") || rights.Contains("910033") || rights.Contains("910034") || rights.Contains("910035") || rights.Contains("910036") || rights.Contains("910037") || rights.Contains("910038") || rights.Contains("910039") || rights.Contains("910040") || rights.Contains("910041") || rights.Contains("910042") || rights.Contains("910043") || rights.Contains("910044") || rights.Contains("910045") || rights.Contains("910046") || rights.Contains("910047") || rights.Contains("910048") || rights.Contains("910049") || rights.Contains("910050") || rights.Contains("910051") || rights.Contains("910052") || rights.Contains("910053") || rights.Contains("910054") || rights.Contains("910055") || rights.Contains("910056") || rights.Contains("910057") || rights.Contains("910058") || rights.Contains("910059") || rights.Contains("910060") || rights.Contains("910061") || rights.Contains("910062") || rights.Contains("910063") || rights.Contains("910064") || rights.Contains("910065") || rights.Contains("910066") || rights.Contains("910067") || rights.Contains("910068") || rights.Contains("910069") || rights.Contains("910070") || rights.Contains("910071") || rights.Contains("910072") || rights.Contains("910073") || rights.Contains("910074") || rights.Contains("910075") || rights.Contains("910076") || rights.Contains("910077") || rights.Contains("910078") || rights.Contains("910079") || rights.Contains("910080") || rights.Contains("910081") || rights.Contains("910082") || rights.Contains("910083") || rights.Contains("910084") || rights.Contains("910085") || rights.Contains("910086") || rights.Contains("910087") || rights.Contains("910088") || rights.Contains("910089") || rights.Contains("910090") || rights.Contains("910091") || rights.Contains("910092") || rights.Contains("910093") || rights.Contains("910094") || rights.Contains("910095") || rights.Contains("910096") || rights.Contains("910097") || rights.Contains("910098"))
                        {
                        }
                        //============================//
                        //----------------- MasterWizard-------------------//
                        //--------Added By Delsi------------
                        if (rights.Contains("999074") || rights.Contains("999075") || rights.Contains("999076") || rights.Contains("999077") || rights.Contains("999078") || rights.Contains("999079") || rights.Contains("999080") || rights.Contains("999081") || rights.Contains("999082"))
                        {
                            divMasterWizard.Attributes.Add("style", "display:block;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }
                        else
                        {
                            divMasterWizard.Attributes.Add("style", "display:none;float: left; height: 150px; width: 150px;float: left; height: 150px; width: 150px;");
                        }

                    }
                    string ses = Convert.ToString(Session["UserCode"]);
                    HideSecurity();
                    Div1.Visible = false;
                    Div2.Visible = false;
                }
            }
        }
    }

    public void HideSecurity()
    {
        allmenu.Visible = true;
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    protected void att_Click(object sender, EventArgs e)
    {
        // Session["MID"] = "M100";
        // Response.Redirect("Default_login.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Attendance";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$attendance");
    }

    protected void cam_Click(object sender, EventArgs e)
    {
        // Session["MID"] = "M200";
        // Response.Redirect("Default_login.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "CAM";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$cam");
    }

    protected void Schedule_Click(object sender, EventArgs e)
    {
        //  Session["MID"] = "M300";
        //Response.Redirect("Default_login.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Schedule";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$schedule");
    }

    protected void Admission_Click(object sender, EventArgs e)
    {
        // Session["MID"] = "M10000";
        //  Response.Redirect("StudentHome.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Admission";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$Admission");
    }

    protected void finance_Click(object sender, EventArgs e) //ss
    {
        // Session["MID"] = "M400";
        //Response.Redirect("FinanceIndex.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Finance";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$Finance");
    }

    protected void hr_Click(object sender, EventArgs e)  //ss
    {
        //  Session["MID"] = "M500";
        //  Response.Redirect("HRMenuIndex.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "HR";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$hr");
    }

    protected void Reports_Click(object sender, EventArgs e)
    {
        // Session["MID"] = "M600";
        Response.Redirect("Default_login.aspx");
        string redirectValue = redirctMethod();
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$reports");
    }

    protected void coe_Click(object sender, EventArgs e)
    {
        //  Session["MID"] = "M1800";
        //Response.Redirect("Default_login.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "COE";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$coe");
    }

    protected void Student_Click(object sender, EventArgs e)
    {
        // Session["MID"] = "M10000";
        //  Response.Redirect("StudentHome.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Student";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$student");
    }

    protected void Blackbox_Click(object sender, EventArgs e)
    {
        // Session["MID"] = "M9001";
        //Response.Redirect("Default_login.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Black Box";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$blackbox");
    }

    protected void SMS_Click(object sender, EventArgs e)
    {
        // Session["MID"] = "M9002";
        // Response.Redirect("Default_login.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "SMS";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$sms");
    }

    protected void Transport_Click(object sender, EventArgs e)
    {
        //Session["MID"] = "M703";
        //  Response.Redirect("TransportIndex.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Transport";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$transport");
    }

    protected void Allotment_Click(object sender, EventArgs e)
    {
        // Session["MID"] = "M712";
        // Response.Redirect("Default_login.aspx");
        string redirectValue = redirctMethod();
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$allotment");
    }

    protected void library_Click(object sender, EventArgs e)//added by saranyadevi 11.1.2018
    {
        string redirectValue = redirctMethod();
        Session["Module"] = "Library";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$library");
    }

    protected void ipatch_Click(object sender, EventArgs e)
    {
        string redirectValue = redirctMethod();
        Session["Module"] = "I Patch";
        Response.Redirect("~/IpatchMod/I_patch_master.aspx");
    }

    protected void backup_Click(object sender, EventArgs e) // poomalar 10.11.17
    {
        string redirectValue = redirctMethod();
        Session["Module"] = "Backup";
        Response.Redirect("~/Backup/DatabaseBackup.aspx");
    }

    protected void Question_Click(object sender, EventArgs e)
    {
        // Session["MID"] = "M800";
        // Response.Redirect("Default_login.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Question";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$question");
    }

    protected void Inventory_Click(object sender, EventArgs e)
    {
        // Session["MID"] = "M900";
        //Response.Redirect("inventoryindex.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Inventory";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$inventory");
    }

    protected void Hostel_Click(object sender, EventArgs e)
    {
        //Session["MID"] = "M1100";
        // Response.Redirect("hostelindex.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Hostel";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$hostel");
    }

    protected void chart_Click(object sender, EventArgs e)
    {
        //  Session["MID"] = "M1200";
        //Response.Redirect("Default_login.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Chart";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$chart");
    }

    protected void school_Click(object sender, EventArgs e)
    {
        // Session["MID"] = "M1300";
        //  Response.Redirect("Default_login.aspx");
        string redirectValue = redirctMethod();
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$school");
    }

    protected void Security_Click(object sender, EventArgs e)
    {
        //Session["MID"] = "palpap";
        Response.Redirect("SecuritySettings.aspx");
    }
    protected void Security2_Click(object sender, EventArgs e)
    {
        //Session["MID"] = "palpap";
        Response.Redirect("NewSecuritySettings.aspx");
    }

    protected void Requestdiv_Click(object sender, EventArgs e)
    {
        //Session["MID"] = "palpap";
        //Response.Redirect("RequestHome.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Request";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$request");
    }

    protected void Officediv_Click(object sender, EventArgs e)
    {
        //Session["MID"] = "palpap";
        //Response.Redirect("OfficeHome.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Office";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$office");
    }

    protected void Feedbackdiv_Click(object sender, EventArgs e)
    {
        //Session["MID"] = "palpap";
        // Response.Redirect("Feedbackhome.aspx");
        string redirectValue = redirctMethod();
        Session["Module"] = "Feed Back";
        Response.Redirect("CommonIndex.aspx?Name=" + redirectValue + "$feedback");
    }

    protected string redirctMethod()
    {
        string redirectValue = string.Empty;
        try
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["Single_User"])))
                redirectValue = Convert.ToString(Session["Single_User"]);
            else
                redirectValue = "SU";
            if (!string.IsNullOrEmpty(Convert.ToString(Session["group_code"])))
                redirectValue += "$" + Convert.ToString(Session["group_code"]);
            else
                redirectValue += "$" + "GC";
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserCode"])))
                redirectValue += "$" + Convert.ToString(Session["UserCode"]);
            else
                redirectValue += "$" + "UC";
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
                redirectValue += "$" + Convert.ToString(Session["UserName"]);
            else
                redirectValue += "$" + "UN";
            if (!string.IsNullOrEmpty(Convert.ToString(Session["Staff_Code"])))
                redirectValue += "$" + Convert.ToString(Session["Staff_Code"]);
            else
                redirectValue += "$" + "SC";
            if (!string.IsNullOrEmpty(Convert.ToString(Session["IsLogin"])))
                redirectValue += "$" + Convert.ToString(Session["IsLogin"]);
            else
                redirectValue += "$" + "IL";
            if (!string.IsNullOrEmpty(Convert.ToString(Session["current_college_code"])))
                redirectValue += "$" + Convert.ToString(Session["current_college_code"]);
            else
                redirectValue += "$" + "CC";
            if (!string.IsNullOrEmpty(Convert.ToString(Session["InternalCollegeCode"])))
                redirectValue += "$" + Convert.ToString(Session["InternalCollegeCode"]);
            else
                redirectValue += "$" + "IC";
        }
        catch { }
        return redirectValue;
    }

    protected void btn_color_click(object sender, EventArgs e)
    {
        try
        {
            if (txt_colorpicker.Text != "")
            {
                string q1 = " if exists (select Farvour_color from user_color where user_code='" + Session["UserCode"] + "' and college_code='" + Session["collegecode"].ToString() + "') update user_color set Farvour_color='" + txt_colorpicker.Text.ToUpper() + "' where college_code='" + Session["collegecode"].ToString() + "' and user_code='" + Session["UserCode"] + "' else insert into user_color (user_code,college_code,Farvour_color)values('" + Session["UserCode"].ToString() + "','" + Session["collegecode"].ToString() + "','" + txt_colorpicker.Text.ToUpper() + "')";
                da.update_method_wo_parameter(q1, "TEXT");
                Response.Redirect("Default_LoginPage.aspx");
            }
        }
        catch { }
    }
    protected void MasterWizard_Click(object sender, EventArgs e)
    {
        string redirectValue = redirctMethod();
        Session["Module"] = "MasterWizard";
        Response.Redirect("~/MasterWizardMod/MasterWizardMenuIndex.aspx");
        //Response.Redirect("MasterWizardMenuIndex.aspx");
    
    }

    //redirect to other index pages added by sudhagar 25/10/2016s
    //colorpicker added barath 20.12.16 

}
